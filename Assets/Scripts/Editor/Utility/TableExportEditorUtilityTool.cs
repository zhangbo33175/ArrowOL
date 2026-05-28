/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TableExportEditorUtilityTool.cs
 * author:    云毅
 * created:   2026
 * descrip:   编辑器工具 - 文本/字符集/TXT 导出工具（合并/分文件/去重）
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 编辑器模式下 配置表/文本数据 导出工具类
    /// 功能：导出字符串到 TXT 文件，支持合并导出 / 分文件导出、自动去重、删除旧文件
    /// 主要用于：导出文本、字库字符集、配置表文本
    /// </summary>
    public static class TableExportEditorUtilityTool
    {
        //=========================================================================
        // 通用 TXT 导出
        //=========================================================================
        #region Common TXT Export
        /// <summary>
        /// 导出字符串数据到 TXT 文件（通用版）
        /// </summary>
        /// <param name="exportDirectory">导出目标文件夹</param>
        /// <param name="mulToOne">是否合并为一个文件</param>
        /// <param name="exportOneFileName">合并后的文件名</param>
        /// <param name="assetChars">文件名 -> 字符串列表</param>
        public static bool CreateTableExportTxt(string exportDirectory, bool mulToOne, string exportOneFileName,
            Dictionary<string, List<string>> assetChars)
        {
            // 路径为空校验
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTableExportTxt 时传入的路径为空");
                return false;
            }

            // 提取 Assets 内部相对路径
            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.txt";

            // 删除旧的合并文件
            if (File.Exists(oneFilePath))
            {
                Log.Info($"CreateTableExportTxt 删除旧的TMP字符集文件{oneFilePath}");
                File.Delete(oneFilePath);
            }

            // 删除所有旧的分文件
            foreach (var item in assetChars)
            {
                string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                if (File.Exists(perFilePath))
                {
                    Log.Info($"CreateTableExportTxt 删除旧的TMP字符集文件{perFilePath}");
                    File.Delete(perFilePath);
                }
            }

            // 合并导出为一个文件
            if (mulToOne)
            {
                string tmpChars = "";
                foreach (var item in assetChars)
                {
                    // 字符串去重
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars += string.Join("", assetStringArray);
                }

                // 拼接完整磁盘路径
                var filePath = AorTxt.Format("{0}/{1}",
                    Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), oneFilePath);

                // UTF8 无BOM 写入
                File.WriteAllText(filePath, tmpChars, new UTF8Encoding(false));
                AssetDatabase.Refresh();
            }
            // 分别导出多个文件
            else
            {
                foreach (var item in assetChars)
                {
                    string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    string tempStr = string.Join("", assetStringArray);

                    var filePath = AorTxt.Format("{0}/{1}",
                        Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), perFilePath);

                    File.WriteAllText(filePath, tempStr, new UTF8Encoding(false));
                }

                AssetDatabase.Refresh();
            }

            return true;
        }
        #endregion

        //=========================================================================
        // 文本专用 TXT 导出
        //=========================================================================
        #region Text Only TXT Export
        /// <summary>
        /// 导出字符串数据到 TXT 文件（文本专用版）
        /// 逻辑与上一方法完全一致，仅做方法区分
        /// </summary>
        /// <param name="exportDirectory">导出目录</param>
        /// <param name="mulToOne">是否合并导出</param>
        /// <param name="exportOneFileName">合并文件名</param>
        /// <param name="assetChars">文件名字符串映射</param>
        public static bool CreateTableExportTextTxt(string exportDirectory, bool mulToOne, string exportOneFileName,
            Dictionary<string, List<string>> assetChars)
        {
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTableExportTextTxt 时传入的路径为空");
                return false;
            }

            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.txt";

            if (File.Exists(oneFilePath))
            {
                Log.Info($"CreateTableExportTextTxt 删除旧的TMP字符集文件{oneFilePath}");
                File.Delete(oneFilePath);
            }

            foreach (var item in assetChars)
            {
                string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                if (File.Exists(perFilePath))
                {
                    Log.Info($"CreateTableExportTextTxt 删除旧的TMP字符集文件{perFilePath}");
                    File.Delete(perFilePath);
                }
            }

            if (mulToOne)
            {
                string tmpChars = "";
                foreach (var item in assetChars)
                {
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars = $"{tmpChars}{string.Join("", assetStringArray)}";
                }

                var filePath = AorTxt.Format("{0}/{1}",
                    Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), oneFilePath);
                File.WriteAllText(filePath, tmpChars, new UTF8Encoding(false));
                AssetDatabase.Refresh();
            }
            else
            {
                foreach (var item in assetChars)
                {
                    string perFilePath = $"{assetFilePath}/{item.Key}.txt";
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    string tempStr = string.Join("", assetStringArray);

                    var filePath = AorTxt.Format("{0}/{1}",
                        Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length), perFilePath);
                    File.WriteAllText(filePath, tempStr, new UTF8Encoding(false));
                }

                AssetDatabase.Refresh();
            }

            return true;
        }
        #endregion
    }
}