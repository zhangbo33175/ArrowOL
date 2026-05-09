using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// GameObject 通用扩展方法
    /// 包含：安全获取组件、无GC获取组件、Lua脚本查找、层级设置、粒子排序等
    /// 全框架最核心的工具扩展类
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 静态缓存列表，用于无GC获取组件 / 变换
        /// </summary>
        private static readonly List<Transform> s_CachedTransforms = new List<Transform>();

        private static readonly List<Component> s_CachedComponents = new List<Component>();

        /// <summary>
        /// 无GC获取组件（避免GC分配，高频调用安全）
        /// </summary>
        public static Component GetComponentNoAlloc(this GameObject gameObject, Type type)
        {
            if (gameObject == null || type == null)
                return null;

            gameObject.GetComponents(type, s_CachedComponents);
            Component comp = s_CachedComponents.Count > 0 ? s_CachedComponents[0] : null;
            s_CachedComponents.Clear();
            return comp;
        }

        /// <summary>
        /// 无GC获取泛型组件（避免GC分配，高频调用安全）
        /// </summary>
        public static T GetComponentNoAlloc<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject == null)
                return null;

            gameObject.GetComponents(typeof(T), s_CachedComponents);
            Component comp = s_CachedComponents.Count > 0 ? s_CachedComponents[0] : null;
            s_CachedComponents.Clear();
            return comp as T;
        }

        /// <summary>
        /// 优先从自身/子物体/父物体找组件，找不到则添加到自身
        /// </summary>
        public static T GetComponentAroundOrAdd<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject == null)
                return null;

            T comp = gameObject.GetComponentInChildren<T>(true);
            if (comp == null)
                comp = gameObject.GetComponentInParent<T>();
            if (comp == null)
                comp = gameObject.AddComponent<T>();

            return comp;
        }

        /// <summary>
        /// 获取或添加组件（不存在则自动Add）
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject == null)
                return null;

            T comp = gameObject.GetComponent<T>();
            if (comp == null)
                comp = gameObject.AddComponent<T>();

            return comp;
        }

        /// <summary>
        /// 获取或添加组件（Type版）
        /// </summary>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            if (gameObject == null || type == null)
                return null;

            Component comp = gameObject.GetComponent(type);
            if (comp == null)
                comp = gameObject.AddComponent(type);

            return comp;
        }

        /// <summary>
        /// 获取挂载了指定Lua脚本的LuaBehaviour
        /// </summary>
        public static Component GetLua(this GameObject gameObject, string luaScriptName)
        {
            if (gameObject == null || string.IsNullOrEmpty(luaScriptName))
                return null;

            LuaBehaviour luaComp = gameObject.GetComponent<LuaBehaviour>();
            if (luaComp != null && luaComp.LuaScriptNames.Contains(luaScriptName))
                return luaComp;

            return null;
        }

        /// <summary>
        /// 在父物体中查找指定Lua脚本的LuaBehaviour
        /// </summary>
        public static Component GetLuaInParent(this GameObject gameObject, string luaScriptName)
        {
            if (gameObject == null || string.IsNullOrEmpty(luaScriptName))
                return null;

            LuaBehaviour luaComp = gameObject.GetComponentInParent<LuaBehaviour>();
            if (luaComp != null && luaComp.LuaScriptNames.Contains(luaScriptName))
                return luaComp;

            return null;
        }

        /// <summary>
        /// 在子物体中查找指定Lua脚本的LuaBehaviour
        /// </summary>
        public static Component GetLuaInChildren(this GameObject gameObject, string luaScriptName)
        {
            if (gameObject == null || string.IsNullOrEmpty(luaScriptName))
                return null;

            LuaBehaviour luaComp = gameObject.GetComponentInChildren<LuaBehaviour>();
            if (luaComp != null && luaComp.LuaScriptNames.Contains(luaScriptName))
                return luaComp;

            return null;
        }

        /// <summary>
        /// 获取所有子物体中包含指定Lua脚本的LuaBehaviour数组
        /// </summary>
        public static Component[] GetLuasInChildren(this GameObject gameObject, string luaScriptName)
        {
            if (gameObject == null || string.IsNullOrEmpty(luaScriptName))
                return Array.Empty<Component>();

            LuaBehaviour[] comps = gameObject.GetComponentsInChildren<LuaBehaviour>();
            List<LuaBehaviour> results = new List<LuaBehaviour>();

            foreach (var luaComp in comps)
            {
                if (luaComp != null && luaComp.LuaScriptNames.Contains(luaScriptName))
                    results.Add(luaComp);
            }

            return results.ToArray();
        }

        /// <summary>
        /// 判断物体是否在场景中（不是Prefab）
        /// </summary>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject != null && gameObject.scene.name != null;
        }

        /// <summary>
        /// 递归设置物体及所有子物体的Layer（无GC）
        /// </summary>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            if (gameObject == null)
                return;

            gameObject.GetComponentsInChildren(true, s_CachedTransforms);
            foreach (var trans in s_CachedTransforms)
            {
                trans.gameObject.layer = layer;
            }

            s_CachedTransforms.Clear();
        }

        /// <summary>
        /// 快捷获取RectTransform
        /// </summary>
        public static RectTransform rectTransform(this GameObject gameObject)
        {
            return gameObject != null ? gameObject.transform as RectTransform : null;
        }

        /// <summary>
        /// 设置粒子特效的SortingOrder（支持保持原有顺序）
        /// </summary>
        public static void SetParticleSortOrder(this GameObject gameObject, int sortOrder, bool keepOriginalOrder)
        {
            if (gameObject == null)
                return;

            ParticleSystemRenderer[] renderers = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();

            if (keepOriginalOrder)
            {
                List<ParticleSystemRenderer> sortedList = new List<ParticleSystemRenderer>(renderers);
                sortedList.Sort((a, b) => a.sortingOrder.CompareTo(b.sortingOrder));

                int lastOrder = 0;
                int currentOrder = sortOrder;

                foreach (var renderer in sortedList)
                {
                    if (lastOrder == 0 || renderer.sortingOrder != lastOrder)
                    {
                        lastOrder = renderer.sortingOrder;
                        currentOrder++;
                    }

                    renderer.sortingOrder = currentOrder;
                }
            }
            else
            {
                foreach (var renderer in renderers)
                {
                    renderer.sortingOrder = sortOrder;
                }
            }
        }
    }
}