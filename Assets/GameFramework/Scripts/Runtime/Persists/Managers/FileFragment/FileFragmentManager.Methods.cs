namespace Honor.Runtime
{
    public sealed partial class FileFragmentManager
    {
        /// <summary>
        /// 检查容器新增
        /// </summary>
        private void CheckAddContainer(string fileFragmentName)
        {
            if (!m_ItemGroups.ContainsKey(fileFragmentName))
            {
                m_ItemGroups.Add(fileFragmentName, new FileFragmentItemGroup());
                m_FilePaths.Add($"{m_FileFragmentsRootDirectoryFullPath}/{fileFragmentName}.dat");
                m_FileFragmentNames.Add(fileFragmentName);
            }
            if (m_FileFragmentNamesForDelete.Contains(fileFragmentName))
            {
                m_FileFragmentNamesForDelete.Remove(fileFragmentName);
            }
        }

        /// <summary>
        /// 检查容器删减
        /// </summary>
        private void CheckRemoveContainer(string fileFragmentName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                if (m_ItemGroups[fileFragmentName].Count == 0)
                {
                    string fullPath = $"{m_FileFragmentsRootDirectoryFullPath}/{fileFragmentName}.dat";
                    m_ItemGroups.Remove(fileFragmentName);
                    m_FilePaths.Remove(fullPath);
                    m_FileFragmentNames.Remove(fileFragmentName);
                    if (!m_FileFragmentNamesForDelete.Contains(fileFragmentName))
                    {
                        m_FileFragmentNamesForDelete.Add(fileFragmentName);
                    }
                }
            }

        }

    }

}


