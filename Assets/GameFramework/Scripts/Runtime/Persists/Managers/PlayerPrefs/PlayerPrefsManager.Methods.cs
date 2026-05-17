/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlayerPrefsManager.Implement.cs
 * author:    云毅
 * created:   2026
 * descrip:   加密 PlayerPrefs 管理器 - 索引加载与刷新（内部实现）
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 加密 PlayerPrefs 管理器 - 内部索引方法实现
    /// </summary>
    public sealed partial class PlayerPrefsManager
    {
        //=========================================================================

        #region 内部索引管理（加载 / 刷新 / 保存分类与键名）

        //=========================================================================

        /// <summary>
        /// 从本地存储中加载【分类列表 + 各分类下的键名列表】
        /// 用于启动时重建内存索引
        /// </summary>
        private void LoadItemNameGroups()
        {
            m_ItemNameGroups.Clear();

            // 读取并解密所有分类名称列表
            string classifyNameListText =
                AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameList",
                    AESEncrypt.EncodeToBase64(string.Empty)));
            if (!string.IsNullOrEmpty(classifyNameListText))
            {
                string[] classifyNameList = classifyNameListText.Split(',');
                for (int classifyNameIndex = 0; classifyNameIndex < classifyNameList.Length; classifyNameIndex++)
                {
                    string classifyName = classifyNameList[classifyNameIndex];
                    m_ItemNameGroups.Add(classifyName, new List<string>());

                    // 读取并解密当前分类下的所有键名
                    string classifyXXXXXItemNameListText = AESEncrypt.DecodeFromBase64(
                        PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameList",
                            AESEncrypt.EncodeToBase64(string.Empty)));
                    if (!string.IsNullOrEmpty(classifyXXXXXItemNameListText))
                    {
                        string[] nameList = classifyXXXXXItemNameListText.Split(',');
                        m_ItemNameGroups[classifyName].AddRange(nameList);
                    }
                }
            }
        }

        /// <summary>
        /// 将所有分类名称加密后保存到本地
        /// 用于新增/删除分类后同步索引
        /// </summary>
        private void RefreshClassifyNameListToSave()
        {
            string classifyNameList = string.Empty;
            foreach (string classifyName in m_ItemNameGroups.Keys)
            {
                classifyNameList = string.IsNullOrEmpty(classifyNameList)
                    ? classifyName
                    : $"{classifyNameList},{classifyName}";
            }

            PlayerPrefs.SetString("ClassifyNameList", AESEncrypt.EncodeToBase64(classifyNameList));
        }

        /// <summary>
        /// 将指定分类下的所有键名加密后保存到本地
        /// 用于新增/删除键后同步索引
        /// </summary>
        /// <param name="classifyNameForSetting">要刷新的分类名</param>
        private void RefreshItemNameListToSave(string classifyNameForSetting)
        {
            if (!string.IsNullOrEmpty(classifyNameForSetting))
            {
                List<string> names = null;
                string key = $"Classify_{classifyNameForSetting}_ItemNameList";

                // 分类存在 → 保存键名列表
                if (m_ItemNameGroups.TryGetValue(classifyNameForSetting, out names))
                {
                    string nameList = string.Empty;
                    foreach (string name in names)
                    {
                        nameList = string.IsNullOrEmpty(nameList) ? name : $"{nameList},{name}";
                    }

                    PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(nameList));
                }
                // 分类不存在 → 删除该列表
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