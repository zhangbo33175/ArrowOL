/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIDefineEnums.cs
 * author:    云毅
 *created:   2026
 * descrip:   UI/布局系统通用枚举定义文件，包含吸附、方位、排列、网格布局等枚举
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    //=========================================================================
    // UI & 布局系统 通用枚举定义
    //=========================================================================

    #region 吸附/对齐状态枚举
    /// <summary>
    /// 吸附/对齐状态枚举
    /// 用于标记UI/物体的吸附移动生命周期状态
    /// </summary>
    public enum SnapStatus
    {
        /// <summary>
        /// 未设置吸附目标
        /// </summary>
        NoTargetSet = 0,

        /// <summary>
        /// 已设置吸附目标
        /// </summary>
        TargetHasSet = 1,

        /// <summary>
        /// 正在执行吸附移动
        /// </summary>
        SnapMoving = 2,

        /// <summary>
        /// 吸附移动完成
        /// </summary>
        SnapMoveFinish = 3
    }
    #endregion

    #region 物品/UI元素四角方位枚举
    /// <summary>
    /// 物品/UI元素四角方位枚举
    /// 用于定义物体四个角落的位置类型
    /// </summary>
    public enum ItemCornerEnum
    {
        /// <summary>
        /// 左下角
        /// </summary>
        LeftBottom = 0,

        /// <summary>
        /// 左上角
        /// </summary>
        LeftTop,

        /// <summary>
        /// 右上角
        /// </summary>
        RightTop,

        /// <summary>
        /// 右下角
        /// </summary>
        RightBottom
    }
    #endregion

    #region 列表项排列方向类型
    /// <summary>
    /// 列表项排列方向类型
    /// 用于控制线性列表（List）的子项布局方向
    /// </summary>
    public enum ListItemArrangeType
    {
        /// <summary>
        /// 从上到下垂直排列
        /// </summary>
        TopToBottom = 0,

        /// <summary>
        /// 从下到上垂直排列
        /// </summary>
        BottomToTop,

        /// <summary>
        /// 从左到右水平排列
        /// </summary>
        LeftToRight,

        /// <summary>
        /// 从右到左水平排列
        /// </summary>
        RightToLeft
    }
    #endregion

    #region 网格项排列方向类型
    /// <summary>
    /// 网格项排列方向类型
    /// 用于控制网格布局（Grid）的子项起始位置与排布方向
    /// </summary>
    public enum GridItemArrangeType
    {
        /// <summary>
        /// 左上起点 → 向右向下排布
        /// </summary>
        TopLeftToBottomRight = 0,

        /// <summary>
        /// 左下起点 → 向右向上排布
        /// </summary>
        BottomLeftToTopRight,

        /// <summary>
        /// 右上起点 → 向左向下排布
        /// </summary>
        TopRightToBottomLeft,

        /// <summary>
        /// 右下起点 → 向左向上排布
        /// </summary>
        BottomRightToTopLeft
    }
    #endregion

    #region 网格固定约束类型
    /// <summary>
    /// 网格固定约束类型
    /// 用于定义网格布局是固定列数还是固定行数
    /// </summary>
    public enum GridFixedType
    {
        /// <summary>
        /// 固定列数，自动计算行数
        /// </summary>
        ColumnCountFixed = 0,

        /// <summary>
        /// 固定行数，自动计算列数
        /// </summary>
        RowCountFixed
    }
    #endregion

    #region 行列索引结构体
    /// <summary>
    /// 行列索引结构体
    /// 用于存储网格布局中的行号与列号，支持等值比较
    /// </summary>
    [Serializable]
    public struct RowColumnPair : IEquatable<RowColumnPair>
    {
        /// <summary>
        /// 行索引
        /// </summary>
        public int mRow;

        /// <summary>
        /// 列索引
        /// </summary>
        public int mColumn;

        /// <summary>
        /// 构造函数：初始化行与列索引
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        public RowColumnPair(int row, int column)
        {
            mRow = row;
            mColumn = column;
        }

        /// <summary>
        /// 强类型等值比较
        /// </summary>
        /// <param name="other">待比较的RowColumnPair</param>
        /// <returns>是否相等</returns>
        public bool Equals(RowColumnPair other)
        {
            return mRow == other.mRow && mColumn == other.mColumn;
        }

        /// <summary>
        /// 重载 == 运算符
        /// </summary>
        public static bool operator ==(RowColumnPair a, RowColumnPair b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 重载 != 运算符
        /// </summary>
        public static bool operator !=(RowColumnPair a, RowColumnPair b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// 重写哈希值（使用行列组合计算，避免哈希冲突）
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(mRow, mColumn);
        }

        /// <summary>
        /// 对象类型等值比较
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is RowColumnPair other && Equals(other);
        }
    }
    #endregion
}