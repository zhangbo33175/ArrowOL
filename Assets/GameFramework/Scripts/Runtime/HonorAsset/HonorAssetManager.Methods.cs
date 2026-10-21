/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorAssetManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源模块门面管理器 - 业务便捷方法转发（partial）
 *            收敛资源加载/预制体/场景/卸载对外能力，外部资源调用统一走本管理器
 ***************************************************************/

using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class HonorAssetManager
    {
        //=========================================================================

        #region 普通资源加载

        //=========================================================================

        /// <summary>
        /// 同步加载任意类型资源（仅加载资源，不实例化）
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AssetBundle路径，必须以Assets开头</param>
        /// <param name="assetName">资源名称</param>
        /// <returns>加载完成的资源对象</returns>
        public UnityEngine.Object LoadAssetSync(string typeName, string abPath, string assetName)
        {
            return Asset.LoadAssetSync(typeName, abPath, assetName);
        }

        /// <summary>
        /// 异步加载任意类型资源
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AssetBundle路径，必须以Assets开头</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="overCallback">加载完成回调</param>
        public void LoadAssetAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            Asset.LoadAssetAsync(typeName, abPath, assetName, overCallback);
        }

        /// <summary>
        /// 异步预加载资源，支持弱引用缓存
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AssetBundle路径，必须以Assets开头</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="overCallback">加载完成回调</param>
        /// <param name="isWeak">是否使用弱引用，默认true</param>
        public void PreLoadAssetAsync(string typeName, string abPath, string assetName,
            AssetLoadOverCallback overCallback, bool isWeak = true)
        {
            Asset.PreLoadAssetAsync(typeName, abPath, assetName, overCallback, isWeak);
        }

        /// <summary>
        /// 判断指定资源是否存在
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AssetBundle路径</param>
        /// <param name="assetName">资源名称</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool IsAssetExist(string typeName, string abPath, string assetName)
        {
            return Asset.IsAssetExist(typeName, abPath, assetName);
        }

        #endregion

        //=========================================================================

        #region 预制体加载与实例化

        //=========================================================================

        /// <summary>
        /// 同步加载预制体并自动实例化
        /// </summary>
        /// <param name="abPath">AssetBundle路径，必须以Assets开头</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="parent">实例化父物体</param>
        /// <param name="luaParams">Lua传递的参数，可选</param>
        /// <returns>实例化后的游戏对象</returns>
        public GameObject LoadPrefabSync(string abPath, string assetName, Transform parent, LuaTable luaParams = null)
        {
            return Asset.LoadPrefabSync(abPath, assetName, parent, luaParams);
        }

        /// <summary>
        /// 异步加载预制体并自动实例化
        /// </summary>
        /// <param name="abPath">AssetBundle路径，必须以Assets开头</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="parent">实例化父物体</param>
        /// <param name="luaParams">Lua传递的参数，可选</param>
        /// <param name="overCallback">加载完成回调</param>
        public void LoadPrefabAsync(string abPath, string assetName, Transform parent, LuaTable luaParams = null,
            PrefabLoadOverCallback overCallback = null)
        {
            Asset.LoadPrefabAsync(abPath, assetName, parent, luaParams, overCallback);
        }

        /// <summary>
        /// 直接克隆游戏对象，支持Lua参数传递
        /// </summary>
        /// <param name="parent">克隆目标父物体</param>
        /// <param name="childTemplateGO">模板游戏对象</param>
        /// <param name="luaParams">Lua传递的参数，可选</param>
        /// <returns>克隆后的游戏对象</returns>
        public GameObject InstantiateGO(Transform parent, GameObject childTemplateGO, LuaTable luaParams = null)
        {
            return Asset.InstantiateGO(parent, childTemplateGO, luaParams);
        }

        #endregion

        //=========================================================================

        #region 场景加载

        //=========================================================================

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="abPath">场景所在AB包路径，必须以Assets开头</param>
        /// <param name="sceneName">场景名称</param>
        public void LoadSceneSync(string abPath, string sceneName)
        {
            Asset.LoadSceneSync(abPath, sceneName);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="abPath">场景所在AB包路径，必须以Assets开头</param>
        /// <param name="sceneName">场景名称</param>
        /// <param name="overCallback">加载完成回调</param>
        public void LoadSceneAsync(string abPath, string sceneName, AssetLoadOverCallback overCallback)
        {
            Asset.LoadSceneAsync(abPath, sceneName, overCallback);
        }

        /// <summary>
        /// 异步预加载场景，支持弱引用缓存
        /// </summary>
        /// <param name="abPath">场景所在AB包路径，必须以Assets开头</param>
        /// <param name="sceneName">场景名称</param>
        /// <param name="overCallback">加载完成回调</param>
        /// <param name="isWeak">是否使用弱引用，默认true</param>
        public void PreLoadSceneAsync(string abPath, string sceneName, AssetLoadOverCallback overCallback,
            bool isWeak = true)
        {
            Asset.PreLoadSceneAsync(abPath, sceneName, overCallback, isWeak);
        }

        /// <summary>
        /// 卸载指定场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="overCallback">卸载完成回调，可选</param>
        public void UnloadScene(string sceneName, AssetUnloadOverCallback overCallback = null)
        {
            Asset.UnloadScene(sceneName, overCallback);
        }

        #endregion

        //=========================================================================

        #region 资源卸载与清理

        //=========================================================================

        /// <summary>
        /// 卸载指定资源，支持立即卸载/延迟卸载
        /// </summary>
        /// <param name="asset">需要卸载的资源对象</param>
        /// <param name="overCallback">卸载完成回调，可选</param>
        /// <param name="rightNow">是否立即卸载，默认false</param>
        public void UnloadAsset(UnityEngine.Object asset, AssetUnloadOverCallback overCallback = null,
            bool rightNow = false)
        {
            Asset.UnloadAsset(asset, overCallback, rightNow);
        }

        /// <summary>
        /// 强制卸载所有未使用的资源，释放内存
        /// </summary>
        /// <param name="overCallback">卸载完成回调，可选</param>
        public void ForceUnloadUnusedAssets(Action overCallback = null)
        {
            Asset.ForceUnloadUnusedAssets(overCallback);
        }

        /// <summary>
        /// 加载AssetBundle清单文件，非编辑器模式下必须调用
        /// </summary>
        public void LoadManifest()
        {
            Asset.LoadManifest();
        }

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

        /// <summary>
        /// 为已实例化的预制体游戏对象增加资源引用计数（转发自 PrefabLoadManager）
        /// </summary>
        /// <param name="abPath">AssetBundle路径，必须以Assets开头</param>
        /// <param name="assetName">预制体资源名称</param>
        /// <param name="go">已实例化的游戏对象</param>
        public void AddPrefabAssetRef(string abPath, string assetName, GameObject go)
        {
            Asset.PrefabLoadManager.AddAssetRef(abPath, assetName, go);
        }

        /// <summary>
        /// 销毁预制体实例并释放资源引用（转发自 PrefabLoadManager）
        /// </summary>
        /// <param name="go">待销毁的游戏对象</param>
        /// <param name="rightNow">是否立即销毁，默认false</param>
        public void DestroyPrefab(GameObject go, bool rightNow = false)
        {
            Asset.PrefabLoadManager.Destroy(go, rightNow);
        }

        #endregion
    }
}
