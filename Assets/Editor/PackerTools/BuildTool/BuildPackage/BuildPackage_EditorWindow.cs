/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_EditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 编辑器窗口
 *            展示打包配置列表（路径/粒度/后缀/启用），提供一键打包、清理输出等操作
 ***************************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Honor.Editor.BuildTool;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // AB 打包编辑器窗口
    //=========================================================================
    /// <summary>
    /// AB 打包编辑器窗口
    /// 说明：以列表形式展示打包配置（路径/粒度/后缀/启用），支持一键打包与清理输出
    /// </summary>
    public class BuildPackage_EditorWindow : EditorWindow
    {
        #region 常量
        /// <summary>
        /// 窗口标题
        /// </summary>
        private const string WindowTitle = "AB 打包工具";
        #endregion

        #region 单例 / 嵌入宿主
        /// <summary>
        /// 已打开的独立窗口实例（OnEnable 赋值 / OnDisable 清空）
        /// </summary>
        public static BuildPackage_EditorWindow Inst { get; private set; }

        /// <summary>
        /// 嵌入宿主实例：供其他窗口（如 Honor 打包工具 PackWindow 的标签页）复用本界面绘制，
        /// 通过 <see cref="CreateInstance{T}"/> 创建（不显示独立窗口），承载打包配置的编辑状态
        /// </summary>
        private static BuildPackage_EditorWindow s_EmbedHost;
        #endregion

        #region 成员
        /// <summary>
        /// 打包配置列表（从默认配置加载，可编辑）
        /// </summary>
        private List<BuildPackageConfig> m_Configs;

        /// <summary>
        /// 滚动位置
        /// </summary>
        private Vector2 m_ScrollPos;

        /// <summary>
        /// 是否启用增量打包（无内容变更则跳过打包）
        /// </summary>
        private bool m_Incremental;
        #endregion

        #region 窗口创建
        /// <summary>
        /// 打开打包窗口
        /// </summary>
        /// <summary>
        /// 打开独立打包窗口（已从菜单移除，保留方法供代码复用；界面已嵌入 PackWindow 的"AB打包工具"标签页）
        /// </summary>
        public static void OpenWindow()
        {
            BuildPackage_EditorWindow win = GetWindow<BuildPackage_EditorWindow>(WindowTitle);
            win.minSize = new Vector2(720, 480);
        }

        /// <summary>
        /// 窗口启用：加载默认配置、登记单例
        /// </summary>
        private void OnEnable()
        {
            Inst = this;
            EnsureConfigsLoaded();
        }

        /// <summary>
        /// 窗口关闭：清空单例
        /// </summary>
        private void OnDisable()
        {
            if (Inst == this) Inst = null;
        }

        /// <summary>
        /// 获取可嵌入绘制实例：优先复用已打开的独立窗口，否则返回隐藏的嵌入宿主
        /// （供其他窗口以标签页形式嵌入本打包配置界面，无需弹出独立窗口）
        /// </summary>
        /// <returns>承载打包配置绘制逻辑的实例</returns>
        public static BuildPackage_EditorWindow GetEmbedInstance()
        {
            if (Inst != null)
                return Inst;

            if (s_EmbedHost == null)
            {
                s_EmbedHost = CreateInstance<BuildPackage_EditorWindow>();
                s_EmbedHost.EnsureConfigsLoaded();
            }

            return s_EmbedHost;
        }

        /// <summary>
        /// 惰性加载默认打包配置（OnEnable 或嵌入宿主创建时调用）
        /// </summary>
        private void EnsureConfigsLoaded()
        {
            if (m_Configs == null || m_Configs.Count == 0)
                m_Configs = BuildPackage_DefaultConfigs.GetDefaults();
        }
        #endregion

        #region 绘制
        /// <summary>
        /// 窗口绘制
        /// </summary>
        private void OnGUI()
        {
            DrawEmbeddedContent();
        }

        /// <summary>
        /// 绘制打包配置界面主体（供本窗口 OnGUI 与外部窗口嵌入共用）
        /// </summary>
        public void DrawEmbeddedContent()
        {
            DrawTopBar();

            EditorGUILayout.Space(6);

            // 检测与默认配置的漂移（窗口可能缓存了旧配置，打包会用错后缀）
            DrawConfigSyncWarning();

            DrawConfigHeader();
            DrawConfigList();

            EditorGUILayout.Space(6);
            DrawBottomBar();
        }

        /// <summary>
        /// 绘制顶部操作栏（一键打包 / 清理输出）
        /// </summary>
        private void DrawTopBar()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"输出目录：{BuildPackage_Define.BuildOutputFullRoot}", EditorStyles.boldLabel);

                // 增量打包开关
                m_Incremental = GUILayout.Toggle(m_Incremental, new GUIContent("增量打包", "无内容变更时跳过打包（签名缓存存于 Library）"), GUILayout.Width(86));

                GUILayout.FlexibleSpace();

                GUI.color = Color.green;
                if (GUILayout.Button("一键打包 AB", GUILayout.Width(110), GUILayout.Height(24)))
                {
                    // 打包属阻塞型编辑器操作，直接在当前 OnGUI 布局作用域（HorizontalScope）内同步执行，
                    // 会在布局 pass 中途重入编辑器主循环，破坏 IMGUI 布局栈，
                    // 引发 “EndLayoutGroup: BeginLayoutGroup must be called first”。
                    // 统一经 EditorDefer.DeferFromOnGUI 推迟到布局 pass 结束后再弹窗与打包。
                    EditorDefer.DeferFromOnGUI(() =>
                    {
                        if (EditorUtility.DisplayDialog("AB打包", "确定开始打包 AssetBundle 吗？", "开始打包", "取消"))
                        {
                            BuildPackage_Builder.BuildAll(m_Configs, m_Incremental);
                        }
                    });
                }
                GUI.color = Color.white;

                if (GUILayout.Button("清理输出", GUILayout.Width(90), GUILayout.Height(24)))
                {
                    EditorDefer.DeferFromOnGUI(() =>
                    {
                        if (EditorUtility.DisplayDialog("AB打包", "确定清空输出目录吗？", "清空", "取消"))
                        {
                            ClearOutput();
                        }
                    });
                }

                // 恢复默认配置（清除窗口缓存的旧配置，重新从默认加载）
                if (GUILayout.Button("恢复默认配置", GUILayout.Width(96), GUILayout.Height(24)))
                {
                    EditorDefer.DeferFromOnGUI(() =>
                    {
                        if (EditorUtility.DisplayDialog("AB打包", "确定恢复为默认打包配置吗？当前自定义将被覆盖。", "恢复", "取消"))
                        {
                            RestoreDefaultConfigs();
                        }
                    });
                }

                // 生成 TMP ShaderVariantCollection（原 Honor/工具 菜单项收口到打包界面）
                // 说明：AB 打包按 Strip Unused 裁剪 TMP shader 变体会导致 AB 内 TMP 文字渲染为空，
                //       需在打包前生成 .shadervariants 到 Resources 保留变体；生成后需重新打包 AB。
                if (GUILayout.Button("生成TMP变体集合", GUILayout.Width(130), GUILayout.Height(24)))
                {
                    EditorDefer.DeferFromOnGUI(() =>
                    {
                        if (EditorUtility.DisplayDialog("TMP SVC",
                                "确定生成 TMP ShaderVariantCollection 吗？\n生成后需重新打包 AB 才能生效。",
                                "生成", "取消"))
                        {
                            Honor.Editor.TMPShaderVariantBuilder.GenerateTmpShaderVariants();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 绘制配置列表表头
        /// </summary>
        private void DrawConfigHeader()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("启用", GUILayout.Width(34));
                GUILayout.Label("源目录", GUILayout.Width(230));
                GUILayout.Label("粒度", GUILayout.Width(120));
                GUILayout.Label("后缀过滤", GUILayout.ExpandWidth(true));
                GUILayout.Label("删除", GUILayout.Width(40));
            }
        }

        /// <summary>
        /// 绘制配置列表（滚动）
        /// </summary>
        private void DrawConfigList()
        {
            using (EditorGUILayout.ScrollViewScope scroll = new EditorGUILayout.ScrollViewScope(m_ScrollPos))
            {
                m_ScrollPos = scroll.scrollPosition;

                if (m_Configs == null)
                    return;

                for (int i = 0; i < m_Configs.Count; i++)
                {
                    BuildPackageConfig cfg = m_Configs[i];
                    if (cfg == null)
                        continue;

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        // 启用开关
                        cfg.Enabled = GUILayout.Toggle(cfg.Enabled, GUIContent.none, GUILayout.Width(34));

                        // 源目录（只读展示）
                        EditorGUI.BeginDisabledGroup(true);
                        GUILayout.TextField(cfg.Path, GUILayout.Width(230));
                        EditorGUI.EndDisabledGroup();

                        // 打包粒度（下拉选择）
                        cfg.Format = (BuildPackageFormat)EditorGUILayout.EnumPopup(cfg.Format, GUILayout.Width(120));

                        // 后缀过滤（下拉选择预设，避免手填输错；选中后回填 Pattern）
                        int curPreset = BuildPackage_PatternPreset.IndexOf(cfg.Pattern);
                        int newPreset = EditorGUILayout.Popup(curPreset, BuildPackage_PatternPreset.Labels, GUILayout.Width(180));
                        if (newPreset != curPreset)
                            cfg.Pattern = BuildPackage_PatternPreset.Patterns[newPreset];

                        // 删除
                        GUI.color = new Color(1f, 0.5f, 0.5f);
                        if (GUILayout.Button("×", GUILayout.Width(40)))
                        {
                            m_Configs.RemoveAt(i);
                            i--;
                        }
                        GUI.color = Color.white;
                    }
                }

                // 新增配置
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Space(34);
                    if (GUILayout.Button("+ 新增打包目录", GUILayout.Width(160)))
                    {
                        m_Configs.Add(new BuildPackageConfig
                        {
                            Path = "Assets/Res/NewFolder",
                            Format = BuildPackageFormat.EachFile,
                            Pattern = string.Empty,
                            Enabled = true,
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 绘制底部状态栏（帮助提示）
        /// </summary>
        private void DrawBottomBar()
        {
            EditorGUILayout.HelpBox(
                "bundle 命名：相对路径 '/'→'@' + 全小写 + '.bundle'，与运行时 AssetBundleLoadManager 约定一致。\n" +
                "粒度说明：每个文件单独 / 当前目录 / 一级目录 / 二级目录 / 三级目录。",
                MessageType.Info);
        }

        /// <summary>
        /// 清空 AB 输出目录
        /// </summary>
        private void ClearOutput()
        {
            string outRoot = BuildPackage_Define.BuildOutputFullRoot;
            if (System.IO.Directory.Exists(outRoot))
            {
                System.IO.Directory.Delete(outRoot, true);
                AssetDatabase.Refresh();
                Debug.Log($"[AB打包] 已清空输出目录：{outRoot}");
            }
        }
        #endregion

        #region 配置同步（防止窗口缓存旧配置）
        /// <summary>
        /// 检测当前配置与默认配置的漂移
        /// 说明：窗口在 OnEnable 时把默认配置缓存进 m_Configs，之后不再随默认配置变更而刷新；
        ///       若默认配置被修改（如 Fonts 增加 .mat），旧窗口仍持有旧配置，打包会漏资源。
        ///       此处按"源目录"比对后缀，发现不一致即提示用户点击『恢复默认配置』。
        /// </summary>
        private void DrawConfigSyncWarning()
        {
            if (m_Configs == null || m_Configs.Count == 0)
                return;

            // 默认配置按源目录索引
            Dictionary<string, string> defaultPatternByPath = new Dictionary<string, string>();
            foreach (BuildPackageConfig def in BuildPackage_DefaultConfigs.GetDefaults())
            {
                if (def != null && !string.IsNullOrEmpty(def.Path) && !defaultPatternByPath.ContainsKey(def.Path))
                    defaultPatternByPath.Add(def.Path, def.Pattern);
            }

            // 统计与默认配置存在后缀漂移的项
            List<string> drifted = new List<string>();
            foreach (BuildPackageConfig cfg in m_Configs)
            {
                if (cfg == null || string.IsNullOrEmpty(cfg.Path))
                    continue;
                if (!defaultPatternByPath.TryGetValue(cfg.Path, out string defaultPattern))
                    continue;
                if (!string.Equals(cfg.Pattern, defaultPattern, System.StringComparison.OrdinalIgnoreCase))
                    drifted.Add($"{cfg.Path}（默认: {defaultPattern}）");
            }

            if (drifted.Count > 0)
            {
                EditorGUILayout.HelpBox(
                    "以下目录的后缀过滤与默认配置不一致（可能是窗口缓存了旧配置，会导致打包漏资源）：\n" +
                    string.Join("\n", drifted) +
                    "\n如需恢复默认，请点击顶部『恢复默认配置』。",
                    MessageType.Warning);
                EditorGUILayout.Space(2);
            }
        }

        /// <summary>
        /// 恢复为默认打包配置（重新从 BuildPackage_DefaultConfigs 加载）
        /// </summary>
        private void RestoreDefaultConfigs()
        {
            m_Configs = BuildPackage_DefaultConfigs.GetDefaults();
            Debug.Log("[AB打包] 已恢复为默认打包配置。");
            Repaint();
        }
        #endregion
    }
}
