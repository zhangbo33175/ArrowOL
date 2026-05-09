using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 场景管理组件
    /// 功能：场景异步/同步加载、卸载、预加载、场景物体清理、多相机管理、相机动画控制
    /// 属于游戏核心系统，通过 GameMainRoot.Scene 访问
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class SceneComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            // 初始化场景管理器
            m_SceneManager = new SceneManager();
            if (m_SceneManager == null)
            {
                Log.Fatal("SceneManager 无效。");
                return;
            }
        }

        private void Start()
        {
        }

        private void OnDestroy()
        {
        }

        /// <summary>
        /// 销毁场景根节点下所有对象
        /// 会自动调用LuaBehaviour的关闭逻辑，防止资源泄漏
        /// </summary>
        /// <param name="root">指定根节点，默认使用场景根节点</param>
        public void DestroyAllSceneGOs(GameObject root = null)
        {
            bool isDefault = root == null;
            if (root == null) root = m_SceneRootGO;

            // 收集所有子物体
            List<GameObject> gameObjects = new List<GameObject>();
            for (int index = 0; index < root.transform.childCount; index++)
            {
                gameObjects.Add(root.transform.GetChild(index).gameObject);
            }

            // 过滤掉子物体（只保留顶级对象）
            gameObjects.RemoveAll((tmp) =>
            {
                foreach (var tmp1 in gameObjects)
                {
                    if (tmp != tmp1 && tmp.transform.IsChildOf(tmp1.transform))
                    {
                        return true;
                    }
                }

                return false;
            });

            // 销毁对象：Lua对象走Lua关闭，普通对象直接销毁
            gameObjects.ForEach((go) =>
            {
                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                if (luaBehaviour != null)
                {
                    luaBehaviour.CallLuaClose();
                }
                else
                {
                    Destroy(go);
                }
            });

            // 如果不是默认根节点，销毁传入的根节点
            if (!isDefault) Destroy(root);
        }

        /// <summary>
        /// 异步预加载场景（后台加载，不激活）
        /// </summary>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">场景资源名</param>
        public void PreLoadSceneAsync(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            m_SceneManager.PreLoadSceneAsync(abPath, assetName);
        }

        /// <summary>
        /// 异步加载场景（可带回调）
        /// </summary>
        public void LoadSceneAsync(string abPath, string assetName, SceneLoadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            m_SceneManager.LoadSceneAsync(abPath, assetName, overCallback);
        }

        /// <summary>
        /// 同步加载场景（阻塞主线程）
        /// </summary>
        public UnityEngine.SceneManagement.Scene LoadSceneSync(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            return m_SceneManager.LoadSceneSync(abPath, assetName);
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        public void UnloadScene(string sceneName, SceneUnloadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameException("Scene sceneName 无效。");
            }

            m_SceneManager.UnloadScene(sceneName, overCallback);
        }

        /// <summary>
        /// 获取已加载/加载中/卸载中的场景列表
        /// </summary>
        public List<List<string>> GetLoadedSceneAssetNames() => m_SceneManager.GetLoadedSceneAssetNames();

        public void GetLoadedSceneAssetNames(List<List<string>> results) =>
            m_SceneManager.GetLoadedSceneAssetNames(results);

        public List<List<string>> GetLoadingSceneAssetNames() => m_SceneManager.GetLoadingSceneAssetNames();

        public void GetLoadingSceneAssetNames(List<List<string>> results) =>
            m_SceneManager.GetLoadingSceneAssetNames(results);

        public List<List<string>> GetUnloadingSceneAssetNames() => m_SceneManager.GetUnloadingSceneAssetNames();

        public void GetUnloadingSceneAssetNames(List<List<string>> results) =>
            m_SceneManager.GetUnloadingSceneAssetNames(results);

        /// <summary>
        /// 检查场景是否已加载
        /// </summary>
        public bool HasScene(string abPath, string assetName)
        {
            return m_SceneManager.HasScene(abPath, assetName);
        }

        #region 场景相机管理

        /// <summary>
        /// 添加相机到场景相机列表
        /// </summary>
        public int AddSceneCamera(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("SceneComponent.AddSceneCamera camera 无效。");
                return -1;
            }

            if (!m_SceneCameras.Contains(camera))
            {
                m_SceneCameras.Add(camera);
            }

            return m_SceneCameras.FindIndex((obj) => obj == camera);
        }

        /// <summary>
        /// 移除场景相机
        /// </summary>
        public bool RemoveSceneCamera(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("SceneComponent.RemoveSceneCamera camera 无效。");
                return false;
            }

            return m_SceneCameras.Remove(camera);
        }

        /// <summary>
        /// 根据索引移除相机
        /// </summary>
        public bool RemoveSceneCameraByIndex(int index)
        {
            if (index < 0 || index >= m_SceneCameras.Count)
            {
                Log.Error("SceneComponent.RemoveSceneCameraByIndex index 无效。");
                return false;
            }

            m_SceneCameras.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// 获取相机索引
        /// </summary>
        public int GetSceneCameraIndex(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("SceneComponent.GetSceneCameraIndex camera 无效。");
                return -1;
            }

            return m_SceneCameras.FindIndex((obj) => obj == camera);
        }

        /// <summary>
        /// 根据索引获取相机
        /// </summary>
        public Camera GetSceneCamera(int index)
        {
            if (index < 0 || index >= m_SceneCameras.Count)
            {
                Log.Error("SceneComponent.GetSceneCamera index 无效。");
                return null;
            }

            return m_SceneCameras[index];
        }

        /// <summary>
        /// 设置相机启用/禁用
        /// </summary>
        public void SetSceneCameraEnable(bool enabled, int index = -1)
        {
            if (index < -1 || index >= m_SceneCameras.Count)
            {
                Log.Error("SceneComponent.SetSceneCameraEnable index 无效。");
            }

            if (index == -1)
            {
                m_SceneCameras.ForEach(cam => cam.enabled = enabled);
            }
            else
            {
                GetSceneCamera(index).enabled = enabled;
            }
        }

        #endregion

        #region 相机动画控制

        /// <summary>
        /// 初始化相机（无动画）
        /// </summary>
        public void InitSceneCamera(int sceneCameraIndex, Vector3 originalPosition, Quaternion originalRotation,
            float sizeOrField)
        {
            if (sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.Init(originalPosition, originalRotation, sizeOrField);
            }
        }

        /// <summary>
        /// 初始化相机（带动画）
        /// </summary>
        public void InitSceneCameraWithAnimation(
            int sceneCameraIndex,
            Vector3 originalPosition,
            Quaternion originalRotation,
            float originalSizeOrField,
            Vector3 targetPosition = default(Vector3),
            Quaternion targetRotation = default(Quaternion),
            float targetSizeOrField = -1f,
            float targetDuration = 2f,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            if (sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                if (targetPosition == default(Vector3)) targetPosition = originalPosition;
                if (targetSizeOrField == -1f) targetSizeOrField = originalSizeOrField;

                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.InitWithAnimation(originalPosition, originalRotation, originalSizeOrField, targetPosition,
                    targetRotation, targetSizeOrField, targetDuration, canInterruptByGestures, overCallback);
            }
        }

        /// <summary>
        /// 播放相机目标动画（位移+旋转+缩放）
        /// </summary>
        public void PlaySceneCameraAnimationToTarget(
            int sceneCameraIndex,
            float duration,
            Vector3 targetPosition,
            Quaternion targetRotation,
            float targetSizeOrField,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            if (sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.PlayAnimationToTarget(duration, targetPosition, targetRotation, targetSizeOrField,
                    canInterruptByGestures, overCallback);
            }
        }

        #endregion
    }
}