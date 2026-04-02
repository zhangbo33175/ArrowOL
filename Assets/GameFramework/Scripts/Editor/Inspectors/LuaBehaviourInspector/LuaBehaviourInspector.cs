using System;
using System.Collections.Generic;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(LuaBehaviour))]
    internal sealed partial class LuaBehaviourInspector : HonorComponentInspector
    {
        private List<LuaInjection.InjectionType> m_InfoExTypeNameTempletes = null;
        private List<string> m_InfoExCmdTempletes = null;
        private List<string> m_InfoExFunctionNameTempletes = null;
        private List<string> m_InfoExFunctionParamTempletes = null;
        private List<string> m_InfoExEventTriggerCmdTempletes = null;

        private SerializedProperty m_PatternType = null;
        private SerializedProperty m_PrefabType = null;
        private SerializedProperty m_UseProc = null;
        private SerializedProperty m_MaskLayer = null;
        private SerializedProperty m_BottomCloseLayer = null;
        private SerializedProperty m_UseCollider2DLifeCycles = null;
        private SerializedProperty m_UseCollider3DLifeCycles = null;
        private SerializedProperty m_UseTrigger2DLifeCycles = null;
        private SerializedProperty m_UseTrigger3DLifeCycles = null;
        private SerializedProperty m_OpenAnimation = null;
        private SerializedProperty m_CloseAnimation = null;
        private SerializedProperty m_ShowRaycastTargetsGizmos = null;
        private SerializedProperty m_LuaAuthorName = null;
        private SerializedProperty m_LuaDescript = null;
        private SerializedProperty m_Collider2DLifeCyclesBehaviour = null;
        private SerializedProperty m_Collider3DLifeCyclesBehaviour = null;
        private SerializedProperty m_Trigger2DLifeCyclesBehaviour = null;
        private SerializedProperty m_Trigger3DLifeCyclesBehaviour = null;

        private SerializedProperty m_Injections = null;
        private List<SerializedProperty> m_InterInjectionComments = null;
        private List<SerializedProperty> m_InterInjectionTypeNames = null;
        private List<SerializedProperty> m_InterInjectionNames = null;
        private List<SerializedProperty> m_InterInjectionIsArrays = null;

        private List<SerializedProperty> m_InterInjectionObjs = null;
        private List<SerializedProperty> m_InterInjectionVariants = null;
        private List<SerializedProperty> m_InterInjectionInfoExs = null;
        private List<SerializedProperty> m_InterInjectionExtendsEnabled = null;
        private List<SerializedProperty> m_InterInjectionExtends = null;

        private List<SerializedProperty> m_InterInjectionElementsObjs = null;
        private List<SerializedProperty> m_InterInjectionElementsVariants = null;
        private List<SerializedProperty> m_InterInjectionElementsInfoExs = null;
        private List<SerializedProperty> m_InterInjectionElementsExtendsEnableds = null;
        private List<SerializedProperty> m_InterInjectionElementsExtends = null;

        private int m_InnerInjectionInsertPosIndex = -1;
        private int m_InnerInjectionDeletePosIndex = -1;
        private int m_InnerInjectionUpwardOriPosIndex = -1;
        private int m_InnerInjectionUpwardTargetPosIndex = -1;
        private int m_InnerInjectionDownwardOriPosIndex = -1;
        private int m_InnerInjectionDownwardTargetPosIndex = -1;

        private Texture2D m_InjectionItemBG = null;
        private GUIStyle m_InjectionItemStyle = null;

        private Texture2D m_BindItemBG = null;
        private GUIStyle m_BindItemStyle = null;

        private HashSet<string> m_OpenedItems;
        private bool m_LuaCanGenerate = false;

        private void OnEnable()
        {
            // 为注入对象添加lua代码模板参数（方便日后随时添加新的模板参数，保证正常生成对应的Lua代码）
            m_InfoExTypeNameTempletes = new List<LuaInjection.InjectionType>();
            m_InfoExCmdTempletes = new List<string>();
            m_InfoExFunctionNameTempletes = new List<string>();
            m_InfoExFunctionParamTempletes = new List<string>();
            m_InfoExEventTriggerCmdTempletes = new List<string>();

            m_InjectionItemBG = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/Framework/Textures/PicsForEditor/InspectorItemBg1.png", typeof(Texture2D));
            m_InjectionItemStyle = new GUIStyle();
            m_InjectionItemStyle.normal.background = m_InjectionItemBG;

            m_BindItemBG = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/Framework/Textures/PicsForEditor/InspectorItemBg2.png", typeof(Texture2D));
            m_BindItemStyle = new GUIStyle();
            m_BindItemStyle.normal.background = m_BindItemBG;

            m_OpenedItems = new HashSet<string>();

            // 给注入对象添加lua代码模板参数
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_Button, "Clicked", "On{0}Clicked", "args");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_InputField, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_InputField, "End", "On{0}EndEdit", "valueEndEdit");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_InputField_TextMeshPro, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_InputField_TextMeshPro, "End", "On{0}EndEdit", "valueEndEdit");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_Dropdown, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_Dropdown_TextMeshPro, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_Toggle, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_Slider, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_ScrollRect, "Changed", "On{0}ValueChanged", "valueChanged");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_Tree, "Chosen", "On{0}Chosen", "treeIndexDesc");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_ListView, "GettingItem", "On{0}GettingItem", "itemIndex");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_ListView, "Start", "On{0}Start", string.Empty);
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_ListView, "SnapItemFinished", "On{0}SnapItemFinished", "item");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_ListView, "SnapNearestChanged", "On{0}SnapNearestChanged", "item");
            RegistInfoExCmdTemplete(LuaInjection.InjectionType.UI_SwitchButton, "Changed", "On{0}ValueChanged", "valueChanged");

            m_InfoExEventTriggerCmdTempletes.Add("Evt_BeginDrag");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Cancel");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Deselect");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Drag");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Drop");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_EndDrag");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_InitializePotentialDrag");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Move");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_PointerClick");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_PointerDown");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_PointerEnter");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_PointerExit");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_PointerUp");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Scroll");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Select");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_Submit");
            m_InfoExEventTriggerCmdTempletes.Add("Evt_UpdateSelected");

            // 获取当前设计模式类型
            m_PatternType = serializedObject.FindProperty("m_PatternType");
            
            // 获取当前预制体类型
            m_PrefabType = serializedObject.FindProperty("m_PrefabType");

            // 初始化None设计模式
            InitPatternNone();

            // 初始化MVVM设计模式
            InitPatternMVVM();

            // 界面配置项属性获取
            m_UseProc = serializedObject.FindProperty("m_UseProc");
            m_MaskLayer = serializedObject.FindProperty("m_MaskLayer");
            m_BottomCloseLayer = serializedObject.FindProperty("m_BottomCloseLayer");
            m_UseCollider2DLifeCycles = serializedObject.FindProperty("m_UseCollider2DLifeCycles");
            m_UseCollider3DLifeCycles = serializedObject.FindProperty("m_UseCollider3DLifeCycles");
            m_UseTrigger2DLifeCycles = serializedObject.FindProperty("m_UseTrigger2DLifeCycles");
            m_UseTrigger3DLifeCycles = serializedObject.FindProperty("m_UseTrigger3DLifeCycles");
            m_OpenAnimation = serializedObject.FindProperty("m_OpenAnimation");
            m_CloseAnimation = serializedObject.FindProperty("m_CloseAnimation");
            m_ShowRaycastTargetsGizmos = serializedObject.FindProperty("m_ShowRaycastTargetsGizmos");
            m_LuaAuthorName = serializedObject.FindProperty("m_LuaAuthorName");
            m_LuaDescript = serializedObject.FindProperty("m_LuaDescript");
            m_Collider2DLifeCyclesBehaviour = serializedObject.FindProperty("m_Collider2DLifeCyclesBehaviour");
            m_Collider3DLifeCyclesBehaviour = serializedObject.FindProperty("m_Collider3DLifeCyclesBehaviour");
            m_Trigger2DLifeCyclesBehaviour = serializedObject.FindProperty("m_Trigger2DLifeCyclesBehaviour");
            m_Trigger3DLifeCyclesBehaviour = serializedObject.FindProperty("m_Trigger3DLifeCyclesBehaviour");

            // 作者描述值初始化
            if (string.IsNullOrEmpty(m_LuaAuthorName.stringValue)) m_LuaAuthorName.stringValue = "???";
            if (string.IsNullOrEmpty(m_LuaDescript.stringValue)) m_LuaDescript.stringValue = "???";

            // 获取注入集合属性
            m_InterInjectionComments = new List<SerializedProperty>();
            m_InterInjectionTypeNames = new List<SerializedProperty>();
            m_InterInjectionNames = new List<SerializedProperty>();
            m_InterInjectionIsArrays = new List<SerializedProperty>();

            m_InterInjectionObjs = new List<SerializedProperty>();
            m_InterInjectionVariants = new List<SerializedProperty>();
            m_InterInjectionInfoExs = new List<SerializedProperty>();
            m_InterInjectionExtendsEnabled = new List<SerializedProperty>();
            m_InterInjectionExtends = new List<SerializedProperty>();

            m_InterInjectionElementsObjs = new List<SerializedProperty>();
            m_InterInjectionElementsVariants = new List<SerializedProperty>();
            m_InterInjectionElementsInfoExs = new List<SerializedProperty>();
            m_InterInjectionElementsExtendsEnableds = new List<SerializedProperty>();
            m_InterInjectionElementsExtends = new List<SerializedProperty>();

            // 根据注入集合属性进行程序内部的分类分组存放
            m_Injections = serializedObject.FindProperty("m_Injections");
            for (int index = 0; index < m_Injections.arraySize; index++)
            {
                SerializedProperty injection = m_Injections.GetArrayElementAtIndex(index);
                m_InterInjectionComments.Add(injection.FindPropertyRelative("Comment"));
                m_InterInjectionTypeNames.Add(injection.FindPropertyRelative("InjectionTypeName"));
                m_InterInjectionNames.Add(injection.FindPropertyRelative("Name"));
                m_InterInjectionIsArrays.Add(injection.FindPropertyRelative("IsArray"));

                m_InterInjectionObjs.Add(injection.FindPropertyRelative("Obj"));
                m_InterInjectionVariants.Add(injection.FindPropertyRelative("Variant"));
                m_InterInjectionInfoExs.Add(injection.FindPropertyRelative("InfoEx"));
                m_InterInjectionExtendsEnabled.Add(injection.FindPropertyRelative("ExtendsEnabled"));
                m_InterInjectionExtends.Add(injection.FindPropertyRelative("Extends"));

                m_InterInjectionElementsObjs.Add(injection.FindPropertyRelative("ElementsObjs"));
                m_InterInjectionElementsVariants.Add(injection.FindPropertyRelative("ElementsVariants"));
                m_InterInjectionElementsInfoExs.Add(injection.FindPropertyRelative("ElementsInfoExs"));
                m_InterInjectionElementsExtendsEnableds.Add(injection.FindPropertyRelative("ElementsExtendsEnableds"));
                m_InterInjectionElementsExtends.Add(injection.FindPropertyRelative("ElementsExtends"));
            }

            // 应用变化的属性
            serializedObject.ApplyModifiedProperties();

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 处理注入信息的行操作
            DisposeInjectionLineOperation();

            // 处理绑定数据的行操作
            DisposeBindValueLineOperation();

            // 是否可以生成Lua标记位
            m_LuaCanGenerate = false;

            // 设计模式功能选项
            m_PatternType.enumValueIndex = EditorGUILayout.Popup("设计模式", m_PatternType.enumValueIndex, Enum.GetNames(typeof(PatternType)));

            // 预制体类型功能选项
            switch ((PatternType)m_PatternType.enumValueIndex)
            {
                case PatternType.None: m_PrefabType.enumValueIndex = EditorGUILayout.Popup("预制体类型", m_PrefabType.enumValueIndex, Enum.GetNames(typeof(Runtime.PrefabType))); break;
                case PatternType.MVVM: m_PrefabType.enumValueIndex = EditorGUILayout.Popup("预制体类型", 0, new string[] { Runtime.PrefabType.UI.ToString() }); break;
            }

            EditorGUILayout.Separator();
            
            // Lua脚本名称GUI
            if((PatternType)m_PatternType.enumValueIndex == PatternType.None)
            {
                OnPatternNoneLuaScriptNameInspectorGUI();
            }
            else if((PatternType)m_PatternType.enumValueIndex == PatternType.MVVM)
            {
                OnPatternMVVMLuaScriptNameInspectorGUI();
            }

            EditorGUILayout.Separator();

            // "使用Proc"功能选项
            m_UseProc.boolValue = EditorGUILayout.Toggle("使用Proc", m_UseProc.boolValue);
            EditorGUILayout.HelpBox("LuaBehaviour已禁用了Update，由Proc取代。\n若不使用Proc，请关闭该选项，心跳将不会被调用，以节省回调开销。\n若使用Proc，心跳的调用顺序将由Proc的调用时机控制。\n所有形式的LuaBehaviour（默认挂载与动态挂载）均由框架自动管理，无需自行调用。", MessageType.Info);

            EditorGUILayout.Separator();

            if ((Runtime.PrefabType)m_PrefabType.enumValueIndex == Runtime.PrefabType.UI)
            {
                // "使用前后遮罩层"功能选项
                m_MaskLayer.boolValue = EditorGUILayout.Toggle("使用前后遮罩层", m_MaskLayer.boolValue);
                EditorGUILayout.HelpBox("注意：使用前后遮罩层，Awake时前后层遮罩将会被激活，不同阶段过程中将产生屏蔽触摸的作用。", MessageType.Info);
                // "使用关闭背景层"功能选项
                m_BottomCloseLayer.boolValue = EditorGUILayout.Toggle("使用关闭背景层", m_BottomCloseLayer.boolValue);
                EditorGUILayout.HelpBox("注意：使用关闭背景层，入场动画稳定时该层会被激活，点击UI界面非底板区域时将触发UI关闭逻辑。", MessageType.Info);
            }

            EditorGUILayout.Separator();

            // "注入Collider2D/3D生命周期函数"功能选项
            GUILayout.BeginHorizontal("box");
            {
                m_UseCollider2DLifeCycles.boolValue = EditorGUILayout.Toggle("注入Collider2D生命周期函数", m_UseCollider2DLifeCycles.boolValue);
                if (m_UseCollider2DLifeCycles.boolValue)
                {
                    m_Collider2DLifeCyclesBehaviour.objectReferenceValue = ((LuaBehaviour)target).gameObject.GetOrAddComponent<Collider2DLifeCyclesBehaviour>();
                }
                else
                {
                    DestroyImmediate(((LuaBehaviour)target).gameObject.GetComponent<Collider2DLifeCyclesBehaviour>());
                    m_Collider2DLifeCyclesBehaviour.objectReferenceValue = null;
                }

                m_UseCollider3DLifeCycles.boolValue = EditorGUILayout.Toggle("注入Collider3D生命周期函数", m_UseCollider3DLifeCycles.boolValue);
                if (m_UseCollider3DLifeCycles.boolValue)
                {
                    m_Collider3DLifeCyclesBehaviour.objectReferenceValue = ((LuaBehaviour)target).gameObject.GetOrAddComponent<Collider3DLifeCyclesBehaviour>();
                }
                else
                {
                    DestroyImmediate(((LuaBehaviour)target).gameObject.GetComponent<Collider3DLifeCyclesBehaviour>());
                    m_Collider3DLifeCyclesBehaviour.objectReferenceValue = null;
                }

            }
            GUILayout.EndHorizontal();

            // "注入Trigger2D/3D生命周期函数"功能选项
            GUILayout.BeginHorizontal("box");
            {
                m_UseTrigger2DLifeCycles.boolValue = EditorGUILayout.Toggle("注入Trigger2D生命周期函数", m_UseTrigger2DLifeCycles.boolValue);
                if (m_UseTrigger2DLifeCycles.boolValue)
                {
                    m_Trigger2DLifeCyclesBehaviour.objectReferenceValue = ((LuaBehaviour)target).gameObject.GetOrAddComponent<Trigger2DLifeCyclesBehaviour>();
                }
                else
                {
                    DestroyImmediate(((LuaBehaviour)target).gameObject.GetComponent<Trigger2DLifeCyclesBehaviour>());
                    m_Trigger2DLifeCyclesBehaviour.objectReferenceValue = null;
                }

                m_UseTrigger3DLifeCycles.boolValue = EditorGUILayout.Toggle("注入Trigger3D生命周期函数", m_UseTrigger3DLifeCycles.boolValue);
                if (m_UseTrigger3DLifeCycles.boolValue)
                {
                    m_Trigger3DLifeCyclesBehaviour.objectReferenceValue = ((LuaBehaviour)target).gameObject.GetOrAddComponent<Trigger3DLifeCyclesBehaviour>();
                }
                else
                {
                    DestroyImmediate(((LuaBehaviour)target).gameObject.GetComponent<Trigger3DLifeCyclesBehaviour>());
                    m_Trigger3DLifeCyclesBehaviour.objectReferenceValue = null;
                }
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            // "使用打开关闭动画"功能选项
            GUILayout.BeginHorizontal("box");
            {
                m_OpenAnimation.boolValue = EditorGUILayout.Toggle("使用打开/出场动画", m_OpenAnimation.boolValue);
                m_CloseAnimation.boolValue = EditorGUILayout.Toggle("使用关闭/退场动画", m_CloseAnimation.boolValue);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            m_ShowRaycastTargetsGizmos.boolValue = EditorGUILayout.Toggle("绘制Gizmo-RaycastTargets", m_ShowRaycastTargetsGizmos.boolValue);
            EditorGUILayout.HelpBox("1.绘制Gizmos信息以辅助提示当前对象所有子节点中的raycastTarget组件。\n2.Android/iOS移动设备该选项会被自动禁用。\n3.该选项默认关闭。", MessageType.Info);

            EditorGUILayout.Separator();

            // "Lua作者描述"输入框
            if (string.IsNullOrEmpty(m_LuaAuthorName.stringValue)) GUI.color = Color.red;
            m_LuaAuthorName.stringValue = EditorGUILayout.TextField("Lua脚本作者", m_LuaAuthorName.stringValue);
            GUI.color = Color.white;

            if (string.IsNullOrEmpty(m_LuaDescript.stringValue)) GUI.color = Color.red;
            m_LuaDescript.stringValue = EditorGUILayout.TextField("Lua脚本描述", m_LuaDescript.stringValue);
            GUI.color = Color.white;

            EditorGUILayout.Separator();

            // 展示注入信息表头信息
            OnInjectionListHeaderInspectorGUI();

            // 展示注入信息格式说明
            OnInjectionIntroductionsInspectorGUI();

            // 注入信息列表条目详情
            OnInjectionItemListInspectorGUI();

            EditorGUILayout.Separator();

            if ((PatternType)m_PatternType.enumValueIndex == PatternType.MVVM)
            {
                // 展示MVVM设计模式下的绑定数据表头信息
                OnPatternMVVMBindValueListHeaderInspectorGUI();

                // 展示MVVM设计模式下的绑定数据格式说明GUI
                OnPatterMVVMBindValueIntroductionsInspectorGUI();

                // 展示MVVM设计模式下的绑定数据列表条目详情
                OnPatternMVVMBindValueListInspectorGUI();
            }

            EditorGUILayout.Separator();

            // 生成/刷新脚本按钮
            if (m_LuaCanGenerate)
            {
                if (GUILayout.Button("生成/刷新Lua脚本"))
                {
                    string historyPath = string.Empty;
                    if ((PatternType)m_PatternType.enumValueIndex == PatternType.None)
                    {
                        CreateOrRefreshLuaFile(ref historyPath, (int)NonePatternType.Default);
                    }
                    else if((PatternType)m_PatternType.enumValueIndex == PatternType.MVVM)
                    {
                        CreateOrRefreshLuaFile(ref historyPath, (int)MVVMPatternType.View);
                        CreateOrRefreshLuaFile(ref historyPath, (int)MVVMPatternType.ViewModel);
                    }
                    AssetDatabase.Refresh();
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.HelpBox("说明：Lua脚本不存在时生成脚本，存在时刷新脚本。", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("请先解决输入问题才能生成/刷新Lua脚本。", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileStart()
        {
            base.OnCompileStart();

        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

        }

        /// <summary>
        /// 处理注入信息的行操作
        /// </summary>
        private void DisposeInjectionLineOperation()
        {
            // 处理中间插入行为的队列变化
            if (m_InnerInjectionInsertPosIndex != -1)
            {
                m_Injections.InsertArrayElementAtIndex(m_InnerInjectionInsertPosIndex);
                SerializedProperty injection = m_Injections.GetArrayElementAtIndex(m_InnerInjectionInsertPosIndex);
                injection.FindPropertyRelative("Comment").stringValue = string.Empty;
                injection.FindPropertyRelative("InjectionTypeName").enumValueIndex = (int)LuaInjection.InjectionType.GameObject;
                injection.FindPropertyRelative("Name").stringValue = string.Empty;
                injection.FindPropertyRelative("IsArray").boolValue = false;

                injection.FindPropertyRelative("Obj").objectReferenceValue = null;
                injection.FindPropertyRelative("Variant").stringValue = string.Empty;
                injection.FindPropertyRelative("InfoEx").stringValue = string.Empty;
                injection.FindPropertyRelative("ExtendsEnabled").boolValue = false;
                injection.FindPropertyRelative("Extends").stringValue = string.Empty;

                injection.FindPropertyRelative("ElementsObjs").ClearArray();
                injection.FindPropertyRelative("ElementsVariants").ClearArray();
                injection.FindPropertyRelative("ElementsInfoExs").ClearArray();
                injection.FindPropertyRelative("ElementsExtendsEnableds").ClearArray();
                injection.FindPropertyRelative("ElementsExtends").ClearArray();
                OnEnable();
                m_InnerInjectionInsertPosIndex = -1;
            }
            else if (m_InnerInjectionDeletePosIndex != -1) // 处理中间删除行为的队列变化
            {
                m_Injections.DeleteArrayElementAtIndex(m_InnerInjectionDeletePosIndex);
                OnEnable();
                m_InnerInjectionDeletePosIndex = -1;
            }
            else if (m_InnerInjectionUpwardTargetPosIndex != -1) // 处理向上移动行为的队列变化
            {
                m_Injections.MoveArrayElement(m_InnerInjectionUpwardOriPosIndex, m_InnerInjectionUpwardTargetPosIndex);
                OnEnable();
                m_InnerInjectionUpwardOriPosIndex = -1;
                m_InnerInjectionUpwardTargetPosIndex = -1;
            }
            else if (m_InnerInjectionDownwardTargetPosIndex != -1) // 处理向下移动行为的队列变化
            {
                m_Injections.MoveArrayElement(m_InnerInjectionDownwardOriPosIndex, m_InnerInjectionDownwardTargetPosIndex);
                OnEnable();
                m_InnerInjectionDownwardOriPosIndex = -1;
                m_InnerInjectionDownwardTargetPosIndex = -1;
            }

            serializedObject.Update();
        }

        /// <summary>
        /// 注入信息列表表头信息GUI
        /// </summary>
        private void OnInjectionListHeaderInspectorGUI()
        {
            // 注入列表表头信息
            GUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("列表注入数量", m_Injections.arraySize.ToString());
                AddInjectionToListEndButtonInspectorGUI();
                DeleteInjectionFromListEndButtonInspectorGUI();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 注入信息格式说明GUI
        /// </summary>
        private void OnInjectionIntroductionsInspectorGUI()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("说明：类型/名称/对象/附加信息");
            stringBuilder.AppendLine("选择不同注入类型时填写对应CMD到附加信息框中即可生成事件的相关注册代码，多个CMD用','隔开。");
            stringBuilder.AppendLine(AorTxt.Format("[GameObject] -> C#组件名称（需要包含完整的名字空间）"));
            stringBuilder.AppendLine(AorTxt.Format("[LuaBehaviour] -> Lua脚本名称（MVVM设计模式下请填写View脚本名称）"));
            for (int index = 0; index < m_InfoExTypeNameTempletes.Count; index++)
            {
                stringBuilder.AppendLine(AorTxt.Format("[{0}] -> {1}", m_InfoExTypeNameTempletes[index].ToString(), m_InfoExCmdTempletes[index]));
            }
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("除以上CMD外，还可根据实际需求为UI对象设定EventTrigger事件的CMD到附加信息框中，具体CMD设定如下：");
            m_InfoExEventTriggerCmdTempletes.ForEach((cmd)=> {
                stringBuilder.Append(cmd+"\t");
            });
            EditorGUILayout.HelpBox(stringBuilder.ToString(), MessageType.Info);
        }

        /// <summary>
        /// 向注入列表末尾添加注入条目按钮
        /// </summary>
        private void AddInjectionToListEndButtonInspectorGUI()
        {
            if (GUILayout.Button("+"))
            {
                m_Injections.arraySize++;
                SerializedProperty injection = m_Injections.GetArrayElementAtIndex(m_Injections.arraySize - 1);

                injection.FindPropertyRelative("Comment").stringValue = string.Empty;
                injection.FindPropertyRelative("InjectionTypeName").enumValueIndex = (int)LuaInjection.InjectionType.GameObject;
                injection.FindPropertyRelative("Name").stringValue = string.Empty;
                injection.FindPropertyRelative("IsArray").boolValue = false;

                injection.FindPropertyRelative("Obj").objectReferenceValue = null;
                injection.FindPropertyRelative("Variant").stringValue = string.Empty;
                injection.FindPropertyRelative("InfoEx").stringValue = string.Empty;
                injection.FindPropertyRelative("ExtendsEnabled").boolValue = false;
                injection.FindPropertyRelative("Extends").stringValue = string.Empty;

                injection.FindPropertyRelative("ElementsObjs").ClearArray();
                injection.FindPropertyRelative("ElementsVariants").ClearArray();
                injection.FindPropertyRelative("ElementsInfoExs").ClearArray();
                injection.FindPropertyRelative("ElementsExtendsEnableds").ClearArray();
                injection.FindPropertyRelative("ElementsExtends").ClearArray();

                m_InterInjectionComments.Add(injection.FindPropertyRelative("Comment"));
                m_InterInjectionTypeNames.Add(injection.FindPropertyRelative("InjectionTypeName"));
                m_InterInjectionNames.Add(injection.FindPropertyRelative("Name"));
                m_InterInjectionIsArrays.Add(injection.FindPropertyRelative("IsArray"));

                m_InterInjectionObjs.Add(injection.FindPropertyRelative("Obj"));
                m_InterInjectionVariants.Add(injection.FindPropertyRelative("Variant"));
                m_InterInjectionInfoExs.Add(injection.FindPropertyRelative("InfoEx"));
                m_InterInjectionExtendsEnabled.Add(injection.FindPropertyRelative("ExtendsEnabled"));
                m_InterInjectionExtends.Add(injection.FindPropertyRelative("Extends"));

                m_InterInjectionElementsObjs.Add(injection.FindPropertyRelative("ElementsObjs"));
                m_InterInjectionElementsVariants.Add(injection.FindPropertyRelative("ElementsVariants"));
                m_InterInjectionElementsInfoExs.Add(injection.FindPropertyRelative("ElementsInfoExs"));
                m_InterInjectionElementsExtendsEnableds.Add(injection.FindPropertyRelative("ElementsExtendsEnableds"));
                m_InterInjectionElementsExtends.Add(injection.FindPropertyRelative("ElementsExtends"));

                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
        }

        /// <summary>
        /// 从注入列表末尾删除注入条目按钮
        /// </summary>
        private void DeleteInjectionFromListEndButtonInspectorGUI()
        {
            if (GUILayout.Button("-"))
            {
                if (m_Injections.arraySize > 0)
                {
                    m_InterInjectionComments.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionTypeNames.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionNames.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionIsArrays.RemoveAt(m_Injections.arraySize - 1);

                    m_InterInjectionObjs.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionVariants.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionInfoExs.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionExtendsEnabled.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionExtends.RemoveAt(m_Injections.arraySize - 1);

                    m_InterInjectionElementsObjs.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionElementsVariants.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionElementsInfoExs.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionElementsExtendsEnableds.RemoveAt(m_Injections.arraySize - 1);
                    m_InterInjectionElementsExtends.RemoveAt(m_Injections.arraySize - 1);

                    m_Injections.DeleteArrayElementAtIndex(m_Injections.arraySize - 1);
                    serializedObject.ApplyModifiedProperties();
                }
                GUIUtility.ExitGUI();
            }
        }

        /// <summary>
        /// 注入信息列表条目详情GUI
        /// </summary>
        private void OnInjectionItemListInspectorGUI()
        {
            GUILayout.BeginVertical("box");
            {
                if (m_Injections != null && m_Injections.arraySize > 0)
                {
                    for (int index = 0; index < m_Injections.arraySize; index++)
                    {
                        GUILayout.BeginVertical(m_InjectionItemStyle);
                        {
                            GUILayout.BeginHorizontal("box");
                            {
                                GUI.color = Color.cyan;
                                GUILayout.Label($"({index + 1})", new GUILayoutOption[] {GUILayout.Width(30)});
                                GUI.color = Color.white;

                                if (string.IsNullOrEmpty(m_InterInjectionComments[index].stringValue)) GUI.color = Color.red;
                                m_InterInjectionComments[index].stringValue = GUILayout.TextField(m_InterInjectionComments[index].stringValue, new GUILayoutOption[] { GUILayout.Width(300) });
                                GUI.color = Color.white;

                                if (GUILayout.Button("+"))  // 增
                                {
                                    m_InnerInjectionInsertPosIndex = index;
                                    GUIUtility.ExitGUI();
                                }
                                if (GUILayout.Button("-"))  // 减
                                {
                                    m_InnerInjectionDeletePosIndex = index;
                                    GUIUtility.ExitGUI();
                                }
                                if (GUILayout.Button('\u25B2'.ToString())) // 上
                                {
                                    m_InnerInjectionUpwardOriPosIndex = index;
                                    m_InnerInjectionUpwardTargetPosIndex = index - 1 < 0 ? 0 : (index - 1);
                                    GUIUtility.ExitGUI();
                                }
                                if (GUILayout.Button('\u25BC'.ToString())) // 下
                                {
                                    m_InnerInjectionDownwardOriPosIndex = index;
                                    m_InnerInjectionDownwardTargetPosIndex = index + 1 >= m_Injections.arraySize ? (m_Injections.arraySize - 1) : (index + 1);
                                    GUIUtility.ExitGUI();
                                }
                            }
                            GUILayout.EndHorizontal();

                            // 数组形式
                            if (m_InterInjectionIsArrays[index].boolValue)
                            {
                                GUILayout.BeginHorizontal("box");
                                {
                                    ShowInjectionItemListCommonInfos(index);
                                }
                                GUILayout.EndHorizontal();

                                bool lastState = m_OpenedItems.Contains(m_InterInjectionNames[index].stringValue);
                                bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("元素列表清单({0})", m_InterInjectionElementsObjs[index].arraySize));
                                if (currentState != lastState)
                                {
                                    if (currentState)
                                    {
                                        m_OpenedItems.Add(m_InterInjectionNames[index].stringValue);
                                    }
                                    else
                                    {
                                        m_OpenedItems.Remove(m_InterInjectionNames[index].stringValue);
                                    }
                                }
                                if (currentState)
                                {
                                    for (int elementIndex = 0; elementIndex < m_InterInjectionElementsObjs[index].arraySize; elementIndex++)
                                    {
                                        GUILayout.BeginHorizontal("box");
                                        {
                                            EditorGUILayout.LabelField($"[{(elementIndex + 1).ToString()}]", new GUILayoutOption[] { GUILayout.Width(30) });
                                            EditorGUI.BeginDisabledGroup(true);
                                            {
                                                EditorGUILayout.Popup(LuaInjection.InjectionTypeRealToDisplayMapping[m_InterInjectionTypeNames[index].enumValueIndex], LuaInjection.DisplayInjectionTypeString, new GUILayoutOption[] { GUILayout.Width(180) });
                                                EditorGUILayout.TextField($"{m_InterInjectionNames[index].stringValue}[{elementIndex + 1}]", new GUILayoutOption[] { GUILayout.Width(120) });
                                            }
                                            EditorGUI.EndDisabledGroup();

                                            UnityEngine.Object historyObject = m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue;
                                            if (string.IsNullOrEmpty(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue) && m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue == null) GUI.color = Color.red;
                                            switch (m_InterInjectionTypeNames[index].enumValueIndex)
                                            {
                                                case (int)LuaInjection.InjectionType.GameObject: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.GameObject), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.Transform: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.Transform), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.RectTransform: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.RectTransform), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.LuaBehaviour: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(LuaBehaviour), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.SpriteRenderer: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.SpriteRenderer), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.Tilemap: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.Tilemaps.Tilemap), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Text: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Text), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Text_TextMeshPro: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(TMPro.TextMeshProUGUI), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Dropdown_TextMeshPro: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(TMPro.TMP_Dropdown), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_InputField_TextMeshPro: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(TMPro.TMP_InputField), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_TextPicMixed: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(AorTextPicMixed), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Image: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Image), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Button: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Button), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Scrollbar: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Scrollbar), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_ScrollRect: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.ScrollRect), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Toggle: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Toggle), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Slider: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Slider), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Dropdown: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.Dropdown), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_InputField: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.UI.InputField), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_Tree: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(AorTree), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_ListView: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(AorListView), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.UI_SwitchButton: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(AorSwitchButton), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.Canvas: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.Canvas), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.Camera: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.Camera), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.ParticleSystem: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.ParticleSystem), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.Light: m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue, typeof(UnityEngine.Light), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                                case (int)LuaInjection.InjectionType.Int32: m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue = EditorGUILayout.IntField(string.IsNullOrEmpty(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue) ? 0 : int.Parse(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue), new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                                case (int)LuaInjection.InjectionType.Float: m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue = EditorGUILayout.FloatField(string.IsNullOrEmpty(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue) ? 0 : float.Parse(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue), new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                                case (int)LuaInjection.InjectionType.String: m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue = EditorGUILayout.TextField(string.IsNullOrEmpty(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue) ? string.Empty : m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue, new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                                case (int)LuaInjection.InjectionType.Boolean: m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue = EditorGUILayout.Toggle(string.IsNullOrEmpty(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue) ? false : bool.Parse(m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(elementIndex).stringValue), new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                            }
                                            GUI.color = Color.white;
                                            m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue = EditorGUILayout.TextField(m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue);

                                            // 更换了注入对象时，变量名称需要自动填充，可能存在的附加信息需要自动填充
                                            if (m_InterInjectionTypeNames[index].enumValueIndex < (int)LuaInjection.InjectionType.Int32 || m_InterInjectionTypeNames[index].enumValueIndex > (int)LuaInjection.InjectionType.Boolean)
                                            {
                                                if (m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue != historyObject)
                                                {
                                                    if (m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue != null)
                                                    {
                                                        m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue = string.Empty;
                                                        for (int templeteIndex = 0; templeteIndex < m_InfoExTypeNameTempletes.Count; templeteIndex++)
                                                        {
                                                            if ((int)m_InfoExTypeNameTempletes[templeteIndex] == m_InterInjectionTypeNames[index].enumValueIndex)
                                                            {
                                                                if (string.IsNullOrEmpty(m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue))
                                                                {
                                                                    m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue = m_InfoExCmdTempletes[templeteIndex];
                                                                }
                                                                else
                                                                {
                                                                    m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue += ("," + m_InfoExCmdTempletes[templeteIndex]);
                                                                }
                                                            }
                                                        }
                                                        m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndex).boolValue = false;
                                                        m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndex).stringValue = string.Empty;
                                                    }
                                                }
                                            }

                                            GUILayout.FlexibleSpace();
                                            // 目前只有button显示扩展按钮 ExtendsEnabled Extends
                                            if (NeedShowExtendButton((LuaInjection.InjectionType)m_InterInjectionTypeNames[index].enumValueIndex))
                                            {
                                                if (m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndex).boolValue) GUI.color = Color.cyan;
                                                m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndex).boolValue = EditorGUILayout.ToggleLeft("扩展", m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndex).boolValue, new GUILayoutOption[] { GUILayout.Width(50) });
                                                GUI.color = Color.white;
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        if (NeedShowExtendDetailInfo(index, elementIndex))
                                        {
                                            GUILayout.BeginHorizontal("box");
                                            {
                                                ShowExtendDetailInfos(index, elementIndex);
                                            }
                                            GUILayout.EndHorizontal();
                                        }
                                    }
                                }
                            }
                            else // 非数组形式
                            {
                                GUILayout.BeginHorizontal("box");
                                {
                                    ShowInjectionItemListCommonInfos(index);

                                    UnityEngine.Object historyObject = m_InterInjectionObjs[index].objectReferenceValue;

                                    if (string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) && m_InterInjectionObjs[index].objectReferenceValue == null) GUI.color = Color.red;
                                    switch (m_InterInjectionTypeNames[index].enumValueIndex)
                                    {
                                        case (int)LuaInjection.InjectionType.GameObject: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.GameObject), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.Transform: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.Transform), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.RectTransform: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.RectTransform), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.LuaBehaviour: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(LuaBehaviour), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.SpriteRenderer: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.SpriteRenderer), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.Tilemap: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.Tilemaps.Tilemap), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Text: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Text), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Text_TextMeshPro: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(TMPro.TextMeshProUGUI), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Dropdown_TextMeshPro: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(TMPro.TMP_Dropdown), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_InputField_TextMeshPro: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(TMPro.TMP_InputField), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_TextPicMixed: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(AorTextPicMixed), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Image: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Image), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Button: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Button), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Scrollbar: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Scrollbar), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_ScrollRect: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.ScrollRect), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Toggle: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Toggle), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Slider: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Slider), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Dropdown: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.Dropdown), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_InputField: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.UI.InputField), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_Tree: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(AorTree), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_ListView: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(AorListView), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.UI_SwitchButton: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(AorSwitchButton), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.Canvas: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.Canvas), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.Camera: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.Camera), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.ParticleSystem: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.ParticleSystem), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.Light: m_InterInjectionObjs[index].objectReferenceValue = EditorGUILayout.ObjectField(m_InterInjectionObjs[index].objectReferenceValue, typeof(UnityEngine.Light), true, new GUILayoutOption[] { GUILayout.Width(120) }); break;
                                        case (int)LuaInjection.InjectionType.Int32: m_InterInjectionVariants[index].stringValue = EditorGUILayout.IntField(string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) ? 0 : int.Parse(m_InterInjectionVariants[index].stringValue), new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                        case (int)LuaInjection.InjectionType.Float: m_InterInjectionVariants[index].stringValue = EditorGUILayout.FloatField(string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) ? 0 : float.Parse(m_InterInjectionVariants[index].stringValue), new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                        case (int)LuaInjection.InjectionType.String: m_InterInjectionVariants[index].stringValue = EditorGUILayout.TextField(string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) ? string.Empty : m_InterInjectionVariants[index].stringValue, new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                        case (int)LuaInjection.InjectionType.Boolean: m_InterInjectionVariants[index].stringValue = EditorGUILayout.Toggle(string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) ? false : bool.Parse(m_InterInjectionVariants[index].stringValue), new GUILayoutOption[] { GUILayout.Width(120) }).ToString(); break;
                                    }
                                    GUI.color = Color.white;
                                    m_InterInjectionInfoExs[index].stringValue = EditorGUILayout.TextField(m_InterInjectionInfoExs[index].stringValue);

                                    // 更换了注入对象时，变量名称需要自动填充，可能存在的附加信息需要自动填充
                                    if (m_InterInjectionTypeNames[index].enumValueIndex < (int)LuaInjection.InjectionType.Int32 || m_InterInjectionTypeNames[index].enumValueIndex > (int)LuaInjection.InjectionType.Boolean)
                                    {
                                        if (m_InterInjectionObjs[index].objectReferenceValue != historyObject)
                                        {
                                            if (m_InterInjectionObjs[index].objectReferenceValue != null)
                                            {
                                                m_InterInjectionNames[index].stringValue = m_InterInjectionObjs[index].objectReferenceValue.name.Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
                                                m_InterInjectionInfoExs[index].stringValue = string.Empty;
                                                for (int templeteIndex = 0; templeteIndex < m_InfoExTypeNameTempletes.Count; templeteIndex++)
                                                {
                                                    if ((int)m_InfoExTypeNameTempletes[templeteIndex] == m_InterInjectionTypeNames[index].enumValueIndex)
                                                    {
                                                        if (string.IsNullOrEmpty(m_InterInjectionInfoExs[index].stringValue))
                                                        {
                                                            m_InterInjectionInfoExs[index].stringValue = m_InfoExCmdTempletes[templeteIndex];
                                                        }
                                                        else
                                                        {
                                                            m_InterInjectionInfoExs[index].stringValue += ("," + m_InfoExCmdTempletes[templeteIndex]);
                                                        }
                                                    }
                                                }
                                                m_InterInjectionExtendsEnabled[index].boolValue = false;
                                                m_InterInjectionExtends[index].stringValue = string.Empty;
                                            }
                                        }
                                    }
                                    GUILayout.FlexibleSpace();
                                    // 目前只有button显示扩展按钮 ExtendsEnabled Extends
                                    if (NeedShowExtendButton((LuaInjection.InjectionType)m_InterInjectionTypeNames[index].enumValueIndex))
                                    {
                                        if (m_InterInjectionExtendsEnabled[index].boolValue) GUI.color = Color.cyan;
                                        m_InterInjectionExtendsEnabled[index].boolValue = EditorGUILayout.ToggleLeft("扩展", m_InterInjectionExtendsEnabled[index].boolValue, new GUILayoutOption[] { GUILayout.Width(50) });
                                        GUI.color = Color.white;
                                    }
                                }
                                GUILayout.EndHorizontal();
                                if(NeedShowExtendDetailInfo(index))
                                {
                                    GUILayout.BeginHorizontal("box");
                                    {
                                        ShowExtendDetailInfos(index);
                                    }
                                    GUILayout.EndHorizontal();
                                }
                            }
                        }
                        GUILayout.EndVertical();
                        GUILayout.Space(10);
                    }
                }
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 生成/刷新Lua文件
        /// </summary>
        private void CreateOrRefreshLuaFile(ref string historyPath, int typeEnumIndex)
        {
            int index = typeEnumIndex;
            string luaScriptName = string.Empty;
            switch((PatternType)m_PatternType.enumValueIndex)
            {
                case PatternType.None: luaScriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex(index).stringValue; break;
                case PatternType.MVVM: luaScriptName = m_LuaScriptNamesMVVM.GetArrayElementAtIndex(index).stringValue; break;
            }
            string luaRootPath = AorTxt.Format("{0}{1}", Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true));
            string[] fileFullPaths = System.IO.Directory.GetFiles(luaRootPath, AorTxt.Format("{0}.lua.txt", luaScriptName), System.IO.SearchOption.AllDirectories);
            // luaRootPath目录及其子目录中没有该名称的lua文件时需要调起保存窗口
            if (fileFullPaths.Length == 0) // 生成文件
            {
                historyPath = string.IsNullOrEmpty(historyPath) ? luaRootPath : historyPath;
                string exportFileName = EditorUtility.SaveFilePanel(AorTxt.Format("生成{0}.lua.txt文件", luaScriptName), historyPath, AorTxt.Format("{0}.lua.txt", luaScriptName), string.Empty);
                if (!string.IsNullOrEmpty(exportFileName))
                {
                    historyPath = exportFileName.Substring(0, exportFileName.LastIndexOf("/"));
                    try
                    {
                        StringBuilder stringBuilderCommentLines = null;
                        StringBuilder stringBuilderEmptyCodeLines = null;

                        if ((PatternType)m_PatternType.enumValueIndex == PatternType.None)
                        {
                            stringBuilderCommentLines = GeneratePatternNoneCommentLines();
                            stringBuilderEmptyCodeLines = GeneratePatternNoneEmptyCodeLines();
                        }
                        else if ((PatternType)m_PatternType.enumValueIndex == PatternType.MVVM)
                        {
                            stringBuilderCommentLines = GeneratePatternMVVMCommentLines(typeEnumIndex);
                            stringBuilderEmptyCodeLines = GeneratePatternMVVMEmptyCodeLines(typeEnumIndex);
                        }

                        string content = AorTxt.Format("{0}{1}", stringBuilderCommentLines.ToString(), stringBuilderEmptyCodeLines.ToString());
                        System.IO.File.WriteAllText(exportFileName, content, new System.Text.UTF8Encoding(false));

                        Log.Debug(AorTxt.Format("生成{0}文件成功。", exportFileName));
                    }
                    catch (Exception exception)
                    {
                        Log.Error(AorTxt.Format("生成{0}文件失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                    }
                }

            }
            // luaRootPath目录及其子目录中存在该名称的lua文件时需要刷新文件内容
            else if (fileFullPaths.Length == 1)  // 刷新文件
            {
                try
                {
                    StringBuilder stringBuilderCommentLines = null;
                    StringBuilder stringBuilderCodeLines = null;

                    if (m_PatternType.enumValueIndex == (int)PatternType.None)
                    {
                        stringBuilderCommentLines = GeneratePatternNoneCommentLines();
                        stringBuilderCodeLines = GeneratePatternNoneCodeLines(fileFullPaths[0]);
                    }
                    else if ((PatternType)m_PatternType.enumValueIndex == PatternType.MVVM)
                    {
                        stringBuilderCommentLines = GeneratePatternMVVMCommentLines(typeEnumIndex);
                        stringBuilderCodeLines = GeneratePatternMVVMCodeLines(fileFullPaths[0], typeEnumIndex);
                    }

                    string content = AorTxt.Format("{0}{1}", stringBuilderCommentLines.ToString(), stringBuilderCodeLines.ToString());
                    System.IO.File.WriteAllText(fileFullPaths[0], content, new System.Text.UTF8Encoding(false));

                    Log.Debug(AorTxt.Format("刷新{0}文件成功。", fileFullPaths[0]));
                }
                catch (Exception exception)
                {
                    Log.Error(AorTxt.Format("刷新{0}文件失败, 异常信息： '{1}'.", fileFullPaths[0], exception.ToString()));
                }
            }
            else
            {
                Log.Error(AorTxt.Format("目录 {0} 及其子目录中遍历到多个文件 {1}.txt ！", luaRootPath, luaScriptName));
            }

        }

        /// <summary>
        /// 给注入对象添加lua代码模板参数
        /// </summary>
        /// <param name="injectionType">注入类型</param>
        /// <param name="CMD">指令</param>
        /// <param name="luaFunctionName">lua层函数名称</param>
        private void RegistInfoExCmdTemplete(LuaInjection.InjectionType injectionType, string CMD, string luaFunctionName, string luaFunctionParam)
        {
            m_InfoExTypeNameTempletes.Add(injectionType);
            m_InfoExCmdTempletes.Add(CMD);
            m_InfoExFunctionNameTempletes.Add(luaFunctionName);
            m_InfoExFunctionParamTempletes.Add(luaFunctionParam);
        }

        /// <summary>
        /// 显示注入条目列表中的公共信息部分
        /// 类型、是否为数组、数组长度、名称等
        /// </summary>
        /// <param name="index"></param>
        private void ShowInjectionItemListCommonInfos(int index)
        {
            int newInjectionTypeName = EditorGUILayout.Popup(LuaInjection.InjectionTypeRealToDisplayMapping[m_InterInjectionTypeNames[index].enumValueIndex], LuaInjection.DisplayInjectionTypeString, new GUILayoutOption[] { GUILayout.Width(180) });
            if (newInjectionTypeName != LuaInjection.InjectionTypeRealToDisplayMapping[m_InterInjectionTypeNames[index].enumValueIndex])
            {
                m_InterInjectionObjs[index].objectReferenceValue = null;
                m_InterInjectionVariants[index].stringValue = string.Empty;
            }
            m_InterInjectionTypeNames[index].enumValueIndex = LuaInjection.InjectionTypeDisplayToRealMapping[newInjectionTypeName];

            EditorGUILayout.LabelField("数组", new GUILayoutOption[] { GUILayout.Width(25) });
            m_InterInjectionIsArrays[index].boolValue = EditorGUILayout.Toggle(m_InterInjectionIsArrays[index].boolValue, new GUILayoutOption[] { GUILayout.Width(15) });
            if (m_InterInjectionIsArrays[index].boolValue)
            {
                EditorGUILayout.LabelField("长度", new GUILayoutOption[] { GUILayout.Width(25) });
                int lastArraySize = m_InterInjectionElementsObjs[index].arraySize;
                int curArraySize = EditorGUILayout.DelayedIntField(lastArraySize, new GUILayoutOption[] { GUILayout.Width(30) });
                
                m_InterInjectionElementsObjs[index].arraySize = curArraySize;
                m_InterInjectionElementsVariants[index].arraySize = curArraySize;
                m_InterInjectionElementsInfoExs[index].arraySize = curArraySize;
                m_InterInjectionElementsExtendsEnableds[index].arraySize = curArraySize;
                m_InterInjectionElementsExtends[index].arraySize = curArraySize;

                for (int idx = lastArraySize; idx < curArraySize; idx++)
                {
                    m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(idx).objectReferenceValue = null;
                    m_InterInjectionElementsVariants[index].GetArrayElementAtIndex(idx).stringValue = string.Empty;
                    m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(idx).stringValue = string.Empty;
                    m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(idx).boolValue = false;
                    m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(idx).stringValue = string.Empty;
                }

                m_InterInjectionObjs[index].objectReferenceValue = null;
                m_InterInjectionVariants[index].stringValue = string.Empty;
                m_InterInjectionInfoExs[index].stringValue = string.Empty;
                m_InterInjectionExtendsEnabled[index].boolValue = false;
                m_InterInjectionExtends[index].stringValue = string.Empty;
            }
            else
            {
                m_InterInjectionElementsObjs[index].ClearArray();
                m_InterInjectionElementsVariants[index].ClearArray();
                m_InterInjectionElementsInfoExs[index].ClearArray();
                m_InterInjectionElementsExtendsEnableds[index].ClearArray();
                m_InterInjectionElementsExtends[index].ClearArray();
            }

            if (string.IsNullOrEmpty(m_InterInjectionNames[index].stringValue)) GUI.color = Color.red;
            m_InterInjectionNames[index].stringValue = EditorGUILayout.TextField(m_InterInjectionNames[index].stringValue, new GUILayoutOption[] { GUILayout.Width(120) });
            GUI.color = Color.white;
        }

        /// <summary>
        /// 是否需要展示扩展按钮
        /// </summary>
        /// <param name="injectionType">注入类型</param>
        /// <returns></returns>
        private bool NeedShowExtendButton(LuaInjection.InjectionType injectionType)
        {
            switch(injectionType)
            {
                case LuaInjection.InjectionType.UI_Button: return true;
            }
            return false;
        }

        /// <summary>
        /// 是否需要展示扩展详情
        /// </summary>
        /// <param name="index">宿主条目索引值</param>
        /// <param name="elementIndex">数组元素索引值</param>
        /// <returns></returns>
        private bool NeedShowExtendDetailInfo(int index, int elementIndex = -1)
        {
            switch ((LuaInjection.InjectionType)m_InterInjectionTypeNames[index].enumValueIndex)
            {
                case LuaInjection.InjectionType.UI_Button:
                    {
                        if (m_InterInjectionIsArrays[index].boolValue)
                        {
                            if (elementIndex >= 0)
                            {
                                return m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndex).boolValue;
                            }
                        }
                        else
                        {
                            return m_InterInjectionExtendsEnabled[index].boolValue;
                        }
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// 展示Extends内容
        /// </summary>
        /// <param name="index">宿主条目索引值</param>
        /// <param name="elementIndex">数组元素索引值</param>
        private void ShowExtendDetailInfos(int index, int elementIndex = -1)
        {
            switch ((LuaInjection.InjectionType)m_InterInjectionTypeNames[index].enumValueIndex)
            {
                case LuaInjection.InjectionType.UI_Button:
                {
                    if(m_InterInjectionIsArrays[index].boolValue)
                    {
                        if(elementIndex >= 0)
                        {
                            if (m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndex).boolValue)
                            {
                                if (string.IsNullOrEmpty(m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndex).stringValue))
                                {
                                    m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndex).stringValue = "##";
                                }
                                GUI.color = Color.cyan;
                                List<string> infos = new List<string>(m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndex).stringValue.Split('#'));
                                EditorGUILayout.LabelField("注释", new GUILayoutOption[] { GUILayout.Width(30) });
                                infos[0] = EditorGUILayout.TextField(infos[0]);
                                EditorGUILayout.LabelField("公共点击回调方法名称", new GUILayoutOption[] { GUILayout.Width(130) });
                                infos[1] = EditorGUILayout.TextField(infos[1]);
                                EditorGUILayout.LabelField("参数值（string）", new GUILayoutOption[] { GUILayout.Width(100) });
                                infos[2] = EditorGUILayout.TextField(infos[2]);
                                m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndex).stringValue = $"{infos[0]}#{infos[1]}#{infos[2]}";
                                GUI.color = Color.white;
                            }
                            else
                            {
                                m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndex).stringValue = "##";
                            }
                        }
                    }
                    else
                    {
                        if (m_InterInjectionExtendsEnabled[index].boolValue)
                        {
                            if (string.IsNullOrEmpty(m_InterInjectionExtends[index].stringValue))
                            {
                                m_InterInjectionExtends[index].stringValue = "##";
                            }
                            GUI.color = Color.cyan;
                            List<string> infos = new List<string>(m_InterInjectionExtends[index].stringValue.Split('#'));
                            EditorGUILayout.LabelField("注释", new GUILayoutOption[] { GUILayout.Width(30) });
                            infos[0] = EditorGUILayout.TextField(infos[0]);
                            EditorGUILayout.LabelField("公共点击回调方法名称", new GUILayoutOption[] { GUILayout.Width(130) });
                            infos[1] = EditorGUILayout.TextField(infos[1]);
                            EditorGUILayout.LabelField("参数值（string）", new GUILayoutOption[] { GUILayout.Width(100) });
                            infos[2] = EditorGUILayout.TextField(infos[2]);
                            m_InterInjectionExtends[index].stringValue = $"{infos[0]}#{infos[1]}#{infos[2]}";
                            GUI.color = Color.white;
                        }
                        else
                        {
                            m_InterInjectionExtends[index].stringValue = "##";
                        }
                    }
                }
                break;
            }
        }

        /// <summary>
        /// 采集InfoEx辅助信息
        /// </summary>
        /// <param name="luaInjectNames"></param>
        /// <param name="luaInjectComments"></param>
        /// <param name="luaInjectFunctionNames"></param>
        /// <param name="luaInjectFunctionParams"></param>
        /// <param name="luaInjectCmds"></param>
        private void CollectInfoExInfos(out List<string> luaInjectNames, out List<string> luaInjectComments, out List<string> luaInjectFunctionNames, out List<string> luaInjectFunctionParams, out List<string> luaInjectCmds)
        {
            luaInjectNames = new List<string>();
            luaInjectComments = new List<string>();
            luaInjectFunctionNames = new List<string>();
            luaInjectFunctionParams = new List<string>();
            luaInjectCmds = new List<string>();
            for (int index = 0; index < m_Injections.arraySize; index++)
            {
                if (m_InterInjectionTypeNames[index].enumValueIndex != (int)LuaInjection.InjectionType.LuaBehaviour)
                {
                    List<List<string>> cmds = new List<List<string>>();
                    List<List<string>> elementIndexesInLua = new List<List<string>>();
                    var innerCmds = new List<string>();
                    var innerElementIndexesInLua = new List<string>();
                    if (m_InterInjectionTypeNames[index].enumValueIndex == (int)LuaInjection.InjectionType.GameObject)
                    {
                        if (m_InterInjectionIsArrays[index].boolValue)
                        {
                            for (int elementIndex = 0; elementIndex < m_InterInjectionElementsInfoExs[index].arraySize; elementIndex++)
                            {
                                innerCmds.Add(m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue);
                                innerElementIndexesInLua.Add((elementIndex + 1).ToString());
                            }
                        }
                        else
                        {
                            innerCmds.Add(m_InterInjectionInfoExs[index].stringValue);
                            innerElementIndexesInLua.Add(string.Empty);
                        }
                    }
                    else
                    {
                        if (m_InterInjectionIsArrays[index].boolValue)
                        {
                            for (int elementIndex = 0; elementIndex < m_InterInjectionElementsInfoExs[index].arraySize; elementIndex++)
                            {
                                string[] contents = m_InterInjectionElementsInfoExs[index].GetArrayElementAtIndex(elementIndex).stringValue.Replace(" ", string.Empty).Split(',');
                                foreach (var content in contents)
                                {
                                    innerCmds.Add(content);
                                    innerElementIndexesInLua.Add((elementIndex + 1).ToString());
                                }
                            }
                        }
                        else
                        {
                            string[] contents = m_InterInjectionInfoExs[index].stringValue.Replace(" ", string.Empty).Split(',');
                            foreach (var content in contents)
                            {
                                innerCmds.Add(content);
                                innerElementIndexesInLua.Add(string.Empty);
                            }
                        }
                    }
                    cmds.Add(innerCmds);
                    elementIndexesInLua.Add(innerElementIndexesInLua);

                    if (cmds != null)
                    {
                        for (int cmdIndex = 0; cmdIndex < cmds.Count; cmdIndex++)
                        {
                            for (int checkIndex = 0; checkIndex < cmds[cmdIndex].Count; checkIndex++)
                            {
                                string elementIndexInLua = elementIndexesInLua[cmdIndex][checkIndex];
                                int elementIndexInCS = m_InterInjectionIsArrays[index].boolValue ? (int.Parse(elementIndexInLua) - 1) : -1;
                                if (m_InfoExEventTriggerCmdTempletes.Contains(cmds[cmdIndex][checkIndex]))
                                {
                                    string functionName = AorTxt.Format("On{0}{1}{2}", m_InterInjectionNames[index].stringValue[0].ToString().ToUpper() + m_InterInjectionNames[index].stringValue.Substring(1, m_InterInjectionNames[index].stringValue.Length - 1), elementIndexInLua, cmds[cmdIndex][checkIndex].Replace("Evt_", "Evt"));
                                    luaInjectComments.Add($"{m_InterInjectionComments[index].stringValue}-{cmds[cmdIndex][checkIndex]}-交互回调");
                                    luaInjectNames.Add(m_InterInjectionIsArrays[index].boolValue ? $"{m_InterInjectionNames[index].stringValue}[{elementIndexInLua}]" : $"{m_InterInjectionNames[index].stringValue}");
                                    luaInjectFunctionNames.Add(functionName);
                                    luaInjectFunctionParams.Add("eventData");
                                    luaInjectCmds.Add(cmds[cmdIndex][checkIndex]);
                                }
                                else if (m_InfoExCmdTempletes.Contains(cmds[cmdIndex][checkIndex]))
                                {
                                    int infoExCmdTempleteIndex = m_InfoExCmdTempletes.IndexOf(cmds[cmdIndex][checkIndex]);
                                    string functionName = string.Empty;
                                    string comment = $"{m_InterInjectionComments[index].stringValue}-交互回调";
                                    if (m_InterInjectionIsArrays[index].boolValue)
                                    {
                                        functionName = AorTxt.Format(m_InfoExFunctionNameTempletes[infoExCmdTempleteIndex], m_InterInjectionNames[index].stringValue[0].ToString().ToUpper() + m_InterInjectionNames[index].stringValue.Substring(1, m_InterInjectionNames[index].stringValue.Length - 1) + elementIndexInLua);
                                        if (m_InterInjectionElementsExtendsEnableds[index].GetArrayElementAtIndex(elementIndexInCS).boolValue)
                                        {
                                            string[] infos = m_InterInjectionElementsExtends[index].GetArrayElementAtIndex(elementIndexInCS).stringValue.Split('#');
                                            if (!string.IsNullOrEmpty(infos[0]))
                                            {
                                                comment = $"{infos[0]}-交互回调";
                                            }
                                            if (!string.IsNullOrEmpty(infos[1]))
                                            {
                                                functionName = AorTxt.Format(m_InfoExFunctionNameTempletes[infoExCmdTempleteIndex], infos[1]);
                                            }
                                        }
                                        else
                                        {
                                            functionName = AorTxt.Format(m_InfoExFunctionNameTempletes[infoExCmdTempleteIndex], m_InterInjectionNames[index].stringValue[0].ToString().ToUpper() + m_InterInjectionNames[index].stringValue.Substring(1, m_InterInjectionNames[index].stringValue.Length - 1) + elementIndexInLua);
                                        }
                                    }
                                    else
                                    {
                                        functionName = AorTxt.Format(m_InfoExFunctionNameTempletes[infoExCmdTempleteIndex], m_InterInjectionNames[index].stringValue[0].ToString().ToUpper() + m_InterInjectionNames[index].stringValue.Substring(1, m_InterInjectionNames[index].stringValue.Length - 1));
                                        if (m_InterInjectionExtendsEnabled[index].boolValue)
                                        {
                                            string[] infos = m_InterInjectionExtends[index].stringValue.Split('#');
                                            if (!string.IsNullOrEmpty(infos[0]))
                                            {
                                                comment = $"{infos[0]}-交互回调";
                                            }
                                            if (!string.IsNullOrEmpty(infos[1]))
                                            {
                                                functionName = AorTxt.Format(m_InfoExFunctionNameTempletes[infoExCmdTempleteIndex], infos[1]);
                                            }
                                        }
                                    }
                                    luaInjectComments.Add(comment);
                                    luaInjectNames.Add(m_InterInjectionIsArrays[index].boolValue ? $"{m_InterInjectionNames[index].stringValue}[{elementIndexInLua}]" : $"{m_InterInjectionNames[index].stringValue}");
                                    luaInjectFunctionNames.Add(functionName);
                                    luaInjectFunctionParams.Add(m_InfoExFunctionParamTempletes[infoExCmdTempleteIndex]);
                                    luaInjectCmds.Add(cmds[cmdIndex][checkIndex]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
