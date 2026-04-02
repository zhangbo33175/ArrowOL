using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class FileFragmentForWebGLManager
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FileFragmentForWebGLManager()
        {
            m_ItemNameGroups = new SortedDictionary<string, List<string>>();
        }

        /// <summary>
        /// 加载条目信息。
        /// </summary>
        /// <returns>是否加载条目信息成功。</returns>
        public bool Load()
        {
            // 从存档中读取所有条目
            LoadItemNameGroups();

            return true;
        }

        /// <summary>
        /// 保存。
        /// </summary>
        /// <returns>是否保存成功。</returns>
        public bool Save()
        {
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// 获取指定分类名称的所有条目名称集合。
        /// </summary>
        /// <returns>条目名称集合。</returns>
        public string[] GetAllItemNames(string classifyName)
        {
            List<string> range = null;
            if (m_ItemNameGroups.TryGetValue(classifyName, out range))
            {
                return range.ToArray();
            }
            return null;
        }

        /// <summary>
        /// 获取指定分类名称的所有条目名称集合。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="results">条目名称集合。</param>
        public void GetAllItemNames(string classifyName, List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results 无效。");
            }

            results.Clear();

            List<string> range = null;
            if (m_ItemNameGroups.TryGetValue(classifyName, out range))
            {
                results = range;
            }

            return;
        }

        /// <summary>
        /// 检查是否存在指定条目。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">要检查条目的名称。</param>
        /// <returns>指定的条目是否存在。</returns>
        public bool HasItem(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// 移除指定条目。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">要移除条目的名称。</param>
        /// <returns>是否移除指定条目成功。</returns>
        public bool RemoveItem(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            if (!PlayerPrefs.HasKey(key))
            {
                return false;
            }

            PlayerPrefs.DeleteKey(key);

            if (m_ItemNameGroups.ContainsKey(classifyName))
            {
                if (m_ItemNameGroups[classifyName].Contains(itemName))
                {
                    m_ItemNameGroups[classifyName].Remove(itemName);
                    if (m_ItemNameGroups[classifyName].Count == 0)
                    {
                        m_ItemNameGroups.Remove(classifyName);
                    }
                }
            }

            RefreshItemNameListToSave(classifyName);
            RefreshClassifyNameListToSave();

            return true;
        }

        /// <summary>
        /// 清空所有条目。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        public void RemoveAllItems(string classifyName)
        {
            if (string.IsNullOrEmpty(classifyName))
            {
                List<string> classifyNames = new List<string>(m_ItemNameGroups.Keys);
                for(int index = 0; index < classifyNames.Count; index++)
                {
                    classifyName = classifyNames[index];
                    List<string> range = null;
                    if (m_ItemNameGroups.TryGetValue(classifyName, out range))
                    {
                        List<string> names = new List<string>();
                        foreach (var name in range)
                        {
                            string key = $"{classifyName}_{name}";
                            PlayerPrefs.DeleteKey(key);
                            names.Add(name);
                        }
                        names.ForEach((name) => { m_ItemNameGroups[classifyName].Remove(name); });

                        if (m_ItemNameGroups[classifyName].Count == 0)
                        {
                            m_ItemNameGroups.Remove(classifyName);
                        }

                    }
                }

                m_ItemNameGroups.Clear();
                RefreshItemNameListToSave(null);
                RefreshClassifyNameListToSave();
            }
            else
            {
                List<string> range = null;
                if (m_ItemNameGroups.TryGetValue(classifyName, out range))
                {
                    List<string> names = new List<string>();
                    foreach (var name in range)
                    {
                        string key = $"{classifyName}_{name}";
                        PlayerPrefs.DeleteKey(key);
                        names.Add(name);
                    }
                    names.ForEach((name) => { m_ItemNameGroups[classifyName].Remove(name); });

                    if (m_ItemNameGroups[classifyName].Count == 0)
                    {
                        m_ItemNameGroups.Remove(classifyName);
                    }

                    RefreshItemNameListToSave(classifyName);
                    RefreshClassifyNameListToSave();
                }
            }
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return bool.Parse(GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key))));
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string classifyName, string itemName, bool defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return bool.Parse(GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(defaultValue.ToString()))))));
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string classifyName, string itemName, bool value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(value.ToString())));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }
            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }

        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return int.Parse(GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key))));
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string classifyName, string itemName, int defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return int.Parse(GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(defaultValue.ToString()))))));
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string classifyName, string itemName, int value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(value.ToString())));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }
            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }

        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return float.Parse(GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key))));
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string classifyName, string itemName, float defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return float.Parse(GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(defaultValue.ToString()))))));
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string classifyName, string itemName, float value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(value.ToString())));

            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }
            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }

        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string classifyName, string itemName)
        {
            string key = $"{classifyName}_{itemName}";
            return GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key)));
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string classifyName, string itemName, string defaultValue)
        {
            string key = $"{classifyName}_{itemName}";
            return GZip.UncompressFromBase64(AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(defaultValue)))));
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string classifyName, string itemName, string value)
        {
            string key = $"{classifyName}_{itemName}";
            PlayerPrefs.SetString(key, AESEncrypt.EncodeToBase64(GZip.CompressToBase64(value)));
            
            bool modified = false;
            if (!m_ItemNameGroups.ContainsKey(classifyName))
            {
                m_ItemNameGroups.Add(classifyName, new List<string>());
                modified = true;
            }
            if (!m_ItemNameGroups[classifyName].Contains(itemName))
            {
                m_ItemNameGroups[classifyName].Add(itemName);
                modified = true;
            }

            if (modified)
            {
                RefreshItemNameListToSave(classifyName);
                RefreshClassifyNameListToSave();
            }

        }

        /// <summary>
        /// 打印所有名称列表
        /// </summary>
        public void PrintAllNameLists()
        {
            string classifyAllNameList = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString("ClassifyNameListForWebGL"));
            Log.Info($"ClassifyNameListFoWebGL = {classifyAllNameList}");
            if (!string.IsNullOrEmpty(classifyAllNameList))
            {
                string[] classifyNameList = classifyAllNameList.Split(',');
                for (int classifyNameIndex = 0; classifyNameIndex < classifyNameList.Length; classifyNameIndex++)
                {
                    string classifyName = classifyNameList[classifyNameIndex];
                    string classifyXXXXXNameListText = AESEncrypt.DecodeFromBase64(PlayerPrefs.GetString($"Classify_{classifyName}_ItemNameListForWebGL", AESEncrypt.EncodeToBase64(string.Empty)));
                    Log.Info($"Classify_{classifyName}_ItemNameListForWebGL = {classifyXXXXXNameListText}");
                }
            }
        }

    }
}


