/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolConstants.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器全局常量定义（配置表路径/资源路径/尺寸常量/EditorPrefs键）
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器全局常量
    /// </summary>
    public static class MapToolConstants
    {
        // ==================== EditorPrefs 键 ====================

        /// <summary>
        /// 全局模板预制体路径的 EditorPrefs 键
        /// </summary>
        public const string TemplatePrefabPrefsKey = "MapTool_TemplatePrefabPath";

        /// <summary>
        /// 地图配置窗口 UXML 资源路径
        /// </summary>
        public const string MapConfigWindowUxmlPath = "Assets/Scripts/Editor/MapTool/MapConfigWindow/MapConfigWindow.uxml";

        /// <summary>
        /// 地图配置窗口 USS 资源路径
        /// </summary>
        public const string MapConfigWindowUssPath = "Assets/Scripts/Editor/MapTool/MapConfigWindow/MapConfigWindow.uss";

        // ==================== 配置表路径（相对 Application.dataPath） ====================

        /// <summary>
        /// 章节配置表路径的 EditorPrefs 键
        /// </summary>
        public const string TableChaptersPathPrefsKey = "MapTool_Path_TableChapters";

        /// <summary>
        /// 章节配置表路径默认值（场景名 / 场景ID / 分区配置来源）
        /// </summary>
        private const string k_TableChaptersPathDefault = "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableChapters.xlsm";

        /// <summary>
        /// 章节配置表路径（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string TableChaptersPath => GetConfigPath(TableChaptersPathPrefsKey);

        /// <summary>
        /// 主关卡配置表路径的 EditorPrefs 键
        /// </summary>
        public const string TableMainLevelsPathPrefsKey = "MapTool_Path_TableMainLevels";

        /// <summary>
        /// 主关卡配置表路径默认值（分区背景图来源）
        /// </summary>
        private const string k_TableMainLevelsPathDefault = "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableMainLevels.xlsm";

        /// <summary>
        /// 主关卡配置表路径（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string TableMainLevelsPath => GetConfigPath(TableMainLevelsPathPrefsKey);

        // ==================== 资源目录（相对 Assets） ====================

        /// <summary>
        /// 物体预览图目录一的 EditorPrefs 键
        /// </summary>
        public const string ObjectPreviewPrefabsDirPrefsKey = "MapTool_Path_ObjectPreviewPrefabs";

        /// <summary>
        /// 物体预览图目录一默认值（Prefabs 下）
        /// </summary>
        private const string k_ObjectPreviewPrefabsDirDefault = "Assets/Res/Prefabs/Map/Objects";

        /// <summary>
        /// 物体预览图目录一（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string ObjectPreviewPrefabsDir => GetConfigPath(ObjectPreviewPrefabsDirPrefsKey);

        /// <summary>
        /// 物体预览图目录二的 EditorPrefs 键
        /// </summary>
        public const string ObjectPreviewTexturesDirPrefsKey = "MapTool_Path_ObjectPreviewTextures";

        /// <summary>
        /// 物体预览图目录二默认值（Textures 下）
        /// </summary>
        private const string k_ObjectPreviewTexturesDirDefault = "Assets/Res/Textures/Map/Objects";

        /// <summary>
        /// 物体预览图目录二（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string ObjectPreviewTexturesDir => GetConfigPath(ObjectPreviewTexturesDirPrefsKey);

        /// <summary>
        /// 分区背景图目录的 EditorPrefs 键
        /// </summary>
        public const string LevelBackgroundDirPrefsKey = "MapTool_Path_LevelBackground";

        /// <summary>
        /// 分区背景图目录默认值（拼接 {bgName}.png）
        /// </summary>
        private const string k_LevelBackgroundDirDefault = "Assets/Res/Textures/Map/LevelBackground";

        /// <summary>
        /// 分区背景图目录（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string LevelBackgroundDir => GetConfigPath(LevelBackgroundDirPrefsKey);

        /// <summary>
        /// 章节地图保存目录的 EditorPrefs 键
        /// </summary>
        public const string MapSaveDirPrefsKey = "MapTool_Path_MapSaveDir";

        /// <summary>
        /// 章节地图保存目录默认值（拼接 Map_{chapterId}.json / .prefab）
        /// </summary>
        private const string k_MapSaveDirDefault = "Assets/GameData/Maps";

        /// <summary>
        /// 章节地图保存目录（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string MapSaveDir => GetConfigPath(MapSaveDirPrefsKey);

        // ---TODO 双格式保存：JSON 新保存目录（预制体路径仍走 MapSaveDir，严禁改其默认值）
        /// <summary>
        /// 章节地图 JSON 保存目录（Map_{chapterId}.json 落此目录；Lua 同源导出）
        /// </summary>
        public const string MapJsonSaveDir = "Assets/GameData/MapDatas";

        /// <summary>
        /// 地图保存目录的父目录（用于按需创建目录结构）
        /// </summary>
        public const string GameDataDir = "Assets/GameData";

        // ==================== 资源整理模块常量 ====================

        /// <summary>
        /// 资源整理：未配置资源扫描目录（相对 Assets）
        /// </summary>
        public const string ResourceScanDir = "Assets/Res/Textures/Map";

        /// <summary>
        /// 地图纹理根目录（相对 Assets）：PSD 切片按 全局共享 / 章节共享 / 关卡专属 三级存放
        /// </summary>
        public const string MapTextureRoot = "Assets/Res/Textures/Map";

        /// <summary>
        /// 资源整理：配置表根目录（相对项目根目录）
        /// </summary>
        public const string ConfigTableRootDir = "Docs/Designs/Excels/Tables/AlTables/Item";

        // ==================== 美术命名前缀 ====================

        /// <summary>背景图命名前缀</summary>
        public const string PrefixBg = "bg_";
        /// <summary>绿化层命名前缀：草地命名前缀</summary>
        public const string PrefixGrass = "grass_";
        /// <summary>绿化层命名前缀：绿植/花草命名前缀</summary>
        public const string PrefixPlant = "plant_";
        /// <summary>道路市政层命名前缀：道路命名前缀</summary>
        public const string PrefixRoad = "road_";
        /// <summary>建筑命名前缀</summary>
        public const string PrefixBuilding = "building_";
        /// <summary>人物/载具命名前缀</summary>
        public const string PrefixCharacter = "char_";
        /// <summary>物件命名前缀</summary>
        public const string PrefixProp = "prop_";
        /// <summary>猫猫命名前缀（猫并入角色/载具动态层，前缀保留兼容）</summary>
        public const string PrefixCat = "cat_";
        /// <summary>前景近景/特效命名前缀</summary>
        public const string PrefixEffect = "eff_";
        /// <summary>前景近景层命名前缀：前景装饰命名前缀</summary>
        public const string PrefixForeground = "fg_";
        /// <summary>气泡命名前缀</summary>
        public const string PrefixBubble = "bubble_";

        // ==================== 筛选类型枚举字符串 ====================

        /// <summary>筛选类型：全部</summary>
        public const string FilterAll = "all";
        /// <summary>筛选类型：背景</summary>
        public const string FilterBg = "bg";
        /// <summary>筛选类型：建筑</summary>
        public const string FilterBuilding = "building";
        /// <summary>筛选类型：人物</summary>
        public const string FilterCharacter = "char";
        /// <summary>筛选类型：物件</summary>
        public const string FilterProp = "prop";
        /// <summary>筛选类型：猫猫</summary>
        public const string FilterCat = "cat";

        // ==================== 配置表文件名 ====================

        /// <summary>背景配置表文件名</summary>
        public const string TableBgFileName = "TableBackgrounds.xlsm";
        /// <summary>建筑配置表文件名</summary>
        public const string TableBuildingFileName = "TableBuildings.xlsm";
        /// <summary>人物配置表文件名</summary>
        public const string TableCharacterFileName = "TableCharacters.xlsm";
        /// <summary>物件配置表文件名</summary>
        public const string TablePropFileName = "TableProps.xlsm";
        /// <summary>猫猫配置表文件名</summary>
        public const string TableCatFileName = "TableCats.xlsm";

        // ==================== 地图尺寸 ====================

        /// <summary>
        /// 单关卡像素尺寸（与 ZoneItem 预制体一致，4096x4096）
        /// </summary>
        public const int LevelPixelSize = 4096;

        /// <summary>
        /// 摆放 / 路径网格的格子尺寸（像素）
        /// </summary>
        public const int GridSize = 32;

        /// <summary>
        /// 最大缩放倍率
        /// </summary>
        public const float MaxZoom = 5f;

        /// <summary>
        /// 最小缩放倍率（LoadMapConfig 自动定位视口时的下限）
        /// </summary>
        public const float MinZoom = 0.1f;

        /// <summary>
        /// 自动定位视口时的目标缩放上限（避免过度放大）
        /// </summary>
        public const float AutoFitMaxZoom = 4f;

        /// <summary>
        /// 地图边缘拖拽留白（像素）
        /// </summary>
        public const float DragEdgeMargin = 100f;

        /// <summary>
        /// 判断点击 / 拖动的移动阈值（像素）
        /// </summary>
        public const float DragThreshold = 4f;

        /// <summary>
        /// 格子二维坐标编码乘数（gy * CellEncodeMultiplier + gx），用于 occupiedCells 去重
        /// 需大于单关最大格子数（LevelPixelSize / GridSize = 4096/32 = 128）
        /// </summary>
        public const long CellEncodeMultiplier = 100000L;

        /// <summary>
        /// 临时提示消息显示时长（秒）
        /// </summary>
        public const float TempMessageDuration = 1.5f;

        /// <summary>
        /// 物体占位色块透明度（未加载预览图时使用）
        /// </summary>
        public const float PlaceholderAlpha = 0.5f;

        /// <summary>
        /// 基线步长配置项的 EditorPrefs 键（相邻显示层之间的基线 SortOrder 步长，可在地图配置弹窗修改）
        /// </summary>
        public const string LayerBaselineStepPrefsKey = "MapTool_LayerBaselineStep";

        /// <summary>
        /// 绘制形状配置项的 EditorPrefs 键（DrawShape code，可在场景编辑面板底部属性区切换）
        /// </summary>
        public const string DrawShapePrefsKey = "MapTool_DrawShape";

        /// <summary>绘制形状默认值：菱形绘制</summary>
        public const string DrawShapeDefault = "diamond";

        /// <summary>
        /// 区域类型配置项的 EditorPrefs 键（RegionType code，可在场景编辑面板底部属性区切换）
        /// </summary>
        public const string RegionTypePrefsKey = "MapTool_RegionType";

        /// <summary>区域类型默认值：摆放区</summary>
        public const string RegionTypeDefault = "place";

        /// <summary>
        /// 摆放模式启用开关的 EditorPrefs 键（勾选后启用当前形状绘制）
        /// </summary>
        public const string PlacementEnabledPrefsKey = "MapTool_PlacementEnabled";

        /// <summary>摆放模式启用开关默认值：勾选</summary>
        public const bool PlacementEnabledDefault = true;

        /// <summary>
        /// DrawClear 启用开关的 EditorPrefs 键（勾选后启用当前区域类型绘制）
        /// </summary>
        public const string DrawClearEnabledPrefsKey = "MapTool_DrawClearEnabled";

        /// <summary>DrawClear 启用开关默认值：勾选</summary>
        public const bool DrawClearEnabledDefault = true;

        /// <summary>
        /// 基线步长默认值：八层基线按 0 / 10 / 20 / 30 / 40 / 50 / 60 / 70 等差拉开
        /// </summary>
        public const int LayerBaselineStepDefault = 10;

        /// <summary>
        /// 基线步长允许下限（必须大于层内槽位容量，层内槽位才不会跨入相邻层基线）
        /// </summary>
        public const int LayerBaselineStepMin = 10;

        /// <summary>
        /// 相邻显示层之间的基线 SortOrder 步长：八层基线按 0 / Step / 2·Step / … 等差拉开，
        /// 层间预留 LayerSlotCount 个层内槽位；取值来自配置弹窗（EditorPrefs 持久化），非法值回退默认并钳到下限以上
        /// </summary>
        public static int LayerBaselineStep => GetConfigInt(LayerBaselineStepPrefsKey, LayerBaselineStepDefault, LayerBaselineStepMin);

        /// <summary>
        /// 当前绘制形状 code（DrawShape.Rect/Diamond）：取值来自场景编辑面板（EditorPrefs 持久化），非法/空值回退默认菱形
        /// </summary>
        public static string DrawShape => GetConfigString(DrawShapePrefsKey, DrawShapeDefault);

        /// <summary>
        /// 当前区域类型 code（RegionType.Place/Forbid/Common）：取值来自场景编辑面板（EditorPrefs 持久化），非法/空值回退默认摆放区
        /// </summary>
        public static string RegionType => GetConfigString(RegionTypePrefsKey, RegionTypeDefault);

        /// <summary>
        /// 摆放模式启用开关：取值来自场景编辑面板（EditorPrefs 持久化），缺失回退默认勾选
        /// </summary>
        public static bool PlacementEnabled => GetConfigBool(PlacementEnabledPrefsKey, PlacementEnabledDefault);

        /// <summary>
        /// DrawClear 启用开关：取值来自场景编辑面板（EditorPrefs 持久化），缺失回退默认勾选
        /// </summary>
        public static bool DrawClearEnabled => GetConfigBool(DrawClearEnabledPrefsKey, DrawClearEnabledDefault);

        /// <summary>
        /// 同层内手动层级（SortOrder）相邻档位步长：置顶/置底与相邻交换时使用
        /// </summary>
        public const int LayerOrderStep = 1;

        /// <summary>
        /// 同层内次序重建等差步长：按排序序列回写 SortOrder 时，相邻物件相差的档位；
        /// 基线按每层 LayerBaselineStep 拉开，层内步长取 1；超出档位容量的物件共享最高档、以脚底 Y 排序，不越界到相邻层
        /// </summary>
        public const int LayerOrderSeqStep = 1;

        /// <summary>
        /// 每层可分配的独立排序档位数量：层内 SortOrder 落在 基线+0 ~ 基线+(LayerSlotCount-1)，
        /// 必须小于 LayerBaselineStep，确保层内槽位不跨入相邻层基线；超出部分钳到最高档同档共享
        /// </summary>
        public const int LayerSlotCount = 10;

        /// <summary>角度单次步进（度，绕 Z 轴）</summary>
        public const float RotationStep = 15f;

        /// <summary>比例单次步进倍率（在当前 Scale 上叠加）</summary>
        public const float ScaleStep = 0.1f;

        /// <summary>缩放倍率下限</summary>
        public const float ScaleMin = 0.2f;

        /// <summary>缩放倍率上限</summary>
        public const float ScaleMax = 3f;

        // ==================== 物体默认尺寸（格子数） ====================

        /// <summary>
        /// 建筑默认尺寸（宽 x 高）
        /// </summary>
        public static readonly Vector2Int BuildingDefaultSize = new Vector2Int(4, 4);

        /// <summary>
        /// 人物默认尺寸（宽 x 高）
        /// </summary>
        public static readonly Vector2Int CharacterDefaultSize = new Vector2Int(1, 2);

        /// <summary>
        /// 物件默认尺寸（宽 x 高）
        /// </summary>
        public static readonly Vector2Int PropDefaultSize = new Vector2Int(1, 1);

        // ==================== 预制体路径 ====================

        /// <summary>
        /// 场景根节点预制体路径
        /// </summary>
        public const string SceneRootPrefabPath = "Assets/Res/Prefabs/SceneRoot/2DSceneRoot.prefab";

        /// <summary>
        /// 区域项（ZoneItem）预制体路径
        /// </summary>
        public const string ZoneItemPrefabPath = "Assets/Res/Prefabs/Map/ZoneItem.prefab";

        // ==================== UI 尺寸常量 ====================

        /// <summary>
        /// 左侧面板物件列表图标尺寸（像素）
        /// </summary>
        public const int LeftPanelIconSize = 32;

        /// <summary>
        /// 中间元件列表图标尺寸（像素）
        /// </summary>
        public const int ItemElementIconSize = 52;

        /// <summary>
        /// 列表项默认右边距（像素）
        /// </summary>
        public const int ListItemMarginRight = 8;

        /// <summary>
        /// 地图预览窗口默认缩放比例
        /// </summary>
        public const float PreviewDefaultZoom = 0.25f;

        /// <summary>
        /// 预览窗口中物体移动速度（像素/秒）
        /// </summary>
        public const float PreviewMoveSpeed = 200f;

        // ==================== 动画相关常量 ====================

        /// <summary>
        /// 默认动画名称
        /// </summary>
        public const string DefaultAnimName = "Idle";

        /// <summary>
        /// 无动画标记（表示不播放动画）
        /// </summary>
        public const string NoAnimName = "none";

        // ==================== 文件扩展名 ====================

        /// <summary>
        /// Excel 配置表文件扩展名（Office Open XML 格式）
        /// </summary>
        public const string ExtExcel = ".xlsm";

        /// <summary>
        /// JSON 文件扩展名
        /// </summary>
        public const string ExtJson = ".json";

        /// <summary>
        /// PNG 图片文件扩展名
        /// </summary>
        public const string ExtPng = ".png";

        /// <summary>
        /// Unity 预制体文件扩展名
        /// </summary>
        public const string ExtPrefab = ".prefab";

        // ==================== 可配置路径读写 ====================

        /// <summary>
        /// 读取可配置路径：优先返回 EditorPrefs 中保存的值，未配置时回退默认值
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        public static string GetConfigPath(string prefsKey)
        {
            return GetConfigPath(prefsKey, GetConfigPathDefault(prefsKey));
        }

        /// <summary>
        /// 读取可配置路径：优先返回 EditorPrefs 中保存的值，未配置时回退指定默认值
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="defaultValue">默认路径</param>
        public static string GetConfigPath(string prefsKey, string defaultValue)
        {
            string saved = EditorPrefs.GetString(prefsKey, "");
            return string.IsNullOrEmpty(saved) ? defaultValue : saved;
        }

        /// <summary>
        /// 保存可配置路径到 EditorPrefs
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="value">要保存的路径值</param>
        public static void SetConfigPath(string prefsKey, string value)
        {
            EditorPrefs.SetString(prefsKey, value);
        }

        /// <summary>
        /// 读取字符串型配置（EditorPrefs 持久化）；空值回退默认值
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>合法字符串值</returns>
        public static string GetConfigString(string prefsKey, string defaultValue)
        {
            string saved = EditorPrefs.GetString(prefsKey, string.Empty);
            return string.IsNullOrEmpty(saved) ? defaultValue : saved;
        }

        /// <summary>
        /// 保存字符串型配置到 EditorPrefs（空值不写入，保留已存配置）
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="value">要保存的字符串值</param>
        public static void SetConfigString(string prefsKey, string value)
        {
            EditorPrefs.SetString(prefsKey, value ?? string.Empty);
        }

        /// <summary>
        /// 读取布尔型配置（EditorPrefs 持久化）；缺失回退默认值
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>布尔值</returns>
        public static bool GetConfigBool(string prefsKey, bool defaultValue)
        {
            return EditorPrefs.GetBool(prefsKey, defaultValue);
        }

        /// <summary>
        /// 保存布尔型配置到 EditorPrefs
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="value">要保存的布尔值</param>
        public static void SetConfigBool(string prefsKey, bool value)
        {
            EditorPrefs.SetBool(prefsKey, value);
        }

        /// <summary>
        /// 读取整数型配置（EditorPrefs 持久化）；解析失败或低于下限时回退默认值并钳到下限
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="minValue">允许下限</param>
        /// <returns>合法整数值</returns>
        public static int GetConfigInt(string prefsKey, int defaultValue, int minValue)
        {
            int saved = EditorPrefs.GetInt(prefsKey, defaultValue);
            return saved < minValue ? Mathf.Max(defaultValue, minValue) : saved;
        }

        /// <summary>
        /// 保存整数型配置到 EditorPrefs（写入前钳到下限）
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="value">要保存的整数值</param>
        /// <param name="minValue">允许下限</param>
        public static void SetConfigInt(string prefsKey, int value, int minValue)
        {
            EditorPrefs.SetInt(prefsKey, Mathf.Max(value, minValue));
        }

        /// <summary>
        /// 获取路径配置的默认值（按 EditorPrefs 键映射）
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        public static string GetConfigPathDefault(string prefsKey)
        {
            if (prefsKey == TableChaptersPathPrefsKey) return k_TableChaptersPathDefault;
            if (prefsKey == TableMainLevelsPathPrefsKey) return k_TableMainLevelsPathDefault;
            if (prefsKey == ObjectPreviewPrefabsDirPrefsKey) return k_ObjectPreviewPrefabsDirDefault;
            if (prefsKey == ObjectPreviewTexturesDirPrefsKey) return k_ObjectPreviewTexturesDirDefault;
            if (prefsKey == LevelBackgroundDirPrefsKey) return k_LevelBackgroundDirDefault;
            if (prefsKey == MapSaveDirPrefsKey) return k_MapSaveDirDefault;
            return "";
        }
    }
}