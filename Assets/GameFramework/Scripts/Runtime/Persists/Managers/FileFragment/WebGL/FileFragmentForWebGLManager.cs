/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentForWebGLManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   WebGL 专用持久化存储管理器（PlayerPrefs + AES + GZip）
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// WebGL 专用文件片段存储管理器
    /// 基于 PlayerPrefs 实现，支持 AES 加密 + GZip 压缩
    /// 提供分类管理、键值存储、增删改查
    /// </summary>
    public sealed partial class FileFragmentForWebGLManager
    {
        #region 构造 & 生命周期
        public FileFragmentForWebGLManager()
        {
            m_NameIndex = new SortedDictionary<string, List<string>>();
        }

        /// <summary>
        /// 加载所有条目索引（从 PlayerPrefs 读取分类与键名）
        /// </summary>
        /// <returns>固定返回 true</returns>
        public bool Load()
        {
            // 从存档中读取所有条目
            RebuildIndexFromStorage();
            return true;
        }

        /// <summary>
        /// 立即保存到本地（调用 PlayerPrefs.Save）
        /// </summary>
        /// <returns>固定返回 true</returns>
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
        /// 读取、解密并解压字符串
        /// </summary>
        private static string ReadPayload(string key)
        {
            return GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key)));
        }

        /// <summary>
        /// 读取、解密并解压字符串（带默认值）
        /// </summary>
        private static string ReadPayload(string key, string defaultPlain)
        {
            string encodedDefault = AESEncrypt.EncodeToBase64(GZip.CompressToBase64(defaultPlain));
            return GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, encodedDefault)));
        }

        /// <summary>
        /// 压缩、加密并写入一条字符串，同时维护内存索引
        /// </summary>
        private void WritePayload(string classifyName, string itemName, string plainValue)
        {
            string key = BuildKey(classifyName, itemName);
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(plainValue)));

            if (CommonUtility.RegisterItemIntoGroups(m_NameIndex, classifyName, itemName))
            {
                PersistItemIndex(classifyName);
                PersistClassifyIndex();
            }
        }

        /// <summary>
        /// 删除指定分类下的所有物理键并移除该分类索引
        /// </summary>
        private void ClearGroup(string classifyName)
        {
            if (!m_NameIndex.TryGetValue(classifyName, out List<string> namesInGroup))
            {
                return;
            }

            List<string> snapshot = new List<string>(namesInGroup);
            foreach (string name in snapshot)
            {
                PlayerPrefs.DeleteKey(BuildKey(classifyName, name));
            }

            m_NameIndex.Remove(classifyName);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 获取指定分类下所有键名（数组）
        /// </summary>
        /// <param name="classifyName">分类名</param>
        /// <returns>键名数组</returns>
        public string[] GetAllItemNames(string classifyName)
        {
            return CommonUtility.GetItemNamesByGroup(m_NameIndex, classifyName);
        }

        /// <summary>
        /// 获取指定分类下所有键名（列表）
        /// </summary>
        /// <param name="classifyName">分类名</param>
        /// <param name="results">输出结果列表</param>
        public void GetAllItemNames(string classifyName, List<string> results)
        {
            CommonUtility.AppendItemNamesByGroup(m_NameIndex, classifyName, results);
        }

        /// <summary>
        /// 判断是否存在某条数据
        /// </summary>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <returns>是否存在</returns>
        public bool HasItem(string classifyName, string itemName)
        {
            return CommonUtility.HasPlayerPrefsItem(classifyName, itemName);
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除单条数据（同步删除索引）
        /// </summary>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <returns>是否删除成功</returns>
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
        /// 清空数据（支持清空全部 / 清空指定分类）
        /// </summary>
        /// <param name="classifyName">分类名（null 则清空全部）</param>
        public void RemoveAllItems(string classifyName)
        {
            if (string.IsNullOrEmpty(classifyName))
            {
                // 逐个清空所有分类
                List<string> allClassifyNames = new List<string>(m_NameIndex.Keys);
                foreach (string name in allClassifyNames)
                {
                    ClearGroup(name);
                }

                m_NameIndex.Clear();
                PersistItemIndex(null);
                PersistClassifyIndex();
                return;
            }

            ClearGroup(classifyName);
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
            return bool.Parse(ReadPayload(BuildKey(classifyName, itemName)));
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        public bool GetBool(string classifyName, string itemName, bool defaultValue)
        {
            return bool.Parse(ReadPayload(BuildKey(classifyName, itemName), defaultValue.ToString()));
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        public void SetBool(string classifyName, string itemName, bool value)
        {
            WritePayload(classifyName, itemName, value.ToString());
        }
        #endregion

        #region Int 存取
        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        public int GetInt(string classifyName, string itemName)
        {
            return int.Parse(ReadPayload(BuildKey(classifyName, itemName)));
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        public int GetInt(string classifyName, string itemName, int defaultValue)
        {
            return int.Parse(ReadPayload(BuildKey(classifyName, itemName), defaultValue.ToString()));
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        public void SetInt(string classifyName, string itemName, int value)
        {
            WritePayload(classifyName, itemName, value.ToString());
        }
        #endregion

        #region Float 存取
        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        public float GetFloat(string classifyName, string itemName)
        {
            return float.Parse(ReadPayload(BuildKey(classifyName, itemName)));
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        public float GetFloat(string classifyName, string itemName, float defaultValue)
        {
            return float.Parse(ReadPayload(BuildKey(classifyName, itemName), defaultValue.ToString()));
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        public void SetFloat(string classifyName, string itemName, float value)
        {
            WritePayload(classifyName, itemName, value.ToString());
        }
        #endregion

        #region String 存取
        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        public string GetString(string classifyName, string itemName)
        {
            return ReadPayload(BuildKey(classifyName, itemName));
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        public string GetString(string classifyName, string itemName, string defaultValue)
        {
            return ReadPayload(BuildKey(classifyName, itemName), defaultValue);
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        public void SetString(string classifyName, string itemName, string value)
        {
            WritePayload(classifyName, itemName, value);
        }
        #endregion

        #region 调试方法
        /// <summary>
        /// 调试：打印所有分类与键名索引（方便排查存储问题）
        /// </summary>
        public void PrintAllNameLists()
        {
            string classifyAllNameList = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameListForWebGL"));
            Log.Info($"ClassifyNameListFoWebGL = {classifyAllNameList}");
            if (string.IsNullOrEmpty(classifyAllNameList))
            {
                return;
            }

            string[] classifyNameList = classifyAllNameList.Split(',');
            for (int slot = 0; slot < classifyNameList.Length; slot++)
            {
                string classifyName = classifyNameList[slot];
                string itemNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameListForWebGL", AESEncrypt.EncodeToBase64(string.Empty)));
                Log.Info($"Classify_{classifyName}_ItemNameListForWebGL = {itemNameListText}");
            }
        }
        #endregion
    }
}
