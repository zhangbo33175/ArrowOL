/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapInBounds.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图可活动区域标记组件
 *            用于定义关卡边界、可交互区域，支持编辑器可视化与自动计算
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 关卡内的区域范围（用于标记地图可交互/可活动区域）
    /// </summary>
    public class MapInBounds : MonoBehaviour
    {
        #region 序列化字段
        //=========================================================================
        // 序列化字段
        //=========================================================================
        [Header("区域包围盒")]
        public Bounds areaBounds;

        [Header("区域碰撞体")]
        public Collider2D mapInCollider;
        #endregion

        #region 编辑器可视化
        //=========================================================================
        // 编辑器可视化
        //=========================================================================
        /// <summary>
        /// 编辑器中绘制线框立方体，可视化区域范围
        /// </summary>
        private void OnDrawGizmos()
        {
            if (mapInCollider == null) 
                return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(areaBounds.center, areaBounds.size);
        }
        #endregion

        #region 编辑器工具方法
        //=========================================================================
        // 编辑器工具方法
        //=========================================================================
        /// <summary>
        /// 自动计算碰撞体的包围盒并赋值，计算后关闭碰撞体
        /// </summary>
        [ContextMenu("计算区域范围")]
        public void CalculateBounds()
        {
            if (mapInCollider == null)
            {
                Debug.LogWarning("未指定 MapInCollider 碰撞体！", gameObject);
                return;
            }

            areaBounds = mapInCollider.bounds;
            mapInCollider.enabled = false;

            Debug.Log($"区域范围计算完成：{areaBounds}", gameObject);
        }
        #endregion
    }
}