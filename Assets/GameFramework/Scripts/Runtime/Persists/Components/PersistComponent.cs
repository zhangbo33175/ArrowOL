/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PersistComponent.cs
 * author:    云毅
 * created:
 * descrip:   全局持久化存储组件，统一封装文件存储 + PlayerPrefs
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 持久化存储组件
    /// 统一管理本地存储：文件分片（WebGL/普通）、PlayerPrefs
    /// 提供 bool/int/float/string 存取、删除、清空、查询接口
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class PersistComponent : GameComponent
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 初始化文件存储（WebGL 特殊处理）
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
            // 初始化 PlayerPrefs
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

        /// <summary>
        /// 启动逻辑
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 销毁逻辑
        /// </summary>
        private void OnDestroy()
        {
        }

        #endregion

        //=========================================================================
        #region 保存操作
        //=========================================================================

        /// <summary>
        /// 保存数据（全量）
        /// </summary>
        /// <param name="wayType">存储方式</param>
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
        /// 按分类保存数据
        /// </summary>
        /// <param name="wayType">存储方式</param>
        /// <param name="classifyName">分类名</param>
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

        #endregion

        //=========================================================================
        #region 数据查询
        //=========================================================================

        /// <summary>
        /// 获取某分类下所有键名（数组）
        /// </summary>
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
        /// 获取某分类下所有键名（列表）
        /// </summary>
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
        /// 判断是否存在某条数据
        /// </summary>
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

        #endregion

        //=========================================================================
        #region 删除操作
        //=========================================================================

        /// <summary>
        /// 删除单条数据
        /// </summary>
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
        /// 清空全部数据
        /// </summary>
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
        /// 清空某一分类下所有数据
        /// </summary>
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

        #endregion

        //=========================================================================
        #region Bool 存取
        //=========================================================================

        /// <summary>
        /// 读取 bool（无默认值）
        /// </summary>
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
        /// 读取 bool（带默认值）
        /// </summary>
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
        /// 写入 bool
        /// </summary>
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

        #endregion

        //=========================================================================
        #region Int 存取
        //=========================================================================

        /// <summary>
        /// 读取 int（无默认值）
        /// </summary>
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
        /// 读取 int（带默认值）
        /// </summary>
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
        /// 写入 int
        /// </summary>
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

        #endregion

        //=========================================================================
        #region Float 存取
        //=========================================================================

        /// <summary>
        /// 读取 float（无默认值）
        /// </summary>
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
        /// 读取 float（带默认值）
        /// </summary>
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
        /// 写入 float
        /// </summary>
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

        #endregion

        //=========================================================================
        #region String 存取
        //=========================================================================

        /// <summary>
        /// 读取 string（无默认值）
        /// </summary>
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
        /// 读取 string（带默认值）
        /// </summary>
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
        /// 写入 string
        /// </summary>
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

        #endregion
    }
}