/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.EventDelegates.cs
 * author:    云毅
 * created:   2026
 * descrip:   Lua 协议通讯绑定模块：绑定 C# <-> Lua 统一 cmd 分发句柄（对齐 SDK SGSDKRunTime）
 *            取代旧 16 固定签名强类型委托桥，新通讯方式为 cmd 定义链接 ID 统一分发
 ***************************************************************/

using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 核心组件 - 协议通讯绑定部分
    /// 负责 C# 与 Lua 之间统一 cmd 分发的句柄绑定与分发中心初始化
    /// </summary>
    public sealed partial class LuaComponent : GameComponent
    {
        //=========================================================================
        #region 协议通讯初始化
        //=========================================================================

        /// <summary>
        /// 初始化 C# <-> Lua 统一 cmd 通讯绑定
        /// <para>
        /// 新通讯方式（对齐 SDK）：
        /// 1. 绑定 C# → Lua 广播句柄（单向事件，如应用生命周期 / 按键 / WebSocket / 事件派发）；
        /// 2. 绑定同步调用句柄（需立即返回结果，如创建 Lua 类 / 获取资源信息 / 本地化文本）；
        /// 3. 初始化协议分发中心。
        /// 协议号（cmd）定义链接 ID，见 LuaProtocolCmd / Protocol.pb。
        /// </para>
        /// </summary>
        public void InitLuaBindings()
        {
            // 绑定 C# → Lua 广播句柄（FrameworkProtocolNotify：单向事件分发）
            LuaProtocolRunTime.LuaNotifyHandle = GetGlobalValue<LuaProtocolSendFun>("FrameworkProtocolNotify");

            // 绑定同步调用句柄（FrameworkProtocolSync：需立即返回结果的调用）
            LuaProtocolRunTime.LuaSyncHandle = GetGlobalValue<LuaProtocolSyncFun>("FrameworkProtocolSync");

            // 初始化协议分发中心（幂等，防重复初始化）
            if (LuaProtocolRunTime.Dispatcher == null)
            {
                LuaProtocolRunTime.Initialize(new LuaProtocolDispatcher());
            }

            if (LuaProtocolRunTime.LuaNotifyHandle == null || LuaProtocolRunTime.LuaSyncHandle == null)
            {
                Log.Warning("[LuaComponent] 协议句柄绑定不完整，请检查 Lua 侧是否定义 FrameworkProtocolNotify / FrameworkProtocolSync。");
            }
        }

        #endregion
    }
}
