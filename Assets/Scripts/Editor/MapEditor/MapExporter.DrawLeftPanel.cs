using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace GameLib.MapEditor
{
    public sealed partial class MapExporter
    {
        /// <summary>
        /// 窗口矩形
        /// </summary>
        private Rect window_Rect_Left;
        /// <summary>
        /// 顶部画布矩形
        /// </summary>
        private Rect top_Panel_Rect_Left;
        /// <summary>
        /// 分割线矩形
        /// </summary>
        private Rect split_Line_Rect_Left;
        /// <summary>
        /// 底部画布矩形
        /// </summary>
        private Rect bottom_Panel_Rect_Left;

        private Toolbar tabToolbar;
        /// <summary>
        /// 绘制左边界面
        /// </summary>
        private void Draw_Left_Panel()
        {
            tabToolbar = new Toolbar();
            tabToolbar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            //1.设置Rect信息
            On_Set_Rect_Left();
            // 2. 处理分割线拖动逻辑（检测鼠标事件）
            //HandLeftleSplitLineDrag(splitLineRect);

            // 3. 绘制顶部 Panel（使用 BeginArea 限定区域）
            Draw_Left_Top_Panel(top_Panel_Rect_Left);

            // 4. 绘制分割线
            Draw_Left_Split_Line(split_Line_Rect_Left);

            // 5. 绘制底部 Panel（使用 BeginArea 限定区域）
            Draw_Left_Bottom_Panel(bottom_Panel_Rect_Left);

            // 6. 限制顶部面板高度，避免超出窗口范围
            _topPanelHeight = Mathf.Clamp(_topPanelHeight, 100f, window_Rect_Left.height - 100f - _splitLineHeight);
        }

        /// <summary>
        /// 设置Rect信息
        /// </summary>
        private void On_Set_Rect_Left()
        {
            window_Rect_Left = this.position;
            // 顶部面板矩形（左上角开始，宽度适配窗口，高度固定/可配置）
            top_Panel_Rect_Left = new Rect(
                0, // 左偏移
                0, // 上偏移
                200, // 宽度
                _topPanelHeight // 高度
            );
            // 分割线矩形（紧跟顶部面板下方）
            split_Line_Rect_Left = new Rect(
                0,
                _topPanelHeight,
                200,
                _splitLineHeight
            );
            // 底部面板矩形（紧跟分割线下方，占满剩余区域）
            bottom_Panel_Rect_Left = new Rect(
                0,
                _topPanelHeight + _splitLineHeight,
                200,
                window_Rect_Left.height - _topPanelHeight - _splitLineHeight
            );
            
        }



        /// <summary>
        /// 绘制底部 Panel（指定区域）
        /// </summary>
        private void Draw_Left_Bottom_Panel(Rect panelRect)
        {
            GUILayout.BeginArea(panelRect);
            EditorGUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="panelRect"></param>
        private void Draw_Left_Top_Panel(Rect panelRect)
        {
            // 核心：使用 BeginArea 限定控件绘制区域
            GUILayout.BeginArea(panelRect);
            EditorGUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
         
            EditorGUILayout.BeginHorizontal();
            
            // 功能按钮组
            ToolbarSpacer spacer = new ToolbarSpacer { flex = true };
            tabToolbar.Add(spacer);
            
            AddToolButton(tabToolbar, "导出设置");
            AddToolButton(tabToolbar, "保存");
            AddToolButton(tabToolbar, "清空摆饰物");
            AddToolButton(tabToolbar, "清空格子数据");
            
            EditorGUILayout.EndHorizontal(); 

            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}