/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorCoreManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   核心模块门面管理器 - HonorCore 分类统一入口（主文件）
 *            统一管理核心框架纯代码组件，外部核心调用统一走本管理器；
 *            UI 相关 Mono 组件（Launcher/UI/Procedure/Scene/Debugger）由 HonorUIManager 门面持有
 *            组件引用与便捷状态见 HonorCoreManager.Visitors.cs；便捷业务方法见 HonorCoreManager.Methods.cs
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 核心模块门面管理器（HonorCore 分类统一入口）
    /// 功能：集中管理核心框架纯代码组件（配置/事件/Lua/音频/数据表/计时器/震动/多语言/持久化/玩法等）；
    ///       UI 相关 Mono 组件（Launcher/UI/Procedure/Scene/Debugger）已迁出至 HonorUIManager 门面持有
    /// 外部调用：HonorCoreManager.Instance.xxx，或 .Config / .Sound 等组件属性访问完整能力
    /// 说明：采用纯代码单例，首次访问 Instance 自动调用 Init()
    /// </summary>
    public sealed partial class HonorCoreManager : Singleton<HonorCoreManager>
    {
        //=========================================================================

        #region 生命周期

        //=========================================================================

        /// <summary>
        /// 初始化：两阶段创建并初始化全部纯代码组件
        /// <para>阶段1 先创建全部纯代码组件实例，保证组件 Init 时可经门面互相访问；
        /// 阶段2 逐个调用组件 Init。
        /// UI 相关 Mono 组件（Launcher/UI/Procedure/Scene/Debugger）已迁出，
        /// 由 HonorUIManager 门面经 GameComponentsGroup 获取持有。</para>
        /// </summary>
        public override void Init()
        {
            // 阶段1：创建全部纯代码组件实例（先创建，保证组件 Init 时可经门面互相访问）
            Config = new ConfigComponent();
            Event = new EventComponent();
            Lua = new LuaComponent();
            Sound = new SoundComponent();
            Table = new TableComponent();
            Timer = new TimerComponent();
            Vibrate = new VibrateComponent();
            Localization = new LocalizationComponent();
            Persist = new PersistComponent();
            Playing = new PlayingComponent();

            // 阶段2：初始化纯代码组件（Timer/Playing 无 Init，仅创建）
            Config.Init();
            Event.Init();
            Lua.Init();
            Sound.Init();
            Table.Init();
            Vibrate.Init();
            Localization.Init();
            Persist.Init();
        }

        /// <summary>
        /// 释放：释放纯代码组件资源并置空引用
        /// <para>UI 相关 Mono 组件由 HonorUIManager.Dispose 置空引用，本处仅处理纯代码组件。</para>
        /// </summary>
        public override void Dispose()
        {
            // 释放纯代码组件资源（各自内部清理；Timer/Playing 已实现 Dispose，统一 try-catch 防止单点崩溃）
            try { Config?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Event?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Lua?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Sound?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Table?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Timer?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Vibrate?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Localization?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Persist?.Dispose(); } catch (Exception e) { Log.Error(e); }
            try { Playing?.Dispose(); } catch (Exception e) { Log.Error(e); }

            Config = null;
            Event = null;
            Lua = null;
            Sound = null;
            Table = null;
            Timer = null;
            Vibrate = null;
            Localization = null;
            Persist = null;
            Playing = null;
        }

        /// <summary>
        /// 每帧驱动：推进需要每帧驱动的核心组件生命周期
        /// <para>驱动 Timer（计时器）、Event（事件队列）、Lua（Lua GC），
        /// 纯代码单例组件自身 Update 不会被 Unity 调用，由 GameMainRoot.Update 每帧调用本方法驱动</para>
        /// </summary>
        public void DriveUpdate()
        {
            if (Timer != null)
            {
                Timer.DriveUpdate();
            }

            if (Event != null)
            {
                Event.DriveUpdate();
            }

            if (Lua != null)
            {
                Lua.DriveUpdate();
            }
        }

        #endregion
    }
}
