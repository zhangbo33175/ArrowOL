using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 通用单例基类（普通C#类单例，不继承MonoBehaviour）
    /// 作用：让所有子类自动拥有单例特性，全局唯一、全局访问
    /// </summary>
    /// <typeparam name="T">子类类型</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// 单例静态实例
        /// </summary>
        private static T m_instance;

        /// <summary>
        /// 全局访问点
        /// 第一次获取时自动创建实例并调用Init()
        /// </summary>
        public static T Instance
        {
            get
            {
                // 双重校验，保证线程安全（基础版）
                if (Singleton<T>.m_instance == null)
                {
                    // 创建实例
                    Singleton<T>.m_instance = Activator.CreateInstance<T>();

                    // 创建成功后调用初始化方法
                    if (Singleton<T>.m_instance != null)
                    {
                        (Singleton<T>.m_instance as Singleton<T>).Init();
                    }
                }

                return Singleton<T>.m_instance;
            }
        }

        /// <summary>
        /// 释放单例引用（置空）
        /// </summary>
        public static void Release()
        {
            if (Singleton<T>.m_instance != null)
            {
                Singleton<T>.m_instance = null;
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
        /// 销毁自身：释放资源 + 置空实例
        /// </summary>
        public void DestroySelf()
        {
            Dispose();
            m_instance = null;
        }

        /// <summary>
        /// 释放资源方法（必须由子类实现）
        /// </summary>
        public abstract void Dispose();
    }
}