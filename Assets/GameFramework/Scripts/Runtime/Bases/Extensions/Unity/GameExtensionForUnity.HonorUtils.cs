/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  GameExtensionForUnity.HonorUtils.cs
 * author:  云毅
 * created:
 * descrip:   Unity 扩展方法补充 - 接口查找/组件查找/层级路径/Layer/滚动条提示
 * 优化记录: 由旧版 HonorUtils GameObjectExtends/TransformExtends/UGUIExtends 迁移，
 *           与框架既有 GameExtensionForUnity.* 共存，避免重复方法名
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// Unity 对象扩展方法（补充批次）
    /// 功能：接口查找、组件查找（含父子级回退）、层级路径、递归 Layer、滚动条提示箭头
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        //=========================================================================
        // GameObject - 接口查找
        //=========================================================================
        /// <summary>
        /// 获取当前 GameObject 上挂载的指定接口（第一个）
        /// </summary>
        public static T GetInterface<T>(this GameObject gameObject) where T : class
        {
            if (!typeof(T).IsInterface)
            {
                return null;
            }

            Component[] components = gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T)
                {
                    return components[i] as T;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取当前 GameObject 上挂载的指定接口集合
        /// </summary>
        public static T[] GetInterfaces<T>(this GameObject gameObject) where T : class
        {
            if (!typeof(T).IsInterface)
            {
                return null;
            }

            Component[] components = gameObject.GetComponents<Component>();
            if (components.Length == 0)
            {
                return null;
            }

            List<T> result = new List<T>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T)
                {
                    result.Add(components[i] as T);
                }
            }
            return result.Count > 0 ? result.ToArray() : null;
        }

        /// <summary>
        /// 获取当前 GameObject 及所有子物体上挂载的指定接口集合
        /// </summary>
        public static T[] GetInterfacesInChildren<T>(this GameObject gameObject) where T : class
        {
            if (!typeof(T).IsInterface)
            {
                return null;
            }

            Component[] components = gameObject.GetComponentsInChildren<Component>();
            if (components.Length == 0)
            {
                return null;
            }

            List<T> result = new List<T>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T)
                {
                    result.Add(components[i] as T);
                }
            }
            return result.Count > 0 ? result.ToArray() : null;
        }

        //=========================================================================
        // GameObject / Transform - 组件查找
        //=========================================================================
        /// <summary>
        /// 查找组件：自身找不到时回退到子物体
        /// </summary>
        public static T FindComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInChildren<T>();
            }
            return component;
        }

        /// <summary>
        /// 查找组件：自身 → 子物体 → 父物体 逐级回退
        /// </summary>
        public static T FindComponentIncParent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInChildren<T>();
            }
            if (component == null)
            {
                component = gameObject.GetComponentInParent<T>();
            }
            return component;
        }

        /// <summary>
        /// 查找或创建组件：自身 → 子物体 → 父物体，全找不到时在自身创建
        /// </summary>
        public static T FindOrCreateComponentIncParent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInChildren<T>();
            }
            if (component == null)
            {
                component = gameObject.GetComponentInParent<T>();
            }
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 查找或创建组件：自身 → 子物体，全找不到时在自身创建
        /// </summary>
        public static T FindOrCreateComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInChildren<T>();
            }
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 按指定 Layer 筛选 GetComponentsInChildren 结果
        /// </summary>
        public static T[] GetComponentsInChildren<T>(this GameObject gameObject, int layer) where T : Component
        {
            List<T> result = new List<T>();
            T[] components = gameObject.GetComponentsInChildren<T>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].gameObject.layer == layer)
                {
                    result.Add(components[i]);
                }
            }
            return result.ToArray();
        }

        //=========================================================================
        // Transform - 节点遍历
        //=========================================================================
        /// <summary>
        /// 从根节点开始按节点顺序返回自身及所有子节点的 Component&lt;T&gt;
        /// </summary>
        public static List<T> FindAllComponentsInOrder<T>(this Transform transform) where T : Component
        {
            List<T> list = new List<T>();
            T rootComponent = transform.root.GetComponent<T>();
            if (rootComponent != null)
            {
                list.Add(rootComponent);
            }

            FindAllComponentsLoop<T>(transform.root, list);
            return list.Count > 0 ? list : null;
        }

        /// <summary>
        /// 按节点顺序返回所有子节点的 Component&lt;T&gt;（不包含自身）
        /// </summary>
        public static List<T> FindAllComponentsInChildrenInOrder<T>(this Transform transform) where T : Component
        {
            List<T> list = new List<T>();
            FindAllComponentsLoop<T>(transform, list);
            return list.Count > 0 ? list : null;
        }

        /// <summary>深度优先遍历收集组件</summary>
        private static void FindAllComponentsLoop<T>(Transform transform, List<T> list) where T : Component
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = transform.GetChild(i);
                T component = child.GetComponent<T>();
                if (component != null)
                {
                    list.Add(component);
                }
                if (child.childCount > 0)
                {
                    FindAllComponentsLoop<T>(child, list);
                }
            }
        }

        /// <summary>
        /// 获取对象在 Hierarchy 中的完整节点路径
        /// </summary>
        public static string GetHierarchyPath(this Transform transform)
        {
            return GetHierarchyPathLoop(transform);
        }

        /// <summary>递归拼接路径</summary>
        private static string GetHierarchyPathLoop(Transform transform, string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = transform.gameObject.name;
            }
            else
            {
                path = transform.gameObject.name + "/" + path;
            }

            return transform.parent != null ? GetHierarchyPathLoop(transform.parent, path) : path;
        }

        //=========================================================================
        // Transform - Layer 递归设置
        //=========================================================================
        /// <summary>
        /// 递归设置 Layer（可跳过指定 Layer 的对象）
        /// </summary>
        /// <param name="transform">目标节点</param>
        /// <param name="layer">目标 Layer</param>
        /// <param name="ignoreLayer">需要跳过的 Layer，默认 int.MinValue 表示不过滤</param>
        public static void SetLayer(this Transform transform, int layer, int ignoreLayer = int.MinValue)
        {
            if (transform.gameObject.layer != ignoreLayer)
            {
                transform.gameObject.layer = layer;
            }

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                SetLayer(transform.GetChild(i), layer, ignoreLayer);
            }
        }

        /// <summary>
        /// 递归设置 Layer 并保存原 Layer 到字典（供 SetLayerByDic 还原）
        /// </summary>
        /// <param name="transform">目标节点</param>
        /// <param name="layer">目标 Layer</param>
        /// <param name="layerDic">以 GetHashCode 为键缓存原 Layer 的字典</param>
        public static void SetLayerAndSaveDic(this Transform transform, int layer, Dictionary<int, int> layerDic)
        {
            if (layerDic != null && !layerDic.ContainsKey(transform.GetHashCode()))
            {
                layerDic.Add(transform.GetHashCode(), transform.gameObject.layer);
            }

            transform.gameObject.layer = layer;
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                SetLayerAndSaveDic(transform.GetChild(i), layer, layerDic);
            }
        }

        /// <summary>
        /// 根据字典递归还原 Layer
        /// </summary>
        public static void SetLayerByDic(this Transform transform, Dictionary<int, int> layerDic)
        {
            int hashCode = transform.GetHashCode();
            if (layerDic != null && layerDic.ContainsKey(hashCode))
            {
                transform.gameObject.layer = layerDic[hashCode];
            }

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                SetLayerByDic(transform.GetChild(i), layerDic);
            }
        }

        //=========================================================================
        // UGUI - 滚动条提示箭头
        //=========================================================================
        /// <summary>
        /// 设置纵向滚动条上下提示箭头（到达顶部/底部自动隐藏）
        /// </summary>
        /// <param name="scrollRect">滚动条</param>
        /// <param name="up">顶部提示</param>
        /// <param name="down">底部提示</param>
        public static void SetScrollTipsByY(this ScrollRect scrollRect, GameObject up = null, GameObject down = null)
        {
            scrollRect.onValueChanged.AddListener((Vector2 value) =>
            {
                float normalized = value.y;
                if (up != null)
                {
                    // 未到顶部时显示，到达顶部（1）隐藏
                    up.SetActive(normalized < 0.99f);
                }

                if (down != null)
                {
                    // 未到底部时显示，到底部（0）隐藏
                    down.SetActive(normalized >= 0.01f);
                }
            });
        }

        /// <summary>
        /// 设置横向滚动条左右提示箭头（到达最左/最右自动隐藏）
        /// </summary>
        /// <param name="scrollRect">滚动条</param>
        /// <param name="left">左侧提示</param>
        /// <param name="right">右侧提示</param>
        public static void SetScrollTipsByX(this ScrollRect scrollRect, GameObject left = null, GameObject right = null)
        {
            scrollRect.onValueChanged.AddListener((Vector2 value) =>
            {
                float normalized = value.x;
                if (right != null)
                {
                    // 未到最右时显示，到达最右（1）隐藏
                    right.SetActive(normalized < 0.99f);
                }

                if (left != null)
                {
                    // 未到最左时显示，到达最左（0）隐藏
                    left.SetActive(normalized >= 0.01f);
                }
            });
        }
    }
}
