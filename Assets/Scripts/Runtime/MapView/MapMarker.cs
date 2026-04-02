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
    [Header("标记配置")]
    public string markerName; // 据点名称
    public int id; // 据点ID
    public Color normalColor = Color.white; // 正常颜色
    public Color highlightColor = Color.yellow; // 高亮颜色
    public UnityEvent OnMarkerClick; // 点击事件（可在Inspector绑定）

    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private bool _isUI = false;

    void Start()
    {
        // 兼容2D Sprite和UI Image两种标记
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

    /// <summary>
    /// 点击标记回调
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 触发点击事件
        OnMarkerClick?.Invoke();
        // 显示据点信息（这里可以扩展为弹出UI面板）
        Debug.Log($"点击了据点：{markerName} (ID:{id})");
        
        // 高亮标记
        SetHighlight(true);
    }

    /// <summary>
    /// 设置标记高亮状态
    /// </summary>
    public void SetHighlight(bool isHighlight)
    {
        if (_isUI && _image != null)
        {
            _image.color = isHighlight ? highlightColor : normalColor;
        }
        else if (_spriteRenderer != null)
        {
            _spriteRenderer.color = isHighlight ? highlightColor : normalColor;
        }
    }

    /// <summary>
    /// 获取标记的世界坐标（用于绘制路径）
    /// </summary>
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// 获取标记的UI坐标（如果是UI标记）
    /// </summary>
    public Vector2 GetUIPosition()
    {
        if (_isUI && _image != null)
        {
            return _image.rectTransform.anchoredPosition;
        }
        return Vector2.zero;
    }
}