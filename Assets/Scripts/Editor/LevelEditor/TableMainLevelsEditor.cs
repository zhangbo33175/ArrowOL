/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: TableMainLevelsEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 主关卡数据表编辑类
***************************************************************/
namespace Editor.MapEditor
{
//=========================================================================
// 主关卡数据表编辑类
// 存储关卡配置、类型、背景、目标物品等核心数据
//=========================================================================
    /// <summary>
    /// 地图编辑器主关卡数据表编辑类
    /// </summary>
    public class TableMainLevelsEditor
    {
        #region 关卡配置字段
        /// <summary>
        /// 所属章节 ID
        /// </summary>
        public string ID;
        /// <summary>
        /// 关卡唯一 ID
        /// </summary>
        public string LevelID;
        /// <summary>
        /// 寻物玩法类型 1 = 填色 2 = 消除
        /// </summary>
        public int FindType;
        /// <summary>
        /// 关卡背景图资源
        /// </summary>
        public string Level_bg;
        /// <summary>
        /// 本关卡需要寻找的物品 ID 集合
        /// </summary>
        public string CatItemID;
        #endregion
    }
}