using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 场景管理组件（属性定义分部类）
    /// 包含：场景相机、场景根节点、场景管理器、触摸组件、各类场景状态列表
    /// </summary>
    public sealed partial class SceneComponent : GameComponent
    {
        /// <summary>
        /// 场景相机列表（编辑器配置）
        /// 管理场景内所有相机，支持多相机切换
        /// </summary>
        [SerializeField] private List<Camera> m_SceneCameras;

        public List<Camera> SceneCameras
        {
            set => m_SceneCameras = value;
            get => m_SceneCameras;
        }

        /// <summary>
        /// 场景根节点（编辑器配置）
        /// 所有场景动态生成的物体都挂在该节点下，方便统一销毁与管理
        /// </summary>
        [SerializeField] private GameObject m_SceneRootGO;

        public GameObject SceneRootGO
        {
            get => m_SceneRootGO;
        }

        /// <summary>
        /// 场景管理器实例
        /// 底层真正执行场景加载、卸载、预加载逻辑的核心对象
        /// </summary>
        private SceneManager m_SceneManager;

        public SceneManager SceneManager
        {
            get => m_SceneManager;
        }

        /// <summary>
        /// 触摸控制组件
        /// 用于相机手势控制、屏幕输入管理
        /// </summary>
        private TouchComponent m_TouchComponent;

        public TouchComponent TouchComponent
        {
            get => m_TouchComponent;
        }

        /// <summary>
        /// 待预加载的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> PreLoadSceneAssetNames
        {
            get => m_SceneManager.PreLoadSceneAssetNames;
        }

        /// <summary>
        /// 正在加载中的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> LoadingSceneAssetNames
        {
            get => m_SceneManager.LoadingSceneAssetNames;
        }

        /// <summary>
        /// 已加载完成的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> LoadedSceneAssetNames
        {
            get => m_SceneManager.LoadedSceneAssetNames;
        }

        /// <summary>
        /// 正在卸载中的场景资源列表
        /// 格式：<<场景资源名>>
        /// </summary>
        public List<List<string>> UnloadingSceneAssetNames
        {
            get => m_SceneManager.UnloadingSceneAssetNames;
        }
    }
}