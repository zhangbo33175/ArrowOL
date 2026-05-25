/***************************************************************
 * (c) copyright 2026 - 2030, 项目版权所有
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapController.cs
 * author:    云毅
 * created:   2026
 * descrip:   帝国OL风格世界地图控制器
 *            实现地图拖拽、滚轮缩放、边界限制、平滑插值功能
 ***************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 帝国OL风格世界地图控制器
/// 实现拖拽、缩放、边界限制核心功能
/// </summary>
public class MapController : MonoBehaviour, IDragHandler, IScrollHandler
{
    #region 序列化配置字段
    //=========================================================================
    // 序列化配置字段
    //=========================================================================
    [Header("地图配置")]
    [Tooltip("地图背景图片的RectTransform")]
    public RectTransform mapBg;

    [Tooltip("地图缩放最小值")]
    public float minScale = 0.8f;

    [Tooltip("地图缩放最大值")]
    public float maxScale = 2.0f;

    [Tooltip("缩放速度")]
    public float scaleSpeed = 0.1f;
    #endregion

    #region 私有运行时字段
    //=========================================================================
    // 私有运行时字段
    //=========================================================================
    /// <summary>
    /// 目标位置（用于平滑插值）
    /// </summary>
    private Vector3 _targetPos;

    /// <summary>
    /// 目标缩放（用于平滑插值）
    /// </summary>
    private Vector3 _targetScale;

    /// <summary>
    /// 画布RectTransform（边界计算基准）
    /// </summary>
    private RectTransform _canvasRect;
    #endregion

    #region 生命周期
    //=========================================================================
    // 生命周期
    //=========================================================================
    private void Start()
    {
        // 获取画布的RectTransform（用于边界计算）
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        
        // 初始化目标位置和缩放
        _targetPos = mapBg.anchoredPosition;
        _targetScale = mapBg.localScale;
    }

    private void Update()
    {
        // 平滑移动和缩放插值
        mapBg.anchoredPosition = Vector3.Lerp(mapBg.anchoredPosition, _targetPos, Time.deltaTime * 10);
        mapBg.localScale = Vector3.Lerp(mapBg.localScale, _targetScale, Time.deltaTime * 5);

        // 限制地图边界（防止拖出屏幕）
        ClampMapPosition();
    }
    #endregion

    #region 输入事件
    //=========================================================================
    // 输入事件
    //=========================================================================
    /// <summary>
    /// 拖拽地图逻辑
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // 忽略UI点击（点击按钮时不拖拽地图）
        if (EventSystem.current.IsPointerOverGameObject()) 
            return;

        _targetPos += (Vector3)eventData.delta;
    }

    /// <summary>
    /// 滚轮缩放地图
    /// </summary>
    public void OnScroll(PointerEventData eventData)
    {
        // 计算新的缩放值
        float scaleDelta = eventData.scrollDelta.y * scaleSpeed;
        float newScale = Mathf.Clamp(_targetScale.x + scaleDelta, minScale, maxScale);

        // 等比缩放
        _targetScale = new Vector3(newScale, newScale, 1);
    }
    #endregion

    #region 地图控制逻辑
    //=========================================================================
    // 地图控制逻辑
    //=========================================================================
    /// <summary>
    /// 限制地图移动边界（确保地图不会完全拖出屏幕）
    /// </summary>
    private void ClampMapPosition()
    {
        // 计算地图和画布的尺寸
        Vector2 canvasSize = _canvasRect.rect.size;
        Vector2 mapSize = new Vector2(
            mapBg.rect.width * mapBg.localScale.x,
            mapBg.rect.height * mapBg.localScale.y
        );

        // 计算X轴边界
        float minX = (canvasSize.x - mapSize.x) / 2;
        float maxX = -minX;

        // 计算Y轴边界
        float minY = (canvasSize.y - mapSize.y) / 2;
        float maxY = -minY;

        // 限制位置
        _targetPos.x = Mathf.Clamp(_targetPos.x, minX, maxX);
        _targetPos.y = Mathf.Clamp(_targetPos.y, minY, maxY);
    }

    /// <summary>
    /// 重置地图到初始位置和缩放
    /// </summary>
    public void ResetMap()
    {
        _targetPos = Vector2.zero;
        _targetScale = Vector3.one;
    }
    #endregion
}