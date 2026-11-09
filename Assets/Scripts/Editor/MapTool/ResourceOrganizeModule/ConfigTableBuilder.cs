/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigTableBuilder.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置表表格构建器 - 负责构建 Excel 样式的表格行和单元格
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool.ResourceOrganize
{
    /// <summary>
    /// 配置表表格构建器：负责构建表格行和单元格（可编辑文本、Toggle、删除按钮）
    /// </summary>
    public static class ConfigTableBuilder
    {
        // ==================== 列宽定义（与 UXML 表头保持一致） ====================

        /// <summary>TemplateId 列宽</summary>
        public const float ColWidthId = 80f;
        /// <summary>ImagePath 列宽</summary>
        public const float ColWidthImagePath = 140f;
        /// <summary>ResName 列宽</summary>
        public const float ColWidthResName = 150f;
        /// <summary>TemplateName 列宽</summary>
        public const float ColWidthName = 90f;
        /// <summary>Loop 列宽</summary>
        public const float ColWidthLoop = 80f;
        /// <summary>Speed 列宽</summary>
        public const float ColWidthSpeed = 80f;
        /// <summary>SortOrder 列宽</summary>
        public const float ColWidthSortOrder = 120f;
        /// <summary>DialogBubble 列宽</summary>
        public const float ColWidthDialog = 120f;
        /// <summary>IsCollectible 列宽</summary>
        public const float ColWidthCollectible = 90f;
        /// <summary>RespawnTime 列宽</summary>
        public const float ColWidthRespawn = 100f;
        /// <summary>操作列宽</summary>
        public const float ColWidthAction = 56f;

        // ==================== 单元格样式常量 ====================

        private static readonly Color s_DefaultTextColor = Color.black;
        private static readonly int s_FontSize = 11;
        private static readonly float s_CellPadding = 6f;
        private static readonly Color s_BorderColor = Color.black;

        /// <summary>
        /// 添加可编辑文本单元格
        /// </summary>
        /// <param name="parent">父容器</param>
        /// <param name="text">初始文本</param>
        /// <param name="width">列宽</param>
        /// <param name="editable">是否可编辑</param>
        public static void AddEditableCell(VisualElement parent, string text, float width, bool editable)
        {
            if (!editable)
            {
                var label = CreateLabel(text, width);
                parent.Add(label);
            }
            else
            {
                var field = CreateTextField(text, width);
                parent.Add(field);
            }
        }

        /// <summary>
        /// 添加 Toggle 单元格（用于 bool 字段）
        /// </summary>
        /// <param name="parent">父容器</param>
        /// <param name="value">初始值</param>
        /// <param name="width">列宽</param>
        public static void AddToggleCell(VisualElement parent, bool value, float width)
        {
            var toggle = new Toggle
            {
                value = value,
                style =
                {
                    width = width,
                    fontSize = s_FontSize,
                    marginLeft = 0,
                    marginRight = 0,
                    paddingLeft = s_CellPadding,
                    paddingRight = s_CellPadding,
                    borderRightWidth = 1,
                    borderRightColor = s_BorderColor,
                    backgroundColor = Color.clear
                }
            };
            parent.Add(toggle);
        }

        /// <summary>
        /// 添加删除按钮单元格
        /// </summary>
        /// <param name="parent">父容器</param>
        /// <param name="onClick">点击回调</param>
        public static void AddDeleteButton(VisualElement parent, System.Action onClick)
        {
            var btn = new Button(() => onClick?.Invoke())
            {
                text = "删除",
                style =
                {
                    width = ColWidthAction,
                    height = 20,
                    fontSize = 10,
                    marginLeft = 2,
                    marginRight = 2
                }
            };
            parent.Add(btn);
        }

        // ==================== 私有辅助方法 ====================

        /// <summary>
        /// 创建文本标签单元格
        /// </summary>
        private static Label CreateLabel(string text, float width)
        {
            return new Label(text)
            {
                style =
                {
                    width = width,
                    fontSize = s_FontSize,
                    color = s_DefaultTextColor,
                    marginLeft = 0,
                    marginRight = 0,
                    paddingLeft = s_CellPadding,
                    paddingRight = s_CellPadding,
                    borderRightWidth = 1,
                    borderRightColor = s_BorderColor
                }
            };
        }

        /// <summary>
        /// 创建可编辑文本框单元格
        /// </summary>
        private static TextField CreateTextField(string text, float width)
        {
            var field = new TextField
            {
                value = text,
                style =
                {
                    width = width,
                    fontSize = s_FontSize,
                    color = s_DefaultTextColor,
                    marginLeft = 0,
                    marginRight = 0,
                    paddingLeft = s_CellPadding,
                    paddingRight = s_CellPadding,
                    borderTopWidth = 0,
                    borderBottomWidth = 0,
                    borderLeftWidth = 0,
                    borderRightWidth = 1,
                    borderRightColor = s_BorderColor,
                    backgroundColor = Color.clear
                }
            };

            // 清除内部输入框的默认样式
            var innerInput = field.Q("unity-text-input");
            if (innerInput != null)
            {
                innerInput.style.backgroundColor = Color.clear;
                innerInput.style.borderTopWidth = 0;
                innerInput.style.borderBottomWidth = 0;
                innerInput.style.borderLeftWidth = 0;
                innerInput.style.borderRightWidth = 0;
                innerInput.style.marginLeft = 0;
                innerInput.style.marginRight = 0;
                innerInput.style.paddingLeft = 0;
                innerInput.style.paddingRight = 0;
                innerInput.style.color = s_DefaultTextColor;
            }

            return field;
        }
    }
}
