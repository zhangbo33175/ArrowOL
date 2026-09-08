/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  Singleton.cs
 * author:  云毅
 * created:
 * descrip:   通用纯C#类单例基类（非Mono），全局唯一、自动初始化、安全释放
 * 优化记录: 实现真正的双重校验锁；CRTP约束去掉冗余类型转换；释放语义完善
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
    /// <remarks>
    /// 线程安全：Instance 采用双重校验锁（Double-Checked Locking）实现，
    /// 与 MonoSingleton 保持一致的线程安全语义，可安全跨线程获取。
    /// 泛型约束：要求 T 必须为继承自 Singleton&lt;T&gt; 的 CRTP 模式，
    /// 从而省去 Instance 中的 as 类型转换，避免每次获取时的多余判断。
    /// </remarks>
    /// <typeparam name="T">子类类型，必须继承 Singleton&lt;T&gt; 且有无参构造</typeparam>
    public abstract class Singleton<T> where T : class, Singleton<T>, new()
    {
        #region 私有常量与静态字段

        /// <summary>
        /// 单例静态实例
        /// </summary>
        private static T m_Instance;

        /// <summary>
        /// 线程锁，保证多线程访问安全
        /// </summary>
        private static readonly object m_Lock = new object();

        #endregion

        #region 全局访问点

        /// <summary>
        /// 全局访问点
        /// 第一次获取时自动创建实例并调用Init()
        /// </summary>
        /// <remarks>
        /// 双重校验锁：先无锁判空（快速路径），再进入锁内二次判空并创建，
        /// 既保证线程安全，又避免每次获取都加锁的性能开销。
        /// lock 为可重入锁，Init() 内部再次访问 Instance 不会死锁。
        /// </remarks>
        public static T Instance
        {
            get
            {
                // 第一次无锁校验：绝大多数情况直接命中，避免加锁开销
                if (m_Instance == null)
                {
                    lock (m_Lock)
                    {
                        // 第二次锁内校验：防止多线程重复创建
                        if (m_Instance == null)
                        {
                            // 反射创建实例（T 受 CRTP 约束，必然继承 Singleton<T>）
                            m_Instance = Activator.CreateInstance<T>();

                            // 自动初始化
                            m_Instance.Init();
                        }
                    }
                }

                return m_Instance;
            }
        }

        #endregion

        #region 生命周期与释放

        /// <summary>
        /// 初始化方法（第一次获取单例时自动调用）
        /// 可被子类重写
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// 释放单例引用（仅置空实例，不调用 Dispose）
        /// </summary>
        /// <remarks>
        /// 适用：需要临时重置单例、不释放其持有资源的场景（如场景切换中的重新初始化）。
        /// 需要连同资源一起释放时，请调用 DestroySelf()。
        /// </remarks>
        public static void Release()
        {
            lock (m_Lock)
            {
                m_Instance = null;
            }
        }

        /// <summary>
        /// 销毁自身：释放资源 + 置空实例
        /// </summary>
        /// <remarks>
        /// 先调用子类 Dispose() 释放资源，再置空单例引用，下次访问会重新创建。
        /// </remarks>
        public void DestroySelf()
        {
            lock (m_Lock)
            {
                Dispose();
                m_Instance = null;
            }
        }

        /// <summary>
        /// 释放资源方法（必须由子类实现）
        /// </summary>
        public abstract void Dispose();

        #endregion
    }
}
