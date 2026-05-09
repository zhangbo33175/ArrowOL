using System.Collections.Generic;

namespace Honor.Editor
{
    /// <summary>
    /// 【类型反射工具类】
    /// 功能：通过反射，自动查找指定程序集中 **某个基类的所有非抽象子类**
    /// 用途：编辑器工具自动检索、自动注册、自动扩展（如编辑器菜单、配置、导出工具）
    /// 属于：框架 -> 编辑器核心 -> 反射工具
    /// </summary>
    internal static class AorType
    {
        /// <summary>
        /// 【运行时程序集列表】
        /// 存放游戏核心逻辑的程序集（正式运行时）
        /// </summary>
        private static readonly string[] AssemblyNames =
        {
            "Honor.Runtime", // 框架运行时程序集
            "Assembly-CSharp" // Unity 默认 C# 主程序集
        };

        /// <summary>
        /// 【编辑器程序集列表】
        /// 存放编辑器工具、扩展、菜单的程序集
        /// </summary>
        private static readonly string[] EditorAssemblyNames =
        {
            "Honor.Editor", // 框架编辑器程序集
            "Assembly-CSharp-Editor" // Unity 默认编辑器程序集
        };

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

            // 遍历所有程序集
            foreach (string assemblyName in assemblyNames)
            {
                System.Reflection.Assembly assembly = null;
                try
                {
                    // 加载程序集
                    assembly = System.Reflection.Assembly.Load(assemblyName);
                }
                catch
                {
                    // 加载失败则跳过（程序集不存在）
                    continue;
                }

                if (assembly == null) continue;

                // 获取程序集中所有类型
                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    // 筛选条件：
                    // 1. 是类
                    // 2. 不是抽象类
                    // 3. 是基类的子类
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            // 排序并返回
            typeNames.Sort();
            return typeNames.ToArray();
        }
    }
}