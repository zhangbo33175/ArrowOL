using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public static partial class GameExtensionForUnity
    {
        private static readonly List<Transform> s_CachedTransforms = new List<Transform>();
        private static readonly List<Component> s_CachedComponents = new List<Component>();

        /// <summary>
        /// 获取组件（降低无效内存开销）
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <param name="type">要获取的组件类型。</param>
        /// <returns>获取的组件。</returns>
        public static Component GetComponentNoAlloc(this GameObject gameObject, System.Type type)
        {
            gameObject.GetComponents(type, s_CachedComponents);
            Component component = s_CachedComponents.Count > 0 ? s_CachedComponents[0] : null;
            s_CachedComponents.Clear();
            return component;
        }

        /// <summary>
        /// 获取组件（降低无效内存开销）
        /// </summary>
        /// <typeparam name="T">要获取的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取的组件。</returns>
        public static T GetComponentNoAlloc<T>(this GameObject gameObject) where T : Component
        {
            gameObject.GetComponents(typeof(T), s_CachedComponents);
            Component component = s_CachedComponents.Count > 0 ? s_CachedComponents[0] : null;
            s_CachedComponents.Clear();
            return component as T;
        }

        /// <summary>
        /// 获取对象/子对象/父对象上的组件，如果未找到，则将其添加到对象
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <typeparam name="T">要获取的组件。</typeparam>
        /// <returns>获取或增加的组件。</returns>
        public static T GetComponentAroundOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponentInChildren<T>(true);
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
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <param name="type">要获取或增加的组件类型。</param>
        /// <returns>获取或增加的组件。</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// 获取LuaBehaviour组件
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component GetLua(this GameObject gameObject, string luaOnComponentName)
        {
            LuaBehaviour component = gameObject.GetComponent<LuaBehaviour>();
            if (component)
            {
                if (component.LuaScriptNames.Contains(luaOnComponentName))
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取父对象上的LuaBehaviour组件
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component GetLuaInParent(this GameObject gameObject, string luaOnComponentName)
        {
            LuaBehaviour component = gameObject.GetComponentInParent<LuaBehaviour>();
            if (component)
            {
                if (component.LuaScriptNames.Contains(luaOnComponentName))
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取孩子对象上的LuaBehaviour组件
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component GetLuaInChildren(this GameObject gameObject, string luaOnComponentName)
        {
            LuaBehaviour component = gameObject.GetComponentInChildren<LuaBehaviour>();
            if (component != null)
            {
                if (component.LuaScriptNames.Contains(luaOnComponentName))
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取孩子对象上的LuaBehaviour组件集合
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component[] GetLuasInChildren(this GameObject gameObject, string luaOnComponentName)
        {
            LuaBehaviour[] components = gameObject.GetComponentsInChildren<LuaBehaviour>();
            List<LuaBehaviour> results = new List<LuaBehaviour>();
            for (int index = 0; index < components.Length; index++)
            {
                if (components[index])
                {
                    if (components[index].LuaScriptNames.Contains(luaOnComponentName))
                    {
                        results.Add(components[index]);
                    }
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取 GameObject 是否在场景中。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>GameObject 是否在场景中。</returns>
        /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject.scene.name != null;
        }

        /// <summary>
        /// 递归设置游戏对象的层次。
        /// </summary>
        /// <param name="gameObject"><see cref="GameObject" /> 对象。</param>
        /// <param name="layer">目标层次的编号。</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.GetComponentsInChildren(true, s_CachedTransforms);
            for (int i = 0; i < s_CachedTransforms.Count; i++)
            {
                s_CachedTransforms[i].gameObject.layer = layer;
            }

            s_CachedTransforms.Clear();
        }

        /// <summary>
        /// 获取RectTransform组件
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <returns></returns>
        public static RectTransform rectTransform(this GameObject gameObject)
        {
            return gameObject.transform as RectTransform;
        }

        /// <summary>
        /// 设置粒子特效的SortingOrder
        /// </summary>
        /// <param name="gameObject">目标对象</param>
        /// <param name="sortOrder">层级值</param>
        /// <param name="isSortParticle">是否排序粒子特效</param>
        public static void SetParticleSortOrder(this GameObject gameObject, int sortOrder, bool isSortParticle)
        {
            if (gameObject == null) return;

            ParticleSystemRenderer[] psRender = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();

            if (isSortParticle)
            {
                int lastSortingOrder = 0;
                int curSortingOrder = sortOrder;
                List<ParticleSystemRenderer> psRenderList = new List<ParticleSystemRenderer>(psRender);
                psRenderList.Sort((a, b) => a.sortingOrder.CompareTo(b.sortingOrder));

                for (int i = 0, len = psRenderList.Count; i < len; i++)
                {
                    if (lastSortingOrder == 0)
                    {
                        lastSortingOrder = psRenderList[i].sortingOrder;
                        psRenderList[i].sortingOrder = curSortingOrder;
                        continue;
                    }

                    if (psRenderList[i].sortingOrder == lastSortingOrder)
                    {
                        psRenderList[i].sortingOrder = curSortingOrder;
                        continue;
                    }

                    lastSortingOrder = psRenderList[i].sortingOrder;

                    ++curSortingOrder;
                    psRenderList[i].sortingOrder = curSortingOrder;
                }
            }
            else
            {
                for (int i = 0, len = psRender.Length; i < len; i++)
                {
                    psRender[i].sortingOrder = sortOrder;
                }
            }
        }
    }
}