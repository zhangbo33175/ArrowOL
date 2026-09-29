/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapMotionLayer.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图运动层组件
 *            定义地图包围盒，支持编辑器一键自动计算边界
 ***************************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLib
{
    /// <summary>
    /// 地图运动层：定义可运动地图的区域包围盒，用于场景范围可视化与边界计算
    /// </summary>
    public class MapMotionLayer : MonoBehaviour
    {
        #region 公共字段
        //=========================================================================
        // 公共字段
        //=========================================================================
        /// <summary>
        /// 地图区域包围盒（2D平面使用，忽略Z轴）
        /// </summary>
        [FormerlySerializedAs("areaBounds")]
        public Bounds m_AreaBounds;

        /// <summary>
        /// 是否在Awake时自动根据子物体重算包围盒
        /// </summary>
        public bool m_AutoRecalculateOnAwake = false;

        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================
        /// <summary>
        /// 唤醒时按配置决定是否自动重算包围盒
        /// </summary>
        private void Awake()
        {
            if (m_AutoRecalculateOnAwake)
            {
                RecalculateBounds();
            }
        }

        /// <summary>
        /// 运行时根据所有子物体Renderers和UI Image重算包围盒
        /// </summary>
        public void RecalculateBounds()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            Bounds resultBounds = new Bounds(transform.position, Vector2.one);

            bool hasAny = false;
            foreach (var renderer in renderers)
            {
                if (renderer == null || renderer is ParticleSystemRenderer) continue;
                if (!hasAny)
                {
                    resultBounds = renderer.bounds;
                    hasAny = true;
                }
                else
                {
                    resultBounds.Encapsulate(renderer.bounds);
                }
            }

            UnityEngine.UI.Image[] images = GetComponentsInChildren<UnityEngine.UI.Image>();
            // 复用同一缓冲区：GetWorldCorners 每次都会覆盖写入全部 4 个角点，无需逐 Image 重新分配
            Vector3[] corners = new Vector3[4];
            foreach (var image in images)
            {
                if (image == null || !image.enabled) continue;
                RectTransform rt = image.rectTransform;
                rt.GetWorldCorners(corners);
                if (!hasAny)
                {
                    resultBounds = new Bounds(corners[0], Vector3.zero);
                    hasAny = true;
                }
                for (int i = 0; i < 4; i++)
                {
                    resultBounds.Encapsulate(corners[i]);
                }
            }

            if (hasAny)
            {
                resultBounds.size = new Vector3(resultBounds.size.x, resultBounds.size.y, 0);
                m_AreaBounds = resultBounds;
                Debug.Log($"[MapMotionLayer] RecalculateBounds: center={resultBounds.center} extents={resultBounds.extents}");
            }
        }
        #endregion

        #region 场景绘制
        //=========================================================================
        // 场景绘制
        //=========================================================================
        /// <summary>
        /// Scene视图绘制包围盒线框，用于可视化编辑
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(m_AreaBounds.center, m_AreaBounds.size);
        }
        #endregion
    }

#if UNITY_EDITOR
    /// <summary>
    /// MapMotionLayer 自定义编辑器
    /// 提供一键根据子物体自动计算包围盒的编辑功能
    /// </summary>
    [CustomEditor(typeof(MapMotionLayer))]
    public class MapMotionLayerEditor : Editor
    {
        #region 静态缓存
        //=========================================================================
        // 静态缓存
        //=========================================================================
        /// <summary>
        /// 子物体渲染器缓存列表，预分配容量减少GC
        /// </summary>
        private static readonly List<Renderer> childRenderers = new List<Renderer>(128);
        #endregion

        #region 编辑器绘制
        //=========================================================================
        // 编辑器绘制
        //=========================================================================
        /// <summary>
        /// 绘制Inspector面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            // 禁用默认字段编辑，仅展示
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;

            // 按钮布局与点击事件
            if (GUILayout.Button("Regenerate Bounds", GUILayout.Height(30)))
            {
                RegenerateBounds();
            }
        }
        #endregion

        #region 私有功能方法
        //=========================================================================
        // 私有功能方法
        //=========================================================================
        /// <summary>
        /// 自动遍历所有子物体渲染器，生成合并后的包围盒
        /// 自动跳过粒子系统渲染器，保持2D平面尺寸
        /// </summary>
        private void RegenerateBounds()
        {
            MapMotionLayer motionLayer = target as MapMotionLayer;
            if (motionLayer == null) return;
            motionLayer.RecalculateBounds();
            EditorUtility.SetDirty(motionLayer);
        }
        #endregion
    }
#endif
}