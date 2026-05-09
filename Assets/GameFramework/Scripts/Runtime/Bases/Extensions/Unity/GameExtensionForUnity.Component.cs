using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Unity 组件/GameObject 通用扩展方法
    /// 提供：获取/添加组件、Lua脚本查找、RectTransform快捷访问等功能
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 获取或添加组件（不存在则自动添加）
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="component">目标对象上的任意组件</param>
        /// <returns>目标组件实例</returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return GetOrAddComponent<T>(component.gameObject);
        }

        /// <summary>
        /// 获取或添加组件（不存在则自动添加）
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="type">组件类型</param>
        /// <returns>组件实例</returns>
        public static Component GetOrAddComponent(this Component component, Type type)
        {
            return GetOrAddComponent(component.gameObject, type);
        }

        /// <summary>
        /// 获取对象上指定名称的 Lua 脚本组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaScriptName">Lua 脚本名称</param>
        /// <returns>Lua 组件</returns>
        public static Component GetLua(this Component component, string luaScriptName)
        {
            return GetLua(component.gameObject, luaScriptName);
        }

        /// <summary>
        /// 在父对象中查找指定名称的 Lua 脚本组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaScriptName">Lua 脚本名称</param>
        /// <returns>Lua 组件</returns>
        public static Component GetLuaInParent(this Component component, string luaScriptName)
        {
            return GetLuaInParent(component.gameObject, luaScriptName);
        }

        /// <summary>
        /// 在子对象中查找指定名称的 Lua 脚本组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaScriptName">Lua 脚本名称</param>
        /// <returns>Lua 组件</returns>
        public static Component GetLuaInChildren(this Component component, string luaScriptName)
        {
            return GetLuaInChildren(component.gameObject, luaScriptName);
        }

        /// <summary>
        /// 获取所有子对象中指定名称的 Lua 脚本组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <param name="luaScriptName">Lua 脚本名称</param>
        /// <returns>Lua 组件数组</returns>
        public static Component[] GetLuasInChildren(this Component component, string luaScriptName)
        {
            return GetLuasInChildren(component.gameObject, luaScriptName);
        }

        /// <summary>
        /// 快速获取 RectTransform 组件
        /// </summary>
        /// <param name="component">目标对象上的任意组件</param>
        /// <returns>RectTransform 实例</returns>
        public static RectTransform rectTransform(this Component component)
        {
            return component.transform as RectTransform;
        }
    }
}