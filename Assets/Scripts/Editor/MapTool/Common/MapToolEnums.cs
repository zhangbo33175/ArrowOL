/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolEnums.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器枚举与编码转换集中定义
 *            物件类型枚举 + 交通类别/交互类型/朝向/绘制形状/区域类型的显示名与代码互转
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器枚举与编码转换
    /// 拆出自 MapToolModels：数据模型（ObjectTemplate/MapConfigData 等）与类型编码不再混居同一文件
    /// </summary>
    #region 枚举与编码转换

    /// <summary>
    /// 地图物体类型枚举
    /// </summary>
    public enum MapObjectType
    {
        /// <summary>建筑类元素</summary>
        Building = 0,

        /// <summary>人物/NPC类元素</summary>
        Character = 1,

        /// <summary>场景物件类元素</summary>
        Prop = 2,

        /// <summary>猫猫类元素（主角/猫咪）</summary>
        Cat = 3
    }

    /// <summary>
    /// 地图物体类型显示名口径（列表/日志/筛选/下拉共用）
    /// 统一入口，避免各处重复写「建筑/人物/物件/猫猫」switch 或三元表达式
    /// </summary>
    public static class ObjectTypeNames
    {
        /// <summary>背景（背景图资源，非 MapObjectType 值）</summary>
        public const string Background = "背景";

        /// <summary>建筑</summary>
        public const string Building = "建筑";

        /// <summary>人物</summary>
        public const string Character = "人物";

        /// <summary>物件</summary>
        public const string Prop = "物件";

        /// <summary>猫猫</summary>
        public const string Cat = "猫猫";

        /// <summary>无法识别</summary>
        public const string Unknown = "未知";

        /// <summary>类型下拉默认展示项（元件编辑用，不含猫猫）</summary>
        public static readonly string[] EditableTypeChoices = { Building, Character, Prop };

        /// <summary>
        /// 物体类型 → 中文显示名
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>中文显示名</returns>
        public static string Of(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return Building;
                case MapObjectType.Character: return Character;
                case MapObjectType.Cat: return Cat;
                default: return Prop;
            }
        }
    }

    /// <summary>
    /// 模板（元件）交通类别目录：在物品元件编辑中给模板打交通分类标签，
    /// 放置实例时按类别继承默认交通角色/载具标记，实例仍可在右栏单独调整。
    /// </summary>
    public static class TemplateTrafficCategory
    {
        /// <summary>不参与交通规则（默认）</summary>
        public const string None = "none";

        /// <summary>路灯（静态道路设施）</summary>
        public const string Streetlight = "streetlight";

        /// <summary>汽车 / 载具</summary>
        public const string Vehicle = "vehicle";

        /// <summary>人物 / 行人</summary>
        public const string Pedestrian = "pedestrian";

        /// <summary>马路路面（可通行地面格，静态地形）</summary>
        public const string Road = "road";

        /// <summary>斑马线 / 人行横道</summary>
        public const string Crosswalk = "crosswalk";

        /// <summary>全部类别键（顺序与下拉展示一致）</summary>
        public static readonly string[] AllKeys =
        {
            None, Streetlight, Vehicle, Pedestrian, Road, Crosswalk
        };

        /// <summary>类别键 → 中文显示名</summary>
        private static readonly Dictionary<string, string> DisplayNameMap = new Dictionary<string, string>
        {
            { None, "不参与" },
            { Streetlight, "路灯" },
            { Vehicle, "汽车" },
            { Pedestrian, "人物" },
            { Road, "马路" },
            { Crosswalk, "斑马线" }
        };

        /// <summary>下拉展示的中文名称列表（与 <see cref="AllKeys"/> 同序，口径取自 DisplayNameMap）</summary>
        public static List<string> AllDisplayNames => new List<string>
        {
            DisplayNameMap[None], DisplayNameMap[Streetlight], DisplayNameMap[Vehicle],
            DisplayNameMap[Pedestrian], DisplayNameMap[Road], DisplayNameMap[Crosswalk]
        };

        /// <summary>
        /// 类别键转中文显示名；未知键回退为「不参与」
        /// </summary>
        public static string ToDisplayName(string category)
        {
            return MapToolEnumDisplay.ToDisplay(category, DisplayNameMap, None);
        }

        /// <summary>
        /// 中文显示名转类别键；未知名称回退为「不参与」
        /// </summary>
        public static string ToKey(string displayName)
        {
            return MapToolEnumDisplay.ToCode(displayName, DisplayNameMap, None);
        }

        /// <summary>
        /// 类别是否为载具（汽车）。仅汽车类别继承载具标记，其余类别均为非载具。
        /// </summary>
        public static bool IsVehicle(string category)
        {
            return category == Vehicle;
        }

        /// <summary>
        /// 由模板类别解析实例默认交通角色：汽车→vehicle，人物→pedestrian，其余静态类别→none。
        /// </summary>
        public static string ResolveTrafficRole(string category)
        {
            if (category == Vehicle) return Vehicle;
            if (category == Pedestrian) return Pedestrian;
            return None;
        }

        /// <summary>
        /// 归一类别输入：兼容英文键与中文显示名，未知值回退为「不参与」。
        /// </summary>
        public static string Normalize(string raw)
        {
            return MapToolEnumDisplay.ToCode(raw, DisplayNameMap, None);
        }
    }

    /// <summary>
    /// 模板交互类型目录：code（none/click/talk/enter/pickup）与右栏下拉中文显示名互转。
    /// 实例右栏可能直接存中文，回写模板时统一归一为 code；未知值回退「无(none)」。
    /// </summary>
    public static class TemplateInteractType
    {
        /// <summary>不交互</summary>
        public const string None = "none";
        /// <summary>点击</summary>
        public const string Click = "click";
        /// <summary>对话</summary>
        public const string Talk = "talk";
        /// <summary>进入</summary>
        public const string Enter = "enter";
        /// <summary>收集</summary>
        public const string Pickup = "pickup";

        /// <summary>code → 中文显示名</summary>
        private static readonly Dictionary<string, string> DisplayMap = new Dictionary<string, string>
        {
            { None, "无" },
            { Click, "点击" },
            { Talk, "对话" },
            { Enter, "进入" },
            { Pickup, "收集" }
        };

        /// <summary>中文显示名 → code（已是 code 时原样返回；未知回退 none）</summary>
        public static string ToCode(string raw)
        {
            return MapToolEnumDisplay.ToCode(raw, DisplayMap, None);
        }

        /// <summary>code → 中文显示名（未知回退「无」）</summary>
        public static string ToDisplay(string code)
        {
            return MapToolEnumDisplay.ToDisplay(code, DisplayMap, None);
        }

        /// <summary>下拉可选项（中文显示名，按「无→点击→对话→进入→收集」顺序；每次返回新列表避免被下拉控件就地修改）</summary>
        public static List<string> DisplayChoices => new List<string>
        {
            DisplayMap[None], DisplayMap[Click], DisplayMap[Talk], DisplayMap[Enter], DisplayMap[Pickup]
        };
    }

    /// <summary>
    /// 模板初始朝向目录：code（up/down/left/right）与右栏下拉中文显示名互转。
    /// 实例右栏可能直接存中文，回写模板时统一归一为 code；未知值回退「下(down)」。
    /// </summary>
    public static class TemplateFacing
    {
        /// <summary>上</summary>
        public const string Up = "up";
        /// <summary>下</summary>
        public const string Down = "down";
        /// <summary>左</summary>
        public const string Left = "left";
        /// <summary>右</summary>
        public const string Right = "right";

        /// <summary>code → 中文显示名</summary>
        private static readonly Dictionary<string, string> DisplayMap = new Dictionary<string, string>
        {
            { Up, "上" },
            { Down, "下" },
            { Left, "左" },
            { Right, "右" }
        };

        /// <summary>中文显示名 → code（已是 code 时原样返回；未知回退 down）</summary>
        public static string ToCode(string raw)
        {
            return MapToolEnumDisplay.ToCode(raw, DisplayMap, Down);
        }

        /// <summary>code → 中文显示名（未知回退「下」）</summary>
        public static string ToDisplay(string code)
        {
            return MapToolEnumDisplay.ToDisplay(code, DisplayMap, Down);
        }

        /// <summary>下拉可选项（中文显示名，按「上→下→左→右」顺序；每次返回新列表避免被下拉控件就地修改）</summary>
        public static List<string> DisplayChoices => new List<string>
        {
            DisplayMap[Up], DisplayMap[Down], DisplayMap[Left], DisplayMap[Right]
        };
    }

    /// <summary>
    /// 摆放/刷格绘制形状目录：code（"rect"/"diamond"）与底部属性面板下拉中文显示名互转。
    /// 默认菱形绘制；矩形为轴对齐矩形区域。未知值回退菱形。
    /// </summary>
    public static class DrawShape
    {
        /// <summary>矩形（轴对齐矩形区域）</summary>
        public const string Rect = "rect";
        /// <summary>菱形（默认，两点为菱形对角）</summary>
        public const string Diamond = "diamond";

        /// <summary>code → 中文显示名</summary>
        private static readonly Dictionary<string, string> DisplayMap = new Dictionary<string, string>
        {
            { Rect, "矩形绘制模式" },
            { Diamond, "菱形绘制模式" }
        };

        /// <summary>中文显示名 → code（已是 code 时原样返回；未知回退菱形）</summary>
        public static string ToCode(string raw)
        {
            return MapToolEnumDisplay.ToCode(raw, DisplayMap, Diamond);
        }

        /// <summary>code → 中文显示名（未知回退「菱形绘制模式」）</summary>
        public static string ToDisplay(string code)
        {
            return MapToolEnumDisplay.ToDisplay(code, DisplayMap, Diamond);
        }

        /// <summary>是否为菱形形状（未知/空值一律按菱形处理）</summary>
        public static bool IsDiamond(string code)
        {
            return ToCode(code) == Diamond;
        }
    }

    /// <summary>
    /// 摆放区类型目录：code（"place"/"forbid"/"common"）与下拉中文显示名互转。
    /// 默认摆放区；禁区禁止摆放，通用区为普通通行/展示区域。未知值回退摆放区。
    /// </summary>
    public static class RegionType
    {
        /// <summary>摆放区（默认，允许摆放物件）</summary>
        public const string Place = "place";
        /// <summary>禁区（禁止摆放物件）</summary>
        public const string Forbid = "forbid";
        /// <summary>通用区（普通通行/展示区域）</summary>
        public const string Common = "common";

        /// <summary>code → 中文显示名</summary>
        private static readonly Dictionary<string, string> DisplayMap = new Dictionary<string, string>
        {
            { Place, "摆放区" },
            { Forbid, "禁区" },
            { Common, "通用区" }
        };

        /// <summary>中文显示名 → code（已是 code 时原样返回；未知回退摆放区）</summary>
        public static string ToCode(string raw)
        {
            return MapToolEnumDisplay.ToCode(raw, DisplayMap, Place);
        }

        /// <summary>code → 中文显示名（未知回退「摆放区」）</summary>
        public static string ToDisplay(string code)
        {
            return MapToolEnumDisplay.ToDisplay(code, DisplayMap, Place);
        }
    }

    /// <summary>
    /// 枚举「code ⇄ 中文显示名」互转的共用实现
    /// 各枚举目录只维护自己的映射表与回退 code，规范化与查找算法统一收敛在这里
    /// </summary>
    internal static class MapToolEnumDisplay
    {
        /// <summary>
        /// 中文显示名（或 code）→ code：空值/未知回退 fallbackCode；入参先 Trim，已是 code 时原样返回
        /// </summary>
        /// <param name="raw">原始值（可能是 code，也可能是中文显示名）</param>
        /// <param name="displayMap">code → 显示名 映射表</param>
        /// <param name="fallbackCode">空值/未知时的回退 code</param>
        /// <returns>对应的 code</returns>
        public static string ToCode(string raw, Dictionary<string, string> displayMap, string fallbackCode)
        {
            if (string.IsNullOrEmpty(raw)) return fallbackCode;

            raw = raw.Trim();
            if (displayMap.ContainsKey(raw)) return raw;
            foreach (var pair in displayMap)
            {
                if (pair.Value == raw) return pair.Key;
            }
            return fallbackCode;
        }

        /// <summary>
        /// code → 中文显示名：空值/未知回退 fallbackCode 对应的显示名；入参先 Trim
        /// </summary>
        /// <param name="code">code 值</param>
        /// <param name="displayMap">code → 显示名 映射表</param>
        /// <param name="fallbackCode">空值/未知时的回退 code</param>
        /// <returns>中文显示名</returns>
        public static string ToDisplay(string code, Dictionary<string, string> displayMap, string fallbackCode)
        {
            if (string.IsNullOrEmpty(code)) return displayMap[fallbackCode];

            string name;
            return displayMap.TryGetValue(code.Trim(), out name) ? name : displayMap[fallbackCode];
        }
    }

    #endregion
}

