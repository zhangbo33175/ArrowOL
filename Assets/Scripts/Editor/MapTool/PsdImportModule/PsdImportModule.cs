/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PsdImportModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   PSD/PSB 导入模块 - 解析 PSD 图层、导出切片、自动生成地图物件
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
    /// PSD/PSB 导入模块：负责解析 PSD 文件、导出图层、生成地图物件
    /// </summary>
    public class PsdImportModule
    {
        // ===== 回调：由 SceneEditModule 注入 =====
        public Action<int, int> OnMapSizeSet;
        public Action<string> OnBackgroundSet;
        public Action<PlacedObject> OnObjectCreated;
        public Action OnRefreshScene;

        private SceneEditModule m_Owner;

        public PsdImportModule(SceneEditModule owner)
        {
            m_Owner = owner;
        }

        /// <summary>
        /// 点击"导入 PSD"按钮
        /// 前置条件：必须先在左侧选择关卡（确定当前所属章节）
        /// </summary>
        public void ImportPsd()
        {
            // ===== 前置校验 1：检查是否已选择关卡/章节 =====
            string chapterId = m_Owner?.CurrentChapterId;
            string levelId = m_Owner?.CurrentLevelId;

            if (string.IsNullOrEmpty(chapterId))
            {
                EditorUtility.DisplayDialog("无法导入",
                    "请先在左侧面板选择关卡，再导入PSD文件。\n\n" +
                    "资源会按章节分类存放，便于同章节内复用。", "确定");
                return;
            }

            // ===== 选择PSD文件 =====
            string psdPath = EditorUtility.OpenFilePanel("选择 PSD/PSB 地图文件", "", "");
            if (string.IsNullOrEmpty(psdPath)) return;

            // ===== 三级资源目录规划 =====
            // L0: Shared 目录（全局共享，所有章节通用）
            // L1: Chapter{id} 目录（当前章节专属）
            string mapRootDir = Path.GetFullPath(Path.Combine(
                Application.dataPath,
                "Res/Textures/Map"
            ));
            string sharedDir = Path.Combine(mapRootDir, "Shared");
            string chapterDir = Path.Combine(mapRootDir, $"Chapter{chapterId}");

            Directory.CreateDirectory(sharedDir);
            Directory.CreateDirectory(chapterDir);

            // JSON结果文件输出到 Map 根目录
            string jsonOutputDir = mapRootDir;

            Debug.Log($"[PSD导入] 当前章节: {chapterId}, 关卡: {levelId}");
            Debug.Log($"[PSD导入] 共享目录: {sharedDir}");
            Debug.Log($"[PSD导入] 章节目录: {chapterDir}");

            // 调用 Python 脚本解析 PSD
            string scriptPath = Path.Combine(
                Application.dataPath,
                "Scripts/Editor/MapTool/Common/psd_import.py"
            );

            // 尝试多个 Python 路径（Unity 的 PATH 里可能没有 python）
            string[] pythonPaths = new string[]
            {
                "python",
                @"C:\Users\Administrator\AppData\Local\Programs\Python\Python312\python.exe",
                @"C:\Python312\python.exe",
                @"C:\Python311\python.exe",
                @"C:\Python310\python.exe",
            };

            string pythonExe = FindPython(pythonPaths);
            if (pythonExe == null)
            {
                EditorUtility.DisplayDialog("导入失败", "找不到 Python，请确认已安装 Python 并添加到 PATH", "确定");
                return;
            }

            // 传入三个目录参数：chapter_dir shared_dir json_dir
            string output, error;
            RunPythonScriptV2(pythonExe, scriptPath, psdPath, chapterDir, sharedDir, jsonOutputDir, out output, out error);

            // 读取生成的 JSON
            string jsonPath = Path.Combine(jsonOutputDir, "import_result.json");
            if (!File.Exists(jsonPath))
            {
                string errorMsg = $"未找到解析结果 JSON 文件\n\n章节目录: {chapterDir}\n共享目录: {sharedDir}\n\nPython 输出:\n{output}\n\n错误:\n{error}";
                EditorUtility.DisplayDialog("导入失败", errorMsg, "确定");
                return;
            }

            string json = File.ReadAllText(jsonPath);
            var importData = JsonUtility.FromJson<PsdImportData>(json);

            // 应用到编辑器
            ApplyImportData(importData, chapterDir);

            // 自动优化新导出图片的纹理格式（压缩+关闭mipmap，节省内存）
            int optimizedCount = OptimizeNewlyExportedTextures(importData);
            Debug.Log($"[PSD导入] 已优化 {optimizedCount} 张图片的导入设置");

            // 生成 MD5 清单文件（记录所有导出图片的 MD5，下次导入时对比变化）
            GenerateMD5Manifest(importData, chapterDir);

            // 统计新导出 vs 已存在（从 Python 输出中提取）
            int newExported = CountOccurrences(output, "Exported:");
            int skippedShared = CountOccurrences(output, "Skip shared:");
            int skippedChapter = CountOccurrences(output, "Skip chapter:");
            int skippedDup = CountOccurrences(output, "Skip duplicate:");

            string msg = $"当前章节: Chapter{chapterId} / 关卡 {levelId}\n" +
                         $"地图尺寸: {importData.mapWidth}x{importData.mapHeight}\n" +
                         $"背景图: {importData.backgroundName}\n" +
                         $"物件总数: {importData.objects.Count}\n" +
                         $"  ├─ 新导出到章节: {newExported} 个\n" +
                         $"  ├─ 引用共享资源: {skippedShared} 个\n" +
                         $"  ├─ 章节内已存在: {skippedChapter} 个\n" +
                         $"  └─ 同PSD内重复: {skippedDup} 个\n" +
                         $"  ────────────────────\n" +
                         $"  已自动优化纹理格式: {optimizedCount} 张\n" +
                         $"  （压缩格式 + 关闭mipmap）\n\n" +
                         $"物件位置已按 PSD 图层位置自动摆放";

            EditorUtility.DisplayDialog("PSD 导入完成", msg, "确定");
        }

        /// <summary>
        /// 统计字符串中出现次数
        /// </summary>
        private int CountOccurrences(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) return 0;
            int count = 0;
            int index = 0;
            while ((index = text.IndexOf(pattern, index)) != -1)
            {
                count++;
                index += pattern.Length;
            }
            return count;
        }

        /// <summary>
        /// 自动优化新导出图片的纹理导入设置
        /// 所有图片统一按照图二的设置：Sprite 类型 + RGBA 32 bit（不压缩）+ 关闭mipmap + Read/Write
        /// </summary>
        /// <param name="importData">PSD导入数据</param>
        /// <returns>优化的图片数量</returns>
        private int OptimizeNewlyExportedTextures(PsdImportData importData)
        {
            int optimizedCount = 0;
            var processedPaths = new HashSet<string>();

            // 处理背景图
            if (!string.IsNullOrEmpty(importData.backgroundPath))
            {
                string assetPath = MapToolPathHelper.ToAssetRelative(importData.backgroundPath);
                if (!string.IsNullOrEmpty(assetPath) && processedPaths.Add(assetPath))
                {
                    if (OptimizeSpriteTexture(assetPath))
                        optimizedCount++;
                }
            }

            // 处理所有物件图片
            foreach (var obj in importData.objects)
            {
                if (string.IsNullOrEmpty(obj.pngPath)) continue;
                string assetPath = MapToolPathHelper.ToAssetRelative(obj.pngPath);
                if (string.IsNullOrEmpty(assetPath)) continue;
                if (!processedPaths.Add(assetPath)) continue;

                if (OptimizeSpriteTexture(assetPath))
                    optimizedCount++;
            }

            // 保存所有修改
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return optimizedCount;
        }

        /// <summary>
        /// 优化图片纹理（按照图二的设置）
        /// 所有图片统一用：Sprite 类型 + RGBA 32 bit（不压缩）+ 关闭mipmap + Read/Write
        /// 注意：只有当设置不正确时才修改，已经正确的不覆盖
        /// </summary>
        private bool OptimizeSpriteTexture(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return false;

            bool changed = false;

            // 1. 设置为 Sprite 类型（2D UI 模式）
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                changed = true;
            }

            // 2. Pixels Per Unit: 100
            if (importer.spritePixelsPerUnit != 100f)
            {
                importer.spritePixelsPerUnit = 100f;
                changed = true;
            }

            // 3. Mesh Type: Tight（spriteMeshType 存放在 TextureImporterSettings 结构体中）
            var importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);
            if (importerSettings.spriteMeshType != SpriteMeshType.Tight)
            {
                importerSettings.spriteMeshType = SpriteMeshType.Tight;
                importer.SetTextureSettings(importerSettings);
                changed = true;
            }

            // 4. 关闭 mipmap（2D 图片不需要）
            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }

            // 5. alpha 通道设置
            if (!importer.alphaIsTransparency)
            {
                importer.alphaIsTransparency = true;
                changed = true;
            }

            // 6. Read/Write 开启（需要在代码中读取像素）
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

            // 10. Default 平台: Max Size 2048 + Mitchell + RGBA 32 bit + 关闭压缩（不压缩）
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

            // 11. 清除旧版本在其他平台残留的压缩覆盖（DXT5/ASTC），恢复默认不覆盖
            // 注意：Default 平台不能这样清除（会触发兼容性报错），其余平台可安全关闭覆盖
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
        /// 清除指定平台上残留的旧压缩覆盖设置，恢复为默认（不覆盖）
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

        /// <summary>
        /// 查找可用的 Python 路径
        /// </summary>
        private string FindPython(string[] pythonPaths)
        {
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
                    if (testProc.ExitCode == 0)
                    {
                        return p;
                    }
                }
                catch { }
            }
            return null;
        }

        /// <summary>
        /// 运行 Python 脚本
        /// </summary>
        private void RunPythonScript(string pythonExe, string scriptPath, string psdPath, string outputDir,
            out string output, out string error)
        {
            var process = new Process();
            process.StartInfo.FileName = pythonExe;
            process.StartInfo.Arguments = $"\"{scriptPath}\" \"{psdPath}\" \"{outputDir}\"";
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

        /// <summary>
        /// 运行 Python 脚本（V2：支持章节目录 + 共享目录 + JSON输出目录 三个参数）
        /// </summary>
        /// <param name="chapterDir">章节专属资源导出目录（Chapter{id}）</param>
        /// <param name="sharedDir">全局共享资源目录（Shared）</param>
        /// <param name="jsonDir">JSON结果文件输出目录（Map根目录）</param>
        private void RunPythonScriptV2(string pythonExe, string scriptPath, string psdPath,
            string chapterDir, string sharedDir, string jsonDir,
            out string output, out string error)
        {
            var process = new Process();
            process.StartInfo.FileName = pythonExe;
            // 参数顺序: script.psd chapter_dir shared_dir json_dir
            process.StartInfo.Arguments = $"\"{scriptPath}\" \"{psdPath}\" \"{chapterDir}\" \"{sharedDir}\" \"{jsonDir}\"";
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
        private void ApplyImportData(PsdImportData data, string outputDir)
        {
            // 1. 设置地图尺寸
            OnMapSizeSet?.Invoke(data.mapWidth, data.mapHeight);

            Debug.Log($"[PSD导入] 地图尺寸: {data.mapWidth} x {data.mapHeight}");
            Debug.Log($"[PSD导入] 背景图路径: {data.backgroundPath}");
            Debug.Log($"[PSD导入] 背景图名称: {data.backgroundName}");
            Debug.Log($"[PSD导入] 物件数量: {data.objects.Count}");

            // 2. 设置背景图（转换为 Assets 相对路径）
            if (!string.IsNullOrEmpty(data.backgroundPath))
            {
                string assetBgPath = MapToolPathHelper.ToAssetRelative(data.backgroundPath);
                Debug.Log($"[PSD导入] 转换后背景路径: {assetBgPath}");

                // 加载背景纹理并打印实际尺寸
                Texture2D bgTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetBgPath);
                if (bgTexture != null)
                {
                    Debug.Log($"[PSD导入] 背景纹理实际尺寸: {bgTexture.width} x {bgTexture.height}");
                }
                else
                {
                    Debug.LogWarning($"[PSD导入] 背景纹理加载失败: {assetBgPath}");
                }

                OnBackgroundSet?.Invoke(assetBgPath);
            }

            // 3. 先刷新 AssetDatabase，确保 Unity 能扫描到刚导出的 PNG
            AssetDatabase.Refresh();

            // 4. 创建物件（同时检查是否已在配置表里）
            var newObjects = new List<string>(); // 不在配置表里的新物件

            foreach (var obj in data.objects)
            {
                MapObjectType objType = MapObjectType.Prop;
                switch (obj.type)
                {
                    case "building": objType = MapObjectType.Building; break;
                    case "character": objType = MapObjectType.Character; break;
                    case "cat": objType = MapObjectType.Cat; break;
                    case "prop": objType = MapObjectType.Prop; break;
                }

                // 检查是否已在配置表里
                var template = ObjectTemplateManager.Instance.GetTemplateByName(obj.name);
                bool isNew = false;

                if (template != null)
                {
                    // 已存在 → 用配置表里的类型和参数
                    objType = template.Type;
                    isNew = false;
                }
                else
                {
                    // 不存在 → 标记为新物件
                    isNew = true;
                    newObjects.Add($"{obj.name} ({obj.type})");
                }

                // 加载预览图
                string assetPngPath = MapToolPathHelper.ToAssetRelative(obj.pngPath);
                Texture2D previewTex = null;
                if (!string.IsNullOrEmpty(assetPngPath))
                {
                    previewTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPngPath);
                    if (previewTex == null)
                        Debug.LogWarning($"[PSD导入] 预览图加载失败: {assetPngPath}");
                }

                // 计算格子尺寸（按像素 / 32）
                int widthCells = Mathf.Max(1, Mathf.RoundToInt(obj.width / 32f));
                int heightCells = Mathf.Max(1, Mathf.RoundToInt(obj.height / 32f));

                var placedObj = new PlacedObject
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                    TemplateId = obj.layerName,
                    Name = obj.name,
                    Type = objType,
                    Position = new Vector2(obj.x, obj.y),
                    Width = widthCells,
                    Height = heightCells,
                    PreviewTexture = previewTex,
                };

                OnObjectCreated?.Invoke(placedObj);
            }

            // 5. 刷新场景
            OnRefreshScene?.Invoke();

            // 6. 提示新物件
            if (newObjects.Count > 0)
            {
                string msg = $"导入完成！\n\n地图尺寸: {data.mapWidth}x{data.mapHeight}\n物件总数: {data.objects.Count}\n\n⚠️ 有 {newObjects.Count} 个新物件不在配置表里：\n";
                foreach (var n in newObjects.Take(10))
                    msg += $"  • {n}\n";
                if (newObjects.Count > 10)
                    msg += $"  ... 等共 {newObjects.Count} 个\n";
                msg += "\n请在「物品元件编辑」Tab 中添加到配置表。";

                EditorUtility.DisplayDialog("PSD 导入完成", msg, "确定");
            }
        }

        // ===== 数据结构 =====
        [Serializable]
        public class PsdImportData
        {
            public int mapWidth;
            public int mapHeight;
            public string backgroundName;
            public string backgroundPath;
            public List<PsdObjectInfo> objects;
        }

        [Serializable]
        public class PsdObjectInfo
        {
            public string name;
            public string layerName;
            public string type;
            public float x;
            public float y;
            public float width;
            public float height;
            public string pngPath;
        }

        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        private string ComputeFileMD5(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        /// <summary>
        /// 生成图片 MD5 清单文件
        /// 记录所有导出图片的 MD5，下次导入时可以对比哪些图片有变化
        /// </summary>
        /// <param name="importData">PSD 导入数据</param>
        /// <param name="chapterDir">章节目录</param>
        private void GenerateMD5Manifest(PsdImportData importData, string chapterDir)
        {
            var manifest = new MD5ManifestData();

            // 背景图
            if (!string.IsNullOrEmpty(importData.backgroundPath) && File.Exists(importData.backgroundPath))
            {
                string fileName = Path.GetFileName(importData.backgroundPath);
                string md5 = ComputeFileMD5(importData.backgroundPath);
                if (md5 != null) manifest.items.Add(new MD5Item { name = fileName, md5 = md5 });
            }

            // 所有物件图片
            foreach (var obj in importData.objects)
            {
                if (string.IsNullOrEmpty(obj.pngPath) || !File.Exists(obj.pngPath)) continue;
                string fileName = Path.GetFileName(obj.pngPath);
                string md5 = ComputeFileMD5(obj.pngPath);
                if (md5 != null) manifest.items.Add(new MD5Item { name = fileName, md5 = md5 });
            }

            // 保存到章节目录下
            string manifestPath = Path.Combine(chapterDir, "md5_manifest.json");
            string json = JsonUtility.ToJson(manifest, true);
            File.WriteAllText(manifestPath, json);

            Debug.Log($"[PSD导入] MD5 清单已生成: {manifestPath}，共 {manifest.items.Count} 个图片");
        }

        /// <summary>
        /// MD5 清单数据结构
        /// </summary>
        [Serializable]
        private class MD5ManifestData
        {
            public List<MD5Item> items = new List<MD5Item>();
        }

        [Serializable]
        private class MD5Item
        {
            public string name;
            public string md5;
        }
    }
}
