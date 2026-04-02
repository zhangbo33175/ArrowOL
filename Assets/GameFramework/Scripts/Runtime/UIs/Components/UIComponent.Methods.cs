using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public delegate void UILoadOverCallback(string abPath, string assetName, LuaTable luaClass, GameObject obj);

    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset component 无效。");
                return;
            }

            m_LocalizationComponent = GameComponentsGroup.GetComponent<LocalizationComponent>();
            if (m_LocalizationComponent == null)
            {
                Log.Fatal("Localization component 无效。");
                return;
            }

            m_ConfigComponent = GameComponentsGroup.GetComponent<ConfigComponent>();
            if (m_ConfigComponent == null)
            {
                Log.Fatal("Config component 无效。");
                return;
            }

            m_CachedJsonObject = new JObject();

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
        /// 生成UIInfo
        /// </summary>
        /// <param name="luaTable">LuaTable</param>
        /// <param name="luaParams">自定义Lua传入参数</param>
        /// <param name="overCallback">异步加载完成回调</param>
        /// <returns></returns>
        private UIInfo GenerateUIInfo(LuaTable luaTable, LuaTable luaParams = null,
            UILoadOverCallback overCallback = null)
        {
            m_CachedJsonObject.RemoveAll();
            luaTable.ForEach<string, object>((key, value) => { m_CachedJsonObject.Add(new JProperty(key, value)); });

            UIInfo uiInfo = JsonConvert.DeserializeObject<UIInfo>(m_CachedJsonObject.ToString());
            uiInfo.LuaParams = luaParams;
            uiInfo.OverCallback = overCallback;

            return uiInfo;
        }
    }
}