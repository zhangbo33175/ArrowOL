/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: TableLevelEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 关卡数据表编辑类（每关一张地图，取消分区）
***************************************************************/
namespace Editor.MapEditor
{
//=========================================================================
// 关卡数据表编辑类
// 每个关卡对应一张独立地图，不再划分分区
//=========================================================================
    /// <summary>
    /// 地图编辑器 - 关卡配置数据表编辑类
    /// </summary>
    public class TableLevelEditor
    {
        #region 关卡配置数据字段
        /// <summary>
        /// 关卡唯一标识 ID
        /// </summary>
        public string ID = string.Empty;
        /// <summary>
        /// 所属章节 ID
        /// </summary>
        public string ChapterID = string.Empty;
        /// <summary>
        /// 关卡显示名称
        /// </summary>
        public string LevelName = string.Empty;
        /// <summary>
        /// 关卡背景图资源名
        /// </summary>
        public string LevelBg = string.Empty;
        /// <summary>
        /// 地图宽度（像素）
        /// </summary>
        public int MapWidth = 2048;
        /// <summary>
        /// 地图高度（像素）
        /// </summary>
        public int MapHeight = 2048;
        /// <summary>
        /// 寻物玩法类型 1 = 填色 2 = 消除
        /// </summary>
        public int FindType = 0;
        /// <summary>
        /// 本关卡需要寻找的物品 ID 集合
        /// </summary>
        public string CatItemID = string.Empty;
        #endregion
    }
}
