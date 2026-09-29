/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PersistComponent.cs
 * author:    云毅
 * created:   2026
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
        /// 组件初始化：按平台创建文件分片存储，并加载 PlayerPrefs
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            InitFragmentStore();
            InitPlayerPrefsStore();
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

        /// <summary>
        /// 创建并加载文件分片存储（WebGL 与常规平台互斥）
        /// </summary>
        private void InitFragmentStore()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            m_WebGLFragmentStore = new FileFragmentForWebGLManager();
            if (!m_WebGLFragmentStore.Load())
            {
                Log.Error("读取FileFragmentForWebGL数据失败。");
            }
#else
            m_DiskFragmentStore = new FileFragmentManager();
            if (!m_DiskFragmentStore.Load())
            {
                Log.Error("读取FileFragment数据失败。");
            }
#endif
        }

        /// <summary>
        /// 创建并加载 PlayerPrefs 存储
        /// </summary>
        private void InitPlayerPrefsStore()
        {
            m_PlayerPrefsStore = new PlayerPrefsManager();
            if (!m_PlayerPrefsStore.Load())
            {
                Log.Error("读取PlayerPrefs数据失败。");
            }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.Save();
#else
                    m_DiskFragmentStore.Save();
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.Save();
                    break;
                }
            }
        }

        /// <summary>
        /// 按分类保存数据
        /// </summary>
        /// <param name="wayType">存储方式</param>
        /// <param name="classifyName">分类名</param>
        public void Save(PersistWayType wayType, string classifyName)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.Save();
#else
                    m_DiskFragmentStore.Save(classifyName);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.Save();
                    break;
                }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetAllItemNames(classifyName);
#else
                    return m_DiskFragmentStore.GetAllItemNames(classifyName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetAllItemNames(classifyName);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取某分类下所有键名（列表）
        /// </summary>
        public void GetAllItemNames(PersistWayType wayType, string classifyName, List<string> results)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.GetAllItemNames(classifyName, results);
#else
                    m_DiskFragmentStore.GetAllItemNames(classifyName, results);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.GetAllItemNames(classifyName, results);
                    break;
                }
            }
        }

        /// <summary>
        /// 判断是否存在某条数据
        /// </summary>
        public bool HasItem(PersistWayType wayType, string classifyName, string itemName)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.HasItem(classifyName, itemName);
#else
                    return m_DiskFragmentStore.HasItem(classifyName, itemName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.HasItem(classifyName, itemName);
                }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.RemoveItem(classifyName, itemName);
#else
                    return m_DiskFragmentStore.RemoveItem(classifyName, itemName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    bool removed = m_PlayerPrefsStore.RemoveItem(classifyName, itemName);
                    SavePlayerPrefsDataAfterFrameEnd();
                    return removed;
                }
            }

            return false;
        }

        /// <summary>
        /// 清空全部数据
        /// </summary>
        public void RemoveAllItems(PersistWayType wayType)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.RemoveAllItems(null);
#else
                    m_DiskFragmentStore.RemoveAllItems(null);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.RemoveAllItems(null);
                    SavePlayerPrefsDataAfterFrameEnd();
                    break;
                }
            }
        }

        /// <summary>
        /// 清空某一分类下所有数据
        /// </summary>
        public void RemoveAllItems(PersistWayType wayType, string classifyName)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.RemoveAllItems(classifyName);
#else
                    m_DiskFragmentStore.RemoveAllItems(classifyName);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.RemoveAllItems(classifyName);
                    SavePlayerPrefsDataAfterFrameEnd();
                    break;
                }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetBool(classifyName, itemName);
#else
                    return m_DiskFragmentStore.GetBool(classifyName, itemName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetBool(classifyName, itemName);
                }
            }

            return false;
        }

        /// <summary>
        /// 读取 bool（带默认值）
        /// </summary>
        public bool GetBool(PersistWayType wayType, string classifyName, string itemName, bool defaultValue)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetBool(classifyName, itemName, defaultValue);
#else
                    return m_DiskFragmentStore.GetBool(classifyName, itemName, defaultValue);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetBool(classifyName, itemName, defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 写入 bool
        /// </summary>
        public void SetBool(PersistWayType wayType, string classifyName, string itemName, bool value)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.SetBool(classifyName, itemName, value);
#else
                    m_DiskFragmentStore.SetBool(classifyName, itemName, value);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.SetBool(classifyName, itemName, value);
                    SavePlayerPrefsDataAfterFrameEnd();
                    break;
                }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetInt(classifyName, itemName);
#else
                    return m_DiskFragmentStore.GetInt(classifyName, itemName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetInt(classifyName, itemName);
                }
            }

            return 0;
        }

        /// <summary>
        /// 读取 int（带默认值）
        /// </summary>
        public int GetInt(PersistWayType wayType, string classifyName, string itemName, int defaultValue)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetInt(classifyName, itemName, defaultValue);
#else
                    return m_DiskFragmentStore.GetInt(classifyName, itemName, defaultValue);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetInt(classifyName, itemName, defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 写入 int
        /// </summary>
        public void SetInt(PersistWayType wayType, string classifyName, string itemName, int value)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.SetInt(classifyName, itemName, value);
#else
                    m_DiskFragmentStore.SetInt(classifyName, itemName, value);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.SetInt(classifyName, itemName, value);
                    SavePlayerPrefsDataAfterFrameEnd();
                    break;
                }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetFloat(classifyName, itemName);
#else
                    return m_DiskFragmentStore.GetFloat(classifyName, itemName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetFloat(classifyName, itemName);
                }
            }

            return 0f;
        }

        /// <summary>
        /// 读取 float（带默认值）
        /// </summary>
        public float GetFloat(PersistWayType wayType, string classifyName, string itemName, float defaultValue)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetFloat(classifyName, itemName, defaultValue);
#else
                    return m_DiskFragmentStore.GetFloat(classifyName, itemName, defaultValue);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetFloat(classifyName, itemName, defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 写入 float
        /// </summary>
        public void SetFloat(PersistWayType wayType, string classifyName, string itemName, float value)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.SetFloat(classifyName, itemName, value);
#else
                    m_DiskFragmentStore.SetFloat(classifyName, itemName, value);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.SetFloat(classifyName, itemName, value);
                    SavePlayerPrefsDataAfterFrameEnd();
                    break;
                }
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
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetString(classifyName, itemName);
#else
                    return m_DiskFragmentStore.GetString(classifyName, itemName);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetString(classifyName, itemName);
                }
            }

            return null;
        }

        /// <summary>
        /// 读取 string（带默认值）
        /// </summary>
        public string GetString(PersistWayType wayType, string classifyName, string itemName, string defaultValue)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    return m_WebGLFragmentStore.GetString(classifyName, itemName, defaultValue);
#else
                    return m_DiskFragmentStore.GetString(classifyName, itemName, defaultValue);
#endif
                }
                case PersistWayType.PlayerPrefs:
                {
                    return m_PlayerPrefsStore.GetString(classifyName, itemName, defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 写入 string
        /// </summary>
        public void SetString(PersistWayType wayType, string classifyName, string itemName, string value)
        {
            switch (wayType)
            {
                case PersistWayType.FileFragment:
                {
#if UNITY_WEBGL && !UNITY_EDITOR
                    m_WebGLFragmentStore.SetString(classifyName, itemName, value);
#else
                    m_DiskFragmentStore.SetString(classifyName, itemName, value);
#endif
                    break;
                }
                case PersistWayType.PlayerPrefs:
                {
                    m_PlayerPrefsStore.SetString(classifyName, itemName, value);
                    SavePlayerPrefsDataAfterFrameEnd();
                    break;
                }
            }
        }

        #endregion
    }
}
