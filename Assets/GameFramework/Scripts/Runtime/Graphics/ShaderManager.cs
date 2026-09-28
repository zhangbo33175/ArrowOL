/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  ShaderManager.cs
 * author:  云毅
 * created:
 * descrip:   Shader 管理器 - 统一查找与缓存 Shader，避免运行时重复 Find
 * 优化记录: 由旧版 HonorGraphics.ShaderManager 迁移，统一命名空间，增加预热接口
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Shader 管理器
    /// 功能：按名称查找 Shader 并缓存，运行时只 Find 一次
    /// 注意：Shader.Find 在真机构建后可用；请确保引用的 Shader 被包含进构建（Always Included）
    /// </summary>
    public static class ShaderManager
    {
        #region 字段

        /// <summary>Shader 缓存表</summary>
        private static readonly Dictionary<string, Shader> m_ShaderCache = new Dictionary<string, Shader>();

        #endregion

        #region 查找

        /// <summary>
        /// 按名称查找 Shader（带缓存）
        /// </summary>
        /// <param name="shaderName">Shader 名称</param>
        /// <returns>Shader，未找到返回 null</returns>
        public static Shader Find(string shaderName)
        {
            if (string.IsNullOrEmpty(shaderName))
            {
                return null;
            }

            if (m_ShaderCache.TryGetValue(shaderName, out Shader shader))
            {
                return shader;
            }

            shader = Shader.Find(shaderName);
            if (shader != null)
            {
                m_ShaderCache.Add(shaderName, shader);
            }
            else
            {
                Log.Warning("ShaderManager.Find 未找到 Shader：{0}", shaderName);
            }

            return shader;
        }

        /// <summary>
        /// 预热缓存一组 Shader（启动时调用，避免首帧卡顿）
        /// </summary>
        /// <param name="shaderNames">Shader 名称列表</param>
        public static void Prewarm(IEnumerable<string> shaderNames)
        {
            if (shaderNames == null)
            {
                return;
            }

            foreach (string name in shaderNames)
            {
                Find(name);
            }
        }

        /// <summary>
        /// 清空 Shader 缓存（场景切换时可选调用）
        /// </summary>
        public static void ClearCache()
        {
            m_ShaderCache.Clear();
        }

        #endregion
    }
}
