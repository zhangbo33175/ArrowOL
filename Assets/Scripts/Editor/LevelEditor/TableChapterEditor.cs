/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: TableChapterEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 章节数据表编辑类
***************************************************************/
namespace Editor.MapEditor
{
//=========================================================================
// 章节数据表编辑类
// 用于编辑和存储章节配置的核心数据信息
//=========================================================================
    /// <summary>
    /// 地图编辑器 - 章节配置数据表编辑类
    /// </summary>
    public class TableChapterEditor
    {
        #region 章节配置数据字段
        /// <summary>
        /// 章节唯一标识 ID
        /// </summary>
        public string ID = string.Empty;
        /// <summary>
        /// 章节显示标题名称
        /// </summary>
        public string ChapterTitle;
        /// <summary>
        /// 章节信息配置 ID
        /// </summary>
        public string ChapterInfoID;
        /// <summary>
        /// 章节关联地图资源名称
        /// </summary>
        public string MapName;
        #endregion
    }
}