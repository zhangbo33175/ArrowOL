/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PersistExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   存档工具窗口（Honor 菜单入口）
 *            提供存档协议导出 + 存档数据管理（数据库持久化 / 文件片段持久化）
 * 调用链：  Open() → OnEnable → OnInitData（创建/初始化检视器）→ OnBindUI → DrawFullInspector
 ***************************************************************/

using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>
    /// 存档工具窗口
    /// <para>作为独立编辑器窗口承载 PersistComponentInspector 的完整存档管理能力：
    /// 存档协议导出（导出 Proto 到 Lua / 打开 Proto 文件夹），以及数据库持久化、
    /// 文件片段持久化的数据编辑与删除。原实现依赖场景中的 PersistManager 物体，
    /// 但因 PersistComponent 已改为纯 C# 单例（无 UnityEngine.Object 实例），无法通过
    /// CreateEditor 绑定 target，故改为直接 new 检视器并调用无 target 初始化入口。</para>
    /// </summary>
    public class PersistExportEditorWindow : BaseToolkitEditorWindow<PersistExportEditorWindow>
    {
        /// <summary>
        /// 存档数据管理检视器实例
        /// <para>PersistComponent 为纯 C# 单例，无 target，直接 new 并调用
        /// <see cref="PersistComponentInspector.InitializeForToolWindow"/> 完成数据加载。</para>
        /// </summary>
        private PersistComponentInspector m_PersistInspector;

        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/PersistComponentInspector/";
        protected override string UxmlName => "PersistExportEditorWindowTool";
        protected override GUIContent Title => new GUIContent("存档工具");
        protected override Vector2 MinSize => new Vector2(600, 500);
        protected override Vector2 MaxSize => new Vector2(600, 500);

        [MenuItem("Honor/存档工具", false, 368)]
        private static void OpenWindow() => Open();

        /// <summary>
        /// 数据初始化：创建并初始化存档数据管理检视器
        /// <para>窗口 UI 构建前调用，确保 DrawFullInspector 渲染时检视器已就绪。</para>
        /// </summary>
        protected override void OnInitData()
        {
            // 【修复】原实现 m_PersistEditor 从未赋值，导致 DrawFullInspector 永远命中
            // "未找到 PersistComponent" 错误分支。PersistComponent 已改为纯 C# 单例，
            // 无法 CreateEditor 绑定，故直接 new 检视器并用无 target 入口初始化。
            m_PersistInspector = new PersistComponentInspector();
            m_PersistInspector.InitializeForToolWindow();
        }

        protected override void OnBindUI()
        {
            BindClick("exportBtn", () =>
            {
                PersistComponentInspector.GenerateAllSaveProtos();
                EditorUtility.DisplayDialog("存档工具", "存档协议导出完成", "确定");
            });

            BindClick("openFolderBtn", () =>
                PersistComponentInspector.OpenProtoFolderStatic());

            BindImgui("debugContainer", DrawFullInspector);
            Q<IMGUIContainer>("debugContainer").style.flexGrow = 1;

            RequestRepaint();
        }

        /// <summary>
        /// 绘制完整存档数据管理面板（数据库持久化 / 文件片段持久化）
        /// </summary>
        private void DrawFullInspector()
        {
            // 兜底保护：检视器初始化失败时给出明确提示，而非静默空界面
            if (m_PersistInspector == null)
            {
                GUILayout.Label("存档数据管理检视器未初始化，请重新打开窗口。");
                return;
            }

            m_PersistInspector.DrawDebugPanels();
        }

        private void OnDisable()
        {
            // 先注销基类的 EditorApplication.update 持续重绘监听（基类 OnDisable 为 protected 非 virtual，
            // 必须显式调用，否则窗口关闭后 update 委托泄漏、每帧空转）
            base.OnDisable();

            // 检视器由 new 创建（非 CreateEditor），直接置空交由 GC 回收，不做 DestroyImmediate
            m_PersistInspector = null;
        }
    }
}
