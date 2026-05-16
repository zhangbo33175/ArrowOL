/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetDelegateDefine.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源系统委托定义文件 - 所有加载/卸载回调委托声明
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    #region 预制体加载完成回调
    /// <summary>
    /// 预制体加载完成回调委托
    /// </summary>
    /// <param name="prefabObject">预制体资源包装对象</param>
    /// <param name="gameObject">实例化后的游戏对象</param>
    public delegate void PrefabLoadOverCallback(PrefabObject prefabObject, GameObject gameObject);
    #endregion

    #region 普通资源加载完成回调
    /// <summary>
    /// 普通资源加载完成回调委托
    /// </summary>
    /// <param name="assetObject">资源包装对象</param>
    /// <param name="asset">加载成功的资源对象</param>
    public delegate void AssetLoadOverCallback(AssetObject assetObject, Object asset);
    #endregion

    #region 资源卸载完成回调
    /// <summary>
    /// 资源卸载完成回调委托
    /// </summary>
    /// <param name="assetObject">已完成卸载的资源包装对象</param>
    public delegate void AssetUnloadOverCallback(AssetObject assetObject);
    #endregion

    #region AssetBundle 加载完成回调
    /// <summary>
    /// AssetBundle加载完成回调委托
    /// </summary>
    /// <param name="assetBundleObject">AB包包装对象</param>
    /// <param name="ab">加载完成的AssetBundle实例</param>
    public delegate void AssetBundleLoadOverCallBack(AssetBundleObject assetBundleObject, AssetBundle ab);
    #endregion

    #region 资源管理组件 - 分部类声明
    /// <summary>
    /// 资源管理组件 - 分部类声明（委托定义部分）
    /// 包含所有资源加载相关的委托定义与分部类声明
    /// </summary>
    public sealed partial class AssetComponent : GameComponent
    {
        
    }
    #endregion
}