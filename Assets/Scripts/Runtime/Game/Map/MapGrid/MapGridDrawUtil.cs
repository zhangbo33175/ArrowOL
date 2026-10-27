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
            /// <summary>分区边界颜色</summary>
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
        /// 计算格线位置与分区矩形
        /// 几何契约：编辑器 Handles 与游戏 Overlay 共用同一套坐标
        /// 未解锁分区的格线返回 -1（跳过绘制），分区矩形返回零尺寸
        /// </summary>
        /// <param name="mapW">地图总宽度（像素）</param>
        /// <param name="mapH">地图总高度（像素）</param>
        /// <param name="gridCols">网格列数</param>
        /// <param name="regionCols">分区列数</param>
        /// <param name="regionRows">分区行数</param>
        /// <param name="unlockedSection">已解锁分区数（从1开始计数）</param>
        /// <param name="verticalLines">输出：垂直线X坐标数组（长度 gridCols+1）</param>
        /// <param name="horizontalLines">输出：水平线Y坐标数组（长度 gridRows+1）</param>
        /// <param name="regionRects">输出：分区矩形数组（长度 regionCols*regionRows）</param>
        public static void ComputeGridLinePositions(float mapW, float mapH,
            int gridCols, int regionCols, int regionRows, int unlockedSection,
            out float[] verticalLines, out float[] horizontalLines,
            out Rect[] regionRects)
        {
            verticalLines = new float[gridCols + 1];
            int gridRows = Mathf.RoundToInt(gridCols * mapH / mapW);
            float cellW = mapW / gridCols;
            float cellH = mapH / gridRows;

            // 垂直线：相邻列任一分区已解锁则绘制
            for (int i = 0; i <= gridCols; i++)
            {
                bool anyUnlocked = false;
                for (int c = i - 1; c <= i; c++)
                {
                    if (c < 0 || c >= gridCols) continue;
                    int secCol = c / Mathf.Max(1, gridCols / regionCols);
                    for (int r = 0; r < regionRows; r++)
                    {
                        int sectionId = r * regionCols + secCol + 1;
                        if (sectionId <= unlockedSection) { anyUnlocked = true; break; }
                    }
                    if (anyUnlocked) break;
                }
                verticalLines[i] = anyUnlocked ? i * cellW : -1f;
            }

            // 水平线：相邻行任一分区已解锁则绘制
            horizontalLines = new float[gridRows + 1];
            for (int i = 0; i <= gridRows; i++)
            {
                bool anyUnlocked = false;
                for (int r = i - 1; r <= i; r++)
                {
                    if (r < 0 || r >= gridRows) continue;
                    int secRow = r / Mathf.Max(1, gridRows / regionRows);
                    for (int c = 0; c < regionCols; c++)
                    {
                        int sectionId = secRow * regionCols + c + 1;
                        if (sectionId <= unlockedSection) { anyUnlocked = true; break; }
                    }
                    if (anyUnlocked) break;
                }
                horizontalLines[i] = anyUnlocked ? i * cellH : -1f;
            }

            // 分区矩形
            regionRects = new Rect[regionCols * regionRows];
            float rw = mapW / regionCols;
            float rh = mapH / regionRows;
            for (int r = 0; r < regionRows; r++)
                for (int c = 0; c < regionCols; c++)
                {
                    int sectionId = r * regionCols + c + 1;
                    if (sectionId <= unlockedSection)
                        regionRects[r * regionCols + c] = new Rect(c * rw, r * rh, rw, rh);
                    else
                        regionRects[r * regionCols + c] = new Rect(0, 0, 0, 0);
                }
        }

        #endregion
    }
}
