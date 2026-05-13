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
        /// <summary>
        /// 地图区域包围盒（2D平面使用，忽略Z轴）
        /// </summary>
        [FormerlySerializedAs("areaBounds")] 
        public Bounds m_AreaBounds;

        /// <summary>
        /// Scene视图绘制包围盒线框，用于可视化编辑
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(m_AreaBounds.center, m_AreaBounds.size);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// MapMotionLayer 自定义编辑器
    /// 提供一键根据子物体自动计算包围盒的编辑功能
    /// </summary>
    [CustomEditor(typeof(MapMotionLayer))]
    public class MapMotionLayerEditor : Editor
    {
        /// <summary>
        /// 子物体渲染器缓存列表，预分配容量减少GC
        /// </summary>
        private static readonly List<Renderer> childRenderers = new List<Renderer>(128);

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

        /// <summary>
        /// 自动遍历所有子物体渲染器，生成合并后的包围盒
        /// 自动跳过粒子系统渲染器，保持2D平面尺寸
        /// </summary>
        private void RegenerateBounds()
        {
            MapMotionLayer motionLayer = target as MapMotionLayer;
            if (motionLayer == null) return;

            childRenderers.Clear();
            motionLayer.GetComponentsInChildren(childRenderers);

            // 新增：获取所有UI Image/RawImage
            List<UnityEngine.UI.Image> uiImages = new List<UnityEngine.UI.Image>();
            motionLayer.GetComponentsInChildren(uiImages);

            Bounds resultBounds = new Bounds(motionLayer.transform.position, Vector2.one);

            // 合并Renderer的Bounds
            foreach (var renderer in childRenderers)
            {
                if (renderer == null || renderer is ParticleSystemRenderer) continue;
                resultBounds.Encapsulate(renderer.bounds);
            }

            // 新增：合并UI Image的Bounds
            foreach (var image in uiImages)
            {
                RectTransform rectTransform = image.rectTransform;
                Vector3[] corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                // 计算UI世界坐标的包围盒
                Bounds uiBounds = new Bounds(corners[0], Vector3.zero);
                for (int i = 0; i < 4; i++)
                {
                    uiBounds.Encapsulate(corners[i]);
                }
                resultBounds.Encapsulate(uiBounds);
            }

            resultBounds.size = new Vector3(resultBounds.size.x, resultBounds.size.y, 0);
            motionLayer.m_AreaBounds = resultBounds;
            EditorUtility.SetDirty(motionLayer);
        }
    }
#endif
}