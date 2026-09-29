/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   本地化管理器 - 核心逻辑层
 ***************************************************************/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 本地化管理器（核心逻辑层）
    /// 负责多语言文本加载、语言配置管理、字体配置管理、JSON解析
    /// </summary>
    public sealed partial class LocalizationManager
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 从组件组获取必需组件，缺失时输出致命错误
        /// </summary>
        /// <typeparam name="T">游戏组件类型（必须为 GameComponent 子类）</typeparam>
        /// <returns>解析到的组件；缺失为 null</returns>
        private T RequireComponent<T>() where T : GameComponent
        {
            T component = GameComponentsGroup.GetComponent<T>();
            if (component == null)
            {
                Log.Fatal($"{typeof(T).Name} 无效。");
            }
            return component;
        }

        /// <summary>
        /// 校验语言入参合法（非 Unspecified）
        /// </summary>
        private static void ValidateLanguage(GameDefinitions.Language language)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("language 无效。");
            }
        }

        /// <summary>
        /// 校验本地化 Key 非空
        /// </summary>
        private static void ValidateKeyName(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new GameException("keyName 无效。");
            }
        }

        /// <summary>
        /// 获取或初始化某个键下的集合（键不存在时自动 new 并挂入映射）
        /// </summary>
        private static TCol GetOrAddCollection<TKey, TCol>(Dictionary<TKey, TCol> map, TKey key) where TCol : new()
        {
            if (!map.TryGetValue(key, out TCol collection))
            {
                collection = new TCol();
                map.Add(key, collection);
            }
            return collection;
        }

        /// <summary>
        /// 构造函数：初始化组件与数据容器
        /// </summary>
        public LocalizationManager()
        {
            m_LauncherComponent = RequireComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                return;
            }

            m_AssetComponent = RequireComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                return;
            }

            m_DefaultLanguages = new List<GameDefinitions.Language>();
            m_DefaultDatas = new Dictionary<GameDefinitions.Language, Dictionary<string, string>>();
            m_FontDatas = new Dictionary<GameDefinitions.Language, List<LocalizationFontData>>();
        }
        #endregion

        //=========================================================================
        // 语言列表管理
        //=========================================================================
        #region Language Management
        /// <summary>
        /// 从 AB 包同步加载 Json 文本资源，读取文本后立即卸载
        /// </summary>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <returns>Json 文本内容</returns>
        private string ReadJsonText(string abPath, string assetName)
        {
            TextAsset jsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", abPath, assetName);
            string jsonText = jsonAsset.text;
            m_AssetComponent.UnloadAsset(jsonAsset, null, true);
            return jsonText;
        }

        /// <summary>
        /// 从 JSON 加载支持的语言列表
        /// </summary>
        public void LoadDefaultLanguages()
        {
            JArray languageArray = JArray.Parse(ReadJsonText(GamePathUtils.Json.GetRootDirectoryRelativePath(), "LocalizationDefaultLanguages"));

            foreach (var entry in languageArray)
            {
                m_DefaultLanguages.Add((GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), entry.ToString()));
            }
        }

        /// <summary>
        /// 检查是否支持指定语言
        /// </summary>
        /// <param name="language">目标语言</param>
        public bool HasDefaultLanguage(GameDefinitions.Language language)
        {
            return m_DefaultLanguages.Contains(language);
        }

        /// <summary>
        /// 清空支持的语言列表
        /// </summary>
        public void RemoveAllDefaultLanguages()
        {
            m_DefaultLanguages.Clear();
        }
        #endregion

        //=========================================================================
        // 本地化文本管理
        //=========================================================================
        #region Localization Text Management
        /// <summary>
        /// 加载指定语言的本地化文本数据
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        public void LoadDefaultDatas(GameDefinitions.Language language, string abPath, string assetName)
        {
            JObject dataRoot = JObject.Parse(ReadJsonText(abPath, assetName));

            foreach (var entry in dataRoot)
            {
                AddDefaultData(language, entry.Key, entry.Value.ToString());
            }
        }

        /// <summary>
        /// 检查是否存在指定本地化 Key
        /// </summary>
        public bool HasDefaultData(GameDefinitions.Language language, string keyName)
        {
            return GetDefaultData(language, keyName) != null;
        }

        /// <summary>
        /// 添加一条本地化文本
        /// </summary>
        public void AddDefaultData(GameDefinitions.Language language, string keyName, string content)
        {
            ValidateLanguage(language);
            ValidateKeyName(keyName);

            Dictionary<string, string> languageContents = GetOrAddCollection(m_DefaultDatas, language);

            if (!languageContents.ContainsKey(keyName))
            {
                languageContents.Add(keyName, content);
            }
        }

        /// <summary>
        /// 移除一条本地化文本
        /// </summary>
        public bool RemoveDefaultData(GameDefinitions.Language language, string keyName)
        {
            if (!HasDefaultData(language, keyName))
            {
                return false;
            }

            return m_DefaultDatas[language].Remove(keyName);
        }

        /// <summary>
        /// 清空所有语言的本地化数据
        /// </summary>
        public void RemoveAllDefaultDatas()
        {
            m_DefaultDatas.Clear();
        }

        /// <summary>
        /// 清空指定语言的本地化数据
        /// </summary>
        public void RemoveAllDefaultDatas(GameDefinitions.Language language)
        {
            if (m_DefaultDatas.ContainsKey(language))
            {
                m_DefaultDatas[language].Clear();
            }
        }

        /// <summary>
        /// 获取本地化文本（核心接口）
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="keyName">文本 Key</param>
        /// <returns>本地化文本内容</returns>
        public string GetDefaultData(GameDefinitions.Language language, string keyName)
        {
            ValidateLanguage(language);
            ValidateKeyName(keyName);

            string content = null;
            m_DefaultDatas.TryGetValue(language, out Dictionary<string, string> languageContents);
            languageContents?.TryGetValue(keyName, out content);

            return content;
        }
        #endregion

        //=========================================================================
        // 字体管理
        //=========================================================================
        #region Font Management
        /// <summary>
        /// 从 JSON 加载多语言字体配置
        /// </summary>
        public void LoadFontDatas(string abPath, string assetName)
        {
            JObject fontRoot = JObject.Parse(ReadJsonText(abPath, assetName));

            foreach (var entry in fontRoot)
            {
                GameDefinitions.Language language = (GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), entry.Key);
                string fontType = entry.Value["FontType"].ToString();
                JToken marks = entry.Value;
                int markIndex = 0;

                while (marks[$"Mark{markIndex}"] != null && !string.IsNullOrEmpty(marks[$"Mark{markIndex}"].ToString()))
                {
                    string markText = marks[$"Mark{markIndex}"].ToString();
                    string markAbPath = marks[$"ABPath{markIndex}"].ToString();
                    string markAssetName = marks[$"AssetName{markIndex}"].ToString();
                    string customMaterialName = marks[$"CustomMaterialName{markIndex}"].ToString();
                    float fontSizeScaleRatio = float.Parse(marks[$"FontSizeScaleRatio{markIndex}"].ToString());

                    LocalizationFontData fontData = new LocalizationFontData(fontType, markText, markAbPath, markAssetName, customMaterialName, fontSizeScaleRatio);
                    AddFontData(language, fontData);
                    markIndex++;
                }
            }
        }

        /// <summary>
        /// 添加语言对应的字体数据（自动去重）
        /// </summary>
        public void AddFontData(GameDefinitions.Language language, LocalizationFontData fontData)
        {
            GetFontDatas(language, out List<LocalizationFontData> existingList);

            if (existingList != null)
            {
                foreach (var data in existingList)
                {
                    if (data.Equals(fontData))
                        return;
                }
            }

            GetOrAddCollection(m_FontDatas, language).Add(fontData);
        }

        /// <summary>
        /// 清空所有字体配置
        /// </summary>
        public void RemoveAllFontDatas()
        {
            m_FontDatas.Clear();
        }

        /// <summary>
        /// 获取指定语言的字体列表
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="fontDatas">输出字体列表</param>
        public void GetFontDatas(GameDefinitions.Language language, out List<LocalizationFontData> fontDatas)
        {
            ValidateLanguage(language);

            m_FontDatas.TryGetValue(language, out fontDatas);
        }
        #endregion
    }
}