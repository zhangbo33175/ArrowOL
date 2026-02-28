using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Honor.Runtime
{
    [Serializable]
    public class GameException : Exception
    {
        /// <summary>
        /// 框架异常类构造
        /// 初始化异常类的新实例
        /// </summary>
        public GameException(): base()
        {
        }

        /// <summary>
        /// 使用指定错误消息初始化框架异常类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public GameException(string message): base(message)
        {
        }

        /// <summary>
        /// 使用指定错误消息和此异常原因内部异常的引用来初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        /// <param name="innerException">导致当前异常的异常，如果 innerException 参数不为空引用，则在处理内部异常的 catch 块中引发当前异常</param>
        public GameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用序列化数据初始化框架异常类的新实例
        /// </summary>
        /// <param name="info">存有有关所引发异常的序列化的对象数据</param>
        /// <param name="context">包含有关源或目标的上下文信息</param>
        protected GameException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

        /// <summary>
        /// 主动上传Lua异常
        /// </summary>
        /// <param name="message"></param>
        public static void ThrowLuaException(string message)
        {
            try
            {
                // 抛出一个异常
                throw new GameException(message);
            }
            catch (Exception e)
            {
                string fileName = Regex.Match(e.Message, @".+.lua").Value;
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileName = "Lua Exception";
                }

                string name = e.TargetSite.Name;
                var field = e.TargetSite.GetType().GetField("name",
                    BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                field.SetValue(e.TargetSite, fileName);
                Debug.LogException(e);
                field.SetValue(e.TargetSite, name);
            }
        }
    }
}