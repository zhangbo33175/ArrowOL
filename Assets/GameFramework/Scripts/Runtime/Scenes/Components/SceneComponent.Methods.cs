/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneComponent.Define.cs
 * author:  云毅
 * created:
 * descrip:   场景组件 - 委托定义与分部类声明
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 场景加载完成委托
    /// </summary>
    /// <param name="abPath">场景所在AB包路径</param>
    /// <param name="assetName">场景资源名称</param>
    /// <param name="scene">加载完成的场景对象</param>
    public delegate void SceneLoadOverCallback(
        string abPath, 
        string assetName, 
        UnityEngine.SceneManagement.Scene scene);

    /// <summary>
    /// 场景卸载完成委托
    /// </summary>
    /// <param name="abPath">场景所在AB包路径</param>
    /// <param name="assetName">场景资源名称</param>
    public delegate void SceneUnloadOverCallback(
        string abPath, 
        string assetName);

    /// <summary>
    /// 场景管理组件（分部类 - 定义部分）
    /// 负责场景加载、卸载、管理、相机控制、场景对象清理
    /// </summary>
    public sealed partial class SceneComponent : GameComponent
    {
        // 该文件仅用于委托声明与分部类定义
        // 实现逻辑位于主文件 SceneComponent.cs
    }
}