using System;
using UnityEngine;

namespace Honor.Runtime
{
    public class AndroidInterface : AndroidJavaProxy
    {
        public Action<AndroidJavaObject> javaObjectCallBack;

        public Action<string> stringCallBack;

        public Action<int> intCallBack;

        public Action<long> longCallBack;

        public Action<float> floatCallBack;

        public Action<byte[]> byteCallBack;

        public Action<string> debugCallBack;

        public Action<bool> boolCallBack;

        public AndroidInterface(string interfaceName) : base(interfaceName)
        {

        }

        public void JavaObjectCallBack(AndroidJavaObject _data)
        {
            if (javaObjectCallBack != null)
            {
                javaObjectCallBack(_data);
            }
        }

        public void BoolCallBack(bool _data)
        {
            if (boolCallBack != null)
            {
                boolCallBack(_data);
            }
        }

        public void StringCallBack(string _data)
        {
            if (stringCallBack != null)
            {
                stringCallBack(_data);
            }
        }

        public void IntCallBack(int _data)
        {
            if (intCallBack != null)
            {
                intCallBack(_data);
            }
        }

        public void LongCallBack(int _data)
        {
            if (longCallBack != null)
            {
                longCallBack(_data);
            }
        }

        public void FloatCallBack(float _data)
        {
            if (floatCallBack != null)
            {
                floatCallBack(_data);
            }
        }

        public void ByteCallBack(byte[] _data)
        {
            if (byteCallBack != null)
            {
                byteCallBack(_data);
            }
        }

        public void DebugCallBack(string _data)
        {
            if (debugCallBack != null)
            {
                debugCallBack(_data);
            }
        }
    }
}