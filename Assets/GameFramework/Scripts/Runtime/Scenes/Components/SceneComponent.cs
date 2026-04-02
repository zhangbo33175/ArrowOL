using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class SceneComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            // m_TouchComponent = GameComponentsGroup.GetComponent<TouchComponent>();
            // if (m_TouchComponent == null)
            // {
            //     Log.Fatal("TouchComponent 无效。");
            //     return;
            // }

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
        /// 销毁场景中所有对象
        /// </summary>
        /// <param name="root">指定根节点</param>
        public void DestroyAllSceneGOs(GameObject root = null)
        {
            bool isDefault = root == null ? true : false;
            if(root == null) root = m_SceneRootGO;

            List<GameObject> gameObjects = new List<GameObject>();
            for (int index = 0; index < root.transform.childCount; index++)
            {
                gameObjects.Add(root.transform.GetChild(index).gameObject);
            }
            gameObjects.RemoveAll((tmp) => {
                foreach (var tmp1 in gameObjects)
                {
                    if (tmp != tmp1 && tmp.transform.IsChildOf(tmp1.transform))
                    {
                        return true;
                    }
                }
                return false;
            });
            gameObjects.ForEach((go) => {
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

            if (!isDefault) Destroy(root);
        }

        /// <summary>
        /// 异步预加载场景。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
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

            // 注意：preload队列中的scene通过AssetComponent加载完毕后将直接记录到loaded队列中，中间态loading将不做表现。
            m_SceneManager.PreLoadSceneAsync(abPath, assetName);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        /// <param name="overCallback">场景加载完毕回调。</param>
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
        /// 同步加载场景。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
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
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneName">场景名称。</param>
        /// <param name="overCallback">场景卸载完毕回调。</param>
        public void UnloadScene(string sceneName, SceneUnloadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameException("Scene sceneName 无效。");
            }
            m_SceneManager.UnloadScene(sceneName, overCallback);
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public List<List<string>> GetLoadedSceneAssetNames()
        {
            return m_SceneManager.GetLoadedSceneAssetNames();
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        public void GetLoadedSceneAssetNames(List<List<string>> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            m_SceneManager.GetLoadedSceneAssetNames(results);
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public List<List<string>> GetLoadingSceneAssetNames()
        {
            return m_SceneManager.GetLoadingSceneAssetNames();
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        public void GetLoadingSceneAssetNames(List<List<string>> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }
            m_SceneManager.GetLoadingSceneAssetNames(results);
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        public List<List<string>> GetUnloadingSceneAssetNames()
        {
            return m_SceneManager.GetUnloadingSceneAssetNames();
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <param name="results">正在卸载场景的资源名称。</param>
        public void GetUnloadingSceneAssetNames(List<List<string>> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            m_SceneManager.GetUnloadingSceneAssetNames(results);
        }

        /// <summary>
        /// 检查场景资源是否存在。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        /// <returns>场景资源是否存在。</returns>
        public bool HasScene(string abPath, string assetName)
        {
            return m_SceneManager.HasScene(abPath, assetName);
        }

        /// <summary>
        /// 追加场景相机
        /// </summary>
        /// <param name="camera">场景相机</param>
        /// <returns>场景相机索引值</returns>
        public int AddSceneCamera(Camera camera)
        {
            if(camera == null)
            {
                Log.Error("SceneComponent.AddSceneCamera camera 无效。");
                return -1;
            }

            if(!m_SceneCameras.Contains(camera))
            {
                m_SceneCameras.Add(camera);
            }

            return m_SceneCameras.FindIndex((obj)=> { return obj == camera; });
        }

        /// <summary>
        /// 移除场景相机
        /// </summary>
        /// <param name="camera">场景相机</param>
        /// <returns>是否移除成功</returns>
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
        /// 根据索引值移除场景相机
        /// </summary>
        /// <param name="index">场景相机索引值</param>
        /// <returns>是否移除成功</returns>
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
        /// 获取场景相机索引值
        /// </summary>
        /// <param name="camera">场景相机</param>
        /// <returns>场景相机索引值</returns>
        public int GetSceneCameraIndex(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("SceneComponent.GetSceneCameraIndex camera 无效。");
                return -1;
            }
            return m_SceneCameras.FindIndex((obj) => { return obj == camera; });
        }

        /// <summary>
        /// 获取场景相机
        /// </summary>
        /// <param name="index">场景相机索引值</param>
        /// <returns>场景相机</returns>
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
        /// 设置场景相机是否可用
        /// </summary>
        /// <param name="enabled">可用</param>
        /// <param name="index">索引值</param>
        public void SetSceneCameraEnable(bool enabled, int index = -1)
        {
            if (index < -1 || index >= m_SceneCameras.Count)
            {
                Log.Error("SceneComponent.SetSceneCameraEnable index 无效。");
            }
            if(index == -1)
            {
                m_SceneCameras.ForEach(camera => camera.enabled = enabled);
            }
            else
            {
                GetSceneCamera(index).enabled = enabled;
            }
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="sceneCameraIndex">场景相机列表中的相机index</param>
        /// <param name="originalPosition">初始场景相机位置</param>
        /// <param name="originalRotation">初始场景相机角度</param>
        /// <param name="sizeOrField">初始场景相机尺寸</param>
        public void InitSceneCamera(int sceneCameraIndex, Vector3 originalPosition, Quaternion originalRotation, float sizeOrField)
        {
            if(sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.Init(originalPosition, originalRotation, sizeOrField);
            }
        }

        /// <summary>
        /// 初始化相机（动画）
        /// </summary>
        /// <param name="sceneCameraIndex">场景相机列表中的相机index</param>
        /// <param name="originalPosition">初始场景相机位置</param>
        /// <param name="originalRotation">初始场景相机角度</param>
        /// <param name="originalSizeOrField">初始场景相机的尺寸</param>
        /// <param name="targetPosition">目标相机位置</param>
        /// <param name="targetRotation">目标相机角度</param>
        /// <param name="targetSizeOrField">目标相机位置</param>
        /// <param name="targetDuration">初始场景相机向目标参数递进的动画持续时间</param>
        /// <param name="canInterruptByGestures">动画是否可以被手势打断</param>
        /// <param name="overCallback">动画结束回调</param>
        public void InitSceneCameraWithAnimation(int sceneCameraIndex, Vector3 originalPosition, Quaternion originalRotation, float originalSizeOrField, Vector3 targetPosition = default(Vector3), Quaternion targetRotation = default(Quaternion), float targetSizeOrField = -1f, float targetDuration = 2f, bool canInterruptByGestures = false, Action overCallback = null)
        {
            if (sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                if (targetPosition == default(Vector3))
                {
                    targetPosition = originalPosition;
                }
                if (targetSizeOrField == -1f)
                {
                    targetSizeOrField = originalSizeOrField;
                }
                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.InitWithAnimation(originalPosition, originalRotation, originalSizeOrField, targetPosition, targetRotation, targetSizeOrField, targetDuration, canInterruptByGestures, overCallback);
            }
        }

        /// <summary>
        /// 播放相机向目标递进的动画
        /// </summary>
        /// <param name="sceneCameraIndex">场景相机列表中的相机index</param>
        /// <param name="duration">动画持续时间</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="targetRotation">目标角度</param>
        /// <param name="targetSizeOrField">目标尺寸</param>
        /// <param name="canInterruptByGestures">动画是否可以被手势打断</param>
        /// <param name="overCallback">动画结束回调</param>
        public void PlaySceneCameraAnimationToTarget(int sceneCameraIndex, float duration, Vector3 targetPosition, Quaternion targetRotation, float targetSizeOrField, bool canInterruptByGestures = false, Action overCallback = null)
        {
            if (sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.PlayAnimationToTarget(duration, targetPosition, targetRotation, targetSizeOrField, canInterruptByGestures, overCallback);
            }
        }

    }

}


