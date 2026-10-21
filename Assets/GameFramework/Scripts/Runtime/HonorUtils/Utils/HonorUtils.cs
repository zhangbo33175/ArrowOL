/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUtils.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局公共工具类
 *            收纳跨模块重复出现、可参数化的纯静态工具方法，
 *            供各业务管理器 / 组件复用，避免重复代码在各处复制一份
 ***************************************************************/

namespace Honor.Runtime
{
    //=========================================================================
    // 游戏全局公共工具类
    //=========================================================================
    /// <summary>
    /// 游戏全局公共工具类（静态）
    /// <para>作用：集中收纳跨模块重复出现、可参数化的纯逻辑工具方法，
    /// 使各业务类保持精简，公共能力单点维护。</para>
    /// </summary>
    public static class HonorUtils
    {
        #region 路径工具

        /// <summary>
        /// 规范化路径分隔符：将反斜杠 '\' 统一替换为斜杠 '/'
        /// <para>解决 Windows / 资源路径混用分隔符导致的匹配失败问题；
        /// 空值或空字符串直接原样返回。</para>
        /// </summary>
        /// <param name="path">原始路径</param>
        /// <returns>统一使用 '/' 分隔的路径；入参为 null 或空时返回原值</returns>
        public static string NormalizePath(string path)
        {
            return string.IsNullOrEmpty(path) ? path : path.Replace('\\', '/');
        }

        #endregion
    }
}
