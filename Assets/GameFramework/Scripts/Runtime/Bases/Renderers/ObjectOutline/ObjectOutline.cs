/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ObjectOutline.cs
 * author:    云毅
 * created:   2026
 * descrip:   物体描边组件，挂载到需要描边的物体上，配合CameraOutlineBuffer使用
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 物体描边组件
    //=========================================================================
    /// <summary>
    /// 物体描边组件
    /// 挂载到需要显示描边的物体上，配合 CameraOutlineBuffer 使用
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class ObjectOutline : MonoBehaviour
    {
        #region 渲染器引用
        /// <summary>
        /// 渲染器组件
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// 精灵渲染器（2D物体）
        /// </summary>
        public SpriteRenderer SpriteRenderer { get; private set; }

        /// <summary>
        /// 蒙皮网格渲染器（角色模型）
        /// </summary>
        public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }

        /// <summary>
        /// 网格过滤器（静态模型）
        /// </summary>
        public MeshFilter MeshFilter { get; private set; }
        #endregion

        #region 配置参数
        [Header("描边颜色ID 0/1/2 对应三种颜色")]
        public int color;

        [Header("是否为擦除模式（用于镂空/遮挡）")]
        public bool eraseRenderer;
        #endregion

        #region 材质缓存
        /// <summary>
        /// 缓存的共享材质数组
        /// </summary>
        private Material[] m_SharedMaterials;

        /// <summary>
        /// 共享材质（自动缓存，避免GC）
        /// </summary>
        public Material[] SharedMaterials
        {
            get
            {
                if (m_SharedMaterials == null)
                {
                    m_SharedMaterials = Renderer.sharedMaterials;
                }
                return m_SharedMaterials;
            }
        }
        #endregion

        #region 生命周期
        private void Awake()
        {
            CacheComponents();
        }

        /// <summary>
        /// 缓存所需组件
        /// </summary>
        private void CacheComponents()
        {
            Renderer = GetComponent<Renderer>();
            SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            MeshFilter = GetComponent<MeshFilter>();
        }

        private void OnEnable()
        {
            // 注册到描边相机
            if (CameraOutlineBuffer.Instance != null)
            {
                CameraOutlineBuffer.Instance.AddOutline(this);
            }
        }

        private void OnDisable()
        {
            // 从描边相机移除
            if (CameraOutlineBuffer.Instance != null)
            {
                CameraOutlineBuffer.Instance.RemoveOutline(this);
            }
        }
        #endregion
    }
}