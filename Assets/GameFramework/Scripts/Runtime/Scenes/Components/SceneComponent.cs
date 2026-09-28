/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneComponent.cs
 * author:  云毅
 * created:
 * descrip:   场景管理组件 - 场景加载/卸载/清理、多相机管理、相机动画控制
 * 优化记录: 修复裁剪UI渲染纹理长期占用临时纹理池的泄漏；重复绑定先释放旧纹理；
 *           销毁逻辑O(n²)过滤改为O(n)；补充相机空值防御
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        #region 生命周期
        //=========================================================================

        protected override void Awake()
        {
            base.Awake();

            // 初始化场景管理器（纯C#管理器，负责场景加载/卸载/预加载的核心逻辑）
            m_SceneManager = new SceneManager();
        }

        // 保留空实现：作为框架扩展点，子业务可在此挂载场景初始化/收尾逻辑
        private void Start()
        {
        }

        private void OnDestroy()
        {
        }

        #endregion

        //=========================================================================
        #region 场景对象清理
        //=========================================================================

        /// <summary>
        /// 销毁场景根节点下所有对象
        /// 会自动调用LuaBehaviour的关闭逻辑，防止资源泄漏
        /// </summary>
        /// <param name="root">指定根节点，默认使用场景根节点</param>
        /// <remarks>
        /// 优化说明：使用哈希集合收集所有严格后代，将过滤嵌套子物体的复杂度由 O(n²) 降为 O(n)。
        /// 注意：LuaBehaviour 对象仅调用 Lua 侧 Close 回调，是否销毁 GameObject 由 Lua 业务自行决定。
        /// </remarks>
        public void DestroyAllSceneGOs(GameObject root = null)
        {
            bool isDefault = root == null;
            if (root == null)
                root = m_SceneRootGO;

            // 收集所有直接子物体，并记录全部严格后代变换集合（用于过滤嵌套物体）
            int childCount = root.transform.childCount;
            List<GameObject> topLevelObjects = new List<GameObject>(childCount);
            HashSet<Transform> descendantSet = new HashSet<Transform>();

            for (int index = 0; index < childCount; index++)
            {
                Transform child = root.transform.GetChild(index);
                topLevelObjects.Add(child.gameObject);

                // GetComponentsInChildren 索引0为自身，其余为严格后代
                Transform[] allTransforms = child.GetComponentsInChildren<Transform>(true);
                for (int i = 1; i < allTransforms.Length; i++)
                {
                    descendantSet.Add(allTransforms[i]);
                }
            }

            // 只保留顶级对象（移除嵌套在其他直接子物体之下的子物体）
            topLevelObjects.RemoveAll(go => descendantSet.Contains(go.transform));

            // 销毁对象：Lua对象走Lua关闭，普通对象直接销毁
            for (int i = 0; i < topLevelObjects.Count; i++)
            {
                GameObject go = topLevelObjects[i];
                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                if (luaBehaviour != null)
                {
                    luaBehaviour.CallLuaClose();
                }
                else
                {
                    Destroy(go);
                }
            }

            // 如果不是默认根节点，销毁传入的根节点
            if (!isDefault)
                Destroy(root);
        }

        #endregion

        //=========================================================================
        #region 场景加载/卸载
        //=========================================================================

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
        public void GetLoadedSceneAssetNames(List<List<string>> results) => m_SceneManager.GetLoadedSceneAssetNames(results);
        public List<List<string>> GetLoadingSceneAssetNames() => m_SceneManager.GetLoadingSceneAssetNames();
        public void GetLoadingSceneAssetNames(List<List<string>> results) => m_SceneManager.GetLoadingSceneAssetNames(results);
        public List<List<string>> GetUnloadingSceneAssetNames() => m_SceneManager.GetUnloadingSceneAssetNames();
        public void GetUnloadingSceneAssetNames(List<List<string>> results) => m_SceneManager.GetUnloadingSceneAssetNames(results);

        /// <summary>
        /// 检查场景是否已加载
        /// </summary>
        public bool HasScene(string abPath, string assetName)
        {
            return m_SceneManager.HasScene(abPath, assetName);
        }

        #endregion

        //=========================================================================
        #region 场景相机管理
        //=========================================================================

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
                // 全量开关（for循环替代Lambda，避免闭包分配，并对已销毁相机做空值防御）
                for (int i = 0; i < m_SceneCameras.Count; i++)
                {
                    Camera camera = m_SceneCameras[i];
                    if (camera != null)
                    {
                        camera.enabled = enabled;
                    }
                }
            }
            else
            {
                GetSceneCamera(index).enabled = enabled;
            }
        }

        #endregion

        //=========================================================================
        #region 相机动画控制
        //=========================================================================

        /// <summary>
        /// 初始化相机（无动画）
        /// </summary>
        public void InitSceneCamera(int sceneCameraIndex, Vector3 originalPosition, Quaternion originalRotation, float sizeOrField)
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
            Vector3 targetPosition = default,
            Quaternion targetRotation = default,
            float targetSizeOrField = -1f,
            float targetDuration = 2f,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            if (sceneCameraIndex >= 0 && sceneCameraIndex < m_SceneCameras.Count)
            {
                if (targetPosition == default)
                    targetPosition = originalPosition;
                if (targetSizeOrField == -1f)
                    targetSizeOrField = originalSizeOrField;

                SceneCameraActor actor = m_SceneCameras[sceneCameraIndex].GetOrAddComponent<SceneCameraActor>();
                actor.InitWithAnimation(
                    originalPosition, originalRotation, originalSizeOrField,
                    targetPosition, targetRotation, targetSizeOrField,
                    targetDuration, canInterruptByGestures, overCallback);
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
                actor.PlayAnimationToTarget(
                    duration, targetPosition, targetRotation, targetSizeOrField,
                    canInterruptByGestures, overCallback);
            }
        }

        #endregion

        //=========================================================================
        #region 相机裁剪UI渲染纹理（红框画面限定）
        //=========================================================================

        /// <summary>
        /// 将相机绑定到指定裁剪UI的RawImage，实现画面限定在红框内
        /// </summary>
        /// <param name="cameraIndex">场景相机索引</param>
        /// <param name="targetRawImage">红框内的RawImage组件</param>
        /// <param name="rtWidth">纹理宽度</param>
        /// <param name="rtHeight">纹理高度</param>
        /// <remarks>
        /// 优化说明：长期持有的渲染纹理必须用 new RenderTexture 创建，
        /// 不能用 GetTemporary（临时纹理池仅适合短期借用，长期占用会导致池耗尽）。
        /// 重复绑定同一相机时会先释放旧纹理，防止泄漏。
        /// </remarks>
        public void BindCameraToClipRawImage(int cameraIndex, RawImage targetRawImage, int rtWidth = 1024, int rtHeight = 1024)
        {
            Camera camera = GetSceneCamera(cameraIndex);
            if (camera == null || targetRawImage == null)
                return;

            // 重复绑定时先释放旧纹理，避免相机纹理被覆盖后旧纹理泄漏
            ReleaseCameraRt(camera);

            // 创建适配尺寸的专属渲染纹理（带深度缓冲，抗锯齿）
            RenderTexture rt = new RenderTexture(rtWidth, rtHeight, 24)
            {
                antiAliasing = 4,
                name = $"SceneCameraRt_{camera.name}",
            };
            camera.targetTexture = rt;

            // 把渲染纹理赋值给RawImage，画面就会被父RectMask2D裁剪在红框里
            targetRawImage.texture = rt;
            m_CameraRtMap[camera] = rt;
        }

        /// <summary>
        /// 释放相机绑定的渲染纹理（卸载场景调用，防内存泄漏）
        /// </summary>
        /// <param name="camera">待释放纹理的场景相机</param>
        public void ReleaseCameraRt(Camera camera)
        {
            if (camera == null)
                return;

            if (m_CameraRtMap.TryGetValue(camera, out RenderTexture rt))
            {
                m_CameraRtMap.Remove(camera);
                camera.targetTexture = null;

                // 配套释放：Release 归还GPU显存，Destroy 销毁托管包装
                rt.Release();
                Destroy(rt);
            }
        }

        #endregion
    }
}
