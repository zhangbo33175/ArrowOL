/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapTemplateColumnSchema.cs
 * author:    云毅
 * created:   2026
 * descrip:   模板配置表列 → ObjectTemplate 字段的唯一映射表。
 *            读表（TemplateConfigReader）与写表（TemplateConfigWriter）
 *            均由本表驱动，新增字段只在此处补一行，避免两侧手写清单漂移。
 ***************************************************************/

using System;
using System.Collections.Generic;
using static Honor.Editor.MapTool.MapTemplateColumns;

namespace Honor.Editor.MapTool
{
    /// <summary>列值类型（驱动 Excel 单元格文本的解析与写入形态）</summary>
    public enum TemplateColumnValueKind
    {
        /// <summary>字符串</summary>
        String,
        /// <summary>布尔（写入为 1/0，读取兼容 true/false 与数字）</summary>
        Bool,
        /// <summary>整数</summary>
        Int,
        /// <summary>浮点</summary>
        Float
    }

    /// <summary>
    /// 单列定义：列名 + 类型 + 读写委托
    /// </summary>
    public sealed class TemplateColumnDef
    {
        /// <summary>列名（Excel 第2行表头）</summary>
        public readonly string Name;
        /// <summary>列值类型（同时作为第3行类型提示的取值依据）</summary>
        public readonly TemplateColumnValueKind Kind;
        /// <summary>表头第3行类型提示文本</summary>
        public readonly string TypeHint;
        /// <summary>表头缺失时是否自动补列</summary>
        public readonly bool EnsureWhenMissing;
        /// <summary>单元格文本 → 模板字段（读表）</summary>
        public readonly Action<ObjectTemplate, string> Assign;
        /// <summary>模板字段 → 单元格值（写表，布尔已转为 1/0）</summary>
        public readonly Func<ObjectTemplate, object> Read;

        /// <summary>
        /// 构造一列定义
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="kind">列值类型</param>
        /// <param name="assign">单元格文本 → 模板字段</param>
        /// <param name="read">模板字段 → 单元格值</param>
        /// <param name="ensureWhenMissing">表头缺失时是否自动补列</param>
        public TemplateColumnDef(string name, TemplateColumnValueKind kind,
            Action<ObjectTemplate, string> assign, Func<ObjectTemplate, object> read,
            bool ensureWhenMissing = false)
        {
            Name = name;
            Kind = kind;
            Assign = assign;
            Read = read;
            EnsureWhenMissing = ensureWhenMissing;
            TypeHint = Kind == TemplateColumnValueKind.Bool
                ? "bool"
                : Kind == TemplateColumnValueKind.Int
                    ? "int"
                    : Kind == TemplateColumnValueKind.Float ? "float" : "string";
        }
    }

    /// <summary>
    /// 模板配置表列 schema（读写同源的唯一字段清单）
    /// </summary>
    public static class MapTemplateColumnSchema
    {
        /// <summary>
        /// 全部列定义（顺序即写表列语义顺序；实际写入按表头列名定位列）
        /// </summary>
        public static readonly IList<TemplateColumnDef> All = new List<TemplateColumnDef>
        {
            new TemplateColumnDef(ColumnTemplateId, TemplateColumnValueKind.String,
                (t, raw) => t.TemplateId = raw, t => t.TemplateId),
            new TemplateColumnDef(ColumnTemplateName, TemplateColumnValueKind.String,
                (t, raw) => t.TemplateName = raw, t => t.TemplateName),
            new TemplateColumnDef(ColumnResName, TemplateColumnValueKind.String,
                (t, raw) => t.ResName = raw,
                t => string.IsNullOrEmpty(t.ResName) ? MapToolConstants.ResNamePrefix + t.Type : t.ResName),
            new TemplateColumnDef(ColumnImagePath, TemplateColumnValueKind.String,
                (t, raw) => t.ImagePath = raw, t => t.ImagePath),
            new TemplateColumnDef(ColumnAnimName, TemplateColumnValueKind.String,
                (t, raw) => t.AnimName = raw, t => t.AnimName),
            new TemplateColumnDef(ColumnIsLoop, TemplateColumnValueKind.Bool,
                (t, raw) => t.Loop = SchemaValues.Bool(raw), t => t.Loop ? 1 : 0),
            new TemplateColumnDef(ColumnSpeed, TemplateColumnValueKind.Float,
                (t, raw) => t.Speed = SchemaValues.Float(raw), t => t.Speed),
            new TemplateColumnDef(ColumnSortOrder, TemplateColumnValueKind.Int,
                (t, raw) => t.SortOrder = SchemaValues.Int(raw), t => t.SortOrder),
            new TemplateColumnDef(ColumnDisplayLayer, TemplateColumnValueKind.String,
                (t, raw) => t.DisplayLayer = raw, t => t.DisplayLayer, true),
            new TemplateColumnDef(ColumnDialogBubble, TemplateColumnValueKind.String,
                (t, raw) => t.DialogBubble = raw, t => t.DialogBubble),
            new TemplateColumnDef(ColumnBubbleId, TemplateColumnValueKind.Int,
                (t, raw) => t.BubbleId = SchemaValues.Int(raw), t => t.BubbleId, true),
            new TemplateColumnDef(ColumnShadowEnabled, TemplateColumnValueKind.Bool,
                (t, raw) => t.ShadowEnabled = SchemaValues.Bool(raw), t => t.ShadowEnabled ? 1 : 0),
            new TemplateColumnDef(ColumnIsCollectible, TemplateColumnValueKind.Bool,
                (t, raw) => t.IsCollectible = SchemaValues.Bool(raw), t => t.IsCollectible ? 1 : 0),
            new TemplateColumnDef(ColumnRespawnTime, TemplateColumnValueKind.Float,
                (t, raw) => t.RespawnTime = SchemaValues.Float(raw), t => t.RespawnTime),
            new TemplateColumnDef(ColumnTrafficCategory, TemplateColumnValueKind.String,
                (t, raw) => t.TrafficCategory = raw, t => t.TrafficCategory),
            new TemplateColumnDef(ColumnIsInteractable, TemplateColumnValueKind.Bool,
                (t, raw) => t.Interactable = SchemaValues.Bool(raw), t => t.Interactable ? 1 : 0, true),
            new TemplateColumnDef(ColumnInteractType, TemplateColumnValueKind.String,
                (t, raw) => t.InteractType = raw, t => t.InteractType, true),
            new TemplateColumnDef(ColumnHasCollision, TemplateColumnValueKind.Bool,
                (t, raw) => t.CollisionEnabled = SchemaValues.Bool(raw), t => t.CollisionEnabled ? 1 : 0, true),
            new TemplateColumnDef(ColumnFacingDirection, TemplateColumnValueKind.String,
                (t, raw) => t.FacingDirection = raw, t => t.FacingDirection, true),
            new TemplateColumnDef(ColumnYSortEnabled, TemplateColumnValueKind.Bool,
                (t, raw) => t.YSortEnabled = SchemaValues.Bool(raw), t => t.YSortEnabled ? 1 : 0, true),
            new TemplateColumnDef(ColumnYSortOffset, TemplateColumnValueKind.Float,
                (t, raw) => t.YSortOffset = SchemaValues.Float(raw), t => t.YSortOffset, true),
            new TemplateColumnDef(ColumnIsMovable, TemplateColumnValueKind.Bool,
                (t, raw) => t.IsMovable = SchemaValues.Bool(raw), t => t.IsMovable ? 1 : 0, true),
            new TemplateColumnDef(ColumnMoveAnimName, TemplateColumnValueKind.String,
                (t, raw) => t.MoveAnimName = raw, t => t.MoveAnimName, true),
            new TemplateColumnDef(ColumnLoopRestartDelay, TemplateColumnValueKind.Float,
                (t, raw) => t.LoopRestartDelay = SchemaValues.Float(raw), t => t.LoopRestartDelay, true),
            new TemplateColumnDef(ColumnHideWhenOffMap, TemplateColumnValueKind.Bool,
                (t, raw) => t.HideWhenOffMap = SchemaValues.Bool(raw), t => t.HideWhenOffMap ? 1 : 0, true),
            new TemplateColumnDef(ColumnIsTeleport, TemplateColumnValueKind.Bool,
                (t, raw) => t.IsTeleport = SchemaValues.Bool(raw), t => t.IsTeleport ? 1 : 0, true),
            new TemplateColumnDef(ColumnTeleportTargetScene, TemplateColumnValueKind.String,
                (t, raw) => t.TeleportTargetScene = raw, t => t.TeleportTargetScene, true),
            new TemplateColumnDef(ColumnTeleportTargetX, TemplateColumnValueKind.Float,
                (t, raw) => t.TeleportTargetX = SchemaValues.Float(raw), t => t.TeleportTargetX, true),
            new TemplateColumnDef(ColumnTeleportTargetY, TemplateColumnValueKind.Float,
                (t, raw) => t.TeleportTargetY = SchemaValues.Float(raw), t => t.TeleportTargetY, true),
        };
    }

    /// <summary>
    /// 单元格文本安全转换（读表侧统一口径：空/非法值回落零值）
    /// </summary>
    public static class SchemaValues
    {
        /// <summary>解析布尔：兼容 true/false 文本与数字（非 0 为真）</summary>
        public static bool Bool(string raw)
        {
            if (bool.TryParse(raw, out bool boolValue))
                return boolValue;
            if (int.TryParse(raw, out int intValue))
                return intValue != 0;
            return false;
        }

        /// <summary>解析整数：非法值回落 0</summary>
        public static int Int(string raw)
        {
            return int.TryParse(raw, out int value) ? value : 0;
        }

        /// <summary>解析浮点：非法值回落 0（与既有读表口径一致，按当前区域设置解析）</summary>
        public static float Float(string raw)
        {
            return float.TryParse(raw, out float value) ? value : 0f;
        }
    }
}
