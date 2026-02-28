using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class PersistManager: GameComponent
    {
        private static PersistManager instance;
        public static PersistManager Instance() {
            return instance;
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        /// <summary>
        /// 保存游戏数据。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        public void Save(PersistWayType wayType)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.Save();
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.Save();
            }
        }

        /// <summary>
        /// 保存游戏数据。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        public void Save(PersistWayType wayType, string classifyName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.Save(classifyName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.Save();
            }
        }

        /// <summary>
        /// 获取所有条目的名称。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <returns>所有条目名称。</returns>
        public string[] GetAllItemNames(PersistWayType wayType, string classifyName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetAllItemNames(classifyName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetAllItemNames(classifyName);
            }

            return null;
        }

        /// <summary>
        /// 获取所有条目的名称。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="results">所有条目名称。</param>
        public void GetAllItemNames(PersistWayType wayType, string classifyName, List<string> results)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.GetAllItemNames(classifyName, results);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.GetAllItemNames(classifyName, results);
            }
        }

        /// <summary>
        /// 检查是否存在指定条目。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>指定的条目是否存在。</returns>
        public bool HasItem(PersistWayType wayType, string classifyName, string itemName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.HasItem(classifyName, itemName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.HasItem(classifyName, itemName);
            }

            return false;
        }

        /// <summary>
        /// 移除指定条目。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        public bool RemoveItem(PersistWayType wayType, string classifyName, string itemName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.RemoveItem(classifyName, itemName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                var result = m_PlayerPrefsComponent.RemoveItem(classifyName, itemName);
                SavePlayerPrefsDataAfterFrameEnd();
                return result;
            }

            return false;
        }

        /// <summary>
        /// 清空所有条目。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        public void RemoveAllItems(PersistWayType wayType)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.RemoveAllItems(null);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.RemoveAllItems(null);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }

        /// <summary>
        /// 清空指定分类名称的所有条目。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        public void RemoveAllItems(PersistWayType wayType, string classifyName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.RemoveAllItems(classifyName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.RemoveAllItems(classifyName);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(PersistWayType wayType, string classifyName, string itemName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetBool(classifyName, itemName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetBool(classifyName, itemName);
            }

            return false;
        }

        /// <summary>
        /// 从指定条目中读取布尔值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(PersistWayType wayType, string classifyName, string itemName, bool defaultValue)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetBool(classifyName, itemName, defaultValue);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetBool(classifyName, itemName, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入布尔值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(PersistWayType wayType, string classifyName, string itemName, bool value)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.SetBool(classifyName, itemName, value);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.SetBool(classifyName, itemName, value);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(PersistWayType wayType, string classifyName, string itemName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetInt(classifyName, itemName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetInt(classifyName, itemName);
            }

            return 0;
        }

        /// <summary>
        /// 从指定条目中读取整数值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(PersistWayType wayType, string classifyName, string itemName, int defaultValue)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetInt(classifyName, itemName, defaultValue);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetInt(classifyName, itemName, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入整数值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(PersistWayType wayType, string classifyName, string itemName, int value)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.SetInt(classifyName, itemName, value);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.SetInt(classifyName, itemName, value);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(PersistWayType wayType, string classifyName, string itemName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetFloat(classifyName, itemName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetFloat(classifyName, itemName);
            }

            return 0f;
        }

        /// <summary>
        /// 从指定条目中读取浮点数值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(PersistWayType wayType, string classifyName, string itemName, float defaultValue)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetFloat(classifyName, itemName, defaultValue);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetFloat(classifyName, itemName, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入浮点数值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(PersistWayType wayType, string classifyName, string itemName, float value)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.SetFloat(classifyName, itemName, value);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.SetFloat(classifyName, itemName, value);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(PersistWayType wayType, string classifyName, string itemName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetString(classifyName, itemName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetString(classifyName, itemName);
            }

            return null;
        }

        /// <summary>
        /// 从指定条目中读取字符串值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="defaultValue">当指定的条目不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(PersistWayType wayType, string classifyName, string itemName, string defaultValue)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.GetString(classifyName, itemName, defaultValue);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.GetString(classifyName, itemName, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// 向指定条目写入字符串值。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        /// <param name="classifyName">分类名称。</param>
        /// <param name="itemName">条目名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(PersistWayType wayType, string classifyName, string itemName, string value)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                m_FileFragmentComponent.SetString(classifyName, itemName, value);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsComponent.SetString(classifyName, itemName, value);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }
    }
}