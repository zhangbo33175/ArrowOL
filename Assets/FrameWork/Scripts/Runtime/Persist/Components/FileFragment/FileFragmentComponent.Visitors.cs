using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class FileFragmentComponent
    {
        /// <summary>
        /// 文件片段根目录绝对路径
        /// </summary>
        private string m_FileFragmentsRootDirectoryFullPath = null;

        /// <summary>
        /// 所有文件片段的名称
        /// 纯文件名称(不带后缀)
        /// </summary>
        private readonly List<string> m_FileFragmentNames = null;
        public List<string> FileFragmentNames
        {
            get
            {
                return m_FileFragmentNames;
            }
        }

        /// <summary>
        /// 所有待删除的文件片段的名称
        /// 纯文件名称(不带后缀)
        /// </summary>
        private readonly List<string> m_FileFragmentNamesForDelete = null;
        public List<string> FileFragmentNamesForDelete
        {
            get
            {
                return m_FileFragmentNamesForDelete;
            }
        }

        /// <summary>
        /// 所有文件片段在读写区的文件路径
        /// </summary>
        private readonly List<string> m_FilePaths = null;
        public List<string> FilePaths
        {
            get
            {
                return m_FilePaths;
            }
        }

        /// <summary>
        /// 所有文件片段的条目信息集合
        /// <文件片段名称，文件片段中Item集合>
        /// </summary>
        private readonly SortedDictionary<string, FileFragmentItemGroup> m_ItemGroups = null;
        public SortedDictionary<string, FileFragmentItemGroup> ItemGroups
        {
            get
            {
                return m_ItemGroups;
            }
        }

        /// <summary>
        /// 获取指定文件片段的条目数量。
        /// </summary>
        /// <param name="fileFragmentName">文件片段名称</param>
        /// <returns></returns>
        public int Count(string fileFragmentName)
        {
            return (m_ItemGroups != null && m_ItemGroups.ContainsKey(fileFragmentName) && m_ItemGroups[fileFragmentName] != null) ? m_ItemGroups[fileFragmentName].Count : 0;
        }
    }
}