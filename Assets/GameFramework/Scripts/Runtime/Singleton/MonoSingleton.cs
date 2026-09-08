/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  MonoSingleton.cs
 * author:  云毅
 * created:
 * descrip:   商业级 MonoBehaviour 单例基类（线程安全、防报错、跨场景留存）
 * 优化记录: 替换过时查找API(FindObjectOfType→FindFirstObjectByType)；
 *           修复编辑器域重载后退出标记不重置的残留BUG；运行时销毁安全化
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
    /// <remarks>
    /// 线程安全：Instance 使用 lock 保证多线程访问安全；
    /// 域重载：通过 RuntimeInitializeOnLoadMethod 在每次进入 Play 模式前重置静态状态，
    ///         避免编辑器停用域重载（Enter Play Mode Options）后退出标记残留导致单例失效。
    /// </remarks>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        #region 静态字段

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

        #endregion

        #region 域重载重置（编辑器专用）

        /// <summary>
        /// 域重载重置：每次进入 Play 模式前调用，清理残留的静态状态
        /// </summary>
        /// <remarks>
        /// 解决：编辑器开启 "Enter Play Mode Options - Reload Domain" 关闭时，
        /// 静态字段不会随退出播放而清空，导致 m_IsApplicationQuitting 残留为 true，
        /// 下次进入播放模式所有单例 Instance 永久返回 null 的严重BUG。
        /// 该回调在 SubsystemRegistration 阶段执行，早于任何场景物体 Awake。
        /// </remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticStateOnDomainReload()
        {
            m_Instance = null;
            m_IsApplicationQuitting = false;
        }

        #endregion

        #region 全局访问点

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
                        // 先从场景中查找（泛型重载，避免非泛型版本的装箱与类型转换开销）
                        m_Instance = FindFirstObjectByType<T>();

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

        #endregion

        #region 生命周期

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
                // 重复实例直接销毁（维持立即销毁语义，避免场景中短暂存在重复实例）
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
        /// 应用退出标记
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            m_IsApplicationQuitting = true;
        }

        #endregion

        #region 销毁

        /// <summary>
        /// 协程方式销毁自身
        /// </summary>
        public IEnumerator CoDestroySelf()
        {
            yield return CoDispose();

            lock (m_Lock)
            {
                m_Instance = null;
            }

            // 应用退出中禁止调用销毁（Unity 会报 Destroying object during quit 警告）
            if (!m_IsApplicationQuitting)
            {
                DestroyImmediate(gameObject);
            }
        }

        /// <summary>
        /// 立即销毁自身
        /// </summary>
        public void DestroySelf()
        {
            Dispose();

            lock (m_Lock)
            {
                m_Instance = null;
            }

            // 应用退出中禁止调用销毁（Unity 会报 Destroying object during quit 警告）
            if (!m_IsApplicationQuitting)
            {
                DestroyImmediate(gameObject);
            }
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

        #endregion
    }
}
