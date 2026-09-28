/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PostEffectSwitchScene.cs
 * author:  云毅
 * created:
 * descrip:   切场溶解后处理 - 场景切换时的全屏溶解转场（支持切层相机与噪声贴图）
 * 优化记录: 由旧版 HonorGraphics.PostEffectSwitchScene 迁移，
 *           移除旧版 FastShadowProjector/AssetUtils 依赖，噪声纹理外部注入
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 切场溶解后处理
    /// 功能：以溶解噪点驱动全屏转场，支持隐藏切层相机（cam2d）并自动推进遮罩
    /// 用法：挂在转场相机上，Init(2dCam, cullingMask) 后启用
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PostEffectSwitchScene : PostEffectsBase
    {
        #region 字段

        private Shader m_PeShader;
        private Material m_PeMat;

        /// <summary>噪点纹理</summary>
        public Texture noiseTex;

        /// <summary>转场帧数</summary>
        [Range(0f, 60f)]
        public float frame = 45f;

        /// <summary>初始遮罩</summary>
        [Range(0.01f, 2f)]
        public float initMask = 1f;

        /// <summary>边缘宽度</summary>
        [Range(0f, 1f)]
        public float edgeSize = 0.05f;

        /// <summary>基础亮度</summary>
        [Range(0f, 3f)]
        public float baseIntensity = 1f;

        /// <summary>边缘亮度</summary>
        [Range(0f, 3f)]
        public float edgeIntensity = 1.83f;

        /// <summary>基础颜色</summary>
        public Color baseColor = Color.white;

        /// <summary>边缘颜色</summary>
        public Color edgeColor = new Color(1f, 0.72f, 0.09f, 1f);

        private float m_Mask;
        private float m_Speed;
        private float m_EdgeSizeSpeed;

        /// <summary>转场相机（本组件宿主）</summary>
        private Camera m_Cam;

        /// <summary>原始 cullingMask（销毁时还原）</summary>
        private int m_MaskSave;

        /// <summary>切层 2D 相机（转场期间禁用）</summary>
        private Camera m_Cam2d;

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化：设置 cullingMask 并停用 2D 切层相机
        /// </summary>
        /// <param name="cam2d">2D 切层相机（可为 null）</param>
        /// <param name="cullingMask">转场相机裁剪层</param>
        public void Init(Camera cam2d, int cullingMask)
        {
            m_Cam2d = cam2d;
            if (m_Cam != null)
            {
                m_Cam.cullingMask = cullingMask;
            }

            if (m_Cam2d != null)
            {
                m_Cam2d.enabled = false;
            }
        }

        private void Awake()
        {
            m_Cam = GetComponent<Camera>();
            if (m_Cam != null)
            {
                m_MaskSave = m_Cam.cullingMask;
            }

            m_Mask = initMask;
            m_Speed = initMask / Mathf.Max(frame, 0.01f);
            m_EdgeSizeSpeed = edgeSize / Mathf.Max(frame, 0.01f);
        }

        #endregion

        #region 资源与更新

        /// <summary>
        /// 校验资源并创建材质
        /// </summary>
        private new bool CheckResources()
        {
            if (!CheckSupport(true))
            {
                return false;
            }

            m_PeShader = ShaderManager.Find("Hidden/PostEffect/PostEffectDissolve");
            m_PeMat = CheckShaderAndCreateMaterial(m_PeShader, m_PeMat);
            if (m_PeMat == null)
            {
                return false;
            }

            if (noiseTex != null)
            {
                m_PeMat.SetTexture("_NoiseTex", noiseTex);
            }

            return true;
        }

        private void Update()
        {
            m_Mask -= m_Speed;
            edgeSize -= m_EdgeSizeSpeed;
            if (m_Mask < -1f)
            {
                DestroyImmediate(this);
            }
        }

        #endregion

        #region 渲染

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!CheckResources())
            {
                Graphics.Blit(source, destination);
                return;
            }

            m_PeMat.SetFloat("_DissolveMask", m_Mask);
            m_PeMat.SetFloat("_EdgeSize", Mathf.Max(0f, edgeSize));
            m_PeMat.SetFloat("_BaseIntensity", baseIntensity);
            m_PeMat.SetFloat("_EdgeIntensity", edgeIntensity);
            m_PeMat.SetColor("_BaseColor", baseColor);
            m_PeMat.SetColor("_EdgeColor", edgeColor);
            Graphics.Blit(source, destination, m_PeMat);
        }

        private void OnDestroy()
        {
            if (m_PeMat != null)
            {
                DestroyImmediate(m_PeMat);
                m_PeMat = null;
            }

            // 还原相机状态
            if (m_Cam != null)
            {
                m_Cam.cullingMask = m_MaskSave;
            }

            if (m_Cam2d != null)
            {
                m_Cam2d.enabled = true;
            }
        }

        #endregion
    }
}
