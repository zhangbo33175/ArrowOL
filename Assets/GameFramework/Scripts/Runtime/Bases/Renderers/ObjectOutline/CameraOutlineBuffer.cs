using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Honor.Runtime
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	public class CameraOutlineBuffer : MonoBehaviour
	{
		public static CameraOutlineBuffer Instance { get; private set; }

		private readonly GameLinkedSet<ObjectOutline> m_Outlines = new GameLinkedSet<ObjectOutline>();

		[Range(1.0f, 6.0f)]
		public float LineThickness = 1.25f;
		[Range(0, 10)]
		public float LineIntensity = .5f;
		[Range(0, 1)]
		public float FillAmount = 0.2f;

		public Color LineColor0 = Color.red;
		public Color LineColor1 = Color.green;
		public Color LineColor2 = Color.blue;

		public bool AdditiveRendering = false;

		public bool BackfaceCulling = true;

		public Color FillColor = Color.blue;
		public bool UseFillColor = false;

		[Header("These settings can affect performance!")]
		public bool CornerOutlines = false;
		public bool AddLinesBetweenColors = false;

		[Header("Advanced settings")]
		public bool ScaleWithScreenSize = true;
		[Range(0.0f, 1.0f)]
		public float AlphaCutoff = .5f;
		public bool FlipY = false;
		public Camera SourceCamera;
		public bool AutoEnableOutlines = false;

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

		Material GetMaterialFromID(int ID)
		{
			switch(ID)
            {
				case 0:return m_Outline1Material;
				case 1:return m_Outline2Material;
				case 2:return m_Outline3Material;
				default:return m_Outline1Material;
            }
		}

		Material CreateMaterial(Color emissionColor)
		{
			Material m = new Material(m_OutlineBufferShader);
			m.SetColor("_Color", emissionColor);
			m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			m.SetInt("_ZWrite", 0);
			m.DisableKeyword("_ALPHATEST_ON");
			m.EnableKeyword("_ALPHABLEND_ON");
			m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			m.renderQueue = 3000;
			return m;
		}

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(this);
				throw new System.Exception("you can only have one outline camera in the scene");
			}

			Instance = this;
		}

		void Start()
		{
			CreateMaterialsIfNeeded();
			UpdateMaterialsPublicProperties();

			if (SourceCamera == null)
			{
				SourceCamera = GetComponent<Camera>();

				if (SourceCamera == null)
                {
					SourceCamera = Camera.main;
				}
			}

			if (OutlineCamera == null)
			{
				foreach (Camera c in GetComponentsInChildren<Camera>())
				{
					if (c.name == "Outline Camera")
					{
						OutlineCamera = c;
						c.enabled = false;
						break;
					}
				}

				if (OutlineCamera == null)
				{
					GameObject cameraGameObject = new GameObject("Outline Camera");
					cameraGameObject.transform.parent = SourceCamera.transform;
					OutlineCamera = cameraGameObject.AddComponent<Camera>();
					OutlineCamera.enabled = false;
				}
			}

			if (RenderTexture != null)
            {
				RenderTexture.Release();
			}
			if (ExtraRenderTexture != null)
            {
				ExtraRenderTexture.Release();
			}

			RenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
			ExtraRenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
			UpdateOutlineCameraFromSource();

			m_CommandBuffer = new CommandBuffer();
			OutlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, m_CommandBuffer);
		}

		public void OnPreRender()
		{
			if (m_CommandBuffer == null)
            {
				return;
            }

			if (m_Outlines.Count == 0)
			{
				if (!m_RenderTheNextFrame)
                {
					return;
                }

				m_RenderTheNextFrame = false;
			}
			else
			{
				m_RenderTheNextFrame = true;
			}

			CreateMaterialsIfNeeded();

			if (RenderTexture == null || RenderTexture.width != SourceCamera.pixelWidth || RenderTexture.height != SourceCamera.pixelHeight)
			{
				if (RenderTexture != null)
                {
					RenderTexture.Release();
				}
				if (ExtraRenderTexture != null)
                {
					ExtraRenderTexture.Release();
				}
				RenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
				ExtraRenderTexture = new RenderTexture(SourceCamera.pixelWidth, SourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
				OutlineCamera.targetTexture = RenderTexture;
			}
			UpdateMaterialsPublicProperties();
			UpdateOutlineCameraFromSource();
			OutlineCamera.targetTexture = RenderTexture;
			m_CommandBuffer.SetRenderTarget(RenderTexture);

			m_CommandBuffer.Clear();

			foreach (ObjectOutline outline in m_Outlines)
			{
				LayerMask l = SourceCamera.cullingMask;

				if (outline != null && l == (l | (1 << outline.gameObject.layer)))
				{
					for (int v = 0; v < outline.SharedMaterials.Length; v++)
					{
						Material m = null;

						if (outline.SharedMaterials[v].HasProperty("_MainTex") && outline.SharedMaterials[v].mainTexture != null && outline.SharedMaterials[v])
						{
							foreach (Material g in m_MaterialBuffer)
							{
								if (g.mainTexture == outline.SharedMaterials[v].mainTexture)
								{
									if (outline.eraseRenderer && g.color == m_OutlineEraseMaterial.color)
                                    {
										m = g;
									}
									else if (!outline.eraseRenderer && g.color == GetMaterialFromID(outline.color).color)
                                    {
										m = g;
									}
								}
							}

							if (m == null)
							{
								if (outline.eraseRenderer)
                                {
									m = new Material(m_OutlineEraseMaterial);
								}
								else
								{ 
									m = new Material(GetMaterialFromID(outline.color)); 
								}

								m.mainTexture = outline.SharedMaterials[v].mainTexture;
								m_MaterialBuffer.Add(m);
							}
						}
						else
						{
							if (outline.eraseRenderer)
                            {
								m = m_OutlineEraseMaterial;
							}
							else
                            {
								m = GetMaterialFromID(outline.color);
							}
						}

						if (BackfaceCulling)
                        {
							m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Back);
						}
						else
                        {
							m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Off);
						}

						MeshFilter mL = outline.MeshFilter;
						SkinnedMeshRenderer sMR = outline.SkinnedMeshRenderer;
						SpriteRenderer sR = outline.SpriteRenderer;
						if (mL)
						{
							if (mL.sharedMesh != null)
							{
								if (v < mL.sharedMesh.subMeshCount)
                                {
									m_CommandBuffer.DrawRenderer(outline.Renderer, m, v, 0);
								}
							}
						}
						else if (sMR)
						{
							if (sMR.sharedMesh != null)
							{
								if (v < sMR.sharedMesh.subMeshCount)
                                {
									m_CommandBuffer.DrawRenderer(outline.Renderer, m, v, 0);
								}
							}
						}
						else if (sR)
						{
							m_CommandBuffer.DrawRenderer(outline.Renderer, m, v, 0);
						}
					}
				}
			}

			OutlineCamera.Render();
		}

		private void OnEnable()
		{
			ObjectOutline[] o = FindObjectsOfType<ObjectOutline>();
			if (AutoEnableOutlines)
			{
				foreach (ObjectOutline oL in o)
				{
					oL.enabled = false;
					oL.enabled = true;
				}
			}
			else
			{
				foreach (ObjectOutline oL in o)
				{
					if (!m_Outlines.Contains(oL))
                    {
						m_Outlines.Add(oL);
					}
				}
			}
		}

		void OnDestroy()
		{
			if (RenderTexture != null)
            {
				RenderTexture.Release();
			}
			if (ExtraRenderTexture != null)
            {
				ExtraRenderTexture.Release();
			}
			DestroyMaterials();
		}

		[ImageEffectOpaque]
		void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (OutlineShaderMaterial != null)
			{
				OutlineShaderMaterial.SetTexture("_OutlineSource", RenderTexture);

				if (AddLinesBetweenColors)
				{
					Graphics.Blit(source, ExtraRenderTexture, OutlineShaderMaterial, 0);
					OutlineShaderMaterial.SetTexture("_OutlineSource", ExtraRenderTexture);
				}
				Graphics.Blit(source, destination, OutlineShaderMaterial, 1);
			}
		}

		private void CreateMaterialsIfNeeded()
		{
			if (m_OutlineShader == null)
            {
				m_OutlineShader = Shader.Find("Honor/GO/ObjectOutlineShader");
			}
			if (m_OutlineBufferShader == null)
			{
				m_OutlineBufferShader = Shader.Find("Honor/GO/CameraOutlineBufferShader");
			}
			if (OutlineShaderMaterial == null)
			{
				OutlineShaderMaterial = new Material(m_OutlineShader);
				OutlineShaderMaterial.hideFlags = HideFlags.HideAndDontSave;
				UpdateMaterialsPublicProperties();
			}
			if (m_OutlineEraseMaterial == null)
            {
				m_OutlineEraseMaterial = CreateMaterial(new Color(0, 0, 0, 0));
			}
			if (m_Outline1Material == null)
            {
				m_Outline1Material = CreateMaterial(new Color(1, 0, 0, 0));
			}
			if (m_Outline2Material == null)
            {
				m_Outline2Material = CreateMaterial(new Color(0, 1, 0, 0));
			}
			if (m_Outline3Material == null)
            {
				m_Outline3Material = CreateMaterial(new Color(0, 0, 1, 0));
			}
		}

		private void DestroyMaterials()
		{
			foreach (Material m in m_MaterialBuffer)
            {
				DestroyImmediate(m);
			}
			m_MaterialBuffer.Clear();
			DestroyImmediate(OutlineShaderMaterial);
			DestroyImmediate(m_OutlineEraseMaterial);
			DestroyImmediate(m_Outline1Material);
			DestroyImmediate(m_Outline2Material);
			DestroyImmediate(m_Outline3Material);
			m_OutlineShader = null;
			m_OutlineBufferShader = null;
			OutlineShaderMaterial = null;
			m_OutlineEraseMaterial = null;
			m_Outline1Material = null;
			m_Outline2Material = null;
			m_Outline3Material = null;
		}

		public void UpdateMaterialsPublicProperties()
		{
			if (OutlineShaderMaterial)
			{
				float scalingFactor = 1;
				if (ScaleWithScreenSize)
				{
					scalingFactor = Screen.height / 360.0f;
				}

				if (ScaleWithScreenSize && scalingFactor < 1)
				{
					if (UnityEngine.XR.XRSettings.isDeviceActive && SourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
					{
						OutlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureWidth) * 1000.0f);
						OutlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureHeight) * 1000.0f);
					}
					else
					{
						OutlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
						OutlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
					}
				}
				else
				{
					if (UnityEngine.XR.XRSettings.isDeviceActive && SourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
					{
						OutlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (LineThickness / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureWidth) * 1000.0f);
						OutlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (LineThickness / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureHeight) * 1000.0f);
					}
					else
					{
						OutlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (LineThickness / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
						OutlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (LineThickness / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
					}
				}
				OutlineShaderMaterial.SetFloat("_LineIntensity", LineIntensity);
				OutlineShaderMaterial.SetFloat("_FillAmount", FillAmount);
				OutlineShaderMaterial.SetColor("_FillColor", FillColor);
				OutlineShaderMaterial.SetFloat("_UseFillColor", UseFillColor ? 1 : 0);
				OutlineShaderMaterial.SetColor("_LineColor1", LineColor0 * LineColor0);
				OutlineShaderMaterial.SetColor("_LineColor2", LineColor1 * LineColor1);
				OutlineShaderMaterial.SetColor("_LineColor3", LineColor2 * LineColor2);
				if (FlipY)
                {
					OutlineShaderMaterial.SetInt("_FlipY", 1);
				}
				else
                {
					OutlineShaderMaterial.SetInt("_FlipY", 0);
				}
				if (!AdditiveRendering)
                {
					OutlineShaderMaterial.SetInt("_Dark", 1);
				}
				else
                {
					OutlineShaderMaterial.SetInt("_Dark", 0);
				}
				if (CornerOutlines)
                {
					OutlineShaderMaterial.SetInt("_CornerOutlines", 1);
				}
				else
                {
					OutlineShaderMaterial.SetInt("_CornerOutlines", 0);
				}

				Shader.SetGlobalFloat("_OutlineAlphaCutoff", AlphaCutoff);
			}
		}

		void UpdateOutlineCameraFromSource()
		{
			OutlineCamera.CopyFrom(SourceCamera);
			OutlineCamera.renderingPath = RenderingPath.Forward;
			OutlineCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			OutlineCamera.clearFlags = CameraClearFlags.SolidColor;
			OutlineCamera.rect = new Rect(0, 0, 1, 1);
			OutlineCamera.cullingMask = 0;
			OutlineCamera.targetTexture = RenderTexture;
			OutlineCamera.enabled = false;
            OutlineCamera.allowHDR = false;
		}

		public void AddOutline(ObjectOutline outline) => m_Outlines.Add(outline);

		public void RemoveOutline(ObjectOutline outline) => m_Outlines.Remove(outline);
	}
}
