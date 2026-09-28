/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TextureBlur.cs
 * author:  云毅
 * created:
 * descrip:   纹理模糊 - 屏幕空间降采样模糊（背景模糊/景深陪衬），支持渐入与循环
 * 优化记录: 由旧版 HonorGraphics.TextureBlur 迁移，统一命名空间，修正材质释放逻辑
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 纹理模糊
    /// 功能：将 MeshRenderer 主纹理替换为模糊 Shader 并驱动模糊强度
    /// 适用：弹窗背景模糊、界面陪衬虚化
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class TextureBlur : MonoBehaviour
    {
        #region 字段

        private Material m_BlurMat;
        private Material m_OriginMat;
        private Texture m_OriginTex;
        private MeshRenderer m_MeshRenderer;
        private RenderTexture m_Buffer0;

        /// <summary>
        /// 模糊材质（首次访问时创建）
        /// </summary>
        public Material material
        {
            get
            {
                if (m_BlurMat == null)
                {
                    m_MeshRenderer = GetComponent<MeshRenderer>();
                    if (m_MeshRenderer != null)
                    {
                        m_OriginMat = m_MeshRenderer.sharedMaterial;
                        m_BlurMat = m_MeshRenderer.sharedMaterial =
                            new Material(ShaderManager.Find("Hidden/TextureProcessing/TextureBlur"));
                        if (m_OriginMat != null && m_OriginMat.HasProperty("_MainTex"))
                        {
                            m_OriginTex = m_OriginMat.mainTexture;
                        }
                    }
                }

                return m_BlurMat;
            }
        }

        /// <summary>最大迭代次数</summary>
        private const int IterationsMax = 60;

        /// <summary>模糊迭代次数</summary>
        [Range(1, IterationsMax)]
        public int iterations = 1;

        /// <summary>模糊扩散度</summary>
        [Range(0f, 2f)]
        public float blurSize = 0.1f;

        /// <summary>降采样倍数</summary>
        [Range(1, 8)]
        public int downSample = 5;

        /// <summary>渐入起始透明度</summary>
        [Range(0f, 1f)]
        public float startFade = 0.8f;

        /// <summary>噪声纹理（模糊扰动）</summary>
        public Texture noiseTex;

        /// <summary>总时长（秒）</summary>
        public float timeMax = 3f;

        /// <summary>是否循环（不循环则在结束时释放）</summary>
        public bool loop = false;

        /// <summary>当前透明度</summary>
        private float m_FadeAlpha = 1f;

        /// <summary>已播放时长</summary>
        private float m_Time;

        #endregion

        #region 模糊处理

        /// <summary>
        /// 执行一次降采样高斯模糊
        /// </summary>
        private void DoBlur()
        {
            if (material == null || m_OriginTex == null)
            {
                return;
            }

            int rtW = m_OriginTex.width / downSample;
            int rtH = m_OriginTex.height / downSample;

            m_Buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            m_Buffer0.filterMode = FilterMode.Bilinear;

            // 首轮：缩放拷贝
            Graphics.Blit(m_OriginTex, m_Buffer0);

            // 迭代模糊
            for (int i = 0; i < iterations; i++)
            {
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                m_BlurMat.SetVector("_TexelOffsetScale",
                    new Vector4(blurSize, blurSize, 0f, 0f) * (i + 1));
                Graphics.Blit(m_Buffer0, buffer1, m_BlurMat);
                RenderTexture.ReleaseTemporary(m_Buffer0);
                m_Buffer0 = buffer1;
            }

            m_BlurMat.SetTexture("_MainTex", m_Buffer0);
        }

        #endregion

        #region 生命周期

        private void OnEnable()
        {
            m_Time = 0f;
            m_FadeAlpha = startFade;
            DoBlur();
        }

        private void Update()
        {
            m_Time += Time.deltaTime;
            m_FadeAlpha = Mathf.Clamp01(m_Time / Mathf.Max(timeMax, 0.01f));

            if (material != null)
            {
                material.SetFloat("_FadeAlpha", m_FadeAlpha);
                if (noiseTex != null)
                {
                    material.SetTexture("_NoiseTex", noiseTex);
                }
            }

            if (!loop && m_Time >= timeMax)
            {
                // 播放结束释放
                Release();
            }
        }

        private void OnDisable()
        {
            Release();
        }

        #endregion

        #region 释放

        /// <summary>
        /// 释放临时 RenderTexture 并还原原始材质
        /// </summary>
        private void Release()
        {
            if (m_Buffer0 != null)
            {
                RenderTexture.ReleaseTemporary(m_Buffer0);
                m_Buffer0 = null;
            }

            if (m_MeshRenderer != null && m_BlurMat != null)
            {
                Destroy(m_BlurMat);
                m_MeshRenderer.sharedMaterial = m_OriginMat;
            }

            m_BlurMat = null;
        }

        #endregion
    }
}
