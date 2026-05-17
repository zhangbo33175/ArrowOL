/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetComponentInspector.cs
 * author:    云毅
 *  created:   2026
 * descrip:   资源管理组件编辑器面板
 *            运行时可视化监控 Asset/AB/Prefab 加载、卸载、引用计数，支持导出CSV
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 资源管理组件编辑器面板
    /// 作用：运行时可视化监控 Asset / AB / Prefab 加载、卸载、引用计数、内存状态，支持导出CSV
    /// </summary>
    [CustomEditor(typeof(AssetComponent))]
    public class AssetComponentInspector : HonorComponentInspector
    {
        #region 【常量配置】
        //=========================================================================
        // 常量配置
        //=========================================================================
        /// <summary>资源最大延迟卸载帧数（1小时=36000帧）</summary>
        private const int UnloadAssetDelayFrameNumMax = 36000;
        
        /// <summary>触发GC的最大累计加载资源数量</summary>
        private const int LoadedMaxNumToCleanMemeryMax = 1000;
        #endregion

        #region 【序列化字段】
        //=========================================================================
        // 序列化字段
        //=========================================================================
        /// <summary>资源延迟卸载帧数</summary>
        private SerializedProperty m_UnloadAssetDelayFrameNum = null;
        
        /// <summary>触发清理内存的累计加载数量</summary>
        private SerializedProperty m_LoadedMaxNumToCleanMemery = null;
        #endregion

        #region 【折叠面板状态】
        //=========================================================================
        // 折叠面板状态
        //=========================================================================
        /// <summary>记录编辑器面板展开状态（运行时）</summary>
        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();
        #endregion

        #region 【编辑器生命周期】
        //=========================================================================
        // 绘制Inspector面板
        //=========================================================================
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            AssetComponent t = (AssetComponent)target;
            bool isEditorResourceMode = t.EditorResourceMode;

            // 编辑器模式警告
            if (isEditorResourceMode)
            {
                EditorGUILayout.HelpBox("当前为编辑器资源模式，个别选项功能会失效。", MessageType.Warning);
            }

            // 滑动条：资源最小过期帧数
            int unloadAssetDelayFrameNum = (int)EditorGUILayout.Slider(
                AorTxt.Format("Asset最小过期帧数"),
                m_UnloadAssetDelayFrameNum.intValue,
                0,
                UnloadAssetDelayFrameNumMax
            );
            
            if (unloadAssetDelayFrameNum != m_UnloadAssetDelayFrameNum.intValue)
            {
                if (EditorApplication.isPlaying)
                    t.UnloadAssetDelayFrameNum = unloadAssetDelayFrameNum;
                else
                    m_UnloadAssetDelayFrameNum.intValue = unloadAssetDelayFrameNum;
            }

            // 滑动条：每轮GC所需累计加载数量
            int loadedMaxNumToCleanMemery = (int)EditorGUILayout.Slider(
                "每轮GC所需异步加载完成资源的累计数量",
                m_LoadedMaxNumToCleanMemery.intValue,
                0,
                LoadedMaxNumToCleanMemeryMax
            );
            
            if (loadedMaxNumToCleanMemery != m_LoadedMaxNumToCleanMemery.intValue)
            {
                if (EditorApplication.isPlaying)
                    t.LoadedMaxNumToCleanMemery = loadedMaxNumToCleanMemery;
                else
                    m_LoadedMaxNumToCleanMemery.intValue = loadedMaxNumToCleanMemery;
            }

            // 运行时才显示各类列表
            if (EditorApplication.isPlaying)
            {
                DrawPrefabList("Prefab加载完成列表");

                DrawAssetList("Asset预加载列表");
                DrawAssetList("Asset加载中列表");
                DrawAssetList("Asset加载完成列表");
                DrawAssetList("Asset准备卸载列表");

                DrawABList("AB准备列表");
                DrawABList("AB加载中列表");
                DrawABList("AB加载完成列表");
                DrawABList("AB待卸载列表");
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        //=========================================================================
        // 编译完成回调
        //=========================================================================
        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
            RefreshTypeNames();
        }

        //=========================================================================
        // 启用时初始化绑定序列化属性
        //=========================================================================
        private void OnEnable()
        {
            m_UnloadAssetDelayFrameNum  = serializedObject.FindProperty("m_UnloadAssetDelayFrameNum");
            m_LoadedMaxNumToCleanMemery = serializedObject.FindProperty("m_LoadedMaxNumToCleanMemery");

            RefreshTypeNames();
        }
        #endregion

        #region 【绘制列表】
        //=========================================================================
        // 绘制Prefab加载列表
        //=========================================================================
        private void DrawPrefabList(string listName)
        {
            AssetComponent t = (AssetComponent)target;
            Dictionary<string, PrefabObject> prefabList = null;

            switch (listName)
            {
                case "Prefab加载完成列表": 
                    prefabList = t.LoadedPrefabList; 
                    break;
            }

            // 折叠栏
            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, prefabList.Count));
            
            if (currentState != lastState)
            {
                if (currentState) 
                    m_OpenedItems.Add(listName);
                else 
                    m_OpenedItems.Remove(listName);
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (prefabList != null && prefabList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Prefab名称", "引用\t其他信息");
                        foreach (var itr in prefabList)
                        {
                            EditorGUILayout.LabelField(
                                itr.Value.AssetName, 
                                AorTxt.Format("{0}\t [AB路径]{1}", itr.Value.RefCount, itr.Value.AssetBundlePath)
                            );
                        }

                        // 导出CSV
                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            ExportPrefabListToCSV(listName, prefabList);
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }
        }

        //=========================================================================
        // 绘制普通资源列表（预加载/加载中/已完成/待卸载）
        //=========================================================================
        private void DrawAssetList(string listName)
        {
            AssetComponent t = (AssetComponent)target;

            Dictionary<string, AssetObject> assetList = null;
            Queue<PreloadAssetObject> preloadedAssetList = null;

            switch (listName)
            {
                case "Asset加载中列表":     assetList = t.LoadingAssetList; break;
                case "Asset加载完成列表":   assetList = t.LoadedAssetList; break;
                case "Asset准备卸载列表":   assetList = t.UnloadAssetList; break;
                case "Asset预加载列表":     preloadedAssetList = t.PreloadedAssetList; break;
            }

            int count = preloadedAssetList == null ? assetList.Count : preloadedAssetList.Count;
            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, count));
            
            if (currentState != lastState)
            {
                if (currentState) 
                    m_OpenedItems.Add(listName);
                else 
                    m_OpenedItems.Remove(listName);
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (assetList != null && assetList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Asset名称", "引用\t常驻\t过期\t来源\t其他信息");
                        foreach (var itr in assetList)
                        {
                            string origin = GetOriginTypeName(itr.Value.Origin);
                            string typeName = FillGap(itr.Value.TypeName);
                            EditorGUILayout.LabelField(
                                itr.Value.AssetName, 
                                AorTxt.Format("{0}\t{1}\t{2}\t{3}\t[类型]{4} \t\t[AB路径]{5}", 
                                    itr.Value.RefCount, 
                                    (!itr.Value.IsWeak).ToString(), 
                                    itr.Value.UnloadTickNum, 
                                    origin, 
                                    typeName, 
                                    itr.Value.AssetBundlePath
                                )
                            );
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            ExportAssetListToCSV(listName, assetList);
                            GUIUtility.ExitGUI();
                        }
                    }
                    else if (preloadedAssetList != null && preloadedAssetList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Asset名称", "常驻\tAB路径\t类型");
                        foreach (var obj in preloadedAssetList)
                        {
                            string typeName = FillGap(obj.TypeName);
                            EditorGUILayout.LabelField(
                                obj.AssetName, 
                                AorTxt.Format("{0}\t{1}\t{2}", (!obj.IsWeak).ToString(), obj.AssetBundlePath, typeName)
                            );
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            ExportPreloadAssetToCSV(listName, preloadedAssetList);
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }
        }

        //=========================================================================
        // 绘制AssetBundle列表
        //=========================================================================
        private void DrawABList(string listName)
        {
            AssetComponent t = (AssetComponent)target;
            Dictionary<string, AssetBundleObject> abList = null;

            switch (listName)
            {
                case "AB准备列表":      abList = t.ReadyABList; break;
                case "AB加载中列表":    abList = t.LoadingABList; break;
                case "AB加载完成列表":  abList = t.LoadedABList; break;
                case "AB待卸载列表":    abList = t.UnloadABList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, abList.Count));
            
            if (currentState != lastState)
            {
                if (currentState) 
                    m_OpenedItems.Add(listName);
                else 
                    m_OpenedItems.Remove(listName);
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (abList != null && abList.Count > 0)
                    {
                        EditorGUILayout.LabelField("AB格式化路径", "引用\t来源\t依赖");
                        foreach (var itr in abList)
                        {
                            string origin = GetOriginTypeName(itr.Value.Origin);
                            EditorGUILayout.LabelField(
                                itr.Value.FormatPath, 
                                AorTxt.Format("{0}\t{1}\t{2}", itr.Value.RefCount, origin, itr.Value.DependLoadingCount)
                            );
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            ExportABListToCSV(listName, abList);
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }
        }
        #endregion

        #region 【CSV 导出】
        //=========================================================================
        // 导出Prefab列表
        //=========================================================================
        private void ExportPrefabListToCSV(string listName, Dictionary<string, PrefabObject> data)
        {
            string path = EditorUtility.SaveFilePanel("导出 CSV", "", $"{listName} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv", "");
            if (string.IsNullOrEmpty(path)) 
                return;

            try
            {
                List<string> lines = new List<string>();
                lines.Add("Prefab名称,引用,AB路径");
                foreach (var itr in data)
                {
                    lines.Add($"{itr.Value.AssetName},{itr.Value.RefCount},{itr.Value.AssetBundlePath}");
                }
                File.WriteAllLines(path, lines, Encoding.UTF8);
                Log.Debug($"导出成功：{path}");
            }
            catch (Exception e)
            {
                Log.Error($"导出失败：{e}");
            }
        }

        //=========================================================================
        // 导出普通资源
        //=========================================================================
        private void ExportAssetListToCSV(string listName, Dictionary<string, AssetObject> data)
        {
            string path = EditorUtility.SaveFilePanel("导出 CSV", "", $"{listName} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv", "");
            if (string.IsNullOrEmpty(path)) 
                return;

            try
            {
                List<string> lines = new List<string>();
                lines.Add("Asset名称,引用,常驻,过期帧数,来源,类型,AB路径");
                foreach (var itr in data)
                {
                    string origin = GetOriginTypeName(itr.Value.Origin);
                    lines.Add($"{itr.Value.AssetName},{itr.Value.RefCount},{!itr.Value.IsWeak},{itr.Value.UnloadTickNum},{origin},{itr.Value.TypeName},{itr.Value.AssetBundlePath}");
                }
                File.WriteAllLines(path, lines, Encoding.UTF8);
                Log.Debug($"导出成功：{path}");
            }
            catch (Exception e)
            {
                Log.Error($"导出失败：{e}");
            }
        }

        //=========================================================================
        // 导出预加载资源
        //=========================================================================
        private void ExportPreloadAssetToCSV(string listName, Queue<PreloadAssetObject> data)
        {
            string path = EditorUtility.SaveFilePanel("导出 CSV", "", $"{listName} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv", "");
            if (string.IsNullOrEmpty(path)) 
                return;

            try
            {
                List<string> lines = new List<string>();
                lines.Add("Asset名称,常驻,AB路径,类型");
                foreach (var itr in data)
                {
                    lines.Add($"{itr.AssetName},{!itr.IsWeak},{itr.AssetBundlePath},{itr.TypeName}");
                }
                File.WriteAllLines(path, lines, Encoding.UTF8);
                Log.Debug($"导出成功：{path}");
            }
            catch (Exception e)
            {
                Log.Error($"导出失败：{e}");
            }
        }

        //=========================================================================
        // 导出AB列表
        //=========================================================================
        private void ExportABListToCSV(string listName, Dictionary<string, AssetBundleObject> data)
        {
            string path = EditorUtility.SaveFilePanel("导出 CSV", "", $"{listName} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv", "");
            if (string.IsNullOrEmpty(path)) 
                return;

            try
            {
                List<string> lines = new List<string>();
                lines.Add("AB格式化路径,引用,来源,依赖加载数量");
                foreach (var itr in data)
                {
                    string origin = GetOriginTypeName(itr.Value.Origin);
                    lines.Add($"{itr.Value.FormatPath},{itr.Value.RefCount},{origin},{itr.Value.DependLoadingCount}");
                }
                File.WriteAllLines(path, lines, Encoding.UTF8);
                Log.Debug($"导出成功：{path}");
            }
            catch (Exception e)
            {
                Log.Error($"导出失败：{e}");
            }
        }
        #endregion

        #region 【工具方法】
        //=========================================================================
        // 刷新类型名称
        //=========================================================================
        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }

        //=========================================================================
        // 获取来源类型名称
        //=========================================================================
        private string GetOriginTypeName(OriginType type)
        {
            return type switch
            {
                OriginType.None => "N",
                OriginType.Editor => "E",
                OriginType.Persistent => "P",
                OriginType.Streaming => "S",
                _ => "?"
            };
        }

        //=========================================================================
        // 补齐空格格式化显示
        //=========================================================================
        private string FillGap(string word, int length = 15)
        {
            if (word.Length >= length)
                return word.Substring(0, length);
            return word.PadRight(length);
        }
        #endregion
    }
}