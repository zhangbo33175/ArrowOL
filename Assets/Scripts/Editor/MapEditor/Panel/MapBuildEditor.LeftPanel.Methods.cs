namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        /// <summary>
        /// 设置Icon的预制体
        /// </summary>
        public void SetIconItem()
        {
            int[] _ItemList= Utils.GetIntList(m_TableMainLevels.CatItemID);  
            for (int i = 0; i < _ItemList.Length; i++)
            {
                for (int j = 0; j < m_TableItemsList.Count; j++)
                {
                    if (_ItemList[i]==m_TableItemsList[j].ID)
                    {
                        m_IconList.Add(new IconData
                        {
                            m_IconName = m_TableItemsList[j].Icon, // 不含后缀的文件名
                            m_IconAssetPath =m_TableItemsList[j].Icon,
                            m_IconTexture = null,
                            m_IocnIsLoaded = false // 默认未选中
                        });
                    }
                }
            }  
        }

    }
}