/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ResDefExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源配置导出工具 - 数据结构定义（分部类）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Honor.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    public partial class ResDefExportEditorWindow : BaseEditorWindow<ResDefExportEditorWindow>
    {
        #region 资源配置数据管理
        /// <summary>
        /// 资源配置集合（静态数据类）
        /// </summary>
        public class ResDefInfos
        {
            /// <summary>
            /// Lua 导出目录
            /// </summary>
            public static string LuaExportFolderPath = GamePathUtils.Editor.ResDef.LuaFolderPath;

            /// <summary>
            /// 所有资源配置数据列表
            /// </summary>
            public static List<ResDefItem> ConvertData = new List<ResDefItem>();

            /// <summary>
            /// 写入 JSON 配置文件
            /// </summary>
            public static void WriteJson()
            {
                string path = GamePathUtils.Editor.ResDef.GetResDefExportWindowsSettingsFullPath();
                
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    ResDefExportSettings settings = new ResDefExportSettings
                    {
                        LuaExportFolderPath = LuaExportFolderPath,
                        DefaultSheetCount = m_OneSheetMaxCount,
                        ConvertData = ConvertData
                    };

                    string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            /// <summary>
            /// 读取并解析 JSON 配置
            /// </summary>
            public static void ConvertJson()
            {
                string path = GamePathUtils.Editor.ResDef.GetResDefExportWindowsSettingsFullPath();
                if (!File.Exists(path))
                {
                    WriteJson();
                    return;
                }

                try
                {
                    string content = File.ReadAllText(path);
                    ResDefExportSettings settings = JsonConvert.DeserializeObject<ResDefExportSettings>(content);

                    // 重新排序 ID
                    int id = 1;
                    foreach (ResDefItem item in settings.ConvertData)
                    {
                        item.ID = id++;
                    }

                    LuaExportFolderPath = settings.LuaExportFolderPath;
                    ConvertData = settings.ConvertData;
                    m_OneSheetMaxCount = settings.DefaultSheetCount;
                }
                catch (Exception ex)
                {
                    Log.Error($"[Editor] 配置文件加载失败：{path}\n{ex}");
                }

                // 修复重复 ID
                int maxId = GetMaxResID();
                List<ResDefItem> duplicateItems = ConvertData
                    .GroupBy(item => item.ID)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.Skip(1))
                    .ToList();

                foreach (ResDefItem item in duplicateItems)
                {
                    item.ID = ++maxId;
                }

                // 自动补全 AssetGUID
                foreach (ResDefItem item in ConvertData)
                {
                    if (string.IsNullOrEmpty(item.AssetGUID))
                    {
                        string[] guids = AssetDatabase.FindAssets(item.AssetName, new[] { item.ABPath });
                        foreach (string guid in guids)
                        {
                            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                            if (Path.GetFileNameWithoutExtension(assetPath) == item.AssetName)
                            {
                                item.AssetGUID = guid;
                                break;
                            }
                        }
                    }
                }

                WriteJson();
            }

            /// <summary>
            /// 获取当前最大资源 ID
            /// </summary>
            public static int GetMaxResID()
            {
                int maxId = 0;
                foreach (ResDefItem item in ConvertData)
                {
                    maxId = Math.Max(maxId, item.ID);
                }
                return maxId;
            }
        }
        #endregion

        #region 数据结构定义
        /// <summary>
        /// 单个资源配置项
        /// </summary>
        public class ResDefItem
        {
            /// <summary>
            /// 资源 ID（运行时编号，不序列化）
            /// </summary>
            [JsonIgnore]
            public int ID = -1;

            /// <summary>
            /// 资源类型
            /// </summary>
            public string ResType;

            /// <summary>
            /// 资源别名
            /// </summary>
            public string AliasName;

            /// <summary>
            /// AB 包路径
            /// </summary>
            public string ABPath;

            /// <summary>
            /// 资源名称
            /// </summary>
            public string AssetName;

            /// <summary>
            /// 资源 GUID
            /// </summary>
            public string AssetGUID;
        }

        /// <summary>
        /// 导出配置文件结构
        /// </summary>
        public class ResDefExportSettings
        {
            public string LuaExportFolderPath;
            public int DefaultSheetCount;
            public List<ResDefItem> ConvertData;
        }

        /// <summary>
        /// 右侧面板折叠项信息
        /// </summary>
        public class TopItemInfo
        {
            /// <summary>
            /// 是否展开
            /// </summary>
            public bool IsShowFoldout = false;

            /// <summary>
            /// 当前页码
            /// </summary>
            public int CurPageIndex = 1;

            /// <summary>
            /// 总页数
            /// </summary>
            public int PageCount = 3;

            /// <summary>
            /// 是否全部展开
            /// </summary>
            public bool IsShowAll = false;

            /// <summary>
            /// 单页显示数量
            /// </summary>
            public int OnePageCount = 12;
        }

        /// <summary>
        /// 文件处理状态
        /// </summary>
        public enum FileUseState
        {
            /// <summary>
            /// 加载成功
            /// </summary>
            LoadSuccess = 0,

            /// <summary>
            /// 导出成功
            /// </summary>
            ExportSuccess = 1,

            /// <summary>
            /// 导出失败 - 重名
            /// </summary>
            ExportFailedToSameName = 2
        }
        #endregion
    }
}