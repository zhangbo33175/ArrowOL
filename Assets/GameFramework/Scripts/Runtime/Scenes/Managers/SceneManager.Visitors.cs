/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneManager.Field.cs
 * author:  云毅
 * created:
 * descrip:   场景管理器 - 成员变量与属性分部类
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 场景管理器（成员变量分部类）
    /// 定义场景加载的所有状态队列、缓存列表与依赖组件
    /// </summary>
    public sealed partial class SceneManager
    {
        //=========================================================================
        #region 成员变量
        //=========================================================================

        /// <summary>
        /// 等待预加载的场景列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        private readonly List<List<string>> m_PreLoadSceneAssetNames;

        /// <summary>
        /// 正在异步加载的场景列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        private readonly List<List<string>> m_LoadingSceneAssetNames;

        /// <summary>
        /// 已加载完成的场景列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        private readonly List<List<string>> m_LoadedSceneAssetNames;

        /// <summary>
        /// 正在异步卸载的场景列表
        /// 格式：<<场景资源名>>（仅存名称）
        /// </summary>
        private readonly List<List<string>> m_UnloadingSceneAssetNames;

        /// <summary>
        /// 资源管理组件（负责AB包加载/卸载）
        /// </summary>
        private AssetComponent m_AssetComponent;

        #endregion

        //=========================================================================
        #region 公共属性
        //=========================================================================

        /// <summary>
        /// 待预加载的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> PreLoadSceneAssetNames => m_PreLoadSceneAssetNames;

        /// <summary>
        /// 正在加载中的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> LoadingSceneAssetNames => m_LoadingSceneAssetNames;

        /// <summary>
        /// 已加载完成的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> LoadedSceneAssetNames => m_LoadedSceneAssetNames;

        /// <summary>
        /// 正在卸载中的场景资源列表
        /// 格式：<<场景资源名>>
        /// </summary>
        public List<List<string>> UnloadingSceneAssetNames => m_UnloadingSceneAssetNames;

        #endregion
    }
}