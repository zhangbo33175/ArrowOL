/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolUiNames.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 UXML 元素名集中定义（消除 Q<Button>(name: "...") 硬编码）
 *            UXML 改名时只需改本文件，编译器会报错同步所有引用
 ***************************************************************/

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器 UXML 元素名常量
    /// 按模块分组：按钮 / 图层 / 状态栏 / 场景编辑面板 / 物件编辑面板 / 左侧面板
    /// </summary>
    public static class MapToolUiNames
    {
        // ==================== 样式类名 ====================
        /// <summary>激活态样式类名（Tab / 子 Tab 高亮）</summary>
        public const string ActiveClass            = MapToolStyleClasses.Active;
        /// <summary>左侧面板样式类名（章节目录 + 资源列表容器）</summary>
        public const string LeftPanelClass         = "left-panel";
        /// <summary>顶部工具栏样式类名</summary>
        public const string TopToolbarClass        = "top-toolbar";
        /// <summary>选中态样式类名（章节/关卡/资源行）</summary>
        public const string SelectedClass          = MapToolStyleClasses.Selected;
        /// <summary>章节行样式类名</summary>
        public const string ChapterItemClass       = "chapter-item";
        /// <summary>章节标题行样式类名</summary>
        public const string ChapterHeaderClass     = "chapter-header";
        /// <summary>章节标题文字样式类名</summary>
        public const string ChapterTitleClass      = "chapter-title";
        /// <summary>展开箭头样式类名</summary>
        public const string ExpandArrowClass       = "expand-arrow";
        /// <summary>关卡容器样式类名</summary>
        public const string LevelContainerClass    = "level-container";
        /// <summary>关卡行样式类名</summary>
        public const string LevelItemClass         = "level-item";
        /// <summary>资源行样式类名</summary>
        public const string ResourceItemClass      = "resource-item";
        /// <summary>资源图标样式类名</summary>
        public const string ResourceIconClass      = "resource-icon";
        /// <summary>资源名称样式类名</summary>
        public const string ResourceNameClass      = "resource-name";

        // ==================== 通用按钮 ====================
        /// <summary>保存地图按钮</summary>
        public const string BtnSave              = "btn-save";
        /// <summary>更新地图按钮（从磁盘 JSON 重新加载）</summary>
        public const string BtnUpdateMap         = "btn-update-map";
        /// <summary>导出切片按钮</summary>
        public const string BtnExportSlice       = "btn-export-slice";
        /// <summary>导出设置按钮</summary>
        public const string BtnExportSettings     = "btn-export-settings";
        /// <summary>预览地图按钮</summary>
        public const string BtnPreviewMap        = "btn-preview-map";
        /// <summary>刷新层级按钮</summary>
        public const string BtnRefreshLayer      = "btn-refresh-layer";
        /// <summary>扫描资源按钮</summary>
        public const string BtnScanResources     = "btn-scan-resources";
        /// <summary>添加到配置表按钮（单个）</summary>
        public const string BtnAddToConfig       = "btn-add-to-config";
        /// <summary>全部添加到配置表按钮</summary>
        public const string BtnAddAllToConfig    = "btn-add-all-to-config";
        /// <summary>保存配置按钮（写回 Excel）</summary>
        public const string BtnSaveConfig        = "btn-save-config";
        /// <summary>打开配置表按钮</summary>
        public const string BtnOpenConfig        = "btn-open-config";
        /// <summary>打开文件夹按钮</summary>
        public const string BtnOpenFolder       = "btn-open-folder";
        /// <summary>清空所有摆放物体按钮</summary>
        public const string BtnClearObjects      = "btn-clear-objects";
        /// <summary>清空格子数据按钮</summary>
        public const string BtnClearGridData     = "btn-clear-grid-data";
        /// <summary>导入 PSD/PSB 按钮</summary>
        public const string BtnImportPsd         = "btn-import-psd";
        /// <summary>删除选中物体按钮</summary>
        public const string BtnDeleteObject      = "btn-delete-object";
        /// <summary>发布地图按钮</summary>
        public const string BtnPublish           = "btn-publish";
        /// <summary>配置按钮（左侧面板）</summary>
        public const string BtnConfig            = "btn-config";
        /// <summary>删除按钮（章节）</summary>
        public const string BtnDelete            = "btn-delete";
        /// <summary>刷新按钮</summary>
        public const string BtnRefresh           = "btn-refresh";
        /// <summary>确认按钮（弹窗）</summary>
        public const string BtnConfirm          = "btn-confirm";
        /// <summary>取消按钮（弹窗）</summary>
        public const string BtnCancel            = "btn-cancel";

        // ==================== 路径编辑按钮 ====================
        /// <summary>绘制路径按钮</summary>
        public const string BtnPathDraw          = "btn-path-draw";
        /// <summary>撤销路径点按钮</summary>
        public const string BtnPathUndo         = "btn-path-undo";
        /// <summary>清空路径按钮</summary>
        public const string BtnPathClear         = "btn-path-clear";
        /// <summary>清除目标按钮</summary>
        public const string BtnClearTarget       = "btn-clear-target";

        // ==================== 图层开关 ====================
        /// <summary>摆放网格图层开关（非层级覆盖）</summary>
        public const string LayerPlaceGrid       = "layer-place-grid";
        /// <summary>路径网格图层开关（非层级覆盖）</summary>
        public const string LayerPathGrid        = "layer-path-grid";
        /// <summary>地图底图开关（非层级覆盖）</summary>
        public const string LayerMap             = "layer-map";
        /// <summary>背景图层开关（background）</summary>
        public const string LayerBackground      = "layer-background";
        /// <summary>绿化图层开关（greenery）</summary>
        public const string LayerGreenery        = "layer-greenery";
        /// <summary>道路市政图层开关（road）</summary>
        public const string LayerRoad            = "layer-road";
        /// <summary>建筑图层开关（building）</summary>
        public const string LayerBuilding        = "layer-building";
        /// <summary>道具图层开关（prop）</summary>
        public const string LayerObject          = "layer-object";
        /// <summary>角色/载具图层开关（character）</summary>
        public const string LayerCharacter       = "layer-character";
        /// <summary>前景近景图层开关（foreground）</summary>
        public const string LayerForeground       = "layer-foreground";
        /// <summary>气泡图层开关（bubble）</summary>
        public const string LayerBubbles         = "layer-bubbles";
        /// <summary>包围盒调试开关（非层级覆盖）</summary>
        public const string LayerBoundingBox     = "layer-bounding-box";

        // ==================== 状态栏 ====================
        /// <summary>状态栏缩放比例显示</summary>
        public const string StatusZoom           = "status-zoom";
        /// <summary>状态栏鼠标 X 坐标</summary>
        public const string StatusMouseX         = "status-mouse-x";
        /// <summary>状态栏鼠标 Y 坐标</summary>
        public const string StatusMouseY         = "status-mouse-y";
        /// <summary>状态栏格子 X 坐标</summary>
        public const string StatusTileX          = "status-tile-x";
        /// <summary>状态栏格子 Y 坐标</summary>
        public const string StatusTileY          = "status-tile-y";
        /// <summary>状态栏格子总数（已占用去重计数）</summary>
        public const string StatusTileCount      = "status-tile-count";
        /// <summary>状态栏网格行列数（Cols×Rows）</summary>
        public const string StatusGridSize       = "status-grid-size";
        /// <summary>状态栏保存路径显示</summary>
        public const string StatusSavePath       = "status-save-path";

        // ==================== 场景编辑面板：地图/场景字段 ====================
        /// <summary>地图预制体行（容器）</summary>
        public const string MapPrefabRow         = "map-prefab-row";
        /// <summary>地图预制体选择字段</summary>
        public const string MapPrefabField       = "map-prefab-field";
        /// <summary>基线步长输入字段（相邻显示层 SortOrder 基线间距）</summary>
        public const string MapBaselineStepField = "map-baseline-step-field";
        /// <summary>摆放模式启用开关（勾选启用当前形状绘制）</summary>
        public const string PlacementEnabledToggle = "placement-enabled-toggle";
        /// <summary>绘制形状下拉字段（矩形/菱形）</summary>
        public const string DrawShapeField        = "draw-shape-field";
        /// <summary>DrawClear 启用开关（勾选启用当前区域类型绘制）</summary>
        public const string DrawClearEnabledToggle = "draw-clear-enabled-toggle";
        /// <summary>区域类型下拉字段（摆放区/禁区/通用区）</summary>
        public const string RegionTypeField       = "region-type-field";
        /// <summary>地图宽度输入字段</summary>
        public const string MapWidthField       = "map-width-field";
        /// <summary>地图高度输入字段</summary>
        public const string MapHeightField      = "map-height-field";
        /// <summary>场景名称输入字段</summary>
        public const string SceneNameField      = "scene-name-field";
        /// <summary>场景 ID 输入字段</summary>
        public const string SceneIdField        = "scene-id-field";

        // ==================== 场景编辑面板：对象属性 ====================
        /// <summary>物体编辑面板（容器）</summary>
        public const string ObjectEditPanel      = "object-edit-panel";
        /// <summary>物体编辑面板标题</summary>
        public const string ObjEditTitle         = "obj-edit-title";
        /// <summary>路径编辑 Tab</summary>
        public const string TabEditPath          = "tab-edit-path";
        /// <summary>动画编辑 Tab</summary>
        public const string TabEditAnim          = "tab-edit-anim";
        /// <summary>路径编辑内容区（容器）</summary>
        public const string PathEditContent      = "path-edit-content";
        /// <summary>动画编辑内容区（容器）</summary>
        public const string AnimEditContent      = "anim-edit-content";
        /// <summary>路径点列表（容器）</summary>
        public const string PathPointList       = "path-point-list";

        // ==================== 动画字段 ====================
        /// <summary>动画名称输入字段</summary>
        public const string AnimNameField       = "anim-name-field";
        /// <summary>动画循环开关</summary>
        public const string AnimLoopToggle      = "anim-loop-toggle";
        /// <summary>动画速度输入字段</summary>
        public const string AnimSpeedField       = "anim-speed-field";

        // ==================== 属性面板（选中物体） ====================
        /// <summary>属性面板（容器）</summary>
        public const string PropertyPanel        = "property-panel";
        /// <summary>属性面板底部常驻操作区（层级/比例/角度按钮）</summary>
        public const string PropertyActionArea   = "property-action-area";
        /// <summary>属性流内变换/交通区挂载容器（删除按钮行之前）</summary>
        public const string PropActionHost       = "prop-action-host";
        /// <summary>模板 ID 显示</summary>
        public const string PropTemplateId      = "prop-template-id";
        /// <summary>标签显示</summary>
        public const string PropTag              = "prop-tag";
        /// <summary>格子坐标显示</summary>
        public const string PropGridPos         = "prop-grid-pos";
        /// <summary>建筑模板选择</summary>
        public const string PropBuildingTemplate = "prop-building-template";
        /// <summary>格子前缀显示</summary>
        public const string PropGridPrefix      = "prop-grid-prefix";
        /// <summary>交互状态显示</summary>
        public const string PropInteractState   = "prop-interact-state";
        /// <summary>交互类型容器</summary>
        public const string PropInteractTypeContainer = "prop-interact-type-container";
        /// <summary>碰撞开关</summary>
        public const string PropCollision       = "prop-collision";
        /// <summary>对话气泡容器</summary>
        public const string PropDialogBubbleContainer = "prop-dialog-bubble-container";
        /// <summary>气泡 ID 容器</summary>
        public const string PropBubbleIdContainer = "prop-bubble-id-container";
        /// <summary>朝向容器</summary>
        public const string PropFacingContainer = "prop-facing-container";
        /// <summary>阴影开关</summary>
        public const string PropShadow           = "prop-shadow";
        /// <summary>可收集开关</summary>
        public const string PropCollectible      = "prop-collectible";
        /// <summary>可收集标记分组（标签+开关）</summary>
        public const string PropCollectibleGroup = "prop-collectible-group";
        /// <summary>交通规则开关（紧跟收集开关）</summary>
        public const string PropTrafficToggle     = "prop-traffic-toggle";
        /// <summary>交通标记分组（标签+开关）</summary>
        public const string PropTrafficGroup      = "prop-traffic-group";
        /// <summary>交通规则子面板（代码动态创建的容器名）</summary>
        public const string PropTrafficPanel      = MapToolStyleClasses.TrafficPanel;
        /// <summary>排序值输入</summary>
        public const string PropSortOrder        = "prop-sort-order";
        /// <summary>显示层（渲染档位）下拉容器</summary>
        public const string PropDisplayLayerContainer = "prop-display-layer-container";

        // ==================== 标签页 ====================
        /// <summary>主 Tab：场景编辑</summary>
        public const string TabScene             = "tab-scene";
        /// <summary>主 Tab：物品元件编辑</summary>
        public const string TabItemElement       = "tab-item-element";
        /// <summary>主 Tab：迷雾编辑</summary>
        public const string TabFog               = "tab-fog";
        /// <summary>主 Tab：资源整理</summary>
        public const string TabResource          = "tab-resource";
        /// <summary>资源整理子 Tab：全部</summary>
        public const string TabAll               = "tab-all";
        /// <summary>资源整理子 Tab：背景</summary>
        public const string TabBg               = "tab-bg";
        /// <summary>资源整理子 Tab：人物</summary>
        public const string TabChar              = "tab-char";
        /// <summary>资源整理子 Tab：猫猫</summary>
        public const string TabCat               = "tab-cat";
        /// <summary>左侧面板子 Tab：地面</summary>
        public const string TabGround           = "tab-ground";
        /// <summary>左侧面板子 Tab：建筑</summary>
        public const string TabBuilding          = "tab-building";
        /// <summary>左侧面板子 Tab：物件</summary>
        public const string TabProp              = "tab-prop";

        // ==================== 模式面板 ====================
        /// <summary>场景编辑模式面板（容器）</summary>
        public const string SceneModePanel       = "scene-mode-panel";
        /// <summary>物品元件编辑模式面板（容器）</summary>
        public const string ItemElementModePanel = "item-element-mode-panel";
        /// <summary>迷雾编辑模式面板（容器）</summary>
        public const string FogModePanel         = "fog-mode-panel";

        // ==================== 资源整理面板子元素 ====================
        /// <summary>未配置资源列表（容器）</summary>
        public const string UnconfiguredResourcesList = "unconfigured-resources-list";
        /// <summary>配置表列表（容器）</summary>
        public const string ConfigListContainer        = "config-list-container";
        /// <summary>当前配置表名称显示</summary>
        public const string CurrentConfigName          = "current-config-name";
        /// <summary>资源预览区（容器）</summary>
        public const string ResourcePreviewContainer   = "resource-preview-container";
        /// <summary>资源详情尺寸显示</summary>
        public const string ResourceDetailSize         = "resource-detail-size";
        /// <summary>资源详情名称显示</summary>
        public const string ResourceDetailName         = "resource-detail-name";
        /// <summary>资源详情类型显示</summary>
        public const string ResourceDetailType         = "resource-detail-type";
        /// <summary>资源详情路径显示</summary>
        public const string ResourceDetailPath         = "resource-detail-path";
        /// <summary>配置表编辑字段：模板ID</summary>
        public const string FieldTemplateId            = "field-template-id";
        /// <summary>配置表编辑字段：图片路径</summary>
        public const string FieldImagePath             = "field-image-path";
        /// <summary>配置表编辑字段：模板名称</summary>
        public const string FieldTemplateName          = "field-template-name";
        /// <summary>配置表编辑字段：动画名</summary>
        public const string FieldAnimName              = "field-anim-name";
        /// <summary>配置表编辑字段：循环开关</summary>
        public const string FieldLoop                  = "field-loop";
        /// <summary>配置表编辑字段：移动速度</summary>
        public const string FieldSpeed                 = "field-speed";
        /// <summary>配置表编辑字段：排序值</summary>
        public const string FieldSortOrder             = "field-sort-order";
        /// <summary>配置表编辑字段：阴影开关</summary>
        public const string FieldShadow                = "field-shadow";
        /// <summary>配置表编辑字段：可收集开关</summary>
        public const string FieldCollectible           = "field-collectible";
        /// <summary>只读预览：模板ID</summary>
        public const string PreviewTemplateId          = "preview-template-id";
        /// <summary>只读预览：图片路径</summary>
        public const string PreviewImagePath           = "preview-image-path";
        /// <summary>只读预览：模板名称</summary>
        public const string PreviewTemplateName        = "preview-template-name";
        /// <summary>只读预览：动画名</summary>
        public const string PreviewAnimName            = "preview-anim-name";
        /// <summary>只读预览：循环</summary>
        public const string PreviewLoop                = "preview-loop";
        /// <summary>只读预览：移动速度</summary>
        public const string PreviewSpeed               = "preview-speed";
        /// <summary>只读预览：排序值</summary>
        public const string PreviewSortOrder           = "preview-sort-order";
        /// <summary>只读预览：阴影</summary>
        public const string PreviewShadow              = "preview-shadow";
        /// <summary>只读预览：可收集</summary>
        public const string PreviewCollectible         = "preview-collectible";

        // ==================== 左侧面板 ====================
        /// <summary>资源整理面板（容器）</summary>
        public const string ResourcePanel       = "resource-panel";
        /// <summary>章节列表（ScrollView）</summary>
        public const string ChapterList          = "chapter-list";
        /// <summary>资源列表（ScrollView）</summary>
        public const string ResourceList         = "resource-list";

        // ==================== 物件编辑面板（item-*） ====================
        /// <summary>添加物品元件按钮</summary>
        public const string BtnAddItem           = "btn-add-item";
        /// <summary>保存全部元件按钮</summary>
        public const string BtnSaveItem          = "btn-save-item";
        /// <summary>导出配置按钮</summary>
        public const string BtnExportConfig     = "btn-export-config";
        /// <summary>导入配置按钮</summary>
        public const string BtnImportConfig    = "btn-import-config";
        /// <summary>清空迷雾按钮</summary>
        public const string BtnClearFog          = "btn-clear-fog";
        /// <summary>重置迷雾按钮</summary>
        public const string BtnResetFog         = "btn-reset-fog";
        /// <summary>元件列表（ScrollView）</summary>
        public const string ItemList             = "item-list";
        /// <summary>元件列表子 Tab：建筑</summary>
        public const string ItemTabBuilding     = "item-tab-building";
        /// <summary>元件列表子 Tab：人物</summary>
        public const string ItemTabCharacter     = "item-tab-character";
        /// <summary>元件列表子 Tab：物件</summary>
        public const string ItemTabProp          = "item-tab-prop";
        /// <summary>元件 ID 输入字段</summary>
        public const string ItemIdField          = "item-id-field";
        /// <summary>元件名称输入字段</summary>
        public const string ItemNameField       = "item-name-field";
        /// <summary>资源预制体下拉容器</summary>
        public const string ItemResNameContainer = "item-resname-container";
        /// <summary>元件图标显示</summary>
        public const string ItemIconField       = "item-icon-field";
        /// <summary>元件类型容器</summary>
        public const string ItemTypeContainer   = "item-type-container";
        /// <summary>碰撞开关</summary>
        public const string ItemCollisionsToggle = "item-collision-toggle";
        /// <summary>阴影开关</summary>
        public const string ItemShadowToggle    = "item-shadow-toggle";
        /// <summary>可收集开关</summary>
        public const string ItemCollectibleToggle = "item-collectible-toggle";
        /// <summary>可交互开关</summary>
        public const string ItemInteractableToggle = "item-interactable-toggle";
        /// <summary>可移动开关</summary>
        public const string ItemMovableToggle   = "item-movable-toggle";
        /// <summary>Y 轴排序开关</summary>
        public const string ItemYsSortToggle    = "item-ysort-toggle";
        /// <summary>Y 轴排序偏移输入字段</summary>
        public const string ItemYsSortOffsetField = "item-ysort-offset-field";
        /// <summary>移动速度输入字段</summary>
        public const string ItemSpeedField      = "item-speed-field";
        /// <summary>动画名称输入字段</summary>
        public const string ItemAnimField       = "item-anim-field";
        /// <summary>动画循环开关</summary>
        public const string ItemLoopToggle      = "item-loop-toggle";
        /// <summary>循环重启时间输入字段</summary>
        public const string ItemLoopRestartField = "item-loop-restart-field";
        /// <summary>对话气泡输入字段</summary>
        public const string ItemBubbleField     = "item-bubble-field";
        /// <summary>朝向容器</summary>
        public const string ItemFacingContainer = "item-facing-container";
        /// <summary>离屏隐藏开关</summary>
        public const string ItemHideOffmapToggle = "item-hide-offmap-toggle";
        /// <summary>重生时间输入字段</summary>
        public const string ItemRespawnField    = "item-respawn-field";
        /// <summary>传送开关</summary>
        public const string ItemTeleportToggle  = "item-teleport-toggle";
        /// <summary>传送目标场景输入字段</summary>
        public const string ItemTeleportSceneField = "item-teleport-scene-field";
        /// <summary>传送目标 X 坐标输入字段</summary>
        public const string ItemTeleportXField  = "item-teleport-x-field";
        /// <summary>传送目标 Y 坐标输入字段</summary>
        public const string ItemTeleportYField  = "item-teleport-y-field";
        /// <summary>交通类别下拉容器</summary>
        public const string ItemTrafficCategoryContainer = "item-traffic-category-container";
        /// <summary>元件属性分组容器：动画</summary>
        public const string ItemAnimContent        = "item-anim-content";
        /// <summary>元件属性分组容器：交互</summary>
        public const string ItemInteractableContent = "item-interactable-content";
        /// <summary>元件属性分组容器：层级</summary>
        public const string ItemLayerContent       = "item-layer-content";
        /// <summary>元件属性分组容器：交通</summary>
        public const string ItemTrafficContent     = "item-traffic-content";
        /// <summary>元件属性分组容器：收集物</summary>
        public const string ItemCollectibleContent = "item-collectible-content";
        /// <summary>元件属性分组容器：Y 轴排序</summary>
        public const string ItemYsSortContent      = "item-ysort-content";
        /// <summary>元件属性分组容器：移动</summary>
        public const string ItemMovableContent     = "item-movable-content";
        /// <summary>元件属性分组容器：传送门</summary>
        public const string ItemTeleportContent    = "item-teleport-content";
        /// <summary>元件气泡 ID 输入字段</summary>
        public const string ItemBubbleIdField      = "item-bubble-id-field";
        /// <summary>元件排序值输入字段</summary>
        public const string ItemSortOrderField     = "item-sort-order-field";
        /// <summary>元件移动动画名称输入字段</summary>
        public const string ItemMoveAnimField      = "item-move-anim-field";
        /// <summary>元件交互类型下拉容器</summary>
        public const string ItemInteractTypeContainer = "item-interact-type-container";
        /// <summary>元件显示层下拉容器</summary>
        public const string ItemLayerContainer     = "item-layer-container";

        // ==================== 元件属性分组折叠头（标题行 / 箭头） ====================
        /// <summary>分组折叠头：动画</summary>
        public const string ItemAnimGroupHeader        = "item-anim-group-header";
        /// <summary>分组折叠箭头：动画</summary>
        public const string ItemAnimArrow              = "item-anim-arrow";
        /// <summary>分组折叠头：交互</summary>
        public const string ItemInteractableGroupHeader = "item-interactable-group-header";
        /// <summary>分组折叠箭头：交互</summary>
        public const string ItemInteractableArrow      = "item-interactable-arrow";
        /// <summary>分组折叠头：层级</summary>
        public const string ItemLayerGroupHeader       = "item-layer-group-header";
        /// <summary>分组折叠箭头：层级</summary>
        public const string ItemLayerArrow             = "item-layer-arrow";
        /// <summary>分组折叠头：交通</summary>
        public const string ItemTrafficGroupHeader     = "item-traffic-group-header";
        /// <summary>分组折叠箭头：交通</summary>
        public const string ItemTrafficArrow           = "item-traffic-arrow";
        /// <summary>分组折叠头：收集物</summary>
        public const string ItemCollectibleGroupHeader = "item-collectible-group-header";
        /// <summary>分组折叠箭头：收集物</summary>
        public const string ItemCollectibleArrow       = "item-collectible-arrow";
        /// <summary>分组折叠头：Y 轴排序</summary>
        public const string ItemYsSortGroupHeader      = "item-ysort-group-header";
        /// <summary>分组折叠箭头：Y 轴排序</summary>
        public const string ItemYsSortArrow            = "item-ysort-arrow";
        /// <summary>分组折叠头：移动</summary>
        public const string ItemMovableGroupHeader     = "item-movable-group-header";
        /// <summary>分组折叠箭头：移动</summary>
        public const string ItemMovableArrow           = "item-movable-arrow";
        /// <summary>分组折叠头：传送门</summary>
        public const string ItemTeleportGroupHeader    = "item-teleport-group-header";
        /// <summary>分组折叠箭头：传送门</summary>
        public const string ItemTeleportArrow          = "item-teleport-arrow";

        // ==================== 场景视图 / 迷雾面板 ====================
        /// <summary>场景视图容器</summary>
        public const string SceneView              = MapToolStyleClasses.SceneView;
        /// <summary>迷雾启用开关</summary>
        public const string FogEnableToggle        = MapToolStyleClasses.FogEnableToggle;
        /// <summary>迷雾浓度滑条</summary>
        public const string FogDensitySlider       = MapToolStyleClasses.FogDensitySlider;
        /// <summary>迷雾颜色字段</summary>
        public const string FogColorField          = MapToolStyleClasses.FogColorField;

        // ==================== 工程设置窗口（路径配置项） ====================
        /// <summary>路径按钮：章节配置表</summary>
        public const string PathBtnTableChapters    = "path-btn-table-chapters";
        /// <summary>路径按钮：主关卡配置表</summary>
        public const string PathBtnTableMainLevels  = "path-btn-table-main-levels";
        /// <summary>路径按钮：物体预览图目录一</summary>
        public const string PathBtnObjectPrefabs    = "path-btn-object-prefabs";
        /// <summary>路径按钮：物体预览图目录二</summary>
        public const string PathBtnObjectTextures   = "path-btn-object-textures";
        /// <summary>路径按钮：分区背景图目录</summary>
        public const string PathBtnLevelBackground  = "path-btn-level-background";
        /// <summary>路径按钮：地图保存目录</summary>
        public const string PathBtnMapSave          = "path-btn-map-save";

        // ==================== 图集打包窗口 ====================
        /// <summary>预览区滚动容器</summary>
        public const string AtlasPreviewScroll = "preview-scroll";
        /// <summary>棋盘格背景图</summary>
        public const string AtlasBoardBackground = "board-bg";

        // ==================== UI Toolkit 内置部件 ====================
        /// <summary>TextField 内部文本输入元素（unity 内置类名，Q 查询用）</summary>
        public const string InnerTextInput = "unity-text-input";
    }
}
