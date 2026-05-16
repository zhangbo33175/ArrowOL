/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameExtensionForUnity.Transform.cs
 * author:    云毅
 * created:   2025
 * descrip:   Transform 扩展方法 - 位置/旋转/缩放/层级路径操作
 ***************************************************************/
using UnityEngine;


namespace Honor.Runtime
{
    /// <summary>
    /// Unity Transform 组件扩展方法工具类
    /// <para>提供位置、缩放、旋转、层级路径等常用便捷操作</para>
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        #region Transform 世界坐标操作
        //=========================================================================
        // 世界坐标 X/Y/Z 单轴设置与增量修改
        //=========================================================================
        /// <summary>
        /// 设置物体世界坐标的 X 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的 X 坐标值</param>
        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 position = transform.position;
            position.x = newValue;
            transform.position = position;
        }

        /// <summary>
        /// 设置物体世界坐标的 Y 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的 Y 坐标值</param>
        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 position = transform.position;
            position.y = newValue;
            transform.position = position;
        }

        /// <summary>
        /// 设置物体世界坐标的 Z 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的 Z 坐标值</param>
        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 position = transform.position;
            position.z = newValue;
            transform.position = position;
        }

        /// <summary>
        /// 增量修改物体世界坐标的 X 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">X 坐标增量值</param>
        public static void AddPositionX(this Transform transform, float deltaValue)
        {
            Vector3 position = transform.position;
            position.x += deltaValue;
            transform.position = position;
        }

        /// <summary>
        /// 增量修改物体世界坐标的 Y 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">Y 坐标增量值</param>
        public static void AddPositionY(this Transform transform, float deltaValue)
        {
            Vector3 position = transform.position;
            position.y += deltaValue;
            transform.position = position;
        }

        /// <summary>
        /// 增量修改物体世界坐标的 Z 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">Z 坐标增量值</param>
        public static void AddPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 position = transform.position;
            position.z += deltaValue;
            transform.position = position;
        }
        #endregion

        #region Transform 局部坐标操作
        //=========================================================================
        // 局部坐标 X/Y/Z 单轴设置与增量修改
        //=========================================================================
        /// <summary>
        /// 设置物体局部坐标（相对父物体）的 X 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的局部 X 坐标值</param>
        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.x = newValue;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// 设置物体局部坐标（相对父物体）的 Y 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的局部 Y 坐标值</param>
        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.y = newValue;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// 设置物体局部坐标（相对父物体）的 Z 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的局部 Z 坐标值</param>
        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.z = newValue;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// 增量修改物体局部坐标（相对父物体）的 X 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">局部 X 坐标增量值</param>
        public static void AddLocalPositionX(this Transform transform, float deltaValue)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.x += deltaValue;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// 增量修改物体局部坐标（相对父物体）的 Y 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">局部 Y 坐标增量值</param>
        public static void AddLocalPositionY(this Transform transform, float deltaValue)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.y += deltaValue;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// 增量修改物体局部坐标（相对父物体）的 Z 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">局部 Z 坐标增量值</param>
        public static void AddLocalPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.z += deltaValue;
            transform.localPosition = localPosition;
        }
        #endregion

        #region Transform 局部缩放操作
        //=========================================================================
        // 局部缩放 X/Y/Z 单轴设置与增量修改
        //=========================================================================
        /// <summary>
        /// 设置物体局部缩放的 X 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的局部缩放 X 值</param>
        public static void SetLocalScaleX(this Transform transform, float newValue)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = newValue;
            transform.localScale = localScale;
        }

        /// <summary>
        /// 设置物体局部缩放的 Y 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的局部缩放 Y 值</param>
        public static void SetLocalScaleY(this Transform transform, float newValue)
        {
            Vector3 localScale = transform.localScale;
            localScale.y = newValue;
            transform.localScale = localScale;
        }

        /// <summary>
        /// 设置物体局部缩放的 Z 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="newValue">新的局部缩放 Z 值</param>
        public static void SetLocalScaleZ(this Transform transform, float newValue)
        {
            Vector3 localScale = transform.localScale;
            localScale.z = newValue;
            transform.localScale = localScale;
        }

        /// <summary>
        /// 增量修改物体局部缩放的 X 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">局部缩放 X 增量值</param>
        public static void AddLocalScaleX(this Transform transform, float deltaValue)
        {
            Vector3 localScale = transform.localScale;
            localScale.x += deltaValue;
            transform.localScale = localScale;
        }

        /// <summary>
        /// 增量修改物体局部缩放的 Y 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">局部缩放 Y 增量值</param>
        public static void AddLocalScaleY(this Transform transform, float deltaValue)
        {
            Vector3 localScale = transform.localScale;
            localScale.y += deltaValue;
            transform.localScale = localScale;
        }

        /// <summary>
        /// 增量修改物体局部缩放的 Z 轴分量
        /// </summary>
        /// <param name="transform">目标 Transform 组件</param>
        /// <param name="deltaValue">局部缩放 Z 增量值</param>
        public static void AddLocalScaleZ(this Transform transform, float deltaValue)
        {
            Vector3 localScale = transform.localScale;
            localScale.z += deltaValue;
            transform.localScale = localScale;
        }
        #endregion

        #region Transform 2D朝向与旋转
        //=========================================================================
        // 2D 空间朝向与旋转操作
        //=========================================================================
        /// <summary>
        /// 2D 空间中使物体朝向目标点（基于世界坐标，修正原逻辑错误）
        /// <para>适用于 Top-Down / 平面 2D 游戏</para>
        /// </summary>
        /// <param name="transform">当前物体 Transform</param>
        /// <param name="lookAtPoint2D">目标 2D 坐标点</param>
        /// <remarks>旋转轴为 Z 轴（2D 标准朝向），忽略 Y 轴高度差异</remarks>
        public static void LookAt2D(this Transform transform, Vector2 lookAtPoint2D)
        {
            // 计算方向向量（世界空间）
            Vector3 direction = new Vector3(lookAtPoint2D.x, 0, lookAtPoint2D.y) - transform.position;
            direction.y = 0;

            // 方向有效时执行旋转
            if (direction.sqrMagnitude > Mathf.Epsilon)
            {
                // 2D 朝向：使用绕 Z 轴旋转，替代原错误的 LookRotation
                float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, -angle, 0);
            }
        }
        #endregion

        #region Transform 层级路径与深度
        //=========================================================================
        // 物体层级路径、父级数量获取
        //=========================================================================
        /// <summary>
        /// 获取物体从根节点到自身的完整层级路径
        /// </summary>
        /// <param name="transform">目标物体 Transform</param>
        /// <param name="splitter">路径分隔符，默认为 /</param>
        /// <returns>完整层级路径字符串</returns>
        public static string GetRoute(this Transform transform, string splitter = "/")
        {
            if (transform == null) return string.Empty;

            System.Text.StringBuilder pathBuilder = new System.Text.StringBuilder();
            pathBuilder.Append(transform.name);

            Transform parent = transform.parent;
            while (parent != null)
            {
                pathBuilder.Insert(0, $"{parent.name}{splitter}");
                parent = parent.parent;
            }

            return pathBuilder.ToString();
        }

        /// <summary>
        /// 获取物体的父级层级数量（根节点为 0 级）
        /// </summary>
        /// <param name="transform">目标物体 Transform</param>
        /// <returns>父级层级总数</returns>
        public static int GetRouteNum(this Transform transform)
        {
            if (transform == null) return 0;

            int layerCount = 0;
            Transform parent = transform.parent;

            while (parent != null)
            {
                layerCount++;
                parent = parent.parent;
            }

            return layerCount;
        }
        #endregion
    }
}