/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentForWebGLManager.Methods.cs
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
        private void RebuildIndexFromStorage()
        {
            CommonUtility.LoadItemNameGroups(m_NameIndex, "ClassifyNameListForWebGL", "Classify_{0}_ItemNameListForWebGL");
        }

        /// <summary>
        /// 将当前所有分类名称刷新并保存到本地
        /// 用于新增/删除分类后同步索引
        /// </summary>
        private void PersistClassifyIndex()
        {
            CommonUtility.SaveClassifyNameList(m_NameIndex, "ClassifyNameListForWebGL");
        }

        /// <summary>
        /// 刷新指定分类下的所有键名并保存到本地
        /// 用于新增/删除键后同步索引
        /// </summary>
        /// <param name="classifyNameForSetting">要刷新的分类名</param>
        private void PersistItemIndex(string classifyNameForSetting)
        {
            CommonUtility.SaveItemNameList(m_NameIndex, classifyNameForSetting, "Classify_{0}_ItemNameListForWebGL");
        }

        #endregion
    }
}