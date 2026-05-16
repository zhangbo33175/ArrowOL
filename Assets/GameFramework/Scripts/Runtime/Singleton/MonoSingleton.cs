/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  MonoSingleton.cs
 * author:  云毅
 * created:
 * descrip:   商业级 MonoBehaviour 单例基类（线程安全、防报错、跨场景留存）
 ***************************************************************/

using System.Collections;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 商业级单例基类
    /// 解决：DontDestroyOnLoad 子物体报错、重复实例、多线程访问、退出空引用
    /// 所有需要全局唯一、跨场景留存的脚本都应继承此类
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        /// <summary>
        /// 单例静态实例
        /// </summary>
        private static T m_Instance = null;

        /// <summary>
        /// 线程锁，保证多线程访问安全
        /// </summary>
        private static readonly object m_Lock = new object();

        /// <summary>
        /// 应用是否正在退出（防止退出后继续创建单例）
        /// </summary>
        private static bool m_IsApplicationQuitting = false;

        /// <summary>
        /// 全局单例访问点
        /// </summary>
        public static T Instance
        {
            get
            {
                // 应用退出时不再创建新实例，避免报错
                if (m_IsApplicationQuitting)
                {
                    Debug.LogWarning($"[{typeof(T).Name}] 单例已销毁，返回 null");
                    return null;
                }

                // 加锁保证多线程安全
                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        // 先从场景中查找
                        m_Instance = FindObjectOfType(typeof(T)) as T;

                        // 找不到则自动创建
                        if (m_Instance == null)
                        {
                            GameObject singletonObj = new GameObject($"[Singleton] {typeof(T).Name}");
                            m_Instance = singletonObj.AddComponent<T>();
                        }
                    }
                }
                return m_Instance;
            }
        }

        /// <summary>
        /// 手动启动初始化（按需调用）
        /// </summary>
        public virtual void Startup()
        {
        }

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;

                // 【关键修复】必须设置为根物体，否则 DontDestroyOnLoad 会报错
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);

                // 初始化逻辑
                Init();
            }
            else if (m_Instance != this)
            {
                // 重复实例直接销毁
                DestroyImmediate(gameObject);
            }
        }

        /// <summary>
        /// 子类重写初始化逻辑
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// 协程方式销毁自身
        /// </summary>
        public IEnumerator CoDestroySelf()
        {
            yield return CoDispose();
            m_Instance = null;
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// 立即销毁自身
        /// </summary>
        public void DestroySelf()
        {
            Dispose();
            m_Instance = null;
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// 资源释放（子类重写）
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 协程资源释放（子类重写）
        /// </summary>
        public virtual IEnumerator CoDispose()
        {
            yield return null;
        }

        /// <summary>
        /// 应用退出标记
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            m_IsApplicationQuitting = true;
        }
    }
}