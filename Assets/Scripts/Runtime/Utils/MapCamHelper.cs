/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapCamHelper.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图相机工具类
 *            提供相机动画、视野可见性检测、射线层级、渲染层级管理
 ***************************************************************/

using System;
using DG.Tweening;
using Honor.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLib
{
    /// <summary>
    /// 地图相机工具类
    /// 提供相机控制、视野检测、射线检测、渲染层级管理等通用功能
    /// </summary>
    public static class MapCamHelper
    {
        #region 静态缓存字段
        //=========================================================================
        // 静态缓存字段
        //=========================================================================
        /// <summary>
        /// 相机视锥体裁剪平面数组
        /// </summary>
        private static readonly Plane[] planes = new Plane[6];

        /// <summary>
        /// 场景2D物理射线检测器
        /// </summary>
        private static Physics2DRaycaster _sceneRaycaster;
        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 地图主相机（场景相机列表第一位）
        /// </summary>
        public static Camera MapCamera => GameMainRoot.Scene.SceneCameras[0];

        /// <summary>
        /// 当前相机正交大小
        /// </summary>
        public static float CurCamSize => MapCamera.orthographicSize;

        /// <summary>
        /// 当前相机本地位置
        /// </summary>
        public static Vector3 CurCamPos => MapCamera.transform.localPosition;
        #endregion

        #region 相机动画控制
        //=========================================================================
        // 相机动画控制
        //=========================================================================
        /// <summary>
        /// 初始化地图相机参数（位置+大小）
        /// </summary>
        /// <param name="normalCamData">相机配置数据</param>
        public static void InitMapCamera(MapCamData normalCamData)
        {
            MapCamera.orthographicSize = normalCamData.size;

            var pos = CurCamPos;
            pos.x = normalCamData.posX;
            pos.y = normalCamData.posY;
            MapCamera.transform.localPosition = pos;
        }

        /// <summary>
        /// 播放相机移动+缩放动画，定位到目标相机数据
        /// </summary>
        /// <param name="mapCamData">目标相机数据</param>
        /// <param name="animTime">动画时长</param>
        /// <param name="finishCallback">动画完成回调</param>
        public static void LookMap(MapCamData mapCamData, float animTime, Action finishCallback)
        {
            if (MapCamera == null)
            {
                finishCallback?.Invoke();
                return;
            }

            var seq = LookMapAnim(mapCamData.posX, mapCamData.posY, mapCamData.size, animTime);
            if (seq != null)
            {
                seq.Play().OnComplete(() => { finishCallback?.Invoke(); });
            }
            else
            {
                finishCallback?.Invoke();
            }
        }

        /// <summary>
        /// 播放相机位置+大小动画
        /// </summary>
        public static void LookMapPositionAndSize(float posX, float posY, float size, float animTime,
            Action finishCallback)
        {
            if (MapCamera == null)
            {
                finishCallback?.Invoke();
                return;
            }

            var seq = LookMapAnim(posX, posY, size, animTime);
            if (seq != null)
            {
                seq.Play().OnComplete(() => { finishCallback?.Invoke(); });
            }
            else
            {
                finishCallback?.Invoke();
            }
        }

        /// <summary>
        /// 直接设置相机位置和大小（无动画）
        /// </summary>
        public static void SetupMapPositionAndSize(float posX, float posY, float size)
        {
            MapCamera.transform.localPosition = new Vector3(posX, posY, CurCamPos.z);
            MapCamera.orthographicSize = size;
        }

        /// <summary>
        /// 仅播放相机位置移动动画
        /// </summary>
        public static void LookMapPosition(float posX, float posY, float animTime, Action finishCallback)
        {
            if (MapCamera == null)
            {
                finishCallback?.Invoke();
                return;
            }

            var seq = LookMapAnim(posX, posY, CurCamSize, animTime);
            if (seq != null)
            {
                seq.Play().OnComplete(() => { finishCallback?.Invoke(); });
            }
            else
            {
                finishCallback?.Invoke();
            }
        }

        /// <summary>
        /// 仅播放相机大小缩放动画
        /// </summary>
        public static void LookMapSizeAnim(float size, float animTime, Action finishCallback)
        {
            var camPos = CurCamPos;
            var seq = LookMapAnim(camPos.x, camPos.y, size, animTime);
            if (seq != null)
            {
                seq.Play().OnComplete(() => { finishCallback?.Invoke(); });
            }
            else
            {
                finishCallback?.Invoke();
            }
        }

        /// <summary>
        /// 播放相机Y坐标+大小动画
        /// </summary>
        public static void LookMapSizeAndPosYAnim(float posY, float size, float animTime, Action finishCallback)
        {
            var camPos = CurCamPos;
            var seq = LookMapAnim(camPos.x, posY, size, animTime);
            if (seq != null)
            {
                seq.Play().OnComplete(() => { finishCallback?.Invoke(); });
            }
            else
            {
                finishCallback?.Invoke();
            }
        }

        /// <summary>
        /// 播放相机X坐标+大小动画
        /// </summary>
        public static void LookMapSizeAndPosXAnim(float posX, float size, float animTime, Action finishCallback)
        {
            var camPos = CurCamPos;
            var seq = LookMapAnim(posX, camPos.y, size, animTime);
            if (seq != null)
            {
                seq.Play().OnComplete(() => { finishCallback?.Invoke(); });
            }
            else
            {
                finishCallback?.Invoke();
            }
        }

        /// <summary>
        /// 判断当前相机数据是否与目标数据完全一致
        /// </summary>
        public static bool IsEqualMapCamData(MapCamData newMapCamData)
        {
            if (newMapCamData == null) return false;
            var mapCamPos = CurCamPos;
            return Mathf.Approximately(mapCamPos.x, newMapCamData.posX) &&
                   Mathf.Approximately(mapCamPos.y, newMapCamData.posY) &&
                   Mathf.Approximately(CurCamSize, newMapCamData.size);
        }

        /// <summary>
        /// 构建相机位移动画序列（内部使用）
        /// </summary>
        private static Sequence LookMapAnim(float posX, float posY, float size, float animTime)
        {
            var mapCamPos = CurCamPos;
            if (!Mathf.Approximately(mapCamPos.x, posX) ||
                !Mathf.Approximately(mapCamPos.y, posY) ||
                !Mathf.Approximately(CurCamSize, size))
            {
                var seq = DOTween.Sequence();
                var action0 = MapCamera.transform.DOLocalMoveX(posX, animTime);
                var action1 = MapCamera.transform.DOLocalMoveY(posY, animTime);
                var action2 = MapCamera.DOOrthoSize(size, animTime);
                seq.Insert(0, action0);
                seq.Insert(0, action1);
                seq.Insert(0, action2);

                return seq;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 可见性判断
        //=========================================================================
        // 可见性判断
        //=========================================================================
        /// <summary>
        /// 判断Bounds是否在相机视锥体内
        /// </summary>
        public static bool IsInCam(Bounds bounds)
        {
            GeometryUtility.CalculateFrustumPlanes(MapCamera, planes);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }

        /// <summary>
        /// 判断世界坐标点是否在相机可视范围内
        /// </summary>
        public static bool IsVisableInCamera(Vector3 pos)
        {
            Vector3 viewPos = MapCamera.WorldToViewportPoint(pos);
            if (viewPos.x < 0 || viewPos.y < 0 || viewPos.x > 1 || viewPos.y > 1) return false;
            return true;
        }

        /// <summary>
        /// 判断包围盒是否有任意顶点在相机可视范围内
        /// </summary>
        public static bool IsBoundsVisableInCamera(Bounds bounds)
        {
            if (bounds != null)
            {
                var min = bounds.min;
                var max = bounds.max;
                var point1 = new Vector3(min.x, max.y, min.z);
                var point2 = new Vector3(min.x, min.y, min.z);
                var point3 = new Vector3(max.x, max.y, min.z);
                var point4 = new Vector3(max.x, min.y, min.z);

                if (IsVisableInCamera(point1) ||
                    IsVisableInCamera(point2) ||
                    IsVisableInCamera(point3) ||
                    IsVisableInCamera(point4))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 相机射线层级
        //=========================================================================
        // 相机射线层级
        //=========================================================================
        /// <summary>
        /// 获取场景射线检测器（自动缓存）
        /// </summary>
        private static Physics2DRaycaster SceneRaycaster()
        {
            if (_sceneRaycaster == null)
            {
                _sceneRaycaster = MapCamera.GetComponent<Physics2DRaycaster>();
            }

            return _sceneRaycaster;
        }

        /// <summary>
        /// 开启射线可检测的层
        /// </summary>
        public static void OpenSceneRaycasterLayer(int layerNumber)
        {
            if (SceneRaycaster() != null)
            {
                SceneRaycaster().eventMask |= 1 << layerNumber;
            }
        }

        /// <summary>
        /// 关闭射线可检测的层
        /// </summary>
        public static void CloseSceneRaycasterLayer(int layerNumber)
        {
            if (SceneRaycaster() != null)
            {
                SceneRaycaster().eventMask &= ~(1 << layerNumber);
            }
        }
        #endregion

        #region 相机渲染与尺寸
        //=========================================================================
        // 相机渲染与尺寸
        //=========================================================================
        /// <summary>
        /// 根据相机大小计算当前可见场景宽度
        /// </summary>
        public static float GetSceneWidthByCamSize(float size)
        {
            var viewSize = Util.GameViewSize();
            var width = viewSize.x / viewSize.y * size * 2;
            return width;
        }

        /// <summary>
        /// 添加相机渲染层
        /// </summary>
        public static void SetCameraRenderLayer(int layerNumber)
        {
            MapCamera.cullingMask |= 1 << layerNumber;
        }

        /// <summary>
        /// 移除相机渲染层
        /// </summary>
        public static void CloseCameraRenderLayer(int layerNumber)
        {
            MapCamera.cullingMask &= ~(1 << layerNumber);
        }
        #endregion
    }
}