using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class UIManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            
        }

        /// <summary>
        /// 生成UIInfo
        /// </summary>
        /// <param name="luaTable">LuaTable</param>
        /// <param name="luaParams">自定义Lua传入参数</param>
        /// <param name="overCallback">异步加载完成回调</param>
        /// <returns></returns>
        private UIInfo GenerateUIInfo(LuaTable luaTable, LuaTable luaParams = null,UILoadOverCallback overCallback = null)
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