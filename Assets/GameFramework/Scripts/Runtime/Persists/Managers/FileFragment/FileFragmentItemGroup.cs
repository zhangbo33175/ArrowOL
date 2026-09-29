/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentItemGroup.cs
 * author:    云毅
 * created:   2026
 * descrip:   文件片段存储分组 - 单个分类数据容器（序列化/加解密/压缩）
 ***************************************************************/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Honor.Runtime
{
    /// <summary>
    /// 文件片段存储分组（单个分类的数据容器）
    /// 负责单个分类下的所有键值对管理、序列化/反序列化、加解密、压缩
    /// </summary>
    public sealed class FileFragmentItemGroup
    {
        //=========================================================================
        #region 字段 & 属性
        //=========================================================================

        /// <summary>
        /// 键值对数据集合
        /// Key：条目名称
        /// Value：数据内容（字符串存储）
        /// </summary>
        private readonly SortedDictionary<string, string> m_Store = new SortedDictionary<string, string>();

        /// <summary>
        /// 获取数据集合（只读）
        /// </summary>
        public SortedDictionary<string, string> Items
        {
            get { return m_Store; }
        }

        /// <summary>
        /// 当前分组内的条目总数
        /// </summary>
        public int Count
        {
            get { return m_Store.Count; }
        }

        #endregion

        //=========================================================================
        #region 构造方法
        //=========================================================================

        /// <summary>
        /// 构造方法
        /// </summary>
        public FileFragmentItemGroup()
        {
        }

        #endregion

        //=========================================================================
        #region 数据查询
        //=========================================================================

        /// <summary>
        /// 获取所有条目的名称（数组）
        /// </summary>
        public string[] GetAllItemNames()
        {
            return m_Store.Keys.ToArray();
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
            results.AddRange(m_Store.Keys);
        }

        /// <summary>
        /// 判断是否存在指定条目
        /// </summary>
        /// <param name="itemName">条目名称</param>
        public bool HasItem(string itemName)
        {
            return m_Store.ContainsKey(itemName);
        }

        #endregion

        //=========================================================================
        #region 删除操作
        //=========================================================================

        /// <summary>
        /// 删除指定条目
        /// </summary>
        public bool RemoveItem(string itemName)
        {
            return m_Store.Remove(itemName);
        }

        /// <summary>
        /// 清空当前分组所有数据
        /// </summary>
        public void RemoveAllItems()
        {
            m_Store.Clear();
        }

        #endregion

        //=========================================================================
        #region 内部读取辅助
        //=========================================================================

        /// <summary>
        /// 尝试读取条目的原始字符串值
        /// </summary>
        /// <param name="itemName">条目名称</param>
        /// <param name="warnIfMissing">条目缺失时是否打印告警</param>
        /// <param name="raw">命中时输出原始字符串</param>
        /// <returns>命中返回 true</returns>
        private bool TryReadRaw(string itemName, bool warnIfMissing, out string raw)
        {
            if (m_Store.TryGetValue(itemName, out raw))
            {
                return true;
            }

            if (warnIfMissing)
            {
                Log.Warning("条目 '{0}' 不存在。", itemName);
            }

            return false;
        }

        #endregion

        //=========================================================================
        #region Bool 存取
        //=========================================================================

        /// <summary>
        /// 读取布尔值（不存在则警告）
        /// </summary>
        public bool GetBool(string itemName)
        {
            if (!TryReadRaw(itemName, true, out string raw))
            {
                return false;
            }

            return int.Parse(raw) != 0;
        }

        /// <summary>
        /// 读取布尔值（带默认值）
        /// </summary>
        public bool GetBool(string itemName, bool defaultValue)
        {
            return TryReadRaw(itemName, false, out string raw) ? int.Parse(raw) != 0 : defaultValue;
        }

        /// <summary>
        /// 写入布尔值（存储为 1/0）
        /// </summary>
        public void SetBool(string itemName, bool value)
        {
            m_Store[itemName] = value ? "1" : "0";
        }

        #endregion

        //=========================================================================
        #region Int 存取
        //=========================================================================

        /// <summary>
        /// 读取整数（不存在则警告）
        /// </summary>
        public int GetInt(string itemName)
        {
            if (!TryReadRaw(itemName, true, out string raw))
            {
                return 0;
            }

            return int.Parse(raw);
        }

        /// <summary>
        /// 读取整数（带默认值）
        /// </summary>
        public int GetInt(string itemName, int defaultValue)
        {
            return TryReadRaw(itemName, false, out string raw) ? int.Parse(raw) : defaultValue;
        }

        /// <summary>
        /// 写入整数
        /// </summary>
        public void SetInt(string itemName, int value)
        {
            m_Store[itemName] = value.ToString();
        }

        #endregion

        //=========================================================================
        #region Float 存取
        //=========================================================================

        /// <summary>
        /// 读取浮点数（不存在则警告）
        /// </summary>
        public float GetFloat(string itemName)
        {
            if (!TryReadRaw(itemName, true, out string raw))
            {
                return 0f;
            }

            return float.Parse(raw);
        }

        /// <summary>
        /// 读取浮点数（带默认值）
        /// </summary>
        public float GetFloat(string itemName, float defaultValue)
        {
            return TryReadRaw(itemName, false, out string raw) ? float.Parse(raw) : defaultValue;
        }

        /// <summary>
        /// 写入浮点数
        /// </summary>
        public void SetFloat(string itemName, float value)
        {
            m_Store[itemName] = value.ToString();
        }

        #endregion

        //=========================================================================
        #region String 存取
        //=========================================================================

        /// <summary>
        /// 读取字符串（不存在则警告）
        /// </summary>
        public string GetString(string itemName)
        {
            if (!TryReadRaw(itemName, true, out string raw))
            {
                return null;
            }

            return raw;
        }

        /// <summary>
        /// 读取字符串（带默认值）
        /// </summary>
        public string GetString(string itemName, string defaultValue)
        {
            return TryReadRaw(itemName, false, out string raw) ? raw : defaultValue;
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        public void SetString(string itemName, string value)
        {
            m_Store[itemName] = value;
        }

        #endregion

        //=========================================================================
        #region 序列化 & 反序列化
        //=========================================================================

        /// <summary>
        /// 序列化数据到文件流
        /// 流程：JObject → Json字符串 → GZip压缩 → AES加密 → Base64 → 写入文件
        /// </summary>
        /// <param name="fs">文件流</param>
        public bool Serialize(FileStream fs)
        {
            JObject json = new JObject();
            foreach (KeyValuePair<string, string> entry in m_Store)
            {
                json[entry.Key] = entry.Value;
            }

            string encoded = AESEncrypt.EncodeToBase64(GZip.CompressToBase64(json.ToString()));
            byte[] rawBytes = Converter.GetBytesByString(encoded);
            fs.Write(rawBytes, 0, rawBytes.Length);
            return true;
        }

        /// <summary>
        /// 从流中反序列化数据
        /// 流程：读取字符串 → Base64 → AES解密 → GZip解压 → Json → JObject → 数据字典
        /// </summary>
        /// <param name="reader">流读取器</param>
        public void Deserialize(StreamReader reader)
        {
            m_Store.Clear();
            string content = reader.ReadToEnd();
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            string decoded = AESEncrypt.DecodeFromBase64(content);
            string plain = GZip.UncompressFromBase64(decoded);
            JObject json = JObject.Parse(plain);
            foreach (var pair in json)
            {
                m_Store.Add(pair.Key, pair.Value.ToString());
            }
        }

        #endregion
    }
}
