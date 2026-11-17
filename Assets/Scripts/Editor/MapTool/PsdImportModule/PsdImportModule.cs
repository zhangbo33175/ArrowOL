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

        /// <summary>
        /// 点击"导入 PSD"按钮
        /// 前置条件：必须先在左侧选择章节与关卡（确定资源归属目录）
        /// </summary>
        public void ImportPsd()
        {
            // ===== 前置校验：章节/关卡 =====
            string chapterId = m_Owner?.CurrentChapterId;
            string levelId = m_Owner?.CurrentLevelId;

            if (string.IsNullOrEmpty(chapterId))
            {
                EditorUtility.DisplayDialog("无法导入",
                    "请先在左侧面板选择关卡，再导入PSD文件。\n\n资源会按 章节/关卡 分类存放，跨关卡复用资源自动归入章节公共目录。", "确定");
                return;
            }
            if (string.IsNullOrEmpty(levelId))
            {
                levelId = "1";
            }

            // ===== 选择 PSD 文件 =====
            string psdPath = EditorUtility.OpenFilePanel("选择 PSD/PSB 地图文件", "", "");
            if (string.IsNullOrEmpty(psdPath)) return;

            // ===== 三级资源目录规划（磁盘绝对路径） =====
            string mapRootDir = Path.GetFullPath(Path.Combine(Application.dataPath, "Res/Textures/Map"));
            string sharedDir = Path.Combine(mapRootDir, "Shared");
            string chapterDir = Path.Combine(mapRootDir, $"Chapter{chapterId}");
            string jsonOutputDir = mapRootDir;

            Directory.CreateDirectory(sharedDir);
            Directory.CreateDirectory(chapterDir);

            Debug.Log($"[PSD导入] 章节: {chapterId}, 关卡: {levelId}");
            Debug.Log($"[PSD导入] 全局共享: {sharedDir}");
            Debug.Log($"[PSD导入] 章节目录: {chapterDir}");

            // ===== 调用 Python 脚本解析 PSD =====
            string scriptPath = Path.Combine(Application.dataPath, "Scripts/Editor/MapTool/Common/psd_import.py");
            string pythonExe = FindPython();
            if (pythonExe == null)
            {
                EditorUtility.DisplayDialog("导入失败", "找不到 Python，请确认已安装 Python、psd-tools 并添加到 PATH", "确定");
                return;
            }

            string output, error;
            RunPythonScript(pythonExe, scriptPath, psdPath, chapterDir, sharedDir, jsonOutputDir, levelId, out output, out error);

            // ===== 读取解析结果 JSON =====
            string jsonPath = Path.Combine(jsonOutputDir, "import_result.json");
            if (!File.Exists(jsonPath))
            {
                string errorMsg = $"未找到解析结果 JSON 文件\n\n章节目录: {chapterDir}\n共享目录: {sharedDir}\n\nPython 输出:\n{output}\n\n错误:\n{error}";
                EditorUtility.DisplayDialog("导入失败", errorMsg, "确定");
                return;
            }

            string json = File.ReadAllText(jsonPath);
            var importData = JsonUtility.FromJson<PsdImportData>(json);
            if (importData.objects == null) importData.objects = new List<PsdObjectInfo>();

            // 应用到编辑器
            ApplyImportData(importData);

            // 自动优化新导出图片的纹理格式（压缩+关闭 mipmap，节省内存）
            int optimizedCount = OptimizeNewlyExportedTextures(importData);
            Debug.Log($"[PSD导入] 已优化 {optimizedCount} 张图片的导入设置");

            // 生成 MD5 清单（记录所有导出图片 MD5，下次导入对比变化）
            GenerateMD5Manifest(importData, chapterDir);

            // 统计新导出 vs 已存在（从 Python 输出中提取；global=跨章节公共，chapter=章节公共，level=既有关卡复用）
            int newExported = CountOccurrences(output, "Exported:");
            int skippedShared = CountOccurrences(output, "Skip global:") + CountOccurrences(output, "Skip chapter:") + CountOccurrences(output, "Skip level:");
            int skippedDup = CountOccurrences(output, "Skip duplicate:");

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

            EditorUtility.DisplayDialog("PSD 导入完成", msg, "确定");
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

            // 1. 地图尺寸
            OnMapSizeSet?.Invoke(data.mapWidth, data.mapHeight);
            Debug.Log($"[PSD导入] 地图尺寸: {data.mapWidth} x {data.mapHeight}");
            Debug.Log($"[PSD导入] 背景图路径: {data.backgroundPath}");
            Debug.Log($"[PSD导入] 物件数量: {data.objects.Count}");

            // 2. 背景图（转换为 Assets 相对路径）
            if (!string.IsNullOrEmpty(data.backgroundPath))
            {
                string assetBgPath = MapToolPathHelper.ToAssetRelative(data.backgroundPath);
                Texture2D bgTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetBgPath);
                if (bgTexture != null)
                    Debug.Log($"[PSD导入] 背景纹理实际尺寸: {bgTexture.width} x {bgTexture.height}");
                else
                    Debug.LogWarning($"[PSD导入] 背景纹理加载失败: {assetBgPath}");

                OnBackgroundSet?.Invoke(assetBgPath);
            }

            // 3. 刷新 AssetDatabase，确保 Unity 扫描到刚导出的 PNG
            AssetDatabase.Refresh();

            // 4. 创建物件（已在配置表中的沿用模板类型与参数）
            var newObjects = new List<string>();

            foreach (var obj in data.objects)
            {
                string layerType = MapLayerCatalog.Normalize(obj.layerType);
                MapObjectType objType = MapLayerCatalog.ResolveObjectType(layerType);

                // 配置表已有同名模板 → 沿用模板类型；否则记入新物件提示
                var template = ObjectTemplateManager.Instance.GetTemplateByName(obj.name);
                if (template != null)
                {
                    objType = template.Type;
                }
                else
                {
                    newObjects.Add($"{obj.name} ({layerType})");
                }

                // 预览图
                string assetPngPath = MapToolPathHelper.ToAssetRelative(obj.pngPath);
                Texture2D previewTex = null;
                if (!string.IsNullOrEmpty(assetPngPath))
                {
                    previewTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPngPath);
                    if (previewTex == null)
                        Debug.LogWarning($"[PSD导入] 预览图加载失败: {assetPngPath}");
                }

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

                OnObjectCreated?.Invoke(placedObj);
            }

            // 5. 刷新场景
            OnRefreshScene?.Invoke();

            // 6. 导入完成后自动落盘编辑器工作信息（仅写工作 JSON，不触发发布 Lua 导出）
            //    下次选中关卡由 LoadMapConfig 直接还原，无需重新导入 PSD
            m_Owner?.SaveWorkingConfig();

            // 7. 提示新物件
            if (newObjects.Count > 0)
            {
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

                EditorUtility.DisplayDialog("PSD 导入完成", msg.ToString(), "确定");
            }
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

        /// <summary>
        /// 优化单张图片纹理导入设置（已正确的不覆盖）
        /// </summary>
        private bool OptimizeSpriteTexture(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return false;

            bool changed = false;

            // 1. Sprite 类型（2D UI 模式）
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                changed = true;
            }

            // 2. Pixels Per Unit: 100
            if (!Mathf.Approximately(importer.spritePixelsPerUnit, 100f))
            {
                importer.spritePixelsPerUnit = 100f;
                changed = true;
            }

            // 3. Mesh Type: Tight
            var importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);
            if (importerSettings.spriteMeshType != SpriteMeshType.Tight)
            {
                importerSettings.spriteMeshType = SpriteMeshType.Tight;
                importer.SetTextureSettings(importerSettings);
                changed = true;
            }

            // 4. 关闭 mipmap
            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }

            // 5. Alpha 透明
            if (!importer.alphaIsTransparency)
            {
                importer.alphaIsTransparency = true;
                changed = true;
            }

            // 6. Read/Write 开启
            if (!importer.isReadable)
            {
                importer.isReadable = true;
                changed = true;
            }

            // 7. Wrap Mode: Clamp
            if (importer.wrapMode != TextureWrapMode.Clamp)
            {
                importer.wrapMode = TextureWrapMode.Clamp;
                changed = true;
            }

            // 8. Filter Mode: Bilinear
            if (importer.filterMode != FilterMode.Bilinear)
            {
                importer.filterMode = FilterMode.Bilinear;
                changed = true;
            }

            // 9. NPOT Scale: ToNearest
            if (importer.npotScale != TextureImporterNPOTScale.ToNearest)
            {
                importer.npotScale = TextureImporterNPOTScale.ToNearest;
                changed = true;
            }

            // 10. Default 平台: Max Size 2048 + Mitchell + RGBA32 + 关闭压缩
            var defaultSettings = importer.GetPlatformTextureSettings("DefaultTexturePlatform");
            if (!defaultSettings.overridden ||
                defaultSettings.format != TextureImporterFormat.RGBA32 ||
                defaultSettings.maxTextureSize != 2048 ||
                defaultSettings.resizeAlgorithm != TextureResizeAlgorithm.Mitchell ||
                defaultSettings.textureCompression != TextureImporterCompression.Uncompressed)
            {
                defaultSettings.overridden = true;
                defaultSettings.format = TextureImporterFormat.RGBA32;
                defaultSettings.maxTextureSize = 2048;
                defaultSettings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
                defaultSettings.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SetPlatformTextureSettings(defaultSettings);
                changed = true;
            }

            // 11. 清除旧版本其他平台残留压缩覆盖
            ClearLegacyPlatformOverride(importer, "Standalone", ref changed);
            ClearLegacyPlatformOverride(importer, "Android", ref changed);
            ClearLegacyPlatformOverride(importer, "iPhone", ref changed);

            if (changed)
            {
                importer.SaveAndReimport();
                Debug.Log($"[纹理优化] {assetPath} → Sprite + RGBA32(不压缩) + 关闭mipmap + Read/Write");
            }

            return changed;
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

                Debug.Log($"[PSD导入] 输出:\n{output}");
                if (!string.IsNullOrEmpty(error))
                    Debug.LogWarning($"[PSD导入] 错误:\n{error}");
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("导入失败", $"无法调用 Python 脚本:\n{e.Message}\n\n请确认已安装 Python 和 psd-tools 库", "确定");
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

            Debug.Log($"[PSD导入] MD5 清单已生成: {manifestPath}，共 {manifest.items.Count} 个图片");
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
