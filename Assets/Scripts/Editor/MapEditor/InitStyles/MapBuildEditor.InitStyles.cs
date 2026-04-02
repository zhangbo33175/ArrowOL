using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        // 样式缓存
        private GUIStyle leftPanelStyle;
        private GUIStyle buttonStyle;
        private GUIStyle tabActiveStyle;
        private GUIStyle scrollViewStyle;
        private GUIStyle statusBarStyle;
        private GUIStyle separatorStyle;
        private GUIStyle darkBoxStyle;
        private GUIStyle labelStyle;
        /// <summary>
        /// 
        /// </summary>
        private GUIStyle itemStyle;
        // 正确唯一的样式
        private GUIStyle _selectedStyle;
        private GUIStyle _normalStyle;
        private void InitStyles()
        {
            // 完全自定义 GUIStyle，不依赖 GUI.skin，避免跨线程问题
            leftPanelStyle = new GUIStyle();
            leftPanelStyle.normal.background = MakeTex(2, 2, new Color(200f, 186f, 186f,97f)); // #C8BABA
            leftPanelStyle.border = new RectOffset(0, 1, 0, 0);
            leftPanelStyle.padding = new RectOffset(0, 0, 0, 0);

            buttonStyle = new GUIStyle();
            buttonStyle.normal.background = MakeTex(2, 2, new Color(0.176f, 0.176f, 0.188f)); // #2D2D30
            buttonStyle.hover.background = MakeTex(2, 2, new Color(0.227f, 0.227f, 0.239f)); // #3A3A3D
            buttonStyle.active.background = MakeTex(2, 2, new Color(0.055f, 0.388f, 0.612f)); // #0E639C
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = Color.white;
            buttonStyle.active.textColor = Color.white;
            buttonStyle.border = new RectOffset(1, 1, 1, 1);
            buttonStyle.padding = new RectOffset(15, 15, 0, 0);
            buttonStyle.fixedHeight = 24;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            tabActiveStyle = new GUIStyle(buttonStyle);
            tabActiveStyle.normal.background = MakeTex(2, 2, new Color(0.055f, 0.388f, 0.612f)); // #0E639C
            tabActiveStyle.fixedHeight = 30;

            scrollViewStyle = new GUIStyle();
            scrollViewStyle.normal.background = MakeTex(2, 2, new Color(0.118f, 0.118f, 0.118f, 0.38f));

            statusBarStyle = new GUIStyle();
            statusBarStyle.normal.background = MakeTex(2, 2, new Color(0.145f, 0.145f, 0.149f)); // #252526
            statusBarStyle.border = new RectOffset(0, 0, 1, 0);
            statusBarStyle.padding = new RectOffset(10, 10, 0, 0);

            separatorStyle = new GUIStyle();
            separatorStyle.normal.background = MakeTex(2, 2, new Color(0.259f, 0.259f, 0.259f)); // #424242
            separatorStyle.fixedHeight = 3;
            separatorStyle.margin = new RectOffset(0, 0, 5, 5);

            darkBoxStyle = new GUIStyle();
            darkBoxStyle.normal.background = MakeTex(2, 2, new Color(0.455f, 0.439f, 0.439f)); // #746F6F
            darkBoxStyle.padding = new RectOffset(0, 0, 0, 0);

            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.padding = new RectOffset(2, 2, 0, 0);
            
            //Item的样式
            itemStyle = new GUIStyle(EditorStyles.helpBox);
            itemStyle.padding = new RectOffset(10, 10, 10, 10);
            itemStyle.margin = new RectOffset(5, 5, 5, 5);
            
            
            _normalStyle = new GUIStyle("Box");
            _selectedStyle = new GUIStyle("Box");
            _selectedStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.5f, 0.8f, 0.3f));
            
        }
        
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}