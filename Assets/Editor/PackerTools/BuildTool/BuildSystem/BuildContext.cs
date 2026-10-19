/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildContext.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 打包上下文集合
 *            以对象类型为键存取各阶段共享的上下文对象，
 *            重复注入 / 缺失读取均抛异常，便于及早暴露装配错误。
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打包时的上下文对象集合
    /// 说明：一次打包流程共享一份上下文，各 BuildOrder 通过
    ///       <see cref="GetContextObject{T}"/> 取出所需平台/配置对象。
    /// </summary>
    public class BuildContext
    {
        #region 字段
        /// <summary>上下文对象表：键为对象的具体运行时类型</summary>
        private readonly Dictionary<Type, IContextObject> _contextObjects = new Dictionary<Type, IContextObject>();
        #endregion

        #region 增删取
        /// <summary>
        /// 添加上下文对象
        /// </summary>
        /// <param name="contextObject">待注入对象（不可为 null，同类型重复注入抛异常）</param>
        /// <exception cref="ArgumentNullException">contextObject 为 null</exception>
        /// <exception cref="Exception">该类型已存在</exception>
        public void AddContextObject(IContextObject contextObject)
        {
            if (contextObject == null)
            {
                throw new ArgumentNullException(nameof(contextObject));
            }

            Type type = contextObject.GetType();
            if (_contextObjects.ContainsKey(type))
            {
                throw new Exception($"[BuildContext] Context object {type} is already existed.");
            }

            _contextObjects.Add(type, contextObject);
        }

        /// <summary>
        /// 获取上下文对象
        /// </summary>
        /// <typeparam name="T">上下文对象类型（必须实现 IContextObject）</typeparam>
        /// <returns>对应类型的上下文对象</returns>
        /// <exception cref="Exception">未找到该类型的上下文对象</exception>
        public T GetContextObject<T>() where T : IContextObject
        {
            Type type = typeof(T);
            if (_contextObjects.TryGetValue(type, out IContextObject contextObject))
            {
                return (T)contextObject;
            }

            throw new Exception($"[BuildContext] Not found context object : {type}");
        }

        /// <summary>
        /// 清理所有上下文对象（每次 BuildBegin 前调用，避免跨次打包残留）
        /// </summary>
        public void ClearAllContext()
        {
            _contextObjects.Clear();
        }
        #endregion
    }
}
