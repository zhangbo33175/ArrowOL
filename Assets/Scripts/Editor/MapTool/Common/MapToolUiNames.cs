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
        public const string BtnSave              = "btn-save";
        public const string BtnUpdateMap         = "btn-update-map";
        public const string BtnExportSlice       = "btn-export-slice";
        public const string BtnExportSettings     = "btn-export-settings";
        public const string BtnPreviewMap        = "btn-preview-map";
        public const string BtnClearObjects      = "btn-clear-objects";
        public const string BtnClearGridData     = "btn-clear-grid-data";
        public const string BtnDeleteObject      = "btn-delete-object";
        public const string BtnPublish           = "btn-publish";
        public const string BtnConfig            = "btn-config";
        public const string BtnDelete            = "btn-delete";
        public const string BtnRefresh           = "btn-refresh";
        public const string BtnConfirm          = "btn-confirm";
        public const string BtnCancel            = "btn-cancel";

        // ==================== 路径编辑按钮 ====================
        public const string BtnPathDraw          = "btn-path-draw";
        public const string BtnPathUndo         = "btn-path-undo";
        public const string BtnPathClear         = "btn-path-clear";
        public const string BtnClearTarget       = "btn-clear-target";

        // ==================== 图层开关 ====================
        public const string LayerPlaceGrid       = "layer-place-grid";
        public const string LayerPathGrid        = "layer-path-grid";
        public const string LayerMap             = "layer-map";
        public const string LayerBuilding        = "layer-building";
        public const string LayerCharacter       = "layer-character";
        public const string LayerObject          = "layer-object";
        public const string LayerBackground      = "layer-background";
        public const string LayerBoundingBox     = "layer-bounding-box";

        // ==================== 状态栏 ====================
        public const string StatusZoom           = "status-zoom";
        public const string StatusMouseX         = "status-mouse-x";
        public const string StatusMouseY         = "status-mouse-y";
        public const string StatusTileX          = "status-tile-x";
        public const string StatusTileY          = "status-tile-y";
        public const string StatusTileCount      = "status-tile-count";
        public const string StatusSavePath       = "status-save-path";

        // ==================== 场景编辑面板：地图/场景字段 ====================
        public const string MapPrefabRow         = "map-prefab-row";
        public const string MapPrefabField       = "map-prefab-field";
        public const string MapWidthField       = "map-width-field";
        public const string MapHeightField      = "map-height-field";
        public const string SceneNameField      = "scene-name-field";
        public const string SceneIdField        = "scene-id-field";

        // ==================== 场景编辑面板：对象属性 ====================
        public const string ObjectEditPanel      = "object-edit-panel";
        public const string ObjEditTitle         = "obj-edit-title";
        public const string TabEditPath          = "tab-edit-path";
        public const string TabEditAnim          = "tab-edit-anim";
        public const string PathEditContent      = "path-edit-content";
        public const string AnimEditContent      = "anim-edit-content";
        public const string PathPointList       = "path-point-list";

        // ==================== 动画字段 ====================
        public const string AnimNameField       = "anim-name-field";
        public const string AnimLoopToggle      = "anim-loop-toggle";
        public const string AnimSpeedField       = "anim-speed-field";

        // ==================== 属性面板（选中物体） ====================
        public const string PropertyPanel        = "property-panel";
        public const string PropTemplateId      = "prop-template-id";
        public const string PropTag              = "prop-tag";
        public const string PropGridPos         = "prop-grid-pos";
        public const string PropBuildingTemplate = "prop-building-template";
        public const string PropGridPrefix      = "prop-grid-prefix";
        public const string PropInteractState   = "prop-interact-state";
        public const string PropInteractTypeContainer = "prop-interact-type-container";
        public const string PropCollision       = "prop-collision";
        public const string PropDialogBubbleContainer = "prop-dialog-bubble-container";
        public const string PropBubbleIdContainer = "prop-bubble-id-container";
        public const string PropFacingContainer = "prop-facing-container";
        public const string PropShadow           = "prop-shadow";
        public const string PropCollectible      = "prop-collectible";
        public const string PropSortOrder        = "prop-sort-order";

        // ==================== 标签页 ====================
        public const string TabScene             = "tab-scene";
        public const string TabItemElement       = "tab-item-element";
        public const string TabFog               = "tab-fog";
        public const string TabGround           = "tab-ground";
        public const string TabBuilding          = "tab-building";
        public const string TabProp              = "tab-prop";

        // ==================== 模式面板 ====================
        public const string SceneModePanel       = "scene-mode-panel";
        public const string ItemElementModePanel = "item-element-mode-panel";
        public const string FogModePanel         = "fog-mode-panel";

        // ==================== 左侧面板 ====================
        public const string ChapterList          = "chapter-list";
        public const string ResourceList         = "resource-list";

        // ==================== 物件编辑面板（item-*） ====================
        public const string BtnAddItem           = "btn-add-item";
        public const string BtnExportConfig     = "btn-export-config";
        public const string BtnImportConfig    = "btn-import-config";
        public const string BtnClearFog          = "btn-clear-fog";
        public const string BtnResetFog         = "btn-reset-fog";
        public const string ItemList             = "item-list";
        public const string ItemTabBuilding     = "item-tab-building";
        public const string ItemTabCharacter     = "item-tab-character";
        public const string ItemTabProp          = "item-tab-prop";
        public const string ItemIdField          = "item-id-field";
        public const string ItemNameField       = "item-name-field";
        public const string ItemResNameField    = "item-resname-field";
        public const string ItemIconField       = "item-icon-field";
        public const string ItemTypeContainer   = "item-type-container";
        public const string ItemCollisionsToggle = "item-collision-toggle";
        public const string ItemShadowToggle    = "item-shadow-toggle";
        public const string ItemCollectibleToggle = "item-collectible-toggle";
        public const string ItemInteractableToggle = "item-interactable-toggle";
        public const string ItemMovableToggle   = "item-movable-toggle";
        public const string ItemYsSortToggle    = "item-ysort-toggle";
        public const string ItemYsSortOffsetField = "item-ysort-offset-field";
        public const string ItemSpeedField      = "item-speed-field";
        public const string ItemAnimField       = "item-anim-field";
        public const string ItemLoopToggle      = "item-loop-toggle";
        public const string ItemLoopRestartField = "item-loop-restart-field";
        public const string ItemBubbleField     = "item-bubble-field";
        public const string ItemFacingContainer = "item-facing-container";
        public const string ItemHideOffmapToggle = "item-hide-offmap-toggle";
        public const string ItemRespawnField    = "item-respawn-field";
        public const string ItemTeleportToggle  = "item-teleport-toggle";
        public const string ItemTeleportSceneField = "item-teleport-scene-field";
        public const string ItemTeleportXField  = "item-teleport-x-field";
        public const string ItemTeleportYField  = "item-teleport-y-field";
    }
}
