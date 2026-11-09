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
        /// </summary>
        public void ImportPsd()
        {
            string psdPath = EditorUtility.OpenFilePanel("选择 PSD/PSB 地图文件", "", "");
            if (string.IsNullOrEmpty(psdPath)) return;

            // 输出目录：PSD 同目录下的 ExportedLayers 文件夹
            string outputDir = Path.Combine(
                Path.GetDirectoryName(psdPath),
                "ExportedLayers"
            );

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

            string output, error;
            RunPythonScript(pythonExe, scriptPath, psdPath, outputDir, out output, out error);

            // 读取生成的 JSON
            string jsonPath = Path.Combine(outputDir, "import_result.json");
            if (!File.Exists(jsonPath))
            {
                string msg = $"未找到解析结果 JSON 文件\n\n输出目录: {outputDir}\n\nPython 输出:\n{output}\n\n错误:\n{error}";
                EditorUtility.DisplayDialog("导入失败", msg, "确定");
                return;
            }

            string json = File.ReadAllText(jsonPath);
            var importData = JsonUtility.FromJson<PsdImportData>(json);

            // 应用到编辑器
            ApplyImportData(importData, outputDir);

            EditorUtility.DisplayDialog("PSD 导入完成",
                $"地图尺寸: {importData.mapWidth}x{importData.mapHeight}\n背景图: {importData.backgroundName}\n物件数量: {importData.objects.Count}\n\n物件位置已按 PSD 图层位置自动摆放",
                "确定");
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
        /// 应用 PSD 导入结果到编辑器
        /// </summary>
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
    }
}
