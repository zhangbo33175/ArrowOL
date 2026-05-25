/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ObjectCommon.cs
 * author:    云毅
 * created:   2026
 * descrip:   Unity 对象通用工具类
 *            提供查找子节点、设置父子关系、空值判断、获取子物体等通用功能
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// Unity 对象通用工具类
    /// 提供查找子节点、设置父子关系、空值判断等常用功能
    /// </summary>
    public static class ObjectCommon
    {
        #region 子物体查找
        //=========================================================================
        // 子物体查找
        //=========================================================================
        /// <summary>
        /// 查找 GameObject 的指定子物体（支持路径查找）
        /// </summary>
        /// <param name="go">父物体</param>
        /// <param name="childName">子物体名称 / 层级路径（如 Body/Head）</param>
        /// <returns>找到的子物体，找不到返回 null</returns>
        public static GameObject GetChild(GameObject go, string childName)
        {
            if (go == null || string.IsNullOrEmpty(childName))
                return null;

            Transform childTransform = go.transform.Find(childName);
            return childTransform != null ? childTransform.gameObject : null;
        }
        #endregion

        #region 父子关系设置
        //=========================================================================
        // 父子关系设置
        //=========================================================================
        /// <summary>
        /// 设置子物体，并重置其 Transform（位置、旋转、缩放归零）
        /// </summary>
        /// <param name="parent">父物体</param>
        /// <param name="child">子物体</param>
        public static void SetChild(GameObject parent, GameObject child)
        {
            if (parent == null || child == null)
                return;

            child.SetActive(true);
            child.transform.SetParent(parent.transform);
            child.transform.localScale = Vector3.one;
            child.transform.localEulerAngles = Vector3.zero;
            child.transform.localPosition = Vector3.zero;
        }
        #endregion

        #region 子物体获取
        //=========================================================================
        // 子物体获取
        //=========================================================================
        /// <summary>
        /// 获取物体所有子节点的 Transform 组件
        /// </summary>
        /// <param name="go">目标物体</param>
        /// <param name="includeInactive">是否包含未激活物体</param>
        /// <returns>Transform 数组</returns>
        public static Transform[] GetTransformsInChildren(GameObject go, bool includeInactive)
        {
            if (go == null)
                return new Transform[0];

            return go.GetComponentsInChildren<Transform>(includeInactive);
        }
        #endregion

        #region 空值判断
        //=========================================================================
        // 空值判断
        //=========================================================================
        /// <summary>
        /// 判断Unity对象是否为兼容Destroy后的对象
        /// </summary>
        /// <param name="obj">Unity 对象</param>
        /// <returns>true=空，false=有效</returns>
        public static bool IsObjectEmpty(Object obj)
        {
            return obj == null || obj.Equals(null);
        }
        #endregion
    }
}