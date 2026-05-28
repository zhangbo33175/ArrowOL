/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: UnlockLevelDataEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 关卡解锁数据编辑类
***************************************************************/
namespace Editor.MapEditor
{
//=========================================================================
// 关卡解锁数据编辑类
// 存储关卡解锁所需的章节 ID 与关卡 ID 配置数据
//=========================================================================
    /// <summary>
    /// 关卡解锁数据编辑类
    /// </summary>
    public class UnlockLevelDataEditor
    {
        #region 解锁配置字段
        /// <summary>
        /// 章节唯一 ID
        /// </summary>
        public int chapterId;
        /// <summary>
        /// 关卡唯一 ID
        /// </summary>
        public int levelId;
        #endregion
    }
}