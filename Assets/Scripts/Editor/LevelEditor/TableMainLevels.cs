namespace Editor.MapEditor
{
    public class TableMainLevels
    {
        /// <summary>
        /// 章节Id
        /// </summary>
        public string ID;

        /// <summary>
        /// 关卡名
        /// </summary>
        public string LevelID;

        /// <summary>
        /// 寻物类型 1填色 2消除
        /// </summary>
        public int FindType;
        
        /// <summary>
        /// 关卡背景图
        /// </summary>
        public string Level_bg;
        
        /// <summary>
        /// 寻找物的id集合
        /// </summary>
        public string CatItemID;
    }
}