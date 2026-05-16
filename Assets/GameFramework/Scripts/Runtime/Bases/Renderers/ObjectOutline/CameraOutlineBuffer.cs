/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  CameraOutlineBuffer.cs
 * author:    云毅
 * created:   2026   2025年
 * descrip:   全局描边渲染单例管理器，使用CommandBuffer实现高效物体轮廓描边
 ***************************************************************/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace Honor.Runtime
{
    //=========================================================================
    // 全局描边渲染管理器
    //=========================================================================
    /// <summary>
    /// 全局描边渲染管理器（单例）
    /// 功能：使用 CommandBuffer 渲染物体轮廓，实现高效全屏描边效果
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class CameraOutlineBuffer : MonoBehaviour
    {
        #region 单例实例
        /// <summary>
        /// 全局唯一实例
        /// </summary>
        public static CameraOutlineBuffer Instance { get; private set; }
        #endregion

        #region 描边对象集合
        /// <summary>
        /// 已注册的描边物体集合
        /// </summary>
        private readonly GameLinkedSet<ObjectOutline> m_Outlines = new GameLinkedSet<ObjectOutline>();
        #endregion

        #region  Inspector 配置
        [Header("描边基础设置")]
        [Range(1.0f, 6.0f)]
        public float LineThickness = 1.25f;

        [Range(0, 10)]
        public float LineIntensity = 0.5f;

        [Range(0, 1)]
        public float FillAmount = 0.2f;

        public Color LineColor0 = Color.red;
        public Color LineColor1 = Color.green;
        public Color LineColor2 = Color.blue;

        public bool AdditiveRendering;
        public bool BackfaceCulling = true;

        public Color FillColor = Color.blue;
        public bool UseFillColor;

        [Header("性能相关设置（可能影响性能）")]
        public bool CornerOutlines;
        public bool AddLinesBetweenColors;

        [Header("高级设置")]
        public bool ScaleWithScreenSize = true;

        [Range(0.0f, 1.0f)]
        public float AlphaCutoff = 0.5f;

        public bool FlipY;
        public Camera SourceCamera;
        public bool AutoEnableOutlines;
        #endregion

        #region 内部引用
        [HideInInspector]
        public Camera OutlineCamera;

        private Material m_Outline1Material;
        private Material m_Outline2Material;
        private Material m_Outline3Material;
        private Material m_OutlineEraseMaterial;

        private Shader m_OutlineShader;
        private Shader m_OutlineBufferShader;

        [HideInInspector]
        public Material OutlineShaderMaterial;

        [HideInInspector]
        public RenderTexture RenderTexture;

        [HideInInspector]
        public RenderTexture ExtraRenderTexture;

        private CommandBuffer m_CommandBuffer;
        private List<Material> m_MaterialBuffer = new List<Material>();
        private bool m_RenderTheNextFrame;
        #endregion

        #region 材质获取工具
        /// <summary>
        /// 根据颜色ID获取对应材质
        /// </summary>
        /// <param name="id">颜色索引ID</param>
        /// <returns>对应描边材质</returns>
        private Material GetMaterialFromID(int id)
        {
            switch (id)
            {
                case 0: return m_Outline1Material;
                case 1: return m_Outline2Material;
                case 2: return m_Outline3Material;
                default: return m_Outline1Material;
            }
        }

        /// <summary>
        /// 创建描边专用材质
        /// </summary>
        /// <param name="emissionColor">自发光颜色</param>
        /// <returns>描边材质</returns>
        private Material CreateMaterial(Color emissionColor)
        {
            Material mat = new Material(m_OutlineBufferShader);
            mat.SetColor("_Color", emissionColor);
            mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            return mat;
        }
        #endregion

        #region 生命周期
        /// <summary>
        /// 单例初始化
        /// </summary>
        private void Awake()
        {
            // 单例安全检测
            if (Instance != null)
            {
                Destroy(this);
                throw new System.Exception("场景中只能存在一个 CameraOutlineBuffer 实例！");
            }

            Instance = this;
        }

        /// <summary>
        /// 初始化材质、相机、渲染纹理、命令缓冲
        /// </summary>
        private void Start()
        {
            CreateMaterialsIfNeeded();
            UpdateMaterialsPublicProperties();

            // 自动绑定主相机
            if (SourceCamera == null)
            {
                SourceCamera = GetComponent<Camera>();
                if (SourceCamera == null)
                    SourceCamera = Camera.main;
            }

            // 初始化描边相机
            if (OutlineCamera == null)
            {
                foreach (Camera c in GetComponentsInChildren<Camera>())
                {
                    if (c.name == "Outline Camera")
                    {
                        OutlineCamera = c;
                        OutlineCamera.enabled = false;
                        break;
                    }
                }

                if (OutlineCamera == null)
                {
                    GameObject camObj = new GameObject("Outline Camera");
                    camObj.transform.parent = SourceCamera.transform;
                    OutlineCamera = camObj.AddComponent<Camera>();
                    OutlineCamera.enabled = false;
                }
            }

            // 释放旧RT
            if (RenderTexture != null) RenderTexture.Release();
            if (ExtraRenderTexture != null) ExtraRenderTexture.Release();

            // 创建RT
            RenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            ExtraRenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);

            UpdateOutlineCameraFromSource();

            // 初始化命令缓冲
            m_CommandBuffer = new CommandBuffer();
            OutlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, m_CommandBuffer);
        }

        /// <summary>
        /// 启用时自动收集场景描边对象
        /// </summary>
        private void OnEnable()
        {
            ObjectOutline[] allOutlines = FindObjectsOfType<ObjectOutline>();

            if (AutoEnableOutlines)
            {
                foreach (var o in allOutlines)
                {
                    o.enabled = false;
                    o.enabled = true;
                }
            }
            else
            {
                foreach (var o in allOutlines)
                {
                    if (!m_Outlines.Contains(o))
                        m_Outlines.Add(o);
                }
            }
        }

        /// <summary>
        /// 销毁时释放资源
        /// </summary>
        private void OnDestroy()
        {
            // 释放渲染纹理
            if (RenderTexture != null) RenderTexture.Release();
            if (ExtraRenderTexture != null) ExtraRenderTexture.Release();

            // 销毁材质
            DestroyMaterials();
            Instance = null;
        }
        #endregion

        #region 渲染逻辑
        /// <summary>
        /// 相机预渲染：绘制所有描边物体到 RT
        /// </summary>
        private void OnPreRender()
        {
            if (m_CommandBuffer == null)
                return;

            // 没有描边物体时跳过渲染
            if (m_Outlines.Count == 0 && !m_RenderTheNextFrame)
                return;

            m_RenderTheNextFrame = m_Outlines.Count > 0;
            CreateMaterialsIfNeeded();

            // 屏幕尺寸变化时重建RT
            if (RenderTexture == null || RenderTexture.width != SourceCamera.pixelWidth || RenderTexture.height != SourceCamera.pixelHeight)
            {
                if (RenderTexture != null) RenderTexture.Release();
                if (ExtraRenderTexture != null) ExtraRenderTexture.Release();

                RenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                ExtraRenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                OutlineCamera.targetTexture = RenderTexture;
            }

            UpdateMaterialsPublicProperties();
            UpdateOutlineCameraFromSource();
            OutlineCamera.targetTexture = RenderTexture;
            m_CommandBuffer.SetRenderTarget(RenderTexture);
            m_CommandBuffer.Clear();

            // 渲染所有描边对象
            foreach (ObjectOutline outline in m_Outlines)
            {
                if (outline == null) continue;

                // 只渲染相机可见层
                if ((SourceCamera.cullingMask & (1 << outline.gameObject.layer)) == 0)
                    continue;

                for (int v = 0; v < outline.SharedMaterials.Length; v++)
                {
                    Material mat = null;
                    Material srcMat = outline.SharedMaterials[v];

                    // 带贴图的物体需要缓存材质
                    if (srcMat != null && srcMat.HasProperty("_MainTex") && srcMat.mainTexture != null)
                    {
                        foreach (Material g in m_MaterialBuffer)
                        {
                            if (g.mainTexture == srcMat.mainTexture)
                            {
                                if (outline.eraseRenderer && g.color == m_OutlineEraseMaterial.color)
                                    mat = g;
                                else if (!outline.eraseRenderer && g.color == GetMaterialFromID(outline.color).color)
                                    mat = g;
                            }
                        }

                        if (mat == null)
                        {
                            mat = outline.eraseRenderer ? new Material(m_OutlineEraseMaterial) : new Material(GetMaterialFromID(outline.color));
                            mat.mainTexture = srcMat.mainTexture;
                            m_MaterialBuffer.Add(mat);
                        }
                    }
                    else
                    {
                        mat = outline.eraseRenderer ? m_OutlineEraseMaterial : GetMaterialFromID(outline.color);
                    }

                    // 剔除模式
                    mat.SetInt("_Culling", BackfaceCulling ? (int)CullMode.Back : (int)CullMode.Off);

                    // 绘制渲染器
                    if (outline.MeshFilter != null && outline.MeshFilter.sharedMesh != null && v < outline.MeshFilter.sharedMesh.subMeshCount)
                    {
                        m_CommandBuffer.DrawRenderer(outline.Renderer, mat, v, 0);
                    }
                    else if (outline.SkinnedMeshRenderer != null && outline.SkinnedMeshRenderer.sharedMesh != null && v < outline.SkinnedMeshRenderer.sharedMesh.subMeshCount)
                    {
                        m_CommandBuffer.DrawRenderer(outline.Renderer, mat, v, 0);
                    }
                    else if (outline.SpriteRenderer != null)
                    {
                        m_CommandBuffer.DrawRenderer(outline.Renderer, mat, v, 0);
                    }
                }
            }

            OutlineCamera.Render();
        }

        /// <summary>
        /// 全屏图像后处理：将描边RT合成到屏幕
        /// </summary>
        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (OutlineShaderMaterial == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            OutlineShaderMaterial.SetTexture("_OutlineSource", RenderTexture);

            if (AddLinesBetweenColors)
            {
                Graphics.Blit(source, ExtraRenderTexture, OutlineShaderMaterial, 0);
                OutlineShaderMaterial.SetTexture("_OutlineSource", ExtraRenderTexture);
            }

            Graphics.Blit(source, destination, OutlineShaderMaterial, 1);
        }
        #endregion

        #region 材质管理
        /// <summary>
        /// 确保所有材质已创建
        /// </summary>
        private void CreateMaterialsIfNeeded()
        {
            m_OutlineShader = Shader.Find("Honor/GO/ObjectOutlineShader");
            m_OutlineBufferShader = Shader.Find("Honor/GO/CameraOutlineBufferShader");

            if (OutlineShaderMaterial == null)
            {
                OutlineShaderMaterial = new Material(m_OutlineShader);
                OutlineShaderMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            m_OutlineEraseMaterial ??= CreateMaterial(new Color(0, 0, 0, 0));
            m_Outline1Material ??= CreateMaterial(new Color(1, 0, 0, 0));
            m_Outline2Material ??= CreateMaterial(new Color(0, 1, 0, 0));
            m_Outline3Material ??= CreateMaterial(new Color(0, 0, 1, 0));
        }

        /// <summary>
        /// 销毁所有动态材质
        /// </summary>
        private void DestroyMaterials()
        {
            foreach (var mat in m_MaterialBuffer)
                DestroyImmediate(mat);

            m_MaterialBuffer.Clear();

            DestroyImmediate(OutlineShaderMaterial);
            DestroyImmediate(m_OutlineEraseMaterial);
            DestroyImmediate(m_Outline1Material);
            DestroyImmediate(m_Outline2Material);
            DestroyImmediate(m_Outline3Material);
        }
        #endregion

        #region 公共更新接口
        /// <summary>
        /// 更新描边材质参数
        /// </summary>
        public void UpdateMaterialsPublicProperties()
        {
            if (!OutlineShaderMaterial) return;

            float scalingFactor = ScaleWithScreenSize ? Screen.height / 360f : 1f;
            bool isXR = XRSettings.isDeviceActive && SourceCamera.stereoTargetEye != StereoTargetEyeMask.None;

            // 设置描边宽度
            if (isXR)
            {
                OutlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * LineThickness / 1000f * (1000f / XRSettings.eyeTextureWidth));
                OutlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * LineThickness / 1000f * (1000f / XRSettings.eyeTextureHeight));
            }
            else
            {
                OutlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * LineThickness / 1000f * (1000f / Screen.width));
                OutlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * LineThickness / 1000f * (1000f / Screen.height));
            }

            OutlineShaderMaterial.SetFloat("_LineIntensity", LineIntensity);
            OutlineShaderMaterial.SetFloat("_FillAmount", FillAmount);
            OutlineShaderMaterial.SetColor("_FillColor", FillColor);
            OutlineShaderMaterial.SetFloat("_UseFillColor", UseFillColor ? 1 : 0);

            OutlineShaderMaterial.SetColor("_LineColor1", LineColor0 * LineColor0);
            OutlineShaderMaterial.SetColor("_LineColor2", LineColor1 * LineColor1);
            OutlineShaderMaterial.SetColor("_LineColor3", LineColor2 * LineColor2);

            OutlineShaderMaterial.SetInt("_FlipY", FlipY ? 1 : 0);
            OutlineShaderMaterial.SetInt("_Dark", AdditiveRendering ? 0 : 1);
            OutlineShaderMaterial.SetInt("_CornerOutlines", CornerOutlines ? 1 : 0);

            Shader.SetGlobalFloat("_OutlineAlphaCutoff", AlphaCutoff);
        }

        /// <summary>
        /// 同步描边相机参数
        /// </summary>
        public void UpdateOutlineCameraFromSource()
        {
            OutlineCamera.CopyFrom(SourceCamera);
            OutlineCamera.renderingPath = RenderingPath.Forward;
            OutlineCamera.backgroundColor = Color.clear;
            OutlineCamera.clearFlags = CameraClearFlags.SolidColor;
            OutlineCamera.rect = new Rect(0, 0, 1, 1);
            OutlineCamera.cullingMask = 0;
            OutlineCamera.targetTexture = RenderTexture;
            OutlineCamera.enabled = false;
            OutlineCamera.allowHDR = false;
        }
        #endregion

        #region 描边对象管理
        /// <summary>
        /// 添加描边对象
        /// </summary>
        /// <param name="outline">描边组件</param>
        public void AddOutline(ObjectOutline outline) => m_Outlines.Add(outline);

        /// <summary>
        /// 移除描边对象
        /// </summary>
        /// <param name="outline">描边组件</param>
        public void RemoveOutline(ObjectOutline outline) => m_Outlines.Remove(outline);
        #endregion
    }
}