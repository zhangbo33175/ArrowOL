/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentForWebGLManager.Implement.cs
 * author:    云毅
 * created:   2026
 * descrip:   WebGL 存储管理器 - 内部索引实现（加载/刷新分类与键名）
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// WebGL 专用文件片段存储管理器 - 内部方法实现
    /// </summary>
    public sealed partial class FileFragmentForWebGLManager
    {
        //=========================================================================
        #region 内部索引管理（加载 / 刷新 / 保存）
        //=========================================================================

        /// <summary>
        /// 从本地存储中加载所有分类与键名索引（内存索引重建）
        /// 读取分类列表 → 读取每个分类下的键名列表 → 构建内存结构
        /// </summary>
        private void LoadItemNameGroups()
        {
            m_ItemNameGroups.Clear();

            // 读取所有分类名称（加密存储）
            string classifyNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameListForWebGL", AESEncrypt.EncodeToBase64(string.Empty)));
            if (!string.IsNullOrEmpty(classifyNameListText))
            {
                string[] classifyNameList = classifyNameListText.Split(',');
                for (int classifyNameIndex = 0; classifyNameIndex < classifyNameList.Length; classifyNameIndex++)
                {
                    string classifyName = classifyNameList[classifyNameIndex];
                    m_ItemNameGroups.Add(classifyName, new List<string>());

                    // 读取当前分类下的所有键名
                    string classifyXXXXXItemNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameListForWebGL", AESEncrypt.EncodeToBase64(string.Empty)));
                    if (!string.IsNullOrEmpty(classifyXXXXXItemNameListText))
                    {
                        string[] nameList = classifyXXXXXItemNameListText.Split(',');
                        m_ItemNameGroups[classifyName].AddRange(nameList);
                    }
                }
            }
        }

        /// <summary>
        /// 将当前所有分类名称刷新并保存到本地
        /// 用于新增/删除分类后同步索引
        /// </summary>
        private void RefreshClassifyNameListToSave()
        {
            string classifyNameList = string.Empty;
            foreach (string classifyName in m_ItemNameGroups.Keys)
            {
                classifyNameList = string.IsNullOrEmpty(classifyNameList) ? classifyName : $"{classifyNameList},{classifyName}";
            }
            // 加密后保存
            PlayerPrefs.SetString("ClassifyNameListForWebGL", AESEncrypt.EncodeToBase64(classifyNameList));
        }

        /// <summary>
        /// 刷新指定分类下的所有键名并保存到本地
        /// 用于新增/删除键后同步索引
        /// </summary>
        /// <param name="classifyNameForSetting">要刷新的分类名</param>
        private void RefreshItemNameListToSave(string classifyNameForSetting)
        {
            if (!string.IsNullOrEmpty(classifyNameForSetting))
            {
                List<string> names = null;
                string key = $"Classify_{classifyNameForSetting}_ItemNameListForWebGL";

                // 存在则保存键名列表
                if (m_ItemNameGroups.TryGetValue(classifyNameForSetting, out names))
                {
                    string nameList = string.Empty;
                    foreach (string name in names)
                    {
                        nameList = string.IsNullOrEmpty(nameList) ? name : $"{nameList},{name}";
                    }
                    PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(nameList));
                }
                // 不存在则删除该分类的键名列表
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