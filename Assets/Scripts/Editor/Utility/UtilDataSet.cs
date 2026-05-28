/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UtilDataSet.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置表解析工具 - DataSet 转强类型实体 List<T>
 ***************************************************************/

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Honor.Runtime;

namespace Editor.MapEditor
{
    /// <summary>
    /// DataSet 扩展工具类
    /// 用于 Excel 配置表解析：自动将 DataSet 转为强类型实体列表
    /// </summary>
    public static class UtilDataSet
    {
        //=========================================================================
        // DataSet 转实体列表
        //=========================================================================
        #region DataSet To List<T>
        /// <summary>
        /// DataSet 转换为 C# 对象列表
        /// 用途：将配置表DataSet（Excel导出）自动解析为强类型List<T>
        /// 表格规则：第2行=字段名，第3行=字段类型，第5行开始=真实数据
        /// </summary>
        /// <param name="dataSet">配置表数据集</param>
        /// <typeparam name="T">目标实体类型</typeparam>
        /// <returns>解析后的对象列表</returns>
        public static List<T> ToList<T>(this DataSet dataSet) where T : class, new()
        {
            List<T> list = new List<T>();

            // 数据合法性校验：空/无表/无有效数据（行数<4说明没有配置内容）
            if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count < 4)
                return list;

            DataTable table = dataSet.Tables[0];
            int columnCount = table.Columns.Count; // 总列数
            int rowCount = table.Rows.Count; // 总行数

            string[] fieldNames = new string[columnCount]; // 字段名数组（第2行）
            string[] fieldTypes = new string[columnCount]; // 字段类型数组（第3行）

            // 读取配置表结构：第2行=字段名，第3行=字段类型
            for (int col = 0; col < columnCount; col++)
            {
                fieldNames[col] = table.Rows[1][col].ToString();
                fieldTypes[col] = table.Rows[2][col].ToString();

                // 有字段名但无类型 → 配置表错误
                if (!string.IsNullOrEmpty(fieldNames[col]) && string.IsNullOrEmpty(fieldTypes[col]))
                {
                    Log.Error($"DataSet 解析错误：字段【{fieldNames[col]}】未配置类型");
                    return list;
                }
            }

            // 读取真实数据：第5行开始（i=4）是有效配置数据
            for (int row = 4; row < rowCount; row++)
            {
                T instance = new T();

                // 遍历每一列，反射赋值给对象字段
                for (int col = 0; col < columnCount; col++)
                {
                    string fieldName = fieldNames[col];
                    string fieldType = fieldTypes[col];

                    // 反射获取T类中的对应字段
                    FieldInfo fieldInfo = typeof(T).GetField(fieldName);
                    if (fieldInfo == null) continue;

                    // 读取单元格字符串值
                    string cellValue = table.Rows[row][col].ToString();

                    // 根据配置表类型进行对应解析赋值
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        if (fieldType == "string")
                        {
                            fieldInfo.SetValue(instance, cellValue);
                        }
                        else if (fieldType is "number" or "int")
                        {
                            fieldInfo.SetValue(instance, int.Parse(cellValue));
                        }
                        else
                        {
                            // 默认直接赋值字符串
                            fieldInfo.SetValue(instance, cellValue);
                        }
                    }
                }

                list.Add(instance);
            }

            return list;
        }
        #endregion
    }
}