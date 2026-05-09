using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Unity 与 Android 原生交互的接口代理类
    /// 作用：接收 Android 端回调，并转发到 C# 逻辑层
    /// 继承 AndroidJavaProxy，实现安卓接口通信
    /// </summary>
    public class AndroidInterface : AndroidJavaProxy
    {
        /// <summary>
        /// AndroidJavaObject 类型回调（返回安卓对象）
        /// </summary>
        public Action<AndroidJavaObject> javaObjectCallBack;

        /// <summary>
        /// 字符串类型回调
        /// </summary>
        public Action<string> stringCallBack;

        /// <summary>
        /// 整型回调
        /// </summary>
        public Action<int> intCallBack;

        /// <summary>
        /// 长整型回调
        /// </summary>
        public Action<long> longCallBack;

        /// <summary>
        /// 浮点型回调
        /// </summary>
        public Action<float> floatCallBack;

        /// <summary>
        /// 字节数组回调
        /// </summary>
        public Action<byte[]> byteCallBack;

        /// <summary>
        /// 调试日志回调
        /// </summary>
        public Action<string> debugCallBack;

        /// <summary>
        /// 布尔类型回调
        /// </summary>
        public Action<bool> boolCallBack;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="interfaceName">安卓端完整接口名（如：com.xxx.xxx.IUnityCallback）</param>
        public AndroidInterface(string interfaceName) : base(interfaceName)
        {
        }

        /// <summary>
        /// 安卓回调：返回 AndroidJavaObject
        /// </summary>
        public void JavaObjectCallBack(AndroidJavaObject _data)
        {
            javaObjectCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：返回布尔值
        /// </summary>
        public void BoolCallBack(bool _data)
        {
            boolCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：返回字符串
        /// </summary>
        public void StringCallBack(string _data)
        {
            stringCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：返回整型
        /// </summary>
        public void IntCallBack(int _data)
        {
            intCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：返回长整型
        /// 【注意】安卓传入 int，C# 接收为 long
        /// </summary>
        public void LongCallBack(int _data)
        {
            longCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：返回浮点数
        /// </summary>
        public void FloatCallBack(float _data)
        {
            floatCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：返回字节数组
        /// </summary>
        public void ByteCallBack(byte[] _data)
        {
            byteCallBack?.Invoke(_data);
        }

        /// <summary>
        /// 安卓回调：调试日志
        /// </summary>
        public void DebugCallBack(string _data)
        {
            debugCallBack?.Invoke(_data);
        }
    }
}