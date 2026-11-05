/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图加载 C# 实现（Lua 参数驱动薄壳）
 *            关卡挂载已迁移至纯 Lua 实现，本类只负责：
 *            地图包围盒计算与适配（SetupMapBounds）
 ***************************************************************/

using UnityEngine;
using XLua;

namespace GameLib
{
    /// <summary>
    /// 地图加载 C# 实现（Lua 参数驱动薄壳）
    /// 单关卡模式：一章一关卡一地图，整图一次加载
    /// </summary>
    [LuaCallCSharp]
    public static class MapLoadManager
    {
        #region 包围盒适配

        /// <summary>
        /// 手动设置地图包围盒（中心坐标系：原点在地图中心）
        /// 同时更新 MapInBounds、BoxCollider2D、ZoneRoot RectTransform
        /// </summary>
        /// <param name="gameMapRoot">GameMainMap 根节点</param>
        /// <param name="rectW">地图宽度（像素）</param>
        /// <param name="rectH">地图高度（像素）</param>
        /// <param name="centerX">包围盒中心X</param>
        /// <param name="centerY">包围盒中心Y</param>
        public static void SetupMapBounds(GameObject gameMapRoot, float rectW, float rectH, float centerX, float centerY)
        {
            if (gameMapRoot == null) return;

            // 禁用 ContentSizeFitter（避免自动布局干扰）
            var csf = gameMapRoot.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            if (csf != null) csf.enabled = false;

            // 设置根节点 RectTransform（中心 pivot）
            var rt = gameMapRoot.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(rectW, rectH);
                rt.localScale = new Vector3(1f, 1f, 1f);
                rt.localPosition = new Vector3(0, 0, 0);
            }

            // 设置 ZoneRoot（中心 pivot）
            var zoneRoot = gameMapRoot.transform.Find("ZoneRoot");
            if (zoneRoot != null)
            {
                var zrt = zoneRoot as RectTransform;
                if (zrt != null)
                {
                    var zcsf = zoneRoot.GetComponent<UnityEngine.UI.ContentSizeFitter>();
                    if (zcsf != null) zcsf.enabled = false;
                    var zglg = zoneRoot.GetComponent<UnityEngine.UI.GridLayoutGroup>();
                    if (zglg != null) zglg.enabled = false;
                    zrt.pivot = new Vector2(0.5f, 0.5f);
                    zrt.anchorMin = new Vector2(0.5f, 0.5f);
                    zrt.anchorMax = new Vector2(0.5f, 0.5f);
                    zrt.localPosition = Vector3.zero;
                    zrt.sizeDelta = new Vector2(rectW, rectH);
                }
            }

            // 设置 MapInBounds
            var bounds = gameMapRoot.GetComponent("GameLib.MapInBounds");
            if (bounds != null)
            {
                var newBounds = new Bounds(new Vector3(centerX, centerY, 0), new Vector3(rectW, rectH, 0));
                SetBoundsValue(bounds, newBounds);
            }

            // 设置 BoxCollider2D
            var col = gameMapRoot.GetComponent<UnityEngine.BoxCollider2D>();
            if (col != null)
            {
                col.size = new Vector2(rectW, rectH);
                col.offset = new Vector2(centerX, centerY);
                col.enabled = true;
            }
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 反射设置 MapInBounds 的包围盒值（兼容字段/属性两种命名）
        /// </summary>
        private static void SetBoundsValue(object bounds, Bounds newBounds)
        {
            var field = bounds.GetType().GetField("m_AreaBounds");
            if (field != null)
            {
                field.SetValue(bounds, newBounds);
            }
            else
            {
                var prop = bounds.GetType().GetProperty("areaBounds");
                if (prop != null) prop.SetValue(bounds, newBounds, null);
            }
            var autoF = bounds.GetType().GetField("m_AutoRecalculateOnAwake");
            if (autoF != null) autoF.SetValue(bounds, false);
        }

        #endregion
    }
}
