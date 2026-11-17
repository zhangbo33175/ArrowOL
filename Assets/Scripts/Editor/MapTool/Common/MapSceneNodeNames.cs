/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapSceneNodeNames.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景层级节点名集中定义（消除场景同步中的 GameObject.Find / 节点名硬编码）
 *            预制体结构改名时只需改本文件，编译器会报错同步所有引用
 ***************************************************************/

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景层级节点名常量：2DSceneRoot 预制体结构 + 编辑器生成的物件节点命名前缀
    /// </summary>
    public static class MapSceneNodeNames
    {
        // ==================== 场景根结构 ====================
        /// <summary>场景根节点实例名（2DSceneRoot 预制体实例）</summary>
        public const string SceneRoot = "2DSceneRoot";
        /// <summary>场景启动器父节点名（GameApp/GameMain/Launcher）</summary>
        public const string Launcher = "Launcher";
        /// <summary>场景根下分层容器节点名</summary>
        public const string SceneLayers = "2DSceneLayers";
        /// <summary>场景启动器节点名（地图预制体挂载父级）</summary>
        public const string SceneLauncher = "SceneLauncher";

        // ==================== 地图实例结构 ====================
        /// <summary>地图预制体实例名</summary>
        public const string MapInstance = "GameMainMap";
        /// <summary>分区根节点名（ZoneItem 挂载父级）</summary>
        public const string ZoneRoot = "ZoneRoot";
        /// <summary>坐标适配器节点名（层节点挂载父级，左上原点坐标系）</summary>
        public const string CoordAdapter = "CoordAdapter";
        /// <summary>背景层节点名</summary>
        public const string BackgroundNode = "Background";
        /// <summary>背景图节点名（Background 下铺满父级的 Image 节点）</summary>
        public const string BackgroundImage = "BG";
        /// <summary>物件路径点挂载节点名</summary>
        public const string Root = "Root";

        // ==================== 运行时生成节点命名前缀 ====================
        /// <summary>分区项节点名前缀（兼容旧命名，清理时按前缀匹配）</summary>
        public const string ZoneItemPrefix = "ZoneItem";
        /// <summary>分区项实例节点名前缀（按关卡ID生成）</summary>
        public const string ZoneItemInstancePrefix = "ZoneItem_";
        /// <summary>静态物件节点名前缀</summary>
        public const string ObjectPrefix = "Obj_";
        /// <summary>角色/载具节点名前缀</summary>
        public const string CharacterPrefix = "Char_";
        /// <summary>路径点节点名前缀</summary>
        public const string PathPointPrefix = "Path_";
        /// <summary>路径连线节点名前缀</summary>
        public const string PathLinePrefix = "PathLine_";
    }
}
