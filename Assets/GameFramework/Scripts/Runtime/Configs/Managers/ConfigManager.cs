/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigManager.cs
 * author:    云毅
 * created: 2025
 * descrip:   配置管理器 - 核心逻辑层
 ***************************************************************/

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 配置管理器（核心逻辑层）
    /// 功能：加密配置加载、JSON解析、开发/正式双配置读取、增删查改管理
    /// 双配置机制：索引0=开发模式，索引1=正式模式
    /// </summary>
    public sealed partial class ConfigManager
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 构造函数：初始化依赖组件与配置字典
        /// </summary>
        public ConfigManager()
        {
            // 获取启动器组件
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                Log.Fatal("Launcher Component 无效。");
                return;
            }

            // 获取资源组件
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            // 初始化配置字典
            m_ConfigDatas = new Dictionary<string, List<ConfigData>>();
        }
        #endregion

        //=========================================================================
        // 配置加载 & 解析
        //=========================================================================
        #region Method - 加载配置
        /// <summary>
        /// 加载全局加密配置文件
        /// 从AB包加载 → XOR解密 → 解析JSON → 存入配置字典
        /// </summary>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">资源名称</param>
        public void LoadConfigs(string abPath, string assetName)
        {
            // 同步加载加密的配置文本资源
            TextAsset configJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("BinaryAsset", abPath, assetName);
            
            // XOR 解密并转换为字符串
            string contentString = Converter.GetString(Encryption.GetQuickXorBytes(configJsonAsset.bytes, ConfigComponent.s_ConfigEncrytionKey));

            // 解析 JSON
            JObject jObject = JObject.Parse(contentString);

            // 遍历所有配置项
            foreach (var data in jObject)
            {
                if (data.Value.Type == JTokenType.Array)
                {
                    foreach (var jd in data.Value)
                    {
                        bool boolValue = false;
                        int intValue = 0;
                        float floatValue = 0f;
                        string stringValue = string.Empty;

                        // 解析字符串类型配置，自动识别 bool / int / float / string
                        if (jd.Type == JTokenType.String)
                        {
                            stringValue = jd.ToString();
                            
                            // 解析布尔
                            if (stringValue.Equals("true"))
                                boolValue = true;
                            else if (stringValue.Equals("false"))
                                boolValue = false;
                            // 解析数字
                            else
                            {
                                int.TryParse(stringValue, out intValue);
                                float.TryParse(stringValue, out floatValue);
                            }
                        }
                        
                        // 添加到配置字典
                        AddConfig(data.Key, boolValue, intValue, floatValue, stringValue);
                    }
                }
            }

            // 卸载配置资源
            m_AssetComponent.UnloadAsset(configJsonAsset, null, true);
        }
        #endregion

        //=========================================================================
        // 配置管理（增删查）
        //=========================================================================
        #region Method - 配置管理
        /// <summary>
        /// 获取所有配置项名称
        /// </summary>
        public List<string> GetAllConfigNames()
        {
            return new List<string>(m_ConfigDatas.Keys);
        }

        /// <summary>
        /// 检查配置项是否存在
        /// </summary>
        public bool HasConfig(string configName)
        {
            return GetConfigData(configName) != null;
        }

        /// <summary>
        /// 添加一条配置数据
        /// 自动创建新列表或追加到现有列表
        /// </summary>
        public void AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            List<ConfigData> configData = GetConfigData(configName);
            
            // 不存在则创建
            if (configData == null)
            {
                configData = new List<ConfigData>();
                m_ConfigDatas.Add(configName, configData);
            }
            
            // 追加配置数据
            configData.Add(new ConfigData(boolValue, intValue, floatValue, stringValue));
        }

        /// <summary>
        /// 移除指定配置项
        /// </summary>
        public bool RemoveConfig(string configName)
        {
            if (!HasConfig(configName))
                return false;

            return m_ConfigDatas.Remove(configName);
        }

        /// <summary>
        /// 清空所有配置
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigDatas.Clear();
        }
        #endregion

        //=========================================================================
        // 配置获取（Bool / Int / Float / String）
        //=========================================================================
        #region Method - 获取配置
        /// <summary>
        /// 获取布尔配置（自动切换开发/正式模式）
        /// </summary>
        public bool GetBool(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));
            
            // 开发模式取[0]，正式模式取[1]
            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].BoolValue;
        }

        /// <summary>
        /// 获取整型配置
        /// </summary>
        public int GetInt(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));

            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].IntValue;
        }

        /// <summary>
        /// 获取浮点配置
        /// </summary>
        public float GetFloat(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));

            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].FloatValue;
        }

        /// <summary>
        /// 获取字符串配置
        /// </summary>
        public string GetString(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));

            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].StringValue;
        }
        #endregion

        //=========================================================================
        // 工具属性
        //=========================================================================
        #region Property - 工具属性
       

        /// <summary>
        /// 是否为开发模式（只读属性）
        /// </summary>
        public bool IsDevelopMode => m_LauncherComponent != null && m_LauncherComponent.DevelopMode;

        /// <summary>
        /// 是否使用本地服务器（只读属性）
        /// </summary>
        public bool IsLocalServer => m_LauncherComponent != null && m_LauncherComponent.IsLocalServer;
        #endregion
    }
}