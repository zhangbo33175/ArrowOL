/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   文件片段管理器 - 字段、属性与计数接口
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 文件片段存储管理器 - 字段与属性模块
    /// </summary>
    public sealed partial class FileFragmentManager
    {
        //=========================================================================
        #region 字段 & 属性
        //=========================================================================

        /// <summary>
        /// 文件片段根目录绝对路径
        /// </summary>
        private string m_RootDir = null;

        /// <summary>
        /// 所有文件片段名称列表（纯名称，不带后缀）
        /// </summary>
        private readonly List<string> m_GroupNames = null;

        /// <summary>
        /// 获取所有文件片段名称列表（纯名称，不带后缀）
        /// </summary>
        public List<string> FileFragmentNames
        {
            get { return m_GroupNames; }
        }

        /// <summary>
        /// 待删除的文件片段名称列表（Save 时统一删除）
        /// </summary>
        private readonly List<string> m_PendingDeleteNames = null;

        /// <summary>
        /// 获取待删除的文件片段名称列表
        /// </summary>
        public List<string> FileFragmentNamesForDelete
        {
            get { return m_PendingDeleteNames; }
        }

        /// <summary>
        /// 所有文件片段的完整物理路径
        /// </summary>
        private readonly List<string> m_FileFullPaths = null;

        /// <summary>
        /// 获取所有文件片段的完整物理路径列表
        /// </summary>
        public List<string> FilePaths
        {
            get { return m_FileFullPaths; }
        }

        /// <summary>
        /// 文件片段数据分组字典
        /// Key：文件片段名称
        /// Value：对应的数据项集合
        /// </summary>
        private readonly SortedDictionary<string, FileFragmentItemGroup> m_Groups = null;

        /// <summary>
        /// 获取文件片段数据分组字典
        /// </summary>
        public SortedDictionary<string, FileFragmentItemGroup> ItemGroups
        {
            get { return m_Groups; }
        }

        #endregion

        //=========================================================================
        #region 公共方法
        //=========================================================================

        /// <summary>
        /// 获取指定文件片段内的数据条目数量
        /// </summary>
        /// <param name="fileFragmentName">文件片段名称</param>
        /// <returns>数据条数</returns>
        public int Count(string fileFragmentName)
        {
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group) && group != null
                ? group.Count
                : 0;
        }

        #endregion
    }
}
