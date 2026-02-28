using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLib.MapEditor
{
    public sealed partial class MapExporter
    {
        // 顶部面板固定高度（也可改为可配置）
        private float _topPanelHeight = 200f;
        // 分割线高度（用于点击拖动调整顶部面板高度）
        private readonly float _splitLineHeight = 5f;
        // 是否正在拖动分割线
        private bool _isDraggingSplitLine = false;
        
     
       
    }
}