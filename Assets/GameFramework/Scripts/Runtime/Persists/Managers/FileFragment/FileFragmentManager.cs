/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileFragmentManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   非WebGL平台文件片段存储管理器 - 多分类文件、加密、压缩
 ***************************************************************/

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
        #region 构造 & 初始化

        //=========================================================================

        /// <summary>
        /// 构造方法
        /// 初始化目录 → 扫描 .dat 文件 → 构建内存索引
        /// </summary>
        public FileFragmentManager()
        {
            m_RootDir = GamePathUtils.FileFragment.GetRootDirectoryFullPath();
            if (!Directory.Exists(m_RootDir))
            {
                Directory.CreateDirectory(m_RootDir);
            }

            m_Groups = new SortedDictionary<string, FileFragmentItemGroup>();
            m_FileFullPaths = new List<string>();
            m_GroupNames = new List<string>();
            m_PendingDeleteNames = new List<string>();

            // 扫描目录下所有 .dat 数据文件，建立名称 ↔ 路径 ↔ 内存分组的索引
            foreach (string scannedPath in Directory.GetFiles(m_RootDir, "*.dat"))
            {
                string normalizedPath = scannedPath.Replace('\\', '/');
                m_FileFullPaths.Add(normalizedPath);

                int slashPos = normalizedPath.LastIndexOf('/') + 1;
                string groupName = normalizedPath.Substring(slashPos, normalizedPath.Length - slashPos - ".dat".Length);
                m_GroupNames.Add(groupName);
                m_Groups.Add(groupName, new FileFragmentItemGroup());
            }
        }

        #endregion

        //=========================================================================

        #region 加载 & 保存

        //=========================================================================

        /// <summary>
        /// 加载所有文件片段数据（从文件读到内存）
        /// </summary>
        /// <returns>是否加载文件片段条目成功。</returns>
        public bool Load()
        {
            for (int slot = 0; slot < m_FileFullPaths.Count; slot++)
            {
                string targetPath = m_FileFullPaths[slot];
                string targetName = m_GroupNames[slot];
                try
                {
                    if (!File.Exists(targetPath))
                    {
                        continue;
                    }

                    Deserialize(targetPath, targetName);
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
            for (int slot = 0; slot < m_PendingDeleteNames.Count; slot++)
            {
                string pendingPath = PathCombine(m_PendingDeleteNames[slot]);
                if (File.Exists(pendingPath))
                {
                    File.Delete(pendingPath);
                }
            }

            m_PendingDeleteNames.Clear();

            // 保存所有文件
            for (int slot = 0; slot < m_FileFullPaths.Count; slot++)
            {
                string targetPath = m_FileFullPaths[slot];
                string targetName = m_GroupNames[slot];
                if (!Serialize(targetPath, targetName))
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
            if (m_PendingDeleteNames.Remove(fileFragmentName))
            {
                string pendingPath = PathCombine(fileFragmentName);
                if (File.Exists(pendingPath))
                {
                    File.Delete(pendingPath);
                }

                return true;
            }

            // 正常保存
            int slot = m_GroupNames.IndexOf(fileFragmentName);
            if (slot >= 0)
            {
                string targetPath = m_FileFullPaths[slot];
                return Serialize(targetPath, fileFragmentName);
            }

            return false;
        }

        #endregion

        //=========================================================================

        #region 数据查询

        //=========================================================================

        /// <summary>
        /// 获取指定分类的条目的名称集合。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <returns>条目名称集合。</returns>
        public string[] GetAllItemNames(string fileFragmentName)
        {
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetAllItemNames()
                : null;
        }

        /// <summary>
        /// 获取指定分类的条目的名称集合。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="results">所有条目的名称。</param>
        public void GetAllItemNames(string fileFragmentName, List<string> results)
        {
            if (TryGetGroup(fileFragmentName, out FileFragmentItemGroup group))
            {
                group.GetAllItemNames(results);
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
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group) && group.HasItem(itemName);
        }

        #endregion

        //=========================================================================

        #region 删除操作

        //=========================================================================

        /// <summary>
        /// 删除指定键（删完自动检查空分类并移除）
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要移除条目的名称。</param>
        /// <returns>是否移除指定条目成功。</returns>
        public bool RemoveItem(string fileFragmentName, string itemName)
        {
            bool removed = true;
            if (TryGetGroup(fileFragmentName, out FileFragmentItemGroup group))
            {
                removed = group.RemoveItem(itemName);
                CheckRemoveContainer(fileFragmentName);
            }

            return removed;
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
                if (Directory.Exists(m_RootDir))
                {
                    Directory.Delete(m_RootDir, true);
                }

                Directory.CreateDirectory(m_RootDir);

                m_Groups.Clear();
                m_FileFullPaths.Clear();
                m_GroupNames.Clear();
                m_PendingDeleteNames.Clear();
                return;
            }

            // 清空单个分类
            if (TryGetGroup(fileFragmentName, out FileFragmentItemGroup group))
            {
                group.RemoveAllItems();
                CheckRemoveContainer(fileFragmentName);
            }
        }

        #endregion

        //=========================================================================

        #region Bool 存取

        //=========================================================================

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string fileFragmentName, string itemName)
        {
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetBool(itemName)
                : false;
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
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetBool(itemName, defaultValue)
                : defaultValue;
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
            m_Groups[fileFragmentName].SetBool(itemName, value);
        }

        #endregion

        //=========================================================================

        #region Int 存取

        //=========================================================================

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string fileFragmentName, string itemName)
        {
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetInt(itemName)
                : 0;
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
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetInt(itemName, defaultValue)
                : defaultValue;
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
            m_Groups[fileFragmentName].SetInt(itemName, value);
        }

        #endregion

        //=========================================================================

        #region Float 存取

        //=========================================================================

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string fileFragmentName, string itemName)
        {
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetFloat(itemName)
                : 0f;
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
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetFloat(itemName, defaultValue)
                : defaultValue;
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
            m_Groups[fileFragmentName].SetFloat(itemName, value);
        }

        #endregion

        //=========================================================================

        #region String 存取

        //=========================================================================

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="fileFragmentName">指定的文件片段名称。</param>
        /// <param name="itemName">要获取条目的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string fileFragmentName, string itemName)
        {
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetString(itemName)
                : null;
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
            return TryGetGroup(fileFragmentName, out FileFragmentItemGroup group)
                ? group.GetString(itemName, defaultValue)
                : defaultValue;
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
            m_Groups[fileFragmentName].SetString(itemName, value);
        }

        #endregion

        //=========================================================================

        #region 序列化 & 反序列化

        //=========================================================================

        /// <summary>
        /// 序列化文件片段
        /// </summary>
        /// <param name="filePath">文件片段路径</param>
        /// <param name="fileFragmentName">文件片段名称</param>
        /// <returns>序列化是否成功</returns>
        private bool Serialize(string filePath, string fileFragmentName = null)
        {
            CheckAddContainer(fileFragmentName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                return m_Groups[fileFragmentName].Serialize(stream);
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
                m_Groups[fileFragmentName].Deserialize(reader);
            }

            return m_Groups[fileFragmentName];
        }

        #endregion
    }
}
