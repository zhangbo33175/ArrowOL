/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PersistComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   持久化组件编辑器检视面板 - 存档数据编辑、Proto导出、序列化管理
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Honor.Runtime;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using XLua;

namespace Honor.Editor
{
    /// <summary>
    /// 持久化组件编辑器检视面板
    /// 提供存档数据编辑、Proto协议导出、数据序列化/反序列化、运行时数据查看功能
    /// </summary>
    [CustomEditor(typeof(PersistComponent))]
    internal sealed partial class PersistComponentInspector : HonorComponentInspector
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region 字段定义
        /// <summary>
        /// 展开的分类项集合
        /// </summary>
        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

        /// <summary>
        /// 持久化数据值集合
        /// 结构：持久化方式 -> 分类名 -> 键值对
        /// </summary>
        private readonly SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, string>>> m_ValueList = new SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, string>>>();

        /// <summary>
        /// 数据编辑状态集合
        /// </summary>
        private readonly SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, bool>>> m_EditStateList = new SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, bool>>>();

        /// <summary>
        /// 编辑中的数据内容集合
        /// </summary>
        private readonly SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, string>>> m_EditValueList = new SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, string>>>();

        /// <summary>
        /// 编辑数据缓存集合
        /// </summary>
        private readonly SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, string>>> m_EditValueCacheList = new SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, string>>>();

        /// <summary>
        /// 滚动视图位置记录集合
        /// </summary>
        private readonly SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, Vector2>>> m_EditScrollViewPositionList = new SortedDictionary<PersistWayType, SortedDictionary<string, SortedDictionary<string, Vector2>>>();

        /// <summary>
        /// 文件片段根目录绝对路径
        /// </summary>
        private string m_FileFragmentsRootDirectoryFullPath = string.Empty;

        /// <summary>
        /// 文件片段名称列表
        /// </summary>
        private List<string> m_FileFragmentNames = new List<string>();

        /// <summary>
        /// 文件片段完整路径列表
        /// </summary>
        private List<string> m_FilePaths = new List<string>();

        /// <summary>
        /// 当前是否为枚举类型
        /// </summary>
        private static bool s_IsCurEnum = false;

        /// <summary>
        /// 当前类名列表
        /// </summary>
        private static List<string> s_CurClsName = new List<string>();

        /// <summary>
        /// 运行时已解码的PB数据缓存
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_RuntimeLastDecodedPbValue = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 运行时Proto协议结构信息
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_RuntimeClassInterFieldTypes = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        //=========================================================================
        // 生命周期
        //=========================================================================
        #region 生命周期方法
        /// <summary>
        /// 启用时初始化数据与状态
        /// </summary>
        private void OnEnable()
        {
            RefreshTypeNames();

            // 非运行时逻辑处理（读取文件中的数据结构）
            if (!EditorApplication.isPlaying)
            {
                m_ValueList.Clear();
                m_ValueList.Add(PersistWayType.PlayerPrefs, new SortedDictionary<string, SortedDictionary<string, string>>());
                m_ValueList.Add(PersistWayType.FileFragment, new SortedDictionary<string, SortedDictionary<string, string>>());

                m_EditStateList.Clear();
                m_EditStateList.Add(PersistWayType.PlayerPrefs, new SortedDictionary<string, SortedDictionary<string, bool>>());
                m_EditStateList.Add(PersistWayType.FileFragment, new SortedDictionary<string, SortedDictionary<string, bool>>());

                m_EditValueList.Clear();
                m_EditValueList.Add(PersistWayType.PlayerPrefs, new SortedDictionary<string, SortedDictionary<string, string>>());
                m_EditValueList.Add(PersistWayType.FileFragment, new SortedDictionary<string, SortedDictionary<string, string>>());

                m_EditValueCacheList.Clear();
                m_EditValueCacheList.Add(PersistWayType.PlayerPrefs, new SortedDictionary<string, SortedDictionary<string, string>>());
                m_EditValueCacheList.Add(PersistWayType.FileFragment, new SortedDictionary<string, SortedDictionary<string, string>>());

                m_EditScrollViewPositionList.Clear();
                m_EditScrollViewPositionList.Add(PersistWayType.PlayerPrefs, new SortedDictionary<string, SortedDictionary<string, Vector2>>());
                m_EditScrollViewPositionList.Add(PersistWayType.FileFragment, new SortedDictionary<string, SortedDictionary<string, Vector2>>());

                m_FileFragmentNames.Clear();
                m_FilePaths.Clear();

                LoadDatasOnEditorMode(PersistWayType.PlayerPrefs);
                LoadDatasOnEditorMode(PersistWayType.FileFragment);
            }
            else
            {
                LoadDatasOnRuntimeMode(PersistWayType.PlayerPrefs);
            }
        }

        /// <summary>
        /// 绘制检视面板GUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.LabelField("存档协议配置（仅支持数据库持久化）");

            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("导出存档协议Proto到Lua"))
            {
                // 生成所有存档proto.lua脚本
                GenerateLuaProtos();

                // 生成所有存档Proto协议到EmmyLua注释
                GenerateLuaDeclares(out List<List<string>> luaFilesDeclaresLines);

                // 生成所有存档Proto协议到Lua数据结构
                GenerateLuaSaveDatas(luaFilesDeclaresLines);

                OnEnable();

                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开存档Proto文件夹"))
            {
                OpenProtoFolder();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndHorizontal();

            PersistComponent t = (PersistComponent)target;

            // 运行时逻辑处理（读取程序中的数据结构）
            if (EditorApplication.isPlaying)
            {
                RefreshGUIOnRuntimeMode(PersistWayType.PlayerPrefs);
                RefreshGUIOnRuntimeMode(PersistWayType.FileFragment);
            }
            else // 非运行时逻辑处理（读取文件中的数据结构）
            {
                RefreshGUIOnEditorMode(PersistWayType.PlayerPrefs);
                RefreshGUIOnEditorMode(PersistWayType.FileFragment);
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        /// <summary>
        /// 编译完成回调
        /// </summary>
        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
            RefreshTypeNames();
        }
        #endregion

        //=========================================================================
        // 公共操作 - 文件夹/类型刷新
        //=========================================================================
        #region 公共操作方法
        /// <summary>
        /// 打开Proto协议存放文件夹
        /// </summary>
        private void OpenProtoFolder()
        {
            string folderPath = Runtime.GamePathUtils.Proto.Save.GetRootDirectoryFullPath();
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start(folderPath);
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folderPath);
                    break;
                default:
                    throw new Exception(AorTxt.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
            }
        }

        /// <summary>
        /// 刷新序列化对象类型名称
        /// </summary>
        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        //=========================================================================
        // Proto解析与结构解析
        //=========================================================================
        #region Proto结构解析
        /// <summary>
        /// 解析Proto文件，获取主结构字段详情
        /// </summary>
        /// <param name="totalList">Lua声明文件内容</param>
        /// <returns>结构字段类型字典</returns>
        private Dictionary<string, Dictionary<string, string>> GetPbFileMainStructorDetailInfos(List<List<string>> totalList)
        {
            void ___recusive(string className, string structorName, string prefix, Dictionary<string, Dictionary<string, string>> classFieldTypes, Dictionary<string, string> baseInterFieldTypes)
            {
                foreach (var itr in classFieldTypes[className])
                {
                    string tmpPrefix = prefix + (string.IsNullOrEmpty(prefix) ? $"{itr.Key}" : $".{itr.Key}");
                    if (itr.Value.StartsWith("SavePBMsgDef.") && !itr.Value.EndsWith("[]"))
                    {
                        ___recusive(itr.Value, structorName, tmpPrefix, classFieldTypes, baseInterFieldTypes);
                    }
                    else
                    {
                        baseInterFieldTypes.Add(structorName + "." + tmpPrefix, classFieldTypes[className][itr.Key]);
                    }
                }
            }

            Dictionary<string, Dictionary<string, string>> classInterFieldTypes = new Dictionary<string, Dictionary<string, string>>();

            List<string> fullNameSaveDataUnderRoot = new List<string>();
            List<string> tidyNameSaveDataUnderRoot = new List<string>();
            List<string> typeNameSaveDataUnderRoot = new List<string>();
            List<string> descNoteSaveDataUnderRoot = new List<string>();

            // 生成存档主结构关键信息的收集
            for (int indexOfList = 0; indexOfList < totalList.Count; indexOfList++)
            {
                List<string> list = totalList[indexOfList];

                string fileName = string.Empty;
                string fileDescNote = string.Empty;

                // 开始清理冗余内容
                for (int index = list.Count - 1; index >= 0; index--)
                {
                    string content = list[index];
                    if (content.StartsWith("--=====================================================================================================") ||
                        content.StartsWith("-- (c)copyright") ||
                        content.StartsWith("-- All Rights Reserved.") ||
                        content.StartsWith("-- ----------------------------------------------------------------------------------------------------") ||
                        content.StartsWith("-- filename:") ||
                        content.StartsWith("-- descrip:") ||
                        content.StartsWith("-- notices:"))
                    {
                        if (content.StartsWith("-- filename:"))
                        {
                            fileName = content.Split(' ')[2].Replace("Declare", string.Empty).Replace(".lua", string.Empty);
                        }
                        else if (content.StartsWith("-- descrip:"))
                        {
                            fileDescNote = content.Split(' ')[2];
                        }
                        list.RemoveAt(index);
                    }
                }

                // 解析所有类型的成员定义
                string classDescNote = string.Empty;
                string className = string.Empty;

                Dictionary<string, string> classNoteInfos = new Dictionary<string, string>();
                Dictionary<string, Dictionary<string, string>> classFieldTypes = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, Dictionary<string, string>> classFieldNotes = new Dictionary<string, Dictionary<string, string>>();

                for (int index = 0; index < list.Count; index++)
                {
                    string content = list[index];

                    // 注释
                    if (!content.StartsWith("-- 协议描述") && content.StartsWith("-- "))
                    {
                        classDescNote = content.Split(' ')[1];
                    }
                    else if (content.StartsWith("---@class")) // 类型定义
                    {
                        string[] strs = content.Split(' ');
                        className = strs[1];
                        classNoteInfos.Add(className, classDescNote);
                        classFieldTypes.Add(className, new Dictionary<string, string>());
                        classFieldNotes.Add(className, new Dictionary<string, string>());
                    }
                    else if (content.StartsWith("---@field"))  // 成员类型
                    {
                        string[] strs = content.Split(' ');
                        string fieldName = strs[1];
                        string fieldType = strs[2];
                        string fieldDescNote = strs[4];
                        classFieldTypes[className].Add(fieldName, fieldType);
                        classFieldNotes[className].Add(fieldName, fieldDescNote);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(className))
                        {
                            classDescNote = string.Empty;
                            className = string.Empty;
                        }
                    }
                }

                classDescNote = string.Empty;
                className = string.Empty;

                // 生成存档主结构lua文件
                foreach (var classNoteInfo in classNoteInfos)
                {
                    if (classNoteInfo.Value.StartsWith("[主结构]"))
                    {
                        className = classNoteInfo.Key;
                        classDescNote = classNoteInfo.Value;

                        fullNameSaveDataUnderRoot.Add(className.Split('.')[1]);
                        tidyNameSaveDataUnderRoot.Add(className.Split('.')[2]);
                        typeNameSaveDataUnderRoot.Add(className);
                        descNoteSaveDataUnderRoot.Add(classDescNote);

                        string fullName = className.Split('.')[1];
                        string structorName = fullName;
                        classInterFieldTypes.Add(fullName, new Dictionary<string, string>());

                        ___recusive(className, structorName, "", classFieldTypes, classInterFieldTypes[fullName]);

                        break;
                    }
                }
            }
            return classInterFieldTypes;
        }
        #endregion

        //=========================================================================
        // PB数据编码/解码
        //=========================================================================
        #region PB编解码
        /// <summary>
        /// PB消息数据编码（Base64+Protobuf）
        /// </summary>
        /// <param name="protoLuaFileName">Proto文件名</param>
        /// <param name="msgName">消息名</param>
        /// <param name="contentString">原始内容</param>
        /// <returns>编码后字符串</returns>
        private string EncodePbMsgData(string protoLuaFileName, string msgName, string contentString)
        {
            LuaEnv luaEnv = new LuaEnv();
            luaEnv.AddLoader((ref string filePathName) =>
            {
                TextAsset asset = (TextAsset)AssetDatabase.LoadAssetAtPath(filePathName, typeof(TextAsset));
                return asset.bytes;
            });

            object[] results = luaEnv.DoString(AorTxt.Format(@"
                require('Assets/LuaScripts/Honor/Singleton/Systems/libs/SystemFuncs.lua.txt')
                local Base64 = require('Assets/LuaScripts/Honor/Singleton/Systems/thirds/SystemBase64.lua.txt')
                local protoc = require('Assets/LuaScripts/Honor/Singleton/Systems/thirds/SystemProtoc.lua.txt')
                local proto = require('Assets/LuaScripts/Game/PBData/Proto/{0}.proto.lua.txt')
                assert(protoc: load(proto))
                local pb = require 'pb'
                local datas = Base64.encode(pb.encode('{1}', TableDecode('{2}')))
                return datas
            ", protoLuaFileName, msgName, contentString));

            return results[0].ToString();
        }

        /// <summary>
        /// PB消息数据解码
        /// </summary>
        /// <param name="protoLuaFileName">Proto文件名</param>
        /// <param name="msgName">消息名</param>
        /// <param name="contentString">编码内容</param>
        /// <returns>解码后明文</returns>
        private string DecodePbMsgData(string protoLuaFileName, string msgName, string contentString)
        {
            LuaEnv luaEnv = new LuaEnv();
            luaEnv.AddLoader((ref string filePathName) =>
            {
                TextAsset asset = (TextAsset)AssetDatabase.LoadAssetAtPath(filePathName, typeof(TextAsset));
                return asset.bytes;
            });

            string protoLuaFilePath = AorTxt.Format("Assets/LuaScripts/Game/PBData/Proto/{0}.proto.lua.txt", protoLuaFileName);
            if(File.Exists(protoLuaFilePath))
            {
                object[] results = luaEnv.DoString(AorTxt.Format(@"
                    require('Assets/LuaScripts/Honor/Singleton/Systems/libs/SystemFuncs.lua.txt')
                    local Base64 = require('Assets/LuaScripts/Honor/Singleton/Systems/thirds/SystemBase64.lua.txt')
                    local protoc = require('Assets/LuaScripts/Honor/Singleton/Systems/thirds/SystemProtoc.lua.txt')
                    local proto = require('{0}')
                    assert(protoc: load(proto))
                    local pb = require 'pb'
                    local data = TableEncode(pb.decode('{1}', Base64.decode('{2}')))
                    return data
                ", protoLuaFilePath, msgName, contentString));
                return results[0].ToString();
            }

            return null;
        }
        #endregion

        //=========================================================================
        // 数据加载（Editor + Runtime）
        //=========================================================================
        #region 数据加载
        /// <summary>
        /// 编辑器模式下加载持久化数据
        /// </summary>
        /// <param name="persisway">持久化方式</param>
        private void LoadDatasOnEditorMode(PersistWayType persisway)
        {
            if (persisway == PersistWayType.PlayerPrefs)
            {
                // 获取所有存档Proto协议数据结构
                GenerateLuaDeclares(out List<List<string>> luaFilesDeclaresLines, false);

                // 获取Pb文件主结构的详细信息
                Dictionary<string, Dictionary<string, string>> classInterFieldTypes = GetPbFileMainStructorDetailInfos(luaFilesDeclaresLines);

                string classifyNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameList", AESEncrypt.EncodeToBase64(string.Empty)));
                if (!string.IsNullOrEmpty(classifyNameListText))
                {
                    string[] classifyNames = classifyNameListText.Split(',');
                    for (int index = 0; index < classifyNames.Length; index++)
                    {
                        string classifyName = classifyNames[index];
                        m_ValueList[PersistWayType.PlayerPrefs].Add(classifyName, new SortedDictionary<string, string>());
                        m_EditStateList[PersistWayType.PlayerPrefs].Add(classifyName, new SortedDictionary<string, bool>());
                        m_EditValueList[PersistWayType.PlayerPrefs].Add(classifyName, new SortedDictionary<string, string>());
                        m_EditValueCacheList[PersistWayType.PlayerPrefs].Add(classifyName, new SortedDictionary<string, string>());
                        m_EditScrollViewPositionList[PersistWayType.PlayerPrefs].Add(classifyName, new SortedDictionary<string, Vector2>());

                        string nameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(AorTxt.Format("Classify_{0}_ItemNameList", classifyName), AESEncrypt.EncodeToBase64(string.Empty)));
                        if (!string.IsNullOrEmpty(nameListText))
                        {
                            string[] nameList = nameListText.Split(',');
                            for (int nameIndex = 0; nameIndex < nameList.Length; nameIndex++)
                            {
                                string itemName = nameList[nameIndex];
                                string key = AorTxt.Format("{0}_{1}", classifyName, itemName);
                                string valueTryString = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key));
                                string fieldName = itemName;
                                string fieldType = classInterFieldTypes[classifyName][fieldName];
                                if (fieldType.EndsWith("[]"))
                                {
                                    fieldType = "table";
                                }
                                switch(fieldType)
                                {
                                    case "number": m_ValueList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, valueTryString); break;
                                    case "string": m_ValueList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, valueTryString); break;
                                    case "table": m_ValueList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, valueTryString); break;
                                    case "boolean": m_ValueList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, valueTryString); break;
                                }
                                m_EditStateList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, false);
                                m_EditValueList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, string.Empty);
                                m_EditValueCacheList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, string.Empty);
                                m_EditScrollViewPositionList[PersistWayType.PlayerPrefs][classifyName].Add(itemName, Vector2.zero);
                            }
                        }
                    }
                }
            }
            else if (persisway == PersistWayType.FileFragment)
            {
                m_FileFragmentsRootDirectoryFullPath = Runtime.GamePathUtils.FileFragment.GetRootDirectoryFullPath();
                if (System.IO.Directory.Exists(m_FileFragmentsRootDirectoryFullPath))
                {
                    string[] fileFragmentFullPaths = System.IO.Directory.GetFiles(m_FileFragmentsRootDirectoryFullPath, "*.dat");
                    for (int index = 0; index < fileFragmentFullPaths.Length; index++)
                    {
                        m_FilePaths.Add(fileFragmentFullPaths[index].Replace('\\', '/'));
                        int startIndex = m_FilePaths[index].LastIndexOf('/') + 1;
                        string fileFragmentName = m_FilePaths[index].Substring(startIndex, m_FilePaths[index].Length - startIndex - ".dat".Length);
                        m_FileFragmentNames.Add(fileFragmentName);
                    }

                    // 读取文件内容
                    for (int index = 0; index < m_FilePaths.Count; index++)
                    {
                        string filePath = m_FilePaths[index];
                        string fileName = m_FileFragmentNames[index];
                        try
                        {
                            if (!System.IO.File.Exists(filePath))
                            {
                                continue;
                            }
                            Deserialize(filePath, fileName);
                        }
                        catch (Exception exception)
                        {
                            Log.Warning("加载 FileFragment 条目失败，异常为 '{0}'.", exception.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 运行时模式加载持久化数据
        /// </summary>
        /// <param name="persisway">持久化方式</param>
        private void LoadDatasOnRuntimeMode(PersistWayType persisway)
        {
            if(persisway == PersistWayType.PlayerPrefs)
            {
                // 获取所有存档Proto协议数据结构
                GenerateLuaDeclares(out List<List<string>> runtimeLuaFilesDeclaresLines, false);

                // 获取所有存档Proto协议主结构的详细信息
                m_RuntimeClassInterFieldTypes = GetPbFileMainStructorDetailInfos(runtimeLuaFilesDeclaresLines);
            }
        }
        #endregion

        //=========================================================================
        // GUI绘制（Editor + Runtime）
        //=========================================================================
        #region GUI绘制
        /// <summary>
        /// 编辑器模式刷新GUI
        /// </summary>
        /// <param name="persistWay">持久化方式</param>
        private void RefreshGUIOnEditorMode(PersistWayType persistWay)
        {
            string persistWayName = string.Empty;
            if (persistWay == PersistWayType.PlayerPrefs)
            {
                persistWayName = "数据库持久化（Editor）";
            }
            else if (persistWay == PersistWayType.FileFragment)
            {
                persistWayName = "文件片段持久化（Editor）";
            }
            bool persistWayLastState = m_OpenedItems.Contains(persistWayName);
            bool persistWayCurrentState = EditorGUILayout.Foldout(persistWayLastState, persistWayName);
            if (persistWayCurrentState != persistWayLastState)
            {
                if (persistWayCurrentState)
                {
                    m_OpenedItems.Add(persistWayName);
                }
                else
                {
                    m_OpenedItems.Remove(persistWayName);
                }
            }

            if (persistWayCurrentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (persistWay == PersistWayType.PlayerPrefs)
                    {
                        if (GUILayout.Button("删除所有数据"))
                        {
                            PlayerPrefs.DeleteAll();
                            PlayerPrefs.SetString("ClassifyNameList", AESEncrypt.EncodeToBase64(string.Empty));
                            PlayerPrefs.Save();
                            m_ValueList[persistWay].Clear();
                            m_EditStateList[persistWay].Clear();
                            m_EditValueList[persistWay].Clear();
                            m_EditValueCacheList[persistWay].Clear();
                            m_EditScrollViewPositionList[persistWay].Clear();
                            GUIUtility.ExitGUI();
                        }
                    }
                    else if (persistWay == PersistWayType.FileFragment)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        if (GUILayout.Button("删除所有数据"))
                        {
                            if (System.IO.Directory.Exists(m_FileFragmentsRootDirectoryFullPath))
                            {
                                System.IO.Directory.Delete(m_FileFragmentsRootDirectoryFullPath, true);
                            }
                            System.IO.Directory.CreateDirectory(m_FileFragmentsRootDirectoryFullPath);
                            m_ValueList[persistWay].Clear();
                            m_EditStateList[persistWay].Clear();
                            m_EditValueList[persistWay].Clear();
                            m_EditValueCacheList[persistWay].Clear();
                            m_EditScrollViewPositionList[persistWay].Clear();
                            GUIUtility.ExitGUI();
                        }
                        if (GUILayout.Button("打开文件片段所在文件夹"))
                        {
                            string folder = AorTxt.Format("\"{0}\"", Runtime.GamePathUtils.FileFragment.GetRootDirectoryFullPath());
                            switch (Application.platform)
                            {
                                case RuntimePlatform.WindowsEditor:
                                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                                    break;

                                case RuntimePlatform.OSXEditor:
                                    Process.Start("open", folder);
                                    break;
                                default:
                                    throw new Exception(AorTxt.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                            }
                            GUIUtility.ExitGUI();
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (m_ValueList[persistWay].Keys.Count > 0)
                    {
                        List<string> classifyNames = new List<string>(m_ValueList[persistWay].Keys);
                        for (int classifyNameIndex = 0; classifyNameIndex < classifyNames.Count; classifyNameIndex++)
                        {
                            string classifyName = classifyNames[classifyNameIndex];
                            bool classifyNameLastState = m_OpenedItems.Contains(classifyName);
                            bool classifyNameCurrentState = EditorGUILayout.Foldout(classifyNameLastState, classifyName);
                            if (classifyNameCurrentState != classifyNameLastState)
                            {
                                if (classifyNameCurrentState)
                                {
                                    m_OpenedItems.Add(classifyName);
                                }
                                else
                                {
                                    m_OpenedItems.Remove(classifyName);
                                }
                            }

                            if (classifyNameCurrentState)
                            {
                                EditorGUILayout.BeginVertical("box");
                                {
                                    if (persistWay == PersistWayType.PlayerPrefs)
                                    {
                                        if (GUILayout.Button("删除当前分类下所有数据"))
                                        {
                                            List<string> itemNamesForDelete = new List<string>(m_ValueList[persistWay][classifyName].Keys);
                                            for (int i = 0; i < itemNamesForDelete.Count; i++)
                                            {
                                                string key = AorTxt.Format("{0}_{1}", classifyName, itemNamesForDelete[i]);
                                                if (PlayerPrefs.HasKey(key))
                                                {
                                                    PlayerPrefs.DeleteKey(key);
                                                }
                                            }
                                            string classifyKey = AorTxt.Format("Classify_{0}_ItemNameList", classifyName);
                                            if (PlayerPrefs.HasKey(classifyKey))
                                            {
                                                PlayerPrefs.DeleteKey(classifyKey);
                                            }
                                            string classifyNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameList", AESEncrypt.EncodeToBase64(string.Empty)));
                                            if (!string.IsNullOrEmpty(classifyNameListText))
                                            {
                                                List<string> classifyNameList = new List<string>(classifyNameListText.Split(','));
                                                classifyNameList.Remove(classifyName);

                                                classifyNameListText = string.Empty;
                                                foreach (string newClassifyName in classifyNameList)
                                                {
                                                    classifyNameListText = string.IsNullOrEmpty(classifyNameListText) ? newClassifyName : AorTxt.Format("{0},{1}", classifyNameListText, newClassifyName);
                                                }
                                                PlayerPrefs.SetString("ClassifyNameList", AESEncrypt.EncodeToBase64(classifyNameListText));
                                            }
                                            PlayerPrefs.Save();
                                            m_ValueList[persistWay].Remove(classifyName);
                                            m_EditStateList[persistWay].Remove(classifyName);
                                            m_EditValueList[persistWay].Remove(classifyName);
                                            m_EditValueCacheList[persistWay].Remove(classifyName);
                                            m_EditScrollViewPositionList[persistWay].Remove(classifyName);
                                            GUIUtility.ExitGUI();
                                        }

                                        ShowPlayerPrefsItemsOnEditorMode(classifyName);
                                    }
                                    else if (persistWay == PersistWayType.FileFragment)
                                    {
                                        if (GUILayout.Button("删除当前分类下所有数据"))
                                        {
                                            string fullPath = AorTxt.Format("{0}/{1}.dat", m_FileFragmentsRootDirectoryFullPath, classifyName);
                                            if (System.IO.File.Exists(fullPath))
                                            {
                                                System.IO.File.Delete(fullPath);
                                            }
                                            m_ValueList[persistWay].Remove(classifyName);
                                            m_EditStateList[persistWay].Remove(classifyName);
                                            m_EditValueList[persistWay].Remove(classifyName);
                                            m_EditValueCacheList[persistWay].Remove(classifyName);
                                            m_EditScrollViewPositionList[persistWay].Remove(classifyName);
                                            GUIUtility.ExitGUI();
                                        }

                                        List<string> itemNames = new List<string>(m_ValueList[persistWay][classifyName].Keys);
                                        for (int i = 0; i < itemNames.Count; i++)
                                        {
                                            string itemName = itemNames[i];
                                            string itemValue = m_ValueList[persistWay][classifyName][itemNames[i]];

                                            EditorGUILayout.BeginHorizontal("box");
                                            {
                                                EditorGUILayout.LabelField(itemName);
                                                EditorGUILayout.LabelField("编辑窗口", new GUILayoutOption[] { GUILayout.Width(50) });
                                                bool state = EditorGUILayout.Toggle(m_EditStateList[persistWay][classifyName][itemName], new GUILayoutOption[] { GUILayout.Width(20) });
                                                if (state != m_EditStateList[persistWay][classifyName][itemName])
                                                {
                                                    m_EditStateList[persistWay][classifyName][itemName] = state;
                                                    m_EditValueList[persistWay][classifyName][itemName] = state ? SwitchToReadableFormat(itemValue) : string.Empty;
                                                    m_EditValueCacheList[persistWay][classifyName][itemName] = m_EditValueList[persistWay][classifyName][itemName];
                                                    m_EditScrollViewPositionList[persistWay][classifyName][itemName] = Vector2.zero;
                                                }
                                                EditorGUI.BeginDisabledGroup((!m_EditStateList[persistWay][classifyName][itemName]) || (m_EditValueList[persistWay][classifyName][itemName] == m_EditValueCacheList[persistWay][classifyName][itemName]));
                                                {
                                                    if (GUILayout.Button("保存" + (m_EditStateList[persistWay][classifyName][itemName] && m_EditValueList[persistWay][classifyName][itemName] != m_EditValueCacheList[persistWay][classifyName][itemName] ? "*" : string.Empty)))
                                                    {
                                                        m_EditValueList[persistWay][classifyName][itemName] = m_EditValueCacheList[persistWay][classifyName][itemName];
                                                        m_ValueList[persistWay][classifyName][itemName] = SwitchToTidyFormat(m_EditValueList[persistWay][classifyName][itemName]);

                                                        int index = m_FileFragmentNames.FindIndex((name) => { return name == classifyName; });
                                                        string fileName = classifyName;
                                                        string filePath = m_FilePaths[index];
                                                        Serialize(filePath, fileName);
                                                        GUIUtility.ExitGUI();
                                                    }
                                                }
                                                EditorGUI.EndDisabledGroup();

                                                if (GUILayout.Button("删除"))
                                                {
                                                    m_ValueList[persistWay][classifyName].Remove(itemName);
                                                    m_EditStateList[persistWay][classifyName].Remove(itemName);
                                                    m_EditValueList[persistWay].Remove(classifyName);
                                                    m_EditValueCacheList[persistWay].Remove(classifyName);
                                                    m_EditScrollViewPositionList[persistWay].Remove(classifyName);
                                                    if (m_ValueList[persistWay][classifyName].Keys.Count == 0)
                                                    {
                                                        m_ValueList[persistWay].Remove(classifyName);
                                                        m_EditStateList[persistWay].Remove(classifyName);
                                                        m_EditValueList[persistWay].Remove(classifyName);
                                                        m_EditValueCacheList[persistWay].Remove(classifyName);
                                                        m_EditScrollViewPositionList[persistWay].Remove(classifyName);
                                                        string fullPath = AorTxt.Format("{0}/{1}.dat", m_FileFragmentsRootDirectoryFullPath, classifyName);
                                                        if (System.IO.File.Exists(fullPath))
                                                        {
                                                            System.IO.File.Delete(fullPath);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        m_ValueList[persistWay][classifyName].Remove(itemName);
                                                        m_EditStateList[persistWay][classifyName].Remove(itemName);
                                                        m_EditValueList[persistWay][classifyName].Remove(itemName);
                                                        m_EditValueCacheList[persistWay][classifyName].Remove(itemName);
                                                        m_EditScrollViewPositionList[persistWay][classifyName].Remove(itemName);
                                                        int index = m_FileFragmentNames.FindIndex((name) => { return name == classifyName; });
                                                        string fileName = classifyName;
                                                        string filePath = m_FilePaths[index];
                                                        Serialize(filePath, fileName);
                                                    }
                                                    GUIUtility.ExitGUI();
                                                }
                                            }
                                            EditorGUILayout.EndHorizontal();

                                            if (m_EditStateList[persistWay][classifyName][itemName])
                                            {
                                                GUI.color = Color.cyan;
                                                m_EditScrollViewPositionList[persistWay][classifyName][itemName] = GUILayout.BeginScrollView(m_EditScrollViewPositionList[persistWay][classifyName][itemName], new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(80) });
                                                {
                                                    m_EditValueCacheList[persistWay][classifyName][itemName] = GUILayout.TextArea(m_EditValueCacheList[persistWay][classifyName][itemName], new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                                                }
                                                GUILayout.EndScrollView();
                                                GUI.color = Color.white;
                                            }

                                        }
                                    }
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// 运行时模式刷新GUI（只读）
        /// </summary>
        /// <param name="persistWay">持久化方式</param>
        private void RefreshGUIOnRuntimeMode(PersistWayType persistWay)
        {
            string persistWayName = string.Empty;
            if (persistWay == PersistWayType.PlayerPrefs)
            {
                persistWayName = "数据库持久化（Runtime）（只读）";
            }
            else if (persistWay == PersistWayType.FileFragment)
            {
                persistWayName = "文件片段持久化（Runtime）（只读）";
            }
            bool persistWayLastState = m_OpenedItems.Contains(persistWayName);
            bool persistWayCurrentState = EditorGUILayout.Foldout(persistWayLastState, persistWayName);
            if (persistWayCurrentState != persistWayLastState)
            {
                if (persistWayCurrentState)
                {
                    m_OpenedItems.Add(persistWayName);
                }
                else
                {
                    m_OpenedItems.Remove(persistWayName);
                }
            }

            if (persistWayCurrentState)
            {
                PersistComponent t = (PersistComponent)target;

                EditorGUILayout.BeginVertical("box");
                {
                    if (persistWay == PersistWayType.FileFragment)
                    {
                        if (GUILayout.Button("打开文件片段所在文件夹"))
                        {
                            string folder = AorTxt.Format("\"{0}\"", Runtime.GamePathUtils.FileFragment.GetRootDirectoryFullPath());
                            switch (Application.platform)
                            {
                                case RuntimePlatform.WindowsEditor:
                                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                                    break;

                                case RuntimePlatform.OSXEditor:
                                    Process.Start("open", folder);
                                    break;

                                default:
                                    throw new Exception(AorTxt.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                                }
                            GUIUtility.ExitGUI();
                        }
                        if (t.FileFragmentManager.ItemGroups.Keys.Count > 0)
                        {
                            foreach (var itr in t.FileFragmentManager.ItemGroups)
                            {
                                string classifyName = itr.Key;
                                bool classifyNameLastState = m_OpenedItems.Contains(classifyName);
                                bool classifyNameCurrentState = EditorGUILayout.Foldout(classifyNameLastState, AorTxt.Format("{0} ({1})", classifyName, itr.Value.Count));
                                if (classifyNameCurrentState != classifyNameLastState)
                                {
                                    if (classifyNameCurrentState)
                                    {
                                        m_OpenedItems.Add(classifyName);
                                    }
                                    else
                                    {
                                        m_OpenedItems.Remove(classifyName);
                                    }
                                }

                                if (classifyNameCurrentState)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    {
                                        List<string> itemNames = new List<string>(itr.Value.Items.Keys);
                                        for (int i = 0; i < itemNames.Count; i++)
                                        {
                                            string itemName = itemNames[i];
                                            string itemValue = SwitchToReadableFormat(itr.Value.Items[itemName]);

                                            GUILayout.Label(itemName);

                                            GUI.color = Color.cyan;
                                            GUILayout.BeginScrollView(Vector2.zero, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                                            {
                                                GUILayout.Label(itemValue, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                                            }
                                            GUILayout.EndScrollView();
                                            GUI.color = Color.white;
                                        }
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                            }
                        }
                        else
                        {
                            GUILayout.Label("列表为空 ...");
                        }
                    }
                    else if(persistWay == PersistWayType.PlayerPrefs)
                    {
                        if (t.PlayerPrefsManager.ItemNameGroups.Keys.Count > 0)
                        {
                            foreach (var itr in t.PlayerPrefsManager.ItemNameGroups)
                            {
                                string classifyName = itr.Key;
                                bool classifyNameLastState = m_OpenedItems.Contains(classifyName);
                                bool classifyNameCurrentState = EditorGUILayout.Foldout(classifyNameLastState, AorTxt.Format("{0} ({1})", classifyName, itr.Value.Count));

                                if (!m_RuntimeLastDecodedPbValue.ContainsKey(classifyName))
                                {
                                    m_RuntimeLastDecodedPbValue.Add(classifyName, new Dictionary<string, string>());
                                }

                                if (classifyNameCurrentState != classifyNameLastState)
                                {
                                    if (classifyNameCurrentState)
                                    {
                                        m_OpenedItems.Add(classifyName);
                                    }
                                    else
                                    {
                                        m_OpenedItems.Remove(classifyName);
                                    }
                                }

                                if (classifyNameCurrentState)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    {
                                        List<string> itemNames = itr.Value;
                                        for (int i = 0; i < itemNames.Count; i++)
                                        {
                                            string itemName = itemNames[i];
                                            string fieldName = itemName;
                                            string fieldType = m_RuntimeClassInterFieldTypes[classifyName][fieldName];
                                            if (!m_RuntimeLastDecodedPbValue[classifyName].ContainsKey(itemName))
                                            {
                                                m_RuntimeLastDecodedPbValue[classifyName].Add(itemName, string.Empty);
                                                RefreshPlayerPrefsItemOnRuntimeMode(classifyName, fieldName, fieldType, itemName);
                                            }

                                            EditorGUILayout.BeginHorizontal("box");
                                            {
                                                GUILayout.Label(itemName);
                                                GUILayout.FlexibleSpace();
                                                if (GUILayout.Button("刷新", new GUILayoutOption[] { GUILayout.Width(100)}))
                                                {
                                                    RefreshPlayerPrefsItemOnRuntimeMode(classifyName, fieldName, fieldType, itemName);
                                                    GUIUtility.ExitGUI();
                                                }
                                            }
                                            EditorGUILayout.EndHorizontal();

                                            GUI.color = Color.cyan;
                                            GUILayout.BeginScrollView(Vector2.zero, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                                            {
                                                GUILayout.Label(m_RuntimeLastDecodedPbValue[classifyName][itemName], new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                                            }
                                            GUILayout.EndScrollView();
                                            GUI.color = Color.white;
                                        }
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                            }
                        }
                        else
                        {
                            GUILayout.Label("列表为空 ...");
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
        #endregion

        //=========================================================================
        // 序列化/反序列化（文件存储）
        //=========================================================================
        #region 序列化与反序列化
        /// <summary>
        /// 序列化文件片段数据（AES+GZIP压缩）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileFragmentName">文件片段名</param>
        public void Serialize(string filePath, string fileFragmentName = null)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
            {
                JObject jObject = new JObject();
                foreach (var item in m_ValueList[PersistWayType.FileFragment][fileFragmentName])
                {
                    jObject.Add(item.Key, item.Value);
                }
                string encodedContent = AESEncrypt.EncodeToBase64(GZip.CompressToBase64(jObject.ToString()));
                byte[] bytes = Converter.GetBytesByString(encodedContent);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 反序列化文件片段数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileFragmentName">文件片段名</param>
        public void Deserialize(string filePath, string fileFragmentName = null)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
            {
                m_ValueList[PersistWayType.FileFragment].Add(fileFragmentName, new SortedDictionary<string, string>());
                m_EditStateList[PersistWayType.FileFragment].Add(fileFragmentName, new SortedDictionary<string, bool>());
                m_EditValueList[PersistWayType.FileFragment].Add(fileFragmentName, new SortedDictionary<string, string>());
                m_EditValueCacheList[PersistWayType.FileFragment].Add(fileFragmentName, new SortedDictionary<string, string>());
                m_EditScrollViewPositionList[PersistWayType.FileFragment].Add(fileFragmentName, new SortedDictionary<string, Vector2>());
                string content = reader.ReadToEnd();
                if(!string.IsNullOrEmpty(content))
                {
                    string decodedContent = AESEncrypt.DecodeFromBase64(content);
                    string uncompressedContent = GZip.UncompressFromBase64(decodedContent);
                    JObject jObject = JObject.Parse(uncompressedContent);
                    foreach (var itr in jObject)
                    {
                        m_ValueList[PersistWayType.FileFragment][fileFragmentName].Add(itr.Key, itr.Value.ToString());
                        m_EditStateList[PersistWayType.FileFragment][fileFragmentName].Add(itr.Key, false);
                        m_EditValueList[PersistWayType.FileFragment][fileFragmentName].Add(itr.Key, string.Empty);
                        m_EditValueCacheList[PersistWayType.FileFragment][fileFragmentName].Add(itr.Key, string.Empty);
                        m_EditScrollViewPositionList[PersistWayType.FileFragment][fileFragmentName].Add(itr.Key, Vector2.zero);
                    }
                }
            }
        }
        #endregion

        //=========================================================================
        // 运行时数据刷新
        //=========================================================================
        #region 运行时数据刷新
        /// <summary>
        /// 运行时刷新PlayerPrefs数据显示
        /// </summary>
        /// <param name="classifyName">分类名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="itemName">条目名</param>
        private void RefreshPlayerPrefsItemOnRuntimeMode(string classifyName, string fieldName, string fieldType, string itemName)
        {
            string key = AorTxt.Format("{0}_{1}", classifyName, itemName);
            string itemValue = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key));
            if (fieldType == "number" || fieldType == "string" || fieldType == "table" || fieldType == "boolean" || fieldType.EndsWith("[]"))
            {
                m_RuntimeLastDecodedPbValue[classifyName][itemName] = SwitchToReadableFormat(itemValue);
            }
        }
        #endregion

        //=========================================================================
        // Editor模式 - PlayerPrefs树形结构展示
        //=========================================================================
        #region PlayerPrefs树形展示
        /// <summary>
        /// 编辑器模式展示PlayerPrefs层级结构
        /// </summary>
        /// <param name="classifyName">分类名</param>
        private void ShowPlayerPrefsItemsOnEditorMode(string classifyName)
        {
            // 递归链表
            List<Node> list = new List<Node>();
            foreach(var itr in m_ValueList[PersistWayType.PlayerPrefs][classifyName])
            {
                string[] picesNames = itr.Key.Split('.');
                GenerateNodeElements(picesNames, 0, list);
            }

            // 递归展示
            foreach(var node in list)
            {
                ShowPlayerPrefsItemOnEditorMode(classifyName, node);
            }
        }

        /// <summary>
        /// 递归生成节点树结构
        /// </summary>
        /// <param name="picesNames">键名分段</param>
        /// <param name="curCheckPicesIndex">当前索引</param>
        /// <param name="list">节点列表</param>
        private void GenerateNodeElements(string[] picesNames, int curCheckPicesIndex, List<Node> list)
        {
            if(curCheckPicesIndex >= picesNames.Length)
            {
                return;
            }

            Node matchedNode = list.Find((node) =>
            {
                return node.Name == picesNames[curCheckPicesIndex];
            });
            if (matchedNode == null)
            {
                string key = string.Empty;
                for(int idx = 0; idx <= curCheckPicesIndex; idx++)
                {
                    key += ((string.IsNullOrEmpty(key) ? "" : ".") + picesNames[idx]);
                }
                matchedNode = new Node(picesNames[curCheckPicesIndex], key);
                list.Add(matchedNode);
            }
            GenerateNodeElements(picesNames, curCheckPicesIndex + 1, matchedNode.NextNodes);
        }

        /// <summary>
        /// 递归绘制PlayerPrefs节点
        /// </summary>
        /// <param name="classifyName">分类名</param>
        /// <param name="node">节点</param>
        private void ShowPlayerPrefsItemOnEditorMode(string classifyName, Node node)
        {
            if (node.IsLeaf)
            {
                PersistWayType persistWay = PersistWayType.PlayerPrefs;
                string itemName = node.Key;
                string itemValue = m_ValueList[persistWay][classifyName][itemName];

                EditorGUILayout.BeginHorizontal("box");
                {
                    EditorGUILayout.LabelField(node.Name);
                    EditorGUILayout.LabelField("编辑窗口", new GUILayoutOption[] { GUILayout.Width(50) });
                    bool state = EditorGUILayout.Toggle(m_EditStateList[persistWay][classifyName][itemName], new GUILayoutOption[] { GUILayout.Width(20) });
                    if (state != m_EditStateList[persistWay][classifyName][itemName])
                    {
                        m_EditStateList[persistWay][classifyName][itemName] = state;
                        m_EditValueList[persistWay][classifyName][itemName] = state ? SwitchToReadableFormat(itemValue) : string.Empty;
                        m_EditValueCacheList[persistWay][classifyName][itemName] = m_EditValueList[persistWay][classifyName][itemName];
                        m_EditScrollViewPositionList[persistWay][classifyName][itemName] = Vector2.zero;
                    }
                    EditorGUI.BeginDisabledGroup((!m_EditStateList[persistWay][classifyName][itemName]) || (m_EditValueList[persistWay][classifyName][itemName] == m_EditValueCacheList[persistWay][classifyName][itemName]));
                    {
                        if (GUILayout.Button("保存" + (m_EditStateList[persistWay][classifyName][itemName] && m_EditValueList[persistWay][classifyName][itemName] != m_EditValueCacheList[persistWay][classifyName][itemName] ? "*" : string.Empty)))
                        {
                            m_EditValueList[persistWay][classifyName][itemName] = m_EditValueCacheList[persistWay][classifyName][itemName];
                            m_ValueList[persistWay][classifyName][itemName] = SwitchToTidyFormat(m_EditValueList[persistWay][classifyName][itemName]);

                            string key = AorTxt.Format("{0}_{1}", classifyName, itemName);
                            GenerateLuaDeclares(out List<List<string>> luaFilesDeclaresLines, false);
                            Dictionary<string, Dictionary<string, string>> classInterFieldTypes = GetPbFileMainStructorDetailInfos(luaFilesDeclaresLines);
                            string[] itemArray = classInterFieldTypes[classifyName][itemName].Split('.');
                            string itemType = itemArray[itemArray.Length - 1];
                            if(itemType.EndsWith("[]"))
                            {
                                itemType = "table";
                            }
                            string result1 = string.Empty;
                            switch (itemType)
                            {
                                case "number": result1 = m_ValueList[persistWay][classifyName][itemName]; break;
                                case "string": result1 = m_ValueList[persistWay][classifyName][itemName]; break;
                                case "table": result1 = m_ValueList[persistWay][classifyName][itemName]; break;
                                case "boolean": result1 = m_ValueList[persistWay][classifyName][itemName]; break;
                            }
                            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(result1));
                            GUIUtility.ExitGUI();
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("删除"))
                    {
                        string key = AorTxt.Format("{0}_{1}", classifyName, itemName);
                        if (PlayerPrefs.HasKey(key))
                        {
                            PlayerPrefs.DeleteKey(key);
                        }
                        m_ValueList[persistWay][classifyName].Remove(itemName);
                        m_EditStateList[persistWay][classifyName].Remove(itemName);
                        m_EditValueList[persistWay][classifyName].Remove(itemName);
                        m_EditValueCacheList[persistWay][classifyName].Remove(itemName);
                        m_EditScrollViewPositionList[persistWay][classifyName].Remove(itemName);
                        if (m_ValueList[persistWay][classifyName].Keys.Count == 0)
                        {
                            m_ValueList[persistWay].Remove(classifyName);
                            m_EditStateList[persistWay].Remove(classifyName);
                            m_EditValueList[persistWay].Remove(classifyName);
                            m_EditValueCacheList[persistWay].Remove(classifyName);
                            m_EditScrollViewPositionList[persistWay].Remove(classifyName);
                            string classifyKey = AorTxt.Format("Classify_{0}_ItemNameList", classifyName);
                            if (PlayerPrefs.HasKey(classifyKey))
                            {
                                PlayerPrefs.DeleteKey(classifyKey);
                            }
                            string classifyNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameList", AESEncrypt.EncodeToBase64(string.Empty)));
                            if (!string.IsNullOrEmpty(classifyNameListText))
                            {
                                List<string> classifyNameList = new List<string>(classifyNameListText.Split(','));
                                classifyNameList.Remove(classifyName);

                                classifyNameListText = string.Empty;
                                foreach (string newClassifyName in classifyNameList)
                                {
                                    classifyNameListText = string.IsNullOrEmpty(classifyNameListText) ? newClassifyName : AorTxt.Format("{0},{1}", classifyNameListText, newClassifyName);
                                }
                                PlayerPrefs.SetString("ClassifyNameList", AESEncrypt.EncodeToBase64(classifyNameListText));
                            }
                        }
                        else
                        {
                            m_ValueList[persistWay][classifyName].Remove(itemName);
                            m_EditStateList[persistWay][classifyName].Remove(itemName);
                            m_EditValueList[persistWay][classifyName].Remove(itemName);
                            m_EditValueCacheList[persistWay][classifyName].Remove(itemName);
                            m_EditScrollViewPositionList[persistWay][classifyName].Remove(itemName);
                            string classifyKey = AorTxt.Format("Classify_{0}_ItemNameList", classifyName);
                            string itemNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(classifyKey, AESEncrypt.EncodeToBase64(string.Empty)));
                            if (!string.IsNullOrEmpty(itemNameListText))
                            {
                                List<string> nameList = new List<string>(itemNameListText.Split(','));
                                nameList.Remove(itemName);

                                itemNameListText = string.Empty;
                                foreach (string newName in nameList)
                                {
                                    itemNameListText = string.IsNullOrEmpty(itemNameListText) ? newName : AorTxt.Format("{0},{1}", itemNameListText, newName);
                                }
                                PlayerPrefs.SetString(classifyKey, AESEncrypt.EncodeToBase64(itemNameListText));
                            }
                        }
                        PlayerPrefs.Save();
                        GUIUtility.ExitGUI();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (m_EditStateList[persistWay][classifyName][itemName])
                {
                    GUI.color = Color.cyan;
                    m_EditScrollViewPositionList[persistWay][classifyName][itemName] = GUILayout.BeginScrollView(m_EditScrollViewPositionList[persistWay][classifyName][itemName], new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(200) });
                    {
                        m_EditValueCacheList[persistWay][classifyName][itemName] = GUILayout.TextArea(m_EditValueCacheList[persistWay][classifyName][itemName], new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    }
                    GUILayout.EndScrollView();
                    GUI.color = Color.white;
                }
            }
            else
            {
                bool nameLastState = m_OpenedItems.Contains(node.Desc);
                bool nameCurrentState = EditorGUILayout.Foldout(nameLastState, node.Desc);
                if (nameCurrentState != nameLastState)
                {
                    if (nameCurrentState)
                    {
                        m_OpenedItems.Add(node.Desc);
                    }
                    else
                    {
                        m_OpenedItems.Remove(node.Desc);
                    }
                }

                if (nameCurrentState)
                {
                    EditorGUILayout.BeginVertical("box");
                    {
                        foreach(var subNode in node.NextNodes)
                        {
                            ShowPlayerPrefsItemOnEditorMode(classifyName, subNode);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
        #endregion

        //=========================================================================
        // 内部树形节点结构
        //=========================================================================
        #region 树形节点结构
        /// <summary>
        /// 树形节点结构类
        /// </summary>
        private class Node
        {
            public Node(string name, string key)
            {
                Name = name;
                Key = key;
                NextNodes = new List<Node>();
            }

            /// <summary>
            /// 节点名称
            /// </summary>
            public string Name;
            
            /// <summary>
            /// 节点完整键
            /// </summary>
            public string Key;
            
            /// <summary>
            /// 子节点列表
            /// </summary>
            public List<Node> NextNodes;
            
            /// <summary>
            /// 节点描述
            /// </summary>
            public string Desc => Name + "(" + NextNodes.Count + ")";
            
            /// <summary>
            /// 是否为叶子节点
            /// </summary>
            public bool IsLeaf => NextNodes.Count == 0;
        }
        #endregion

        //=========================================================================
        // 数据格式化工具（可读/压缩格式）
        //=========================================================================
        #region 数据格式化工具
        /// <summary>
        /// 转换为带缩进的可读格式
        /// </summary>
        /// <param name="gameData">原始字符串</param>
        /// <returns>格式化字符串</returns>
        private static string SwitchToReadableFormat(string gameData)
        {
            int tabCount = 0;
            string luaStr = "";
            for (int i = 0; i < gameData.Length; i++)
            {
                if (gameData[i] == '{')
                {
                    if (IsNeedLineFeedByLeftBracket(gameData, i))
                    {
                        tabCount++;
                        luaStr += gameData[i] + "\n" + GetTabStringByTabCount(tabCount);
                        if (i < gameData.Length - 1 && gameData[i + 1] == ' ')
                        {
                            i++;
                        }
                    }
                    else
                    {
                        luaStr += gameData[i];
                    }
                }
                else if (gameData[i] == '}')
                {
                    if (IsNeedLineFeedByRightBracket(gameData, i - 1))
                    {
                        tabCount--;
                        luaStr += "\n" + GetTabStringByTabCount(tabCount) + gameData[i];
                        if (i < gameData.Length - 1 && gameData[i + 1] == ' ')
                        {
                            i++;
                        }
                    }
                    else
                    {
                        luaStr += gameData[i];
                    }
                    if (i < gameData.Length - 1 && gameData[i + 1] == ',')
                    {
                        luaStr += gameData[i + 1] + "\n" + GetTabStringByTabCount(tabCount);
                        i++;
                        if (i < gameData.Length - 1 && gameData[i + 1] == ' ')
                        {
                            i++;
                        }
                    }
                }
                else if (gameData[i] == ',')
                {
                    if (IsNeedLineFeedByComma(gameData, i))
                    {
                        luaStr += gameData[i] + "\n" + GetTabStringByTabCount(tabCount);
                        if (i < gameData.Length - 1 && gameData[i + 1] == ' ')
                        {
                            i++;
                        }
                    }
                    else
                    {
                        luaStr += gameData[i];
                    }
                }
                else
                {
                    luaStr += gameData[i];
                }
            }
            return luaStr;
        }

        /// <summary>
        /// 转换为压缩无格式字符串
        /// </summary>
        /// <param name="gameData">原始字符串</param>
        /// <returns>精简格式字符串</returns>
        private static string SwitchToTidyFormat(string gameData)
        {
            string result = gameData.Replace("\n", string.Empty).Replace("\t", string.Empty);
            return result;
        }

        /// <summary>
        /// 判断左大括号是否需要换行
        /// </summary>
        private static bool IsNeedLineFeedByLeftBracket(string gameData, int index)
        {
            if (index == gameData.Length)
            {
                return false;
            }
            for (int i = index + 1; i < gameData.Length; i++)
            {
                if (gameData[i] == '}')
                {
                    return false;
                }
                else if ((gameData[i] == '=') || (gameData[i] == ':') || (gameData[i] == '{'))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断右大括号是否需要换行
        /// </summary>
        private static bool IsNeedLineFeedByRightBracket(string gameData, int index)
        {
            if (index == 0)
            {
                return false;
            }
            for (int i = index; i >= 0; i--)
            {
                if (gameData[i] == '=' || gameData[i] == ':' || gameData[i] == '}')
                {
                    return true;
                }
                else if (gameData[i] == '{')
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断逗号是否需要换行
        /// </summary>
        private static bool IsNeedLineFeedByComma(string gameData, int index)
        {
            if (index == 0)
            {
                return false;
            }
            for (int i = index - 1; i >= 0; i--)
            {
                if (gameData[i] == '=' || gameData[i] == ':')
                {
                    return true;
                }
                else if (gameData[i] == '{')
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定数量的Tab缩进字符串
        /// </summary>
        private static string GetTabStringByTabCount(int tabCount)
        {
            string tabStr = "";
            if (tabCount >= 0)
            {
                for (int i = 0; i < tabCount; i++)
                {
                    tabStr += "\t";
                }
            }
            else
            {
                Log.Error("[Editor] 表格数量不能小于0。");
            }

            return tabStr;
        }
        #endregion
    }
}