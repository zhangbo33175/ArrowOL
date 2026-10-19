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
        #region 列名常量

        private const string ColumnTemplateId = "TemplateId";
        private const string ColumnTemplateName = "TemplateName";
        private const string ColumnResName = "ResName";
        private const string ColumnImagePath = "ImagePath";
        private const string ColumnAnimName = "AnimName";
        private const string ColumnIsLoop = "Loop";
        private const string ColumnSpeed = "Speed";
        private const string ColumnIsInteractable = "Interactable";
        private const string ColumnInteractType = "InteractType";
        private const string ColumnSortOrder = "SortOrder";
        private const string ColumnHasCollision = "CollisionEnabled";
        private const string ColumnDialogBubble = "DialogBubble";
        private const string ColumnFacingDirection = "FacingDirection";
        private const string ColumnHasShadow = "ShadowEnabled";
        private const string ColumnIsCollectible = "IsCollectible";
        private const string ColumnRespawnTime = "RespawnTime";

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
                Debug.LogWarning("[TemplateConfigReader] Excel文件路径为空");
                return templates;
            }

            if (!File.Exists(excelFilePath))
            {
                Debug.LogWarning($"[TemplateConfigReader] Excel文件不存在: {excelFilePath}");
                return templates;
            }

            try
            {
                // 使用项目已有的Excel读取工具
                DataSet dataSet = TableExportEditorUtility.GetExcelData(excelFilePath);

                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    Debug.LogWarning($"[TemplateConfigReader] Excel读取失败或没有数据: {Path.GetFileName(excelFilePath)}");
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

                Debug.Log($"[TemplateConfigReader] 读取完成: {Path.GetFileName(excelFilePath)}，共 {templates.Count} 个模板");
            }
            catch (Exception e)
            {
                Debug.LogError($"[TemplateConfigReader] 读取Excel异常: {e.Message}\n{e.StackTrace}");
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
                Debug.LogWarning("[TemplateConfigReader] Excel行数不足，找不到列名行");
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
                Debug.Log($"[TemplateConfigReader] 列映射: {columnName} -> 索引{columnIndex}");
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

        /// <summary>
        /// 解析一行数据为模板对象
        /// </summary>
        private static ObjectTemplate ParseDataRow(DataRow dataRow, Dictionary<string, int> columnIndexMap)
        {
            try
            {
                var template = new ObjectTemplate
                {
                    TemplateId = GetStringValue(dataRow, columnIndexMap, ColumnTemplateId),
                    TemplateName = GetStringValue(dataRow, columnIndexMap, ColumnTemplateName),
                    ImagePath = GetStringValue(dataRow, columnIndexMap, ColumnImagePath),
                    AnimName = GetStringValue(dataRow, columnIndexMap, ColumnAnimName),
                    Loop = GetBoolValue(dataRow, columnIndexMap, ColumnIsLoop),
                    Speed = GetFloatValue(dataRow, columnIndexMap, ColumnSpeed),
                    Interactable = GetBoolValue(dataRow, columnIndexMap, ColumnIsInteractable),
                    InteractType = GetStringValue(dataRow, columnIndexMap, ColumnInteractType),
                    SortOrder = GetIntValue(dataRow, columnIndexMap, ColumnSortOrder),
                    CollisionEnabled = GetBoolValue(dataRow, columnIndexMap, ColumnHasCollision),
                    DialogBubble = GetStringValue(dataRow, columnIndexMap, ColumnDialogBubble),
                    FacingDirection = GetStringValue(dataRow, columnIndexMap, ColumnFacingDirection),
                    ShadowEnabled = GetBoolValue(dataRow, columnIndexMap, ColumnHasShadow),
                    IsCollectible = GetBoolValue(dataRow, columnIndexMap, ColumnIsCollectible),
                    RespawnTime = GetFloatValue(dataRow, columnIndexMap, ColumnRespawnTime),
                };

                // 根据资源名判断物体类型
                string resName = GetStringValue(dataRow, columnIndexMap, ColumnResName);
                template.Type = DetermineObjectType(resName);

                return template;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[TemplateConfigReader] 解析行数据失败，跳过该行: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 根据资源名判断物体类型
        /// </summary>
        private static MapObjectType DetermineObjectType(string resName)
        {
            if (string.IsNullOrEmpty(resName))
                return MapObjectType.Prop;

            if (resName.Contains("Building", StringComparison.OrdinalIgnoreCase))
                return MapObjectType.Building;

            if (resName.Contains("Character", StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// 安全获取布尔值
        /// </summary>
        private static bool GetBoolValue(DataRow dataRow, Dictionary<string, int> columnIndexMap, string columnName)
        {
            string stringValue = GetStringValue(dataRow, columnIndexMap, columnName);

            bool boolValue;
            if (bool.TryParse(stringValue, out boolValue))
                return boolValue;

            // 支持数字格式：1=true, 0=false
            int intValue;
            if (int.TryParse(stringValue, out intValue))
                return intValue != 0;

            return false;
        }

        /// <summary>
        /// 安全获取整数值
        /// </summary>
        private static int GetIntValue(DataRow dataRow, Dictionary<string, int> columnIndexMap, string columnName)
        {
            string stringValue = GetStringValue(dataRow, columnIndexMap, columnName);

            int intValue;
            if (int.TryParse(stringValue, out intValue))
                return intValue;

            return 0;
        }

        /// <summary>
        /// 安全获取浮点数值
        /// </summary>
        private static float GetFloatValue(DataRow dataRow, Dictionary<string, int> columnIndexMap, string columnName)
        {
            string stringValue = GetStringValue(dataRow, columnIndexMap, columnName);

            float floatValue;
            if (float.TryParse(stringValue, out floatValue))
                return floatValue;

            return 0f;
        }

        #endregion
    }
}
