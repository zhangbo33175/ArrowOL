/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  RTransformExtension.cs
 * author:    云毅
 * created:   2026
 * descrip:   Transform / EasyTouch BaseFinger 扩展方法
 *            提供坐标、增量坐标快速获取，简化代码调用
 ***************************************************************/

using HedgehogTeam.EasyTouch;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 坐标相关扩展方法
    /// 为 Transform 和 EasyTouch BaseFinger 提供快捷坐标获取
    /// </summary>
    public static class RTransformExtension
    {
        #region Transform 坐标扩展
        //=========================================================================
        // Transform 坐标扩展
        //=========================================================================
        /// <summary>
        /// 获取世界坐标 X
        /// </summary>
        public static float GetPositionX(this Transform transform)
        {
            return transform.position.x;
        }

        /// <summary>
        /// 获取世界坐标 Y
        /// </summary>
        public static float GetPositionY(this Transform transform)
        {
            return transform.position.y;
        }

        /// <summary>
        /// 获取世界坐标 Z
        /// </summary>
        public static float GetPositionZ(this Transform transform)
        {
            return transform.position.z;
        }
        #endregion

        #region EasyTouch BaseFinger 扩展
        //=========================================================================
        // EasyTouch BaseFinger 扩展
        //=========================================================================
        /// <summary>
        /// 获取手指位置 X
        /// </summary>
        public static float GetPositionX(this BaseFinger transform)
        {
            return transform.position.x;
        }

        /// <summary>
        /// 获取手指位置 Y
        /// </summary>
        public static float GetPositionY(this BaseFinger transform)
        {
            return transform.position.y;
        }

        /// <summary>
        /// 获取手指增量位置 X（偏移量）
        /// </summary>
        public static float GetDeltaPositionX(this BaseFinger finger)
        {
            return finger.deltaPosition.x;
        }

        /// <summary>
        /// 获取手指增量位置 Y（偏移量）
        /// </summary>
        public static float GetDeltaPositionY(this BaseFinger finger)
        {
            return finger.deltaPosition.y;
        }
        #endregion
    }
}