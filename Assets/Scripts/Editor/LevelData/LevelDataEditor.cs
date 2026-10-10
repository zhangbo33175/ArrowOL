/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: LevelDataEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 关卡数据编辑类
***************************************************************/
using Honor.Editor;
using Honor.Runtime;
namespace Editor.MapEditor
{
//=========================================================================
// 关卡数据编辑类
// 存储地图编辑器中章节与关卡的配置数据
//=========================================================================
    /// <summary>
    /// 地图编辑器关卡数据编辑类
    /// </summary>
    public class LevelDataEditor
    {
        #region 关卡配置数据
        /// <summary>
        /// 章节唯一标识 ID
        /// </summary>
        public string ChapterId = string.Empty;
        /// <summary>
        /// 关卡唯一标识 ID
        /// </summary>
        public int LevelId;
        /// <summary>
        /// 关卡类型
        /// </summary>
        public string Type;
        /// <summary>
        /// 章节显示名称
        /// </summary>
        public string ChapterName;
        /// <summary>
        /// 背景资源 / 背景配置
        /// </summary>
        public string Backgrond;
        /// <summary>
        /// 分类物品 ID / 关联道具配置 ID
        /// </summary>
        public string CatItemID;

        /// <summary>
        /// 寻找物的 id 集合（挑战表 TableDailyLevels 的 LevelItems 列，数组字符串，如 {1001,1002,...}）
        /// 用于挑战关卡地图编辑时加载可放置/寻物目标图标
        /// </summary>
        public string LevelItems = string.Empty;

        /// <summary>
        /// 挑战关卡背景图路径（挑战表 TableDailyLevels 的 Level_bg 列，与运行时 Level_bg 字段同名以保证按列名映射）
        /// </summary>
        public string Level_bg = string.Empty;
        #endregion
    }
}