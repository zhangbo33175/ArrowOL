using System;
using DG.Tweening;
using Honor.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLib
{
    public static class MapCamHelper
    {
        private static Plane[] planes = new Plane[6];

        public static Camera MapCamera => GameMainRoot.Scene.SceneCameras[0]; //第一位为地图场景相机

        private static Physics2DRaycaster _sceneRaycaster; //场景射线检测器

        //public static Camera EFCamera => GameMainRoot.Effect.efCamera; //特效相机

        /// <summary>
        /// 目前相机大小
        /// </summary>
        public static float CurCamSize => MapCamera.orthographicSize;

        /// <summary>
        /// 目前相机位置
        /// </summary>
        public static Vector3 CurCamPos => MapCamera.transform.localPosition;

        #region 相机动画

        /// <summary>
        /// 初始化地图相机
        /// </summary>
        /// <param name="normalCamData"></param>
        public static void InitMapCamera(MapCamData normalCamData)
        {
            MapCamera.orthographicSize = normalCamData.size;

            var pos = CurCamPos;
            pos.x = normalCamData.posX;
            pos.y = normalCamData.posY;
            MapCamera.transform.localPosition = pos;
        }

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
        /// 播放位置 + Size变化动画
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="size"></param>
        /// <param name="animTime"></param>
        /// <param name="finishCallback"></param>
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
        /// 设置坐标及Size
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="size"></param>
        public static void SetupMapPositionAndSize(float posX, float posY, float size)
        {
            MapCamera.transform.localPosition = new Vector3(posX, posY, CurCamPos.z);
            MapCamera.orthographicSize = size;
            //EFCamera.orthographicSize = size; //特效相机也要变化
        }

        /// <summary>
        /// 播放位置变化动画
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="animTime"></param>
        /// <param name="finishCallback"></param>
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
        /// 播放Size变化动画
        /// </summary>
        /// <param name="size"></param>
        /// <param name="animTime"></param>
        /// <param name="finishCallback"></param>
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
        /// 当前的相机数据是否和新的相机数据相等
        /// </summary>
        /// <param name="newMapCamData"></param>
        /// <returns></returns>
        public static bool IsEqualMapCamData(MapCamData newMapCamData)
        {
            var mapCamPos = CurCamPos;
            return Mathf.Approximately(mapCamPos.x, newMapCamData.posX) &&
                   Mathf.Approximately(mapCamPos.y, newMapCamData.posY) &&          
                   Mathf.Approximately(CurCamSize, newMapCamData.size);
        }

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
                //var action3 = EFCamera.DOOrthoSize(size, animTime); //特效相机也要变化
                seq.Insert(0, action0);
                seq.Insert(0, action1);
                seq.Insert(0, action2);
                //seq.Insert(0, action3);

                return seq;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 可见性判断

        /// <summary>
        /// 是否在相机视野内
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static bool IsInCam(Bounds bounds)
        {
            GeometryUtility.CalculateFrustumPlanes(MapCamera, planes);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }


        /// <summary>
        /// 是否在相机视角内
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool IsVisableInCamera(Vector3 pos)
        {
            //转化为视角坐标
            Vector3 viewPos = MapCamera.WorldToViewportPoint(pos);
            /*// z<0代表在相机背后
            if (viewPos.z < 0) return false;
            //太远了！看不到了！
            if (viewPos.z > MapCamera.farClipPlane)
                return false;*/
            // x,y取值在 0~1之外时代表在视角范围外；
            if (viewPos.x < 0 || viewPos.y < 0 || viewPos.x > 1 || viewPos.y > 1) return false;
            return true;
        }

        /// <summary>
        /// Bounds是否在相机视角内
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
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

        #region 设置相机射线层级

        /// <summary>
        /// 场景相机射线
        /// </summary>
        /// <returns></returns>
        private static Physics2DRaycaster SceneRaycaster()
        {
            if (_sceneRaycaster == null)
            {
                _sceneRaycaster = MapCamera.GetComponent<Physics2DRaycaster>();
            }

            return _sceneRaycaster;
        }

        /// <summary>
        /// 开启场景相机射线层级
        /// </summary>
        /// <param name="layerNumber"></param>
        public static void OpenSceneRaycasterLayer(int layerNumber)
        {
            if (SceneRaycaster() != null)
            {
                SceneRaycaster().eventMask |= 1 << layerNumber;
            }
        }

        /// <summary>
        /// 关闭场景相机射线
        /// </summary>
        /// <param name="layerNumber"></param>
        public static void CloseSceneRaycasterLayer(int layerNumber)
        {
            if (SceneRaycaster() != null)
            {
                SceneRaycaster().eventMask &= ~ (1 << layerNumber);
            }
        }

        #endregion

        /// <summary>
        /// 通过相机Size获取Size下,对于相机可见场景宽度
        /// </summary>
        /// <param name="size">相机Size</param>
        /// <returns>可见场景的宽度</returns>
        public static float GetSceneWidthByCamSize(float size)
        {
            var viewSize = Util.GameViewSize();
            var width = viewSize.x / viewSize.y * size * 2;
            return width;
        }

        /// <summary>
        /// 增加渲染层
        /// </summary>
        /// <param name="layerNumber"></param>
        public static void SetCameraRenderLayer(int layerNumber)
        {
            MapCamera.cullingMask |= 1 << layerNumber;
        }

        /// <summary>
        /// 关闭渲染层
        /// </summary>
        /// <param name="layerNumber"></param>
        public static void CloseCameraRenderLayer(int layerNumber)
        {
            MapCamera.cullingMask &= ~ (1 << layerNumber);
        }
    }
}