/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentManager.Implement.cs
 * author:    云毅
 * created:
 * descrip:   文件片段管理器 - 内部容器管理（创建/删除分类）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 文件片段存储管理器 - 内部方法实现
    /// </summary>
    public sealed partial class FileFragmentManager
    {
        //=========================================================================
        #region 内部容器管理（创建 / 删除分类）
        //=========================================================================

        /// <summary>
        /// 检查并创建数据容器（分类不存在时自动创建）
        /// 同时将该分类从待删除列表中移除
        /// </summary>
        /// <param name="fileFragmentName">分类名称</param>
        private void CheckAddContainer(string fileFragmentName)
        {
            if (!m_ItemGroups.ContainsKey(fileFragmentName))
            {
                m_ItemGroups.Add(fileFragmentName, new FileFragmentItemGroup());
                m_FilePaths.Add($"{m_FileFragmentsRootDirectoryFullPath}/{fileFragmentName}.dat");
                m_FileFragmentNames.Add(fileFragmentName);
            }

            // 重新使用则取消删除标记
            if (m_FileFragmentNamesForDelete.Contains(fileFragmentName))
            {
                m_FileFragmentNamesForDelete.Remove(fileFragmentName);
            }
        }

        /// <summary>
        /// 检查并删除空数据容器
        /// 分类为空时标记为待删除，Save 时统一删除文件
        /// </summary>
        /// <param name="fileFragmentName">分类名称</param>
        private void CheckRemoveContainer(string fileFragmentName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                // 分类为空则移除
                if (m_ItemGroups[fileFragmentName].Count == 0)
                {
                    string fullPath = $"{m_FileFragmentsRootDirectoryFullPath}/{fileFragmentName}.dat";
                    m_ItemGroups.Remove(fileFragmentName);
                    m_FilePaths.Remove(fullPath);
                    m_FileFragmentNames.Remove(fileFragmentName);

                    // 加入待删除列表，等待 Save 时删除文件
                    if (!m_FileFragmentNamesForDelete.Contains(fileFragmentName))
                    {
                        m_FileFragmentNamesForDelete.Add(fileFragmentName);
                    }
                }
            }
        }

        #endregion
    }
}