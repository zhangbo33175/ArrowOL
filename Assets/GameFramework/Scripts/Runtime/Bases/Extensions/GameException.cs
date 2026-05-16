/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameException.cs
 * author:    云毅
 * created:   2026   2025
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

                    // 安全获取字段（防御反射失败）
                    FieldInfo field = targetSite.GetType().GetField(
                        "name",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);

                    if (field != null)
                    {
                        // 若匹配到文件名则使用，否则标记为 Lua 异常
                        string displayName = string.IsNullOrEmpty(fileName) ? "Lua Exception" : fileName;
                        field.SetValue(targetSite, displayName);
                    }
                }

                // 输出异常
                Debug.LogException(e);

                // 还原方法名（避免污染运行时）
                if (targetSite != null && !string.IsNullOrEmpty(originalMethodName))
                {
                    FieldInfo field = targetSite.GetType().GetField(
                        "name",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);

                    field?.SetValue(targetSite, originalMethodName);
                }
            }
        }
        #endregion
    }
}