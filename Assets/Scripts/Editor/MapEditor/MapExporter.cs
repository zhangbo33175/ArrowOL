using UnityEditor;
using UnityEngine;

namespace GameLib.MapEditor
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MapExporter : EditorWindow
    {
        // MapExporter.Visitors
      
        
        [MenuItem("Tools/地图编辑器", false, 10000)]
        public static void ShowExample()
        {
            MapExporter wnd = GetWindow<MapExporter>();
            wnd.titleContent = new GUIContent("地图编辑器");
            wnd.maxSize = new Vector2(1334, 860);
            wnd.minSize = new Vector2(1334, 860);
        }

        void OnEnable()
        {
          
        }


        void OnGUI()
        {
            //创建左边信息
            Draw_Left_Panel();
            //创建右边信息
            Draw_Right_Panel();
        }

     
        void OnInspectorUpdate()
        {
            // 实时更新鼠标坐标
            // mousePos = Event.current.mousePosition;
            //Repaint();
        }
    }
}
