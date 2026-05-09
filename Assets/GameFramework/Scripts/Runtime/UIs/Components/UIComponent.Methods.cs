using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 加载完成回调委托
    /// </summary>
    /// <param name="abPath">AB包路径</param>
    /// <param name="assetName">资源名称</param>
    /// <param name="luaClass">Lua 回调表</param>
    /// <param name="obj">加载完成的 UI 对象</param>
    public delegate void UILoadOverCallback(string abPath, string assetName, LuaTable luaClass, GameObject obj);

    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// UI 组件初始化入口
        /// 获取依赖组件、创建 UI 管理器、初始化配置
        /// </summary>
        private void Initialize()
        {
            // 获取资源管理组件
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset component 无效。");
                return;
            }

            // 获取多语言管理组件
            m_LocalizationComponent = GameComponentsGroup.GetComponent<LocalizationComponent>();
            if (m_LocalizationComponent == null)
            {
                Log.Fatal("Localization component 无效。");
                return;
            }

            // 获取配置管理组件
            m_ConfigComponent = GameComponentsGroup.GetComponent<ConfigComponent>();
            if (m_ConfigComponent == null)
            {
                Log.Fatal("Config component 无效。");
                return;
            }

            // 初始化 JSON 缓存对象（用于 LuaTable 互转）
            m_CachedJsonObject = new JObject();

            // 创建 UI 核心管理器
            m_UIManager = new UIManager(m_AssetComponent, m_LocalizationComponent, this,
                m_ScreenUICameras, m_SceneUICameras,
                m_ScreenUICanvas, m_SceneUICanvas,
                m_ScreenDesignedResolution, m_ScreenWidthHeightMatchValue,
                m_DestroyMaxNumPerFrame,
                m_CheckTextLocalizings,
                m_WaitingUIABPath, m_WaitingUIAssetName,
                m_FloatWordsUIABPath, m_FloatWordsUIAssetName, m_FloatWordsDuration);

            if (m_UIManager == null)
            {
                Log.Fatal("UIManager 无效。");
                return;
            }
        }

        /// <summary>
        /// 将 LuaTable 转换成 UIInfo 结构
        /// 用于 C# 与 XLua 之间的 UI 配置传递
        /// </summary>
        /// <param name="luaTable">Lua 传入的 UI 配置表</param>
        /// <param name="luaParams">传递给 UI 的自定义参数</param>
        /// <param name="overCallback">UI 加载完成回调</param>
        /// <returns>构造完成的 UIInfo</returns>
        private UIInfo GenerateUIInfo(LuaTable luaTable, LuaTable luaParams = null,
            UILoadOverCallback overCallback = null)
        {
            // 清空缓存 JSON 对象
            m_CachedJsonObject.RemoveAll();

            // 遍历 LuaTable，转为 JObject
            luaTable.ForEach<string, object>((key, value) => { m_CachedJsonObject.Add(new JProperty(key, value)); });

            // JSON 反序列化为 UIInfo
            UIInfo uiInfo = JsonConvert.DeserializeObject<UIInfo>(m_CachedJsonObject.ToString());

            // 附加参数与回调
            uiInfo.LuaParams = luaParams;
            uiInfo.OverCallback = overCallback;

            return uiInfo;
        }
    }
}