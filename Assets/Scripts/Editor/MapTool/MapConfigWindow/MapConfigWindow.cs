/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapConfigWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   章节地图预制体配置窗口（EditorWindow，UI 完全由 UXML/USS 驱动）
 ***************************************************************/

using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图配置窗口
    /// 配置章节对应的地图预制体与工具路径（布局取自 MapConfigWindow.uxml/.uss）
    /// </summary>
    public class MapConfigWindow : EditorWindow
    {
        #region 路径项配置表

        /// <summary>
        /// 可配置路径项定义（与 UXML 中 path-btn-* 元素一一对应）
        /// </summary>
        private readonly struct PathConfigItem
        {
            /// <summary>UXML 按钮元素名</summary>
            public readonly string ElementName;
            /// <summary>配置项名称（选择器标题）</summary>
            public readonly string Title;
            /// <summary>EditorPrefs 键</summary>
            public readonly string PrefsKey;
            /// <summary>是否为文件（配置表）；false 表示目录</summary>
            public readonly bool IsFile;

            public PathConfigItem(string elementName, string title, string prefsKey, bool isFile)
            {
                ElementName = elementName;
                Title = title;
                PrefsKey = prefsKey;
                IsFile = isFile;
            }
        }

        /// <summary>
        /// 全部可配置路径项（章节配置表/主关卡配置表为文件，其余为目录）
        /// </summary>
        private static readonly PathConfigItem[] PathConfigItems =
        {
            new PathConfigItem("path-btn-table-chapters", "章节配置表", MapToolConstants.TableChaptersPathPrefsKey, true),
            new PathConfigItem("path-btn-table-main-levels", "主关卡配置表", MapToolConstants.TableMainLevelsPathPrefsKey, true),
            new PathConfigItem("path-btn-object-prefabs", "物体预览图目录一", MapToolConstants.ObjectPreviewPrefabsDirPrefsKey, false),
            new PathConfigItem("path-btn-object-textures", "物体预览图目录二", MapToolConstants.ObjectPreviewTexturesDirPrefsKey, false),
            new PathConfigItem("path-btn-level-background", "分区背景图目录", MapToolConstants.LevelBackgroundDirPrefsKey, false),
            new PathConfigItem("path-btn-map-save", "地图保存目录", MapToolConstants.MapSaveDirPrefsKey, false),
        };

        #endregion

        #region 字段

        /// <summary>窗口视觉树资源（Inspector 可配置）</summary>
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        /// <summary>窗口样式表（Inspector 可配置）</summary>
        [SerializeField]
        private StyleSheet m_StyleSheet = default;

        /// <summary>当前配置的章节ID</summary>
        private string m_ChapterId;
        /// <summary>地图预制体路径</summary>
        private string m_MapPrefabPath;
        /// <summary>地图预制体对象</summary>
        private GameObject m_MapPrefab;
        /// <summary>预制体选择框</summary>
        private ObjectField m_PrefabField;

        /// <summary>配置完成回调（章节ID, 预制体路径）</summary>
        public System.Action<string, string> OnConfigConfirmed;

        #endregion

        #region 窗口入口

        /// <summary>
        /// 显示地图配置窗口
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="currentPath">当前预制体路径</param>
        /// <param name="onConfirmed">配置确认回调</param>
        public static void ShowWindow(string chapterId, string currentPath, System.Action<string, string> onConfirmed = null)
        {
            // 当前路径为空时，回退到全局模板路径
            string templatePath = EditorPrefs.GetString(MapToolConstants.TemplatePrefabPrefsKey, "");

            var window = GetWindow<MapConfigWindow>(true, "地图配置");
            window.minSize = new Vector2(720, 470);
            window.m_ChapterId = chapterId;
            window.m_MapPrefabPath = string.IsNullOrEmpty(currentPath) ? templatePath : currentPath;
            window.OnConfigConfirmed = onConfirmed;
            window.Show();
            window.Focus();

            // 窗口重新显示后，刷新预制体选择框
            window.RefreshPrefabField();
        }

        /// <summary>
        /// 刷新预制体选择框（按当前路径加载并回显）
        /// </summary>
        public void RefreshPrefabField()
        {
            if (m_PrefabField == null)
                return;

            m_MapPrefab = string.IsNullOrEmpty(m_MapPrefabPath)
                ? null
                : AssetDatabase.LoadAssetAtPath<GameObject>(m_MapPrefabPath);
            m_PrefabField.value = m_MapPrefab;
        }

        #endregion

        #region UI 构建

        /// <summary>
        /// 构建 UI：加载样式表与视觉树，查找元素并绑定事件
        /// 资源优先使用 Inspector 字段（校验有效性），失效或未配置时按约定路径自动加载；
        /// 视觉树缺失时输出警告（UI 元素均由 UXML 定义，不做代码回退）
        /// </summary>
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            // 加载样式表
            StyleSheet styleSheet = IsValidAsset(m_StyleSheet)
                ? m_StyleSheet
                : AssetDatabase.LoadAssetAtPath<StyleSheet>(MapToolConstants.MapConfigWindowUssPath);
            if (styleSheet != null)
                root.styleSheets.Add(styleSheet);

            // 加载视觉树
            VisualTreeAsset visualTree = IsValidAsset(m_VisualTreeAsset)
                ? m_VisualTreeAsset
                : AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MapToolConstants.MapConfigWindowUxmlPath);
            if (visualTree == null)
            {
                Debug.LogWarning($"[MapConfigWindow] 视觉树资源缺失：{MapToolConstants.MapConfigWindowUxmlPath}");
                return;
            }

            root.Add(visualTree.Instantiate());

            FindElements();
            BindEvents();
            RefreshPrefabField();
        }

        /// <summary>
        /// 校验资产引用是否有效（非空且仍存在于资产数据库）
        /// 防止文件移动/重命名后序列化字段指向已失效资产导致 "Accessing invalid property"
        /// </summary>
        /// <param name="asset">待校验的资产引用</param>
        private static bool IsValidAsset(Object asset)
        {
            return asset != null && AssetDatabase.Contains(asset);
        }

        #endregion

        #region 元素查找与事件绑定

        /// <summary>
        /// 查找预制体选择框与全部路径按钮，回显当前配置值并绑定事件
        /// </summary>
        private void FindElements()
        {
            VisualElement root = rootVisualElement;

            m_PrefabField = root.Q<ObjectField>(name: "map-prefab-field");
            m_PrefabField?.RegisterValueChangedCallback(OnPrefabChanged);

            foreach (PathConfigItem item in PathConfigItems)
                CollectPathButton(item);
        }

        /// <summary>
        /// 收集一个路径选择按钮：回显当前值并绑定点击事件
        /// </summary>
        /// <param name="item">路径项配置</param>
        private void CollectPathButton(PathConfigItem item)
        {
            var pathButton = rootVisualElement.Q<Button>(name: item.ElementName);
            if (pathButton == null)
                return;

            pathButton.text = MapToolConstants.GetConfigPath(item.PrefsKey);
            pathButton.clicked += () => OnPathFieldClicked(item, pathButton);
        }

        /// <summary>
        /// 绑定取消 / 确定按钮点击事件
        /// </summary>
        private void BindEvents()
        {
            VisualElement root = rootVisualElement;

            var cancelBtn = root.Q<Button>(name: "btn-cancel");
            if (cancelBtn != null)
                cancelBtn.clicked += CancelClicked;

            var confirmBtn = root.Q<Button>(name: "btn-confirm");
            if (confirmBtn != null)
                confirmBtn.clicked += ConfirmClicked;
        }

        #endregion

        #region 路径选择

        /// <summary>
        /// 路径选择按钮点击：按类型弹出文件/文件夹选择器，选中后自动保存
        /// </summary>
        /// <param name="item">路径项配置</param>
        /// <param name="pathButton">路径显示按钮</param>
        private void OnPathFieldClicked(PathConfigItem item, Button pathButton)
        {
            string current = MapToolConstants.GetConfigPath(item.PrefsKey);
            string startDir = ResolveStartDirectory(item, current);
            string selected = item.IsFile
                ? EditorUtility.OpenFilePanel($"选择{item.Title}", startDir, "xlsm")
                : EditorUtility.OpenFolderPanel($"选择{item.Title}", startDir, "");

            if (string.IsNullOrEmpty(selected))
                return;

            // 转为相对路径并自动保存
            string relative = MakeRelativePath(item.IsFile ? Application.dataPath : GetProjectRoot(), selected);
            MapToolConstants.SetConfigPath(item.PrefsKey, relative);
            pathButton.text = relative;
        }

        /// <summary>
        /// 解析选择器初始目录：将当前配置的相对路径规范化（消除 ../）并定位到实际目录；
        /// 路径无效或不存在时回退到基准目录
        /// </summary>
        /// <param name="item">路径项配置</param>
        /// <param name="current">当前配置的相对路径</param>
        private static string ResolveStartDirectory(PathConfigItem item, string current)
        {
            string baseDir = item.IsFile ? Application.dataPath : GetProjectRoot();
            if (string.IsNullOrEmpty(current))
                return baseDir;

            // 规范化相对路径（消除 ../，得到绝对路径）
            string full = Path.GetFullPath(Path.Combine(baseDir, current));

            // 文件型配置：初始目录取其所在目录
            if (item.IsFile)
                full = Path.GetDirectoryName(full) ?? baseDir;

            return Directory.Exists(full) ? full : baseDir;
        }

        /// <summary>
        /// 获取项目根目录（Assets 的父目录）
        /// </summary>
        private static string GetProjectRoot()
        {
            return Path.GetDirectoryName(Application.dataPath);
        }

        /// <summary>
        /// 将目标路径转为相对 basePath 的相对路径（统一为正斜杠）
        /// </summary>
        /// <param name="basePath">基准目录</param>
        /// <param name="targetPath">目标路径</param>
        private static string MakeRelativePath(string basePath, string targetPath)
        {
            string[] baseParts = Path.GetFullPath(basePath).Split('\\', '/');
            string[] targetParts = Path.GetFullPath(targetPath).Split('\\', '/');

            // 计算共同前缀长度
            int common = 0;
            while (common < baseParts.Length && common < targetParts.Length
                   && string.Equals(baseParts[common], targetParts[common], System.StringComparison.OrdinalIgnoreCase))
            {
                common++;
            }

            var sb = new StringBuilder();
            for (int i = common; i < baseParts.Length; i++)
                sb.Append("../");
            for (int i = common; i < targetParts.Length; i++)
            {
                if (i > common)
                    sb.Append('/');
                sb.Append(targetParts[i]);
            }
            return sb.ToString().Replace('\\', '/');
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 预制体选择变化：同步路径与对象
        /// </summary>
        private void OnPrefabChanged(ChangeEvent<Object> evt)
        {
            m_MapPrefab = evt.newValue as GameObject;
            m_MapPrefabPath = m_MapPrefab != null ? AssetDatabase.GetAssetPath(m_MapPrefab) : "";
        }

        /// <summary>
        /// 确定按钮点击：保存全局模板路径并触发确认回调
        /// （配置路径已在选择时自动保存到 EditorPrefs）
        /// </summary>
        private void ConfirmClicked()
        {
            // 保存全局模板路径到 EditorPrefs
            EditorPrefs.SetString(MapToolConstants.TemplatePrefabPrefsKey, m_MapPrefabPath);

            OnConfigConfirmed?.Invoke(m_ChapterId, m_MapPrefabPath);
            Close();
        }

        /// <summary>
        /// 取消按钮点击
        /// </summary>
        private void CancelClicked()
        {
            Close();
        }

        #endregion
    }
}
