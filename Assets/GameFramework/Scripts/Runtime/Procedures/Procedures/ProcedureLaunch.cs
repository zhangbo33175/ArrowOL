/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ProcedureLaunch.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏启动流程 - 游戏入口第一个流程，负责版本升级、初始化、跳转到预加载
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏启动流程（游戏入口第一个流程）
    /// 功能：版本升级、存档转换、GDPR、性能设置、配置加载、多语言初始化、WebGL特殊处理
    /// 完成后自动跳转到预加载流程 ProcedurePreload
    /// </summary>
    public class ProcedureLaunch : ProcedureState
    {
        //=========================================================================
        #region 私有变量
        //=========================================================================

        /// <summary>
        /// WebGL 启动加载界面组件
        /// </summary>
        private UILauncherLoadingView _mUILauncherLoadingView;

        #endregion

        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 流程初始化：设置流程名称
        /// </summary>
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);
            m_Name = "ProcedureLaunch";
        }

        /// <summary>
        /// 进入启动流程：版本转换、GDPR、性能设置、WebGL 热更检查
        /// </summary>
        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            // 切换流程时清空所有资源
            RemoveAllContentsOnProcedureTransition = true;

            // ==============================================
            // 存档版本升级逻辑（旧版本存档自动升级兼容）
            // ==============================================
            string versionStringInSave = GameMainRoot.Persist.GetString(
                GameConstants.Persist.Common.WayType,
                GameConstants.Persist.Common.ClassifyName,
                GameConstants.Persist.Common.ItemKey.Version,
                "1.3.0");

            if (Version.Parse(versionStringInSave) < Version.Parse("1.3.0"))
            {
                SortedDictionary<string, List<string>> ItemNameGroups =
                    GameMainRoot.Persist.PlayerPrefsManager.ItemNameGroups;
                foreach (var itr in ItemNameGroups)
                {
                    string classifyName = itr.Key;
                    List<string> itemNames = itr.Value;
                    foreach (var itemName in itemNames)
                    {
                        // 旧存档迁移逻辑（已注释）
                    }
                }

                GameMainRoot.Persist.PlayerPrefsManager.Save();
            }

            // 刷新本地版本号记录
            RefreshVersionRecorders();

            // 注册 GDPR 隐私政策完成回调
            GameMainRoot.Event.Subscribe(GameEventCmd.GDPROver, this, OnGDPRStateChanged);
            
            // 非 WebGL 平台直接初始化启动
            InitLaunch(ownerMachine);
        }

        /// <summary>
        /// 启动流程更新：等待进入完成
        /// </summary>
        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            if (!m_EnterOver) return;
            base.OnUpdate(ownerMachine);
        }

        /// <summary>
        /// 离开启动流程：注销所有事件
        /// </summary>
        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            base.OnLeave(ownerMachine, isShutdown);

            // 注销 GDPR 事件
            GameMainRoot.Event.Unsubscribe(GameEventCmd.GDPROver, this, OnGDPRStateChanged);
        }

        #endregion

        //=========================================================================
        #region 核心初始化逻辑
        //=========================================================================

        /// <summary>
        /// 初始化启动核心逻辑（全平台通用）
        /// 性能设置 → 资源清单 → 配置表 → 多语言 → 字体 → 跳转预加载
        /// </summary>
        private void InitLaunch(StateMachine<ProcedureComponent> ownerMachine)
        {
            // 根据设备性能自动设置画质
            if (GameMainRoot.Launcher.UseDevicePerformance)
            {
                DevicePerformance.ModifyQualitySettingsBasedOnPerformanceLevel();
            }

            // 加载 AB 清单
            GameMainRoot.Asset.LoadManifest();

            // 加载全局配置表
            GameMainRoot.Config.LoadConfigs();

            // 加载支持的语言列表
            GameMainRoot.Localization.LoadDefaultLanguages();

            // 初始化当前语言
            GameMainRoot.Localization.InitCurLanguage();

            // 加载默认语言数据
            GameMainRoot.Localization.LoadDefaultDatas();

            // 加载字体配置
            GameMainRoot.Localization.LoadFontDatas();

            // 设置语言并刷新
            GameMainRoot.Localization.SetLanguage(GameMainRoot.Localization.Language, true);

            // 刷新 UI 适配比例
            GameMainRoot.UI.RefreshScreenMatchValue();

            // 调用基类进入逻辑
            base.OnEnter(ownerMachine);

            // 启动流程完成 → 跳转到预加载流程
            PrepareToNextProcedure(typeof(ProcedurePreload));
        }

        #endregion

        //=========================================================================
        #region 事件回调
        //=========================================================================

        /// <summary>
        /// GDPR 完成回调：隐私政策确认后进入游戏
        /// </summary>
        private void OnGDPRStateChanged(object sender = null, object userData = null, EventParams e = null)
        {
            if (userData != this) return;
            
            // 进入预加载流程
            PrepareToNextProcedure(typeof(ProcedurePreload));
        }

        /// <summary>
        /// WebGL 跳过下载回调
        /// </summary>
        private void OnWebGLSkipEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            InitLaunch(m_OwnerMachine);
        }

        #endregion

        //=========================================================================
        #region 版本管理
        //=========================================================================

        /// <summary>
        /// 刷新本地版本记录：包体版本变化时自动更新存档版本
        /// </summary>
        private void RefreshVersionRecorders()
        {
            string versionStringInSave = GameMainRoot.Persist.GetString(
                GameConstants.Persist.Common.WayType,
                GameConstants.Persist.Common.ClassifyName,
                GameConstants.Persist.Common.ItemKey.Version,
                GameConstants.MinGameVersion);

            string versionStringInPackage = Application.version;

            if (versionStringInSave != versionStringInPackage)
            {
                GameMainRoot.Persist.SetString(
                    GameConstants.Persist.Common.WayType,
                    GameConstants.Persist.Common.ClassifyName,
                    GameConstants.Persist.Common.ItemKey.Version,
                    versionStringInPackage);

                GameMainRoot.Persist.Save(
                    GameConstants.Persist.Common.WayType,
                    GameConstants.Persist.Common.ClassifyName);
            }
        }

        #endregion
    }
}