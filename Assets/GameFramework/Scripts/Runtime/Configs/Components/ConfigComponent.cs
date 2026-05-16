/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigComponent.cs
 * author:    云毅
 * created: 2025
 * descrip:   全局配置管理组件 - 核心逻辑
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 全局配置管理组件
    /// 功能：统一管理游戏配置文件、提供Bool/Int/Float/String配置读取、支持平台差异化配置
    /// 挂载方式：游戏启动时自动初始化，全局唯一
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class ConfigComponent : GameComponent
    {
        //=========================================================================
        // 生命周期
        //=========================================================================
        #region MonoBehaviour
        /// <summary>
        /// 初始化：获取依赖组件、创建配置管理器
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 获取全局资源组件
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            // 初始化配置管理器
            m_ConfigManager = new ConfigManager();
            if (m_ConfigManager == null)
            {
                Log.Fatal("ConfigManager 无效。");
                return;
            }
        }

        /// <summary>
        /// 生命周期 Start（暂未使用）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 销毁时清理（暂未使用）
        /// </summary>
        private void OnDestroy()
        {
        }
        #endregion

        //=========================================================================
        // 配置加载 & 管理
        //=========================================================================
        #region Method - 配置管理
        /// <summary>
        /// 加载全局配置文件
        /// 从指定路径加载所有Configs配置
        /// </summary>
        public void LoadConfigs()
        {
            m_ConfigManager.LoadConfigs(GamePathUtils.Json.GetRootDirectoryRelativePath(), "Configs");
        }

        /// <summary>
        /// 获取所有已加载的配置名称列表
        /// </summary>
        /// <returns>配置名称集合</returns>
        public List<string> GetAllConfigNames()
        {
            return m_ConfigManager.GetAllConfigNames();
        }

        /// <summary>
        /// 检查是否存在指定配置项
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否自动拼接平台后缀</param>
        /// <returns>是否存在</returns>
        public bool HasConfig(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.HasConfig(configName);
        }

        /// <summary>
        /// 移除指定配置项
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否自动拼接平台后缀</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveConfig(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.RemoveConfig(configName);
        }

        /// <summary>
        /// 清空所有配置项
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigManager.RemoveAllConfigs();
        }
        #endregion

        //=========================================================================
        // 配置获取（Bool / Int / Float / String）
        //=========================================================================
        #region Method - 配置获取
        /// <summary>
        /// 获取布尔型配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否自动拼接平台后缀</param>
        /// <returns>布尔值</returns>
        public bool GetBool(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetBool(configName);
        }

        /// <summary>
        /// 获取整型配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否自动拼接平台后缀</param>
        /// <returns>整型值</returns>
        public int GetInt(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetInt(configName);
        }

        /// <summary>
        /// 获取浮点型配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否自动拼接平台后缀</param>
        /// <returns>浮点值</returns>
        public float GetFloat(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetFloat(configName);
        }

        /// <summary>
        /// 获取字符串型配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否自动拼接平台后缀</param>
        /// <returns>字符串值</returns>
        public string GetString(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetString(configName);
        }
        #endregion

        //=========================================================================
        // 平台相关
        //=========================================================================
        #region Method - 平台判断
        /// <summary>
        /// 获取当前构建平台名称（用于差异化配置）
        /// </summary>
        /// <returns>平台名称：iOS / WebGL / Android / Amazon</returns>
        public string GetCurBuildPlatformName()
        {
#if UNITY_IOS
            return "iOS";
#elif UNITY_WEBGL
            return "WebGL";
#else
            return GameMainRoot.Launcher.IsAmazonStore ? "Amazon" : "Android";
#endif
        }
        #endregion
    }
}