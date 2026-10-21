/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameException.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏框架自定义异常类 - 异常抛出、日志上报、Lua异常包装
 ***************************************************************/
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;


namespace Honor.Runtime
{
    //=========================================================================
    // 
    //=========================================================================
    /// <summary>
    /// 游戏框架自定义异常类
    /// <para>提供游戏运行时异常抛出、日志上报、Lua 异常包装功能</para>
    /// </summary>
    [Serializable]
    public class GameException : Exception
    {
        #region 构造函数
        //=========================================================================
        // 异常构造函数
        //=========================================================================
        /// <summary>
        /// 游戏框架异常构造函数
        /// 初始化异常类的新实例
        /// </summary>
        public GameException()
            : base()
        {
        }

        /// <summary>
        /// 使用指定错误消息初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public GameException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 使用指定错误消息和内部异常，初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        /// <param name="innerException">导致当前异常的内部异常</param>
        public GameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// 序列化构造函数（反序列化时使用）
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">流上下文</param>
        protected GameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Lua 异常抛出
        //=========================================================================
        // Lua 异常上报与包装处理
        //=========================================================================
        /// <summary>
        /// 主动抛出并上报 Lua 异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public static void ThrowLuaException(string message)
        {
            try
            {
                throw new GameException(message);
            }
            catch (GameException e)
            {
                // 安全匹配 Lua 文件名
                string fileName = Regex.Match(e.Message, @"[^\\/\:\*\?""<>\|]+\.lua").Value;
                string originalMethodName = string.Empty;

                // 安全获取目标方法（防御空引用）
                MethodBase targetSite = e.TargetSite;
                if (targetSite != null)
                {
                    originalMethodName = targetSite.Name;

                    // TODO: 抽离重复的反射字段查找为私有辅助方法，避免两次 GetField 调用重复代码
                    // 若匹配到文件名则使用，否则标记为 Lua 异常
                    string displayName = string.IsNullOrEmpty(fileName) ? "Lua Exception" : fileName;
                    SetTargetSiteDisplayName(targetSite, displayName);
                }

                // 输出异常
                Debug.LogException(e);

                // 还原方法名（避免污染运行时）
                if (targetSite != null && !string.IsNullOrEmpty(originalMethodName))
                {
                    // TODO: 复用私有辅助方法还原 TargetSite 显示名，与上方修改逻辑保持一致
                    SetTargetSiteDisplayName(targetSite, originalMethodName);
                }
            }
        }
        #endregion

        #region 私有辅助方法
        //=========================================================================
        // 私有辅助方法
        //=========================================================================
        /// <summary>
        /// 通过反射修改 TargetSite 的内部 name 字段（用于 Lua 异常日志美化）
        /// </summary>
        /// <param name="targetSite">异常栈目标方法</param>
        /// <param name="displayName">要设置的显示名</param>
        /// <remarks>反射失败时静默忽略，不影响主流程</remarks>
        private static void SetTargetSiteDisplayName(MethodBase targetSite, string displayName)
        {
            if (targetSite == null)
                return;

            // 安全获取字段（防御反射失败）
            FieldInfo field = targetSite.GetType().GetField(
                "name",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);

            field?.SetValue(targetSite, displayName);
        }
        #endregion
    }
}