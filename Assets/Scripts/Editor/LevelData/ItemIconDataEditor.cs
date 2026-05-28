/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: ItemIconDataEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 物品图标数据编辑类
***************************************************************/
namespace Editor.MapEditor
{
//=========================================================================
// 物品图标数据编辑类
// 用于管理地图编辑器内物品图标的 ID、资源路径、名称、选中状态、类型等数据
//=========================================================================
    /// <summary>
    /// 地图编辑器物品图标数据编辑类
    /// </summary>
    public class ItemIconDataEditor
    {
        #region 图标数据字段
        /// <summary>
        /// 图片唯一标识 ID
        /// </summary>
        public int m_ID;
        /// <summary>
        /// 图标资源文件路径
        /// </summary>
        public string m_IconAssetPath;
        /// <summary>
        /// 当前是否处于选中状态
        /// </summary>
        public bool m_IsChoose;
        /// <summary>
        /// 图标显示名称
        /// </summary>
        public string m_IconName;
        /// <summary>
        /// 图标对应的物体类型（默认值：消散类型）
        /// </summary>
        public RMapIconType m_Type = RMapIconType.Dissipate;
        #endregion
    }
}