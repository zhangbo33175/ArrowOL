/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapConfigRepository.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图配置仓库（纯磁盘 IO 层）
 *            职责：JSON 文件读写、配置表加载；不持有 UI 状态，不依赖 SceneEditModule
 *            组装数据（UI→DTO）和还原数据（DTO→UI）仍由 SceneEditModule 负责
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Editor.MapEditor;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图配置仓库（纯磁盘 IO 层）
    /// 职责边界：
    /// - 读：JSON 文件 → MapConfigData；配置表 → 关卡ID→背景名字典
    /// - 写：MapConfigData → JSON 文件
    /// - 不管：UI 字段怎么填、摆放物列表怎么更新（这些留在 SceneEditModule）
    /// </summary>
    public static class MapConfigRepository
    {
        /// <summary>
        /// 从 JSON 文件读取地图配置
        /// </summary>
        /// <param name="mapSavePath">Assets 相对路径（Assets/xxx.json）</param>
        /// <returns>解析后的配置数据；文件不存在或解析失败返回 null</returns>
        public static MapConfigData LoadFromJson(string mapSavePath)
        {
            if (string.IsNullOrEmpty(mapSavePath)) return null;
            string fullPath = MapToolPathHelper.ToAbsolutePath(mapSavePath);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"地图配置文件不存在: {mapSavePath}");
                return null;
            }
            string json = File.ReadAllText(fullPath, Encoding.UTF8);
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                return JsonUtility.FromJson<MapConfigData>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"地图配置 JSON 解析失败: {mapSavePath}\n{e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 把地图配置写入 JSON 文件
        /// </summary>
        /// <param name="data">要保存的配置数据</param>
        /// <param name="mapSavePath">Assets 相对路径（Assets/xxx.json）</param>
        /// <returns>是否写入成功</returns>
        public static bool SaveToJson(MapConfigData data, string mapSavePath)
        {
            if (data == null)
            {
                Debug.LogWarning("SaveToJson: data 为 null");
                return false;
            }
            if (string.IsNullOrEmpty(mapSavePath))
            {
                Debug.LogWarning("SaveToJson: mapSavePath 为空");
                return false;
            }
            string relativePath = MapToolPathHelper.NormalizeToAssetRelative(mapSavePath);
            string fullPath = MapToolPathHelper.ToAbsolutePath(relativePath);
            try
            {
                string dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(fullPath, json, new UTF8Encoding(false));
                AssetDatabase.Refresh();
                Debug.Log($"关卡地图已保存: {relativePath}（建筑 {data.buildings.Count}，物件 {data.objects.Count}，人物 {data.characters.Count}）");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"地图配置写入失败: {fullPath}\n{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从主配置表加载 关卡ID→背景名 字典
        /// </summary>
        /// <returns>字典（key=关卡ID，value=背景名）；加载失败返回空字典</returns>
        public static Dictionary<string, string> LoadLevelBgDict()
        {
            var dict = new Dictionary<string, string>();
            var dataList = MapToolEditorUtils.LoadTableData<TableMainLevelsEditor>(MapToolConstants.TableMainLevelsPath);
            foreach (var data in dataList)
            {
                if (!string.IsNullOrEmpty(data.ID) && !string.IsNullOrEmpty(data.Level_bg))
                    dict[data.ID] = data.Level_bg;
            }
            Debug.Log($"关卡配置表加载完成，共 {dict.Count} 条");
            return dict;
        }
    }
}
