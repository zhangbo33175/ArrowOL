/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameExtensionForUnity.Physics3D.cs
 * author:    云毅
 * created:   2025
 * descrip:   3D物理射线检测扩展方法
 ***************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Honor.Runtime
{
   /// <summary>
    /// Unity 3D 物理检测扩展方法
    /// <para>提供 3D 射线检测、物体碰撞检测等便捷功能</para>
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        #region 3D 射线检测扩展
        //=========================================================================
        // 3D 物理射线检测（屏幕坐标转世界射线）
        //=========================================================================
        /// <summary>
        /// 从屏幕坐标发射 3D 物理射线，获取第一个碰撞到的物体 Transform
        /// 适用于鼠标点击、触摸检测 3D 物体
        /// </summary>
        /// <param name="screenPosition">屏幕坐标（例如 Input.mousePosition）</param>
        /// <param name="camera">发射射线的摄像机，为空时自动使用 Camera.main</param>
        /// <returns>碰撞到的物体 Transform，未碰撞则返回 null</returns>
        public static Transform GetRaycastHit3DTransform(this Vector3 screenPosition, Camera camera = null)
        {
            // 自动获取主相机，增强方法易用性
            if (camera == null)
                camera = Camera.main;

            // 相机为空直接返回 null，避免空引用异常
            if (camera == null)
                return null;

            // 从屏幕坐标创建射线
            Ray ray = camera.ScreenPointToRay(screenPosition);

            // 执行 3D 射线检测
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform;
            }

            return null;
        }
        #endregion
    }
}