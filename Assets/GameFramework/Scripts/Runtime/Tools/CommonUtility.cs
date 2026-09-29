/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  CommonUtility.cs
 * author:    云毅
 * created:   2026
 * descrip:   通用纯静态工具类，收纳跨业务类重复、可参数化的纯逻辑
 ***************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 通用纯静态工具类
    /// 作用：收纳各业务管理器之间重复出现、可参数化的纯逻辑，
    /// 避免相同的"分类 -> 键名列表"字典查询/统计代码在多个 sealed 管理器中各自复制一份。
    /// </summary>
    public static class CommonUtility
    {
        #region 分类索引纯查询（classifyName -> itemName 列表）

        /// <summary>
        /// 在"分类 -> 键名列表"索引中，取出指定分类下的全部键名（数组形式）
        /// </summary>
        /// <param name="groups">分类索引字典：键为分类名，值为该分类下键名列表。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <returns>键名数组；分类不存在时返回 null。</returns>
        public static string[] GetItemNamesByGroup(SortedDictionary<string, List<string>> groups, string classifyName)
        {
            List<string> range = null;
            if (groups.TryGetValue(classifyName, out range))
            {
                return range.ToArray();
            }

            return null;
        }

        /// <summary>
        /// 在"分类 -> 键名列表"索引中，把指定分类下的全部键名填充到 results
        /// </summary>
        /// <param name="groups">分类索引字典：键为分类名，值为该分类下键名列表。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="results">输出集合：方法内部先清空再填充。</param>
        public static void AppendItemNamesByGroup(SortedDictionary<string, List<string>> groups, string classifyName, List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results 无效。");
            }

            results.Clear();

            List<string> range = null;
            if (groups.TryGetValue(classifyName, out range))
            {
                results.AddRange(range);
            }
        }

        /// <summary>
        /// 统计"分类 -> 键名列表"索引中指定分类下的键名数量
        /// </summary>
        /// <param name="groups">分类索引字典：键为分类名，值为该分类下键名列表。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <returns>键名数量；字典或分类不存在时返回 0。</returns>
        public static int CountItemsByGroup(SortedDictionary<string, List<string>> groups, string classifyName)
        {
            if (groups != null && groups.ContainsKey(classifyName))
            {
                return groups[classifyName].Count;
            }

            return 0;
        }

        #endregion

        #region 分类索引 - 持久化读写（PlayerPrefs 系管理器复用，仅以常量键名参数化）

        /// <summary>
        /// 判断指定分类下是否存在某键（PlayerPrefs 键名为 {classifyName}_{itemName}）
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">键名。</param>
        /// <returns>是否存在该键。</returns>
        public static bool HasPlayerPrefsItem(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// 把键名登记进"分类 -> 键名列表"内存索引（缺失时补充分类/键名）
        /// </summary>
        /// <param name="groups">分类索引字典。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">键名。</param>
        /// <returns>索引是否被改动（true=新增了分类或键名，调用方据此决定是否落盘刷新）。</returns>
        public static bool RegisterItemIntoGroups(SortedDictionary<string, List<string>> groups, string classifyName, string itemName)
        {
            bool modified = false;
            if (!groups.ContainsKey(classifyName))
            {
                groups.Add(classifyName, new List<string>());
                modified = true;
            }

            if (!groups[classifyName].Contains(itemName))
            {
                groups[classifyName].Add(itemName);
                modified = true;
            }

            return modified;
        }

        /// <summary>
        /// 从"分类 -> 键名列表"内存索引中移除指定键；分类清空时一并移除空分类
        /// </summary>
        /// <param name="groups">分类索引字典。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">键名。</param>
        public static void RemoveItemFromGroups(SortedDictionary<string, List<string>> groups, string classifyName, string itemName)
        {
            if (groups.ContainsKey(classifyName))
            {
                if (groups[classifyName].Contains(itemName))
                {
                    groups[classifyName].Remove(itemName);
                    if (groups[classifyName].Count == 0)
                    {
                        groups.Remove(classifyName);
                    }
                }
            }
        }

        /// <summary>
        /// 从 PlayerPrefs 重建"分类 -> 键名列表"内存索引
        /// </summary>
        /// <param name="groups">目标内存索引（方法内先清空）。</param>
        /// <param name="classifyListKey">分类列表在 PlayerPrefs 中的键名。</param>
        /// <param name="itemListKeyFormat">键名列表键名模板，形如 "Classify_{0}_ItemNameList"。</param>
        public static void LoadItemNameGroups(SortedDictionary<string, List<string>> groups, string classifyListKey, string itemListKeyFormat)
        {
            groups.Clear();

            // 读取并解密所有分类名称列表
            string classifyNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(classifyListKey,
                AESEncrypt.EncodeToBase64(string.Empty)));
            if (!string.IsNullOrEmpty(classifyNameListText))
            {
                string[] classifyNameList = classifyNameListText.Split(',');
                for (int classifyNameIndex = 0; classifyNameIndex < classifyNameList.Length; classifyNameIndex++)
                {
                    string classifyName = classifyNameList[classifyNameIndex];
                    groups.Add(classifyName, new List<string>());

                    // 读取并解密当前分类下的所有键名
                    string itemKey = string.Format(itemListKeyFormat, classifyName);
                    string itemNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(itemKey,
                        AESEncrypt.EncodeToBase64(string.Empty)));
                    if (!string.IsNullOrEmpty(itemNameListText))
                    {
                        string[] nameList = itemNameListText.Split(',');
                        groups[classifyName].AddRange(nameList);
                    }
                }
            }
        }

        /// <summary>
        /// 把当前所有分类名加密后写入 PlayerPrefs
        /// </summary>
        /// <param name="groups">分类索引字典。</param>
        /// <param name="classifyListKey">分类列表在 PlayerPrefs 中的键名。</param>
        public static void SaveClassifyNameList(SortedDictionary<string, List<string>> groups, string classifyListKey)
        {
            string classifyNameList = string.Empty;
            foreach (string classifyName in groups.Keys)
            {
                classifyNameList = string.IsNullOrEmpty(classifyNameList)
                    ? classifyName
                    : $"{classifyNameList},{classifyName}";
            }

            PlayerPrefs.SetString(classifyListKey, AESEncrypt.EncodeToBase64(classifyNameList));
        }

        /// <summary>
        /// 把指定分类下的键名列表加密后写入 PlayerPrefs；分类不存在时删除该键名列表
        /// </summary>
        /// <param name="groups">分类索引字典。</param>
        /// <param name="classifyNameForSetting">要刷新的分类名。</param>
        /// <param name="itemListKeyFormat">键名列表键名模板，形如 "Classify_{0}_ItemNameList"。</param>
        public static void SaveItemNameList(SortedDictionary<string, List<string>> groups, string classifyNameForSetting, string itemListKeyFormat)
        {
            if (!string.IsNullOrEmpty(classifyNameForSetting))
            {
                List<string> names = null;
                string key = string.Format(itemListKeyFormat, classifyNameForSetting);

                // 分类存在 -> 保存键名列表
                if (groups.TryGetValue(classifyNameForSetting, out names))
                {
                    string nameList = string.Empty;
                    foreach (string name in names)
                    {
                        nameList = string.IsNullOrEmpty(nameList) ? name : $"{nameList},{name}";
                    }

                    PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(nameList));
                }
                // 分类不存在 -> 删除该列表
                else
                {
                    if (PlayerPrefs.HasKey(key))
                    {
                        PlayerPrefs.DeleteKey(key);
                    }
                }
            }
        }

        #endregion
    }
}
