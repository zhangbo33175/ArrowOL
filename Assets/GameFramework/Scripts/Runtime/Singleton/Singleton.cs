/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  Singleton.cs
 * author:  云毅
 * created:
 * descrip:   通用纯C#类单例基类（非Mono），全局唯一、自动初始化、安全释放
 ***************************************************************/

using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 通用单例基类（普通C#类单例，不继承MonoBehaviour）
    /// 作用：让所有子类自动拥有单例特性，全局唯一、全局访问
    /// 适合：管理器、工具类、配置类、逻辑类
    /// </summary>
    /// <typeparam name="T">子类类型，必须有无参构造</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// 单例静态实例
        /// </summary>
        private static T m_Instance;

        /// <summary>
        /// 全局访问点
        /// 第一次获取时自动创建实例并调用Init()
        /// </summary>
        public static T Instance
        {
            get
            {
                // 双重校验锁（基础线程安全）
                if (m_Instance == null)
                {
                    // 反射创建实例
                    m_Instance = Activator.CreateInstance<T>();

                    // 自动初始化
                    (m_Instance as Singleton<T>)?.Init();
                }

                return m_Instance;
            }
        }

        /// <summary>
        /// 初始化方法（第一次获取单例时自动调用）
        /// 可被子类重写
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// 释放单例引用（置空）
        /// </summary>
        public static void Release()
        {
            m_Instance = null;
        }

        /// <summary>
        /// 销毁自身：释放资源 + 置空实例
        /// </summary>
        public void DestroySelf()
        {
            Dispose();
            m_Instance = null;
        }

        /// <summary>
        /// 释放资源方法（必须由子类实现）
        /// </summary>
        public abstract void Dispose();
    }
}