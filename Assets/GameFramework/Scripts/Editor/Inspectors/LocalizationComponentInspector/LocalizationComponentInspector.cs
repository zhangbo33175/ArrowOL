using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using GameLib;
using Honor.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(LocalizationComponent))]
    internal sealed partial class LocalizationComponentInspector : HonorComponentInspector
    {
        // 从本地文件中获取配置参数
        private JObject m_TMPExportConfig = null;

        // 导出TMP页面配置数据
        private bool m_UseTMP = false;
        private bool m_MulLangToOne = false;

        private string m_TMPCharsSaveDirectory = "";

        // 从component中获取的配置数据
        private SerializedProperty m_Language = null;
        private SerializedProperty m_AutoFontAdapt = null;

        /// <summary>
        /// 自定义表格需要排除的表格路径集合
        /// </summary>
        private List<string> m_ExcludeExcelsPathsForCustoms = null;

        /// <summary>
        /// 是否导出lua增量文件
        /// </summary>
        public bool m_EnableExportLuaIncrease = false;

        /// <summary>
        /// 导出增量lua文件的时候只保留一个版本
        /// </summary>
        public bool m_EnableExportLuaIncreaseSingleVersion = false;

        /// <summary>
        /// 导出增量lua文件的自定义路径
        /// </summary>
        public string m_ExportLuaIncreasePath = null;

        /// <summary>
        /// 所有多语言key
        /// </summary>
        private static List<string> m_AllKey = new List<string>();

        /// <summary>
        /// 重复key筛查
        /// </summary>
        private static List<string> m_RepeatKey = new List<string>();

        protected void OnEnable()
        {
            m_Language = serializedObject.FindProperty("m_Language");
            m_AutoFontAdapt = serializedObject.FindProperty("m_AutoFontAdapt");

            // 读取本地配置并反序列化展示
            m_TMPExportConfig = LoadTMPExportConfig();
            m_UseTMP = (bool)m_TMPExportConfig["UseTMP"];
            m_MulLangToOne = (bool)m_TMPExportConfig["MulLangToOne"];
            m_TMPCharsSaveDirectory = (string)m_TMPExportConfig["TMPCharsSaveDirectory"];

            m_ExcludeExcelsPathsForCustoms = new List<string>();
            m_ExcludeExcelsPathsForCustoms.Add(EditorPath.Localization.ExcelFileFullPath);
            m_ExcludeExcelsPathsForCustoms.Add(EditorPath.Localization.ExcelDefaultFileFullPath);
            m_ExcludeExcelsPathsForCustoms.Add(EditorPath.Localization.ExcelFontFileFullPath);

            LoadLocalizationSettings(out m_EnableExportLuaIncrease, out m_EnableExportLuaIncreaseSingleVersion,
                out m_ExportLuaIncreasePath);
        }

        /// <summary>
        /// 加载本地化配置信息
        /// </summary>
        /// <param name="enableExportLuaIncrease"></param>
        /// <param name="enableExportLuaIncreaseSingleVersion"></param>
        /// <param name="exportLuaIncreasePath"></param>
        public static void LoadLocalizationSettings(out bool enableExportLuaIncrease,
            out bool enableExportLuaIncreaseSingleVersion, out string exportLuaIncreasePath)
        {
            enableExportLuaIncrease = false;
            enableExportLuaIncreaseSingleVersion = false;
            exportLuaIncreasePath = string.Empty;

            string content = File.ReadAllText(EditorPath.Editor.Localization.ProjectSettingFileFullPath);
            JObject jObject = JObject.Parse(content);
            if (jObject.ContainsKey("EnableExportLuaIncrease"))
            {
                enableExportLuaIncrease = (bool)jObject["EnableExportLuaIncrease"];
            }

            if (jObject.ContainsKey("EnableExportLuaIncreaseSingleVersion"))
            {
                enableExportLuaIncreaseSingleVersion = (bool)jObject["EnableExportLuaIncreaseSingleVersion"];
            }

            if (jObject.ContainsKey("ExportLuaIncreasePath"))
            {
                exportLuaIncreasePath = (string)jObject["ExportLuaIncreasePath"];
            }

            if (string.IsNullOrEmpty(exportLuaIncreasePath))
            {
                exportLuaIncreasePath = EditorPath.Localization.LuaIncreaseFolderName;
            }
        }

        /// <summary>
        /// 保存本地化配置信息
        /// </summary>
        /// <param name="enableExportLuaIncrease"></param>
        /// <param name="enableExportLuaIncreaseSingleVersion"></param>
        /// <param name="exportLuaIncreasePath"></param>
        public static void SaveLocalizationSettings(bool enableExportLuaIncrease,
            bool enableExportLuaIncreaseSingleVersion, string exportLuaIncreasePath)
        {
            JObject jObject = new JObject();
            jObject["EnableExportLuaIncrease"] = enableExportLuaIncrease;
            jObject["EnableExportLuaIncreaseSingleVersion"] = enableExportLuaIncreaseSingleVersion;
            jObject["ExportLuaIncreasePath"] = exportLuaIncreasePath;
            File.WriteAllText(EditorPath.Editor.Localization.ProjectSettingFileFullPath, jObject.ToString());
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            string codeName = ((GameDefinitions.Language)m_Language.enumValueIndex).ToString();
            EditorGUILayout.LabelField("当前语言类型",
                AorTxt.Format("{0}({1})", codeName, GameDefinitions.LanguageDesc[m_Language.enumValueIndex]));

            List<string> luaFileNames = new List<string>();
            bool hasLuaLocalizationsFile = false;

            string[] luaFileFullPaths = Directory.GetFiles(EditorPath.Localization.LuaScriptsFolderFullPath,
                "Localization*.lua.txt", SearchOption.AllDirectories);
            for (int index = 0; index < luaFileFullPaths.Length; index++)
            {
                string name1 = System.IO.Path.GetFileNameWithoutExtension(luaFileFullPaths[index]);
                string name = name1.Substring(0, name1.Length - ".lua".Length);
                if (!name.Equals("Localizations") && !name.Equals("LocalizationFonts") && !name.EndsWith("Increase") &&
                    !name.EndsWith("LocalizationSettingDataMgr"))
                {
                    luaFileNames.Add(name);
                }
                else
                {
                    hasLuaLocalizationsFile = true;
                }
            }

            bool canExport = true;

            if (GUILayout.Button("打开 ChatGPT-Translation 多语言批量翻译工具"))
            {
                //ChatGPTTranslationEditorWindow.Open();
                GUIUtility.ExitGUI();
            }

            // 获取自定义多语言表格路径集合
            List<string> customExcelsFullPaths =new List<string>(Directory.GetFiles(EditorPath.Localization.ExcelFolderFullPath, "*.xlsm",
                    SearchOption.TopDirectoryOnly));
            customExcelsFullPaths.RemoveAll(path =>
            {
                path = path.Replace("\\", "/");
                if (m_ExcludeExcelsPathsForCustoms.Contains(path) ||
                    Path.GetFileNameWithoutExtension(path).StartsWith("~"))
                {
                    return true;
                }

                return false;
            });

            // 添加多语言表格展示模块
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("打开多语言主表Excel"))
                    {
                        TableExportEditorUtility.OpenExcel(EditorPath.Localization.ExcelFileFullPath);
                        GUIUtility.ExitGUI();
                    }

                    // Excel导出（导出Localizations目录下的多语言表Excel到Lua）
                    EditorGUI.BeginDisabledGroup(!canExport);
                    if (GUILayout.Button("导出所有多语言表Excel到Lua（多合一）"))
                    {
                        m_AllKey.Clear();
                        m_RepeatKey.Clear();
                        CheckDefaultConfigKeyRepeat();
                        var increasePath =
                            $"{EditorPath.Localization.LuaScriptsFolderFullPath}/{m_ExportLuaIncreasePath}";
                        ExportAllExcelsLanguageToLua(customExcelsFullPaths, m_EnableExportLuaIncrease,
                            m_EnableExportLuaIncreaseSingleVersion, increasePath);
                        if (m_RepeatKey.Count > 0)
                        {
                            string res = "";
                            m_RepeatKey.ForEach(x => res += $"{x}、");
                            res = res.TrimEnd('、');
                            var message = $"重复key:{res}";
                            EditorUtility.DisplayDialog("检查到配置表Key重复", $"重复key:{res}", "确定");
                        }

                        GUIUtility.ExitGUI();
                    }

                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("打开多语言表Excel所在文件夹"))
                    {
                        TableExportEditorUtility.OpenDirectory(EditorPath.Localization.ExcelFolderFullPath);
                        GUIUtility.ExitGUI();
                    }
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                {
                    bool enableExportLuaIncrease =
                        EditorGUILayout.ToggleLeft($"增量导出Excel到Lua - 对比现有Excel与上次Lua,增量生成需要更新的多语言配置到Lua",
                            m_EnableExportLuaIncrease);
                    if (enableExportLuaIncrease != m_EnableExportLuaIncrease)
                    {
                        m_EnableExportLuaIncrease = enableExportLuaIncrease;
                        SaveLocalizationSettings(m_EnableExportLuaIncrease, m_EnableExportLuaIncreaseSingleVersion,
                            m_ExportLuaIncreasePath);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    bool enableExportLuaIncreaseSingleVersion =
                        EditorGUILayout.ToggleLeft($"生成单版本增量Lua文件", m_EnableExportLuaIncreaseSingleVersion);
                    if (enableExportLuaIncreaseSingleVersion != m_EnableExportLuaIncreaseSingleVersion)
                    {
                        m_EnableExportLuaIncreaseSingleVersion = enableExportLuaIncreaseSingleVersion;
                        SaveLocalizationSettings(m_EnableExportLuaIncrease, m_EnableExportLuaIncreaseSingleVersion,
                            m_ExportLuaIncreasePath);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    if (m_EnableExportLuaIncrease)
                    {
                        EditorGUILayout.LabelField($"导出增量文件路径: {EditorPath.LuaScript.Game.FolderPath}",
                            new GUILayoutOption[] { GUILayout.Width(250) });
                        string exportLuaIncreasePath = EditorGUILayout.TextField(m_ExportLuaIncreasePath);
                        if (exportLuaIncreasePath != m_ExportLuaIncreasePath)
                        {
                            m_ExportLuaIncreasePath = exportLuaIncreasePath;
                            SaveLocalizationSettings(m_EnableExportLuaIncrease, m_EnableExportLuaIncreaseSingleVersion,
                                m_ExportLuaIncreasePath);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (customExcelsFullPaths.Count == 0)
                {
                    EditorGUILayout.LabelField("自定义多语言表名称列表：暂无。");
                }
                else
                {
                    EditorGUILayout.LabelField($"自定义多语言表名称列表（{customExcelsFullPaths.Count}）:");
                    for (int index = 0; index < customExcelsFullPaths.Count; index++)
                    {
                        string path = customExcelsFullPaths[index].Replace("\\", "/");
                        string name = System.IO.Path.GetFileNameWithoutExtension(path);
                        EditorGUILayout.BeginHorizontal("box");
                        {
                            EditorGUILayout.LabelField(name);
                            if (GUILayout.Button("打开Excel"))
                            {
                                TableExportEditorUtility.OpenExcel(path);
                                GUIUtility.ExitGUI();
                            }

                            if (GUILayout.Button("删除Excel"))
                            {
                                File.Delete(path);
                                GUIUtility.ExitGUI();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.LabelField("多语言类型数量", luaFileNames.Count.ToString());

                for (int index = 0; index < luaFileNames.Count; index++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.LabelField(luaFileNames[index]);
                        string name = luaFileNames[index].Substring("Localization".Length,
                            luaFileNames[index].Length - "Localization".Length);
                        int nameIndex =
                            (int)(GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), name);
                        string nameDesc = GameDefinitions.LanguageDesc[nameIndex];
                        EditorGUILayout.LabelField(nameDesc);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (!hasLuaLocalizationsFile)
                {
                    EditorGUILayout.HelpBox("缺少Localizations.lua脚本！请生成本地化脚本来修复此问题！", MessageType.Error);
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            string jsonRootDirPath = EditorPath.Json.FolderFullPath;
            List<string> jsonFileNames = new List<string>();

            if (Directory.Exists(jsonRootDirPath))
            {
                string[] jsonFileFullPaths = System.IO.Directory.GetFiles(jsonRootDirPath, "*.json");
                for (int index = 0; index < jsonFileFullPaths.Length; index++)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(jsonFileFullPaths[index]);
                    if (name.StartsWith("LocalizationDefault") && !name.Equals("LocalizationDefaultLanguages"))
                    {
                        jsonFileNames.Add(name);
                    }
                }
            }

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("打开默认多语言表Excel"))
                    {
                        TableExportEditorUtility.OpenExcel(EditorPath.Localization.ExcelDefaultFileFullPath);
                        GUIUtility.ExitGUI();
                    }

                    // Excel导出（导出Localizations目录下的默认多语言表Excel到Json）
                    EditorGUI.BeginDisabledGroup(!canExport);
                    if (GUILayout.Button("导出默认多语言表Excel到Json"))
                    {
                        ExportExcelDefaultLanguageToJson();
                        GUIUtility.ExitGUI();
                    }

                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("打开默认多语言表Excel所在文件夹"))
                    {
                        TableExportEditorUtility.OpenDirectory(EditorPath.Localization.ExcelFolderFullPath);
                        GUIUtility.ExitGUI();
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("默认多语言类型数量", jsonFileNames.Count.ToString());
                for (int index = 0; index < jsonFileNames.Count; index++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.LabelField(jsonFileNames[index]);
                        string name = jsonFileNames[index].Substring("LocalizationDefault".Length,
                            jsonFileNames[index].Length - "LocalizationDefault".Length);
                        int nameIndex =
                            (int)(GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), name);
                        string nameDesc = GameDefinitions.LanguageDesc[nameIndex];
                        EditorGUILayout.LabelField(nameDesc);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            List<string> fontLocalizationNames = new List<string>();
            List<string> fontNames = new List<string>();
            // 收集所有使用TMP的语言和TMP字体名称
            Dictionary<string, List<string>> TMPFontData = new Dictionary<string, List<string>>();
            List<string> TMPFontNameList = new List<string>();
            string jsonFilePath = AorTxt.Format("{0}/{1}/{2}",
                Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                GamePathUtils.Json.GetRootDirectoryRelativePath(), "LocalizationFonts.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                string content = System.IO.File.ReadAllText(jsonFilePath);
                JObject jsonData = JsonConvert.DeserializeObject<JObject>(content);
                foreach (var configName in jsonData)
                {
                    string assetName = string.Empty;
                    if (configName.Value != null)
                    {
                        JObject fontInfo = JObject.FromObject(configName.Value);
                        if (fontInfo.ContainsKey("AssetName0") &&
                            !string.IsNullOrEmpty(fontInfo["AssetName0"].ToString()))
                        {
                            assetName = fontInfo["AssetName0"].ToString();
                        }

                        if (fontInfo.ContainsKey("FontType") && fontInfo["FontType"].ToString() == "FontTMP")
                        {
                            // 当前语言使用TMP字体,遍历所有的Asset
                            for (int fonti = 0; fonti < 50; fonti++)
                            {
                                if (fontInfo.ContainsKey($"AssetName{fonti}") &&
                                    !string.IsNullOrEmpty(fontInfo[$"AssetName{fonti}"].ToString()))
                                {
                                    string fontName = fontInfo[$"AssetName{fonti}"].ToString();
                                    if (!TMPFontData.ContainsKey(fontName))
                                    {
                                        TMPFontData.Add(fontName, new List<string>());
                                        TMPFontNameList.Add(fontName);
                                    }

                                    if (!TMPFontData[fontName].Contains(configName.Key))
                                    {
                                        TMPFontData[fontName].Add(configName.Key);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (!fontLocalizationNames.Contains(configName.Key))
                    {
                        fontLocalizationNames.Add(configName.Key);
                        fontNames.Add(assetName);
                    }
                }
            }

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("打开字体表Excel"))
                    {
                        TableExportEditorUtility.OpenExcel(EditorPath.Localization.ExcelFontFileFullPath);
                        GUIUtility.ExitGUI();
                    }

                    // Excel导出（导出Localizations目录下的字体表Excel到Json）
                    if (GUILayout.Button("导出字体表Excel到Json"))
                    {
                        ExportExcelFontsToJson();
                        GUIUtility.ExitGUI();
                    }

                    if (GUILayout.Button("打开字体表Excel所在文件夹"))
                    {
                        TableExportEditorUtility.OpenDirectory(EditorPath.Localization.ExcelFolderFullPath);
                        GUIUtility.ExitGUI();
                    }
                }
                EditorGUILayout.EndHorizontal();

                m_AutoFontAdapt.boolValue = EditorGUILayout.Toggle("字体自动适配", m_AutoFontAdapt.boolValue);
                EditorGUILayout.LabelField("字体类型数量", fontLocalizationNames.Count.ToString());
                for (int index = 0; index < fontLocalizationNames.Count; index++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.LabelField(fontLocalizationNames[index]);
                        int nameIndex = (int)(GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language),
                            fontLocalizationNames[index]);
                        string nameDesc = GameDefinitions.LanguageDesc[nameIndex];
                        EditorGUILayout.LabelField(nameDesc, GUILayout.MaxWidth(120));
                        EditorGUILayout.LabelField("主字体：" + fontNames[index]);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            // 添加文字资源导出配置模块
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("box");
            {
                m_UseTMP = EditorGUILayout.ToggleLeft($"TextMeshPro（FontTMP）字符集生成", m_UseTMP);
                if (m_UseTMP != (bool)m_TMPExportConfig["UseTMP"])
                {
                    m_TMPExportConfig["UseTMP"] = m_UseTMP;
                    SaveTMPExportConfig(m_TMPExportConfig);
                }

                if (m_UseTMP)
                {
                    if (System.IO.File.Exists(jsonFilePath))
                    {
                        if (TMPFontNameList.Count == 0)
                        {
                            EditorGUILayout.LabelField("项目中没有使用TMP字体。");
                        }
                        else
                        {
                            // 配置导出文件夹
                            EditorGUILayout.BeginHorizontal("box");
                            EditorGUILayout.LabelField(string.IsNullOrEmpty(m_TMPCharsSaveDirectory)
                                ? $"文件夹位置：暂未设定。"
                                : $"文件夹位置：{m_TMPCharsSaveDirectory}");
                            if (GUILayout.Button("选择文件夹"))
                            {
                                m_TMPCharsSaveDirectory = EditorUtility.SaveFolderPanel("选择保存TMP字符集文件夹",
                                    Application.dataPath, string.Empty);
                                if (m_TMPCharsSaveDirectory != (string)m_TMPExportConfig["TMPCharsSaveDirectory"])
                                {
                                    m_TMPExportConfig["TMPCharsSaveDirectory"] = m_TMPCharsSaveDirectory;
                                    SaveTMPExportConfig(m_TMPExportConfig);
                                }

                                GUIUtility.ExitGUI();
                            }

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.HelpBox(
                                "勾选“字符集生成”选项：\n1.需配置导出路径，否则导出按钮无法使用。\n2.自动分析字体表中所有TMPFont，并列举出每个TMPFont对应的所有语种类型。\n3.导出时将自动搜集多语言表中所有匹配字符，去重后导出为字符集文件。",
                                MessageType.Info);

                            // 列举所有的TMPFont
                            for (int i = 0; i < TMPFontNameList.Count; i++)
                            {
                                // 展示TMPFont名称
                                EditorGUILayout.BeginVertical("box");

                                EditorGUILayout.LabelField($"[字体名称] {TMPFontNameList[i]}");

                                string langListContent = "[匹配语种] ";
                                List<string> langNames = TMPFontData[TMPFontNameList[i]];
                                for (int langIdx = 0; langIdx < langNames.Count; langIdx++)
                                {
                                    langListContent += langNames[langIdx] +
                                                       (langIdx == langNames.Count - 1 ? string.Empty : ", ");
                                }

                                EditorGUILayout.LabelField(langListContent);
                                if (langNames.Count > 0)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    if (GUILayout.Button("导出字符集(多语言合一)"))
                                    {
                                        ExprotTMPCharsForFont(TMPFontNameList[i],
                                            (string)m_TMPExportConfig["TMPCharsSaveDirectory"], langNames, true);
                                    }

                                    if (GUILayout.Button("导出字符集(多语言独立)"))
                                    {
                                        ExprotTMPCharsForFont(TMPFontNameList[i],
                                            (string)m_TMPExportConfig["TMPCharsSaveDirectory"], langNames, false);
                                    }

                                    EditorGUILayout.EndHorizontal();
                                }

                                EditorGUILayout.EndVertical();
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("配置TMP字符集需要先导出LocalizationFonts.json文件");
                    }
                }

                EditorGUILayout.EndVertical();
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
        /// 导出配置的字体表格到json
        /// </summary>
        /// <returns></returns>
        public static bool ExportExcelFontsToJson()
        {
            string openExcelNamePre =Path.GetFileNameWithoutExtension(EditorPath.Localization.ExcelFontFileFullPath);
            // 要打开的excel根路径
            string excelPath = $"{EditorPath.Localization.ExcelFolderFullPath}/{openExcelNamePre}.xlsm";
            // 要写入的json文件根路径
            string strSubJsonFilePath = EditorPath.Json.FolderFullPath;
            string strFilePathList = strSubJsonFilePath + "/" + openExcelNamePre + ".json";
            if (Directory.Exists(strSubJsonFilePath))
            {
                if (File.Exists(strFilePathList))
                {
                    File.Delete(strFilePathList);
                    File.Delete(strFilePathList + ".meta");
                }
            }
            else
            {
                Directory.CreateDirectory(strSubJsonFilePath);
            }

            return TableExportEditorUtility.ExportExcelToJson(excelPath, strFilePathList);
        }

        /// <summary>
        /// cs中使用的多语言转为json文件
        /// </summary>
        /// <returns></returns>
        public static bool ExportExcelDefaultLanguageToJson()
        {
            string openExcelNamePre =Path.GetFileNameWithoutExtension(EditorPath.Localization.ExcelDefaultFileFullPath);
            // 要打开的excel根路径
            string excelPath = $"{EditorPath.Localization.ExcelFolderFullPath}/{openExcelNamePre}.xlsm";
            DataSet result = TableExportEditorUtility.GetExcelData(excelPath);
            // 要写入的json文件夹路径
            string toJsonRootPath = EditorPath.Json.FolderFullPath;
            if (Directory.Exists(toJsonRootPath))
            {
                string[] files = Directory.GetFiles(toJsonRootPath, "LocalizationDefault*.json",
                    SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        File.Delete(file + ".meta");
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(toJsonRootPath);
            }

            // 所有多语言字符分类<文件名，asset内容>
            Dictionary<string, List<string>> assetFileDic = new Dictionary<string, List<string>>();

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            string[] keyList = new string[columns];
            string[] typeList = new string[columns];
            keyList[0] = "";
            typeList[0] = "";
            for (int excleCol = 1; excleCol < columns; excleCol++)
            {
                keyList[excleCol] = result.Tables[0].Rows[1][excleCol].ToString();
                typeList[excleCol] = result.Tables[0].Rows[2][excleCol].ToString();
                // 添加asset中语种统计
                assetFileDic.Add(keyList[excleCol], new List<string>());

                if (keyList[excleCol] != string.Empty && typeList[excleCol] == string.Empty)
                {
                    Log.Error("" + openExcelNamePre + "  表格错误, " + keyList[excleCol] + " 没有设置类型。");
                    result = null;
                    return false;
                }
            }

            // 语种类型汇总文件
            StringBuilder stringBuilderMain = new StringBuilder();
            stringBuilderMain.Append("[");

            // 开始按照列转换多语言数据, 从第6行开始每列转出一个文件
            for (int excleCol = 6; excleCol < columns; excleCol++)
            {
                if (excleCol > 6)
                {
                    stringBuilderMain.Append(",");
                }

                stringBuilderMain.Append("\"" + keyList[excleCol] + "\"");

                // 单个导出对应语种的json文件
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("{");
                string thisRow = null;
                for (int excleRow = 4; excleRow < rows; excleRow++)
                {
                    thisRow = null;
                    if (result.Tables[0].Rows[excleRow][1] == null)
                    {
                        Log.Error("" + openExcelNamePre + "  表格错误, " + "第 " + (excleRow + 1) + " 行Name为null。");
                    }
                    else
                    {
                        if (typeList[excleCol] == "table")
                        {
                            thisRow = "    \"" + result.Tables[0].Rows[excleRow][1].ToString() + "\" : " +
                                      result.Tables[0].Rows[excleRow][excleCol].ToString();
                        }
                        else
                        {
                            thisRow = "    \"" + result.Tables[0].Rows[excleRow][1].ToString() + "\" : \"" +
                                      result.Tables[0].Rows[excleRow][excleCol].ToString() + "\"";
                        }

                        if (excleRow < rows - 1)
                        {
                            thisRow = thisRow + ",";
                        }
                    }

                    if (thisRow != null)
                    {
                        stringBuilder.AppendLine(thisRow);
                    }
                }

                // 每一列都会导出一个json文件
                stringBuilder.AppendLine("}");
                string jsonFile = toJsonRootPath + "/LocalizationDefault" + keyList[excleCol] + ".json";
                if (File.Exists(jsonFile))
                {
                    File.Delete(jsonFile);
                }

                File.WriteAllText(jsonFile, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
                Log.Debug($"{jsonFile} 转换完成。");
            }

            stringBuilderMain.AppendLine("]");
            string jsonFileMain = toJsonRootPath + "/LocalizationDefaultLanguages.json";
            if (File.Exists(jsonFileMain))
            {
                File.Delete(jsonFileMain);
            }

            File.WriteAllText(jsonFileMain, stringBuilderMain.ToString(), new System.Text.UTF8Encoding(false));

            AssetDatabase.Refresh();

            Log.Debug($"表格 {openExcelNamePre} 导出所有多语言Json文件完成。");
            return true;
        }

        /// <summary>
        /// lua文件头部组装
        /// </summary>
        private static void MakeLuaFileTitle(StringBuilder stringBuilder, string fileName, string keyName,
            string descrip)
        {
            stringBuilder
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("-- (c) copyright 2026 - 2030, Honor")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine(
                    "-- ----------------------------------------------------------------------------------------------------");
            if (string.IsNullOrEmpty(keyName))
            {
                stringBuilder.AppendLine(AorTxt.Format("-- filename:  {0}.lua", fileName));
            }
            else
            {
                stringBuilder.AppendLine(AorTxt.Format("-- filename:  LocalPart_{0}_{1}.lua", fileName, keyName));
            }

            stringBuilder.AppendLine(AorTxt.Format("-- descrip:   {0}", descrip))
                .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("");
        }

        /// <summary>
        /// 写入引导require文件
        /// </summary>
        private static void MakeLuaRequireFile(string mainExcelName, string tableDetailTidy, int columns,
            string[] keyList, string[] langInfo, Dictionary<string, string> exportFolderPaths)
        {
            // 首先统计所有的语种，注册到Localizations.lua中
            string strClassDef0 = "---@class " + mainExcelName + " @" + tableDetailTidy;
            string strClassDef1 = mainExcelName + " = {";
            StringBuilder stringBuilder = new StringBuilder();
            MakeLuaFileTitle(stringBuilder, mainExcelName, null, tableDetailTidy);
            stringBuilder.AppendLine(strClassDef0)
                .AppendLine(strClassDef1);
            for (int i = 6; i < columns; i++)
            {
                if (keyList[i] != "")
                {
                    string refStr = null;
                    // 添加注释
                    if (langInfo[i] == null)
                    {
                        refStr = "    ---@type " + mainExcelName + "." + keyList[i];
                    }
                    else
                    {
                        refStr = "    ---@type " + mainExcelName + "." + keyList[i] + " @" + langInfo[i];
                    }

                    if (refStr != null)
                    {
                        stringBuilder.AppendLine(refStr);
                    }

                    // 添加table注册代码
                    string tableRegStr = string.Format("    {0} = nil,", keyList[i]);
                    stringBuilder.AppendLine(tableRegStr);
                }
            }

            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---Lua层本地化语言表数据关联回调全局事件派发(由C#回调回来)(此处自动生成，请不要手动修改！)");
            stringBuilder.AppendLine("---@type fun():void");
            // 开始添加注册回调函数
            stringBuilder.AppendLine("function Relate_Localization_Table_Data()");
            int Index = 1;
            for (int i = 6; i < columns; i++)
            {
                if (keyList[i] != "")
                {
                    string requireSZ = $"require('Localization{keyList[i]}')";
                    var incrementalFile = FindIncrementalFile(keyList[i]);
                    if (incrementalFile != null && incrementalFile.Length > 0)
                    {
                        foreach (var fileName in incrementalFile)
                        {
                            requireSZ += $" require('{fileName}')";
                        }
                    }
                    string luaFun = Index == 1
                        ? $"    if tostring(GameMainRoot.Localization.LanguageName) == '{keyList[i]}' then if Localizations.{keyList[i]} == nil then {requireSZ} end Lang = Localizations['{keyList[i]}']"
                        : $"    elseif tostring(GameMainRoot.Localization.LanguageName) == '{keyList[i]}' then if Localizations.{keyList[i]} == nil then {requireSZ} end Lang = Localizations['{keyList[i]}']";

                    stringBuilder.AppendLine(luaFun);
                    Index++;
                }
            }

            stringBuilder.AppendLine("    end");
            stringBuilder.AppendLine("end");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Relate_Localization_Table_Data()");

            string luaRegFileName = exportFolderPaths["Main"] + "/Localizations.lua.txt";
            File.WriteAllText(luaRegFileName, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            stringBuilder.Clear();
        }

        /// <summary>
        /// 生成各个语言的lua文件
        /// </summary>
        private static void MakeLanguageLuaFile(int columns, string[] keyList,
            Dictionary<string, string> exportFolderPaths, string tableDetailTidy, string[] langInfo,
            List<string> fullPaths, List<string> fileNames)
        {
            // 提前清空历史生成的所有文件夹
            foreach (var itr in exportFolderPaths)
            {
                if (Directory.Exists(itr.Value))
                {
                    Directory.Delete(itr.Value, true);
                }

                Directory.CreateDirectory(itr.Value);
            }

            AssetDatabase.Refresh();

            // 根据每一种语言类型，依次遍历所有多语言表格，生成Part文件和LocalizationXXX文件
            for (int i = 6; i < columns; i++)
            {
                if (keyList[i] != string.Empty)
                {
                    string exportFolderPath = exportFolderPaths[keyList[i]] + "/Localization" + keyList[i];
                    if (!Directory.Exists(exportFolderPath))
                    {
                        Directory.CreateDirectory(exportFolderPath);
                    }

                    StringBuilder luaBuilder = null;
                    var isArabic = keyList[i] == GameDefinitions.Language.Arabic.ToString();
                    for (int index = 0; index < fullPaths.Count; index++)
                    {
                        luaBuilder = new StringBuilder();

                        MakeLuaFileTitle(luaBuilder, fileNames[index], keyList[i], $"{tableDetailTidy}: {langInfo[i]}");

                        if (fileNames[index].Equals("Localizations"))
                        {
                            luaBuilder.AppendLine(AorTxt.Format("---@class Localizations.{0} @{1}: {2}", keyList[i],
                                tableDetailTidy, langInfo[i]));
                        }
                        else
                        {
                            luaBuilder.AppendLine(AorTxt.Format("---@type Localizations.{0} @{1}: {2}", keyList[i],
                                tableDetailTidy, langInfo[i]));
                        }

                        var lowerKeyName = keyList[i].ToLower();
                        luaBuilder.AppendLine($"local {lowerKeyName} = Localizations.{keyList[i]}");
                        luaBuilder.AppendLine("");

                        // 生成内容
                        DataSet excelDatas = TableExportEditorUtility.GetExcelData(fullPaths[index]);
                        int rows = excelDatas.Tables[0].Rows.Count;
                        for (int row = 4; row < rows; row++)
                        {
                            string commonStr = null;
                            string langIDStr = null;
                            string valuseStr = null;
                            if (excelDatas.Tables[0].Rows[row][1] != null)
                            {
                                langIDStr = excelDatas.Tables[0].Rows[row][1].ToString();
                            }

                            if (excelDatas.Tables[0].Rows[row][2] != null)
                            {
                                commonStr = excelDatas.Tables[0].Rows[row][2].ToString();
                            }

                            if (excelDatas.Tables[0].Rows[row][i] != null)
                            {
                                valuseStr = excelDatas.Tables[0].Rows[row][i].ToString();
                            }

                            if (!string.IsNullOrEmpty(langIDStr))
                            {
                                string strDef = null;
                                if (i == 6)
                                {
                                    CollectAndCheckKeyRepeat(langIDStr);
                                }

                                if (!string.IsNullOrEmpty(commonStr))
                                {
                                    strDef = AorTxt.Format("---@field {0} string @{1}", langIDStr, commonStr);
                                }
                                else
                                {
                                    strDef = AorTxt.Format("---@field {0} string @ 注释缺失", langIDStr);
                                }

                                luaBuilder.AppendLine(strDef);
                            }

                            luaBuilder.AppendLine($"{lowerKeyName}.{langIDStr} = \"{valuseStr}\"");
                            luaBuilder.AppendLine("");
                        }

                        luaBuilder.AppendLine(AorTxt.Format("---@type Localizations.{0} @{1}: {2}", keyList[i],
                            tableDetailTidy, langInfo[i]));
                        luaBuilder.AppendLine($"Localizations.{keyList[i]} = {lowerKeyName}");
                        luaBuilder.AppendLine("");

                        string makingFilePath = $"{exportFolderPath}/LocalPart_{fileNames[index]}_{keyList[i]}.lua.txt";
                        File.WriteAllText(makingFilePath, luaBuilder.ToString(), new System.Text.UTF8Encoding(false));
                    }

                    // 生成LocalizationXXX文件
                    luaBuilder = new StringBuilder();
                    MakeLuaFileTitle(luaBuilder, $"Localization{keyList[i]}", null,
                        $"{tableDetailTidy}: {langInfo[i]}");
                    luaBuilder.AppendLine("Localizations." + keyList[i] + " = {}");
                    for (int index = 0; index < fileNames.Count; index++)
                    {
                        luaBuilder.AppendLine($"require('LocalPart_{fileNames[index]}_{keyList[i]}')");
                    }

                    string exportFilePath = $"{exportFolderPath}/Localization{keyList[i]}.lua.txt";
                    File.WriteAllText(exportFilePath, luaBuilder.ToString(), new System.Text.UTF8Encoding(false));
                    Log.Debug($"多语言文案 {keyList[i]} 导出成功。");
                }
            }
        }

        /// <summary>
        /// 检查默认表中是否有key重复
        /// </summary>
        private static void CheckDefaultConfigKeyRepeat()
        {
            DataSet excelDatas =
                TableExportEditorUtility.GetExcelData(EditorPath.Localization.ExcelDefaultFileFullPath);
            int rows = excelDatas.Tables[0].Rows.Count;
            for (int i = 4; i < rows; i++)
            {
                var key = excelDatas.Tables[0].Rows[i][1];
                if (key != null)
                {
                    CollectAndCheckKeyRepeat(key.ToString());
                }
            }
        }

        /// <summary>
        /// 导出Localizations中所有多语言到lua文件（主表+自定义表）
        /// </summary>
        /// <returns></returns>
        public static bool ExportAllExcelsLanguageToLua(List<string> customExcelsFullPaths,
            bool enableExportLuaIncrease = false, bool enableExportLuaIncreaseSingleVersion = false,
            string exportLuaIncreasePath = "")
        {
            if (!enableExportLuaIncrease) //增量导出lua变动不需要删除以前的lua导出配置
            {
                // 清理历史Table脚本
                List<string> oldFilesPaths = new List<string>();
                oldFilesPaths.AddRange(Directory.GetFiles(Application.dataPath, "Localization*.lua.txt",
                    SearchOption.AllDirectories));
                oldFilesPaths.AddRange(Directory.GetFiles(Application.dataPath, "LocalPart_*.lua.txt",
                    SearchOption.AllDirectories));
                foreach (var path in oldFilesPaths)
                {
                    File.Delete(path);
                }

                if (!string.IsNullOrEmpty(exportLuaIncreasePath) && Directory.Exists(exportLuaIncreasePath))
                {
                    Directory.Delete(exportLuaIncreasePath, true);
                    if (File.Exists($"{exportLuaIncreasePath}.meta"))
                    {
                        File.Delete($"{exportLuaIncreasePath}.meta");
                    }
                }

                AssetDatabase.Refresh();
            }

            // 收集所有多语言Excel表格的路径信息和文件名称集合
            List<string> fullPaths = new List<string>();
            List<string> fileNames = new List<string>();

            string mainExcelFullPath = EditorPath.Localization.ExcelFileFullPath;
            string mainExcelName = System.IO.Path.GetFileNameWithoutExtension(mainExcelFullPath);

            fullPaths.Add(mainExcelFullPath);
            fileNames.Add(mainExcelName);
            customExcelsFullPaths.ForEach(customExcelFullPath =>
            {
                customExcelFullPath = customExcelFullPath.Replace("\\", "/");
                string customExcelName = System.IO.Path.GetFileNameWithoutExtension(customExcelFullPath);
                fullPaths.Add(customExcelFullPath);
                fileNames.Add(customExcelName);
            });

            // 获取主表的表头信息
            DataSet mainExcelDatas = TableExportEditorUtility.GetExcelData(mainExcelFullPath);
            string tableDetail = mainExcelDatas.Tables[0].Rows[0][1].ToString();
            string tableDetailTidy = tableDetail;

            // 简化表格B1的描述信息
            int startIndex = 0;
            int endIndex = 0;
            while (startIndex != -1)
            {
                startIndex = tableDetailTidy.IndexOf("[", endIndex);
                if (startIndex != -1)
                {
                    endIndex = tableDetailTidy.IndexOf("]", startIndex);
                }

                if (startIndex != -1 && endIndex != -1)
                {
                    tableDetailTidy = tableDetailTidy.Remove(startIndex, endIndex - startIndex + 1);
                }

                startIndex = tableDetailTidy.IndexOf("[");
                endIndex = startIndex;
            }

            tableDetailTidy = tableDetailTidy.Replace("\n", string.Empty).Replace("\r", string.Empty);

            int columns = mainExcelDatas.Tables[0].Columns.Count;
            string[] keyList = new string[columns];
            string[] typeList = new string[columns];
            string[] langInfo = new string[columns];
            for (int col = 0; col < columns; col++)
            {
                if (mainExcelDatas.Tables[0].Rows[1][col] != null)
                {
                    keyList[col] = mainExcelDatas.Tables[0].Rows[1][col].ToString();
                }

                if (mainExcelDatas.Tables[0].Rows[2][col] != null)
                {
                    typeList[col] = mainExcelDatas.Tables[0].Rows[2][col].ToString();
                }

                if (mainExcelDatas.Tables[0].Rows[3][col] != null)
                {
                    langInfo[col] = mainExcelDatas.Tables[0].Rows[3][col].ToString();
                }

                if (keyList[col] != string.Empty && typeList[col] == string.Empty)
                {
                    Log.Error("" + mainExcelName + "  表格错误, " + keyList[col] + " 没有设置类型。");
                    return false;
                }
            }

            Dictionary<string, string> exportFolderPaths = GetTableExportPath(tableDetail);
            for (int i = 6; i < columns; i++)
            {
                string lang = keyList[i];
                if (lang != string.Empty)
                {
                    if (!exportFolderPaths.ContainsKey(lang) || string.IsNullOrEmpty(exportFolderPaths[lang]))
                    {
                        exportFolderPaths[lang] = EditorPath.Localization.LuaFolderFullPath;
                    }
                }
            }

            foreach (var path in exportFolderPaths)
            {
                if (!Directory.Exists(path.Value))
                {
                    Directory.CreateDirectory(path.Value);
                }
            }

            if (enableExportLuaIncrease) //使用增量导出lua多语言配置
            {
                FindIncrease(columns, fullPaths, keyList, tableDetailTidy, langInfo,
                    enableExportLuaIncreaseSingleVersion, exportLuaIncreasePath);
            }
            else //正常导出lua多语言配置
            {
                // 开始添加lua文件的内容
                MakeLanguageLuaFile(columns, keyList, exportFolderPaths, tableDetailTidy, langInfo, fullPaths,
                    fileNames);
            }

            MakeLuaRequireFile(mainExcelName, tableDetailTidy, columns, keyList, langInfo, exportFolderPaths);

            AssetDatabase.Refresh();
            Log.Debug("所有多语言 lua 文件导出完成。");

            return true;
        }

        public void ExportTMPChars(List<string> customExcelsFullPaths, JObject tmpConfig = null,
            Dictionary<string, string> exportFonts = null)
        {
            if (tmpConfig == null || (bool)tmpConfig["UseTMP"] == false)
            {
                Log.Debug("ExportTMPChars 不需要导出。");
                return;
            }

            // 收集所有多语言Excel表格的路径信息和文件名称集合
            List<string> fullPaths = new List<string>();
            List<string> fileNames = new List<string>();
            // 所有多语言字符分类<文件名，asset内容>
            // <导出字符集名称，asset内容>
            Dictionary<string, List<string>> assetFileDic = new Dictionary<string, List<string>>();
            // 语言名称/导出字符集名称
            Dictionary<string, string> assetFileNameDic = new Dictionary<string, string>();

            string mainExcelFullPath = EditorPath.Localization.ExcelFileFullPath;
            string mainExcelName = System.IO.Path.GetFileNameWithoutExtension(mainExcelFullPath);

            fullPaths.Add(mainExcelFullPath);
            fileNames.Add(mainExcelName);
            customExcelsFullPaths.ForEach(customExcelFullPath =>
            {
                customExcelFullPath = customExcelFullPath.Replace("\\", "/");
                string customExcelName = System.IO.Path.GetFileNameWithoutExtension(customExcelFullPath);
                fullPaths.Add(customExcelFullPath);
                fileNames.Add(customExcelName);
            });

            // 获取主表的表头信息
            DataSet mainExcelDatas = TableExportEditorUtility.GetExcelData(mainExcelFullPath);
            int columns = mainExcelDatas.Tables[0].Columns.Count;
            string[] keyList = new string[columns];
            for (int col = 0; col < columns; col++)
            {
                if (mainExcelDatas.Tables[0].Rows[1][col] != null)
                {
                    keyList[col] = mainExcelDatas.Tables[0].Rows[1][col].ToString();
                }
            }

            // 开始添加lua文件的内容
            for (int i = 6; i < columns; i++)
            {
                if (keyList[i] != string.Empty)
                {
                    assetFileNameDic.Add(keyList[i], $"Localization{keyList[i]}FontTMPChars");
                    var isArabic = keyList[i] == GameDefinitions.Language.Arabic.ToString();

                    assetFileDic.Add(assetFileNameDic[keyList[i]], new List<string>());
                    for (int index = 0; index < fullPaths.Count; index++)
                    {
                        DataSet excelDatas = TableExportEditorUtility.GetExcelData(fullPaths[index]);
                        // 开始逐行加入多语言内容
                        for (int row = 4; row < excelDatas.Tables[0].Rows.Count; row++)
                        {
                            if (excelDatas.Tables[0].Rows[row][2] != null)
                            {
                                string valuseStr = excelDatas.Tables[0].Rows[row][i].ToString();
                                if (!string.IsNullOrEmpty(valuseStr))
                                {
                                    valuseStr = TableExportEditorUtility.DelNewLineFlag(valuseStr);
                                    for (int fonti = 0; fonti < valuseStr.Length; fonti++)
                                    {
                                        assetFileDic[assetFileNameDic[keyList[i]]].Add(valuseStr.Substring(fonti, 1));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 开始添加默认多语言字符集

            string excelPath = EditorPath.Localization.ExcelDefaultFileFullPath;
            DataSet defaultExcelData = TableExportEditorUtility.GetExcelData(excelPath);
            int defaultColumns = defaultExcelData.Tables[0].Columns.Count;
            // 开始按照列转换多语言数据, 从第6行开始每列转出一个文件
            for (int defaultCol = 6; defaultCol < defaultColumns; defaultCol++)
            {
                if (defaultExcelData.Tables[0].Rows[1][defaultCol] != null)
                {
                    string defaultKey = defaultExcelData.Tables[0].Rows[1][defaultCol].ToString();
                    if (!assetFileNameDic.ContainsKey(defaultKey))
                    {
                        assetFileNameDic.Add(defaultKey, $"Localization{defaultKey}FontTMPChars");
                    }

                    if (!string.IsNullOrEmpty(defaultKey))
                    {
                        if (!assetFileDic.ContainsKey(assetFileNameDic[defaultKey]))
                        {
                            assetFileDic.Add(assetFileNameDic[defaultKey], new List<string>());
                        }

                        for (int defaultRow = 4; defaultRow < defaultExcelData.Tables[0].Rows.Count; defaultRow++)
                        {
                            if (defaultExcelData.Tables[0].Rows[defaultRow][1] != null)
                            {
                                if (!string.IsNullOrEmpty(defaultExcelData.Tables[0].Rows[defaultRow][1].ToString()))
                                {
                                    string itemValue = defaultExcelData.Tables[0].Rows[defaultRow][defaultCol]
                                        .ToString();
                                    if (!string.IsNullOrEmpty(itemValue))
                                    {
                                        itemValue = TableExportEditorUtility.DelNewLineFlag(itemValue);
                                        for (int fonti = 0; fonti < itemValue.Length; fonti++)
                                        {
                                            assetFileDic[assetFileNameDic[defaultKey]]
                                                .Add(itemValue.Substring(fonti, 1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (TableExportEditorUtilityTool.CreateTableExportTextTxt((string)tmpConfig["TMPCharsSaveDirectory"],
                    (bool)tmpConfig["MulLangToOne"], "Localization_AllLanguages_FontTMP_Chars", assetFileDic))
            {
                Log.Debug("所有多语言字符集导出完成。");
            }
        }

        /// <summary>
        /// 获取指定的导出路径（目录）
        /// </summary>
        /// <param name="excelDescrip">表格中B1字段</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetTableExportPath(string excelDescrip)
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            paths.Add("Main", EditorPath.Localization.LuaFolderFullPath);
            string keyWords = "[exportPath:";
            if (string.IsNullOrEmpty(excelDescrip) || !excelDescrip.Contains(keyWords))
            {
                return paths;
            }

            int startIndex = 0;
            int endIndex = 0;
            while (startIndex != -1)
            {
                startIndex = excelDescrip.IndexOf(keyWords, endIndex);
                endIndex = excelDescrip.IndexOf("]", startIndex);
                string desc = excelDescrip.Substring(startIndex + 1, endIndex - startIndex - 1);
                string[] result = desc.Split(":");
                if (result.Length == 2)
                {
                    paths["Main"] = result[1];
                }
                else
                {
                    if (!paths.ContainsKey(result[1]))
                    {
                        paths.Add(result[1], null);
                    }

                    paths[result[1]] = result[2];
                }

                startIndex = excelDescrip.IndexOf(keyWords, endIndex);
            }

            return paths;
        }

        /// <summary>
        /// 读取TMP字符集文件配置信息
        /// </summary>
        /// <returns></returns>
        public JObject LoadTMPExportConfig()
        {
            string configFilePath = EditorPath.Editor.LocalizationFontTMPExportSettings.ProjectSettingFileFullPath;
            JObject configJsonData = null;
            if (File.Exists(configFilePath))
            {
                string content = System.IO.File.ReadAllText(configFilePath);
                configJsonData = JsonConvert.DeserializeObject<JObject>(content);
            }

            if (configJsonData == null)
            {
                configJsonData = new JObject();
                configJsonData["UseTMP"] = false;
                configJsonData["MulLangToOne"] = false;
                configJsonData["TMPCharsSaveDirectory"] = "";
            }

            return configJsonData;
        }

        /// <summary>
        /// 写入TMP字符集文件配置信息
        /// </summary>
        /// <param name="curConfigData"></param>
        public void SaveTMPExportConfig(JObject curConfigData)
        {
            if (curConfigData == null)
            {
                Log.Error("SaveTMPExportConfig 要保存的导出配置信息为null。");
                return;
            }

            string configFilePath = EditorPath.Editor.LocalizationFontTMPExportSettings.ProjectSettingFileFullPath;
            if (File.Exists(configFilePath))
            {
                File.Delete(configFilePath);
            }

            string saveString = curConfigData.ToString();
            File.WriteAllText(configFilePath, saveString);
            Log.Debug($"保存 TMP 导出配置信息成功，配置文件路径：{configFilePath}。");
        }


        /// <summary>
        /// 获取Docs\Designs\Excels\Localizations文件夹下面所有包含多语言字段的表格(包含通用表格和自定义表格)
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllLocalizationExcelPaths()
        {
            // 获取自定义多语言表格路径集合
            List<string> localizationExcelList =
                new List<string>(Directory.GetFiles(EditorPath.Localization.ExcelFolderFullPath, "*.xlsm",
                    SearchOption.TopDirectoryOnly));
            localizationExcelList.RemoveAll(path =>
            {
                path = path.Replace("\\", "/");
                if (System.IO.Path.GetFileNameWithoutExtension(path).StartsWith("~"))
                {
                    return true;
                }

                if (path == EditorPath.Localization.ExcelFontFileFullPath)
                {
                    return true;
                }

                return false;
            });
            return localizationExcelList;
        }

        /// <summary>
        /// 导出多语言字符集
        /// </summary>
        /// <param name="TMPFontName">需要导出多语言字符集的TMP字体文件，用来创建字体文件夹名称</param>
        /// <param name="exportLangNames">需要导出多语言字符集的多语言类型，用来筛选字符集</param>
        /// <param name="exprotOneFile">是否将多个多语言合并到一个导出文件中</param>
        public void ExprotTMPCharsForFont(string TMPFontName, string exportRootPath, List<string> exportLangNames,
            bool exprotOneFile)
        {
            string oneCharName = null;
            // 收集所有多语言Excel表格的路径信息和文件名称集合
            List<string> fullPaths = new List<string>();
            List<string> fileNames = new List<string>();
            // 所有多语言字符分类<文件名，asset内容>
            // <语言名称，asset内容>
            Dictionary<string, List<string>> assetFileDic = new Dictionary<string, List<string>>();
            // 语言名称/导出字符集名称
            Dictionary<string, string> assetFileNameDic = new Dictionary<string, string>();

            List<string> excelFullPaths = GetAllLocalizationExcelPaths();
            excelFullPaths.ForEach(perPath =>
            {
                fullPaths.Add(perPath.Replace("\\", "/"));
                fileNames.Add(System.IO.Path.GetFileNameWithoutExtension(perPath));
            });

            exportLangNames.ForEach(p =>
            {
                assetFileDic.Add(p, new List<string>());
                if (oneCharName == null)
                {
                    oneCharName = p;
                }
                else
                {
                    oneCharName = $"{oneCharName}_{p}";
                }
            });
            oneCharName = $"Localization_{oneCharName}_FontTMP_Chars";

            // 开始遍历所有多语言表格
            for (int i = 0; i < fullPaths.Count; i++)
            {
                // 获取多语言表格数据
                DataSet excelData = TableExportEditorUtility.GetExcelData(fullPaths[i]);
                // 判断当前列对应的多语言是否需要导出
                for (int col = 6; col < excelData.Tables[0].Columns.Count; col++)
                {
                    string curColLangName = excelData.Tables[0].Rows[1][col].ToString();
                    if (string.IsNullOrEmpty(curColLangName) == false && assetFileDic.ContainsKey(curColLangName))
                    {
                        Log.Debug($"TMP 字符集，添加 {fullPaths[i]} 中多语言 {curColLangName} 。");
                        // 开始逐行加入多语言内容
                        for (int row = 4; row < excelData.Tables[0].Rows.Count; row++)
                        {
                            string valuseStr = excelData.Tables[0].Rows[row][col].ToString();
                            if (!string.IsNullOrEmpty(valuseStr))
                            {
                                valuseStr = TableExportEditorUtility.DelNewLineFlag(valuseStr);
                                for (int fonti = 0; fonti < valuseStr.Length; fonti++)
                                {
                                    assetFileDic[curColLangName].Add(valuseStr.Substring(fonti, 1));
                                }
                            }
                        }
                    }
                    else
                    {
                        //   Log.Debug(LogTag.Editor, $"TMP 字符集，表格 {fullPaths[i]} 中未找到多语言 {curColLangName} 。");
                    }
                }
            }

            if (Directory.Exists(exportRootPath) == false)
            {
                Directory.CreateDirectory(exportRootPath);
            }

            string exportSubDirectory = $"{exportRootPath}/{TMPFontName}";

            if (Directory.Exists(exportSubDirectory) == false)
            {
                Directory.CreateDirectory(exportSubDirectory);
            }

            if (TableExportEditorUtilityTool.CreateTableExportTextTxt(exportSubDirectory, exprotOneFile, oneCharName,
                    assetFileDic))
            {
                Log.Debug("所有多语言字符集导出完成。");
            }
        }
    }
}