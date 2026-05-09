using System.IO;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{    
    /// <summary>
    /// 【声音系统编辑器面板】
    /// 功能：声音组件的可视化配置 + 声音配置表一键导出
    /// 作用：在 Inspector 面板直接管理音频混响器、声音组、导出声音表
    /// 属于：框架 -> 声音系统 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(SoundComponent))]
    internal sealed class SoundComponentInspector: HonorComponentInspector
    {
        // 序列化字段：绑定 SoundComponent 中的变量
        private SerializedProperty m_AudioMixer = null;       // 音频混响器
        private SerializedProperty m_SoundGroupShells = null; // 声音分组配置

        /// <summary>
        /// 初始化：绑定序列化属性
        /// </summary>
        private void OnEnable()
        {
            m_AudioMixer = serializedObject.FindProperty("m_AudioMixer");
            m_SoundGroupShells = serializedObject.FindProperty("m_SoundGroupShells");
        }

        /// <summary>
        /// 绘制编辑器面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SoundComponent soundComp = (SoundComponent)target;

            // ====================== 配置表操作按钮 ======================
            EditorGUILayout.BeginHorizontal("box");
            {
                // 打开声音配置 Excel
                if (GUILayout.Button("打开声音表Excel"))
                {
                    TableExportEditorUtility.OpenExcel(GamePathUtils.Sound.GetExcelFileFullPath());
                    GUIUtility.ExitGUI();
                }

                // 一键导出声音表 → Lua
                if (GUILayout.Button("导出声音表Excel到Lua"))
                {
                    string filePath = GamePathUtils.Sound.GetExcelFileFullPath();
                    ExportExcelToLuaFromSound(Path.GetFileNameWithoutExtension(filePath));
                    GUIUtility.ExitGUI();
                }

                // 打开 Excel 所在文件夹
                if (GUILayout.Button("打开配置表Excel所在文件夹"))
                {
                    TableExportEditorUtility.OpenDirectory(GamePathUtils.Sound.GetExcelRootDirectoryFullPath());
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndHorizontal();

            // ====================== 声音组件配置 ======================
            // 音频混响器（控制音量、音效、背景音乐等）
            EditorGUILayout.PropertyField(m_AudioMixer);
            // 声音组配置（可折叠数组，配置多组声音）
            EditorGUILayout.PropertyField(m_SoundGroupShells, true);

            // ====================== 运行时调试信息 ======================
            if (EditorApplication.isPlaying && IsPrefabInHierarchy(soundComp.gameObject))
            {
                EditorGUILayout.LabelField("声音组数量", soundComp.SoundGroupCount.ToString());
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        /// <summary>
        /// 【核心导出】声音 Excel 表 → Lua 配置文件
        /// 自动生成声音表，供 Lua 层直接播放声音
        /// </summary>
        /// <param name="excelFileName">Excel 文件名</param>
        /// <returns>是否导出成功</returns>
        public bool ExportExcelToLuaFromSound(string excelFileName)
        {
            // Excel 源文件路径
            string excelPath = $"{GamePathUtils.Sound.GetExcelRootDirectoryFullPath()}/{excelFileName}.xlsm";
            // 导出的 Lua 文件路径
            string luaRootPath = GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath();
            string luaPath = Path.Combine(luaRootPath, "RTables", $"Table{excelFileName}.lua.txt");

            // 目录不存在则创建
            if (!Directory.Exists(luaRootPath))
            {
                Directory.CreateDirectory(luaRootPath);
            }
            
            // 调用工具类执行导出
            return TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
        }
    }
}