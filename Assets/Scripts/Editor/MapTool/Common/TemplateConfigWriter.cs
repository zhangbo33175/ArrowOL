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
        // 列名与取值口径统一由 MapTemplateColumns / MapTemplateColumnSchema 提供（与 Reader 同源）

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
                MapToolLog.Warn("[TemplateConfigWriter] Excel 路径为空");
                return false;
            }

            if (templates == null)
            {
                MapToolLog.Warn("[TemplateConfigWriter] 模板列表为空");
                return false;
            }

            // EPPlus 5+ 需要设置 License
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // 检查文件是否被占用
                if (IsFileLocked(excelPath))
                {
                    MapToolLog.Error($"[TemplateConfigWriter] 文件被占用，请先关闭 Excel: {excelPath}");
                    return false;
                }

                FileInfo fileInfo = new FileInfo(excelPath);
                using (var package = new ExcelPackage(fileInfo))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        MapToolLog.Error("[TemplateConfigWriter] Excel 中没有工作表");
                        return false;
                    }

                    var worksheet = package.Workbook.Worksheets[0];

                    // 1. 构建列名到列索引的映射，并补齐模型全集所需列
                    var columnMap = BuildColumnMap(worksheet);
                    EnsureRequiredColumns(worksheet, columnMap);

                    // 2. 清除旧数据行
                    ClearDataRows(worksheet);

                    // 3. 逐行写入新数据（按 TemplateId 数值升序）
                    WriteTemplateRows(worksheet, columnMap, SortByTemplateId(templates));

                    // 4. 保存文件
                    package.Save();
                }

                MapToolLog.Info($"[TemplateConfigWriter] 写入成功: {Path.GetFileName(excelPath)}，共 {templates.Count} 行数据");
                return true;
            }
            catch (Exception e)
            {
                MapToolLog.Error($"[TemplateConfigWriter] 写入 Excel 异常: {e.Message}\n{e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 按 TemplateId 数值升序排序（非数字 ID 排到末尾）
        /// </summary>
        /// <param name="templates">模板列表</param>
        /// <returns>排序后的新列表</returns>
        private static List<ObjectTemplate> SortByTemplateId(List<ObjectTemplate> templates)
        {
            return templates
                .OrderBy(t => int.TryParse(t.TemplateId, out int id) ? id : int.MaxValue)
                .ToList();
        }

        /// <summary>
        /// 清除数据起始行之后的旧数据
        /// </summary>
        /// <param name="worksheet">目标工作表</param>
        private static void ClearDataRows(ExcelWorksheet worksheet)
        {
            int oldDataRows = worksheet.Dimension?.Rows ?? 0;
            if (oldDataRows < DataStartRow) return;

            worksheet.DeleteRow(DataStartRow, oldDataRows - DataStartRow + 1);
        }

        /// <summary>
        /// 批量写入模板行
        /// </summary>
        /// <param name="worksheet">目标工作表</param>
        /// <param name="columnMap">列名 → 列索引映射</param>
        /// <param name="sortedTemplates">已排序的模板列表</param>
        private static void WriteTemplateRows(ExcelWorksheet worksheet, Dictionary<string, int> columnMap,
            List<ObjectTemplate> sortedTemplates)
        {
            for (int i = 0; i < sortedTemplates.Count; i++)
            {
                WriteTemplateRow(worksheet, columnMap, DataStartRow + i, sortedTemplates[i]);
            }
        }

        /// <summary>
        /// 写入单个模板行（列取值口径与读表逻辑一一对应）
        /// </summary>
        /// <param name="worksheet">目标工作表</param>
        /// <param name="columnMap">列名 → 列索引映射</param>
        /// <param name="row">目标行号（1-based）</param>
        /// <param name="t">模板</param>
        private static void WriteTemplateRow(ExcelWorksheet worksheet, Dictionary<string, int> columnMap, int row, ObjectTemplate t)
        {
            // 列清单与取值口径统一来自 MapTemplateColumnSchema（与读表同源）
            foreach (var def in MapTemplateColumnSchema.All)
            {
                WriteCell(worksheet, columnMap, def.Name, row, def.Read(t));
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
        /// 补齐模板模型全集所需列：表头已存在的列跳过，缺失列在表尾新建第2行列名、第3行类型。
        /// </summary>
        private static void EnsureRequiredColumns(ExcelWorksheet worksheet, Dictionary<string, int> columnMap)
        {
            int nextCol = (worksheet.Dimension?.Columns ?? 0) + 1;
            foreach (var def in MapTemplateColumnSchema.All)
            {
                if (!def.EnsureWhenMissing) continue;

                int existing;
                if (columnMap.TryGetValue(def.Name, out existing)) continue;

                worksheet.Cells[2, nextCol].Value = def.Name;
                worksheet.Cells[3, nextCol].Value = def.TypeHint;
                columnMap[def.Name] = nextCol;
                nextCol++;
            }
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
