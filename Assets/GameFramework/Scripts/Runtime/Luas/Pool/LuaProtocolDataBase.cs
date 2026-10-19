/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolDataBase.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议数据基类（对齐 SDK SGSDKDataBase / SGSDKErrorData）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 协议数据基类：所有强类型协议数据类的统一基类
    /// 供 LuaProtocolDataMap 以 Type 方式统一持有
    /// </summary>
    public class LuaProtocolDataBase
    {
    }

    /// <summary>
    /// 协议错误数据（对齐 SDK SGSDKErrorData）：
    /// 承载协议失败时的错误码与原始错误 Key
    /// </summary>
    public class LuaProtocolErrorData
    {
        /// <summary>错误码</summary>
        public int ErrorCode;

        /// <summary>原始错误 Key</summary>
        public string OriginKey;
    }
}
