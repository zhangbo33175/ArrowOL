using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
   /// <summary>
        /// 加载封装
        /// </summary>
        /// <summary>
        /// 预加载AB封装对象
        /// </summary>
        public class LoadABData
        {
            /// <summary>
            /// AB标准名称格式
            /// </summary>
            public string FormatPath;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int RefCount;

            /// <summary>
            /// 依赖资源总数
            /// </summary>
            public int DependLoadingCount;

            /// <summary>
            /// 资源位置来源
            /// </summary>
            public ResourceType Resource;

            /// <summary>
            /// AB异步加载请求
            /// </summary>
            public AssetBundleCreateRequest Request;

            /// <summary>
            /// AssetBundle
            /// </summary>
            public AssetBundle AssetBundle;

            /// <summary>
            /// 依赖AB集合
            /// </summary>
            public readonly List<LoadABData> Depends = new List<LoadABData>();

            /// <summary>
            /// AB异步加载完成回调列表
            /// </summary>
            public readonly List<LoadAssetBundleOverCallBack> ABLoadOverCallbacksList =
                new List<LoadAssetBundleOverCallBack>();
        }

        /// <summary>
        /// 预加载AssetObject封装对象
        /// </summary>
        public class LoadAssetData
        {
            /// <summary>
            /// Asset类型名称
            /// </summary>
            public string TypeName;

            /// <summary>
            /// AB包路径
            /// 以Assets开头
            /// </summary>
            public string ABPath;

            /// <summary>
            /// AB中Asset名称
            /// </summary>
            public string AssetName;

            /// <summary>
            /// 经ABPath与AssetName结合后Asset路径
            /// </summary>
            public string AssetPath;

            /// <summary>
            /// 资源位置来源
            /// </summary>
            public ResourceType Resource;

            /// <summary>
            /// 是否为Scene
            /// </summary>
            public bool IsScene;

            /// <summary>
            /// 本轮（帧）已经确定下来的需要在下一帧进行的回调数量
            /// 保证异步是下一帧回调
            /// </summary>
            public int LockCallbackCount;

            /// <summary>
            /// Asset加载完成回调函数集合
            /// </summary>
            public List<LoadAssetOverCallback> LoadAssetOverCallbackList = new List<LoadAssetOverCallback>();

            /// <summary>
            /// Asset卸载完成回调函数集合
            /// </summary>
            public List<UnloadAssetOverCallback> UnloadAssetOverCallbackList = new List<UnloadAssetOverCallback>();

            /// <summary>
            /// Asset实例ID
            /// </summary>
            public int InstanceID;

            /// <summary>
            /// 异步请求
            /// </summary>
            public AsyncOperation Request;

            /// <summary>
            /// Asset资源
            /// 当加载资源为Scene时该Asset为null
            /// </summary>
            public Object Asset;

            /// <summary>
            /// 是否是弱引用
            /// 用于预加载和释放
            /// 为true时，表示这个资源可以在没有引用时卸载，否则常驻内存。
            /// 常驻内存是指引用计数为0也不卸载。
            /// </summary>
            public bool IsWeak = true;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int RefCount;

            /// <summary>
            /// 延迟卸载的帧数
            /// UNLOAD_DELAY_FIXED_FRAME_NUM + m_UnloadList.Count
            /// 用上面的方式赋值来保证在加入unload释放队列中的时候一定是后加入的Asset比前一个加入的Asset晚一帧释放
            /// 这样可以保证在某个时刻大量卸载的时候，资源卸载的压力平摊到后面一段时间上，兼顾效率和内存
            /// </summary>
            public int UnloadTickNum;
        }

        /// <summary>
        /// 预加载PrefabObject封装对象
        /// </summary>
        public class LoadPrefabData
        {
            /// <summary>
            /// AB包路径
            /// 以Assets开头
            /// </summary>
            public string ABPath;


            /// <summary>
            /// AB中Asset名称
            /// </summary>
            public string AssetName;

            /// <summary>
            /// 经ABPath与AssetName结合后Asset路径
            /// </summary>
            public string AssetPath;

            /// <summary>
            /// 本轮（帧）已经确定下来的需要在下一帧进行的回调数量
            /// 保证异步是下一帧回调
            /// </summary>
            public int LockCallbackCount;

            /// <summary>
            /// Prefab加载完成回调函数集合
            /// 按帧规律抛出
            /// </summary>
            public List<LoadPrefabOverCallback> PrefabLoadOverCallbackList = new List<LoadPrefabOverCallback>();
            //
            // /// <summary>
            // /// Prefab加载完成自定义传入参数集合
            // /// 按帧规律抛出
            // /// </summary>
             public List<LuaTable> PrefabLoadLuaTableParamList = new List<LuaTable>();

            /// <summary>
            /// Prefab实例化时指定的父对象集合
            /// 按帧规律缓存并设置后移除
            /// </summary>
            public List<Transform> PrefabInstancingGOParentList = new List<Transform>();

            /// <summary>
            /// Asset资源
            /// </summary>
            public UnityEngine.Object Asset;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int RefCount;

            /// <summary>
            /// 通过该Prefab实例化的对象的实例ID集合
            /// 实时维护
            /// </summary>
            public HashSet<int> GOInstanceIDs = new HashSet<int>();
        }

        /// <summary>
        /// 预加载PreloadAssetObject封装对象
        /// </summary>
        public class PreloadAssetData
        {
            /// <summary>
            /// Asset类型名称
            /// </summary>
            public string TypeName;

            /// <summary>
            /// AB包路径
            /// 以Assets开头
            /// </summary>
            public string ABPath;

            /// <summary>
            /// AB中Asset名称
            /// </summary>
            public string AssetName;

            /// <summary>
            /// 经ABPath与AssetName结合后Asset路径
            /// </summary>
            public string AssetPath;

            /// <summary>
            /// 是否为Scene
            /// </summary>
            public bool IsScene;

            /// <summary>
            /// 是否是弱引用
            /// 用于预加载和释放
            /// 为true时，表示这个资源可以在没有引用时卸载，否则常驻内存。
            /// 常驻内存是指引用计数为0也不卸载。
            /// </summary>
            public bool IsWeak = true;

            /// <summary>
            /// Asset预加载完成回调函数
            /// </summary>
            public LoadAssetOverCallback LoadAssetOverCallback = null;
        }
}