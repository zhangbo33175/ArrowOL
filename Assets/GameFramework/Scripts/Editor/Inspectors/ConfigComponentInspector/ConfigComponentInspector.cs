/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置组件编辑器
 *            一键导出 Excel → 加密 JsonBytes，支持查看、打开、定位
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Honor.Runtime;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【配置组件编辑器】
    /// 功能：提供配置表Excel的一键导出、查看、打开、文件夹定位
    /// 作用：负责将策划Excel → 加密JsonBytes，供游戏运行时读取
    /// </summary>
    [CustomEditor(typeof(ConfigComponent))]
    public class ConfigComponentInspector : HonorComponentInspector
    {
        #region 【字段定义】
        /// <summary>
        /// 配置表数据缓存
        /// <para>Key = 配置项名称；Value = [0]开发环境值 / [1]生产环境值</para>
        /// </summary>
        private Dictionary<string, List<string>> m_ConfigEntryMap;
        #endregion

        #region 【编辑器生命周期】
        /// <summary>
        /// 编辑器启用回调
        /// <para>初始化配置数据缓存容器</para>
        /// </summary>
        private void OnEnable()
        {
            m_ConfigEntryMap = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// 绘制 Inspector 面板
        /// <para>提供 Excel 打开/导出/定位按钮，并预览开发与生产环境配置</para>
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            DrawActionButtons();

            // ====================== 配置信息预览区 ======================
            if (ReadConfigEntries())
            {
                // 开发环境与生产环境两列对照展示
                DrawConfigColumn("开发环境配置", 0);
                DrawConfigColumn("生产环境配置", 1);
            }
            else
            {
                // 配置文件不存在提示
                EditorGUILayout.HelpBox("配置文件不存在，请检查是否已生成。", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
        #endregion

        #region 【按钮区绘制】
        /// <summary>
        /// 绘制功能按钮行：打开 Excel / 导出 JsonBytes / 定位文件夹
        /// </summary>
        private void DrawActionButtons()
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                DrawActionButton("打开配置表Excel",
                    () => TableExportEditorUtility.OpenExcel(GamePathUtils.Config.GetExcelFileFullPath()));

                DrawActionButton("导出配置表Excel到Json",
                    () => ExportCurrentExcel());

                DrawActionButton("打开配置表Excel所在文件夹",
                    () => TableExportEditorUtility.OpenDirectory(GamePathUtils.Config.GetExcelRootDirectoryFullPath()));
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制单个功能按钮，点击后执行回调并退出本次 GUI 事件
        /// </summary>
        /// <param name="buttonText">按钮显示文案</param>
        /// <param name="clickAction">点击时执行的业务动作</param>
        private static void DrawActionButton(string buttonText, Action clickAction)
        {
            if (!GUILayout.Button(buttonText))
            {
                return;
            }

            clickAction();
            GUIUtility.ExitGUI();
        }

        /// <summary>
        /// 导出当前配置表 Excel 为加密 JsonBytes
        /// </summary>
        private void ExportCurrentExcel()
        {
            string excelFilePath = GamePathUtils.Config.GetExcelFileFullPath();
            string excelName = Path.GetFileNameWithoutExtension(excelFilePath);
            ExportExcelConfigToBytes(excelName);
        }
        #endregion

        #region 【配置预览绘制】
        /// <summary>
        /// 绘制单列环境配置预览（一个纵向 box 内逐行列出所有配置项）
        /// </summary>
        /// <param name="columnTitle">环境列标题</param>
        /// <param name="envIndex">值列表中环境值的下标（0=开发 / 1=生产）</param>
        private void DrawConfigColumn(string columnTitle, int envIndex)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(columnTitle);
            foreach (KeyValuePair<string, List<string>> entry in m_ConfigEntryMap)
            {
                EditorGUILayout.LabelField(entry.Key, entry.Value[envIndex]);
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 【配置读取】
        /// <summary>
        /// 读取加密配置表
        /// <para>从 Configs.bytes 读取并解密，解析成键值对供面板显示</para>
        /// </summary>
        /// <returns>配置文件存在且读取成功返回 true，否则返回 false</returns>
        private bool ReadConfigEntries()
        {
            m_ConfigEntryMap.Clear();

            // 配置文件路径
            string configBytesPath = AorTxt.Format("{0}/{1}/{2}",
                Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                GamePathUtils.Json.GetRootDirectoryRelativePath(),
                "Configs.bytes");

            if (!File.Exists(configBytesPath))
            {
                return false;
            }

            // 读取字节 -> 解密 -> 转字符串
            byte[] rawBytes = File.ReadAllBytes(configBytesPath);
            byte[] decryptedBytes = Encryption.GetQuickXorBytes(rawBytes, ConfigComponent.s_ConfigEncrytionKey);
            string jsonContent = Converter.GetString(decryptedBytes);

            // 解析Json结构，按配置项聚合开发/生产两个环境的值
            JObject configJson = JObject.Parse(jsonContent);
            foreach (KeyValuePair<string, JToken> configEntry in configJson)
            {
                List<string> envValues = new List<string>();
                foreach (JToken envToken in configEntry.Value)
                {
                    envValues.Add(envToken.ToString());
                }

                m_ConfigEntryMap[configEntry.Key] = envValues;
            }

            return true;
        }
        #endregion

        #region 【配置导出】
        /// <summary>
        /// 将 Excel 导出为加密配置 Bytes
        /// <para>读取策划配置 Excel，导出为加密的 Configs.bytes</para>
        /// </summary>
        /// <param name="excelName">配置表文件名（不含扩展名）</param>
        /// <returns>导出成功返回 true</returns>
        private bool ExportExcelConfigToBytes(string excelName)
        {
            // 1. 读取Excel文件内容
            string excelPath = $"{GamePathUtils.Config.GetExcelRootDirectoryFullPath()}/{excelName}.xlsm";
            DataSet excelDataSet = TableExportEditorUtility.GetExcelData(excelPath);

            // 2. 输出目标路径
            string outputDirectory = GamePathUtils.Json.GetRootDirectoryFullPath();
            string outputFilePath = $"{outputDirectory}/{excelName}.bytes";

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // 3. 拼接Json格式字符串（固定格式：key: [开发值, 生产值]）
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.AppendLine("{");

            int rowCount = excelDataSet.Tables[0].Rows.Count;
            for (int rowIndex = 4; rowIndex < rowCount; rowIndex++) // 第5行开始是真实配置
            {
                string rowText = BuildConfigRow(excelDataSet.Tables[0].Rows[rowIndex]);
                if (rowIndex < rowCount - 1)
                {
                    rowText += ",";
                }

                jsonBuilder.AppendLine(rowText);
            }

            jsonBuilder.AppendLine("}");

            // 4. 写入文件并加密
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            byte[] plainBytes = Converter.GetBytesByString(jsonBuilder.ToString());
            byte[] encryptedBytes = Encryption.GetQuickXorBytes(plainBytes, ConfigComponent.s_ConfigEncrytionKey);
            File.WriteAllBytes(outputFilePath, encryptedBytes);

            // 刷新Unity
            AssetDatabase.Refresh();
            return true;
        }

        /// <summary>
        /// 将 Excel 数据行格式化为一行 Json 配置文本
        /// </summary>
        /// <param name="dataRow">Excel 中的一行数据</param>
        /// <returns>形如 "key":["dev","prod"] 的 Json 片段</returns>
        private static string BuildConfigRow(DataRow dataRow)
        {
            return $"    \"{dataRow[1]}\":[\"{dataRow[3]}\",\"{dataRow[4]}\"]";
        }
        #endregion
    }
}
