using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class PersistComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

#if UNITY_WEBGL && !UNITY_EDITOR
            m_FileFragmentForWebGLManager = new FileFragmentForWebGLManager();
            if (m_FileFragmentForWebGLManager == null)
            {
                Log.Fatal("FileFragmentForWebGLManager manager 无效。");
                return;
            }

            if (!m_FileFragmentForWebGLManager.Load())
            {
                Log.Error("读取FileFragmentForWebGL数据失败。");
            }
#else
            m_FileFragmentManager = new FileFragmentManager();
            if (m_FileFragmentManager == null)
            {
                Log.Fatal("FileFragment manager 无效。");
                return;
            }

            if (!m_FileFragmentManager.Load())
            {
                Log.Error("读取FileFragment数据失败。");
            }
#endif
            m_PlayerPrefsManager = new PlayerPrefsManager();
            if (m_PlayerPrefsManager == null)
            {
                Log.Error("PlayerPrefs manager 无效。");
                return;
            }

            if (!m_PlayerPrefsManager.Load())
            {
                Log.Error("读取PlayerPrefs数据失败。");
            }
        }

        private void Start()
        {
        }

        private void OnDestroy()
        {
        }

        /// <summary>
        /// 保存游戏数据。
        /// </summary>
        /// <param name="wayType">持久化存储方式。</param>
        public void Save(PersistWayType wayType)
        {
            if (wayType == PersistWayType.FileFragment)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.Save();
#else
                m_FileFragmentManager.Save();
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.Save();
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.Save();
#else
                m_FileFragmentManager.Save(classifyName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.Save();
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetAllItemNames(classifyName);
#else
                return m_FileFragmentManager.GetAllItemNames(classifyName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetAllItemNames(classifyName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.GetAllItemNames(classifyName, results);
#else
                m_FileFragmentManager.GetAllItemNames(classifyName, results);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.GetAllItemNames(classifyName, results);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.HasItem(classifyName, itemName);
#else
                return m_FileFragmentManager.HasItem(classifyName, itemName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.HasItem(classifyName, itemName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.RemoveItem(classifyName, itemName);
#else
                return m_FileFragmentManager.RemoveItem(classifyName, itemName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                var result = m_PlayerPrefsManager.RemoveItem(classifyName, itemName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.RemoveAllItems(null);
#else
                m_FileFragmentManager.RemoveAllItems(null);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.RemoveAllItems(null);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.RemoveAllItems(classifyName);
#else
                m_FileFragmentManager.RemoveAllItems(classifyName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.RemoveAllItems(classifyName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetBool(classifyName, itemName);
#else
                return m_FileFragmentManager.GetBool(classifyName, itemName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetBool(classifyName, itemName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetBool(classifyName, itemName, defaultValue);
#else
                return m_FileFragmentManager.GetBool(classifyName, itemName, defaultValue);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetBool(classifyName, itemName, defaultValue);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.SetBool(classifyName, itemName, value);
#else
                m_FileFragmentManager.SetBool(classifyName, itemName, value);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.SetBool(classifyName, itemName, value);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetInt(classifyName, itemName);
#else
                return m_FileFragmentManager.GetInt(classifyName, itemName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetInt(classifyName, itemName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetInt(classifyName, itemName, defaultValue);
#else
                return m_FileFragmentManager.GetInt(classifyName, itemName, defaultValue);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetInt(classifyName, itemName, defaultValue);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.SetInt(classifyName, itemName, value);
#else
                m_FileFragmentManager.SetInt(classifyName, itemName, value);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.SetInt(classifyName, itemName, value);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetFloat(classifyName, itemName);
#else
                return m_FileFragmentManager.GetFloat(classifyName, itemName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetFloat(classifyName, itemName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetFloat(classifyName, itemName, defaultValue);
#else
                return m_FileFragmentManager.GetFloat(classifyName, itemName, defaultValue);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetFloat(classifyName, itemName, defaultValue);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.SetFloat(classifyName, itemName, value);
#else
                m_FileFragmentManager.SetFloat(classifyName, itemName, value);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.SetFloat(classifyName, itemName, value);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetString(classifyName, itemName);
#else
                return m_FileFragmentManager.GetString(classifyName, itemName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetString(classifyName, itemName);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.GetString(classifyName, itemName, defaultValue);
#else
                return m_FileFragmentManager.GetString(classifyName, itemName, defaultValue);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.GetString(classifyName, itemName, defaultValue);
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
#if UNITY_WEBGL && !UNITY_EDITOR
                m_FileFragmentForWebGLManager.SetString(classifyName, itemName, value);
#else
                m_FileFragmentManager.SetString(classifyName, itemName, value);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                m_PlayerPrefsManager.SetString(classifyName, itemName, value);
                SavePlayerPrefsDataAfterFrameEnd();
            }
        }
    }
}