using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(AssetComponent))]
    public class AssetComponentInspector: HonorComponentInspector
    {
         private const int UnloadAssetDelayFrameNumMax = 36000;
        private const int LoadedMaxNumToCleanMemeryMax = 1000;

        private SerializedProperty m_UnloadAssetDelayFrameNum = null;
        private SerializedProperty m_LoadedMaxNumToCleanMemery = null;

        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            AssetComponent t = (AssetComponent)target;

            bool isEditorResourceMode = t.EditorResourceMode;

            if (isEditorResourceMode)
            {
                EditorGUILayout.HelpBox("当前为编辑器资源模式，个别选项功能会失效。", MessageType.Warning);
            }

            int unloadAssetDelayFrameNum = (int)EditorGUILayout.Slider(AorTxt.Format("Asset最小过期帧数"), m_UnloadAssetDelayFrameNum.intValue, 0, UnloadAssetDelayFrameNumMax);
            if (unloadAssetDelayFrameNum != m_UnloadAssetDelayFrameNum.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.UnloadAssetDelayFrameNum = unloadAssetDelayFrameNum;
                }
                else
                {
                    m_UnloadAssetDelayFrameNum.intValue = unloadAssetDelayFrameNum;
                }
            }

            int loadedMaxNumToCleanMemery = (int)EditorGUILayout.Slider("每轮GC所需异步加载完成资源的累计数量", m_LoadedMaxNumToCleanMemery.intValue, 0, LoadedMaxNumToCleanMemeryMax);
            if (loadedMaxNumToCleanMemery != m_LoadedMaxNumToCleanMemery.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.LoadedMaxNumToCleanMemery = loadedMaxNumToCleanMemery;
                }
                else
                {
                    m_LoadedMaxNumToCleanMemery.intValue = loadedMaxNumToCleanMemery;
                }
            }

            DrawPrefabList("Prefab加载完成列表");

            DrawAssetList("Asset预加载列表");
            DrawAssetList("Asset加载中列表");
            DrawAssetList("Asset加载完成列表");
            DrawAssetList("Asset准备卸载列表");

            DrawABList("AB准备列表");
            DrawABList("AB加载中列表");
            DrawABList("AB加载完成列表");
            DrawABList("AB待卸载列表");

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_UnloadAssetDelayFrameNum = serializedObject.FindProperty("m_UnloadAssetDelayFrameNum");
            m_LoadedMaxNumToCleanMemery = serializedObject.FindProperty("m_LoadedMaxNumToCleanMemery");

            RefreshTypeNames();
        }

        private void DrawPrefabList(string listName)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            AssetComponent t = (AssetComponent)target;

            Dictionary<string, PrefabObject> prefabList = null;

            switch (listName)
            {
                case "Prefab加载完成列表": prefabList = t.LoadedPrefabList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, prefabList.Count));
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
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
                            EditorGUILayout.LabelField(itr.Value.AssetName, AorTxt.Format("{0}\t [AB路径]{1}", itr.Value.RefCount, itr.Value.AssetBundlePath));
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = prefabList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, AorTxt.Format("{0} {1}.csv", listName, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[prefabList.Count + 1];
                                    data[index++] = "Prefab名称,引用,AB路径";
                                    foreach (var itr in prefabList)
                                    {
                                        data[index++] = AorTxt.Format("{0},{1},{2}", itr.Value.AssetName, itr.Value.RefCount, itr.Value.AssetBundlePath);
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Log.Debug(AorTxt.Format("导出 CSV 数据到 '{0}' 成功。", exportFileName));
                                }
                                catch (Exception exception)
                                {
                                    Log.Error(AorTxt.Format("导出 CSV 数据到 '{0}' 失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                                }
                            }
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

        private void DrawAssetList(string listName)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            AssetComponent t = (AssetComponent)target;

            Dictionary<string, AssetObject> assetList = null;
            Queue<PreloadAssetObject> preloadedAssetList = null;
            switch (listName)
            {
                case "Asset加载中列表": assetList = t.LoadingAssetList; break;
                case "Asset加载完成列表": assetList = t.LoadedAssetList; break;
                case "Asset准备卸载列表": assetList = t.UnloadAssetList; break;
                case "Asset预加载列表": preloadedAssetList = t.PreloadedAssetList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, preloadedAssetList == null ? assetList.Count : preloadedAssetList.Count));
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
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
                            string origin = string.Empty;
                            switch(itr.Value.Origin)
                            {
                                case OriginType.None: origin = "N"; break;
                                case OriginType.Editor: origin = "E"; break;
                                case OriginType.Persistent: origin = "P"; break;
                                case OriginType.Streaming: origin = "S"; break;
                            }
                            string typeName = FillGap(itr.Value.TypeName);
                            EditorGUILayout.LabelField(itr.Value.AssetName, AorTxt.Format("{0}\t{1}\t{2}\t{3}\t[类型]{4} \t\t[AB路径]{5}", itr.Value.RefCount, (!itr.Value.IsWeak).ToString(), itr.Value.UnloadTickNum, origin, typeName, itr.Value.AssetBundlePath));
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = assetList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, AorTxt.Format("{0} {1}.csv", listName, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[assetList.Count + 1];
                                    data[index++] = "Asset名称,引用,常驻,过期帧数,来源,类型,AB路径";
                                    foreach (var itr in assetList)
                                    {
                                        string origin = string.Empty;
                                        switch (itr.Value.Origin)
                                        {
                                            case OriginType.None: origin = "N"; break;
                                            case OriginType.Editor: origin = "E"; break;
                                            case OriginType.Persistent: origin = "P"; break;
                                            case OriginType.Streaming: origin = "S"; break;
                                        }
                                        string typeName = FillGap(itr.Value.TypeName);
                                        data[index++] = AorTxt.Format("{0},{1},{2},{3},{4},{5},{6}", itr.Value.AssetName, itr.Value.RefCount, (!itr.Value.IsWeak).ToString(), itr.Value.UnloadTickNum, origin, typeName, itr.Value.AssetBundlePath);
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Log.Debug(AorTxt.Format("导出 CSV 数据到 '{0}' 成功。", exportFileName));
                                }
                                catch (Exception exception)
                                {
                                    Log.Error(AorTxt.Format("导出 CSV 数据到 '{0}' 失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                                }
                            }
                            GUIUtility.ExitGUI();
                        }
                    }
                    else if (preloadedAssetList != null && preloadedAssetList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Asset名称", "常驻\tAB路径\t类型");
                        foreach (var preloadAssetObj in preloadedAssetList)
                        {
                            string typeName = FillGap(preloadAssetObj.TypeName);
                            EditorGUILayout.LabelField(preloadAssetObj.AssetName, AorTxt.Format("{0}\t{1}\t{2}", (!preloadAssetObj.IsWeak).ToString(), preloadAssetObj.AssetBundlePath, typeName));
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = assetList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, AorTxt.Format("{0} {1}.csv", listName, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[assetList.Count + 1];
                                    data[index++] = "Asset名称,常驻,AB路径,类型";
                                    foreach (var itr in assetList)
                                    {
                                        string typeName = FillGap(itr.Value.TypeName);
                                        data[index++] = AorTxt.Format("{0},{1},{2},{3}", itr.Value.AssetName, (!itr.Value.IsWeak).ToString(), itr.Value.AssetBundlePath, typeName);
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Log.Debug(AorTxt.Format("导出 CSV 数据到 '{0}' 成功。", exportFileName));
                                }
                                catch (Exception exception)
                                {
                                    Log.Error(AorTxt.Format("导出 CSV 数据到 '{0}' 失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                                }
                            }
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

        private void DrawABList(string listName)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            AssetComponent t = (AssetComponent)target;

            Dictionary<string, AssetBundleObject> abList = null;
            switch (listName)
            {
                case "AB准备列表": abList = t.ReadyABList; break;
                case "AB加载中列表": abList = t.LoadingABList; break;
                case "AB加载完成列表": abList = t.LoadedABList; break;
                case "AB待卸载列表": abList = t.UnloadABList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, abList.Count));
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
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
                            string origin = string.Empty;
                            switch (itr.Value.Origin)
                            {
                                case OriginType.None: origin = "N"; break;
                                case OriginType.Editor: origin = "E"; break;
                                case OriginType.Persistent: origin = "P"; break;
                                case OriginType.Streaming: origin = "S"; break;
                            }
                            EditorGUILayout.LabelField(itr.Value.FormatPath, AorTxt.Format("{0}\t{1}\t{2}", itr.Value.RefCount, origin, itr.Value.DependLoadingCount));
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = abList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, AorTxt.Format("{0} {1}.csv", listName, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[abList.Count + 1];
                                    data[index++] = "AB格式化路径,引用,来源,依赖";
                                    foreach (var itr in abList)
                                    {
                                        string origin = string.Empty;
                                        switch (itr.Value.Origin)
                                        {
                                            case OriginType.None: origin = "N"; break;
                                            case OriginType.Editor: origin = "E"; break;
                                            case OriginType.Persistent: origin = "P"; break;
                                            case OriginType.Streaming: origin = "S"; break;
                                        }
                                        data[index++] = AorTxt.Format("{0},{1},{2},{3}", itr.Value.FormatPath, itr.Value.RefCount, origin, itr.Value.DependLoadingCount);
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Log.Debug(AorTxt.Format("导出 CSV 数据到 '{0}' 成功。", exportFileName));
                                }
                                catch (Exception exception)
                                {
                                    Log.Error(AorTxt.Format("导出 CSV 数据到 '{0}' 失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                                }
                            }
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

        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }

        private string FillGap(string word, int length = 15)
        {
            while(word.Length < length)
            {
                word += " ";
            }
            word = word.Substring(0, length);
            return word;
        }
    }
}