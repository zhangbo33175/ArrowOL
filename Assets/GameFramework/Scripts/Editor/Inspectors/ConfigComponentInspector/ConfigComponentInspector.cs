/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigComponentInspector.cs
 * author:    云毅
 *  created:   2026
 * descrip:   配置组件编辑器
 *            一键导出 Excel → 加密 JsonBytes，支持查看、打开、定位
 ***************************************************************/
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
        //=========================================================================
        // 配置表数据缓存
        // Key = 配置项名称
        // Value = [0]开发环境值 / [1]生产环境值
        //=========================================================================
        private Dictionary<string, List<string>> m_ConfigDatas;
        #endregion

        #region 【编辑器生命周期】
        //=========================================================================
        // 启用时初始化数据容器
        //=========================================================================
        private void OnEnable()
        {
            m_ConfigDatas = new Dictionary<string, List<string>>();
        }

        //=========================================================================
        // 绘制Inspector面板
        //=========================================================================
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            // ====================== 功能按钮区 ======================
            EditorGUILayout.BeginHorizontal("box");
            {
                // 打开配置表Excel
                if (GUILayout.Button("打开配置表Excel"))
                {
                    TableExportEditorUtility.OpenExcel(GamePathUtils.Config.GetExcelFileFullPath());
                    GUIUtility.ExitGUI();
                }

                // 将Excel导出为加密JsonBytes
                if (GUILayout.Button("导出配置表Excel到Json"))
                {
                    string filePath = GamePathUtils.Config.GetExcelFileFullPath();
                    ExportExcelConfigToBytes(Path.GetFileNameWithoutExtension(filePath));
                    GUIUtility.ExitGUI();
                }

                // 打开Excel所在文件夹
                if (GUILayout.Button("打开配置表Excel所在文件夹"))
                {
                    TableExportEditorUtility.OpenDirectory(GamePathUtils.Config.GetExcelRootDirectoryFullPath());
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndHorizontal();

            // ====================== 配置信息预览区 ======================
            if (ReadConfigDatas())
            {
                // 显示开发环境配置
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("开发环境配置");
                foreach (var itr in m_ConfigDatas)
                {
                    EditorGUILayout.LabelField(itr.Key, itr.Value[0]);
                }
                EditorGUILayout.EndVertical();

                // 显示生产环境配置
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("生产环境配置");
                foreach (var itr in m_ConfigDatas)
                {
                    EditorGUILayout.LabelField(itr.Key, itr.Value[1]);
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                // 配置文件不存在提示
                EditorGUILayout.HelpBox("配置文件不存在，请检查是否已生成。", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        //=========================================================================
        // 编译开始回调
        //=========================================================================
        protected override void OnCompileStart()
        {
            base.OnCompileStart();
        }

        //=========================================================================
        // 编译完成回调
        //=========================================================================
        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
        }
        #endregion

        #region 【配置读取】
        //=========================================================================
        // 【读取加密配置表】
        // 从 Configs.bytes 读取并解密，解析成键值对供面板显示
        //=========================================================================
        private bool ReadConfigDatas()
        {
            m_ConfigDatas.Clear();

            // 配置文件路径
            string jsonFilePath = AorTxt.Format("{0}/{1}/{2}",
                Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                GamePathUtils.Json.GetRootDirectoryRelativePath(),
                "Configs.bytes");

            if (File.Exists(jsonFilePath))
            {
                // 读取字节 -> 解密 -> 转字符串
                byte[] contentBytes = File.ReadAllBytes(jsonFilePath);
                byte[] bytes = Encryption.GetQuickXorBytes(contentBytes, ConfigComponent.s_ConfigEncrytionKey);
                string content = Converter.GetString(bytes);

                // 解析Json结构
                JObject jObject = JObject.Parse(content);
                foreach (var configItr in jObject)
                {
                    if (!m_ConfigDatas.ContainsKey(configItr.Key))
                    {
                        List<string> configData = new List<string>();
                        m_ConfigDatas.Add(configItr.Key, configData);
                    }

                    // 读取开发/生产两个环境的值
                    foreach (JToken elementItr in configItr.Value)
                    {
                        m_ConfigDatas[configItr.Key].Add(elementItr.ToString());
                    }
                }

                return true;
            }

            return false;
        }
        #endregion

        #region 【配置导出】
        //=========================================================================
        // 【Excel → 加密Bytes】
        // 读取策划配置Excel，导出为加密的 Configs.bytes
        //=========================================================================
        private bool ExportExcelConfigToBytes(string openExcelNamePre)
        {
            // 1. 读取Excel文件内容
            string excelPath = $"{GamePathUtils.Config.GetExcelRootDirectoryFullPath()}/{openExcelNamePre}.xlsm";
            DataSet result = TableExportEditorUtility.GetExcelData(excelPath);

            // 2. 输出目标路径
            string strSubJsonDirectoryPath = GamePathUtils.Json.GetRootDirectoryFullPath();
            string strFilePathList = $"{strSubJsonDirectoryPath}/{openExcelNamePre}.bytes";

            if (!Directory.Exists(strSubJsonDirectoryPath))
            {
                Directory.CreateDirectory(strSubJsonDirectoryPath);
            }

            // 3. 拼接Json格式字符串（固定格式：key: [开发值, 生产值]）
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");

            int rows = result.Tables[0].Rows.Count;
            for (int i = 4; i < rows; i++) // 第5行开始是真实配置
            {
                string thisRow = $"    \"{result.Tables[0].Rows[i][1]}\":[\"{result.Tables[0].Rows[i][3]}\",\"{result.Tables[0].Rows[i][4]}\"]";

                if (i < rows - 1)
                {
                    thisRow += ",";
                }

                stringBuilder.AppendLine(thisRow);
            }

            stringBuilder.AppendLine("}");

            // 4. 写入文件并加密
            if (File.Exists(strFilePathList))
            {
                File.Delete(strFilePathList);
            }

            byte[] fileBytes = Converter.GetBytesByString(stringBuilder.ToString());
            byte[] encryptBytes = Encryption.GetQuickXorBytes(fileBytes, ConfigComponent.s_ConfigEncrytionKey);
            File.WriteAllBytes(strFilePathList, encryptBytes);

            // 刷新Unity
            AssetDatabase.Refresh();
            return true;
        }
        #endregion
    }
}