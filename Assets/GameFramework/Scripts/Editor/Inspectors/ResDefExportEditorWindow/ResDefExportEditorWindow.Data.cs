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
        /// <summary>
        /// 资源集合
        /// </summary>
        public class ResDefInfos
        {
            /// <summary>
            /// Lua导出路径
            /// </summary>
            public static string LuaExportFolderPath = Runtime.GamePathUtils.Editor.ResDef.LuaFolderPath;

            /// <summary>
            /// 存储所有类型的ResDefItem数据
            /// </summary>
            public static List<ResDefItem> ConvertData = new List<ResDefItem>();

            /// <summary>
            /// 写Json文件
            /// </summary>
            public static void WriteJson()
            {
                using (FileStream fs = new FileStream(Runtime.GamePathUtils.Editor.ResDef.GetResDefExportWindowsSettingsFullPath(), FileMode.Create, FileAccess.Write))
                {
                    var resDefExportSettings = new ResDefExportSettings();
                    resDefExportSettings.LuaExportFolderPath = LuaExportFolderPath;
                    resDefExportSettings.DefaultSheetCount = m_OneSheetMaxCount;
                    resDefExportSettings.ConvertData = ConvertData;
                    var bytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resDefExportSettings,Formatting.Indented));
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            /// <summary>
            /// 转换json数据到变量
            /// </summary>
            public static void ConvertJson()
            {
                string filePath = Runtime.GamePathUtils.Editor.ResDef.GetResDefExportWindowsSettingsFullPath();
                if (File.Exists(filePath))
                {
                    var file = File.ReadAllText(filePath);
                    try
                    {
                        var resDefExportSettings = JsonConvert.DeserializeObject<ResDefExportSettings>(file);
                        int idCounter = 1;
                        foreach (var item in resDefExportSettings.ConvertData)
                        {
                            item.ID = idCounter++;
                        }
                        LuaExportFolderPath = resDefExportSettings.LuaExportFolderPath;
                        ConvertData = resDefExportSettings.ConvertData;
                        m_OneSheetMaxCount = resDefExportSettings.DefaultSheetCount;
                    
                    }
                    catch (Exception ex)
                    {
                        Log.Error( $"[Editor] {filePath} 加载失败，错误信息：{ex} 。");
                    }

                    // 获取当前最大的ResID
                    var curMaxData = GetMaxResID();
                    // 矫正重复的ID,去除每个重复的数组的第一个元素
                    var sortConvertData = ConvertData.GroupBy(item => item.ID).Where(group => group.Count() > 1).SelectMany(group => group.Skip(1)).ToList();
                    // 重新给ID赋值
                    foreach (var resDefItem in sortConvertData)
                    {
                        resDefItem.ID = ++curMaxData;
                    }
                }

                ConvertData.ForEach(resDefItem =>
                {
                    if (string.IsNullOrEmpty(resDefItem.AssetGUID))
                    {
                        var GUIDArray = AssetDatabase.FindAssets(resDefItem.AssetName, new string[] { resDefItem.ABPath });
                        if (GUIDArray.Length > 0)
                        {
                            foreach (var GUID in GUIDArray)
                            {
                                var assetPath = AssetDatabase.GUIDToAssetPath(GUID);
                                if (System.IO.Path.GetFileNameWithoutExtension(assetPath) == resDefItem.AssetName) // 文件名字一致，则可以进行guid赋值
                                {
                                    resDefItem.AssetGUID = GUID;
                                }
                            }
                        }
                    }
                });
                WriteJson();
            }

            /// <summary>
            /// 得到存档中的最大的ResID
            /// </summary>
            public static int GetMaxResID()
            {
                var maxResID = 0;
                foreach (var resDefItemInfo in ConvertData)
                {
                    maxResID = Math.Max(maxResID, resDefItemInfo.ID);
                }

                return maxResID;
            }
        }

        /// <summary>
        /// 资源的信息定义
        /// </summary>
        public class ResDefItem
        {
            /// <summary>
            /// 资源的编号ID （每次启动时初始化编号）
            /// </summary>
            [JsonIgnore]
            public int ID = -1;

            /// <summary>
            /// 资源的类型
            /// </summary>
            public string ResType;

            /// <summary>
            /// 资源的别名
            /// </summary>
            public string AliasName;

            /// <summary>
            /// AB路径
            /// </summary>
            public string ABPath;

            /// <summary>
            /// 资源名称
            /// </summary>
            public string AssetName;

            /// <summary>
            /// 资源的uid
            /// </summary>
            public string AssetGUID;
        }

        
        /// <summary>
        /// 导出数据的数据结构
        /// </summary>
        public class ResDefExportSettings
        {
            public string LuaExportFolderPath;
            public int DefaultSheetCount;
            public List<ResDefItem> ConvertData;
        }

        /// <summary>
        /// 右侧导出项的信息标记
        /// </summary>
        public class TopItemInfo
        {
            /// <summary>
            /// 是否是折叠状态
            /// </summary>
            public bool IsShowFoldout = false;

            /// <summary>
            /// 当前page下表
            /// </summary>
            public int CurPageIndex = 1;

            /// <summary>
            /// page的总数量
            /// </summary>
            public int PageCount = 3;

            /// <summary>
            /// 是否是全部展开状态
            /// </summary>
            public bool IsShowAll = false;

            /// <summary>
            /// 一页显示多少个
            /// </summary>
            public int OnePageCount = 12;
        }


        /// <summary>
        /// 文件的使用状态
        /// </summary>
        public enum FileUseState
        {
            /// <summary>
            /// 文件加载成功
            /// </summary>
            LoadSuccess = 0,

            /// <summary>
            /// 文件追加成功
            /// </summary>
            ExportSuccess = 1,

            /// <summary>
            /// 文件追加失败-同名
            /// </summary>
            ExportFailedToSameName = 2,
        }
    }
}