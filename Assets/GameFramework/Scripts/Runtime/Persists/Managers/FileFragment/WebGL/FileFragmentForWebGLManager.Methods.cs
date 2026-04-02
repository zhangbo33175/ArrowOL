using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class FileFragmentForWebGLManager
    {
        /// <summary>
        /// 从存档中读取所有条目
        /// </summary>
        private void LoadItemNameGroups()
        {
            m_ItemNameGroups.Clear();
            string classifyNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameListForWebGL", AESEncrypt.EncodeToBase64(string.Empty)));
            if (!string.IsNullOrEmpty(classifyNameListText))
            {
                string[] classifyNameList = classifyNameListText.Split(',');
                for (int classifyNameIndex = 0; classifyNameIndex < classifyNameList.Length; classifyNameIndex++)
                {
                    string classifyName = classifyNameList[classifyNameIndex];
                    m_ItemNameGroups.Add(classifyName, new List<string>());

                    string classifyXXXXXItemNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameListForWebGL", AESEncrypt.EncodeToBase64(string.Empty)));
                    if (!string.IsNullOrEmpty(classifyXXXXXItemNameListText))
                    {
                        string[] nameList = classifyXXXXXItemNameListText.Split(',');
                        m_ItemNameGroups[classifyName].AddRange(nameList);
                    }
                }
            }

        }

        /// <summary>
        /// 刷新分类名称列表到存档
        /// </summary>
        private void RefreshClassifyNameListToSave()
        {
            string classifyNameList = string.Empty;
            foreach (string classifyName in m_ItemNameGroups.Keys)
            {
                classifyNameList = string.IsNullOrEmpty(classifyNameList) ? classifyName : $"{classifyNameList},{classifyName}";
            }
            PlayerPrefs.SetString("ClassifyNameListForWebGL", AESEncrypt.EncodeToBase64(classifyNameList));
        }

        /// <summary>
        /// 刷新条目指定分类名称下的所有数据到存档
        /// </summary>
        /// <param name="classifyNameForSetting">分类名称</param>
        private void RefreshItemNameListToSave(string classifyNameForSetting)
        {
            if (!string.IsNullOrEmpty(classifyNameForSetting))
            {
                List<string> names = null;
                string key = $"Classify_{classifyNameForSetting}_ItemNameListForWebGL";
                if (m_ItemNameGroups.TryGetValue(classifyNameForSetting, out names))
                {
                    string nameList = string.Empty;
                    foreach (string name in names)
                    {
                        nameList = string.IsNullOrEmpty(nameList) ? name : $"{nameList},{name}";
                    }
                    PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(nameList));
                }
                else
                {
                    if (PlayerPrefs.HasKey(key))
                    {
                        PlayerPrefs.DeleteKey(key);
                    }
                }
            }
        }
    }
}


