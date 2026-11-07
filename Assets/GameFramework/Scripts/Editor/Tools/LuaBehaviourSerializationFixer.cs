/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviourSerializationFixer.cs
 * author:    云毅
 * created:   2026
 * descrip:   批量修复 LuaBehaviour 序列化字段丢失问题
 *            类名重命名后，预制体上的序列化字段可能静默丢失
 ***************************************************************/

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Honor.Runtime.Editor
{
    /// <summary>
    /// LuaBehaviour 序列化字段修复工具
    /// </summary>
    public static class LuaBehaviourSerializationFixer
    {
        [MenuItem("Honor/工具/批量检查 LuaBehaviour 配置")]
        public static void CheckAllLuaBehaviour()
        {
            Debug.Log("开始检查所有 LuaBehaviour 配置...");

            // 找到所有预制体
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
            int totalCount = 0;
            int issueCount = 0;
            List<string> issuePrefabs = new List<string>();

            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null) continue;

                // 获取所有 HonorLuaBehaviour 组件
                LuaBehaviour[] behaviours = prefab.GetComponentsInChildren<LuaBehaviour>(true);
                if (behaviours.Length == 0) continue;

                foreach (LuaBehaviour behaviour in behaviours)
                {
                    totalCount++;

                    // 检查是否有问题
                    if (HasIssue(behaviour))
                    {
                        issueCount++;
                        if (!issuePrefabs.Contains(path))
                        {
                            issuePrefabs.Add(path);
                        }
                    }
                }
            }

            Debug.Log($"检查完成！共检查 {totalCount} 个 LuaBehaviour 组件，发现 {issueCount} 个有问题。");
            if (issuePrefabs.Count > 0)
            {
                Debug.Log("有问题的预制体:");
                foreach (string path in issuePrefabs.Take(10))
                {
                    Debug.Log($"  {path}");
                }
                if (issuePrefabs.Count > 10)
                {
                    Debug.Log($"  ... 还有 {issuePrefabs.Count - 10} 个");
                }
            }
        }

        [MenuItem("Honor/工具/批量修复 LuaBehaviour 序列化字段")]
        public static void FixAllLuaBehaviourSerialization()
        {
            Debug.Log("开始批量修复 LuaBehaviour 序列化字段...");

            // 找到所有预制体
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
            int fixedCount = 0;
            int totalCount = 0;

            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null) continue;

                // 获取所有 LuaBehaviour 组件
                LuaBehaviour[] behaviours = prefab.GetComponentsInChildren<LuaBehaviour>(true);
                if (behaviours.Length == 0) continue;

                bool modified = false;

                foreach (LuaBehaviour behaviour in behaviours)
                {
                    totalCount++;

                    // 修复序列化字段
                    if (FixBehaviour(behaviour))
                    {
                        modified = true;
                        fixedCount++;
                    }
                }

                if (modified)
                {
                    EditorUtility.SetDirty(prefab);
                }
            }

            // 保存所有修改
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"批量修复完成！共检查 {totalCount} 个 LuaBehaviour 组件，修复 {fixedCount} 个。");
        }

        /// <summary>
        /// 检查是否有问题
        /// </summary>
        private static bool HasIssue(LuaBehaviour behaviour)
        {
            // 获取私有字段
            FieldInfo patternTypeField = typeof(LuaBehaviour).GetField("m_PatternType",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (patternTypeField == null) return false;

            PatternType currentValue = (PatternType)patternTypeField.GetValue(behaviour);

            // 如果是 None 模式，检查 None 模式的脚本列表是否为空
            if (currentValue == PatternType.None)
            {
                FieldInfo noneListField = typeof(LuaBehaviour).GetField("m_LuaScriptNamesNone",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (noneListField != null)
                {
                    List<string> noneList = (List<string>)noneListField.GetValue(behaviour);
                    if (noneList == null || noneList.Count == 0)
                    {
                        // 检查 MVVM 列表是否有值
                        FieldInfo mvvmListField = typeof(LuaBehaviour).GetField("m_LuaScriptNamesMVVM",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        if (mvvmListField != null)
                        {
                            List<string> mvvmList = (List<string>)mvvmListField.GetValue(behaviour);
                            if (mvvmList != null && mvvmList.Count > 0)
                            {
                                // MVVM 列表有值，但是模式是 None，说明序列化丢失了
                                Debug.LogWarning($"发现问题: {behaviour.gameObject.name} - 模式是 None，但 MVVM 列表有值");
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 修复单个 LuaBehaviour
        /// </summary>
        private static bool FixBehaviour(LuaBehaviour behaviour)
        {
            // 获取私有字段
            FieldInfo patternTypeField = typeof(LuaBehaviour).GetField("m_PatternType",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (patternTypeField == null) return false;

            PatternType currentValue = (PatternType)patternTypeField.GetValue(behaviour);

            // 检查是否需要修复
            bool needFix = false;

            if (currentValue == PatternType.None)
            {
                // 检查 MVVM 列表是否有值
                FieldInfo mvvmListField = typeof(LuaBehaviour).GetField("m_LuaScriptNamesMVVM",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (mvvmListField != null)
                {
                    List<string> mvvmList = (List<string>)mvvmListField.GetValue(behaviour);
                    if (mvvmList != null && mvvmList.Count > 0)
                    {
                        // MVVM 列表有值，但是模式是 None，说明序列化丢失了
                        // 修复：把模式改成 MVVM
                        patternTypeField.SetValue(behaviour, PatternType.MVVM);
                        needFix = true;
                        Debug.Log($"已修复: {behaviour.gameObject.name} - 模式从 None 改为 MVVM");
                    }
                }
            }

            return needFix;
        }
    }
}
