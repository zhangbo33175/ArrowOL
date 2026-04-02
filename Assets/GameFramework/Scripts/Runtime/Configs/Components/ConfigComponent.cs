
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class ConfigComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            m_ConfigManager = new ConfigManager();
            if (m_ConfigManager == null)
            {
                Log.Fatal("ConfigManager 无效。");
                return;
            }

        }

        private void Start()
        {

        }

        private void OnDestroy()
        {

        }

        /// <summary>
        /// 加载全局配置
        /// </summary>
        public void LoadConfigs()
        {
            m_ConfigManager.LoadConfigs(GamePathUtils.Json.GetRootDirectoryRelativePath(), "Configs");
        }

        /// <summary>
        /// 获取所有配置名称集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllConfigNames()
        {
            return m_ConfigManager.GetAllConfigNames();
        }

        /// <summary>
        /// 检查是否存在指定全局配置项。
        /// </summary>
        /// <param name="configName">要检查全局配置项的名称。</param>
        /// <param name="onPlatform">是否整合平台类型。</param>
        /// <returns>指定的全局配置项是否存在。</returns>
        public bool HasConfig(string configName, bool onPlatform = false)
        {
            if(onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.HasConfig(configName);
        }

        /// <summary>
        /// 移除指定全局配置项。
        /// </summary>
        /// <param name="configName">要移除全局配置项的名称。</param>
        /// <param name="onPlatform">是否整合平台类型。</param>
        /// <returns>是否移除全局配置项成功。</returns>
        public bool RemoveConfig(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.RemoveConfig(configName);
        }

        /// <summary>
        /// 清空所有全局配置项。
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigManager.RemoveAllConfigs();
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="onPlatform">是否整合平台类型。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetBool(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="onPlatform">是否整合平台类型。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetInt(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="onPlatform">是否整合平台类型。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetFloat(configName);
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="onPlatform">是否整合平台类型。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName, bool onPlatform = false)
        {
            if (onPlatform)
            {
                configName = AorTxt.Format("{0}{1}", configName, GetCurBuildPlatformName());
            }
            return m_ConfigManager.GetString(configName);
        }

        /// <summary>
        /// 获取当前构建平台名称
        /// </summary>
        /// <returns></returns>
        public string GetCurBuildPlatformName()
        {
#if UNITY_IOS
            return "iOS";
#elif UNITY_WEBGL
            return "WebGL";
#else
            return GameMainRoot.Launcher.IsAmazonStore? "Amazon" : "Android";
#endif
        }

    }

}
