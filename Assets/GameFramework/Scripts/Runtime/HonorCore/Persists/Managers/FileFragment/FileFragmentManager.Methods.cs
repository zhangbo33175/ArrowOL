/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentManager.Methods.cs
 * author:    云毅
 * created:   2026
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
        /// 尝试按分类名取出对应的数据分组（单次字典查找）
        /// </summary>
        /// <param name="fileFragmentName">分类名称</param>
        /// <param name="group">命中时输出的数据分组</param>
        /// <returns>存在该分组返回 true</returns>
        private bool TryGetGroup(string fileFragmentName, out FileFragmentItemGroup group)
        {
            return m_Groups.TryGetValue(fileFragmentName, out group);
        }

        /// <summary>
        /// 拼接分类对应的 .dat 文件绝对路径
        /// </summary>
        /// <param name="fileFragmentName">分类名称</param>
        /// <returns>完整文件路径</returns>
        private string PathCombine(string fileFragmentName)
        {
            return $"{m_RootDir}/{fileFragmentName}.dat";
        }

        /// <summary>
        /// 检查并创建数据容器（分类不存在时自动创建）
        /// 同时将该分类从待删除列表中移除
        /// </summary>
        /// <param name="fileFragmentName">分类名称</param>
        private void CheckAddContainer(string fileFragmentName)
        {
            if (!m_Groups.ContainsKey(fileFragmentName))
            {
                m_Groups.Add(fileFragmentName, new FileFragmentItemGroup());
                m_FileFullPaths.Add(PathCombine(fileFragmentName));
                m_GroupNames.Add(fileFragmentName);
            }

            // 重新使用则取消删除标记
            m_PendingDeleteNames.Remove(fileFragmentName);
        }

        /// <summary>
        /// 检查并删除空数据容器
        /// 分类为空时标记为待删除，Save 时统一删除文件
        /// </summary>
        /// <param name="fileFragmentName">分类名称</param>
        private void CheckRemoveContainer(string fileFragmentName)
        {
            if (!TryGetGroup(fileFragmentName, out FileFragmentItemGroup group))
            {
                return;
            }

            // 分类为空则移除
            if (group.Count == 0)
            {
                string fullPath = PathCombine(fileFragmentName);
                m_Groups.Remove(fileFragmentName);
                m_FileFullPaths.Remove(fullPath);
                m_GroupNames.Remove(fileFragmentName);

                // 加入待删除列表，等待 Save 时删除文件
                if (!m_PendingDeleteNames.Contains(fileFragmentName))
                {
                    m_PendingDeleteNames.Add(fileFragmentName);
                }
            }
        }

        #endregion
    }
}
