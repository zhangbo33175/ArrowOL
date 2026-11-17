/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PsdImportModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   PSD/PSB 导入模块 - 解析 PSD 图层、三级目录切片、生成地图物件
 *            目录: 全局 Shared / 章节 Shared / 关卡 Level（对齐 MapLayerCatalog 八层）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Honor.Editor.MapTool;
using Debug = UnityEngine.Debug;

namespace Honor.Editor.MapTool.PsdImport
{
    /// <summary>
    /// PSD/PSB 导入模块：解析 PSD 文件、按章节/关卡切片导出、生成地图物件
    /// </summary>
    public class PsdImportModule
    {
        #region 回调注入

        /// <summary>地图尺寸变更回调（宽/高，像素）</summary>
        public Action<int, int> OnMapSizeSet;

        /// <summary>背景图路径回调（Assets 相对路径）</summary>
        public Action<string> OnBackgroundSet;

        /// <summary>物件创建回调</summary>
        public Action<PlacedObject> OnObjectCreated;

        /// <summary>场景刷新回调</summary>
        public Action OnRefreshScene;

        #endregion

        private readonly SceneEditModule m_Owner;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner">所属场景编辑模块</param>
        public PsdImportModule(SceneEditModule owner)
        {
            m_Owner = owner;
        }

        #region 导入入口

        public void ImportPsd()
        {
            // ===== 前置校验：章节/关卡 =====
            string chapterId, levelId;
            if (!TryResolveImportTarget(out chapterId, out levelId)) return;

            // ===== 选择 PSD 文件 =====
            string psdPath = EditorUtility.OpenFilePanel("选择 PSD/PSB 地图文件", "", "");
            if (string.IsNullOrEmpty(psdPath)) return;

            // ===== 解析 PSD 并读取结果 =====
            PsdImportData importData;
            string pythonOutput;
            if (!TryParsePsd(psdPath, chapterId, levelId, out importData, out pythonOutput)) return;

            // 应用到编辑器
            ApplyImportData(importData);

            // 自动优化新导出图片的纹理格式（压缩+关闭 mipmap，节省内存）
            int optimizedCount = OptimizeNewlyExportedTextures(importData);
            MapToolLog.Info($"[PSD导入] 已优化 {optimizedCount} 张图片的导入设置");

            // 生成 MD5 清单（记录所有导出图片 MD5，下次导入对比变化）
            GenerateMD5Manifest(importData, GetChapterDir(chapterId));

            ShowImportSummary(chapterId, levelId, importData, pythonOutput, optimizedCount);
        }

        /// <summary>
        /// 校验并取回导入目标：章节缺失时提示并中止，关卡缺失时回退 "1"
        /// </summary>
        /// <param name="chapterId">输出：章节 ID</param>
        /// <param name="levelId">输出：关卡 ID</param>
        /// <returns>章节有效返回 true</returns>
        private bool TryResolveImportTarget(out string chapterId, out string levelId)
        {
            chapterId = m_Owner?.CurrentChapterId;
            levelId = m_Owner?.CurrentLevelId;

            if (string.IsNullOrEmpty(chapterId))
            {
                MapToolDialog.Info("无法导入", "请先在左侧面板选择关卡，再导入PSD文件。\n\n资源会按 章节/关卡 分类存放，跨关卡复用资源自动归入章节公共目录。");
                return false;
            }

            if (string.IsNullOrEmpty(levelId)) levelId = "1";
            return true;
        }

        /// <summary>
        /// 调用 Python 解析 PSD 并读取解析结果 JSON
        /// </summary>
        /// <param name="psdPath">PSD/PSB 文件路径</param>
        /// <param name="chapterId">章节 ID</param>
        /// <param name="levelId">关卡 ID</param>
        /// <param name="importData">输出：解析数据</param>
        /// <param name="pythonOutput">输出：Python 标准输出</param>
        /// <returns>解析结果可用返回 true</returns>
        private bool TryParsePsd(string psdPath, string chapterId, string levelId,
            out PsdImportData importData, out string pythonOutput)
        {
            importData = null;
            pythonOutput = string.Empty;

            // ===== 三级资源目录规划（磁盘绝对路径） =====
            string mapRootDir = GetMapRootDir();
            string sharedDir = GetSharedDir();
            string chapterDir = GetChapterDir(chapterId);

            Directory.CreateDirectory(sharedDir);
            Directory.CreateDirectory(chapterDir);

            MapToolLog.Info($"[PSD导入] 章节: {chapterId}, 关卡: {levelId}");
            MapToolLog.Info($"[PSD导入] 全局共享: {sharedDir}");
            MapToolLog.Info($"[PSD导入] 章节目录: {chapterDir}");

            // ===== 调用 Python 脚本解析 PSD =====
            string scriptPath = MapToolPathHelper.ToAbsolutePath(MapToolConstants.PsdImportScriptPath);
            string pythonExe = FindPython();
            if (pythonExe == null)
            {
                MapToolDialog.Info("导入失败", "找不到 Python，请确认已安装 Python、psd-tools 并添加到 PATH");
                return false;
            }

            string error;
            RunPythonScript(pythonExe, scriptPath, psdPath, chapterDir, sharedDir, mapRootDir, levelId, out pythonOutput, out error);

            // ===== 读取解析结果 JSON =====
            string jsonPath = Path.Combine(mapRootDir, MapToolConstants.PsdImportResultFileName);
            if (!File.Exists(jsonPath))
            {
                MapToolDialog.Info("导入失败", $"未找到解析结果 JSON 文件\n\n章节目录: {chapterDir}\n共享目录: {sharedDir}\n\nPython 输出:\n{pythonOutput}\n\n错误:\n{error}");
                return false;
            }

            importData = JsonUtility.FromJson<PsdImportData>(File.ReadAllText(jsonPath));
            if (importData.objects == null) importData.objects = new List<PsdObjectInfo>();
            return true;
        }

        /// <summary>
        /// 地图资源根目录（磁盘绝对路径）
        /// </summary>
        /// <returns>根目录路径</returns>
        private static string GetMapRootDir()
        {
            return MapToolPathHelper.ToAbsolutePath(MapToolConstants.PsdMapTextureRootDir);
        }

        /// <summary>
        /// 跨章节公共资源目录
        /// </summary>
        /// <returns>目录路径</returns>
        private static string GetSharedDir()
        {
            return Path.Combine(GetMapRootDir(), MapToolConstants.PsdSharedDirName);
        }

        /// <summary>
        /// 章节资源目录
        /// </summary>
        /// <param name="chapterId">章节 ID</param>
        /// <returns>目录路径</returns>
        private static string GetChapterDir(string chapterId)
        {
            return Path.Combine(GetMapRootDir(), MapToolConstants.ChapterDirPrefix + chapterId);
        }

        /// <summary>
        /// 展示导入结果统计（新导出 / 复用公共 / 同 PSD 重复）
        /// </summary>
        /// <param name="chapterId">章节 ID</param>
        /// <param name="levelId">关卡 ID</param>
        /// <param name="importData">解析数据</param>
        /// <param name="pythonOutput">Python 输出（用于统计计数）</param>
        /// <param name="optimizedCount">已优化纹理数量</param>
        private void ShowImportSummary(string chapterId, string levelId, PsdImportData importData,
            string pythonOutput, int optimizedCount)
        {
            int newExported = CountOccurrences(pythonOutput, "Exported:");
            int skippedShared = CountOccurrences(pythonOutput, "Skip global:")
                                 + CountOccurrences(pythonOutput, "Skip chapter:")
                                 + CountOccurrences(pythonOutput, "Skip level:");
            int skippedDup = CountOccurrences(pythonOutput, "Skip duplicate:");

            string msg = $"当前章节: Chapter{chapterId} / 关卡 {levelId}\n" +
                         $"地图尺寸: {importData.mapWidth}x{importData.mapHeight}\n" +
                         $"背景图: {importData.backgroundName}\n" +
                         $"物件总数: {importData.objects.Count}\n" +
                         $"  ├─ 新导出到关卡: {newExported} 个\n" +
                         $"  ├─ 复用公共资源: {skippedShared} 个\n" +
                         $"  └─ 同PSD内重复: {skippedDup} 个\n" +
                         $"  ────────────────────\n" +
                         $"  已自动优化纹理格式: {optimizedCount} 张\n\n" +
                         $"物件位置/尺寸已按 PSD 图层还原";

            MapToolDialog.Info("PSD 导入完成", msg);
        }

        #endregion

        #region 结果应用

        /// <summary>
        /// 将 PSD 解析结果应用到编辑器：背景、地图尺寸、摆放物件
        /// </summary>
        /// <param name="data">PSD 解析结果</param>
        private void ApplyImportData(PsdImportData data)
        {
            if (data == null) return;
            if (data.objects == null) data.objects = new List<PsdObjectInfo>();

            // 重新导入：先清空当前关卡旧摆放物 / 背景，按新 PSD 覆盖重建（避免重复导入追加残留）
            m_Owner?.ClearWorkingObjects();

            // 1-2. 地图尺寸 + 背景图
            ApplyMapMeta(data);

            // 3. 刷新 AssetDatabase，确保 Unity 扫描到刚导出的 PNG
            AssetDatabase.Refresh();

            // 4. 创建物件（已在配置表中的沿用模板类型与参数）
            var newObjects = new List<string>();
            foreach (var obj in data.objects)
            {
                PlacedObject placedObj = CreatePlacedObjectFromPsd(obj, out bool isNewTemplate);
                if (placedObj == null) continue;

                if (isNewTemplate) newObjects.Add($"{placedObj.Name} ({placedObj.LayerType})");
                OnObjectCreated?.Invoke(placedObj);
            }

            // 5-6. 刷新场景 + 自动落盘编辑器工作信息（仅写工作 JSON，不触发发布 Lua 导出）
            //      下次选中关卡由 LoadMapConfig 直接还原，无需重新导入 PSD
            OnRefreshScene?.Invoke();
            m_Owner?.SaveWorkingConfig();

            // 7. 提示新物件
            ShowImportResult(data, newObjects);
        }

        /// <summary>
        /// 应用地图尺寸与背景图（尺寸回调 + 背景纹理校验 + 背景回调）
        /// </summary>
        /// <param name="data">PSD 解析结果</param>
        private void ApplyMapMeta(PsdImportData data)
        {
            OnMapSizeSet?.Invoke(data.mapWidth, data.mapHeight);
            MapToolLog.Info($"[PSD导入] 地图尺寸: {data.mapWidth} x {data.mapHeight}");
            MapToolLog.Info($"[PSD导入] 背景图路径: {data.backgroundPath}");
            MapToolLog.Info($"[PSD导入] 物件数量: {data.objects.Count}");

            if (string.IsNullOrEmpty(data.backgroundPath)) return;

            string assetBgPath = MapToolPathHelper.ToAssetRelative(data.backgroundPath);
            Texture2D bgTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetBgPath);
            if (bgTexture != null)
                MapToolLog.Info($"[PSD导入] 背景纹理实际尺寸: {bgTexture.width} x {bgTexture.height}");
            else
                MapToolLog.Warn($"[PSD导入] 背景纹理加载失败: {assetBgPath}");

            OnBackgroundSet?.Invoke(assetBgPath);
        }

        /// <summary>
        /// 由 PSD 单个图层信息生成摆放物（类型优先沿用配置表中的同名模板）
        /// </summary>
        /// <param name="obj">PSD 图层信息</param>
        /// <param name="isNewTemplate">输出：配置表中无同名模板（需要在元件面板补录）</param>
        /// <returns>摆放物实例，失败返回 null</returns>
        private static PlacedObject CreatePlacedObjectFromPsd(PsdObjectInfo obj, out bool isNewTemplate)
        {
            isNewTemplate = false;

            string layerType = MapLayerCatalog.Normalize(obj.layerType);
            MapObjectType objType = MapLayerCatalog.ResolveObjectType(layerType);

            // 配置表已有同名模板 → 沿用模板类型；否则标记为新物件
            var template = ObjectTemplateManager.Instance.GetTemplateByName(obj.name);
            if (template != null)
            {
                objType = template.Type;
            }
            else
            {
                isNewTemplate = true;
            }

            Texture2D previewTex = LoadPsdPreview(obj);

            // 像素尺寸：优先新字段，旧 JSON 回退 width/height
            float pixelWidth = obj.pixelWidth > 0 ? obj.pixelWidth : obj.width;
            float pixelHeight = obj.pixelHeight > 0 ? obj.pixelHeight : obj.height;

            // 格子尺寸（兼容旧网格占用计算；像素尺寸以 PixelWidth/PixelHeight 为准）
            int widthCells = Mathf.Max(1, Mathf.RoundToInt(pixelWidth / MapToolConstants.GridSize));
            int heightCells = Mathf.Max(1, Mathf.RoundToInt(pixelHeight / MapToolConstants.GridSize));

            var placedObj = new PlacedObject
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                TemplateId = obj.layerName,
                Name = obj.name,
                Type = objType,
                LayerType = layerType,
                Position = new Vector2(obj.x, obj.y),
                Width = widthCells,
                Height = heightCells,
                PixelWidth = pixelWidth,
                PixelHeight = pixelHeight,
                Stretch = obj.stretch,
                SortOrder = MapLayerCatalog.GetSortOrder(layerType),
                PreviewTexture = previewTex,
            };

            // ---TODO [存量归一] PSD 配置可能带旧 100 步长全局键/旧三档裸值；换算到新 10 步长八层基线坐标系
            if (obj.sortOrder > 0)
            {
                placedObj.SortOrder = MapToolEditorUtils.NormalizeLegacySortOrder(placedObj, obj.sortOrder);
            }

            return placedObj;
        }

        /// <summary>
        /// 加载 PSD 图层的预览纹理（路径非法或缺失时返回 null）
        /// </summary>
        /// <param name="obj">PSD 图层信息</param>
        /// <returns>预览纹理</returns>
        private static Texture2D LoadPsdPreview(PsdObjectInfo obj)
        {
            string assetPngPath = MapToolPathHelper.ToAssetRelative(obj.pngPath);
            if (string.IsNullOrEmpty(assetPngPath)) return null;

            Texture2D previewTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPngPath);
            if (previewTex == null)
                MapToolLog.Warn($"[PSD导入] 预览图加载失败: {assetPngPath}");
            return previewTex;
        }

        /// <summary>
        /// 展示导入结果对话框（含未配置新物件清单，最多列 10 条）
        /// </summary>
        /// <param name="data">PSD 解析结果</param>
        /// <param name="newObjects">未在配置表中的新物件</param>
        private static void ShowImportResult(PsdImportData data, List<string> newObjects)
        {
            if (newObjects.Count == 0) return;

            var msg = new System.Text.StringBuilder();
            msg.AppendLine("导入完成！");
            msg.AppendLine($"地图尺寸: {data.mapWidth}x{data.mapHeight}");
            msg.AppendLine($"物件总数: {data.objects.Count}");
            msg.AppendLine();
            msg.AppendLine($"⚠️ 有 {newObjects.Count} 个新物件不在配置表里：");
            foreach (var n in newObjects.Take(10))
                msg.AppendLine($"  • {n}");
            if (newObjects.Count > 10)
                msg.AppendLine($"  ... 等共 {newObjects.Count} 个");
            msg.AppendLine();
            msg.AppendLine("请在「物品元件编辑」Tab 中添加到配置表。");

            MapToolDialog.Info("PSD 导入完成", msg.ToString());
        }

        #endregion

        #region 纹理优化

        /// <summary>
        /// 自动优化新导出图片的纹理导入设置
        /// 统一：Sprite 类型 + RGBA32（不压缩）+ 关闭 mipmap + Read/Write
        /// </summary>
        /// <param name="importData">PSD 导入数据</param>
        /// <returns>优化的图片数量</returns>
        private int OptimizeNewlyExportedTextures(PsdImportData importData)
        {
            int optimizedCount = 0;
            var processedPaths = new HashSet<string>();

            if (!string.IsNullOrEmpty(importData.backgroundPath))
            {
                string assetPath = MapToolPathHelper.ToAssetRelative(importData.backgroundPath);
                if (!string.IsNullOrEmpty(assetPath) && processedPaths.Add(assetPath) && OptimizeSpriteTexture(assetPath))
                    optimizedCount++;
            }

            foreach (var obj in importData.objects)
            {
                if (string.IsNullOrEmpty(obj.pngPath)) continue;
                string assetPath = MapToolPathHelper.ToAssetRelative(obj.pngPath);
                if (string.IsNullOrEmpty(assetPath) || !processedPaths.Add(assetPath)) continue;
                if (OptimizeSpriteTexture(assetPath))
                    optimizedCount++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return optimizedCount;
        }

        private bool OptimizeSpriteTexture(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return false;

            bool changed = false;

            changed |= ApplySpriteIdentity(importer);              // Sprite 类型 / PPU / Mesh / Alpha
            changed |= MapTextureImportRules.ApplyCommonBasics(importer, true); // Read/Write / mipmap / Filter / Wrap / RGBA32
            changed |= ApplyNpotScaleToNearest(importer);
            changed |= ApplyDefaultPlatformBudget(importer);        // Max Size 2048 + Mitchell

            // 清除旧版本其他平台残留压缩覆盖
            foreach (var platform in LegacyPlatforms)
            {
                ClearLegacyPlatformOverride(importer, platform, ref changed);
            }

            if (changed)
            {
                importer.SaveAndReimport();
                MapToolLog.Info($"[纹理优化] {assetPath} → Sprite + RGBA32(不压缩) + 关闭mipmap + Read/Write");
            }

            return changed;
        }

        /// <summary>
        /// 旧版本遗留的平台压缩覆盖（需逐个清除，避免继承旧压缩规则）
        /// </summary>
        private static readonly string[] LegacyPlatforms = { "Standalone", "Android", "iPhone" };

        /// <summary>
        /// 应用 Sprite 身份设置：类型 / 导入模式 / Pixels Per Unit / Mesh Type / Alpha 透明
        /// </summary>
        /// <param name="importer">纹理导入器</param>
        /// <returns>是否发生修改</returns>
        private static bool ApplySpriteIdentity(TextureImporter importer)
        {
            bool changed = false;

            // Sprite 类型（2D UI 模式）
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                changed = true;
            }

            // Pixels Per Unit: 100
            if (!Mathf.Approximately(importer.spritePixelsPerUnit, 100f))
            {
                importer.spritePixelsPerUnit = 100f;
                changed = true;
            }

            // Mesh Type: Tight
            var importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);
            if (importerSettings.spriteMeshType != SpriteMeshType.Tight)
            {
                importerSettings.spriteMeshType = SpriteMeshType.Tight;
                importer.SetTextureSettings(importerSettings);
                changed = true;
            }

            // Alpha 透明（PSD 图层导出图需要）
            if (!importer.alphaIsTransparency)
            {
                importer.alphaIsTransparency = true;
                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// NPOT 缩放策略设为 ToNearest（PSD 切片按最近邻取整，避免非 2 次幂缩放模糊）
        /// </summary>
        /// <param name="importer">纹理导入器</param>
        /// <returns>是否发生修改</returns>
        private static bool ApplyNpotScaleToNearest(TextureImporter importer)
        {
            if (importer.npotScale == TextureImporterNPOTScale.ToNearest) return false;

            importer.npotScale = TextureImporterNPOTScale.ToNearest;
            return true;
        }

        /// <summary>
        /// Default 平台补充：Max Size 2048 + Mitchell 重采样
        /// </summary>
        /// <param name="importer">纹理导入器</param>
        /// <returns>是否发生修改</returns>
        private static bool ApplyDefaultPlatformBudget(TextureImporter importer)
        {
            var defaultSettings = importer.GetPlatformTextureSettings(MapTextureImportRules.DefaultPlatform);
            if (defaultSettings.maxTextureSize == 2048 &&
                defaultSettings.resizeAlgorithm == TextureResizeAlgorithm.Mitchell)
            {
                return false;
            }

            defaultSettings.overridden = true;
            defaultSettings.maxTextureSize = 2048;
            defaultSettings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            importer.SetPlatformTextureSettings(defaultSettings);
            return true;
        }

        /// <summary>
        /// 清除指定平台残留的旧压缩覆盖设置，恢复默认（不覆盖）
        /// </summary>
        private void ClearLegacyPlatformOverride(TextureImporter importer, string platform, ref bool changed)
        {
            var settings = importer.GetPlatformTextureSettings(platform);
            if (settings.overridden)
            {
                settings.overridden = false;
                importer.SetPlatformTextureSettings(settings);
                changed = true;
            }
        }

        #endregion

        #region Python 调用

        /// <summary>
        /// 查找可用的 Python 解释器（PATH + 常见安装目录）
        /// </summary>
        private string FindPython()
        {
            string[] pythonPaths =
            {
                "python",
                @"C:\Users\Administrator\AppData\Local\Programs\Python\Python313\python.exe",
                @"C:\Users\Administrator\AppData\Local\Programs\Python\Python312\python.exe",
                @"C:\Python312\python.exe",
                @"C:\Python311\python.exe",
                @"C:\Python310\python.exe",
            };

            foreach (var p in pythonPaths)
            {
                try
                {
                    var testProc = Process.Start(new ProcessStartInfo
                    {
                        FileName = p,
                        Arguments = "--version",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                    });
                    testProc.WaitForExit(3000);
                    if (testProc.ExitCode == 0) return p;
                }
                catch { /* 尝试下一个候选路径 */ }
            }
            return null;
        }

        /// <summary>
        /// 运行 Python 解析脚本（参数: script.psd chapter_dir shared_dir json_dir level_id）
        /// </summary>
        private void RunPythonScript(string pythonExe, string scriptPath, string psdPath,
            string chapterDir, string sharedDir, string jsonDir, string levelId,
            out string output, out string error)
        {
            var process = new Process();
            process.StartInfo.FileName = pythonExe;
            process.StartInfo.Arguments = $"\"{scriptPath}\" \"{psdPath}\" \"{chapterDir}\" \"{sharedDir}\" \"{jsonDir}\" \"{levelId}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            output = "";
            error = "";
            try
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                MapToolLog.Info($"[PSD导入] 输出:\n{output}");
                if (!string.IsNullOrEmpty(error))
                    MapToolLog.Warn($"[PSD导入] 错误:\n{error}");
            }
            catch (Exception e)
            {
                MapToolDialog.Info("导入失败", $"无法调用 Python 脚本:\n{e.Message}\n\n请确认已安装 Python 和 psd-tools 库");
            }
        }

        #endregion

        #region MD5 清单

        /// <summary>
        /// 生成图片 MD5 清单（下次导入对比哪些图片有变化）
        /// </summary>
        private void GenerateMD5Manifest(PsdImportData importData, string chapterDir)
        {
            var manifest = new MD5ManifestData();

            if (!string.IsNullOrEmpty(importData.backgroundPath) && File.Exists(importData.backgroundPath))
            {
                string md5 = ComputeFileMD5(importData.backgroundPath);
                if (md5 != null)
                    manifest.items.Add(new MD5Item { name = Path.GetFileName(importData.backgroundPath), md5 = md5 });
            }

            foreach (var obj in importData.objects)
            {
                if (string.IsNullOrEmpty(obj.pngPath) || !File.Exists(obj.pngPath)) continue;
                string md5 = ComputeFileMD5(obj.pngPath);
                if (md5 != null)
                    manifest.items.Add(new MD5Item { name = Path.GetFileName(obj.pngPath), md5 = md5 });
            }

            string manifestPath = Path.Combine(chapterDir, "md5_manifest.json");
            string json = JsonUtility.ToJson(manifest, true);
            File.WriteAllText(manifestPath, json);

            MapToolLog.Info($"[PSD导入] MD5 清单已生成: {manifestPath}，共 {manifest.items.Count} 个图片");
        }

        /// <summary>计算文件 MD5 值</summary>
        private string ComputeFileMD5(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>统计字符串出现次数</summary>
        private int CountOccurrences(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) return 0;
            int count = 0, index = 0;
            while ((index = text.IndexOf(pattern, index)) != -1)
            {
                count++;
                index += pattern.Length;
            }
            return count;
        }

        #endregion

        #region 数据结构

        /// <summary>PSD 解析结果根数据</summary>
        [Serializable]
        public class PsdImportData
        {
            /// <summary>地图宽（像素）</summary>
            public int mapWidth;

            /// <summary>地图高（像素）</summary>
            public int mapHeight;

            /// <summary>背景图层名</summary>
            public string backgroundName;

            /// <summary>背景图路径（磁盘绝对路径）</summary>
            public string backgroundPath;

            /// <summary>物件集合</summary>
            public List<PsdObjectInfo> objects;
        }

        /// <summary>PSD 单图层物件数据</summary>
        [Serializable]
        public class PsdObjectInfo
        {
            /// <summary>图层显示名</summary>
            public string name;

            /// <summary>PSD 原始图层名</summary>
            public string layerName;

            /// <summary>类型键（兼容旧字段）</summary>
            public string type;

            /// <summary>八层类型键（background/ground/building/prop/character/cat/effect/bubble）</summary>
            public string layerType;

            /// <summary>渲染层级 SortOrder</summary>
            public int sortOrder;

            /// <summary>PSD 左上角 X（像素）</summary>
            public float x;

            /// <summary>PSD 左上角 Y（像素）</summary>
            public float y;

            /// <summary>PSD 图层宽（像素，旧字段）</summary>
            public float width;

            /// <summary>PSD 图层高（像素，旧字段）</summary>
            public float height;

            /// <summary>PSD 图层原始像素宽</summary>
            public float pixelWidth;

            /// <summary>PSD 图层原始像素高</summary>
            public float pixelHeight;

            /// <summary>是否拉伸到配置尺寸</summary>
            public bool stretch;

            /// <summary>切片 PNG 路径（磁盘绝对路径）</summary>
            public string pngPath;
        }

        /// <summary>MD5 清单根数据</summary>
        [Serializable]
        private class MD5ManifestData
        {
            /// <summary>清单条目集合</summary>
            public List<MD5Item> items = new List<MD5Item>();
        }

        /// <summary>MD5 清单条目</summary>
        [Serializable]
        private class MD5Item
        {
            /// <summary>图片文件名</summary>
            public string name;

            /// <summary>文件 MD5</summary>
            public string md5;
        }

        #endregion
    }
}
