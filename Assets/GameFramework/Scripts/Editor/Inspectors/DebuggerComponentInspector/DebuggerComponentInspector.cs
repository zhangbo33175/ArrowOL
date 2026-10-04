/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   Debugger组件编辑器面板定制
 ***************************************************************/
using Newtonsoft.Json.Linq;
using Honor.Runtime;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(DebuggerComponent))]
    internal sealed class DebuggerComponentInspector : HonorComponentInspector
    {
        private SerializedProperty m_Skin = null;
        private SerializedProperty m_MiniSkin = null;
        private SerializedProperty m_PopWindowSkin = null;
        private SerializedProperty m_StayBorder = null;
        private SerializedProperty m_IdleState = null;
        private SerializedProperty m_EnterIdleTime = null;
        private SerializedProperty m_ConsoleWindow = null;

        private string m_AndroidAABAbsolutelyPath = null;
        private string m_AndroidAPKSAbsolutelyPath = null;
        private string m_AndroidBundleToolAbsolutelyPath = null;
        private string m_AndroidSignatureAbsolutelyPath = null;
        private string m_AndroidSignatureAlias = null;
        private string m_AndroidSignaturePass = null;
        private string m_AndroidADBAbsolutelyPath = null;

        private void OnEnable()
        {
            m_Skin = serializedObject.FindProperty("m_Skin");
            m_MiniSkin = serializedObject.FindProperty("m_MiniSkin");
            m_PopWindowSkin = serializedObject.FindProperty("m_PopWindowSkin");
            m_StayBorder = serializedObject.FindProperty("m_StayBorder");
            m_IdleState = serializedObject.FindProperty("m_IdleState");
            m_EnterIdleTime = serializedObject.FindProperty("m_EnterIdleTime");
            m_ConsoleWindow = serializedObject.FindProperty("m_ConsoleWindow");
            serializedObject.ApplyModifiedProperties();
            if (string.IsNullOrEmpty(m_AndroidAABAbsolutelyPath)) m_AndroidAABAbsolutelyPath = "None";
            if (string.IsNullOrEmpty(m_AndroidAPKSAbsolutelyPath)) m_AndroidAPKSAbsolutelyPath = "None";
            if (string.IsNullOrEmpty(m_AndroidBundleToolAbsolutelyPath)) m_AndroidBundleToolAbsolutelyPath = "None";
            if (string.IsNullOrEmpty(m_AndroidSignatureAbsolutelyPath)) m_AndroidSignatureAbsolutelyPath = "None";
            if (string.IsNullOrEmpty(m_AndroidSignatureAlias)) m_AndroidSignatureAlias = "None";
            if (string.IsNullOrEmpty(m_AndroidSignaturePass)) m_AndroidSignaturePass = "None";
            if (string.IsNullOrEmpty(m_AndroidADBAbsolutelyPath)) m_AndroidADBAbsolutelyPath = "None";

            // 读取AAB包体安装工具相关信息
            ReadAABToolsFromLibraryConfigFile();

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            DebuggerComponent t = (DebuggerComponent)target;

            m_Skin.objectReferenceValue = EditorGUILayout.ObjectField("窗口皮肤", m_Skin.objectReferenceValue, typeof(GUISkin), true);
            m_MiniSkin.objectReferenceValue = EditorGUILayout.ObjectField("Mini窗口皮肤", m_MiniSkin.objectReferenceValue, typeof(GUISkin), true);
            m_PopWindowSkin.objectReferenceValue = EditorGUILayout.ObjectField("弹出窗口皮肤", m_PopWindowSkin.objectReferenceValue, typeof(GUISkin), true);
            m_StayBorder.boolValue = EditorGUILayout.Toggle("开启吸附功能", m_StayBorder.boolValue);
            m_EnterIdleTime.intValue = EditorGUILayout.IntField("进入透明显示状态的时间(秒)", m_EnterIdleTime.intValue);
            EditorGUILayout.HelpBox("在设置时间内没有操作debug窗口，debug窗口将半透明显示。\r\n设置为-1将不进行半透明显示", MessageType.Info);
          
            EditorGUILayout.PropertyField(m_ConsoleWindow, true);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("【Android-AAB包体安装工具】");

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Android-bundletool工具位置：");
            EditorGUILayout.LabelField(m_AndroidBundleToolAbsolutelyPath);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            if (GUILayout.Button("选择工具", GUILayout.MaxWidth(130)))
            {
                string rootPath = Application.dataPath;
                string selectedFileName = EditorUtility.OpenFilePanel("选择bundletool工具", rootPath, "jar");
                if (!string.IsNullOrEmpty(selectedFileName))
                {
                    m_AndroidBundleToolAbsolutelyPath = selectedFileName;
                }
                WriteAABToolsToLibraryConfigFile();
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("打开bundletool目录", GUILayout.MaxWidth(130)))
            {
                OpenFolder(string.Format("\"{0}\"", m_AndroidBundleToolAbsolutelyPath.Substring(0, m_AndroidBundleToolAbsolutelyPath.LastIndexOf("/") + 1)));
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("bundletool工具位于Unity安装目录：Editor\\Data\\PlaybackEngines\\AndroidPlayer\\Tools下。", MessageType.Info);

            bool bundletoolExist = false;
            if (!m_AndroidBundleToolAbsolutelyPath.Equals("None"))
            {
                bundletoolExist = System.IO.File.Exists(m_AndroidBundleToolAbsolutelyPath);
            }

            if (!bundletoolExist)
            {
                m_AndroidBundleToolAbsolutelyPath = "None";
                EditorGUI.BeginDisabledGroup(true);
            }

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Android-ADB工具位置：");
            EditorGUILayout.LabelField(m_AndroidADBAbsolutelyPath);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            if (GUILayout.Button("选择工具", GUILayout.MaxWidth(100)))
            {
                string rootPath = Application.dataPath;
                string selectedFileName = EditorUtility.OpenFilePanel("选择ADB工具", rootPath, Application.platform == RuntimePlatform.WindowsEditor ? "exe" : string.Empty);
                if (!string.IsNullOrEmpty(selectedFileName))
                {
                    m_AndroidADBAbsolutelyPath = selectedFileName;
                }
                WriteAABToolsToLibraryConfigFile();
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("打开ADB目录", GUILayout.MaxWidth(100)))
            {
                OpenFolder(string.Format("\"{0}\"", m_AndroidADBAbsolutelyPath.Substring(0, m_AndroidADBAbsolutelyPath.LastIndexOf("/") + 1)));
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("adb工具位于Android-SDK/platform-tools目录下。", MessageType.Info);

            bool adbExist = false;
            if (!m_AndroidADBAbsolutelyPath.Equals("None"))
            {
                adbExist = System.IO.File.Exists(m_AndroidADBAbsolutelyPath);
            }

            if (!adbExist)
            {
                m_AndroidADBAbsolutelyPath = "None";
                EditorGUI.BeginDisabledGroup(true);
            }

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Android-签名位置：");
            EditorGUILayout.LabelField(m_AndroidSignatureAbsolutelyPath);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            if (GUILayout.Button("选择签名", GUILayout.MaxWidth(100)))
            {
                string rootPath = string.Format("{0}/{1}", Application.dataPath, "../Docs/Programs/Certificates/Android");
                string selectedFileName = EditorUtility.OpenFilePanel("选择签名", rootPath, "keystore");
                if (!string.IsNullOrEmpty(selectedFileName))
                {
                    m_AndroidSignatureAbsolutelyPath = selectedFileName;
                }
                WriteAABToolsToLibraryConfigFile();
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("打开签名目录", GUILayout.MaxWidth(100)))
            {
                OpenFolder(string.Format("\"{0}\"", m_AndroidSignatureAbsolutelyPath.Substring(0, m_AndroidSignatureAbsolutelyPath.LastIndexOf("/") + 1)));
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            bool signatureExist = false;
            if (!m_AndroidSignatureAbsolutelyPath.Equals("None"))
            {
                signatureExist = System.IO.File.Exists(m_AndroidSignatureAbsolutelyPath);
            }

            if (!signatureExist)
            {
                m_AndroidSignatureAbsolutelyPath = "None";
                EditorGUI.BeginDisabledGroup(true);
            }

            string alias = EditorGUILayout.TextField("Android-签名Alias", m_AndroidSignatureAlias);
            if (alias != m_AndroidSignatureAlias)
            {
                m_AndroidSignatureAlias = alias;
                WriteAABToolsToLibraryConfigFile();
            }

            string pass = EditorGUILayout.TextField("Android-签名Pass", m_AndroidSignaturePass);
            if (pass != m_AndroidSignaturePass)
            {
                m_AndroidSignaturePass = pass;
                WriteAABToolsToLibraryConfigFile();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Android-AAB安装包位置：");
            EditorGUILayout.LabelField(m_AndroidAABAbsolutelyPath);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            if (GUILayout.Button("选择安装包", GUILayout.MaxWidth(100)))
            {
                string rootPath = string.Format("{0}/{1}", Application.dataPath, "../Natives/Android");
                string selectedFileName = EditorUtility.OpenFilePanel("选择AAB安装包", rootPath, "aab");
                if (!string.IsNullOrEmpty(selectedFileName))
                {
                    m_AndroidAABAbsolutelyPath = selectedFileName;
                }
                WriteAABToolsToLibraryConfigFile();
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("打开AAB目录", GUILayout.MaxWidth(100)))
            {
                OpenFolder(string.Format("\"{0}\"", m_AndroidAABAbsolutelyPath.Substring(0, m_AndroidAABAbsolutelyPath.LastIndexOf("/") + 1)));
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            bool abExist = false;
            bool apksExist = false;
            if (!m_AndroidAABAbsolutelyPath.Equals("None"))
            {
                abExist = System.IO.File.Exists(m_AndroidAABAbsolutelyPath);
            }
            if (!m_AndroidAPKSAbsolutelyPath.Equals("None"))
            {
                apksExist = System.IO.File.Exists(m_AndroidAPKSAbsolutelyPath);
            }

            if (!abExist)
            {
                m_AndroidAABAbsolutelyPath = "None";
                EditorGUI.BeginDisabledGroup(true);
            }

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Android-APKS导出位置：");
            EditorGUILayout.LabelField(m_AndroidAPKSAbsolutelyPath);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            if (GUILayout.Button("导出APKS", GUILayout.MaxWidth(100)))
            {
                m_AndroidAPKSAbsolutelyPath = string.Format("{0}{1}", m_AndroidAABAbsolutelyPath.Substring(0, m_AndroidAABAbsolutelyPath.Length - ".aab".Length), ".apks");
                string arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" {4} {5}", m_AndroidAPKSAbsolutelyPath, m_AndroidBundleToolAbsolutelyPath, m_AndroidAABAbsolutelyPath, m_AndroidSignatureAbsolutelyPath, m_AndroidSignatureAlias, m_AndroidSignaturePass);
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
#if UNITY_EDITOR_WIN
                        RunBatScript("Tools/Build/aab-install", "export_apks.bat", arguments);
#endif
                        break;

                    case RuntimePlatform.OSXEditor:
#if UNITY_EDITOR_OSX
                        RunShellScript("Tools/Build/aab-install", "export_apks.sh", arguments);
#endif
                        break;

                    default:
                        throw new GameException(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                }
                apksExist = System.IO.File.Exists(m_AndroidAPKSAbsolutelyPath);
                if (!apksExist)
                {
                    m_AndroidAPKSAbsolutelyPath = "None";
                }
                WriteAABToolsToLibraryConfigFile();
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("打开APKS目录", GUILayout.MaxWidth(100)))
            {
                OpenFolder(string.Format("\"{0}\"", m_AndroidAABAbsolutelyPath.Substring(0, m_AndroidAABAbsolutelyPath.LastIndexOf("/") + 1)));
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (!apksExist)
            {
                m_AndroidAPKSAbsolutelyPath = "None";
                EditorGUI.BeginDisabledGroup(true);
            }

            if (GUILayout.Button("安装APKS到Android设备"))
            {
                string arguments = string.Format("\"{0}\" \"{1}\" \"{2}\"", m_AndroidAPKSAbsolutelyPath, m_AndroidBundleToolAbsolutelyPath, m_AndroidADBAbsolutelyPath);
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
#if UNITY_EDITOR_WIN
                        RunBatScript("Tools/Build/aab-install", "install_apks.bat", arguments);
#endif
                        break;

                    case RuntimePlatform.OSXEditor:
#if UNITY_EDITOR_OSX
                        RunShellScript("Tools/Build/aab-install", "install_apks.sh", arguments);
#endif
                        break;

                    default:
                        throw new GameException(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                }
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }

            if (!apksExist)
            {
                EditorGUI.EndDisabledGroup();
            }

            if (!abExist)
            {
                EditorGUI.EndDisabledGroup();
            }

            if (!signatureExist)
            {
                EditorGUI.EndDisabledGroup();
            }

            if (!adbExist)
            {
                m_AndroidADBAbsolutelyPath = "None";
                EditorGUI.EndDisabledGroup();
            }

            if (!bundletoolExist)
            {
                m_AndroidBundleToolAbsolutelyPath = "None";
                EditorGUI.EndDisabledGroup();
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        /// <summary>
        /// 打开目录
        /// </summary>
        private void OpenFolder(string folderPath)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folderPath.Replace('/', '\\'));
                    break;

                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folderPath);
                    break;

                default:
                    throw new GameException(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
            }
        }

        /// <summary>
        /// 内部调用 bat 脚本（Windows 编辑器）
        /// </summary>
        /// <param name="directoryPathFromProj">相对工程根的目录路径</param>
        /// <param name="batFileName">bat 文件名称（带扩展名）</param>
        /// <param name="arguments">运行参数</param>
        private void RunBatScript(string directoryPathFromProj, string batFileName, string arguments = null)
        {
            Process proc = new Process();
            string path = string.Format("{0}{1}", Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), directoryPathFromProj).Replace('/', '\\');
            proc.StartInfo.WorkingDirectory = path;
            proc.StartInfo.FileName = batFileName;
            proc.StartInfo.Arguments = arguments;
            proc.Start();
            proc.WaitForExit();
        }

        /// <summary>
        /// 内部调用 shell 脚本（macOS 编辑器）
        /// </summary>
        /// <param name="directoryPathFromProj">相对工程根的目录路径</param>
        /// <param name="shFileName">sh 文件名称（带扩展名）</param>
        /// <param name="arguments">运行参数</param>
        private void RunShellScript(string directoryPathFromProj, string shFileName, string arguments = null)
        {
            string shell = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + directoryPathFromProj + "/" + shFileName + " ";
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.ErrorDialog = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = shell + (arguments == null ? "" : arguments);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.WorkingDirectory = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + directoryPathFromProj;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                UnityEngine.Debug.Log(output);
            }
            process.WaitForExit();
        }

        /// <summary>
        /// 读取AAB包体安装工具相关信息
        /// </summary>
        private void ReadAABToolsFromLibraryConfigFile()
        {
            string dataPath = string.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/AABTools.dat").Replace("\\", "/");
            if (File.Exists(dataPath))
            {
                string content = System.IO.File.ReadAllText(dataPath);
                JObject jObject = JObject.Parse(content);
                m_AndroidAABAbsolutelyPath = jObject["AABPath"].ToString();
                m_AndroidAPKSAbsolutelyPath = jObject["APKSPath"].ToString();
                m_AndroidBundleToolAbsolutelyPath = jObject["BundleToolPath"].ToString();
                m_AndroidSignatureAbsolutelyPath = jObject["SignaturePath"].ToString();
                m_AndroidSignatureAlias = jObject["SignatureAlias"].ToString();
                m_AndroidSignaturePass = jObject["SignaturePass"].ToString();
                m_AndroidADBAbsolutelyPath = jObject["ADBPath"].ToString();
            }
            else
            {
                WriteAABToolsToLibraryConfigFile();
            }
        }

        /// <summary>
        /// 写入AAB包体安装工具相关信息
        /// </summary>
        private void WriteAABToolsToLibraryConfigFile()
        {
            string dataPath = string.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/AABTools.dat").Replace("\\", "/");
            JObject jObject = new JObject();
            jObject.Add("AABPath", m_AndroidAABAbsolutelyPath);
            jObject.Add("APKSPath", m_AndroidAPKSAbsolutelyPath);
            jObject.Add("BundleToolPath", m_AndroidBundleToolAbsolutelyPath);
            jObject.Add("SignaturePath", m_AndroidSignatureAbsolutelyPath);
            jObject.Add("SignatureAlias", m_AndroidSignatureAlias);
            jObject.Add("SignaturePass", m_AndroidSignaturePass);
            jObject.Add("ADBPath", m_AndroidADBAbsolutelyPath);
            File.WriteAllText(dataPath, jObject.ToString());
        }

    }
}
