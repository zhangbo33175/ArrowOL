/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolDelegates.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议统一委托定义（对齐 SDK SdkSendFun / SdkCsharpPushFun）
 *            统一 int cmd 首参，作为跨语言消息分发契约
 ***************************************************************/

using XLua;

namespace Honor.Runtime
{
    //=========================================================================
    #region 协议统一委托
    //=========================================================================

    /// <summary>
    /// 协议发送委托（对齐 SDK SdkSendFun）：
    /// 请求 / 广播的统一签名，cmd 定义链接 ID
    /// </summary>
    /// <param name="cmd">协议号（LuaProtocolCmd 链接 ID）</param>
    /// <param name="content">协议内容（强类型数据类，或原始 JSON）</param>
    /// <param name="state">状态（eReceiveType：Success / Fail）</param>
    [CSharpCallLua]
    public delegate void LuaProtocolSendFun(int cmd, object content, object state);

    /// <summary>
    /// 协议推送委托（对齐 SDK SdkCsharpPushFun）：
    /// C# 侧监听 / 服务端推送的统一签名
    /// </summary>
    /// <param name="cmd">协议号（LuaProtocolCmd 链接 ID）</param>
    /// <param name="content">协议内容</param>
    [CSharpCallLua]
    public delegate void LuaProtocolPushFun(int cmd, object content);

    /// <summary>
    /// 协议同步调用委托：
    /// 需要 Lua 立即返回结果的调用（如创建 Lua 类、获取资源信息、本地化文本），
    /// 仍统一走 cmd 分发但保持同步语义（调用栈内完成并返回结果）
    /// </summary>
    /// <param name="cmd">协议号（LuaProtocolCmd 链接 ID）</param>
    /// <param name="content">协议内容（强类型数据类）</param>
    /// <returns>Lua 处理结果（LuaTable / string / 其它 object）</returns>
    [CSharpCallLua]
    public delegate object LuaProtocolSyncFun(int cmd, object content);

    #endregion
}
