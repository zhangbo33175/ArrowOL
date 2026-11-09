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
        /// <summary>摆放网格图层开关</summary>
        public const string LayerPlaceGrid       = "layer-place-grid";
        /// <summary>路径网格图层开关</summary>
        public const string LayerPathGrid        = "layer-path-grid";
        /// <summary>地图图层开关</summary>
        public const string LayerMap             = "layer-map";
        /// <summary>建筑图层开关</summary>
        public const string LayerBuilding        = "layer-building";
        /// <summary>人物图层开关</summary>
        public const string LayerCharacter       = "layer-character";
        /// <summary>物件图层开关</summary>
        public const string LayerObject          = "layer-object";
        /// <summary>猫猫图层开关</summary>
        public const string LayerCat             = "layer-cat";
        /// <summary>背景图层开关</summary>
        public const string LayerBackground      = "layer-background";
        /// <summary>包围盒图层开关</summary>
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
        /// <summary>状态栏格子总数</summary>
        public const string StatusTileCount      = "status-tile-count";
        /// <summary>状态栏保存路径显示</summary>
        public const string StatusSavePath       = "status-save-path";

        // ==================== 场景编辑面板：地图/场景字段 ====================
        /// <summary>地图预制体行（容器）</summary>
        public const string MapPrefabRow         = "map-prefab-row";
        /// <summary>地图预制体选择字段</summary>
        public const string MapPrefabField       = "map-prefab-field";
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
        /// <summary>排序值输入</summary>
        public const string PropSortOrder        = "prop-sort-order";

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
        /// <summary>资源名称输入字段</summary>
        public const string ItemResNameField    = "item-resname-field";
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
    }
}
