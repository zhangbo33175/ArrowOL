/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorType.cs
 * author:    云毅
 * created:   2026
 * descrip:   Honor框架 编辑器类型反射工具类
 *            自动查找基类的所有非抽象子类，用于编辑器自动注册
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Editor
{
    #region 类型反射工具类
    /// <summary>
    /// 【类型反射工具类】
    /// 功能：通过反射，自动查找指定程序集中 **某个基类的所有非抽象子类**
    /// 用途：编辑器工具自动检索、自动注册、自动扩展（如编辑器菜单、配置、导出工具）
    /// 属于：框架 -> 编辑器核心 -> 反射工具
    /// </summary>
    internal static class AorType
    {
        #region 程序集定义
        /// <summary>
        /// 【运行时程序集列表】
        /// 存放游戏核心逻辑的程序集（正式运行时）
        /// </summary>
        private static readonly string[] AssemblyNames =
        {
            "Honor.Runtime",
            "Assembly-CSharp"
        };

        /// <summary>
        /// 【编辑器程序集列表】
        /// 存放编辑器工具、扩展、菜单的程序集
        /// </summary>
        private static readonly string[] EditorAssemblyNames =
        {
            "Honor.Editor",
            "Assembly-CSharp-Editor"
        };
        #endregion

        #region 公共获取方法
        /// <summary>
        /// 【获取运行时子类名称】
        /// 在游戏运行时程序集中，查找指定基类的所有非抽象子类
        /// </summary>
        /// <param name="typeBase">基类 Type</param>
        /// <returns>子类全名数组</returns>
        internal static string[] GetTypeNames(System.Type typeBase)
        {
            return GetTypeNames(typeBase, AssemblyNames);
        }

        /// <summary>
        /// 【获取编辑器子类名称】
        /// 在编辑器程序集中，查找指定基类的所有非抽象子类
        /// </summary>
        /// <param name="typeBase">基类 Type</param>
        /// <returns>子类全名数组</returns>
        internal static string[] GetEditorTypeNames(System.Type typeBase)
        {
            return GetTypeNames(typeBase, EditorAssemblyNames);
        }
        #endregion

        #region 核心反射实现
        /// <summary>
        /// 【核心反射方法】
        /// 从指定程序集列表中，查找基类的所有非抽象子类，并返回全名
        /// </summary>
        /// <param name="typeBase">要查找的基类</param>
        /// <param name="assemblyNames">程序集名称数组</param>
        /// <returns>排序后的子类全名数组</returns>
        private static string[] GetTypeNames(System.Type typeBase, string[] assemblyNames)
        {
            List<string> typeNames = new List<string>();

            foreach (string assemblyName in assemblyNames)
            {
                System.Reflection.Assembly assembly = null;

                try
                {
                    assembly = System.Reflection.Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                    continue;

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }
        #endregion
    }
    #endregion
}