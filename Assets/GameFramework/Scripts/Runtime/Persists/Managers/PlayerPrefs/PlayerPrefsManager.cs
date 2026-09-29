/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlayerPrefsManager.cs
 * author:    云毅
 * created:   2026
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

        /// <summary>
        /// 构造方法：初始化内存索引字典
        /// </summary>
        public PlayerPrefsManager()
        {
            m_NameIndex = new SortedDictionary<string, List<string>>();
        }

        /// <summary>
        /// 加载所有分类与键名索引（从 PlayerPrefs 读取）
        /// </summary>
        public bool Load()
        {
            RebuildIndexFromStorage();
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

        #region 内部读写辅助

        /// <summary>
        /// 拼接分类+条目的 PlayerPrefs 物理键
        /// </summary>
        private static string BuildKey(string classifyName, string itemName)
        {
            return $"{classifyName}_{itemName}";
        }

        /// <summary>
        /// 读取并解密字符串（无默认值，缺失时由 PlayerPrefs 返回空串）
        /// </summary>
        private static string ReadEncrypted(string key)
        {
            return AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key));
        }

        /// <summary>
        /// 读取并解密字符串（带默认值，默认值先加密后入库）
        /// </summary>
        private static string ReadEncrypted(string key, string defaultValue)
        {
            return AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, AESEncrypt.EncodeToBase64(defaultValue)));
        }

        /// <summary>
        /// 写入一条加密字符串并维护内存索引
        /// 仅当索引发生变化时才刷新落盘列表
        /// </summary>
        private void WriteEntry(string classifyName, string itemName, string plainValue)
        {
            string key = BuildKey(classifyName, itemName);
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(plainValue));

            if (CommonUtility.RegisterItemIntoGroups(m_NameIndex, classifyName, itemName))
            {
                PersistItemIndex(classifyName);
                PersistClassifyIndex();
            }
        }

        #endregion

        #region 数据查询

        /// <summary>
        /// 获取指定分类下所有键名（数组）
        /// </summary>
        public string[] GetAllItemNames(string classifyName)
        {
            return CommonUtility.GetItemNamesByGroup(m_NameIndex, classifyName);
        }

        /// <summary>
        /// 获取指定分类下所有键名（列表）
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="results">条目名称集合。</param>
        public void GetAllItemNames(string classifyName, List<string> results)
        {
            CommonUtility.AppendItemNamesByGroup(m_NameIndex, classifyName, results);
        }

        /// <summary>
        /// 判断指定分类下是否存在某键
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">要检查条目的名称。</param>
        /// <returns>指定的条目是否存在。</returns>
        public bool HasItem(string classifyName, string itemName)
        {
            return CommonUtility.HasPlayerPrefsItem(classifyName, itemName);
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
            string key = BuildKey(classifyName, itemName);
            if (!PlayerPrefs.HasKey(key))
            {
                return false;
            }

            PlayerPrefs.DeleteKey(key);

            CommonUtility.RemoveItemFromGroups(m_NameIndex, classifyName, itemName);

            PersistItemIndex(classifyName);
            PersistClassifyIndex();

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
                m_NameIndex.Clear();
                PersistItemIndex(null);
                PersistClassifyIndex();
                return;
            }

            if (!m_NameIndex.TryGetValue(classifyName, out List<string> namesInGroup))
            {
                return;
            }

            // 快照后逐个删除物理键
            List<string> snapshot = new List<string>(namesInGroup);
            foreach (string name in snapshot)
            {
                PlayerPrefs.DeleteKey(BuildKey(classifyName, name));
            }

            m_NameIndex.Remove(classifyName);

            PersistItemIndex(classifyName);
            PersistClassifyIndex();
        }

        #endregion

        #region Bool 存取

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        public bool GetBool(string classifyName, string itemName)
        {
            return bool.Parse(ReadEncrypted(BuildKey(classifyName, itemName)));
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        public bool GetBool(string classifyName, string itemName, bool defaultValue)
        {
            return bool.Parse(ReadEncrypted(BuildKey(classifyName, itemName), defaultValue.ToString()));
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        public void SetBool(string classifyName, string itemName, bool value)
        {
            WriteEntry(classifyName, itemName, value.ToString());
        }

        #endregion

        #region Int 存取

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        public int GetInt(string classifyName, string itemName)
        {
            return int.Parse(ReadEncrypted(BuildKey(classifyName, itemName)));
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        public int GetInt(string classifyName, string itemName, int defaultValue)
        {
            return int.Parse(ReadEncrypted(BuildKey(classifyName, itemName), defaultValue.ToString()));
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        public void SetInt(string classifyName, string itemName, int value)
        {
            WriteEntry(classifyName, itemName, value.ToString());
        }

        #endregion

        #region Float 存取

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        public float GetFloat(string classifyName, string itemName)
        {
            return float.Parse(ReadEncrypted(BuildKey(classifyName, itemName)));
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        public float GetFloat(string classifyName, string itemName, float defaultValue)
        {
            return float.Parse(ReadEncrypted(BuildKey(classifyName, itemName), defaultValue.ToString()));
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        public void SetFloat(string classifyName, string itemName, float value)
        {
            WriteEntry(classifyName, itemName, value.ToString());
        }

        #endregion

        #region String 存取

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        public string GetString(string classifyName, string itemName)
        {
            return ReadEncrypted(BuildKey(classifyName, itemName));
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        public string GetString(string classifyName, string itemName, string defaultValue)
        {
            return ReadEncrypted(BuildKey(classifyName, itemName), defaultValue);
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        public void SetString(string classifyName, string itemName, string value)
        {
            WriteEntry(classifyName, itemName, value);
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
            if (string.IsNullOrEmpty(classifyAllNameList))
            {
                return;
            }

            string[] classifyNameList = classifyAllNameList.Split(',');
            for (int slot = 0; slot < classifyNameList.Length; slot++)
            {
                string classifyName = classifyNameList[slot];
                string itemNameListText = AESEncrypt.DecodeFromBase64(
                    PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameList",
                        AESEncrypt.EncodeToBase64(string.Empty)));
                Log.Info($"Classify_{classifyName}_ItemNameList = {itemNameListText}");
            }
        }

        #endregion
    }
}
