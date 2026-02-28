using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLib.MapEditor
{
    public sealed partial class MapExporter
    {
        /*/// <summary>
        /// 窗口矩形
        /// </summary>
        private Rect windowRect;
        /// <summary>
        /// 顶部画布矩形
        /// </summary>
        private Rect topPanelRect;
        /// <summary>
        /// 分割线矩形
        /// </summary>
        private Rect splitLineRect;
        /// <summary>
        /// 底部画布矩形
        /// </summary>
        private Rect bottomPanelRect;*/
        /// <summary>
        /// 绘制右边界面
        /// </summary>
        public void Draw_Right_Panel()
        {
            // 图层选择（复选框）
            Toolbar layerToolbar = new Toolbar();
            layerToolbar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            layerToolbar.style.marginTop = 2;

            layerToolbar.Add(new Label("图层: "));
            AddLayerToggle(layerToolbar, "摆放物图层");
            AddLayerToggle(layerToolbar, "默认图层");
            AddLayerToggle(layerToolbar, "物件层");
            AddLayerToggle(layerToolbar, "背景层");
            
        }
    }
}