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
        #endregion
    }
}