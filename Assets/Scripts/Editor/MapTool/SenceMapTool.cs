/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SenceMapTool.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景地图编辑器主窗口 - 公有接口部分（菜单入口/创建GUI）
 ***************************************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景地图编辑器窗口（UIToolkit） - 公有接口部分
    /// 编辑器菜单入口与 UXML / USS 装配入口
    /// </summary>
    public partial class SenceMapTool : EditorWindow
    {
        /// <summary>
        /// 打开窗口
        /// </summary>
        [MenuItem("Honor/Map Tool/场景地图编辑器")]
        public static void ShowExample()
        {
            SenceMapTool wnd = GetWindow<SenceMapTool>();
            wnd.titleContent = new GUIContent("场景地图编辑器");
            wnd.minSize = new Vector2(1700, 860);
            wnd.maxSize = new Vector2(1700, 860);
        }

        /// <summary>
        /// 创建GUI：加载样式表与视觉树，装配各编辑模块
        /// </summary>
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            // 添加样式表
            if (m_StyleSheet != null)
                root.styleSheets.Add(m_StyleSheet);

            // 实例化 UXML
            if (m_VisualTreeAsset == null)
                return;

            VisualElement visualTree = m_VisualTreeAsset.Instantiate();
            root.Add(visualTree);

            // 查找切页按钮 / 初始化模块 / 绑定点击事件
            FindElements(visualTree);
            InitModules(visualTree);
            BindTabEvents();
        }
    }
}
