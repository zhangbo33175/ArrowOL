/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameMainRoot.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局根管理器 - 唯一核心入口
 *            组织方式参照 GameMain：区域化分区 + Setup 方法拆分 + 生命周期分工。
 *            持有分类门面 Manager（含 HonorUIManager）；Mono 组件统一由 HonorUIManager 持有，
 *            组件实例一律经门面 Manager 访问，本类不再暴露组件转发属性
 ***************************************************************/

using System;
using System.Globalization;
using System.Threading;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏全局根管理器（游戏唯一核心入口）
    /// 功能：持有分类门面管理器，统一调度各门面生命周期
    /// 说明：MonoBehaviour 组件（Launcher/UI/Procedure/Scene/Debugger）统一由 HonorUIManager 门面持有，
    ///       纯代码单例组件一律收敛进门面 Manager（HonorAsset/HonorNet/HonorUtils/HonorGraphics/HonorCore）持有，
    ///       GameMainRoot 不再直接持有任何组件实例，也不暴露组件转发属性，组件一律经门面 Manager 访问
    /// </summary>
    public partial class GameMainRoot : MonoBehaviour
    {
        private static GameMainRoot _instance;

        /// <summary>
        /// 游戏全局根管理器单例实例
        /// </summary>
        public static GameMainRoot Instance => _instance;

        //=========================================================================

        #region 静态组件（全局访问点）

        /// <summary>
        /// 游戏核心单例（第三方/通用管理器）
        /// </summary>
        public static GameManager gameManager { get; private set; }

        //---- ② 分类门面管理器：纯代码单例组件实例唯一由门面 Manager 持有 ----

        /// <summary>
        /// 资源模块门面管理器（HonorAsset 分类统一入口，持有 AssetComponent）
        /// </summary>
        public static HonorAssetManager HonorAssetManager { get; private set; }

        /// <summary>
        /// 网络模块门面管理器（HonorNet 分类统一入口，持有 NetworkComponent）
        /// </summary>
        public static HonorNetManager HonorNetManager { get; private set; }

        /// <summary>
        /// 工具模块门面管理器（HonorUtils 分类统一入口，持有 TouchComponent）
        /// </summary>
        public static HonorUtilsManager HonorUtilsManager { get; private set; }

        /// <summary>
        /// 图形模块门面管理器（HonorGraphics 分类统一入口，转发图形静态工具）
        /// </summary>
        public static HonorGraphicsManager HonorGraphicsManager { get; private set; }

        /// <summary>
        /// 核心模块门面管理器（HonorCore 分类统一入口，持有全部核心纯代码组件）
        /// </summary>
        public static HonorCoreManager HonorCoreManager { get; private set; }

        /// <summary>
        /// UI 模块门面管理器（HonorUI 分类统一入口，持有 Launcher/UI/Procedure/Scene/Debugger 组件）
        /// </summary>
        public static HonorUIManager HonorUIManager { get; private set; }

        #endregion

        //=========================================================================

        #region 生命周期

        /// <summary>
        /// Awake：登记单例 + 初始化基础环境（文化信息）
        /// </summary>
        private void Awake()
        {
            _instance = this;
            SetupCulture();
        }

        /// <summary>
        /// Start：初始化游戏核心单例 + 初始化分类门面管理器
        /// <para>Mono 组件（Launcher/UI/Procedure/Scene/Debugger）由 HonorUIManager 在首次访问时
        /// 经 GameComponentsGroup 自动获取并持有；纯代码单例组件由各门面 Manager 首次访问时自动 Init</para>
        /// </summary>
        private void Start()
        {
            SetupComponents();
            SetupManagers();
            SetupCameras();
        }

        /// <summary>
        /// Update：调度分类门面管理器驱动各纯 C# 单例组件的生命周期
        /// <para>纯代码单例组件自身 Update 不会被 Unity 调用，生命周期驱动统一收敛进门面 Manager，
        /// 本处仅调用各 Manager.DriveUpdate()，保证异步资源加载、计时器、事件队列、Lua GC 持续推进；
        /// UI 相关 Mono 组件由 Unity 生命周期直接驱动，HonorUIManager.DriveUpdate 为统一入口预留。</para>
        /// </summary>
        private void Update()
        {
            HonorAssetManager?.DriveUpdate();
            HonorUIManager?.DriveUpdate();
            HonorCoreManager?.DriveUpdate();
        }

        /// <summary>
        /// OnDestroy：释放分类门面管理器及纯代码组件资源
        /// <para>门面 Manager.Dispose 内部统一 try-catch 释放各组件，避免单点崩溃；最后置空单例。</para>
        /// </summary>
        private void OnDestroy()
        {
            try
            {
                HonorAssetManager?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            try
            {
                HonorUIManager?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            try
            {
                HonorNetManager?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            try
            {
                HonorUtilsManager?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            try
            {
                HonorGraphicsManager?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            try
            {
                HonorCoreManager?.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            // 取消相机近裁剪面持续守卫事件注册，防止单例销毁后静态回调泄漏
            Camera.onPreCull -= OnCameraPreCull;
            // 取消视锥外告警日志监听
            Application.logMessageReceived -= OnFrustumLogMessage;

            _instance = null;
        }

        #endregion

        //=========================================================================

        #region 初始化（Setup）

        /// <summary>
        /// 初始化游戏核心单例
        /// <para>Mono 组件（Launcher/UI/Procedure/Scene/Debugger）统一由 HonorUIManager 门面
        /// 在 Init 阶段经 GameComponentsGroup 获取持有，本类不再直接获取。</para>
        /// </summary>
        private void SetupComponents()
        {
            gameManager = GameManager.Instance;
        }

        /// <summary>
        /// 初始化分类门面管理器（纯代码单例组件实例统一由门面 Manager 持有）
        /// </summary>
        private void SetupManagers()
        {
            HonorAssetManager = Honor.Runtime.HonorAssetManager.Instance;
            HonorNetManager = Honor.Runtime.HonorNetManager.Instance;
            HonorUtilsManager = Honor.Runtime.HonorUtilsManager.Instance;
            HonorGraphicsManager = Honor.Runtime.HonorGraphicsManager.Instance;
            HonorCoreManager = Honor.Runtime.HonorCoreManager.Instance;
            HonorUIManager = Honor.Runtime.HonorUIManager.Instance;
        }

        /// <summary>
        /// 统一设置所有相机近裁剪面为负值
        /// <para>消除 EventSystem/SendMouseEvents 对 z=0 坐标转换(ScreenToWorldPoint/ScreenPointToRay)
        /// 时的 "Screen position out of view frustum" 告警——near 为负时 z=0 换算出的视口深度为正，
        /// 不再越界。与地图模块 MapCamHelper.SetAllCamerasNear 策略一致，全局统一覆盖所有场景相机，
        /// 规避地图场景外（主界面/UI）相机未设负 near 导致的告警。</para>
        /// </summary>
        private void SetupCameras()
        {
            const float near = -10f;
            foreach (Camera cam in Camera.allCameras)
            {
                // 仅对正交相机设置负 near：透视相机负 near 会导致投影矩阵反转/退化，
                // 反而触发 "Screen position out of view frustum" 告警（反投影后视口 x/y 越界）。
                // 正交相机 near=-10 使 z=0 坐标转换落入视锥深度范围内，消除告警。
                //TODO: [修复] 透视相机排除负 near，避免投影矩阵退化触发视锥外告警
                if (cam != null && cam.orthographic && cam.farClipPlane > near)
                {
                    cam.nearClipPlane = near;
                }
            }

            // 持续守卫：注册 Camera.onPreCull 静态事件，任何相机渲染前强制设负 near。
            // 一次性遍历仅覆盖 Start 时刻 Camera.allCameras 内的相机；动态创建 / 后激活 /
            // 绑定 RenderTexture(targetTexture) 后不在 allCameras 的相机近裁剪面仍为正，
            // EventSystem/SendMouseEvents 做 z=0 坐标转换(ScreenToWorldPoint/ScreenPointToRay)
            // 时 z=0 &lt; near 触发 "Screen position out of view frustum" 告警。
            // onPreCull 在每次渲染前回调，逐相机设 near，彻底覆盖所有渲染相机。
            Camera.onPreCull -= OnCameraPreCull;
            Camera.onPreCull += OnCameraPreCull;

            // 自动诊断：捕获 "Screen position out of view frustum" 报错并限频打印相机清单，
            // 用于定位产生该告警的具体相机（含 transform/正交/视口/渲染纹理）。
            Application.logMessageReceived -= OnFrustumLogMessage;
            Application.logMessageReceived += OnFrustumLogMessage;

            // 启动时无条件打印一次所有相机清单，供定位报错相机（logMessageReceived 无法捕获
            // Camera 原生通道的 "Screen position out of view frustum" 报错，故主动输出基线）
            Debug.Log("[CameraInit] 启动相机清单:\n" + BuildCameraDiagnosis());
        }

        /// <summary>
        /// Application.logMessageReceived 回调：捕获视锥外告警并限频打印相机清单（诊断用）
        /// </summary>
        private static float m_LastFrustumLogTime = -10f;

        private static void OnFrustumLogMessage(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Error && type != LogType.Warning) return;
            if (logString == null || !logString.Contains("Screen position out of view frustum")) return;

            // 限频：2 秒内最多打印一次，避免刷屏
            if (Time.realtimeSinceStartup - m_LastFrustumLogTime < 2f) return;
            m_LastFrustumLogTime = Time.realtimeSinceStartup;

            // 从告警日志中解析 Camera rect，精准定位报错相机
            // 日志格式: Screen position out of view frustum (screen pos x, y) (Camera rect x y w h)
            Rect? matchedRect = ParseCameraRectFromLog(logString);
            string matchedCamInfo = FindCameraByPixelRect(matchedRect);

            Debug.Log("[FrustumDebug] 视锥外告警触发!\n" +
                      "原始日志: " + logString + "\n" +
                      "匹配相机: " + matchedCamInfo + "\n" +
                      "全部相机清单:\n" + BuildCameraDiagnosis());
        }

        /// <summary>
        /// 从告警日志中解析 Camera rect 像素坐标
        /// 日志格式示例: (Camera rect 0 190 1080 1610)
        /// </summary>
        private static Rect? ParseCameraRectFromLog(string logString)
        {
            try
            {
                int idx = logString.IndexOf("Camera rect", System.StringComparison.Ordinal);
                if (idx < 0) return null;

                string rest = logString.Substring(idx + "Camera rect".Length).TrimStart();
                var parts = rest.Split(new[] { ' ', '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 4) return null;

                if (float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y) &&
                    float.TryParse(parts[2], out float w) &&
                    float.TryParse(parts[3], out float h))
                {
                    return new Rect(x, y, w, h);
                }
            }
            catch { /* 解析失败忽略 */ }
            return null;
        }

        /// <summary>
        /// 根据像素视口匹配对应的相机，返回相机详细信息
        /// </summary>
        private static string FindCameraByPixelRect(Rect? targetRect)
        {
            if (!targetRect.HasValue) return "未解析到 Camera rect";

            Rect rect = targetRect.Value;
            foreach (Camera cam in Camera.allCameras)
            {
                if (cam == null) continue;
                Rect camRect = cam.pixelRect;
                // 容差 2 像素，避免浮点误差
                if (Mathf.Abs(camRect.x - rect.x) < 2f &&
                    Mathf.Abs(camRect.y - rect.y) < 2f &&
                    Mathf.Abs(camRect.width - rect.width) < 2f &&
                    Mathf.Abs(camRect.height - rect.height) < 2f)
                {
                    var t = cam.transform;
                    return $"★{cam.name}★ | active={cam.gameObject.activeInHierarchy} | " +
                           $"ortho={cam.orthographic} | near={cam.nearClipPlane:F2} | " +
                           $"far={cam.farClipPlane:F2} | size={(cam.orthographic ? cam.orthographicSize : cam.fieldOfView):F2} | " +
                           $"pos={t.position.ToString("F1")} | rt={(cam.targetTexture == null ? "null" : cam.targetTexture.name)}";
                }
            }
            return $"未找到匹配相机 (目标 rect: {rect.x},{rect.y},{rect.width},{rect.height})";
        }

        /// <summary>
        /// 汇总当前所有相机的诊断信息（含旋转/位置/正交尺寸/视口/渲染纹理），定位报错相机
        /// </summary>
        private static string BuildCameraDiagnosis()
        {
            var sb = new System.Text.StringBuilder();
            foreach (Camera cam in Camera.allCameras)
            {
                if (cam == null) continue;
                var t = cam.transform;
                sb.Append(cam.name)
                  .Append("|active=").Append(cam.gameObject.activeInHierarchy)
                  .Append("|near=").Append(cam.nearClipPlane.ToString("F2"))
                  .Append("|far=").Append(cam.farClipPlane.ToString("F2"))
                  .Append("|ortho=").Append(cam.orthographic ? "1" : "0")
                  .Append("|size=").Append((cam.orthographic ? cam.orthographicSize : cam.fieldOfView).ToString("F2"))
                  .Append("|euler=").Append(t.eulerAngles.ToString("F1"))
                  .Append("|pos=").Append(t.position.ToString("F1"))
                  .Append("|rect=").Append(cam.pixelRect)
                  .Append("|rt=").Append(cam.targetTexture == null ? "null" : cam.targetTexture.name)
                  .Append(";");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Camera.onPreCull 静态回调：每个相机渲染前确保近裁剪面为负值
        /// <para>同时限频打印"有旋转的相机"信息——旋转相机是 frustum 告警的高可疑源，
        /// 用于在编辑器/地图界面定位报错相机（logMessageReceived 捕获不到 Camera 原生报错）。</para>
        /// </summary>
        /// <param name="cam">即将渲染的相机</param>
        private static float m_LastRotCamLogTime = -10f;

        private static void OnCameraPreCull(Camera cam)
        {
            if (cam == null) return;

            // 仅对正交相机设置负 near：透视相机负 near 会导致投影矩阵反转/退化，
            // 反投影后视口 x/y 越界，反而触发 "Screen position out of view frustum" 告警。
            //TODO: [修复] 透视相机排除负 near，保持原始正 near，避免投影矩阵退化
            if (cam.orthographic && cam.farClipPlane > -10f)
            {
                cam.nearClipPlane = -10f;
            }

            // 守卫：正交相机 orthographicSize <= 0 时投影矩阵退化，
            // EventSystem 做 ScreenPointToRay 时任何坐标都越界，触发视锥外告警。
            // 临时设为最小有效值，避免退化（地图加载后会被 InitMapCamera 覆盖）。
            if (cam.orthographic && cam.orthographicSize <= 0f)
            {
                cam.orthographicSize = 6.4f; // 与 2DUICamera 默认 size 对齐
            }

            // 诊断：仅打印有旋转的相机（等轴/斜视角相机），限频 5 秒一次
            if (cam.transform.eulerAngles.sqrMagnitude > 0.01f)
            {
                if (Time.realtimeSinceStartup - m_LastRotCamLogTime > 5f)
                {
                    m_LastRotCamLogTime = Time.realtimeSinceStartup;
                    Debug.Log("[CameraDebug] 旋转相机: " + cam.name
                              + "|euler=" + cam.transform.eulerAngles
                              + "|near=" + cam.nearClipPlane
                              + "|far=" + cam.farClipPlane
                              + "|ortho=" + cam.orthographic
                              + "|size=" + (cam.orthographic ? cam.orthographicSize : cam.fieldOfView)
                              + "|rt=" + (cam.targetTexture == null ? "null" : cam.targetTexture.name));
                }
            }
        }

        /// <summary>
        /// 设置全局文化信息
        /// 解决不同国家语言环境下，日期、数字、字符串格式不统一导致的BUG
        /// </summary>
        private void SetupCulture()
        {
            try
            {
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        #endregion
    }
}