using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LocalizationManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LocalizationManager()
        {
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                Log.Fatal("Launcher Component 无效。");
                return;
            }

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            m_DefaultLanguages = new List<GameDefinitions.Language>();
            m_DefaultDatas = new Dictionary<GameDefinitions.Language, Dictionary<string, string>>();
            m_FontDatas = new Dictionary<GameDefinitions.Language, List<LocalizationFontData>>();
        }

        /// <summary>
        /// 加载默认支持语种类型集合
        /// </summary>
        public void LoadDefaultLanguages()
        {
            TextAsset languagesJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", GamePathUtils.Json.GetRootDirectoryRelativePath(), "LocalizationDefaultLanguages");
            JArray jArray = JArray.Parse(languagesJsonAsset.text);
            foreach (var name in jArray)
            {
                m_DefaultLanguages.Add((GameDefinitions.Language)System.Enum.Parse(typeof(GameDefinitions.Language), name.ToString()));
            }
            m_AssetComponent.UnloadAsset(languagesJsonAsset, null, true);
        }

        /// <summary>
        /// 检查是否支持指定的默认语种类型
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <returns>是否支持指定的默认语种类型</returns>
        public bool HasDefaultLanguage(GameDefinitions.Language language)
        {
            return m_DefaultLanguages.Contains(language);
        }

        /// <summary>
        /// 移除所有默认支持语种类型集合
        /// </summary>
        public void RemoveAllDefaultLanguages()
        {
            m_DefaultLanguages.Clear();
        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset资源名称</param>
        public void LoadDefaultDatas(GameDefinitions.Language language, string abPath, string assetName)
        {
            TextAsset configJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", abPath, assetName);
            JObject jObject = JObject.Parse(configJsonAsset.text);

            foreach (var item in jObject)
            {
                AddDefaultData(language, item.Key, item.Value.ToString());
            }

            m_AssetComponent.UnloadAsset(configJsonAsset, null, true);
        }


        /// <summary>
        /// 检查是否存在指定的默认数据
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="keyName">字段名称</param>
        /// <returns>是否存在指定的默认数据</returns>
        public bool HasDefaultData(GameDefinitions.Language language, string keyName)
        {
            return GetDefaultData(language, keyName) != null;
        }

        /// <summary>
        /// 添加指定的默认数据
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="keyName">字段名称</param>
        /// <param name="content">数据内容</param>
        public void AddDefaultData(GameDefinitions.Language language, string keyName, string content)
        {
            string data = GetDefaultData(language, keyName);
            if (data == null)
            {
                if (!m_DefaultDatas.ContainsKey(language))
                {
                    m_DefaultDatas.Add(language, new Dictionary<string, string>());
                }
                m_DefaultDatas[language].Add(keyName, content);
            }
        }

        /// <summary>
        /// 移除指定的默认数据
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="keyName">字段名称</param>
        /// <returns>是否成功移除指定的默认数据</returns>
        public bool RemoveDefaultData(GameDefinitions.Language language, string keyName)
        {
            if (!HasDefaultData(language, keyName))
            {
                return false;
            }

            return m_DefaultDatas[language].Remove(keyName);
        }

        /// <summary>
        /// 移除所有默认数据
        /// </summary>
        public void RemoveAllDefaultDatas()
        {
            m_DefaultDatas.Clear();
        }

        /// <summary>
        /// 移除指定语种的所有默认数据
        /// </summary>
        /// <param name="language">语种类型</param>
        public void RemoveAllDefaultDatas(GameDefinitions.Language language)
        {
            if (m_DefaultDatas.ContainsKey(language))
            {
                m_DefaultDatas[language].Clear();
            }
        }

        /// <summary>
        /// 获取指定语种的字段内容（默认）
        /// 数据来源于json的多语言配置文件
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="keyName">字段名称</param>
        /// <returns></returns>
        public string GetDefaultData(GameDefinitions.Language language, string keyName)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("language 无效。");
            }

            if (string.IsNullOrEmpty(keyName))
            {
                throw new GameException("keyName 无效。");
            }

            string content = null;

            Dictionary<string, string> languageContents = null;
            m_DefaultDatas.TryGetValue(language, out languageContents);
            if (languageContents != null)
            {
                languageContents.TryGetValue(keyName, out content);
            }

            return content;
        }

        /// <summary>
        /// 加载字体配置数据
        /// </summary>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset资源名称</param>
        public void LoadFontDatas(string abPath, string assetName)
        {
            TextAsset configJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", abPath, assetName);
            JObject jObject = JObject.Parse(configJsonAsset.text);
            foreach (var item in jObject)
            {
                GameDefinitions.Language language = (GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), item.Key);
                string fontType = item.Value["FontType"].ToString();
                int index = 0;
                while (item.Value[$"Mark{index}"] != null && !string.IsNullOrEmpty(item.Value[$"Mark{index}"].ToString()))
                {
                    string itemMark = item.Value[$"Mark{index}"].ToString();
                    string itemABPath = item.Value[$"ABPath{index}"].ToString();
                    string itemAssetName = item.Value[$"AssetName{index}"].ToString();
                    string itemCustomMaterialName = item.Value[$"CustomMaterialName{index}"].ToString();
                    float itemFontSizeScaleRatio = float.Parse(item.Value[$"FontSizeScaleRatio{index}"].ToString());
                    LocalizationFontData fontData = new LocalizationFontData(fontType, itemMark, itemABPath, itemAssetName, itemCustomMaterialName, itemFontSizeScaleRatio);
                    AddFontData(language, fontData);
                    index++;
                }
            }

            m_AssetComponent.UnloadAsset(configJsonAsset, null, true);
        }

        /// <summary>
        /// 添加指定的字体配置数据
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="fontData">字体数据</param
        public void AddFontData(GameDefinitions.Language language, LocalizationFontData fontData)
        {
            GetFontDatas(language, out List<LocalizationFontData> tmpfontDatas);
            if(tmpfontDatas != null)
            {
                foreach (var data in tmpfontDatas)
                {
                    if (data.Equals(fontData))
                    {
                        return;
                    }
                }
            }
            if (!m_FontDatas.ContainsKey(language))
            {
                m_FontDatas.Add(language, new List<LocalizationFontData>());
            }
            m_FontDatas[language].Add(fontData);
        }

        /// <summary>
        /// 移除所有字体配置数据
        /// </summary>
        public void RemoveAllFontDatas()
        {
            m_FontDatas.Clear();
        }

        /// <summary>
        /// 获取指定语种的字体配置数据
        /// 数据来源于json的多语言配置文件
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="fontDatas">字体数据集合</param>
        /// <returns></returns>
        public void GetFontDatas(GameDefinitions.Language language, out List<LocalizationFontData> fontDatas)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("language 无效。");
            }
            m_FontDatas.TryGetValue(language, out fontDatas);
        }

    }

}


