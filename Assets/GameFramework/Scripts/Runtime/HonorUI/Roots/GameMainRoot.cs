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
                // 仅当 far 大于目标 near 时设置，保证 near &lt; far 构成有效视锥，不裁掉远处物体
                if (cam != null && cam.farClipPlane > near)
                {
                    cam.nearClipPlane = near;
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