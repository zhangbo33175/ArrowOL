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
    [CustomEditor(typeof(ConfigComponent))]
    public class ConfigComponentInspector: HonorComponentInspector
    {
         private Dictionary<string, List<string>> m_ConfigDatas;

        private void OnEnable()
        {
            m_ConfigDatas = new Dictionary<string, List<string>>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("打开配置表Excel"))
            {
                TableExportEditorUtility.OpenExcel(GamePathUtils.Config.GetExcelFileFullPath());
                GUIUtility.ExitGUI();
            }

            // Excel导出（导出Configs目录下的Excel到Json）
            if (GUILayout.Button("导出配置表Excel到Json"))
            {
                string filePath = GamePathUtils.Config.GetExcelFileFullPath();
                ExportExcelConfigToBytes(Path.GetFileNameWithoutExtension(filePath));

                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开配置表Excel所在文件夹"))
            {
                TableExportEditorUtility.OpenDirectory(GamePathUtils.Config.GetExcelRootDirectoryFullPath());
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndHorizontal();

            if(ReadConfigDatas())
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("开发环境配置");
                foreach (var itr in m_ConfigDatas)
                {
                    EditorGUILayout.LabelField(itr.Key, itr.Value[0].ToString());
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("生产环境配置");
                foreach (var itr in m_ConfigDatas)
                {
                    EditorGUILayout.LabelField(itr.Key, itr.Value[1].ToString());
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("配置文件不存在，请检查是否已生成。", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileStart()
        {
            base.OnCompileStart();

        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

        }

        /// <summary>
        /// 读取Config数据集合
        /// </summary>
        /// <returns>是否成功</returns>
        private bool ReadConfigDatas()
        {
            m_ConfigDatas.Clear();
            string jsonFilePath = AorTxt.Format("{0}/{1}/{2}", Application.dataPath.Substring(0, Application.dataPath.Length- "Assets".Length), GamePathUtils.Json.GetRootDirectoryRelativePath(), "Configs.bytes");
            if (File.Exists(jsonFilePath))
            {
                byte[] contentBytes = File.ReadAllBytes(jsonFilePath);
                byte[] bytes = Encryption.GetQuickXorBytes(contentBytes, ConfigComponent.s_ConfigEncrytionKey);
                string content = Converter.GetString(bytes);
                JObject jObject = JObject.Parse(content);
                foreach (var configItr in jObject)
                {
                    if (!m_ConfigDatas.ContainsKey(configItr.Key))
                    {
                        var configData = new List<string>();
                        m_ConfigDatas.Add(configItr.Key, configData);
                    }
                    foreach (JToken elementItr in configItr.Value)
                    {
                        m_ConfigDatas[configItr.Key].Add(elementItr.ToString());
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从Excel中导出到bytes（json加密后存为bytes文件）
        /// </summary>
        /// <param name="openExcelNamePre"></param>
        /// <returns></returns>
        private bool ExportExcelConfigToBytes(string openExcelNamePre)
        {
            // 读取excel的内容
            string excelPath = $"{GamePathUtils.Config.GetExcelRootDirectoryFullPath()}/{openExcelNamePre}.xlsm";
            DataSet result = TableExportEditorUtility.GetExcelData(excelPath);

            // 要写入的bytes文件根路径
            string strSubJsonDirectoryPath = GamePathUtils.Json.GetRootDirectoryFullPath();
            string strFilePathList = strSubJsonDirectoryPath + "/" + openExcelNamePre + ".bytes";
            if(!Directory.Exists(strSubJsonDirectoryPath))
            {
                Directory.CreateDirectory(strSubJsonDirectoryPath);
            }

            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");

            int rows = result.Tables[0].Rows.Count;
            for (int i = 4; i < rows; i++)
            {
                string thisRow = "    \"" + result.Tables[0].Rows[i][1].ToString() + "\":[\"" + result.Tables[0].Rows[i][3].ToString() + "\",\"" + result.Tables[0].Rows[i][4].ToString() + "\"]";
                if (i < rows - 1)
                {
                    thisRow = thisRow + ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            stringBuilder.AppendLine("}");
            if (File.Exists(strFilePathList))
            {
                File.Delete(strFilePathList);
            }
            File.WriteAllBytes(strFilePathList, Encryption.GetQuickXorBytes(Converter.GetBytesByString(stringBuilder.ToString()), ConfigComponent.s_ConfigEncrytionKey));

            AssetDatabase.Refresh();
            return true;
        }
    }
}