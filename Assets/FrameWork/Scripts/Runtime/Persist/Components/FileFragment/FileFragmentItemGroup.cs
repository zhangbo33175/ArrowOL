using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Honor.Runtime
{
public sealed class FileFragmentItemGroup
    {
        /// <summary>
        /// 所有item的数据集合
        /// <条目名称,条目数据>
        /// </summary>
        private readonly SortedDictionary<string, string> m_Items = new SortedDictionary<string, string>();
        public SortedDictionary<string, string> Items
        {
            get
            {
                return m_Items;
            }
        }

        /// <summary>
        /// 初始化条目集合新实例。
        /// </summary>
        public FileFragmentItemGroup()
        {

        }

        /// <summary>
        /// 获取条目数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Items.Count;
            }
        }

        /// <summary>
        /// 获取所有条目的名称。
        /// </summary>
        /// <returns>所有条目的名称。</returns>
        public string[] GetAllItemNames()
        {
            string[] allItemNames = new string[m_Items.Count];

            int index = 0;
            foreach (KeyValuePair<string, string> item in m_Items)
            {
                allItemNames[index++] = item.Key;
            }

            return allItemNames;
        }

        /// <summary>
        /// 获取所有条目的名称。
        /// </summary>
        /// <param name="results">所有条目的名称。</param>
        public void GetAllItemNames(List<string> results)
        {
            if (results == null)
            {
                throw new Exception("Results 无效。");
            }
            results.Clear();
            results.AddRange(m_Items.Keys);
        }

        /// <summary>
        /// 检查是否存在指定条目。
        /// </summary>
        /// <param name="itemName">要检查条目的名称。</param>
        /// <returns>指定的条目是否存在。</returns>
        public bool HasItem(string itemName)
        {
            return m_Items.ContainsKey(itemName);
        }

        /// <summary>
        /// 移除指定条目。
        /// </summary>
        /// <param name="itemName">要移除条目的名称。</param>
        /// <returns>是否移除指定条目成功。</returns>
        public bool RemoveItem(string itemName)
        {
            return m_Items.Remove(itemName);
        }

        /// <summary>
        /// 清空所有条目。
        /// </summary>
        public void RemoveAllItems()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string itemName)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                Log.Warning("条目 '{0}' 不存在。", itemName);
                return false;
            }

            return int.Parse(value) != 0;
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string itemName, bool defaultValue)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                return defaultValue;
            }

            return int.Parse(value) != 0;
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string itemName, bool value)
        {
            m_Items[itemName] = value ? "1" : "0";
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string itemName)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                Log.Warning("条目 '{0}' 不存在。", itemName);
                return 0;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string itemName, int defaultValue)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                return defaultValue;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string itemName, int value)
        {
            m_Items[itemName] = value.ToString();
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string itemName)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                Log.Warning("条目 '{0}' 不存在。", itemName);
                return 0f;
            }

            return float.Parse(value);
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string itemName, float defaultValue)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                return defaultValue;
            }

            return float.Parse(value);
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string itemName, float value)
        {
            m_Items[itemName] = value.ToString();
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string itemName)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                Log.Warning("条目 '{0}' 不存在。", itemName);
                return null;
            }

            return value;
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string itemName, string defaultValue)
        {
            string value = null;
            if (!m_Items.TryGetValue(itemName, out value))
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string itemName, string value)
        {
            m_Items[itemName] = value;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="fs">文件流写入器</param>
        public bool Serialize(FileStream fs)
        {
            JObject jObject = new JObject();
            foreach (var item in m_Items)
            {
                jObject[item.Key] = item.Value;
            }
            string encodedContent = AESEncrypt.EncodeToBase64(GameGZip.CompressToBase64(jObject.ToString()));
            byte[] bytes = Converter.GetBytesByString(encodedContent);
            fs.Write(bytes, 0, bytes.Length);
            return true;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader">流读取器</param>
        public void Deserialize(StreamReader reader)
        {
            m_Items.Clear();
            string content = reader.ReadToEnd();
            if (!string.IsNullOrEmpty(content))
            {
                string decodedContent = AESEncrypt.DecodeFromBase64(content);
                string uncompressedContent = GameGZip.UncompressFromBase64(decodedContent);
                JObject jObject = JObject.Parse(uncompressedContent);
                foreach (var itr in jObject)
                {
                    m_Items.Add(itr.Key, itr.Value.ToString());
                }
            }
        }

    }
}