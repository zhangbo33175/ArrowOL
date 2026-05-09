using System;
using System.Collections.Generic;
using System.IO;

namespace Honor.Runtime
{
    /// <summary>
    /// 文件片段存储管理器（非 WebGL 平台使用）
    /// 管理多个分类文件（.dat），每个文件独立加密压缩存储
    /// 自动扫描目录、加载、保存、删除文件
    /// </summary>
    public sealed partial class FileFragmentManager
    {
        /// <summary>
        /// 构造方法
        /// 初始化目录 → 扫描 .dat 文件 → 构建内存索引
        /// </summary>
        public FileFragmentManager()
        {
            m_FileFragmentsRootDirectoryFullPath = GamePathUtils.FileFragment.GetRootDirectoryFullPath();
            if (!Directory.Exists(m_FileFragmentsRootDirectoryFullPath))
            {
                Directory.CreateDirectory(m_FileFragmentsRootDirectoryFullPath);
            }

            m_ItemGroups = new SortedDictionary<string, FileFragmentItemGroup>();
            m_FilePaths = new List<string>();
            m_FileFragmentNames = new List<string>();
            m_FileFragmentNamesForDelete = new List<string>();

            // 扫描目录下所有 .dat 数据文件
            string[] fileFragmentFullPaths = Directory.GetFiles(m_FileFragmentsRootDirectoryFullPath, "*.dat");
            for (int index = 0; index < fileFragmentFullPaths.Length; index++)
            {
                m_FilePaths.Add(fileFragmentFullPaths[index].Replace('\\', '/'));
                int startIndex = m_FilePaths[index].LastIndexOf('/') + 1;
                string name = m_FilePaths[index].Substring(startIndex, m_FilePaths[index].Length - startIndex - ".dat".Length);
                m_FileFragmentNames.Add(name);
                m_ItemGroups.Add(name, new FileFragmentItemGroup());
            }

        }

        /// <summary>
        /// 加载所有文件片段数据（从文件读到内存）
        /// </summary>
        /// <returns>是否加载文件片段条目成功。</returns>
        public bool Load()
        {
            for (int index = 0; index < m_FilePaths.Count; index++)
            {
                string filePath = m_FilePaths[index];
                string fileFragmentName = m_FileFragmentNames[index];
                try
                {
                    if (!File.Exists(filePath))
                    {
                        continue;
                    }
                    Deserialize(filePath, fileFragmentName);
                }
                catch (Exception exception)
                {
                    Log.Warning("加载 FileFragment 条目失败，异常为 '{0}'.", exception.ToString());
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 保存所有文件片段（内存 → 文件）
        /// 先删除标记删除的文件，再全部保存
        /// </summary>
        /// <returns>是否保存成功。</returns>
        public bool Save()
        {
            // 删除标记的文件
            for (int index = 0; index < m_FileFragmentNamesForDelete.Count; index++)
            {
                string fullPath = $"{m_FileFragmentsRootDirectoryFullPath}/{m_FileFragmentNamesForDelete[index]}.dat";
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            m_FileFragmentNamesForDelete.Clear();

            // 保存所有文件
            for (int index = 0; index < m_FilePaths.Count; index++)
            {
                string filePath = m_FilePaths[index];
                string fileFragmentName = m_FileFragmentNames[index];
                if (!Serialize(filePath, fileFragmentName))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 单独保存指定分类文件
        /// </summary>
        /// <returns>是否保存成功。</returns>
        public bool Save(string fileFragmentName)
        {
            // 如果是标记删除的，先删除
            if (m_FileFragmentNamesForDelete.Contains(fileFragmentName))
            {
                string fullPath = $"{m_FileFragmentsRootDirectoryFullPath}/{fileFragmentName}.dat";
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                m_FileFragmentNamesForDelete.Remove(fileFragmentName);
                return true;
            }

            // 正常保存
            if (m_FileFragmentNames.Contains(fileFragmentName))
            {
                int index = m_FileFragmentNames.FindIndex((name) => { return name == fileFragmentName; });
                string filePath = m_FilePaths[index];
                if (!Serialize(filePath, fileFragmentName))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定分类的条目的名称集合。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <returns>条目名称集合。</returns>
        public string[] GetAllItemNames(string fileFragmentName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetAllItemNames();
            }
            return null;
        }

        /// <summary>
        /// 获取指定分类的条目的名称集合。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="results">所有条目的名称。</param>
        public void GetAllItemNames(string fileFragmentName, List<string> results)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                m_ItemGroups[fileFragmentName].GetAllItemNames(results);
            }
        }

        /// <summary>
        /// 检查是否存在指定条目。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要检查条目的名称。</param>
        /// <returns>指定的条目是否存在。</returns>
        public bool HasItem(string fileFragmentName, string itemName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].HasItem(itemName);
            }
            return false;
        }

        /// <summary>
        /// 删除指定键（删完自动检查空分类并移除）
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要移除条目的名称。</param>
        /// <returns>是否移除指定条目成功。</returns>
        public bool RemoveItem(string fileFragmentName, string itemName)
        {
            bool result = true;
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                result = m_ItemGroups[fileFragmentName].RemoveItem(itemName);
                CheckRemoveContainer(fileFragmentName);
            }
            return result;
        }

        /// <summary>
        /// 清空数据
        /// null = 清空全部；指定名称 = 清空该分类
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        public void RemoveAllItems(string fileFragmentName)
        {
            if (string.IsNullOrEmpty(fileFragmentName))
            {
                // 清空全部：删除目录重建
                if (Directory.Exists(m_FileFragmentsRootDirectoryFullPath))
                {
                    Directory.Delete(m_FileFragmentsRootDirectoryFullPath, true);
                }
                Directory.CreateDirectory(m_FileFragmentsRootDirectoryFullPath);

                m_ItemGroups.Clear();
                m_FilePaths.Clear();
                m_FileFragmentNames.Clear();
                m_FileFragmentNamesForDelete.Clear();
            }
            else
            {
                // 清空单个分类
                if (m_ItemGroups.ContainsKey(fileFragmentName))
                {
                    m_ItemGroups[fileFragmentName].RemoveAllItems();
                    CheckRemoveContainer(fileFragmentName);
                }
            }

        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string fileFragmentName, string itemName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetBool(itemName);
            }
            return false;
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string fileFragmentName, string itemName, bool defaultValue)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetBool(itemName, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string fileFragmentName, string itemName, bool value)
        {
            CheckAddContainer(fileFragmentName);
            m_ItemGroups[fileFragmentName].SetBool(itemName, value);
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string fileFragmentName, string itemName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetInt(itemName);
            }
            return 0;
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string fileFragmentName, string itemName, int defaultValue)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetInt(itemName, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string fileFragmentName, string itemName, int value)
        {
            CheckAddContainer(fileFragmentName);
            m_ItemGroups[fileFragmentName].SetInt(itemName, value);
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string fileFragmentName, string itemName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetFloat(itemName);
            }
            return 0f;
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string fileFragmentName, string itemName, float defaultValue)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetFloat(itemName, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string fileFragmentName, string itemName, float value)
        {
            CheckAddContainer(fileFragmentName);
            m_ItemGroups[fileFragmentName].SetFloat(itemName, value);
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string fileFragmentName, string itemName)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetString(itemName);
            }
            return null;
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string fileFragmentName, string itemName, string defaultValue)
        {
            if (m_ItemGroups.ContainsKey(fileFragmentName))
            {
                return m_ItemGroups[fileFragmentName].GetString(itemName, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要写入条目的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string fileFragmentName, string itemName, string value)
        {
            CheckAddContainer(fileFragmentName);
            m_ItemGroups[fileFragmentName].SetString(itemName, value);
        }

        /// <summary>
        /// 序列化文件片段
        /// </summary>
        /// <param name="filePath">文件片段路径</param>
        /// <param name="fileFragmentName">文件片段名称</param>
        /// <returns>序列化是否成功</returns>
        private bool Serialize(string filePath, string fileFragmentName = null)
        {
            CheckAddContainer(fileFragmentName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return m_ItemGroups[fileFragmentName].Serialize(fs);
            }
        }

        /// <summary>
        /// 反序列化文件片段
        /// </summary>
        /// <param name="filePath">文件片段路径</param>
        /// <param name="fileFragmentName">文件片段名称</param>
        /// <returns>条目集合</returns>
        private FileFragmentItemGroup Deserialize(string filePath, string fileFragmentName = null)
        {
            CheckAddContainer(fileFragmentName);
            using (StreamReader reader = new StreamReader(filePath))
            {
                m_ItemGroups[fileFragmentName].Deserialize(reader);
            }
            return m_ItemGroups[fileFragmentName];
        }
    }

}


