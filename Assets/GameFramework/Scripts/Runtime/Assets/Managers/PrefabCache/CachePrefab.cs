/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  CachePrefab.cs
 * author:  云毅
 * created:
 * descrip:   预制体缓存标记组件 - 记录实例来源路径，供缓存池回收定位
 * 优化记录: 由旧版 HonorAsset.CachePrefab 迁移，统一命名空间并补齐缓存池引用
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体缓存标记组件
    /// 功能：挂在缓存池实例上，记录其来源资源路径，回收时用于归还对应缓存槽
    /// 说明：PrefabCacheManager 生成的实例自动携带本组件
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CachePrefab : MonoBehaviour
    {
        #region 字段

        /// <summary>来源资源路径（AB 资源名）</summary>
        [SerializeField]
        private string m_SrcPath;

        /// <summary>所属缓存池实例</summary>
        [SerializeField]
        private PrefabCacheManager m_Pool;

        #endregion

        #region 属性

        /// <summary>
        /// 来源资源路径
        /// </summary>
        public string SrcPath
        {
            get { return m_SrcPath; }
            set { m_SrcPath = value; }
        }

        /// <summary>
        /// 所属缓存池
        /// </summary>
        public PrefabCacheManager Pool
        {
            get { return m_Pool; }
            set { m_Pool = value; }
        }

        /// <summary>
        /// 是否属于有效缓存池
        /// </summary>
        public bool IsValid
        {
            get { return m_Pool != null && !string.IsNullOrEmpty(m_SrcPath); }
        }

        #endregion
    }
}
