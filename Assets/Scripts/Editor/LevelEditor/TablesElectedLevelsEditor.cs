/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TablesElectedLevelsEditor.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 选中关卡数据编辑类
 ***************************************************************/

using System.Collections.Generic;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 - 已选关卡数据编辑类
    /// 存储当前在地图编辑器中选中的章节 / 关卡配置及保存路径
    /// </summary>
    public class TablesElectedLevelsEditor
    {
        #region 章节配置数据字段
        /// <summary>
        /// 章节唯一标识 ID
        /// </summary>
        public string ChapterId = string.Empty;
        /// <summary>
        /// 关卡唯一标识 ID
        /// </summary>
        public string LevelId = string.Empty;
        
        /// <summary>
        /// 地图标识 ID
        /// </summary>
        public int MapId = 0;
        /// <summary>
        /// 章节显示标题名称
        /// </summary>
        public string ChapterTitle;
        /// <summary>
        /// 章节关联地图资源名称
        /// </summary>
        public string MapName;

        /// <summary>
        /// 关卡背景图资源路径
        /// </summary>
        public string Background;

        /// <summary>
        /// 关卡创建时间字符串
        /// </summary>
        public string Time;

        /// <summary>
        /// 自定义保存子路径（相对关卡根目录，可为空）
        /// </summary>
        public string SavePath;

        #endregion
    }
}