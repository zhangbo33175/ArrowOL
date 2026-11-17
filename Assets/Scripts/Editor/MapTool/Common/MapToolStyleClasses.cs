/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolStyleClasses.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 USS 样式类名集中定义
 *            消除 AddToClassList("...") / RemoveFromClassList("...") 中的字面量
 ***************************************************************/

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器 USS 样式类名常量
    /// 按归属分组：通用状态 / 属性面板 / 路径编辑 / 精灵列表 / 交通面板 / 迷雾面板 / 元件列表
    /// </summary>
    public static class MapToolStyleClasses
    {
        // ==================== 通用状态 ====================
        /// <summary>激活态（Tab / 子 Tab 高亮）</summary>
        public const string Active      = "active";
        /// <summary>选中态（章节 / 关卡 / 资源行）</summary>
        public const string Selected    = "selected";
        /// <summary>次级按钮 / 次级文字</summary>
        public const string Secondary   = "secondary";

        // ==================== 场景视图 ====================
        /// <summary>场景视图容器</summary>
        public const string SceneView   = "scene-view";

        // ==================== 属性面板操作行 ====================
        /// <summary>属性操作行</summary>
        public const string PropActionRow    = "prop-action-row";
        /// <summary>属性操作行标签</summary>
        public const string PropActionLabel  = "prop-action-label";
        /// <summary>属性操作行按钮</summary>
        public const string PropActionBtn    = "prop-action-btn";
        /// <summary>属性操作行取值显示</summary>
        public const string PropActionValue  = "prop-action-value";
        /// <summary>收集物操作行</summary>
        public const string CollectibleAction = "collectible-action";

        // ==================== 路径点 ====================
        /// <summary>路径点行</summary>
        public const string PathPointItem    = "path-point-item";
        /// <summary>路径点序号</summary>
        public const string PathPointIndex   = "path-point-index";
        /// <summary>路径点坐标</summary>
        public const string PathPointPos     = "path-point-pos";
        /// <summary>路径点删除按钮</summary>
        public const string PathPointDel     = "path-point-del";

        // ==================== 精灵/资源列表 ====================
        /// <summary>精灵行</summary>
        public const string SpriteItem       = "sprite-item";
        /// <summary>精灵行图标</summary>
        public const string SpriteItemImage  = "sprite-item-image";
        /// <summary>精灵行信息区</summary>
        public const string SpriteItemInfo   = "sprite-item-info";
        /// <summary>精灵行名称</summary>
        public const string SpriteItemName   = "sprite-item-name";
        /// <summary>精灵行尺寸</summary>
        public const string SpriteItemSize   = "sprite-item-size";
        /// <summary>精灵名称标签</summary>
        public const string SpriteName       = "sprite-name";
        /// <summary>精灵尺寸标签</summary>
        public const string SpriteSize       = "sprite-size";

        // ==================== 交通面板 ====================
        /// <summary>交通面板容器</summary>
        public const string TrafficPanel          = "traffic-panel";
        /// <summary>交通字段行</summary>
        public const string TrafficFieldRow       = "traffic-field-row";
        /// <summary>交通字段标签</summary>
        public const string TrafficFieldLabel     = "traffic-field-label";
        /// <summary>交通输入字段</summary>
        public const string TrafficInput          = "traffic-input";
        /// <summary>交通开关行</summary>
        public const string TrafficToggleRow      = "traffic-toggle-row";
        /// <summary>交通开关标签</summary>
        public const string TrafficToggleLabel    = "traffic-toggle-label";
        /// <summary>交通开关</summary>
        public const string TrafficToggle         = "traffic-toggle";
        /// <summary>交通工具按钮行</summary>
        public const string TrafficToolRow        = "traffic-tool-row";
        /// <summary>交通工具按钮</summary>
        public const string TrafficToolBtn        = "traffic-tool-btn";
        /// <summary>交通全选按钮</summary>
        public const string TrafficSelectAll      = "traffic-select-all";
        /// <summary>交通灯绑定列表</summary>
        public const string TrafficLightBindList  = "traffic-light-bind-list";
        /// <summary>交通灯项</summary>
        public const string TrafficLightItem      = "traffic-light-item";
        /// <summary>交通空态提示</summary>
        public const string TrafficItemEmpty      = "traffic-item-empty";

        // ==================== 迷雾面板 ====================
        /// <summary>迷雾开关</summary>
        public const string FogEnableToggle    = "fog-enable-toggle";
        /// <summary>迷雾浓度滑条</summary>
        public const string FogDensitySlider   = "fog-density-slider";
        /// <summary>迷雾颜色字段</summary>
        public const string FogColorField      = "fog-color-field";

        // ==================== 物品元件列表 ====================
        /// <summary>元件列表行</summary>
        public const string ItemElementItem  = "item-element-item";
        /// <summary>元件行图标</summary>
        public const string ItemIcon         = "item-icon";
        /// <summary>元件行名称</summary>
        public const string ItemName         = "item-name";
        /// <summary>元件行说明</summary>
        public const string ItemDesc         = "item-desc";
        /// <summary>元件行显隐开关</summary>
        public const string ItemShowToggle   = "item-show-toggle";
        /// <summary>元件行文本区</summary>
        public const string ItemTextArea     = "item-text-area";
    }
}
