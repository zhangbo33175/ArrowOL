using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体资源封装对象
    /// 管理Prefab加载、实例化、回调、引用计数与实例ID
    /// </summary>
    public class PrefabObject
    {
        /// <summary>
        /// AB包路径（必须以 Assets 开头）
        /// </summary>
        public string AssetBundlePath;

        /// <summary>
        /// AB包内的资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 资源完整路径（ABPath + AssetName 组合）
        /// </summary>
        public string AssetPath;

        /// <summary>
        /// 锁定的回调数量
        /// 标记当前帧确定、下一帧执行的回调数，保证异步下一帧回调
        /// </summary>
        public int LockCallbackCount;

        /// <summary>
        /// 预制体加载完成回调列表
        /// 按帧序统一派发
        /// </summary>
        public List<PrefabLoadOverCallback> PrefabLoadOverCallbackList = new List<PrefabLoadOverCallback>();

        /// <summary>
        /// 预制体加载时传入的Lua参数列表
        /// </summary>
        public List<LuaTable> PrefabLoadLuaTableParamList = new List<LuaTable>();

        /// <summary>
        /// 预制体实例化父节点列表
        /// 缓存并按帧设置后自动移除
        /// </summary>
        public List<Transform> PrefabInstancingGOParentList = new List<Transform>();

        /// <summary>
        /// 加载完成的资源对象
        /// </summary>
        public UnityEngine.Object Asset;

        /// <summary>
        /// 资源引用计数
        /// </summary>
        public int RefCount;

        /// <summary>
        /// 由该Prefab实例化出的所有GameObject实例ID集合
        /// 用于实时管理对象生命周期
        /// </summary>
        public HashSet<int> GOInstanceIDs = new HashSet<int>();
    }

    /// <summary>
    /// AssetBundle 封装对象
    /// 管理AB包加载、依赖、引用计数、回调与资源本体
    /// </summary>
    public class AssetBundleObject
    {
        /// <summary>
        /// AB包标准格式化路径
        /// </summary>
        public string FormatPath;

        /// <summary>
        /// AB包引用计数
        /// </summary>
        public int RefCount;

        /// <summary>
        /// 待加载的依赖资源数量
        /// </summary>
        public int DependLoadingCount;

        /// <summary>
        /// 资源来源类型
        /// </summary>
        public OriginType Origin;

        /// <summary>
        /// AB包异步加载请求
        /// </summary>
        public AssetBundleCreateRequest Request;

        /// <summary>
        /// AB包资源本体
        /// </summary>
        public AssetBundle AssetBundles;

        /// <summary>
        /// 依赖的AB包列表
        /// </summary>
        public readonly List<AssetBundleObject> Depends = new List<AssetBundleObject>();

        /// <summary>
        /// AB包加载完成回调列表
        /// </summary>
        public readonly List<AssetBundleLoadOverCallBack> AssetBundleLoadOverCallbacksList =
            new List<AssetBundleLoadOverCallBack>();
    }

    /// <summary>
    /// 普通资源封装对象
    /// 管理资源加载、卸载、引用、弱引用、延迟释放、异步回调
    /// </summary>
    public class AssetObject
    {
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public string TypeName;

        /// <summary>
        /// AB包路径（必须以 Assets 开头）
        /// </summary>
        public string AssetBundlePath;

        /// <summary>
        /// AB包内资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 资源完整路径（ABPath + AssetName）
        /// </summary>
        public string AssetPath;

        /// <summary>
        /// 资源来源类型
        /// </summary>
        public OriginType Origin;

        /// <summary>
        /// 是否为场景资源
        /// </summary>
        public bool IsScene;

        /// <summary>
        /// 锁定的回调数量
        /// 标记当前帧确定、下一帧执行的回调数，保证异步下一帧回调
        /// </summary>
        public int LockCallbackCount;

        /// <summary>
        /// 资源加载完成回调列表
        /// </summary>
        public List<AssetLoadOverCallback> AssetLoadOverCallbackList = new List<AssetLoadOverCallback>();

        /// <summary>
        /// 资源卸载完成回调列表
        /// </summary>
        public List<AssetUnloadOverCallback> AssetUnloadOverCallbackList = new List<AssetUnloadOverCallback>();

        /// <summary>
        /// 资源实例ID
        /// </summary>
        public int InstanceID;

        /// <summary>
        /// 资源异步加载请求
        /// </summary>
        public AsyncOperation Request;

        /// <summary>
        /// 资源本体
        /// 若为场景资源，此字段为 null
        /// </summary>
        public UnityEngine.Object Asset;

        /// <summary>
        /// 是否为弱引用
        /// true：无引用时可自动卸载
        /// false：常驻内存，引用计数为0也不释放
        /// </summary>
        public bool IsWeak = true;

        /// <summary>
        /// 资源引用计数
        /// </summary>
        public int RefCount;

        /// <summary>
        /// 延迟卸载帧数
        /// 用于平摊大量资源卸载性能压力
        /// </summary>
        public int UnloadTickNum;
    }

    /// <summary>
    /// 预加载资源封装对象
    /// 用于预加载队列，记录预加载信息与完成回调
    /// </summary>
    public class PreloadAssetObject
    {
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public string TypeName;

        /// <summary>
        /// AB包路径（必须以 Assets 开头）
        /// </summary>
        public string AssetBundlePath;

        /// <summary>
        /// AB包内资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 资源完整路径
        /// </summary>
        public string AssetPath;

        /// <summary>
        /// 是否为场景资源
        /// </summary>
        public bool IsScene;

        /// <summary>
        /// 是否为弱引用
        /// true：无引用时可自动卸载
        /// false：常驻内存
        /// </summary>
        public bool IsWeak = true;

        /// <summary>
        /// 预加载完成回调
        /// </summary>
        public AssetLoadOverCallback AssetLoadOverCallback = null;
    }
}