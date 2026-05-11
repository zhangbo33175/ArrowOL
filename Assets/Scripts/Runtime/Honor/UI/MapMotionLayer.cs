using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        public Bounds areaBounds;

        /// <summary>
        /// Scene视图绘制包围盒线框，用于可视化编辑
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(areaBounds.center, areaBounds.size);
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
            
            // 空值安全校验
            if (motionLayer == null)
            {
                return;
            }

            // 清空并获取所有子物体Renderer
            childRenderers.Clear();
            motionLayer.GetComponentsInChildren(childRenderers);
            
            // 初始化包围盒
            Bounds resultBounds = new Bounds(motionLayer.transform.position, Vector2.one);

            // 遍历合并包围盒
            for (int i = 0; i < childRenderers.Count; i++)
            {
                Renderer renderer = childRenderers[i];
                
                // 跳过粒子渲染器 & 空对象
                if (renderer == null || renderer is ParticleSystemRenderer)
                {
                    continue;
                }

                resultBounds.Encapsulate(renderer.bounds);
            }

            // 保持2D，重置Z轴尺寸为0
            resultBounds.size = new Vector3(resultBounds.size.x, resultBounds.size.y, 0);
            
            // 赋值并标记修改，确保可保存
            motionLayer.areaBounds = resultBounds;
            EditorUtility.SetDirty(motionLayer);
        }
    }
#endif
}