using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Honor.Runtime
{
    /// <summary>
    /// 文件片段存储分组（单个分类的数据容器）
    /// 负责单个分类下的所有键值对管理、序列化/反序列化、加解密、压缩
    /// </summary>
    public sealed class FileFragmentItemGroup
    {
        /// <summary>
        /// 键值对数据集合
        /// Key：条目名称
        /// Value：数据内容（字符串存储）
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
        /// 构造方法
        /// </summary>
        public FileFragmentItemGroup()
        {

        }

        /// <summary>
        /// 当前分组内的条目总数
        /// </summary>
        public int Count
        {
            get
            {
                return m_Items.Count;
            }
        }

        /// <summary>
        /// 获取所有条目的名称（数组）
        /// </summary>
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
        /// 获取所有条目的名称（列表）
        /// </summary>
        /// <param name="results">输出结果列表</param>
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
        /// 判断是否存在指定条目
        /// </summary>
        /// <param name="itemName">条目名称</param>
        public bool HasItem(string itemName)
        {
            return m_Items.ContainsKey(itemName);
        }

        /// <summary>
        /// 删除指定条目
        /// </summary>
        public bool RemoveItem(string itemName)
        {
            return m_Items.Remove(itemName);
        }

        /// <summary>
        /// 清空当前分组所有数据
        /// </summary>
        public void RemoveAllItems()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// 读取布尔值（不存在则警告）
        /// </summary>
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
        /// 读取布尔值（带默认值）
        /// </summary>
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
        /// 写入布尔值（存储为 1/0）
        /// </summary>
        public void SetBool(string itemName, bool value)
        {
            m_Items[itemName] = value ? "1" : "0";
        }

        /// <summary>
        /// 读取整数（不存在则警告）
        /// </summary>
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
        /// 读取整数（带默认值）
        /// </summary>
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
        /// 写入整数
        /// </summary>
        public void SetInt(string itemName, int value)
        {
            m_Items[itemName] = value.ToString();
        }

        /// <summary>
        /// 读取浮点数（不存在则警告）
        /// </summary>
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
        /// 读取浮点数（带默认值）
        /// </summary>
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
        /// 写入浮点数
        /// </summary>
        public void SetFloat(string itemName, float value)
        {
            m_Items[itemName] = value.ToString();
        }

        /// <summary>
        /// 读取字符串（不存在则警告）
        /// </summary>
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
        /// 读取字符串（带默认值）
        /// </summary>
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
        /// 写入字符串
        /// </summary>
        public void SetString(string itemName, string value)
        {
            m_Items[itemName] = value;
        }

        /// <summary>
        /// 序列化数据到文件流
        /// 流程：JObject → Json字符串 → GZip压缩 → AES加密 → Base64 → 写入文件
        /// </summary>
        /// <param name="fs">文件流</param>
        public bool Serialize(FileStream fs)
        {
            JObject jObject = new JObject();
            foreach (var item in m_Items)
            {
                jObject[item.Key] = item.Value;
            }
            string encodedContent = AESEncrypt.EncodeToBase64(GZip.CompressToBase64(jObject.ToString()));
            byte[] bytes = Converter.GetBytesByString(encodedContent);
            fs.Write(bytes, 0, bytes.Length);
            return true;
        }

        /// <summary>
        /// 从流中反序列化数据
        /// 流程：读取字符串 → Base64 → AES解密 → GZip解压 → Json → JObject → 数据字典
        /// </summary>
        /// <param name="reader">流读取器</param>
        public void Deserialize(StreamReader reader)
        {
            m_Items.Clear();
            string content = reader.ReadToEnd();
            if (!string.IsNullOrEmpty(content))
            {
                string decodedContent = AESEncrypt.DecodeFromBase64(content);
                string uncompressedContent = GZip.UncompressFromBase64(decodedContent);
                JObject jObject = JObject.Parse(uncompressedContent);
                foreach (var itr in jObject)
                {
                    m_Items.Add(itr.Key, itr.Value.ToString());
                }
            }
        }

    }
}