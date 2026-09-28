/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  HideEffect.cs
 * author:  云毅
 * created:
 * descrip:   隐藏特效辅助 - 按名称隐藏/关闭特效节点中的指定子物体
 * 优化记录: 由旧版 HonorGraphics.HideEffect 迁移，统一命名空间
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 隐藏特效辅助
    /// 功能：隐藏对象及其子节点中名称匹配指定关键字的节点
    /// 典型用途：残影/分身特效中屏蔽粒子效果，只保留模型
    /// </summary>
    public static class HideEffect
    {
        /// <summary>
        /// 隐藏对象及子节点中名称含关键字的所有节点
        /// </summary>
        /// <param name="root">根对象</param>
        /// <param name="keyword">名称关键字（大小写敏感）</param>
        public static void Hide(GameObject root, string keyword)
        {
            if (root == null || string.IsNullOrEmpty(keyword))
            {
                return;
            }

            if (root.name.Contains(keyword))
            {
                root.SetActive(false);
            }

            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name.Contains(keyword))
                {
                    children[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 隐藏"EffectView"节点（残影特效专用）
        /// </summary>
        public static void HideEffectView(GameObject root)
        {
            Hide(root, "EffectView");
        }
    }
}
