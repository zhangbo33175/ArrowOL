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

    #region 资源管理组件 - 声明部分
    /// <summary>
    /// 资源管理组件（声明部分）
    /// 包含所有资源加载相关的委托定义与分部类声明
    /// </summary>
    public sealed partial class AssetComponent : GameComponent
    {

    }
    #endregion
}