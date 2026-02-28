using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using TMPro;

public class TileMapEditorWindow : EditorWindow
{
    // 状态变量（匹配截图信息）
    private float _zoom = 90f;
    private Vector2 _mousePos = new Vector2(387, 786);
    private Vector2 _tilePos = new Vector2(29, 49);
    private int _gridIndex = 6525;
    private string _sceneName = "105";
    private int _sceneWidth = 1320;
    private int _sceneHeight = 990;
    private string _currentTemplate = "B_1014";
    private string _currentTag = "105_166435248135";

    // UI元素引用
    private VisualElement _sceneEditArea;
    private ListView _leftResourceList;
    private ListView _rightItemList;
    private Label _statusLabel;
    private TextField _sceneNameField;
    private Slider _zoomSlider;


    // 打开窗口入口
    [MenuItem("Tools/瓦片地图编辑器")]
    public static void ShowWindow()
    {
        var window = GetWindow<TileMapEditorWindow>("场景编辑器");
        window.minSize = new Vector2(1200, 1080); // 匹配截图的宽高比
    }


    // UI初始化（核心布局构建）
    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        root.style.paddingLeft = 2;
        root.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);


        // ========== 1. 顶部功能栏（标签页+操作按钮） ==========
        BuildTopToolbar(root);


        // ========== 2. 中间核心分栏（左+中+右） ==========
        VisualElement mainContainer = new VisualElement();
        mainContainer.style.flexDirection = FlexDirection.Row;
        mainContainer.style.flexGrow = 1;
       // mainContainer.style.gap = 2;

        // 左侧资源栏
        VisualElement leftPanel = BuildLeftResourcePanel();
        // 中间场景编辑区
        VisualElement middlePanel = BuildMiddleScenePanel();
        // 右侧面板（列表+属性）
        VisualElement rightPanel = BuildRightPanel();

        mainContainer.Add(leftPanel);
        mainContainer.Add(middlePanel);
        mainContainer.Add(rightPanel);
        root.Add(mainContainer);


        // ========== 3. 底部操作栏 ==========
        BuildBottomToolbar(root);


        // 初始化数据显示
        UpdateStatusInfo();
    }


    #region 模块1：顶部功能栏
    private void BuildTopToolbar(VisualElement root)
    {
        // 标签页容器（场景编辑/物品编辑/连笔编辑）
        Toolbar tabToolbar = new Toolbar();
        tabToolbar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

        // 标签页按钮（模拟Tab切换）
        AddTabButton(tabToolbar, "场景编辑", true);
        AddTabButton(tabToolbar, "物品元件编辑", false);
        AddTabButton(tabToolbar, "连笔编辑", false);

        // 功能按钮组
        ToolbarSpacer spacer = new ToolbarSpacer { flex = true };
        tabToolbar.Add(spacer);

        AddToolButton(tabToolbar, "导出设置");
        AddToolButton(tabToolbar, "保存");
        AddToolButton(tabToolbar, "清空摆饰物");
        AddToolButton(tabToolbar, "清空格子数据");

        // 图层选择（复选框）
        Toolbar layerToolbar = new Toolbar();
        layerToolbar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        layerToolbar.style.marginTop = 2;

        layerToolbar.Add(new Label("图层: "));
        AddLayerToggle(layerToolbar, "摆放物图层");
        AddLayerToggle(layerToolbar, "默认图层");
        AddLayerToggle(layerToolbar, "物件层");
        AddLayerToggle(layerToolbar, "背景层");

        root.Add(tabToolbar);
        root.Add(layerToolbar);
    }

    // 添加标签页按钮
    private void AddTabButton(Toolbar parent, string text, bool isActive)
    {
        Button tabBtn = new Button(() => SwitchTab(text)) { text = text };
        tabBtn.style.backgroundColor = isActive ? new Color(0.3f, 0.3f, 0.3f) : new Color(0.2f, 0.2f, 0.2f);
        tabBtn.style.color = Color.white;
        tabBtn.style.marginRight = 2;
        parent.Add(tabBtn);
    }

    // 添加功能按钮
    private void AddToolButton(Toolbar parent, string text)
    {
        Button btn = new Button(() => Debug.Log($"点击功能按钮: {text}")) { text = text };
        btn.style.minWidth = 80;
        btn.style.marginRight = 2;
        parent.Add(btn);
    }

    // 添加图层复选框
    private void AddLayerToggle(Toolbar parent, string label)
    {
        Toggle toggle = new Toggle(label);
        toggle.style.color = Color.white;
        toggle.value = true; // 默认选中
        parent.Add(toggle);
    }

    // 标签页切换逻辑
    private void SwitchTab(string tabName)
    {
        Debug.Log($"切换到标签页: {tabName}");
        // 实际项目中可隐藏/显示对应区域
    }
    #endregion


    #region 模块2：左侧资源栏
    private VisualElement BuildLeftResourcePanel()
    {
        VisualElement leftPanel = new VisualElement();
        leftPanel.style.width = 120;
        leftPanel.style.flexDirection = FlexDirection.Column;
        leftPanel.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
        //leftPanel.style.padding = 2;

        // 分类标签（建筑/地表/物件）
        AddCategoryLabel(leftPanel, "建筑");
        AddCategoryLabel(leftPanel, "地表");
        AddCategoryLabel(leftPanel, "物件");
        AddCategoryLabel(leftPanel, "刷新");

        // 资源列表（模拟截图中的资源图标）
        List<string> resourceList = new List<string>();
        for (int i = 101; i <= 1022; i++) resourceList.Add($"-{i}");

        _leftResourceList = new ListView();
        _leftResourceList.itemsSource = resourceList;
        _leftResourceList.makeItem = () => new Label();
        _leftResourceList.bindItem = (elem, idx) =>
        {
            Label label = elem as Label;
            label.text = resourceList[idx];
            label.style.color = Color.gray;
            //label.style.padding = 2;
        };
        _leftResourceList.style.flexGrow = 1;
        leftPanel.Add(_leftResourceList);

        return leftPanel;
    }

    // 添加分类标签
    private void AddCategoryLabel(VisualElement parent, string text)
    {
        Label label = new Label(text);
        label.style.color = Color.white;
        label.style.fontSize = 11;
        //label.style.fontWeight = FontWeight.Bold;
        label.style.marginBottom = 2;
        parent.Add(label);
    }
    #endregion


    #region 模块3：中间场景编辑区
    private VisualElement BuildMiddleScenePanel()
    {
        VisualElement middlePanel = new VisualElement();
        middlePanel.style.flexGrow = 1;
        middlePanel.style.flexDirection = FlexDirection.Column;
        middlePanel.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

        // 场景编辑区域（显示地图预览）
        _sceneEditArea = new VisualElement();
        _sceneEditArea.style.flexGrow = 1;
        _sceneEditArea.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        // 模拟截图中的地图背景（实际项目中可绘制Texture2D）
        _sceneEditArea.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/map_preview.png"));
        //_sceneEditArea.style.backgroundSize = new StyleBackgroundSize(BackgroundSize.Cover);
        // 监听鼠标事件（更新坐标）
        _sceneEditArea.RegisterCallback<MouseMoveEvent>(OnSceneMouseMove);
        middlePanel.Add(_sceneEditArea);

        // 状态信息栏
        _statusLabel = new Label();
        _statusLabel.style.color = Color.gray;
        _statusLabel.style.fontSize = 10;
        //_statusLabel.style.padding = 2;
        middlePanel.Add(_statusLabel);

        return middlePanel;
    }

    // 场景区域鼠标移动：更新坐标信息
    private void OnSceneMouseMove(MouseMoveEvent evt)
    {
        //_mousePos = evt.localPosition;
        // 模拟瓦片坐标计算（实际项目中根据瓦片大小转换）
        //_tilePos = new Vector2((int)(_mousePos.x / 32), (int)(_mousePos.y / 32));
        UpdateStatusInfo();
    }

    // 更新状态信息（鼠标/瓦片/格子）
    private void UpdateStatusInfo()
    {
        _statusLabel.text = $"鼠标: {_mousePos:F0} | 瓦片: {_tilePos} | 格子数: {_gridIndex} | 缩放: {_zoom}%";
    }
    #endregion


    #region 模块4：右侧面板（列表+属性）
    private VisualElement BuildRightPanel()
    {
        VisualElement rightPanel = new VisualElement();
        rightPanel.style.width = 300;
        rightPanel.style.flexDirection = FlexDirection.Column;
        rightPanel.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
        //rightPanel.style.padding = 2;

        // 在线列表（上半部分）
        VisualElement itemListPanel = new VisualElement();
        itemListPanel.style.flexDirection = FlexDirection.Column;
        itemListPanel.style.flexGrow = 1;

        Label listTitle = new Label("在线列表");
        listTitle.style.color = Color.white;
        listTitle.style.marginBottom = 2;
        itemListPanel.Add(listTitle);

        // 在线物品列表
        List<string> itemList = new List<string>();
        for (int i = 31; i <= 40; i++) itemList.Add($"mid_xm0{i}");
        itemList.AddRange(new List<string> { "mid_xm002", "mid_xm003", "mid_ym001" });

        _rightItemList = new ListView();
        _rightItemList.itemsSource = itemList;
        _rightItemList.makeItem = () => new Label();
        _rightItemList.bindItem = (elem, idx) =>
        {
            Label label = elem as Label;
            label.text = itemList[idx];
            //label.style.color = Color.lightGray;
            //label.style.padding = 2;
        };
        _rightItemList.style.flexGrow = 1;
        itemListPanel.Add(_rightItemList);


        // 属性面板（下半部分）
        VisualElement propPanel = new VisualElement();
        propPanel.style.flexDirection = FlexDirection.Column;
        propPanel.style.marginTop = 2;
        propPanel.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

        // 属性表格（模拟截图中的键值对）
        AddPropRow(propPanel, "模板ID", _currentTemplate);
        AddPropRow(propPanel, "tag", _currentTag);
        AddPropRow(propPanel, "格子坐标", $"{_tilePos.x}-{_tilePos.y}");
        AddPropRow(propPanel, "建筑模板", "B_1014P27");
        AddPropRow(propPanel, "交互状态", "点击&移动");


        rightPanel.Add(itemListPanel);
        rightPanel.Add(propPanel);
        return rightPanel;
    }

    // 添加属性行（标签+值）
    private void AddPropRow(VisualElement parent, string label, string value)
    {
        VisualElement row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.paddingLeft = 2;

        Label lab = new Label(label) { style = { color = Color.gray, width = 80 } };
        Label val = new Label(value) { style = { color = Color.blue } };

        row.Add(lab);
        row.Add(val);
        parent.Add(row);
    }
    #endregion


    #region 模块5：底部操作栏
    private void BuildBottomToolbar(VisualElement root)
    {
        VisualElement bottomBar = new VisualElement();
        bottomBar.style.flexDirection = FlexDirection.Row;
        bottomBar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        //bottomBar.style.padding = 2;
        bottomBar.style.marginTop = 2;

        // 左侧功能按钮
        AddBottomButton(bottomBar, "更新地图");
        AddBottomButton(bottomBar, "导出切片");

        // 场景参数设置
        VisualElement paramArea = new VisualElement();
        paramArea.style.flexDirection = FlexDirection.Row;
        paramArea.style.flexGrow = 1;
        paramArea.style.paddingLeft = 10;

        AddParamField(paramArea, "场景名", _sceneName);
        AddParamField(paramArea, "宽", _sceneWidth.ToString());
        AddParamField(paramArea, "高", _sceneHeight.ToString());

        // 缩放滑块
        _zoomSlider = new Slider("缩放", 50, 150);
        _zoomSlider.value = _zoom;
        _zoomSlider.style.width = 150;
        _zoomSlider.RegisterValueChangedCallback(evt =>
        {
            _zoom = evt.newValue;
            UpdateStatusInfo();
        });
        paramArea.Add(_zoomSlider);

        bottomBar.Add(paramArea);

        // 操作说明（右侧文本）
        Label tipLabel = new Label("操作: 按住左键拖拽地图 | 点击放置资源");
        tipLabel.style.color = Color.gray;
        tipLabel.style.fontSize = 10;
        bottomBar.Add(tipLabel);

        root.Add(bottomBar);
    }

    // 添加底部按钮
    private void AddBottomButton(VisualElement parent, string text)
    {
        Button btn = new Button(() => Debug.Log($"点击底部按钮: {text}")) { text = text };
        btn.style.minWidth = 80;
        btn.style.marginRight = 5;
        parent.Add(btn);
    }

    // 添加参数输入框
    private void AddParamField(VisualElement parent, string label, string value)
    {
        VisualElement field = new VisualElement();
        field.style.flexDirection = FlexDirection.Row;
        field.style.marginRight = 10;

        Label lab = new Label(label) { style = { color = Color.gray, width = 50 } };
        TextField input = new TextField { value = value, style = { width = 60 } };
        field.Add(lab);
        field.Add(input);
        parent.Add(field);
    }
    #endregion
}