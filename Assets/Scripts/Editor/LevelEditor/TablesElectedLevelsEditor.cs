using System.Collections.Generic;

namespace Editor.MapEditor
{
    public class TablesElectedLevelsEditor
    {
        #region 章节配置数据字段
        /// <summary>
        /// 章节唯一标识 ID
        /// </summary>
        public string ChapterId = string.Empty;
        /// <summary>
        /// 章节唯一标识 ID
        /// </summary>
        public string LevelId = string.Empty;
        /// <summary>
        /// 章节显示标题名称
        /// </summary>
        public string ChapterTitle;
        /// <summary>
        /// 章节关联地图资源名称
        /// </summary>
        public string MapName;
        
        public string Background;
        
        public string Time;
        
        public string SavePath;
        
        #endregion 
    }
}