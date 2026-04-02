using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class SceneComponent : GameComponent
    {
        /// <summary>
        /// 场景相机列表
        /// </summary>
        [SerializeField]
        private List<Camera> m_SceneCameras;
        public List<Camera> SceneCameras
        {
            set
            {
                m_SceneCameras = value;
            }
            get
            {
                return m_SceneCameras;
            }
        }

        /// <summary>
        /// 场景根节点对象
        /// </summary>
        [SerializeField]
        private GameObject m_SceneRootGO;
        public GameObject SceneRootGO
        {
            get
            {
                return m_SceneRootGO;
            }
        }

        /// <summary>
        /// Scene管理器
        /// </summary>
        private SceneManager m_SceneManager;
        public SceneManager SceneManager
        {
            get
            {
                return m_SceneManager;
            }
        }

        /// <summary>
        /// Touch组件
        /// </summary>
        private TouchComponent m_TouchComponent;
        public TouchComponent TouchComponent
        {
            get
            {
                return m_TouchComponent;
            }
        }

        /// <summary>
        /// 准备预加载Scene资源信息
        /// 格式：<<abPath, assetName>>
        /// </summary>
        public List<List<string>> PreLoadSceneAssetNames
        {
            get
            {
                return m_SceneManager.PreLoadSceneAssetNames;
            }
        }

        /// <summary>
        /// 正在加载中的Scene资源信息
        /// 格式：<<abPath, assetName>>
        /// </summary>
        public List<List<string>> LoadingSceneAssetNames
        {
            get
            {
                return m_SceneManager.LoadingSceneAssetNames;
            }
        }

        /// <summary>
        /// 已经加载的Scene资源信息
        /// 格式：<<abPath, assetName>>
        /// </summary>
        public List<List<string>> LoadedSceneAssetNames
        {
            get
            {
                return m_SceneManager.LoadedSceneAssetNames;
            }
        }

        /// <summary>
        /// 正在卸载中的Scene资源信息
        /// 格式：<<assetName>>
        /// </summary>
        public List<List<string>> UnloadingSceneAssetNames
        {
            get
            {
                return m_SceneManager.UnloadingSceneAssetNames;
            }
        }

    }

}


