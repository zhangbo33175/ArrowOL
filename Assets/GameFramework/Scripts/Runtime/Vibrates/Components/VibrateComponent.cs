/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  VibrateComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   震动管理组件 - 设备震动控制、Lua 调用、参数校验
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 震动管理组件（游戏框架核心组件）
    /// 功能：提供设备震动的外部调用接口，支持Lua调用、参数校验、类型震动/自定义震动
    /// 归属：GameFramework -> 输入反馈模块
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class VibrateComponent : GameComponent
    {
        /// <summary>
        /// 组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 创建震动管理器实例
            m_HapticService = new VibrateManager();
            if (m_HapticService == null)
            {
                Log.Fatal("Vibrate manager 无效。");
                return;
            }
        }

        /// <summary>
        /// 启动逻辑（暂未使用）
        /// </summary>
        private void Start()
        {
            // 初始化逻辑
        }

        /// <summary>
        /// 销毁时清理资源（暂未使用）
        /// </summary>
        private void OnDestroy()
        {
            // 资源清理
        }

        /// <summary>
        /// 播放系统默认简单震动
        /// </summary>
        public void Play()
        {
            m_HapticService.Play();
        }

        /// <summary>
        /// 播放预设类型的震动
        /// </summary>
        /// <param name="type">震动类型枚举</param>
        public void Play(VibrateType type)
        {
            m_HapticService.Play(type);
        }

        /// <summary>
        /// 播放自定义连续震动
        /// </summary>
        /// <param name="intensity">强度 0~1</param>
        /// <param name="sharpness">触感尖锐度 0~1</param>
        /// <param name="preDuration">前置延迟</param>
        /// <param name="duration">持续时间</param>
        public void PlayCustom(float intensity, float sharpness, float preDuration = 0f, float duration = 0f)
        {
            // 参数合法性校验
            if (!CheckRatio(intensity, nameof(intensity)))
            {
                return;
            }
            if (!CheckRatio(sharpness, nameof(sharpness)))
            {
                return;
            }
            if (!CheckNonNegative(preDuration, nameof(preDuration)))
            {
                return;
            }
            if (!CheckNonNegative(duration, nameof(duration)))
            {
                return;
            }

            m_HapticService.PlayCustom(intensity, sharpness, preDuration, duration, null);
        }

        /// <summary>
        /// 播放一组自定义连续震动（Lua配置表传入）
        /// </summary>
        /// <param name="luaTable">Lua配置表</param>
        public void PlayCustomGroup(LuaTable luaTable)
        {
            if (!CheckLuaTable(luaTable, nameof(PlayCustomGroup)))
            {
                return;
            }

            m_HapticService.PlayCustomGroup(luaTable);
        }

        /// <summary>
        /// 播放点震动能效（短促、间隔式震动）
        /// </summary>
        /// <param name="amplitude">振幅</param>
        /// <param name="frequency">频率</param>
        /// <param name="preDuration">延迟</param>
        /// <param name="interval">间隔</param>
        public void PlayEmphasis(float amplitude, float frequency, float preDuration = 0f, float interval = 0f)
        {
            if (!CheckRatio(amplitude, nameof(amplitude)))
            {
                return;
            }
            if (!CheckRatio(frequency, nameof(frequency)))
            {
                return;
            }
            if (!CheckNonNegative(preDuration, nameof(preDuration)))
            {
                return;
            }
            if (!CheckNonNegative(interval, nameof(interval)))
            {
                return;
            }

            m_HapticService.PlayEmphasis(amplitude, frequency, preDuration, interval, null);
        }

        /// <summary>
        /// 播放一组点震动（Lua配置表）
        /// </summary>
        public void PlayEmphasisGroup(LuaTable luaTable)
        {
            if (!CheckLuaTable(luaTable, nameof(PlayEmphasisGroup)))
            {
                return;
            }

            m_HapticService.PlayEmphasisGroup(luaTable);
        }

        /// <summary>
        /// 停止所有正在播放的震动
        /// </summary>
        public void StopAll()
        {
            m_HapticService.StopAll();
        }

        /// <summary>
        /// 设置震动总开关
        /// </summary>
        public void SetEnable(bool enable)
        {
            m_HapticService.SetEnable(enable);
        }

        /// <summary>
        /// 获取震动开关状态
        /// </summary>
        public bool GetEnable()
        {
            return m_HapticService.GetEnable();
        }

        /// <summary>
        /// 当前设备是否支持震动
        /// </summary>
        public bool IsSupported()
        {
            return m_HapticService.IsSupported();
        }

        /// <summary>
        /// 校验取值是否落在 [0,1] 区间，越界则记录错误并返回 false
        /// </summary>
        /// <param name="value">待校验数值</param>
        /// <param name="paramName">参数名（用于错误日志）</param>
        private static bool CheckRatio(float value, string paramName)
        {
            if (value < 0f || value > 1f)
            {
                Log.Error($"VibrateComponent.{paramName} 须在 0~1 之间，当前 {value}。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验取值是否为非负数，为负则记录错误并返回 false
        /// </summary>
        /// <param name="value">待校验数值</param>
        /// <param name="paramName">参数名（用于错误日志）</param>
        private static bool CheckNonNegative(float value, string paramName)
        {
            if (value < 0f)
            {
                Log.Error($"VibrateComponent.{paramName} 不能为负，当前 {value}。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验Lua配置表是否为空，为空则记录错误并返回 false
        /// </summary>
        /// <param name="luaTable">待校验Lua表</param>
        /// <param name="methodName">调用方法名（用于错误日志）</param>
        private static bool CheckLuaTable(LuaTable luaTable, string methodName)
        {
            if (luaTable == null)
            {
                Log.Error($"VibrateComponent.{methodName} luaTable 无效。");
                return false;
            }
            return true;
        }
    }
}
