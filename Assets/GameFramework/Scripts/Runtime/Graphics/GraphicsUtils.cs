/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  GraphicsUtils.cs
 * author:  云毅
 * created:
 * descrip:   图形工具 - 设计分辨率/主运行标记/角色根节点查找
 * 优化记录: 由旧版 HonorGraphics.GraphicsUtils 迁移，统一命名空间，补齐屏幕信息刷新
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 图形工具
    /// 功能：设计分辨率常量、主运行标记（美术线程相关）、角色根节点查找
    /// </summary>
    public static class GraphicsUtils
    {
        #region 常量

        /// <summary>设计宽度（美术出图基准）</summary>
        public const float designWidth = 720f;

        /// <summary>设计高度（美术出图基准）</summary>
        public const float designHeight = 1280f;

        #endregion

        #region 主运行标记

        /// <summary>
        /// 是否主运行（美术特效依赖；真机与编辑器均为 true）
        /// </summary>
        public static bool IsMainRun
        {
            get { return true; }
        }

        #endregion

        #region 分辨率适配

        /// <summary>
        /// 屏幕分辨率类型
        /// </summary>
        public enum EScreenType
        {
            /// <summary>按宽度适配</summary>
            Width,

            /// <summary>按高度适配</summary>
            Height,
        }

        /// <summary>
        /// 计算实际分辨率与设计分辨率的比值
        /// </summary>
        public static float ScreenRatio(EScreenType type)
        {
            if (type == EScreenType.Width)
            {
                return Screen.width / designWidth;
            }

            return Screen.height / designHeight;
        }

        /// <summary>
        /// 刷新屏幕信息（分辨率/帧率变化后调用）
        /// </summary>
        public static void UpdateScreenInfoOnScreenChange()
        {
            // 预留：后续可在此统一处理多分辨率通知
            Log.Debug("GraphicsUtils.UpdateScreenInfoOnScreenChange {0}x{1}", Screen.width, Screen.height);
        }

        #endregion

        #region 角色根节点

        /// <summary>
        /// 从指定节点开始向父级查找指定 Layer 的根节点（角色根）
        /// </summary>
        /// <param name="trans">起始节点</param>
        /// <param name="layerMask">角色层 LayerMask</param>
        /// <returns>角色根 Transform，未找到返回 null</returns>
        public static Transform GetRoleRootTransform(Transform trans, LayerMask layerMask)
        {
            if (trans == null)
            {
                return null;
            }

            Transform cur = trans;
            while (cur != null)
            {
                if ((layerMask & (1 << cur.gameObject.layer)) != 0)
                {
                    return cur;
                }

                cur = cur.parent;
            }

            return null;
        }

        /// <summary>
        /// 查找所在角色根节点（使用 DefLayer.Role 层）
        /// </summary>
        public static Transform GetRoleRootTransform(Transform trans)
        {
            return GetRoleRootTransform(trans, LayerMask.GetMask(DefLayer.Role));
        }

        #endregion
    }
}
