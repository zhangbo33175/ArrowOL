/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TablesBridge.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置表桥接类
 *            映射捏脸、换装、颜色配置表，供编辑器 + 运行时使用
 ***************************************************************/

using System.Collections.Generic;

namespace GameLib
{
    /// <summary>
    /// 配置表桥接类
    /// 作用：将 Excel/Lua 中的【捏脸、换装、颜色配置】表映射为 C# 数据结构
    /// 供 编辑器 + 游戏运行时 使用
    /// </summary>
    public class TablesBridge
    {
        #region 自定义装扮/捏脸点位配置表（TableCustomize）
        //=========================================================================
        // 自定义装扮/捏脸点位配置表
        //=========================================================================
        /// <summary>
        /// 单个装扮/捏脸点位数据
        /// 对应表：TableCustomize
        /// </summary>
        public class TableCustomizeItem
        {
            /// <summary>
            /// 捏脸/换装点位唯一ID
            /// </summary>
            public string ID;

            /// <summary>
            /// 点位类型（如：Face、Hair、Clothes、Eyes 等）
            /// </summary>
            public string Type;

            /// <summary>
            /// 材质/皮肤插槽名称（对应模型的 Material 插槽）
            /// </summary>
            public string SlotName;
        }
        #endregion

        #region 捏脸/换装颜色控制配置表（TableAvatarCustomize）
        //=========================================================================
        // 捏脸/换装颜色控制配置表
        //=========================================================================
        /// <summary>
        /// 一组捏脸/换装的颜色控制数据
        /// 对应表：TableAvatarCustomize
        /// </summary>
        public class TableAvatarCustomizeItem
        {
            /// <summary>
            /// 颜色组唯一ID
            /// </summary>
            public string ID;

            /// <summary>
            /// 是否允许换色
            /// </summary>
            public bool EnableColorChange;

            /// <summary>
            /// 受此颜色控制的插槽列表（对应 SlotName）
            /// </summary>
            public List<string> ColorSlotName;

            /// <summary>
            /// 可使用的颜色列表
            /// </summary>
            public List<TableAvatarCustomizeItemColor> ColorRGB;

            /// <summary>
            /// 随机颜色时使用的ID组（对应 ColorRGB 中的 ID）
            /// </summary>
            public List<int> RandomColorID;
        }

        /// <summary>
        /// 单个颜色数据
        /// </summary>
        public class TableAvatarCustomizeItemColor
        {
            /// <summary>
            /// 颜色ID
            /// </summary>
            public int ID;

            /// <summary>
            /// 颜色值（格式：#FFFFFF 或 R,G,B,A）
            /// </summary>
            public string Color;
        }
        #endregion
    }
}