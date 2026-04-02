using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public class ProcedureLaunch : ProcedureState
    {
        /// <summary>
        /// WebGL热更新UI界面组件
        /// </summary>
        private UILauncherLoadingView _mUILauncherLoadingView;

        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);

            m_Name = "ProcedureLaunch";

        }
        
        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            RemoveAllContentsOnProcedureTransition = true;

            // PlayPrefs存档版本转换
            string versionStringInSave = GameMainRoot.Persist.GetString(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Version, "1.3.0");
            if(Version.Parse(versionStringInSave) < Version.Parse("1.3.0"))
            {
                SortedDictionary<string, List<string>> ItemNameGroups = GameMainRoot.Persist.PlayerPrefsManager.ItemNameGroups;
                foreach (var itr in ItemNameGroups)
                {
                    string classifyName = itr.Key;
                    List<string> itemNames = itr.Value;
                    foreach (var itemName in itemNames)
                    {
                        //string content = GameMainRoot.Persist.PlayerPrefsManager.GetString_old(classifyName, itemName);
                        //GameMainRoot.Persist.PlayerPrefsManager.SetString(classifyName, itemName, content);
                    }
                }
                GameMainRoot.Persist.PlayerPrefsManager.Save();
            }

            // 刷新本地版本号记录
            RefreshVersionRecorders();

            // 注册GDPR状态变化监听回调
            GameMainRoot.Event.Subscribe(GameEventCmd.GDPROver, this, OnGDPRStateChanged);

#if UNITY_WEBGL

            // 注册事件监听-WebGL下载跳过
            Root.Event.Subscribe(EventCmd.HotfixSkip, this, OnWebGLSkipEventCallback);

            // 注册事件监听-WebGL下载准备
            Root.Event.Subscribe(EventCmd.HotfixReady, this, OnWebGLReadyEventCallback);

            // 注册事件监听-流程许可
            Root.Event.Subscribe(EventCmd.FlowPermit, this, OnProcedureTransitionEventCallback);

            // 检查WebGL下载
            Root.Hotfix.CheckWebGLLauncher();
#else
            // 初始化启动配置
            InitLaunch(ownerMachine);
#endif

        }

        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            if (!m_EnterOver) return;
            base.OnUpdate(ownerMachine);
        }

        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            base.OnLeave(ownerMachine, isShutdown);

            // 注销GDPR状态变化监听回调
            GameMainRoot.Event.Unsubscribe(GameEventCmd.GDPROver, this, OnGDPRStateChanged);

#if UNITY_WEBGL

            // 注销事件监听-WebGL下载跳过
            Root.Event.Unsubscribe(EventCmd.HotfixSkip, this, OnWebGLSkipEventCallback);

            // 注销事件监听-WebGL下载准备
            Root.Event.Unsubscribe(EventCmd.HotfixReady, this, OnWebGLReadyEventCallback);

            // 注销事件监听-流程许可
            Root.Event.Unsubscribe(EventCmd.FlowPermit, this, OnProcedureTransitionEventCallback);

            // 隐藏Loading显示对象
            if (m_UILoadingBehaviour != null)
            {
                Root.UI.HideLoading(m_UILoadingBehaviour);
                m_UILoadingBehaviour = null;
            }
#endif
        }

        /// <summary>
        /// 初始化启动配置
        /// </summary>
        private void InitLaunch(StateMachine<ProcedureComponent> ownerMachine)
        {
            // 根据机型硬件性能评级自动设置项目质量参数
            if (GameMainRoot.Launcher.UseDevicePerformance)
            {
                DevicePerformance.ModifyQualitySettingsBasedOnPerformanceLevel();
            }

            // 加载Manifest
            GameMainRoot.Asset.LoadManifest();

            // 加载全局配置
            GameMainRoot.Config.LoadConfigs();

            // 加载默认支持语种类型集合
            GameMainRoot.Localization.LoadDefaultLanguages();

            // 初始化当前语言类型
            GameMainRoot.Localization.InitCurLanguage();

            // 加载当前默认本地化数据
            GameMainRoot.Localization.LoadDefaultDatas();

            // 加载字库本地化配置数据
            GameMainRoot.Localization.LoadFontDatas();

            // 设置启动时语言类型
            GameMainRoot.Localization.SetLanguage(GameMainRoot.Localization.Language, true);

            // 刷新屏幕宽高适比例配阀值
            GameMainRoot.UI.RefreshScreenMatchValue();
            
            // 基类流程
            base.OnEnter(ownerMachine);
            
            PrepareToNextProcedure(typeof(ProcedurePreload));
        }

        /// <summary>
        /// GDPR状态改变回调
        /// </summary>
        private void OnGDPRStateChanged(object sender = null, object userData = null, EventParams e = null)
        {
            if (userData != this) return;
#if UNITY_WEBGL
            // WebGL没有热更新，默认直接跳过Hotfix
            Root.Hotfix.OpenVersionCheck = false;
            // 切换到preload（随后会销毁所有游戏内容）
            PrepareToNextProcedure(typeof(ProcedurePreload));
#else
            // 切换到preload（随后会销毁所有游戏内容）
            PrepareToNextProcedure(typeof(ProcedurePreload));
#endif
        }

        /// <summary>
        /// 事件监听回调-WebGL下载跳过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebGLSkipEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;

            InitLaunch(m_OwnerMachine);
        }
        
        /// <summary>
        /// 事件监听回调-流程切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProcedureTransitionEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;

            InitLaunch(m_OwnerMachine);
        }

        /// <summary>
        /// 刷新本地版本号记录
        /// </summary>
        private void RefreshVersionRecorders()
        {
            // 更新存档中记录的版本号为当前包体中的版本号
            string versionStringInSave = GameMainRoot.Persist.GetString(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Version, GameConstants.MinGameVersion);
            string versionStringInPackage = Application.version;
            if (versionStringInSave != versionStringInPackage)
            {
                GameMainRoot.Persist.SetString(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Version, versionStringInPackage);
                GameMainRoot.Persist.Save(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName);
            }
        }

    }
}

