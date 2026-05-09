using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 程序集反射工具类（静态）
    /// 功能：缓存所有程序集、快速获取类型、全局类型缓存，避免频繁反射造成性能消耗
    /// 用于框架内部自动查找类、创建实例、获取组件等核心反射操作
    /// </summary>
    public static class Assembly
    {
        /// <summary>
        /// 全局缓存的所有程序集（静态构造时初始化）
        /// </summary>
        private static readonly System.Reflection.Assembly[] s_Assemblies = null;

        /// <summary>
        /// 类型缓存字典（全名 → Type）
        /// 避免重复从程序集遍历查找类型，大幅提升反射性能
        /// </summary>
        private static readonly Dictionary<string, Type> s_CachedTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 静态构造函数
        /// 程序启动时自动获取所有已加载的程序集并缓存
        /// </summary>
        static Assembly()
        {
            s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// 获取所有已加载的程序集
        /// </summary>
        public static System.Reflection.Assembly[] GetAssemblies()
        {
            return s_Assemblies;
        }

        /// <summary>
        /// 获取所有程序集中的所有类型
        /// </summary>
        public static Type[] GetTypes()
        {
            List<Type> results = new List<Type>();
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }
            return results.ToArray();
        }

        /// <summary>
        /// 获取所有程序集中的所有类型（List 重载，减少 GC）
        /// </summary>
        /// <param name="results">接收类型结果的列表</param>
        public static void GetTypes(List<Type> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            results.Clear();
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }
        }

        /// <summary>
        /// 根据类型全名获取 Type（带缓存，高性能）
        /// 先查缓存 → 再直接获取 → 最后遍历所有程序集查找
        /// </summary>
        /// <param name="typeFullName">类型全名（包含命名空间）</param>
        /// <returns>查找到的 Type，找不到返回 null</returns>
        public static Type GetType(string typeFullName)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                throw new GameException("Type fullName 无效。");
            }

            // 1. 优先从缓存获取
            if (s_CachedTypes.TryGetValue(typeFullName, out Type type))
            {
                return type;
            }

            // 2. 尝试直接获取
            type = Type.GetType(typeFullName);
            if (type != null)
            {
                s_CachedTypes.Add(typeFullName, type);
                return type;
            }

            // 3. 遍历所有程序集尝试加载
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                type = Type.GetType(AorTxt.Format("{0}, {1}", typeFullName, assembly.FullName));
                if (type != null)
                {
                    s_CachedTypes.Add(typeFullName, type);
                    return type;
                }
            }

            // 未找到
            return null;
        }
    }
}