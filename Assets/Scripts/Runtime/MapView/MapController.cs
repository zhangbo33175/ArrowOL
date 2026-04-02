using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 帝国OL风格世界地图控制器
/// 实现拖拽、缩放、边界限制核心功能
/// </summary>
public class MapController : MonoBehaviour,IDragHandler, IScrollHandler
{
    [Header("地图配置")] [Tooltip("地图背景图片的RectTransform")]
    public RectTransform mapBg;

    [Tooltip("地图缩放最小值")] public float minScale = 0.8f;
    [Tooltip("地图缩放最大值")] public float maxScale = 2.0f;
    [Tooltip("缩放速度")] public float scaleSpeed = 0.1f;

    // 私有变量
    private Vector3 _targetPos;
    private Vector3 _targetScale;
    private RectTransform _canvasRect;

    void Start()
    {
        var sss = GetComponentInParent<Canvas>();
        // 获取画布的RectTransform（用于边界计算）
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        // 初始化目标位置和缩放
        _targetPos = mapBg.anchoredPosition;
        _targetScale = mapBg.localScale;
    }

    void Update()
    {
        // 平滑移动和缩放（避免瞬移）
        mapBg.anchoredPosition = Vector3.Lerp(mapBg.anchoredPosition, _targetPos, Time.deltaTime * 10);
        mapBg.localScale = Vector3.Lerp(mapBg.localScale, _targetScale, Time.deltaTime * 5);

        // 限制地图边界（防止拖出屏幕）
        ClampMapPosition();
    }

    /// <summary>
    /// 拖拽地图逻辑
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // 忽略UI点击（比如点击按钮时不拖拽地图）
        if (EventSystem.current.IsPointerOverGameObject()) return;

        _targetPos+=(Vector3)eventData.delta;
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
}