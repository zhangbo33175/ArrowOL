using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 场景管理器（成员变量分部类）
    /// 定义场景加载的所有状态队列、缓存列表与依赖组件
    /// </summary>
    public sealed partial class SceneManager
    {
        /// <summary>
        /// 等待预加载的场景列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        private readonly List<List<string>> m_PreLoadSceneAssetNames;

        public List<List<string>> PreLoadSceneAssetNames
        {
            get => m_PreLoadSceneAssetNames;
        }

        /// <summary>
        /// 正在异步加载的场景列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        private readonly List<List<string>> m_LoadingSceneAssetNames;

        public List<List<string>> LoadingSceneAssetNames
        {
            get => m_LoadingSceneAssetNames;
        }

        /// <summary>
        /// 已加载完成的场景列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        private readonly List<List<string>> m_LoadedSceneAssetNames;

        public List<List<string>> LoadedSceneAssetNames
        {
            get => m_LoadedSceneAssetNames;
        }

        /// <summary>
        /// 正在异步卸载的场景列表
        /// 格式：<<场景资源名>>（仅存名称）
        /// </summary>
        private readonly List<List<string>> m_UnloadingSceneAssetNames;

        public List<List<string>> UnloadingSceneAssetNames
        {
            get => m_UnloadingSceneAssetNames;
        }

        /// <summary>
        /// 资源管理组件（负责AB包加载/卸载）
        /// </summary>
        private AssetComponent m_AssetComponent;
    }
}