
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class ConfigManager
    {
        public ConfigManager()
        {
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                Log.Fatal("Launcher Component 无效。");
                return;
            }

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            m_ConfigDatas = new Dictionary<string, List<ConfigData>>();

        }

        /// <summary>
        /// 加载全局配置
        /// </summary>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset资源名称</param>
        public void LoadConfigs(string abPath, string assetName)
        {
            TextAsset configJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("BinaryAsset", abPath, assetName);
            string contentString = Converter.GetString(Encryption.GetQuickXorBytes(configJsonAsset.bytes, ConfigComponent.s_ConfigEncrytionKey));

            JObject jObject = JObject.Parse(contentString);

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
                        if (jd.Type == JTokenType.String)
                        {
                            stringValue = jd.ToString();
                            if (stringValue.Equals("true"))
                            {
                                boolValue = true;
                            }
                            else if (stringValue.Equals("false"))
                            {
                                boolValue = false;
                            }
                            else
                            {
                                int.TryParse(stringValue, out intValue);
                                float.TryParse(stringValue, out floatValue);
                            }
                        }
                        AddConfig(data.Key, boolValue, intValue, floatValue, stringValue);
                    }
                }
            }

            m_AssetComponent.UnloadAsset(configJsonAsset, null, true);
        }

        /// <summary>
        /// 获取所有配置名称集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllConfigNames()
        {
            return new List<string>(m_ConfigDatas.Keys);
        }

        /// <summary>
        /// 检查是否存在指定全局配置项。
        /// </summary>
        /// <param name="configName">要检查全局配置项的名称。</param>
        /// <returns>指定的全局配置项是否存在。</returns>
        public bool HasConfig(string configName)
        {
            return GetConfigData(configName) != null;
        }

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="boolValue">全局配置项布尔值。</param>
        /// <param name="intValue">全局配置项整数值。</param>
        /// <param name="floatValue">全局配置项浮点数值。</param>
        /// <param name="stringValue">全局配置项字符串值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
        public void AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
            {
                configData = new List<ConfigData>();
                m_ConfigDatas.Add(configName, configData);
            }
            configData.Add(new ConfigData(boolValue, intValue, floatValue, stringValue));
        }

        /// <summary>
        /// 移除指定全局配置项。
        /// </summary>
        /// <param name="configName">要移除全局配置项的名称。</param>
        public bool RemoveConfig(string configName)
        {
            if (!HasConfig(configName))
            {
                return false;
            }

            return m_ConfigDatas.Remove(configName);
        }

        /// <summary>
        /// 清空所有全局配置项。
        /// </summary>
        public void RemoveAllConfigs()
        {
            m_ConfigDatas.Clear();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值
        /// </summary>
        /// <param name="configName">要获取配置项的名称</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
            {
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));
            }
            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].BoolValue;
        }

        /// <summary>
        /// 从指定配置项中读取整数值
        /// </summary>
        /// <param name="configName">要获取配置项的名称</param>
        /// <returns>读取的整数值</returns>
        public int GetInt(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
            {
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));
            }
            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].IntValue;
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值
        /// </summary>
        /// <param name="configName">要获取配置项的名称</param>
        /// <returns>读取的浮点数值</returns>
        public float GetFloat(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
            {
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));
            }
            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].FloatValue;
        }

        /// <summary>
        /// 从指定配置项中读取字符串值
        /// </summary>
        /// <param name="configName">要获取配置项的名称</param>
        /// <returns>读取的字符串值</returns>
        public string GetString(string configName)
        {
            List<ConfigData> configData = GetConfigData(configName);
            if (configData == null)
            {
                throw new GameException(AorTxt.Format("配置项'{0}'不存在。", configName));
            }
            return configData[m_LauncherComponent.DevelopMode ? 0 : 1].StringValue;
        }
        
        /// <summary>
        /// 是否是开发模式
        /// </summary>
        public bool IsDevelopMode => m_LauncherComponent != null && m_LauncherComponent.DevelopMode;
        
        /// <summary>
        /// 是否使用本地服务器
        /// </summary>
        public bool IsLocalServer => m_LauncherComponent != null && m_LauncherComponent.IsLocalServer;
    }

}


