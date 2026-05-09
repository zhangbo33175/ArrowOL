using System.Collections;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 商业级单例基类
    /// 解决：DontDestroyOnLoad 子物体报错
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T mInstance = null;
        private static readonly object mLock = new object();
        private static bool mIsApplicationQuitting = false;

        public static T Instance
        {
            get
            {
                if (mIsApplicationQuitting)
                {
                    Debug.LogWarning($"[{typeof(T).Name}] 单例已销毁，返回null");
                    return null;
                }

                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = FindObjectOfType(typeof(T)) as T;

                        if (mInstance == null)
                        {
                            GameObject go = new GameObject($"[Singleton] {typeof(T).Name}");
                            mInstance = go.AddComponent<T>();
                        }
                    }
                }
                return mInstance;
            }
        }

        public virtual void Startup()
        {
        }

        protected virtual void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;

                // 【关键修复】必须是根物体才能用 DontDestroyOnLoad
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);

                Init();
            }
            else if (mInstance != this)
            {
                DestroyImmediate(gameObject);
            }
        }

        protected virtual void Init()
        {
        }

        public IEnumerator CoDestroySelf()
        {
            yield return CoDispose();
            mInstance = null;
            DestroyImmediate(gameObject);
        }

        public void DestroySelf()
        {
            Dispose();
            mInstance = null;
            DestroyImmediate(gameObject);
        }

        public virtual void Dispose()
        {
        }

        public virtual IEnumerator CoDispose()
        {
            yield return null;
        }

        protected virtual void OnApplicationQuit()
        {
            mIsApplicationQuitting = true;
        }
    }
}