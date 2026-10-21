/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlayerPrefsManager.Methods.cs
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
        private void RebuildIndexFromStorage()
        {
            CommonUtility.LoadItemNameGroups(m_NameIndex, "ClassifyNameList", "Classify_{0}_ItemNameList");
        }

        /// <summary>
        /// 将所有分类名称加密后保存到本地
        /// 用于新增/删除分类后同步索引
        /// </summary>
        private void PersistClassifyIndex()
        {
            CommonUtility.SaveClassifyNameList(m_NameIndex, "ClassifyNameList");
        }

        /// <summary>
        /// 将指定分类下的所有键名加密后保存到本地
        /// 用于新增/删除键后同步索引
        /// </summary>
        /// <param name="classifyNameForSetting">要刷新的分类名</param>
        private void PersistItemIndex(string classifyNameForSetting)
        {
            CommonUtility.SaveItemNameList(m_NameIndex, classifyNameForSetting, "Classify_{0}_ItemNameList");
        }

        #endregion
    }
}