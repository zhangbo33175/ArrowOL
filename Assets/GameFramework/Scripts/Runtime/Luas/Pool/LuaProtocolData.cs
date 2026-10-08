/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolData.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议强类型数据类（对齐 SDK SGSDKMap 的 CS/SC 类）
 *            字段编号与 Docs/ProgramConfig/ProtoPB/Protocol.pb 一致
 ***************************************************************/

using XLua;

namespace Honor.Runtime
{
    //=========================================================================
    #region ① 类创建 / 生命周期数据类
    //=========================================================================

    /// <summary>
    /// 创建 LuaBehaviour 类参数（cmd: 1001）
    /// </summary>
    public class CreateLuaClass : LuaProtocolDataBase
    {
        /// <summary>Lua 环境（字段 1）</summary>
        public LuaTable Env;

        /// <summary>Lua 脚本名（字段 2）</summary>
        public string LuaScriptName;
    }

    /// <summary>
    /// 创建流程（Procedure）Lua 类参数（cmd: 1002）
    /// </summary>
    public class CreateProcedureLuaClass : LuaProtocolDataBase
    {
        /// <summary>Lua 环境（字段 1）</summary>
        public LuaTable Env;

        /// <summary>Lua 脚本名（字段 2）</summary>
        public string LuaScriptName;
    }

    #endregion

    //=========================================================================
    #region ② 应用 / 输入数据类
    //=========================================================================

    /// <summary>
    /// 应用暂停 / 唤醒参数（cmd: 1011）
    /// </summary>
    public class ApplicationPause : LuaProtocolDataBase
    {
        /// <summary>是否暂停（字段 1）</summary>
        public bool Pause;
    }

    /// <summary>
    /// 应用退出参数（cmd: 1012，无字段）
    /// </summary>
    public class ApplicationQuit : LuaProtocolDataBase
    {
    }

    /// <summary>
    /// 键盘按键抬起参数（cmd: 1013）
    /// </summary>
    public class KeysUp : LuaProtocolDataBase
    {
        /// <summary>按键码（字段 1）</summary>
        public int KeyCode;
    }

    #endregion

    //=========================================================================
    #region ③ 登录授权数据类
    //=========================================================================

    /// <summary>
    /// Apple / Google 登录结果参数（cmd: 1021）
    /// </summary>
    public class SignInWithApple : LuaProtocolDataBase
    {
        /// <summary>登录结果表（字段 1）</summary>
        public LuaTable ResultTable;
    }

    /// <summary>
    /// 登录状态查询参数（cmd: 1022）
    /// </summary>
    public class SignInWithAppleState : LuaProtocolDataBase
    {
        /// <summary>状态表（字段 1）</summary>
        public LuaTable StateTable;
    }

    #endregion

    //=========================================================================
    #region ④ 事件 / 资源 / 本地化数据类
    //=========================================================================

    /// <summary>
    /// C# 事件派发参数（cmd: 1031）
    /// </summary>
    public class ReceiveEvent : LuaProtocolDataBase
    {
        /// <summary>事件码（字段 1）</summary>
        public int EventCmd;

        /// <summary>事件对象（字段 2）</summary>
        public object EventObj;
    }

    /// <summary>
    /// 本地化语言表数据关联参数（cmd: 1032，无字段）
    /// </summary>
    public class RelateLocalizationTableData : LuaProtocolDataBase
    {
    }

    /// <summary>
    /// 获取资源定义信息参数（cmd: 1033）
    /// </summary>
    public class GetResDefInfo : LuaProtocolDataBase
    {
        /// <summary>资源名（字段 1）</summary>
        public string Name;
    }

    /// <summary>
    /// UI 本地化文本获取参数（cmd: 1034）
    /// </summary>
    public class Localizing : LuaProtocolDataBase
    {
        /// <summary>本地化 Key（字段 1）</summary>
        public string LocalizingKey;
    }

    #endregion

    //=========================================================================
    #region ⑤ WebSocket 数据类
    //=========================================================================

    /// <summary>
    /// WebSocket 建立成功参数（cmd: 1041）
    /// </summary>
    public class WebSocketOpen : LuaProtocolDataBase
    {
        /// <summary>运行时承载 WebSocket 实例（非 pb 字段，供 Lua 侧定位与回发）</summary>
        public object Socket;
    }

    /// <summary>
    /// WebSocket 文本消息接收参数（cmd: 1042）
    /// </summary>
    public class WebSocketMessageReceived : LuaProtocolDataBase
    {
        /// <summary>运行时承载 WebSocket 实例（非 pb 字段）</summary>
        public object Socket;

        /// <summary>接收的文本消息（字段 1）</summary>
        public string Message;
    }

    /// <summary>
    /// WebSocket 字节流消息接收参数（cmd: 1043）
    /// </summary>
    public class WebSocketBinaryReceived : LuaProtocolDataBase
    {
        /// <summary>运行时承载 WebSocket 实例（非 pb 字段）</summary>
        public object Socket;

        /// <summary>接收的字节流（字段 1）</summary>
        public byte[] Datas;
    }

    /// <summary>
    /// WebSocket 关闭参数（cmd: 1044）
    /// </summary>
    public class WebSocketClosed : LuaProtocolDataBase
    {
        /// <summary>运行时承载 WebSocket 实例（非 pb 字段）</summary>
        public object Socket;

        /// <summary>关闭码（字段 1）</summary>
        public int Code;

        /// <summary>关闭描述（字段 2）</summary>
        public string Message;
    }

    /// <summary>
    /// WebSocket 错误参数（cmd: 1045）
    /// </summary>
    public class WebSocketError : LuaProtocolDataBase
    {
        /// <summary>运行时承载 WebSocket 实例（非 pb 字段）</summary>
        public object Socket;

        /// <summary>错误信息（字段 1）</summary>
        public string Error;
    }

    #endregion
}
