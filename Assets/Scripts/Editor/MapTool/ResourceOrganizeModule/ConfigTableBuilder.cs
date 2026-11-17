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

        /// <summary>偶数行背景色（交替行样式）</summary>
        private static readonly Color s_EvenRowColor = new Color(0.95f, 0.95f, 0.95f);

        // ==================== 列标识（控件 name，保存时按标识回读） ====================
        /// <summary>图片路径列</summary>
        public const string ColKeyImagePath   = "cfg-col-image-path";
        /// <summary>模板名称列</summary>
        public const string ColKeyName        = "cfg-col-name";
        /// <summary>移动速度列</summary>
        public const string ColKeySpeed       = "cfg-col-speed";
        /// <summary>排序值列</summary>
        public const string ColKeySortOrder   = "cfg-col-sort-order";
        /// <summary>对话气泡列</summary>
        public const string ColKeyDialog      = "cfg-col-dialog";
        /// <summary>重生时间列</summary>
        public const string ColKeyRespawn     = "cfg-col-respawn";
        /// <summary>循环开关列</summary>
        public const string ColKeyLoop        = "cfg-col-loop";
        /// <summary>可收集开关列</summary>
        public const string ColKeyCollectible = "cfg-col-collectible";

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
        /// <param name="columnKey">列标识（写入控件 name，保存时按标识取回，避免依赖子控件顺序）</param>
        public static void AddEditableCell(VisualElement parent, string text, float width, bool editable, string columnKey = null)
        {
            if (!editable)
            {
                var label = CreateLabel(text, width);
                parent.Add(label);
                return;
            }

            var field = CreateTextField(text, width);
            if (!string.IsNullOrEmpty(columnKey)) field.name = columnKey;
            parent.Add(field);
        }

        /// <summary>
        /// 添加 Toggle 单元格（用于 bool 字段）
        /// </summary>
        /// <param name="parent">父容器</param>
        /// <param name="value">初始值</param>
        /// <param name="width">列宽</param>
        /// <param name="columnKey">列标识（写入控件 name，保存时按标识取回，避免依赖子控件顺序）</param>
        public static void AddToggleCell(VisualElement parent, bool value, float width, string columnKey = null)
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
            if (!string.IsNullOrEmpty(columnKey)) toggle.name = columnKey;
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
                text = MapToolConstants.DeleteButtonText,
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

        /// <summary>
        /// 创建一行模板数据（列顺序与表头一致，唯一维护点；可编辑列带列标识，保存时按标识回读）
        /// </summary>
        /// <param name="template">模板数据</param>
        /// <param name="rowIndex">行序号（用于交替背景色）</param>
        /// <param name="onDelete">删除回调</param>
        /// <returns>表格行容器</returns>
        public static VisualElement CreateTemplateRow(ObjectTemplate template, int rowIndex, System.Action onDelete)
        {
            var row = CreateRowContainer(rowIndex % 2 == 0);
            row.userData = template; // 保存模板引用，供保存时回读

            AddEditableCell(row, template.TemplateId, ColWidthId, false); // ID 只读
            AddEditableCell(row, template.ImagePath, ColWidthImagePath, true, ColKeyImagePath);
            AddEditableCell(row, MapToolConstants.ResNamePrefix + template.Type, ColWidthResName, false); // ResName（固定格式）
            AddEditableCell(row, template.TemplateName, ColWidthName, true, ColKeyName);
            AddToggleCell(row, template.Loop, ColWidthLoop, ColKeyLoop);
            AddEditableCell(row, template.Speed.ToString("0.#"), ColWidthSpeed, true, ColKeySpeed);
            AddEditableCell(row, template.SortOrder.ToString(), ColWidthSortOrder, true, ColKeySortOrder);
            AddEditableCell(row, template.DialogBubble, ColWidthDialog, true, ColKeyDialog);
            AddToggleCell(row, template.IsCollectible, ColWidthCollectible, ColKeyCollectible);
            AddEditableCell(row, template.RespawnTime.ToString("0.#"), ColWidthRespawn, true, ColKeyRespawn);

            AddDeleteButton(row, onDelete);
            return row;
        }

        /// <summary>
        /// 创建数据行容器（横向布局 + 边框 + 交替背景色）
        /// </summary>
        /// <param name="evenRow">是否为偶数行（决定浅灰/白背景）</param>
        /// <returns>行容器</returns>
        private static VisualElement CreateRowContainer(bool evenRow)
        {
            var row = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    borderBottomWidth = 1,
                    borderBottomColor = Color.black,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderLeftColor = Color.black,
                    borderRightColor = Color.black,
                    backgroundColor = evenRow ? s_EvenRowColor : Color.white
                }
            };
            return row;
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
            var innerInput = field.Q(MapToolUiNames.InnerTextInput);
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
