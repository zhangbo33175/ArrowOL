/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GaussianBlur.cs
 * author:    云毅
 * created:   2026
 * descrip:   Unity相机高斯模糊后处理组件，输出模糊画面至RawImage
 ***************************************************************/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 高斯模糊后处理工具
/// 功能：对相机画面进行实时高斯模糊，并将结果输出到 RawImage 上
/// 适用：UI背景模糊、弹窗模糊、场景虚化效果
/// 注：依赖指定的高斯模糊Shader（分离高斯 垂直/水平）
/// </summary>
[RequireComponent(typeof(Camera))]
public class GaussianBlur : MonoBehaviour
{
    #region 模糊配置参数
    [Header("模糊基础设置")]
    [Range(0, 4)] 
    public int iterations = 3;

    [Range(0.2f, 3.0f)] 
    public float blurSpread = 0.6f;

    [Range(1, 8)] 
    public int downSample = 2;

    [Header("引用设置")]
    [SerializeField] 
    private Material _material;
    #endregion

    #region 私有成员字段
    /// <summary>
    /// 承载模糊画面的UI组件
    /// </summary>
    private RawImage _blurImage;

    /// <summary>
    /// 模糊专用渲染相机
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// 场景主相机
    /// </summary>
    private Camera _sceneCamera;

    /// <summary>
    /// 运行时实例化材质，避免改动原始资源
    /// </summary>
    private Material _instanceMaterial;
    #endregion

    #region 全局单例与Shader缓存
    /// <summary>
    /// 高斯模糊全局单例
    /// </summary>
    public static GaussianBlur Instance;

    /// <summary>
    /// 模糊大小Shader属性ID，预缓存优化性能
    /// </summary>
    private readonly int _blurSizeId = Shader.PropertyToID("_BlurSize");
    #endregion

    //=========================================================================
    // 生命周期初始化
    //=========================================================================
    #region 初始化逻辑
    /// <summary>
    /// 组件唤醒初始化
    /// </summary>
    private void Awake()
    {
        // 单例唯一性校验
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitMaterial();
    }

    /// <summary>
    /// 组件启动初始化
    /// </summary>
    private void Start()
    {
        _sceneCamera = Camera.main;
        _camera = GetComponent<Camera>();
        // 关闭相机自动渲染，由代码手动控制渲染流程
        _camera.enabled = false;
    }

    /// <summary>
    /// 初始化运行时材质实例
    /// </summary>
    private void InitMaterial()
    {
        if (_material != null)
            _instanceMaterial = Instantiate(_material);
    }
    #endregion

    //=========================================================================
    // 外部公开调用接口
    //=========================================================================
    #region 对外业务接口
    /// <summary>
    /// 将相机模糊画面赋值到指定RawImage
    /// </summary>
    /// <param name="image">目标显示UI对象</param>
    public void CreateBlurImage(RawImage image)
    {
        if (image == null) return;

        _blurImage = image;
        if(_sceneCamera == null) _sceneCamera = Camera.main;
        // 同步主相机视角与渲染参数
        _camera.CopyFrom(_sceneCamera);
        // 手动执行相机渲染
        _camera.Render();
    }

    /// <summary>
    /// 清空指定UI的模糊画面绑定
    /// </summary>
    /// <param name="image">目标UI对象</param>
    public void RemoveBlurImage(RawImage image)
    {
        if (_blurImage == image)
            _blurImage = null;
    }
    #endregion

    //=========================================================================
    // 后处理核心渲染逻辑
    //=========================================================================
    #region 高斯模糊渲染处理
    /// <summary>
    /// Unity后处理渲染回调
    /// </summary>
    /// <param name="src">原始渲染纹理</param>
    /// <param name="dest">输出目标纹理</param>
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_instanceMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        // 计算降采样后纹理尺寸
        int rtW = src.width / downSample;
        int rtH = src.height / downSample;

        // 申请临时渲染缓冲区
        RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
        buffer0.filterMode = FilterMode.Bilinear;
        Graphics.Blit(src, buffer0);

        // 分层迭代执行高斯模糊
        for (int i = 0; i < iterations; i++)
        {
            float blurSize = 1.0f + i * blurSpread;
            _instanceMaterial.SetFloat(_blurSizeId, blurSize);

            // 执行垂直方向模糊
            RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            Graphics.Blit(buffer0, buffer1, _instanceMaterial, 0);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;

            // 执行水平方向模糊
            buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            Graphics.Blit(buffer0, buffer1, _instanceMaterial, 1);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;
        }

        // 将最终模糊纹理赋值给UI
        if (_blurImage != null)
        {
            _blurImage.texture = buffer0;
        }

        // 释放临时纹理资源，杜绝内存泄漏
        RenderTexture.ReleaseTemporary(buffer0);

        // 原始画面直通输出
        Graphics.Blit(src, dest);
    }
    #endregion

    //=========================================================================
    // 资源释放销毁逻辑
    //=========================================================================
    #region 资源回收销毁
    /// <summary>
    /// 组件销毁释放资源
    /// </summary>
    private void OnDestroy()
    {
        // 销毁运行时创建的材质实例
        if (_instanceMaterial != null)
            Destroy(_instanceMaterial);

        // 清空单例引用
        if (Instance == this)
            Instance = null;
    }
    #endregion
}