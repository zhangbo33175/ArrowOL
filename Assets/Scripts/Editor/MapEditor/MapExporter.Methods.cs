using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLib.MapEditor
{
    public sealed partial class MapExporter
    {
        /// <summary>
        /// 绘制分割线
        /// </summary>
        public void Draw_Left_Split_Line(Rect splitLineRect)
        {
            GUILayout.BeginArea(splitLineRect);
            // 绘制分割线背景（选中时变色）
            GUI.backgroundColor = _isDraggingSplitLine ? Color.gray : new Color(0.3f, 0.3f, 0.3f);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUI.backgroundColor = Color.white; // 恢复默认背景色
            GUILayout.EndArea();
        }

        // 添加功能按钮
        public void AddToolButton(Toolbar parent, string text)
        {
            Button btn = new Button(() => Debug.Log($"点击功能按钮: {text}")) { text = text };
            btn.style.minWidth = 20;
            btn.style.marginRight = 2;
            parent.Add(btn);
        }
        // 添加图层复选框
        public void AddLayerToggle(Toolbar parent, string label)
        {
            Toggle toggle = new Toggle(label);
            toggle.style.color = Color.white;
            toggle.value = true; // 默认选中
            parent.Add(toggle);
        }

    }
}