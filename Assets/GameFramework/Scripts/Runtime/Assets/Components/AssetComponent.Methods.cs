using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 预制体加载完成回调
    //=========================================================================
    /// <summary>
    /// 预制体加载完成回调
    /// </summary>
    /// <param name="prefabObject">预制体资源对象</param>
    /// <param name="gameObject">实例化后的GameObject</param>
    public delegate void PrefabLoadOverCallback(PrefabObject prefabObject, GameObject gameObject);

    //=========================================================================
    // 普通资源加载完成回调
    //=========================================================================
    /// <summary>
    /// 普通资源加载完成回调
    /// </summary>
    /// <param name="assetObject">资源对象</param>
    /// <param name="asset">加载到的资源</param>
    public delegate void AssetLoadOverCallback(AssetObject assetObject, Object asset);

    //=========================================================================
    // 资源卸载完成回调
    //=========================================================================
    /// <summary>
    /// 资源卸载完成回调
    /// </summary>
    /// <param name="assetObject">已卸载的资源对象</param>
    public delegate void AssetUnloadOverCallback(AssetObject assetObject);

    //=========================================================================
    // AssetBundle 加载完成回调
    //=========================================================================
    /// <summary>
    /// AssetBundle 加载完成回调
    /// </summary>
    /// <param name="assetBundleObject">AB包对象</param>
    /// <param name="ab">加载完成的AssetBundle</param>
    public delegate void AssetBundleLoadOverCallBack(AssetBundleObject assetBundleObject, AssetBundle ab);

    #region 【资源管理组件 - 声明部分】
    /// <summary>
    /// 资源管理组件（声明部分）
    /// 包含所有资源加载相关的委托定义与组件声明
    /// </summary>
    public sealed partial class AssetComponent : GameComponent
    {

    }
    #endregion
}