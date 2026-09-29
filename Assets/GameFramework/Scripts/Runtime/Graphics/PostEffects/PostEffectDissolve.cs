/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PostEffectDissolve.cs
 * author:  云毅
 * created:
 * descrip:   全屏溶解后处理 - 屏幕淡出/场景切换溶解（支持底层纹理叠加）
 * 优化记录: 由旧版 HonorGraphics.PostEffectDissolve 迁移，噪点纹理改为外部注入，统一命名空间
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 全屏溶解后处理
    /// 功能：使用噪点纹理驱动屏幕溶解，从 1→-1 自动推进，结束自动销毁
    /// 用法：挂在相机上并调用 Init(originCamera) 可选叠加底层相机画面
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PostEffectDissolve : PostEffectsBase
    {
        #region 字段

        /// <summary>噪点纹理（溶解随机源）</summary>
        public Texture noiseTexture;

        /// <summary>溶解后处理 Shader</summary>
        private Shader m_PeShader;

        /// <summary>溶解后处理材质</summary>
        private Material m_PeMat;

        /// <summary>溶解帧数</summary>
        [Range(0f, 60f)]
        public float frame = 30f;

        /// <summary>初始遮罩</summary>
        [Range(0.01f, 2f)]
        public float initMask = 1.1f;

        /// <summary>边缘宽度</summary>
        [Range(0f, 1f)]
        public float edgeSize = 0.2f;

        /// <summary>基础亮度</summary>
        [Range(0f, 3f)]
        public float baseIntensity = 1f;

        /// <summary>边缘亮度</summary>
        [Range(0f, 3f)]
        public float edgeIntensity = 1f;

        /// <summary>基础颜色</summary>
        public Color baseColor = Color.white;

        /// <summary>边缘颜色</summary>
        public Color edgeColor = Color.grey;

        /// <summary>当前遮罩值</summary>
        private float m_Mask;

        /// <summary>遮罩推进速度</summary>
        private float m_Speed;

        /// <summary>底层叠加相机</summary>
        private Camera m_OriginCamera;

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化（可选）：指定底层相机，将其渲染结果叠加到溶解底层
        /// </summary>
        public void Init(Camera originCamera)
        {
            m_OriginCamera = originCamera;
            if (m_OriginCamera != null)
            {
                m_OriginCamera.targetTexture = new RenderTexture(
                    m_OriginCamera.pixelWidth, m_OriginCamera.pixelHeight, 16, RenderTextureFormat.DefaultHDR);
                m_OriginCamera.targetTexture.hideFlags = HideFlags.DontSave;
            }
        }

        /// <summary>
        /// 初始化遮罩推进速度并激活底层相机
        /// </summary>
        private void Start()
        {
            m_Mask = initMask;
            m_Speed = initMask / Mathf.Max(frame, 0.01f);

            if (m_OriginCamera != null)
            {
                m_OriginCamera.gameObject.SetActive(true);
            }
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

            if (noiseTexture != null)
            {
                m_PeMat.SetTexture("_NoiseTex", noiseTexture);
            }

            return true;
        }

        /// <summary>
        /// 每帧推进遮罩，低于 -1 后销毁自身
        /// </summary>
        private void Update()
        {
#if UNITY_EDITOR
            // 编辑器调试：空格重置遮罩
            if (Platform.IsEditor && Input.GetKeyDown(KeyCode.Space))
            {
                m_Mask = initMask;
            }
#endif

            m_Mask -= m_Speed;
            if (m_Mask < -1f)
            {
                DestroyImmediate(this);
            }
        }

        #endregion

        #region 渲染

        /// <summary>
        /// 渲染回调：应用溶解材质参数后 Blit 到目标
        /// </summary>
        /// <param name="source">源渲染纹理</param>
        /// <param name="destination">目标渲染纹理</param>
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!CheckResources())
            {
                Graphics.Blit(source, destination);
                return;
            }

            if (m_OriginCamera != null && m_OriginCamera.targetTexture != null)
            {
                m_PeMat.SetTexture("_BaseTex", m_OriginCamera.targetTexture);
            }

            m_PeMat.SetFloat("_DissolveMask", m_Mask);
            m_PeMat.SetFloat("_EdgeSize", edgeSize);
            m_PeMat.SetFloat("_BaseIntensity", baseIntensity);
            m_PeMat.SetFloat("_EdgeIntensity", edgeIntensity);
            m_PeMat.SetColor("_BaseColor", baseColor);
            m_PeMat.SetColor("_EdgeColor", edgeColor);
            Graphics.Blit(source, destination, m_PeMat);
        }

        /// <summary>
        /// 销毁时释放材质并还原底层相机
        /// </summary>
        private void OnDestroy()
        {
            if (m_PeMat != null)
            {
                DestroyImmediate(m_PeMat);
                m_PeMat = null;
            }

            if (m_OriginCamera != null)
            {
                m_OriginCamera.targetTexture = null;
                m_OriginCamera.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
