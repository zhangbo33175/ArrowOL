/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneComponent.Property.cs
 * author:  云毅
 * created:
 * descrip:   场景组件 - 属性定义分部类（编辑器配置 + 外部访问接口）
 ***************************************************************/

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
        //=========================================================================
        #region 序列化字段（编辑器配置）
        //=========================================================================

        /// <summary>
        /// 场景相机列表（编辑器配置）
        /// 管理场景内所有相机，支持多相机切换
        /// </summary>
        [SerializeField] 
        private List<Camera> m_SceneCameras;

        /// <summary>
        /// 场景根节点（编辑器配置）
        /// 所有场景动态生成的物体都挂在该节点下，方便统一销毁与管理
        /// </summary>
        [SerializeField] 
        private GameObject m_SceneRootGO;

        #endregion

        //=========================================================================
        #region 内部组件
        //=========================================================================

        /// <summary>
        /// 场景管理器实例
        /// 底层真正执行场景加载、卸载、预加载逻辑的核心对象
        /// </summary>
        private SceneManager m_SceneManager;

        /// <summary>
        /// 触摸控制组件
        /// 用于相机手势控制、屏幕输入管理
        /// </summary>
        private TouchComponent m_TouchComponent;

        #endregion

        //=========================================================================
        #region 公共属性（外部访问）
        //=========================================================================

        /// <summary>
        /// 场景相机列表
        /// </summary>
        public List<Camera> SceneCameras
        {
            set => m_SceneCameras = value;
            get => m_SceneCameras;
        }

        /// <summary>
        /// 场景根节点
        /// </summary>
        public GameObject SceneRootGO => m_SceneRootGO;

        /// <summary>
        /// 场景管理器
        /// </summary>
        public SceneManager SceneManager => m_SceneManager;

        /// <summary>
        /// 触摸组件
        /// </summary>
        public TouchComponent TouchComponent => m_TouchComponent;

        /// <summary>
        /// 待预加载的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> PreLoadSceneAssetNames => m_SceneManager.PreLoadSceneAssetNames;

        /// <summary>
        /// 正在加载中的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> LoadingSceneAssetNames => m_SceneManager.LoadingSceneAssetNames;

        /// <summary>
        /// 已加载完成的场景资源列表
        /// 格式：<<ab包路径, 场景资源名>>
        /// </summary>
        public List<List<string>> LoadedSceneAssetNames => m_SceneManager.LoadedSceneAssetNames;

        /// <summary>
        /// 正在卸载中的场景资源列表
        /// 格式：<<场景资源名>>
        /// </summary>
        public List<List<string>> UnloadingSceneAssetNames => m_SceneManager.UnloadingSceneAssetNames;

        #endregion
    }
}