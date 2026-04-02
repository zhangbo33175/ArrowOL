using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))] // 需要相机组件
public class GaussianBlur : MonoBehaviour
{
    [Range(0, 4)] public int iterations = 3; // 高斯模糊迭代次数
    [Range(0.2f, 3.0f)] public float blurSpread = 0.6f; // 每次迭代纹理坐标偏移的速度
    [Range(1, 8)] public int downSample = 2; // 降采样比率

    [SerializeField] private Material _material; // 材质
    private RawImage _blurImage;
    private Camera _camera;
    private Camera _sceneCamera;

    public static GaussianBlur Instance;

    private readonly int s_BlurSizeID = Shader.PropertyToID("_BlurSize");
    private Material InstantiateMaterial;

    private void Awake()
    {
        Instance = this;
        if (_material != null)
        {
            InstantiateMaterial = Instantiate(_material);
        }
    }

    private void Start()
    {
        _sceneCamera = Camera.main;
        //_material = new Material(Shader.Find("Blur/GaussianBlur"));
        _camera = gameObject.GetComponent<Camera>();
        _camera.enabled = false;
    }

    /// <summary>
    /// 创建模糊背景
    /// </summary>
    /// <param name="image"></param>
    public void CreateBlurImage(RawImage image)
    {
        if (image == null)
        {
            return;
        }

        _blurImage = image;
        _camera.CopyFrom(_sceneCamera);
        _camera.Render();
    }

    /// <summary>
    /// 移除模糊背景
    /// </summary>
    public void RemoveBlurImage(RawImage image)
    {
        // 确保移除的是当前的模糊背景
        if (_blurImage == image)
        {
            _blurImage = null;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (InstantiateMaterial != null)
        {
            var oldBlurSize = InstantiateMaterial.GetFloat(s_BlurSizeID);
            int rtW = src.width / downSample; // 降采样的纹理宽度
            int rtH = src.height / downSample; // 降采样的纹理高度
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear; // 滤波模式设置为双线性
            Graphics.Blit(src, buffer0);
            for (int i = 0; i < iterations; i++)
            {
                InstantiateMaterial.SetFloat(s_BlurSizeID, 1.0f + i * blurSpread); // 设置模糊尺寸(纹理坐标的偏移量)
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, InstantiateMaterial, 0); // 渲染垂直的Pass
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, InstantiateMaterial, 1); // 渲染水平的Pass
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }

            _blurImage.texture = buffer0;

            InstantiateMaterial.SetFloat(s_BlurSizeID, oldBlurSize);
//            Graphics.Blit(buffer0, dest);
//            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}