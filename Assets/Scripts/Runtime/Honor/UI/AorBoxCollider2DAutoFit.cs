/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorBoxCollider2DAutoFit.cs
 * author:    云毅
 * created:   2026
 * descrip:   2D碰撞盒自动适配组件，自动根据Image精灵尺寸适配UI大小与碰撞盒尺寸
 ***************************************************************/
using Honor.Runtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2D碰撞盒自动适配组件
/// <para>自动将Image设置为原生尺寸，并同步更新BoxCollider2D的尺寸与偏移</para>
/// <para>支持运行时精灵切换自动刷新、编辑器内实时预览适配效果</para>
/// </summary>
[RequireComponent(typeof(Image), typeof(BoxCollider2D), typeof(RectTransform))]
public class AorBoxCollider2DAutoFit : MonoBehaviour
{
    #region 私有字段
    /// <summary>
    /// 图片组件，用于渲染精灵与设置原生尺寸
    /// </summary>
    private Image _img;

    /// <summary>
    /// UI矩形变换组件，用于获取当前UI实际尺寸
    /// </summary>
    private RectTransform _rt;

    /// <summary>
    /// 2D盒碰撞组件，用于自动适配尺寸
    /// </summary>
    private BoxCollider2D _col;

    /// <summary>
    /// 上一帧的精灵对象，用于检测精灵是否发生变更
    /// </summary>
    private Sprite _lastSprite;
    #endregion

    #region Unity生命周期函数
    //=========================================================================
    // 组件初始化阶段
    //=========================================================================
    /// <summary>
    /// 唤醒时获取组件引用
    /// </summary>
    private void Awake()
    {
        _img = GetComponent<Image>();
        _rt = GetComponent<RectTransform>();
        _col = GetComponent<BoxCollider2D>();
    }

    //=========================================================================
    // 游戏启动阶段
    //=========================================================================
    /// <summary>
    /// 启动时初始化精灵状态并执行首次适配
    /// </summary>
    private void Start()
    {
        _lastSprite = _img.sprite;
        Fit();
    }

    //=========================================================================
    // 每帧更新逻辑
    //=========================================================================
    /// <summary>
    /// 每帧检测精灵是否变更，若变更则重新执行适配
    /// </summary>
    private void Update()
    {
        // 检测图片精灵是否被替换
        if (_img.sprite != _lastSprite)
        {
            _lastSprite = _img.sprite;
            Fit();
        }
    }

    //=========================================================================
    // 编辑器验证与预览
    //=========================================================================
    /// <summary>
    /// 编辑器模式下验证/修改参数时自动调用，实现实时预览适配效果
    /// </summary>
    private void OnValidate()
    {
        // 自动补全组件引用，防止空引用异常
        if (_img == null) _img = GetComponent<Image>();
        if (_rt == null) _rt = GetComponent<RectTransform>();
        if (_col == null) _col = GetComponent<BoxCollider2D>();
        
        Fit();
    }
    #endregion

    #region 核心适配方法
    //=========================================================================
    // 核心适配逻辑：图片原生尺寸 + 碰撞盒同步适配
    //=========================================================================
    /// <summary>
    /// 自动适配图片尺寸与碰撞盒尺寸
    /// </summary>
    private void Fit()
    {
        // 无精灵时直接返回，避免空引用
        if (_img.sprite == null)
            return;

        // 将Image设置为图片原始尺寸
        _img.SetNativeSize();

        // 同步碰撞盒尺寸与UI尺寸，偏移量归零
        _col.size = _rt.rect.size;
        _col.offset = Vector2.zero;
    }
    #endregion
}