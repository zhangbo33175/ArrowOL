/***************************************************************
 * (c) copyright 2026 - 2030, 项目版权所有
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapMarker.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图据点标记组件
 *            支持 2D Sprite / UI Image，提供点击、高亮、坐标获取功能
 ***************************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 地图标记（城市/据点）
/// 支持点击、高亮、显示信息
/// </summary>
public class MapMarker : MonoBehaviour, IPointerClickHandler
{
    #region 序列化配置字段
    //=========================================================================
    // 序列化配置字段
    //=========================================================================
    [Header("标记配置")]
    [Tooltip("据点显示名称")]
    public string markerName;

    [Tooltip("据点唯一ID")]
    public int id;

    [Tooltip("正常状态颜色")]
    public Color normalColor = Color.white;

    [Tooltip("高亮选中颜色")]
    public Color highlightColor = Color.yellow;

    [Tooltip("点击触发事件（Inspector可绑定）")]
    public UnityEvent OnMarkerClick;
    #endregion

    #region 私有字段
    //=========================================================================
    // 私有字段
    //=========================================================================
    /// <summary>
    /// 2D精灵渲染器
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// UI图片组件
    /// </summary>
    private Image _image;

    /// <summary>
    /// 是否为UI类型标记
    /// </summary>
    private bool _isUI;
    #endregion

    #region 生命周期
    //=========================================================================
    // 生命周期
    //=========================================================================
    private void Start()
    {
        // 自动识别是2D物体还是UI物体
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();

        if (_image != null)
        {
            _isUI = true;
            _image.color = normalColor;
        }
        else if (_spriteRenderer != null)
        {
            _spriteRenderer.color = normalColor;
        }
    }
    #endregion

    #region 点击事件
    //=========================================================================
    // 点击事件
    //=========================================================================
    /// <summary>
    /// 点击标记回调
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 触发绑定的点击事件
        OnMarkerClick?.Invoke();
        
        // 打印据点信息
        Debug.Log($"点击了据点：{markerName} (ID:{id})");
        
        // 高亮当前标记
        SetHighlight(true);
    }
    #endregion

    #region 公共控制方法
    //=========================================================================
    // 公共控制方法
    //=========================================================================
    /// <summary>
    /// 设置标记高亮状态
    /// </summary>
    public void SetHighlight(bool isHighlight)
    {
        Color targetColor = isHighlight ? highlightColor : normalColor;

        if (_isUI && _image != null)
        {
            _image.color = targetColor;
        }
        else if (_spriteRenderer != null)
        {
            _spriteRenderer.color = targetColor;
        }
    }

    /// <summary>
    /// 获取标记世界坐标（用于绘制路径）
    /// </summary>
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// 获取标记UI锚点坐标（仅UI类型有效）
    /// </summary>
    public Vector2 GetUIPosition()
    {
        if (_isUI && _image != null)
        {
            return _image.rectTransform.anchoredPosition;
        }
        
        return Vector2.zero;
    }
    #endregion
}