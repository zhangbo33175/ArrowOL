/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图加载 C# 实现（Lua 参数驱动薄壳）
 *            分区挂载 / 包围盒适配 / 相机同步 / 分区解锁锁定
 ***************************************************************/

using UnityEngine;
using XLua;

namespace GameLib
{
    /// <summary>
    /// 地图加载 C# 实现（Lua 参数驱动薄壳）
    /// 网格逻辑已迁移至 GridManager，本类只负责：
    /// 1. 分区挂载与定位（SetupZoneItem / CreateZoneRoot）
    /// 2. 地图包围盒计算与适配（SetupMapBounds / RefreshMapBoundsAndCamera）
    /// 3. 分区解锁与锁定（UnlockZone / LockZone）
    /// </summary>
    [LuaCallCSharp]
    public static class MapLoadManager
    {
        #region 分区挂载

        /// <summary>
        /// 挂载 ZoneItem 到 ZoneRoot
        /// 根据 sectionId 计算行列坐标，设置位置与背景图
        /// </summary>
        /// <param name="zoneGo">ZoneItem 预制体实例</param>
        /// <param name="zoneRoot">ZoneRoot 容器 Transform</param>
        /// <param name="sectionId">分区ID（从1开始）</param>
        /// <param name="cols">分区列数</param>
        /// <param name="rows">分区行数</param>
        /// <param name="scale">缩放（预留）</param>
        /// <param name="bgSprite">背景图 Sprite</param>
        /// <param name="unlocked">是否已解锁（控制显隐）</param>
        public static void SetupZoneItem(GameObject zoneGo, Transform zoneRoot, int sectionId,
            int cols, int rows, float scale, Sprite bgSprite, bool unlocked)
        {
            if (zoneGo == null || zoneRoot == null) return;

            zoneGo.transform.SetParent(zoneRoot, false);
            zoneGo.name = "ZoneItem_" + sectionId;

            int idx = sectionId - 1;
            int col = idx % cols;
            int row = idx / cols;

            // 分区尺寸：优先从预制体 RectTransform 读取，默认 2048
            var zoneRt = zoneGo.transform as RectTransform;
            float zoneW = 2048f, zoneH = 2048f;
            if (zoneRt != null)
            {
                zoneW = zoneRt.sizeDelta.x > 0 ? zoneRt.sizeDelta.x : 2048f;
                zoneH = zoneRt.sizeDelta.y > 0 ? zoneRt.sizeDelta.y : 2048f;
            }

            // top-left anchor 坐标系：向右为X正，向下为Y负
            float worldX = col * zoneW;
            float worldY = -row * zoneH;
            if (zoneRt != null)
            {
                zoneRt.anchoredPosition = new Vector2(worldX, worldY);
            }
            else
            {
                zoneGo.transform.localPosition = new Vector3(worldX, worldY, 0);
            }

            // 设置背景图
            var bg = zoneGo.transform.Find("RawImage");
            if (bg != null)
            {
                var rt = bg as RectTransform;
                if (rt != null)
                {
                    rt.sizeDelta = Vector2.zero;
                    rt.localScale = new Vector3(1f, 1f, 1f);
                }
                if (bgSprite != null)
                {
                    var img = bg.GetComponent<UnityEngine.UI.Image>();
                    if (img != null) img.sprite = bgSprite;
                }
            }

            // 设置分区包围盒（反射方式，避免硬引用 MapInBounds 类型）
            var bounds = zoneGo.GetComponent("GameLib.MapInBounds");
            if (bounds != null)
            {
                Vector3 boundsCenter = new Vector3(worldX + zoneW * 0.5f, worldY - zoneH * 0.5f, 0f);
                var zoneBounds = new Bounds(boundsCenter, new Vector3(zoneW, zoneH, 0f));
                SetBoundsValue(bounds, zoneBounds);
            }

            zoneGo.SetActive(unlocked);
        }

        /// <summary>
        /// 创建或获取 ZoneRoot 容器节点
        /// 查找顺序：parent/ZoneRoot → parent/GridOverlay/ZoneRoot → 新建
        /// </summary>
        /// <param name="parent">GameMainMap 根节点</param>
        /// <returns>ZoneRoot Transform</returns>
        public static Transform CreateZoneRoot(Transform parent)
        {
            if (parent == null) return null;

            // 优先直接查找
            var direct = parent.Find("ZoneRoot");
            if (direct != null) return direct;

            // 其次查找 GridOverlay 下
            var nested = parent.Find("GridOverlay/ZoneRoot");
            if (nested != null)
            {
                nested.SetParent(parent, false);
                nested.localPosition = Vector3.zero;
                return nested;
            }

            // 新建
            var go = new GameObject("ZoneRoot");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;
            return go.transform;
        }

        #endregion

        #region 包围盒与相机

        /// <summary>
        /// 手动设置地图包围盒
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

            // 设置根节点 RectTransform
            var rt = gameMapRoot.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.pivot = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(rectW, rectH);
                rt.localScale = new Vector3(1f, 1f, 1f);
                rt.localPosition = new Vector3(0, 0, 0);
            }

            // 设置 ZoneRoot
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
                    zrt.pivot = new Vector2(0, 0);
                    zrt.anchorMin = Vector2.zero;
                    zrt.anchorMax = Vector2.zero;
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

            Debug.Log($"[相机] 地图包围盒适配 {rectW:F0}x{rectH:F0} center=({centerX:F0},{centerY:F0})");
        }

        /// <summary>
        /// 根据已解锁分区重新计算包围盒并适配相机
        /// 自动遍历 ZoneRoot 下所有 active 的分区，计算外接矩形
        /// </summary>
        /// <param name="gameMainMap">GameMainMap 根节点</param>
        /// <param name="zoneSize">默认分区尺寸（分区 RectTransform 未设置时使用）</param>
        /// <param name="camera">可选：同步设置相机位置与尺寸</param>
        public static void RefreshMapBoundsAndCamera(GameObject gameMainMap, float zoneSize = 2048f, Camera camera = null)
        {
            if (gameMainMap == null) return;
            var zoneRoot = gameMainMap.transform.Find("ZoneRoot");
            if (zoneRoot == null) return;

            bool hasAny = false;
            float minLeft = float.MaxValue, minTop = float.MinValue;
            float maxRight = float.MinValue, maxBottom = float.MaxValue;
            int activeCount = 0;

            // 遍历所有 active 分区，计算外接矩形
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                Transform child = zoneRoot.GetChild(i);
                if (child == null || !child.gameObject.activeSelf) continue;

                var childRt = child as RectTransform;
                float ax = 0f, ay = 0f;
                float childW = zoneSize, childH = zoneSize;
                if (childRt != null)
                {
                    ax = childRt.anchoredPosition.x;
                    ay = childRt.anchoredPosition.y;
                    if (childRt.sizeDelta.x > 0) childW = childRt.sizeDelta.x;
                    if (childRt.sizeDelta.y > 0) childH = childRt.sizeDelta.y;
                }
                else
                {
                    ax = child.localPosition.x;
                    ay = child.localPosition.y;
                }

                // ZoneItem pivot=top-left(0,1)：从 (ax, ay) 向右下延伸 childW × childH
                float left = ax;
                float right = ax + childW;
                float top = ay;
                float bottom = ay - childH;

                if (!hasAny)
                {
                    minLeft = left; minTop = top;
                    maxRight = right; maxBottom = bottom;
                    hasAny = true;
                }
                else
                {
                    if (left < minLeft) minLeft = left;
                    if (right > maxRight) maxRight = right;
                    if (top > minTop) minTop = top;
                    if (bottom < maxBottom) maxBottom = bottom;
                }
                activeCount++;
            }

            if (!hasAny) return;

            float totalW = maxRight - minLeft;
            float totalH = minTop - maxBottom;
            float centerX = (minLeft + maxRight) * 0.5f;
            float centerY = (maxBottom + minTop) * 0.5f;

            // 设置 ZoneRoot RectTransform
            var zrt = zoneRoot as RectTransform;
            if (zrt != null)
            {
                var csf = zoneRoot.GetComponent<UnityEngine.UI.ContentSizeFitter>();
                if (csf != null) csf.enabled = false;
                var glg = zoneRoot.GetComponent<UnityEngine.UI.GridLayoutGroup>();
                if (glg != null) glg.enabled = false;
                zrt.anchorMin = new Vector2(0f, 1f);
                zrt.anchorMax = new Vector2(0f, 1f);
                zrt.pivot = new Vector2(0f, 1f);
                zrt.anchoredPosition = Vector2.zero;
                zrt.sizeDelta = new Vector2(totalW, totalH);
            }

            // 设置 MapInBounds
            var bounds = gameMainMap.GetComponent("GameLib.MapInBounds");
            if (bounds != null)
            {
                var newBounds = new Bounds(new Vector3(centerX, centerY, 0f), new Vector3(totalW, totalH, 0f));
                SetBoundsValue(bounds, newBounds);
            }

            // 设置 MapMotionLayer 包围盒
            var motionLayer = gameMainMap.GetComponent("GameLib.MapMotionLayer");
            if (motionLayer != null)
            {
                var newBounds = new Bounds(new Vector3(centerX, centerY, 0f), new Vector3(totalW, totalH, 0f));
                var mlField = motionLayer.GetType().GetField("m_AreaBounds");
                if (mlField != null) mlField.SetValue(motionLayer, newBounds);
                var mlAuto = motionLayer.GetType().GetField("m_AutoRecalculateOnAwake");
                if (mlAuto != null) mlAuto.SetValue(motionLayer, false);
            }

            // 设置 BoxCollider2D
            var col = gameMainMap.GetComponent<UnityEngine.BoxCollider2D>();
            if (col != null)
            {
                col.size = new Vector2(totalW, totalH);
                col.offset = new Vector2(centerX, centerY);
                col.enabled = true;
            }

            // 同步相机
            if (camera != null)
            {
                float aspect = camera.aspect;
                float sizeByHeight = totalH * 0.5f;
                float sizeByWidth = totalW * 0.5f / aspect;
                float minSize = Mathf.Max(sizeByHeight, sizeByWidth);
                camera.orthographicSize = minSize;
                camera.transform.position = new Vector3(centerX, centerY, camera.transform.position.z);
                Debug.Log($"[MapBounds] 相机: size={minSize:F0} aspect={aspect:F2} center=({centerX:F0},{centerY:F0})");
            }

            Debug.Log($"[MapBounds] 刷新: 分区={activeCount} 尺寸={totalW:F0}x{totalH:F0} 中心=({centerX:F0},{centerY:F0})");
        }

        #endregion

        #region 分区解锁/锁定

        /// <summary>
        /// 解锁指定分区（SetActive(true) 后刷新包围盒）
        /// </summary>
        public static void UnlockZone(GameObject gameMainMap, int sectionId, float zoneSize = 2048f, Camera camera = null)
        {
            SetZoneActive(gameMainMap, sectionId, true);
            RefreshMapBoundsAndCamera(gameMainMap, zoneSize, camera);
        }

        /// <summary>
        /// 锁定指定分区（SetActive(false) 后刷新包围盒）
        /// </summary>
        public static void LockZone(GameObject gameMainMap, int sectionId, float zoneSize = 2048f, Camera camera = null)
        {
            SetZoneActive(gameMainMap, sectionId, false);
            RefreshMapBoundsAndCamera(gameMainMap, zoneSize, camera);
        }

        #endregion

        #region 内部方法

        /// <summary>设置分区显隐</summary>
        private static void SetZoneActive(GameObject gameMainMap, int sectionId, bool active)
        {
            if (gameMainMap == null) return;
            var zoneRoot = gameMainMap.transform.Find("ZoneRoot");
            if (zoneRoot == null) return;

            string zoneName = $"ZoneItem_{sectionId}";
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                Transform child = zoneRoot.GetChild(i);
                if (child.name == zoneName)
                {
                    child.gameObject.SetActive(active);
                    Debug.Log($"[MapBounds] 分区{(active ? "解锁" : "锁定")}: {zoneName}");
                    break;
                }
            }
        }

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
