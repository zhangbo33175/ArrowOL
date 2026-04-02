using System;
using UnityEngine;

namespace Honor.Runtime
{
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="component">目标对象上的任意组件。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return GetOrAddComponent<T>(component.gameObject);
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <param name="component">目标对象上的任意组件。</param>
        /// <param name="type">要获取或增加的组件类型。</param>
        /// <returns>获取或增加的组件。</returns>
        public static Component GetOrAddComponent(this Component component, Type type)
        {
            return GetOrAddComponent(component.gameObject, type);
        }

        /// <summary>
        /// 获取LuaBehaviour组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component GetLua(this Component component, string luaOnComponentName)
        {
            return GetLua(component.gameObject, luaOnComponentName);
        }

        /// <summary>
        /// 获取父对象上的LuaBehaviour组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component GetLuaInParent(this Component component, string luaOnComponentName)
        {
            return GetLuaInParent(component.gameObject, luaOnComponentName);
        }

        /// <summary>
        /// 获取孩子对象上的LuaBehaviour组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component GetLuaInChildren(this Component component, string luaOnComponentName)
        {
            return GetLuaInChildren(component.gameObject, luaOnComponentName);
        }

        /// <summary>
        /// 获取孩子对象上的LuaBehaviour组件集合
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaOnComponentName">LuaBehaviour组件上挂靠的lua脚本名称</param>
        /// <returns></returns>
        public static Component[] GetLuasInChildren(this Component component, string luaOnComponentName)
        {
            return GetLuasInChildren(component.gameObject, luaOnComponentName);
        }

        /// <summary>
        /// 获取RectTransform组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <returns></returns>
        public static RectTransform rectTransform(this Component component)
        {
            return component.transform as RectTransform;
        }

    }


}


