/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SoundExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音表导出工具窗口（Honor 菜单入口）
 ***************************************************************/

using System.IO;
using Honor.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>声音表导出工具窗口。Honor → 声音表工具。</summary>
    public class SoundExportEditorWindow : BaseToolkitEditorWindow<SoundExportEditorWindow>
    {
        //=========================================================================
        #region 私有字段
        //=========================================================================

        /// <summary>
        /// 音频配置资产的序列化对象（绑定 SoundConfig.asset，可序列化、可编辑）
        /// </summary>
        private SerializedObject m_SoundSO;

        /// <summary>
        /// 音频配置资产引用（编辑器可序列化载体）
        /// </summary>
        private SoundConfig m_SoundConfig;
        #endregion

        //=========================================================================
        #region 窗口配置（抽象实现）
        //=========================================================================

        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/SoundComponentInspector/";
        protected override string UxmlName => "SoundExportEditorWindowTool";
        protected override GUIContent Title => new GUIContent("声音表工具");
        protected override Vector2 MinSize => new Vector2(600, 380);
        protected override Vector2 MaxSize => new Vector2(600, 380);

        [MenuItem("Honor/声音表工具", false, 363)]
        private static void OpenWindow() => Open();
        #endregion

        //=========================================================================
        #region 初始化
        //=========================================================================

        /// <summary>
        /// 加载（或创建）音频配置资产并绑定序列化对象
        /// <para>音频配置不再依赖纯 C# 组件（SoundComponent）——组件不走 Unity 序列化，
        /// 无法被 SerializedObject 绑定。此处改挂到 ScriptableObject 资产，实现可编辑持久化。</para>
        /// </summary>
        protected override void OnInitData()
        {
            m_SoundConfig = SoundConfigEditorUtils.GetOrCreateSoundConfig();
            if (m_SoundConfig != null)
            {
                m_SoundSO = new SerializedObject(m_SoundConfig);
            }
        }

        protected override void OnBindUI()
        {
            BindClick("openExcelBtn", () =>
                TableExportEditorUtility.OpenExcel(GamePathUtils.Sound.GetExcelFileFullPath()));

            BindClick("exportBtn", () =>
            {
                string f = GamePathUtils.Sound.GetExcelFileFullPath();
                string name = Path.GetFileNameWithoutExtension(f);
                string excelPath = $"{GamePathUtils.Sound.GetExcelRootDirectoryFullPath()}/{name}.xlsm";
                string luaPath = Path.Combine(GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath(), "AlTables", $"Table{name}.lua.txt");
                TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
            });

            BindClick("openDirBtn", () =>
                TableExportEditorUtility.OpenDirectory(GamePathUtils.Sound.GetExcelRootDirectoryFullPath()));

            // 音频配置：绑定 SoundConfig 资产的 AudioMixer 字段
            //（UXML 已通过 object-type 声明为 AudioMixer，此处仅做取值与回写）
            var mixerField = Q<ObjectField>("audioMixerField");
            if (mixerField != null)
            {
                if (m_SoundSO != null)
                {
                    SerializedProperty mixerProp = m_SoundSO.FindProperty("m_AudioMixer");
                    mixerField.value = mixerProp?.objectReferenceValue;
                    mixerField.RegisterValueChangedCallback(evt =>
                    {
                        if (m_SoundSO == null) return;
                        SerializedProperty prop = m_SoundSO.FindProperty("m_AudioMixer");
                        if (prop != null)
                        {
                            prop.objectReferenceValue = evt.newValue;
                            // 仅提交到内存：落盘由明确的保存动作（Ctrl+S / 关窗）触发
                            m_SoundSO.ApplyModifiedProperties();
                        }
                    });
                }
            }

            // 声音分组：IMGUI 容器绘制可编辑的声音组列表
            BindImgui("groupShellsContainer", DrawGroupShells);

            // 注册 Ctrl+S 快捷键：手动保存音频配置到磁盘资产
            RegisterSaveShortcut();

            RequestRepaint();
        }
        #endregion

        //=========================================================================
        #region 快捷键保存
        //=========================================================================

        /// <summary>
        /// 注册 Ctrl+S / Cmd+S 快捷键：窗口内按快捷键立即将音频配置落盘
        /// <para>让用户可手动、确定地保存当前配置到 SoundConfig.asset，
        /// 不依赖自动保存/关窗时机，按需强制持久化。</para>
        /// </summary>
        private void RegisterSaveShortcut()
        {
            rootVisualElement.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt == null) return;
                // Ctrl(win) / Cmd(mac) + S：强制保存当前配置到磁盘
                if (evt.keyCode == KeyCode.S && (evt.ctrlKey || evt.commandKey))
                {
                    // 先提交序列化改动，再强制落盘
                    if (m_SoundSO != null)
                    {
                        m_SoundSO.ApplyModifiedProperties();
                    }
                    SaveSoundConfigToDisk();
                    Debug.Log("[声音表工具] 已通过 Ctrl+S 保存音频配置。");
                    // 阻止默认保存（避免触发 Unity 的全局保存资产）
                    evt.StopPropagation();
                }
            });
        }
        #endregion

        //=========================================================================
        #region 释放
        //=========================================================================

        /// <summary>
        /// 窗口关闭时释放资源并兜底落盘
        /// <para>先注销基类的 EditorApplication.update 持续重绘监听（基类 OnDisable 为
        /// protected 非 virtual，必须显式调用，否则窗口关闭后 update 委托泄漏、每帧空转），
        /// 再兜底保存一次配置（防止最后一次改动未被绘制循环覆盖），最后释放资产引用。</para>
        /// </summary>
        private void OnDisable()
        {
            base.OnDisable();

            // 注意：此处不做无条件自动保存！
            // 若在 Unity 脚本重载 / 窗口重开时 OnDisable 触发，此时 m_SoundConfig
            // 可能处于未初始化或空状态，无条件 SaveSoundConfigToDisk 会把空配置写回资产，
            // 覆盖磁盘上已有的真实数据（即"设置后重开又没了"的根因）。
            // 保存只由明确的 Ctrl+S 手动动作触发，保证绝不意外覆盖。

            m_SoundSO = null;
            m_SoundConfig = null;
        }
        #endregion

        //=========================================================================
        #region 声音分组绘制
        //=========================================================================

        /// <summary>
        /// 绘制声音分组配置列表（基于 SoundConfig 资产的序列化数组）
        /// <para>用 EditorGUILayout.PropertyField 绘制数组，支持增删改。
        /// 每次绘制仅把改动提交到内存（ApplyModifiedProperties），不做时时落盘；
        /// 磁盘保存只由明确的保存动作触发（Ctrl+S 手动保存 / 关闭窗口兜底）。</para>
        /// </summary>
        private void DrawGroupShells()
        {
            if (m_SoundSO == null)
            {
                EditorGUILayout.HelpBox("音频配置资产未加载，请先重新打开本窗口。", MessageType.Warning);
                return;
            }

            m_SoundSO.Update();
            SerializedProperty prop = m_SoundSO.FindProperty("m_SoundGroupShells");
            if (prop != null)
            {
                // 只渲染数组头，不自动展开子项，保持界面紧凑
                EditorGUILayout.PropertyField(prop, includeChildren: true);
            }
            // 仅提交到内存（Apply 后编辑值对资产实例生效、界面可编辑），
            // 不做时时落盘：SaveAssets 仅由明确的保存动作触发
            // （Ctrl+S 手动保存 / 关闭窗口兜底），避免每次交互都写磁盘。
            m_SoundSO.ApplyModifiedProperties();
        }

        /// <summary>
        /// 将音频配置资产改动写回磁盘
        /// <para>AssetDatabase.SaveAssets 会把内存中的序列化改动持久化到 .asset 文件，
        /// 使再次打开窗口或重启 Unity 后能读取到最新配置。此方法幂等、无副作用。</para>
        /// </summary>
        private void SaveSoundConfigToDisk()
        {
            if (m_SoundConfig == null)
            {
                return;
            }

            EditorUtility.SetDirty(m_SoundConfig);
            AssetDatabase.SaveAssets();
        }
        #endregion
    }

    /// <summary>
    /// 音频配置资产的工具方法（查找 / 创建）
    /// </summary>
    internal static class SoundConfigEditorUtils
    {
        /// <summary>
        /// 音频配置资产的存放相对路径（Assets/Resources 下，保证运行时 Resources.Load 可加载）
        /// </summary>
        private const string SoundConfigAssetPath = "Assets/Resources/SoundConfig.asset";

        /// <summary>
        /// 获取或创建音频配置资产
        /// <para>已存在则直接加载；不存在则创建并落盘。供编辑器窗口初始化使用。
        /// 安全保护：若磁盘上已存在资产文件但加载失败，绝不新建覆盖（防止破坏已有配置），
        /// 而是报错返回 null。</para>
        /// </summary>
        /// <returns>音频配置资产实例；创建/加载失败返回 null</returns>
        public static SoundConfig GetOrCreateSoundConfig()
        {
            // 磁盘上已存在资产文件 → 必须成功加载，绝不新建覆盖（避免覆盖已有数据）
            if (File.Exists(SoundConfigAssetPath))
            {
                SoundConfig existing = AssetDatabase.LoadAssetAtPath<SoundConfig>(SoundConfigAssetPath);
                if (existing != null)
                {
                    return existing;
                }

                // 文件存在但加载失败：报错并返回 null，防止用空实例覆盖已有配置
                Debug.LogError("[SoundConfig] 资产文件存在但加载失败（可能 SoundGroupShell 序列化定义变更），" +
                               "已停止创建以防覆盖已有数据。请检查配置。");
                return null;
            }

            SoundConfig config = ScriptableObject.CreateInstance<SoundConfig>();
            if (config == null)
            {
                Debug.LogError("[SoundConfig] 创建音频配置资产失败。");
                return null;
            }

            // 确保 Assets/Resources 目录存在（Unity 内置 Resources 体系，运行时 Resources.Load 可加载）
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            AssetDatabase.CreateAsset(config, SoundConfigAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return config;
        }
    }
}