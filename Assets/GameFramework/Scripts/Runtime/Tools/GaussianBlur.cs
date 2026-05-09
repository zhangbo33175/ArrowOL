using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 高斯模糊后处理工具
/// 功能：对相机画面进行实时高斯模糊，并将结果输出到 RawImage 上
/// 适用：UI背景模糊、弹窗模糊、场景虚化效果
/// 注：依赖指定的高斯模糊Shader
/// </summary>
[RequireComponent(typeof(Camera))]
public class GaussianBlur : MonoBehaviour
{
    [Header("模糊基础设置")]
    [Range(0, 4)] 
    public int iterations = 3;             // 高斯模糊迭代次数，值越大模糊越强

    [Range(0.2f, 3.0f)] 
    public float blurSpread = 0.6f;        // 模糊扩散系数，控制每次迭代的模糊强度

    [Range(1, 8)] 
    public int downSample = 2;             // 降采样比例，值越高性能越好但精度越低

    [Header("引用设置")]
    [SerializeField] 
    private Material _material;            // 高斯模糊专用材质

    private RawImage _blurImage;           // 显示模糊结果的UI图像
    private Camera _camera;                // 用于渲染模糊效果的相机
    private Camera _sceneCamera;           // 场景主相机
    private Material InstantiateMaterial; // 实例化材质，避免修改原资源

    /// <summary>
    /// 高斯模糊单例（方便全局调用）
    /// </summary>
    public static GaussianBlur Instance;

    /// <summary>
    /// Shader属性ID：模糊大小（预缓存提升性能）
    /// </summary>
    private readonly int s_BlurSizeID = Shader.PropertyToID("_BlurSize");

    /// <summary>
    /// 初始化单例 & 实例化模糊材质
    /// </summary>
    private void Awake()
    {
        Instance = this;

        // 实例化材质，防止运行中修改原始材质
        if (_material != null)
        {
            InstantiateMaterial = Instantiate(_material);
        }
    }

    /// <summary>
    /// 初始化相机
    /// </summary>
    private void Start()
    {
        _sceneCamera = Camera.main;
        _camera = GetComponent<Camera>();
        _camera.enabled = false; // 关闭自动渲染，手动控制
    }

    /// <summary>
    /// 创建模糊背景并显示到目标RawImage
    /// </summary>
    /// <param name="image">显示模糊效果的RawImage</param>
    public void CreateBlurImage(RawImage image)
    {
        if (image == null) return;

        _blurImage = image;
        // 复制主相机参数，保持视角一致
        _camera.CopyFrom(_sceneCamera);
        // 手动渲染一帧模糊
        _camera.Render();
    }

    /// <summary>
    /// 移除指定RawImage的模糊显示
    /// </summary>
    /// <param name="image">要移除的RawImage</param>
    public void RemoveBlurImage(RawImage image)
    {
        if (_blurImage == image)
        {
            _blurImage = null;
        }
    }

    /// <summary>
    /// Unity图像后处理核心方法
    /// 对渲染纹理进行高斯模糊处理
    /// </summary>
    /// <param name="src">源纹理</param>
    /// <param name="dest">目标纹理</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (InstantiateMaterial != null)
        {
            // 保存旧的模糊参数，渲染完成后恢复
            float oldBlurSize = InstantiateMaterial.GetFloat(s_BlurSizeID);
            
            // 降采样，降低分辨率提升性能
            int rtW = src.width / downSample;
            int rtH = src.height / downSample;

            // 申请临时渲染纹理
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;

            // 将源纹理缩放到临时缓冲
            Graphics.Blit(src, buffer0);

            // 迭代模糊处理（垂直 + 水平 分离高斯）
            for (int i = 0; i < iterations; i++)
            {
                // 设置当前迭代的模糊大小
                InstantiateMaterial.SetFloat(s_BlurSizeID, 1.0f + i * blurSpread);

                // 垂直模糊Pass
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, InstantiateMaterial, 0);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;

                // 水平模糊Pass
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, InstantiateMaterial, 1);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }

            // 将模糊结果输出到UI
            if (_blurImage != null)
            {
                _blurImage.texture = buffer0;
            }

            // 恢复模糊参数
            InstantiateMaterial.SetFloat(s_BlurSizeID, oldBlurSize);
        }
        else
        {
            // 无材质时直接显示原图
            Graphics.Blit(src, dest);
        }
    }
}