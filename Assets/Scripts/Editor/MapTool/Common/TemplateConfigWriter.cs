/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TemplateConfigWriter.cs
 * author:    云毅
 * created:   2026
 * descrip:   模板配置写入器 - 将内存中的模板数据写回 Excel(.xlsm) 文件
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 模板配置写入器
    /// 将 ObjectTemplateManager 中的模板数据写回 Excel(.xlsm) 文件
    /// 保持前4行表头不变（标题/列名/类型/备注），仅重写数据行
    /// </summary>
    public static class TemplateConfigWriter
    {
        // ==================== 列名常量（与 Reader 保持一致） ====================

        private const string ColumnTemplateId = "TemplateId";
        private const string ColumnTemplateName = "TemplateName";
        private const string ColumnResName = "ResName";
        private const string ColumnImagePath = "ImagePath";
        private const string ColumnAnimName = "AnimName";
        private const string ColumnIsLoop = "Loop";
        private const string ColumnSpeed = "Speed";
        private const string ColumnSortOrder = "SortOrder";
        private const string ColumnDialogBubble = "DialogBubble";
        private const string ColumnShadowEnabled = "ShadowEnabled";
        private const string ColumnIsCollectible = "IsCollectible";
        private const string ColumnRespawnTime = "RespawnTime";

        /// <summary>
        /// 数据起始行（第5行，1-based）
        /// </summary>
        private const int DataStartRow = 5;

        // ==================== 公共写入方法 ====================

        /// <summary>
        /// 将指定类型的模板数据写回 Excel 文件
        /// </summary>
        /// <param name="excelPath">Excel 文件绝对路径</param>
        /// <param name="templates">要写入的模板列表（已按 ID 排序）</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteToExcel(string excelPath, List<ObjectTemplate> templates)
        {
            if (string.IsNullOrEmpty(excelPath))
            {
                Debug.LogWarning("[TemplateConfigWriter] Excel 路径为空");
                return false;
            }

            if (templates == null)
            {
                Debug.LogWarning("[TemplateConfigWriter] 模板列表为空");
                return false;
            }

            // EPPlus 5+ 需要设置 License
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // 检查文件是否被占用
                if (IsFileLocked(excelPath))
                {
                    Debug.LogError($"[TemplateConfigWriter] 文件被占用，请先关闭 Excel: {excelPath}");
                    return false;
                }

                FileInfo fileInfo = new FileInfo(excelPath);
                using (var package = new ExcelPackage(fileInfo))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        Debug.LogError("[TemplateConfigWriter] Excel 中没有工作表");
                        return false;
                    }

                    var worksheet = package.Workbook.Worksheets[0];

                    // 1. 构建列名到列索引的映射（从第2行读取列名）
                    var columnMap = BuildColumnMap(worksheet);

                    // 2. 清除旧数据行（从数据起始行开始）
                    int oldDataRows = worksheet.Dimension?.Rows ?? 0;
                    if (oldDataRows >= DataStartRow)
                    {
                        int deleteCount = oldDataRows - DataStartRow + 1;
                        worksheet.DeleteRow(DataStartRow, deleteCount);
                    }

                    // 3. 按 TemplateId 数值升序排序，保证写入顺序正确
                    var sortedTemplates = templates.OrderBy(t => {
                        int id;
                        return int.TryParse(t.TemplateId, out id) ? id : int.MaxValue;
                    }).ToList();

                    // 4. 逐行写入新数据
                    for (int i = 0; i < sortedTemplates.Count; i++)
                    {
                        var t = sortedTemplates[i];
                        int row = DataStartRow + i;

                        WriteCell(worksheet, columnMap, ColumnTemplateId, row, t.TemplateId);
                        WriteCell(worksheet, columnMap, ColumnImagePath, row, t.ImagePath);
                        WriteCell(worksheet, columnMap, ColumnResName, row, "GameObject_" + t.Type.ToString());
                        WriteCell(worksheet, columnMap, ColumnTemplateName, row, t.TemplateName);
                        WriteCell(worksheet, columnMap, ColumnAnimName, row, t.AnimName);
                        WriteCell(worksheet, columnMap, ColumnIsLoop, row, t.Loop ? 1 : 0);
                        WriteCell(worksheet, columnMap, ColumnSpeed, row, t.Speed);
                        WriteCell(worksheet, columnMap, ColumnSortOrder, row, t.SortOrder);
                        WriteCell(worksheet, columnMap, ColumnDialogBubble, row, t.DialogBubble);
                        WriteCell(worksheet, columnMap, ColumnShadowEnabled, row, t.ShadowEnabled ? 1 : 0);
                        WriteCell(worksheet, columnMap, ColumnIsCollectible, row, t.IsCollectible ? 1 : 0);
                        WriteCell(worksheet, columnMap, ColumnRespawnTime, row, t.RespawnTime);
                    }

                    // 5. 保存文件
                    package.Save();
                }

                Debug.Log($"[TemplateConfigWriter] 写入成功: {Path.GetFileName(excelPath)}，共 {templates.Count} 行数据");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[TemplateConfigWriter] 写入 Excel 异常: {e.Message}\n{e.StackTrace}");
                return false;
            }
        }

        // ==================== 私有辅助方法 ====================

        /// <summary>
        /// 从第2行（列名行）构建列名到列索引的映射
        /// </summary>
        private static Dictionary<string, int> BuildColumnMap(ExcelWorksheet worksheet)
        {
            var map = new Dictionary<string, int>();
            int colCount = worksheet.Dimension?.Columns ?? 0;

            for (int col = 1; col <= colCount; col++)
            {
                string colName = worksheet.Cells[2, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(colName))
                {
                    map[colName] = col;
                }
            }

            return map;
        }

        /// <summary>
        /// 写入单元格值（如果列不存在则跳过）
        /// </summary>
        private static void WriteCell(ExcelWorksheet worksheet, Dictionary<string, int> columnMap,
            string columnName, int row, object value)
        {
            int col;
            if (columnMap.TryGetValue(columnName, out col))
            {
                worksheet.Cells[row, col].Value = value ?? "";
            }
        }

        /// <summary>
        /// 检查文件是否被其他进程占用
        /// </summary>
        private static bool IsFileLocked(string path)
        {
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }
    }
}
