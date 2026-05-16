/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlayerPrefsManager.cs
 * author:    云毅
 * created:
 * descrip:   加密 PlayerPrefs 管理器 - 全平台通用、AES加密、分类管理
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// PlayerPrefs 加密存储管理器
    /// 基于 Unity PlayerPrefs 实现，带 AES 加密、分类管理
    /// 轻量级、全平台通用（含 WebGL）
    /// </summary>
    public sealed partial class PlayerPrefsManager
    {
        #region 构造 & 生命周期

        public PlayerPrefsManager()
        {
            m_ItemNameGroups = new SortedDictionary<string, List<string>>();
        }

        /// <summary>
        /// 加载所有分类与键名索引（从 PlayerPrefs 读取）
        /// </summary>
        public bool Load()
        {
            LoadItemNameGroups();
            return true;
        }

        /// <summary>
        /// 立即保存到本地磁盘
        /// </summary>
        public bool Save()
        {
            PlayerPrefs.Save();
            return true;
        }

        #endregion

        #region 数据查询

        /// <summary>
        /// 获取指定分类下所有键名（数组）
        /// </summary>
        public string[] GetAllItemNames(string classifyName)
        {
            List<string> range = null;
            if (m_ItemNameGroups.TryGetValue(classifyName, out range))
            {
                return range.ToArray();
            }

            return null;
        }

        /// <summary>
        /// 获取指定分类下所有键名（列表）
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="results">条目名称集合。</param>
        public void GetAllItemNames(string classifyName, List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results 无效。");
            }

            results.Clear();

            List<string> range = null;
            if (m_ItemNameGroups.TryGetValue(classifyName, out range))
            {
                results = range;
            }

            return;
        }

        /// <summary>
        /// 判断指定分类下是否存在某键
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">要检查条目的名称。</param>
        /// <returns>指定的条目是否存在。</returns>
        public bool HasItem(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return PlayerPrefs.HasKey(key);
        }

        #endregion

        #region 删除操作

        /// <summary>
        /// 删除指定键（同步删除内存索引并刷新保存）
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">要移除条目的名称。</param>
        /// <returns>是否移除指定条目成功。</returns>
        public bool RemoveItem(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            if (!PlayerPrefs.HasKey(key))
            {
                return false;
            }

            PlayerPrefs.DeleteKey(key);

            if (m_ItemNameGroups.ContainsKey(classifyName))
            {
                if (m_ItemNameGroups[classifyName].Contains(itemName))
                {
                    m_ItemNameGroups[classifyName].Remove(itemName);
                    if (m_ItemNameGroups[classifyName].Count == 0)
                    {
                        m_ItemNameGroups.Remove(classifyName);
                    }
                }
            }

            RefreshItemNameListToSave(classifyName);
            RefreshClassifyNameListToSave();

            return true;
        }

        /// <summary>
        /// 清空数据
        /// classifyName = null 清空全部；否则清空指定分类
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        public void RemoveAllItems(string classifyName)
        {
            if (string.IsNullOrEmpty(classifyName))
            {
                PlayerPrefs.DeleteAll();
                m_ItemNameGroups.Clear();
                RefreshItemNameListToSave(null);
                RefreshClassifyNameListToSave();
            }
            else
            {
                List<string> range = null;
                if (m_ItemNameGroups.TryGetValue(classifyName, out range))
                {
                    List<string> names = new List<string>();
                    foreach (var name in range)
                    {
                        string key = $"{classifyName}_{name}";
                        PlayerPrefs.DeleteKey(key);
                        names.Add(name);
                    }

                    names.ForEach((name) => { m_ItemNameGroups[classifyName].Remove(name); });

                    if (m_ItemNameGroups[classifyName].Count == 0)
                    {
                        m_ItemNameGroups.Remove(classifyName);
                    }

                    RefreshItemNameListToSave(classifyName);
                    RefreshClassifyNameListToSave();
                }
            }
        }

        #endregion

        #region Bool 存取

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        public bool GetBool(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return bool.Parse(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key)));
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        public bool GetBool(string classifyName, string itemName, bool defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return bool.Parse(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key,
                AESEncrypt.EncodeToBase64(defaultValue.ToString()))));
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        public void SetBool(string classifyName, string itemName, bool value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(value.ToString()));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }

            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }
        }

        #endregion

        #region Int 存取

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        public int GetInt(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return int.Parse(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key)));
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        public int GetInt(string classifyName, string itemName, int defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return int.Parse(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key,
                AESEncrypt.EncodeToBase64(defaultValue.ToString()))));
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        public void SetInt(string classifyName, string itemName, int value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(value.ToString()));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }

            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }
        }

        #endregion

        #region Float 存取

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        public float GetFloat(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return float.Parse(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key)));
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        public float GetFloat(string classifyName, string itemName, float defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return float.Parse(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key,
                AESEncrypt.EncodeToBase64(defaultValue.ToString()))));
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        public void SetFloat(string classifyName, string itemName, float value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(value.ToString()));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }

            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }
        }

        #endregion

        #region String 存取

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        public string GetString(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key));
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        public string GetString(string classifyName, string itemName, string defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, AESEncrypt.EncodeToBase64(defaultValue)));
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        public void SetString(string classifyName, string itemName, string value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(value));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }

            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }
        }

        #endregion

        #region 调试方法

        /// <summary>
        /// 调试：打印所有分类与键名索引
        /// </summary>
        public void PrintAllNameLists()
        {
            string classifyAllNameList = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameList"));
            Log.Info($"ClassifyNameList = {classifyAllNameList}");
            if (!string.IsNullOrEmpty(classifyAllNameList))
            {
                string[] classifyNameList = classifyAllNameList.Split(',');
                for (int classifyNameIndex = 0; classifyNameIndex < classifyNameList.Length; classifyNameIndex++)
                {
                    string classifyName = classifyNameList[classifyNameIndex];
                    string classifyXXXXXNameListText = AESEncrypt.DecodeFromBase64(
                        PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameList",
                            AESEncrypt.EncodeToBase64(string.Empty)));
                    Log.Info($"Classify_{classifyName}_ItemNameList = {classifyXXXXXNameListText}");
                }
            }
        }

        #endregion
    }
}