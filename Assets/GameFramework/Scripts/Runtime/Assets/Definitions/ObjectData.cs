/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ObjectData.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源系统实体类定义
 *            包含 PrefabObject / AssetBundleObject / AssetObject / PreloadAssetObject
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    #region 预制体资源封装对象
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

        //=========================================================================

        /// <summary>
        /// 新建预制体封装对象并填充路径基础字段（引用计数置 1）
        /// </summary>
        /// <param name="abPath">AB包路径（必须以 Assets 开头）</param>
        /// <param name="assetName">AB包内资源名称</param>
        /// <param name="assetPath">资源完整路径（唯一标识）</param>
        /// <returns>填充好路径字段的预制体对象</returns>
        public static PrefabObject CreatePrefabObject(string abPath, string assetName, string assetPath)
        {
            return new PrefabObject
            {
                AssetBundlePath = abPath,
                AssetName = assetName,
                AssetPath = assetPath,
                RefCount = 1,
            };
        }
    }
    #endregion

    #region AssetBundle 封装对象
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
    #endregion

    #region 普通资源封装对象
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

        //=========================================================================

        /// <summary>
        /// 新建资源封装对象并填充基础字段
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AB包路径（必须以 Assets 开头）</param>
        /// <param name="assetName">AB包内资源名称</param>
        /// <param name="assetPath">资源完整路径（唯一标识）</param>
        /// <param name="overCallback">加载完成回调（默认 null；非空时自动登记进回调列表）</param>
        /// <param name="refCount">引用计数（默认 0；同步新增常驻资源时传 1）</param>
        /// <returns>填充好基础字段的资源对象</returns>
        public static AssetObject CreateAssetObject(string typeName, string abPath, string assetName,
            string assetPath, AssetLoadOverCallback overCallback = null, int refCount = 0)
        {
            AssetObject assetObj = new AssetObject
            {
                TypeName = typeName,
                AssetBundlePath = abPath,
                AssetName = assetName,
                AssetPath = assetPath,
                IsScene = typeName.Equals("Scene"),
                RefCount = refCount,
            };

            if (overCallback != null)
            {
                assetObj.AssetLoadOverCallbackList.Add(overCallback);
            }

            return assetObj;
        }
    }
    #endregion

    #region 预加载资源封装对象
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

        //=========================================================================

        /// <summary>
        /// 新建预加载资源封装对象并填充基础字段
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">AB包内资源名称</param>
        /// <param name="assetPath">资源完整路径</param>
        /// <param name="isWeak">是否为弱引用</param>
        /// <param name="overCallback">预加载完成回调（默认 null）</param>
        /// <returns>填充好基础字段的预加载对象</returns>
        public static PreloadAssetObject CreatePreloadAssetObject(string typeName, string abPath,
            string assetName, string assetPath, bool isWeak, AssetLoadOverCallback overCallback = null)
        {
            return new PreloadAssetObject
            {
                TypeName = typeName,
                AssetBundlePath = abPath,
                AssetName = assetName,
                AssetPath = assetPath,
                IsScene = typeName.Equals("Scene"),
                IsWeak = isWeak,
                AssetLoadOverCallback = overCallback,
            };
        }
    }
    #endregion
}