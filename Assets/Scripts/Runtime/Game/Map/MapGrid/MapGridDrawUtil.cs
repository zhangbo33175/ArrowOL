/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapGridDrawUtil.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格绘制共享核心
 *            编辑器 Scene 视图与运行时 Game 视图共用的几何计算工具
 ***************************************************************/

using UnityEngine;
using XLua;

namespace GameLib
{
    /// <summary>
    /// 网格绘制共享核心（静态纯工具，无状态）
    /// 统一编辑器 Handles 与运行时 Overlay 的网格几何计算，保证 1:1 对齐
    /// </summary>
    [LuaCallCSharp]
    public static class MapGridDrawUtil
    {
        #region 数据结构

        /// <summary>网格绘制风格配置</summary>
        public struct GridStyle
        {
            /// <summary>格线颜色</summary>
            public Color lineColor;
            /// <summary>边界颜色</summary>
            public Color boundaryColor;
            /// <summary>占用格颜色</summary>
            public Color occupiedColor;
        }

        #endregion

        #region 默认风格

        /// <summary>
        /// 默认风格（对齐编辑器 SceneEditModule 常量）
        /// </summary>
        public static GridStyle DefaultStyle()
        {
            return new GridStyle
            {
                lineColor = new Color(0.5f, 0.5f, 0.5f, 0.3f),
                boundaryColor = new Color(0.7f, 0.7f, 0.7f, 0.8f),
                occupiedColor = new Color(1f, 0f, 0f, 0.45f)
            };
        }

        #endregion

        #region 几何计算

        /// <summary>
        /// 计算格线位置（单关卡模式：整图网格）
        /// 几何契约：编辑器 Handles 与游戏 Overlay 共用同一套坐标
        /// </summary>
        /// <param name="mapW">地图总宽度（像素）</param>
        /// <param name="mapH">地图总高度（像素）</param>
        /// <param name="gridCols">网格列数</param>
        /// <param name="verticalLines">输出：垂直线X坐标数组（长度 gridCols+1）</param>
        /// <param name="horizontalLines">输出：水平线Y坐标数组（长度 gridRows+1）</param>
        /// <param name="mapRect">输出：地图矩形（整图范围）</param>
        public static void ComputeGridLinePositions(float mapW, float mapH,
            int gridCols,
            out float[] verticalLines, out float[] horizontalLines,
            out Rect mapRect)
        {
            int gridRows = Mathf.RoundToInt(gridCols * mapH / mapW);
            float cellW = mapW / gridCols;
            float cellH = mapH / gridRows;

            // 垂直线
            verticalLines = new float[gridCols + 1];
            for (int i = 0; i <= gridCols; i++)
            {
                verticalLines[i] = i * cellW;
            }

            // 水平线
            horizontalLines = new float[gridRows + 1];
            for (int i = 0; i <= gridRows; i++)
            {
                horizontalLines[i] = i * cellH;
            }

            // 整图矩形
            mapRect = new Rect(0, 0, mapW, mapH);
        }

        #endregion
    }
}
