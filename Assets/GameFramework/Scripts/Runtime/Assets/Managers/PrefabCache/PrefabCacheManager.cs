/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PrefabCacheManager.cs
 * author:  云毅
 * created:
 * descrip:   预制体缓存池 - 统一管理预制体实例的获取/回收/预加载，降低实例化开销
 * 优化记录: 由旧版 HonorAsset.CachePrefab/资源缓存思想迁移重构，
 *           接入框架 AssetComponent（abPath/assetName 规范），实例自动携带 CachePrefab 标记
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体缓存池（全局单例）
    /// 功能：按资源路径缓存预制体实例，支持 获取/回收/预加载/清空
    /// 使用：PrefabCacheManager.Instance.Spawn(abPath, assetName, parent, callback)
    /// </summary>
    public sealed class PrefabCacheManager : MonoSingleton<PrefabCacheManager>
    {
        #region 常量与字段

        /// <summary>单路径最大缓存数量（防泄漏）</summary>
        private const int MaxCacheCountPerPath = 20;

        /// <summary>缓存槽：空闲实例栈（键 = abPath + '/' + assetName）</summary>
        private readonly Dictionary<string, Stack<GameObject>> m_FreeCache = new Dictionary<string, Stack<GameObject>>();

        /// <summary>使用中实例集合（回收时校验归属）</summary>
        private readonly HashSet<GameObject> m_UsingSet = new HashSet<GameObject>();

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化缓存池（跨场景留存）
        /// </summary>
        protected override void Init()
        {
            base.Init();
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region 获取实例

        /// <summary>
        /// 从缓存池获取预制体实例（空闲缓存命中则直接返回，否则异步加载创建）
        /// </summary>
        /// <param name="abPath">AB 包路径</param>
        /// <param name="assetName">预制体资源名</param>
        /// <param name="parent">父节点（可为 null）</param>
        /// <param name="callback">回调（instance），加载失败返回 null</param>
        public void Spawn(string abPath, string assetName, Transform parent, System.Action<GameObject> callback)
        {
            string cacheKey = GetCacheKey(abPath, assetName);

            // 1. 优先命中空闲缓存
            if (TryGetFromCache(cacheKey, parent, out GameObject cached))
            {
                m_UsingSet.Add(cached);
                callback?.Invoke(cached);
                return;
            }

            // 2. 缓存未命中，走框架资源组件异步加载
            AssetComponent asset = GameComponentsGroup.GetComponent<AssetComponent>();
            if (asset == null)
            {
                Log.Fatal("PrefabCacheManager.Spawn 未找到 AssetComponent！");
                return;
            }

            asset.LoadPrefabAsync(abPath, assetName, parent, null, (prefabObject, gameObject) =>
            {
                if (gameObject == null)
                {
                    Log.Error("PrefabCacheManager.Spawn 加载失败：{0}/{1}", abPath, assetName);
                    callback?.Invoke(null);
                    return;
                }

                OnSpawned(gameObject, cacheKey);
                m_UsingSet.Add(gameObject);
                callback?.Invoke(gameObject);
            });
        }

        /// <summary>
        /// 同步获取实例（仅支持已缓存或编辑器/同步可加载场景）
        /// </summary>
        public GameObject SpawnSync(string abPath, string assetName, Transform parent)
        {
            string cacheKey = GetCacheKey(abPath, assetName);
            if (TryGetFromCache(cacheKey, parent, out GameObject cached))
            {
                m_UsingSet.Add(cached);
                return cached;
            }

            AssetComponent asset = GameComponentsGroup.GetComponent<AssetComponent>();
            if (asset == null)
            {
                return null;
            }

            GameObject instance = asset.LoadPrefabSync(abPath, assetName, parent);
            if (instance != null)
            {
                OnSpawned(instance, cacheKey);
                m_UsingSet.Add(instance);
            }

            return instance;
        }

        /// <summary>实例生成后的通用初始化</summary>
        private void OnSpawned(GameObject instance, string cacheKey)
        {
            CachePrefab mark = instance.GetComponent<CachePrefab>();
            if (mark == null)
            {
                mark = instance.AddComponent<CachePrefab>();
            }

            mark.SrcPath = cacheKey;
            mark.Pool = this;
            instance.SetActive(true);
        }

        /// <summary>尝试从空闲缓存获取</summary>
        private bool TryGetFromCache(string cacheKey, Transform parent, out GameObject instance)
        {
            instance = null;
            if (m_FreeCache.TryGetValue(cacheKey, out Stack<GameObject> stack) && stack != null && stack.Count > 0)
            {
                instance = stack.Pop();
                if (instance != null)
                {
                    if (parent != null)
                    {
                        instance.transform.SetParent(parent, false);
                    }

                    instance.SetActive(true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>拼接缓存键</summary>
        private static string GetCacheKey(string abPath, string assetName)
        {
            return string.IsNullOrEmpty(abPath) ? assetName : abPath + "/" + assetName;
        }

        #endregion

        #region 回收实例

        /// <summary>
        /// 回收实例到缓存池（携带 CachePrefab 标记的实例自动归还对应槽位）
        /// </summary>
        /// <param name="instance">待回收实例</param>
        public void Recycle(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            // 校验归属：只回收本池发出的实例
            if (!m_UsingSet.Contains(instance))
            {
                Log.Warning("PrefabCacheManager.Recycle 实例不属于本缓存池，直接销毁。");
                Destroy(instance);
                return;
            }

            m_UsingSet.Remove(instance);

            CachePrefab mark = instance.GetComponent<CachePrefab>();
            string cacheKey = mark != null ? mark.SrcPath : null;
            if (string.IsNullOrEmpty(cacheKey))
            {
                Destroy(instance);
                return;
            }

            // 超过单路径上限则直接销毁，防止缓存无限增长
            if (!m_FreeCache.TryGetValue(cacheKey, out Stack<GameObject> stack))
            {
                stack = new Stack<GameObject>();
                m_FreeCache.Add(cacheKey, stack);
            }

            if (stack.Count >= MaxCacheCountPerPath)
            {
                Destroy(instance);
                return;
            }

            // 挂回缓存池节点并隐藏
            instance.transform.SetParent(transform, false);
            instance.SetActive(false);
            stack.Push(instance);
        }

        /// <summary>
        /// 延迟回收（帧末执行，避免遍历中回收）
        /// </summary>
        public void Recycle(GameObject instance, float delay)
        {
            if (delay <= 0f)
            {
                Recycle(instance);
                return;
            }

            StartCoroutine(DelayRecycleCoroutine(instance, delay));
        }

        /// <summary>延迟回收协程</summary>
        private System.Collections.IEnumerator DelayRecycleCoroutine(GameObject instance, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (instance != null)
            {
                Recycle(instance);
            }
        }

        #endregion

        #region 预加载与清空

        /// <summary>
        /// 预加载指定数量的实例（提前异步加载并缓存）
        /// </summary>
        /// <param name="abPath">AB 包路径</param>
        /// <param name="assetName">预制体资源名</param>
        /// <param name="count">预加载数量</param>
        public void Preload(string abPath, string assetName, int count)
        {
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Spawn(abPath, assetName, transform, Recycle);
            }
        }

        /// <summary>
        /// 清空全部缓存实例
        /// </summary>
        public void ClearAll()
        {
            foreach (var pair in m_FreeCache)
            {
                while (pair.Value != null && pair.Value.Count > 0)
                {
                    GameObject instance = pair.Value.Pop();
                    if (instance != null)
                    {
                        Destroy(instance);
                    }
                }
            }

            m_FreeCache.Clear();
            m_UsingSet.Clear();
        }

        /// <summary>
        /// 当前空闲缓存总量（诊断用）
        /// </summary>
        public int FreeCount
        {
            get
            {
                int count = 0;
                foreach (var pair in m_FreeCache)
                {
                    count += pair.Value.Count;
                }

                return count;
            }
        }

        #endregion
    }
}
