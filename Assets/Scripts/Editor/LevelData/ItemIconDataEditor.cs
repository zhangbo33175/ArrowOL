namespace Editor.MapEditor
{
    public class ItemIconDataEditor
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public int m_ID;

        /// <summary>
        /// 图标Icon
        /// </summary>
        public string m_IconAssetPath;
        
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool m_IsChoose;
        
        /// <summary>
        /// 是否选中
        /// </summary>
        public string m_IconName;
        
        /// <summary>
        /// 物体类型
        /// </summary>
        public RMapIconType m_Type = RMapIconType.Dissipate;
    }
}