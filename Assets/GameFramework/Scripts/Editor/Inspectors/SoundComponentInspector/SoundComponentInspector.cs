using System.IO;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{    
    [CustomEditor(typeof(SoundComponent))]
    internal sealed class SoundComponentInspector: HonorComponentInspector
    {
          private SerializedProperty m_AudioMixer = null;
        private SerializedProperty m_SoundGroupShells = null;

        private void OnEnable()
        {
            m_AudioMixer = serializedObject.FindProperty("m_AudioMixer");
            m_SoundGroupShells = serializedObject.FindProperty("m_SoundGroupShells");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SoundComponent t = (SoundComponent)target;

            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("打开声音表Excel"))
            {
                TableExportEditorUtility.OpenExcel(GamePathUtils.Sound.GetExcelFileFullPath());
                GUIUtility.ExitGUI();
            }

            // Excel导出（导出Sounds目录下的Excel到Lua）
            if (GUILayout.Button("导出声音表Excel到Lua"))
            {
                string filePath = GamePathUtils.Sound.GetExcelFileFullPath();
                ExportExcelToLuaFromSound(System.IO.Path.GetFileNameWithoutExtension(filePath));
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开配置表Excel所在文件夹"))
            {
                TableExportEditorUtility.OpenDirectory(GamePathUtils.Sound.GetExcelRootDirectoryFullPath());
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(m_AudioMixer);
            EditorGUILayout.PropertyField(m_SoundGroupShells, true);

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("声音组数量", t.SoundGroupCount.ToString());
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        /// <summary>
        /// 导出声音表格到lua
        /// </summary>
        /// <param name="openExcelNamePre"></param>
        /// <returns></returns>
        public bool ExportExcelToLuaFromSound(string openExcelNamePre)
        {
            // 获取excel路径
            string excelPath = $"{GamePathUtils.Sound.GetExcelRootDirectoryFullPath()}/{openExcelNamePre}.xlsm";
            // 获取lua路径
            string luaRootPath = GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath();
            string luaPath = luaRootPath + "/RTables/Table" + openExcelNamePre + ".lua.txt";
            if(!Directory.Exists(luaRootPath))
            {
                Directory.CreateDirectory(luaRootPath);
            }
            // 导出数据
            return TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
        }
    }
}