/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TemplateConfigReader.cs
 * author:    云毅
 * created:   2026
 * descrip:   模板配置读取器 - 从Excel(.xlsm)文件读取模板配置
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Honor.Editor;
using static Honor.Editor.MapTool.MapTemplateColumns;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 模板配置读取器
    /// 从Excel(.xlsm)文件读取地图元素模板配置
    /// 配置表格式：
    ///   第1行：标题行
    ///   第2行：列名行
    ///   第3行：类型行
    ///   第4行：备注行
    ///   第5行起：数据行
    /// </summary>
    public static class TemplateConfigReader
    {
        #region 行索引常量

        // 列名常量由 MapTemplateColumns 提供；字段读取由 MapTemplateColumnSchema 驱动（与写表同源）

        /// <summary>
        /// 列名行索引（第2行）
        /// </summary>
        private const int HeaderRowIndex = 1;

        /// <summary>
        /// 数据起始行索引（第5行）
        /// </summary>
        private const int DataStartRowIndex = 4;

        #endregion

        #region 公共读取方法

        /// <summary>
        /// 从Excel文件读取模板配置
        /// </summary>
        /// <param name="excelFilePath">Excel文件绝对路径（.xlsm格式）</param>
        /// <returns>模板列表，读取失败返回空列表</returns>
        public static List<ObjectTemplate> ReadFromExcel(string excelFilePath)
        {
            var templates = new List<ObjectTemplate>();

            // 参数校验
            if (string.IsNullOrEmpty(excelFilePath))
            {
                MapToolLog.Warn("[TemplateConfigReader] Excel文件路径为空");
                return templates;
            }

            if (!File.Exists(excelFilePath))
            {
                MapToolLog.Warn($"[TemplateConfigReader] Excel文件不存在: {excelFilePath}");
                return templates;
            }

            try
            {
                // 使用项目已有的Excel读取工具
                DataSet dataSet = TableExportEditorUtility.GetExcelData(excelFilePath);

                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    MapToolLog.Warn($"[TemplateConfigReader] Excel读取失败或没有数据: {Path.GetFileName(excelFilePath)}");
                    return templates;
                }

                // 获取第一个Sheet
                DataTable dataTable = dataSet.Tables[0];

                // 根据列名动态构建列索引映射
                Dictionary<string, int> columnIndexMap = BuildColumnIndexMap(dataTable);

                // 遍历数据行
                for (int rowIndex = DataStartRowIndex; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    DataRow dataRow = dataTable.Rows[rowIndex];

                    // 跳过空行
                    if (IsRowEmpty(dataRow, columnIndexMap))
                        continue;

                    // 解析行数据
                    ObjectTemplate template = ParseDataRow(dataRow, columnIndexMap);
                    if (template != null)
                    {
                        templates.Add(template);
                    }
                }

                MapToolLog.Info($"[TemplateConfigReader] 读取完成: {Path.GetFileName(excelFilePath)}，共 {templates.Count} 个模板");
            }
            catch (Exception e)
            {
                MapToolLog.Error($"[TemplateConfigReader] 读取Excel异常: {e.Message}\n{e.StackTrace}");
            }

            return templates;
        }

        #endregion

        #region 内部解析方法

        /// <summary>
        /// 根据列名行构建列名到列索引的映射
        /// </summary>
        private static Dictionary<string, int> BuildColumnIndexMap(DataTable dataTable)
        {
            var columnIndexMap = new Dictionary<string, int>();

            if (dataTable.Rows.Count <= HeaderRowIndex)
            {
                MapToolLog.Warn("[TemplateConfigReader] Excel行数不足，找不到列名行");
                return columnIndexMap;
            }

            DataRow headerRow = dataTable.Rows[HeaderRowIndex];

            // 遍历所有列，建立列名->索引映射
            for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
            {
                string columnName = headerRow[columnIndex]?.ToString().Trim();
                if (string.IsNullOrEmpty(columnName))
                    continue;

                columnIndexMap[columnName] = columnIndex;
                MapToolLog.Info($"[TemplateConfigReader] 列映射: {columnName} -> 索引{columnIndex}");
            }

            return columnIndexMap;
        }

        /// <summary>
        /// 判断数据行是否为空
        /// </summary>
        private static bool IsRowEmpty(DataRow dataRow, Dictionary<string, int> columnIndexMap)
        {
            int idColumnIndex;
            if (!columnIndexMap.TryGetValue(ColumnTemplateId, out idColumnIndex))
                return true;

            if (idColumnIndex >= dataRow.Table.Columns.Count)
                return true;

            object cellValue = dataRow[idColumnIndex];
            if (cellValue == null)
                return true;

            string idValue = cellValue.ToString().Trim();
            return string.IsNullOrEmpty(idValue);
        }

        private static ObjectTemplate ParseDataRow(DataRow dataRow, Dictionary<string, int> columnIndexMap)
        {
            try
            {
                var template = ReadRawFields(dataRow, columnIndexMap);
                NormalizeParsedTemplate(template, dataRow, columnIndexMap);
                return template;
            }
            catch (Exception e)
            {
                MapToolLog.Warn($"[TemplateConfigReader] 解析行数据失败，跳过该行: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 按列名读取原始字段值（不做任何归一，归一统一由 NormalizeParsedTemplate 处理）
        /// </summary>
        /// <param name="dataRow">数据行</param>
        /// <param name="columnIndexMap">列名 → 列索引</param>
        /// <returns>原始模板对象</returns>
        private static ObjectTemplate ReadRawFields(DataRow dataRow, Dictionary<string, int> columnIndexMap)
        {
            var template = new ObjectTemplate();

            // 列清单与解析口径统一来自 MapTemplateColumnSchema（与写表同源）
            foreach (var def in MapTemplateColumnSchema.All)
            {
                def.Assign(template, GetStringValue(dataRow, columnIndexMap, def.Name));
            }

            return template;
        }

        /// <summary>
        /// 原始值归一：类型判定 / 交通类别 / 交互类型 / 朝向 / 移动动画缺省 / 显示层 / 排序档位
        /// </summary>
        /// <param name="template">待归一模板</param>
        /// <param name="dataRow">数据行</param>
        /// <param name="columnIndexMap">列名 → 列索引</param>
        private static void NormalizeParsedTemplate(ObjectTemplate template, DataRow dataRow, Dictionary<string, int> columnIndexMap)
        {
            // 根据资源名判断物体类型
            template.ResName = GetStringValue(dataRow, columnIndexMap, ColumnResName);
            template.Type = DetermineObjectType(template.ResName);

            // 交通类别空值/中文显示名归一为类别键
            template.TrafficCategory = TemplateTrafficCategory.Normalize(
                GetStringValue(dataRow, columnIndexMap, ColumnTrafficCategory));

            // 交互类型/朝向归一为 code（存量表可能存中文显示名）
            template.InteractType = TemplateInteractType.ToCode(template.InteractType);
            template.FacingDirection = TemplateFacing.ToCode(template.FacingDirection);

            // 移动动画缺省回落模型默认 walk
            if (string.IsNullOrEmpty(template.MoveAnimName))
                template.MoveAnimName = "walk";

            // 层级档非空归一为八层键；空值保留，加载时跟随类型归属层
            if (!string.IsNullOrWhiteSpace(template.DisplayLayer))
                template.DisplayLayer = MapLayerCatalog.Normalize(template.DisplayLayer);

            // 渲染层级归一到类型所属八层基线区间（旧三档/裸值按层内槽位叠基线）
            template.SortOrder = MapToolEditorUtils.NormalizeTemplateSortOrder(template, template.SortOrder);
        }

        /// <summary>
        /// 根据资源名判断物体类型
        /// </summary>
        private static MapObjectType DetermineObjectType(string resName)
        {
            if (string.IsNullOrEmpty(resName))
                return MapObjectType.Prop;

            if (resName.Contains(MapToolConstants.ResNameKeywordBuilding, StringComparison.OrdinalIgnoreCase))
                return MapObjectType.Building;

            if (resName.Contains(MapToolConstants.ResNameKeywordCharacter, StringComparison.OrdinalIgnoreCase))
                return MapObjectType.Character;

            return MapObjectType.Prop;
        }

        #endregion

        #region 安全类型转换

        /// <summary>
        /// 安全获取字符串值
        /// </summary>
        private static string GetStringValue(DataRow dataRow, Dictionary<string, int> columnIndexMap, string columnName)
        {
            int columnIndex;
            if (!columnIndexMap.TryGetValue(columnName, out columnIndex))
                return string.Empty;

            if (columnIndex >= dataRow.Table.Columns.Count)
                return string.Empty;

            object cellValue = dataRow[columnIndex];
            return cellValue?.ToString().Trim() ?? string.Empty;
        }

        #endregion
    }
}
