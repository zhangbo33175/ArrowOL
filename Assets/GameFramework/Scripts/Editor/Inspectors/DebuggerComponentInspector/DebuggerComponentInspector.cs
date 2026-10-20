/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   Debugger组件编辑器面板定制（UIElements 版本）
 ***************************************************************/
using Newtonsoft.Json.Linq;
using Honor.Runtime;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    [CustomEditor(typeof(DebuggerComponent))]
    internal sealed class DebuggerComponentInspector : UnityEditor.Editor
    {
        private VisualElement m_Root;
        private SerializedProperty m_Skin;
        private SerializedProperty m_MiniSkin;
        private SerializedProperty m_PopWindowSkin;
        private SerializedProperty m_StayBorder;
        private SerializedProperty m_EnterIdleTime;
        private SerializedProperty m_ConsoleWindow;

        private string m_AndroidAABAbsolutelyPath = "None";
        private string m_AndroidAPKSAbsolutelyPath = "None";
        private string m_AndroidBundleToolAbsolutelyPath = "None";
        private string m_AndroidSignatureAbsolutelyPath = "None";
        private string m_AndroidSignatureAlias = "None";
        private string m_AndroidSignaturePass = "None";
        private string m_AndroidADBAbsolutelyPath = "None";

        private void OnEnable()
        {
            m_Skin = serializedObject.FindProperty("m_Skin");
            m_MiniSkin = serializedObject.FindProperty("m_MiniSkin");
            m_PopWindowSkin = serializedObject.FindProperty("m_PopWindowSkin");
            m_StayBorder = serializedObject.FindProperty("m_StayBorder");
            m_EnterIdleTime = serializedObject.FindProperty("m_EnterIdleTime");
            m_ConsoleWindow = serializedObject.FindProperty("m_ConsoleWindow");
            ReadAABToolsFromLibraryConfigFile();
        }

        /// <summary>
        /// 本编辑器面板对应的 UXML/USS 资源相对路径（位于 Inspector 目录，不在 Resources 下，
        /// 因此不能使用 Resources.Load，必须走 AssetDatabase.LoadAssetAtPath）。
        /// </summary>
        private const string InspectorUxmlPath =
            "Assets/GameFramework/Scripts/Editor/Inspectors/DebuggerComponentInspector/DebuggerComponentInspectorWindow.uxml";
        private const string InspectorUssPath =
            "Assets/GameFramework/Scripts/Editor/Inspectors/DebuggerComponentInspector/DebuggerComponentInspectorWindow.uss";

        public override VisualElement CreateInspectorGUI()
        {
            // 【修复】资源不在 Resources 目录下，Resources.Load 必然返回 null，导致后续 Q<>() 全部
            // 为 null 并抛 NullReferenceException、Inspector 界面错乱；改用 AssetDatabase 按相对路径加载
            //（与 LuaBehaviourInspector / PersistComponentInspector 的项目内统一写法保持一致）。
            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(InspectorUxmlPath);
            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>(InspectorUssPath);

            // 兜底保护：即使 UXML 缺失也回退到默认 IMGUI 面板，不再抛异常中断 Inspector 渲染
            if (tree == null)
            {
                UnityEngine.Debug.LogWarning(
                    $"[DebuggerComponentInspector] 未找到 UXML：{InspectorUxmlPath}，回退默认 Inspector。");
                return base.CreateInspectorGUI();
            }

            m_Root = tree.CloneTree();
            if (style != null) m_Root.styleSheets.Add(style);

            // 判空保护：CloneTree 正常时元素必定存在，仍按查不到即回退，避免二次崩溃
            var skinField = m_Root.Q<UnityEditor.UIElements.ObjectField>("skinField");
            if (skinField != null)
            {
                skinField.value = m_Skin.objectReferenceValue;
                skinField.RegisterValueChangedCallback(e => { m_Skin.objectReferenceValue = e.newValue; serializedObject.ApplyModifiedProperties(); });
            }

            var miniField = m_Root.Q<UnityEditor.UIElements.ObjectField>("miniSkinField");
            if (miniField != null)
            {
                miniField.value = m_MiniSkin.objectReferenceValue;
                miniField.RegisterValueChangedCallback(e => { m_MiniSkin.objectReferenceValue = e.newValue; serializedObject.ApplyModifiedProperties(); });
            }

            var popField = m_Root.Q<UnityEditor.UIElements.ObjectField>("popSkinField");
            if (popField != null)
            {
                popField.value = m_PopWindowSkin.objectReferenceValue;
                popField.RegisterValueChangedCallback(e => { m_PopWindowSkin.objectReferenceValue = e.newValue; serializedObject.ApplyModifiedProperties(); });
            }

            var stayBorderToggle = m_Root.Q<Toggle>("stayBorderToggle");
            if (stayBorderToggle != null)
            {
                stayBorderToggle.value = m_StayBorder.boolValue;
                stayBorderToggle.RegisterValueChangedCallback(e => { m_StayBorder.boolValue = e.newValue; serializedObject.ApplyModifiedProperties(); });
            }

            var idleTimeField = m_Root.Q<IntegerField>("idleTimeField");
            if (idleTimeField != null)
            {
                idleTimeField.value = m_EnterIdleTime.intValue;
                idleTimeField.RegisterValueChangedCallback(e => { m_EnterIdleTime.intValue = e.newValue; serializedObject.ApplyModifiedProperties(); });
            }

            return m_Root;
        }

        private void OpenFolder(string folderPath)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folderPath.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folderPath);
                    break;
            }
        }

        private void ReadAABToolsFromLibraryConfigFile()
        {
            string dataPath = Path.GetFullPath("Library/AABTools.dat");
            if (File.Exists(dataPath))
            {
                var j = JObject.Parse(File.ReadAllText(dataPath));
                m_AndroidAABAbsolutelyPath = j["AABPath"].ToString();
                m_AndroidAPKSAbsolutelyPath = j["APKSPath"].ToString();
                m_AndroidBundleToolAbsolutelyPath = j["BundleToolPath"].ToString();
                m_AndroidSignatureAbsolutelyPath = j["SignaturePath"].ToString();
                m_AndroidSignatureAlias = j["SignatureAlias"].ToString();
                m_AndroidSignaturePass = j["SignaturePass"].ToString();
                m_AndroidADBAbsolutelyPath = j["ADBPath"].ToString();
            }
        }
    }
}
