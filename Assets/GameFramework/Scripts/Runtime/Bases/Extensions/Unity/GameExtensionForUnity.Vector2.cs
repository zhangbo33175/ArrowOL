using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Unity 向量 & 2D 物理射线检测扩展方法
    /// 提供 Vector2/Vector3 转换、屏幕坐标转世界坐标射线、线检测、球形检测等便捷功能
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 将 Vector2 转换为 Vector3，Y 轴固定为 0
        /// 格式：(x, y) → (x, 0, y)
        /// </summary>
        /// <param name="vector2">待转换的 2D 向量</param>
        /// <returns>转换后的 3D 向量</returns>
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0f, vector2.y);
        }

        /// <summary>
        /// 将 Vector2 转换为 Vector3，使用自定义 Y 值
        /// 格式：(x, y) → (x, customY, y)
        /// </summary>
        /// <param name="vector2">待转换的 2D 向量</param>
        /// <param name="y">指定的 3D 向量 Y 轴值</param>
        /// <returns>转换后的 3D 向量</returns>
        public static Vector3 ToVector3(this Vector2 vector2, float y)
        {
            return new Vector3(vector2.x, y, vector2.y);
        }

        /// <summary>
        /// 屏幕 2D 坐标发射 2D 射线，返回第一个碰撞到的 Transform
        /// 适用于鼠标/触摸点击检测 2D 物体
        /// </summary>
        /// <param name="screenPos">屏幕坐标（如 Input.mousePosition）</param>
        /// <param name="camera">照射使用的相机（默认为 Camera.main）</param>
        /// <returns>碰撞到的 Transform，无碰撞返回 null</returns>
        /// <exception cref="System.NullReferenceException">camera 为 null 时抛出</exception>
        public static Transform GetRaycastHit2DTransform(this Vector2 screenPos, Camera camera)
        {
            if (camera == null) return null;

            Vector2 worldPoint = camera.ScreenToWorldPoint(screenPos);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            return hit ? hit.transform : null;
        }

        /// <summary>
        /// 屏幕 2D 坐标发射 2D 射线，返回所有碰撞到的 Transform 数组
        /// </summary>
        /// <param name="screenPos">屏幕坐标（如 Input.mousePosition）</param>
        /// <param name="camera">照射使用的相机</param>
        /// <returns>所有碰撞到的 Transform 数组</returns>
        public static Transform[] GetRaycastHit2DAllTransforms(this Vector2 screenPos, Camera camera)
        {
            if (camera == null) return new Transform[0];

            Vector2 worldPoint = camera.ScreenToWorldPoint(screenPos);
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            return hits
                .Where(hit => hit.transform != null)
                .Select(hit => hit.transform)
                .ToArray();
        }

        /// <summary>
        /// 屏幕两点之间执行 2D 线段检测（Linecast），返回第一个碰撞物体
        /// </summary>
        /// <param name="startScreenPos">起点屏幕坐标</param>
        /// <param name="endScreenPos">终点屏幕坐标</param>
        /// <param name="camera">照射使用的相机</param>
        /// <returns>碰撞到的 Transform，无碰撞返回 null</returns>
        public static Transform GetLinecastHit2DTransform(
            this Vector2 startScreenPos,
            Vector2 endScreenPos,
            Camera camera)
        {
            if (camera == null) return null;

            Vector2 startWorld = camera.ScreenToWorldPoint(startScreenPos);
            Vector2 endWorld = camera.ScreenToWorldPoint(endScreenPos);
            RaycastHit2D hit = Physics2D.Linecast(startWorld, endWorld);

            return hit ? hit.transform : null;
        }

        /// <summary>
        /// 屏幕两点之间执行 2D 线段检测，返回所有碰撞物体
        /// </summary>
        /// <param name="startScreenPos">起点屏幕坐标</param>
        /// <param name="endScreenPos">终点屏幕坐标</param>
        /// <param name="camera">照射使用的相机</param>
        /// <returns>所有碰撞到的 Transform 数组</returns>
        public static Transform[] GetLinecastHit2DAllTransforms(
            this Vector2 startScreenPos,
            Vector2 endScreenPos,
            Camera camera)
        {
            if (camera == null) return new Transform[0];

            Vector2 startWorld = camera.ScreenToWorldPoint(startScreenPos);
            Vector2 endWorld = camera.ScreenToWorldPoint(endScreenPos);
            RaycastHit2D[] hits = Physics2D.LinecastAll(startWorld, endWorld);

            return hits
                .Where(hit => hit.transform != null)
                .Select(hit => hit.transform)
                .ToArray();
        }

        /// <summary>
        /// 屏幕两点之间执行 2D 球形投射检测（CircleCast），返回第一个碰撞物体
        /// </summary>
        /// <param name="startScreenPos">起点屏幕坐标</param>
        /// <param name="radius">球形检测半径</param>
        /// <param name="endScreenPos">终点屏幕坐标</param>
        /// <param name="camera">照射使用的相机</param>
        /// <returns>碰撞到的 Transform，无碰撞返回 null</returns>
        public static Transform GetCircleCastHit2DTransform(
            this Vector2 startScreenPos,
            float radius,
            Vector2 endScreenPos,
            Camera camera)
        {
            if (camera == null) return null;

            Vector2 startWorld = camera.ScreenToWorldPoint(startScreenPos);
            Vector2 endWorld = camera.ScreenToWorldPoint(endScreenPos);
            Vector2 direction = endWorld - startWorld;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.CircleCast(startWorld, radius, direction.normalized, distance);

            return hit ? hit.transform : null;
        }

        /// <summary>
        /// 屏幕两点之间执行 2D 球形投射检测，返回所有碰撞物体
        /// </summary>
        /// <param name="startScreenPos">起点屏幕坐标坐标</param>
        /// <param name="radius">球形检测半径</param>
        /// <param name="endScreenPos">终点屏幕坐标</param>
        /// <param name="camera">照射使用的相机</param>
        /// <returns>所有碰撞到的 Transform 数组</returns>
        public static Transform[] GetCircleCastHit2DAllTransforms(
            this Vector2 startScreenPos,
            float radius,
            Vector2 endScreenPos,
            Camera camera)
        {
            if (camera == null) return new Transform[0];

            Vector2 startWorld = camera.ScreenToWorldPoint(startScreenPos);
            Vector2 endWorld = camera.ScreenToWorldPoint(endScreenPos);
            Vector2 direction = endWorld - startWorld;
            float distance = direction.magnitude;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(startWorld, radius, direction.normalized, distance);

            return hits
                .Where(hit => hit.transform != null)
                .Select(hit => hit.transform)
                .ToArray();
        }
    }
}