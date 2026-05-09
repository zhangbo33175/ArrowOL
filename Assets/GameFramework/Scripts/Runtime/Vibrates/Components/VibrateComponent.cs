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
            m_VibrateManager = new VibrateManager();
            if (m_VibrateManager == null)
            {
                Log.Fatal("Vibrate manager 无效。");
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
        /// 播放系统默认简单震动
        /// </summary>
        public void Play()
        {
            m_VibrateManager.Play();
        }

        /// <summary>
        /// 播放预设类型的震动
        /// </summary>
        /// <param name="type">震动类型枚举</param>
        public void Play(VibrateType type)
        {
            m_VibrateManager.Play(type);
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
            if (intensity < 0f || intensity > 1f)
            {
                Log.Error("VibrateComponent.PlayCustom intensity 无效。");
                return;
            }

            if (sharpness < 0f || sharpness > 1f)
            {
                Log.Error("VibrateComponent.PlayCustom sharpness 无效。");
                return;
            }

            if (preDuration < 0f)
            {
                Log.Error("VibrateComponent.PlayCustom preDuration 无效。");
                return;
            }

            if (duration < 0f)
            {
                Log.Error("VibrateComponent.PlayCustom duration 无效。");
                return;
            }

            m_VibrateManager.PlayCustom(intensity, sharpness, preDuration, duration, null);
        }

        /// <summary>
        /// 播放一组自定义连续震动（Lua配置表传入）
        /// </summary>
        /// <param name="luaTable">Lua配置表</param>
        public void PlayCustomGroup(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("VibrateComponent.PlayCustomGroup luaTable 无效。");
                return;
            }

            m_VibrateManager.PlayCustomGroup(luaTable);
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
            if (amplitude < 0f || amplitude > 1f)
            {
                Log.Error("VibrateComponent.PlayEmphasis amplitude 无效。");
                return;
            }

            if (frequency < 0f || frequency > 1f)
            {
                Log.Error("VibrateComponent.PlayEmphasis frequency 无效。");
                return;
            }

            if (preDuration < 0f)
            {
                Log.Error("VibrateComponent.PlayEmphasis preDuration 无效。");
                return;
            }

            if (interval < 0f)
            {
                Log.Error("VibrateComponent.PlayEmphasis interval 无效。");
                return;
            }

            m_VibrateManager.PlayEmphasis(amplitude, frequency, preDuration, interval, null);
        }

        /// <summary>
        /// 播放一组点震动（Lua配置表）
        /// </summary>
        public void PlayEmphasisGroup(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("VibrateComponent.PlayEmphasisGroup luaTable 无效。");
                return;
            }

            m_VibrateManager.PlayEmphasisGroup(luaTable);
        }

        /// <summary>
        /// 停止所有正在播放的震动
        /// </summary>
        public void StopAll()
        {
            m_VibrateManager.StopAll();
        }

        /// <summary
        /// 设置震动总开关
        /// </summary>
        public void SetEnable(bool enable)
        {
            m_VibrateManager.SetEnable(enable);
        }

        /// <summary>
        /// 获取震动开关状态
        /// </summary>
        public bool GetEnable()
        {
            return m_VibrateManager.GetEnable();
        }

        /// <summary>
        /// 当前设备是否支持震动
        /// </summary>
        public bool IsSupported()
        {
            return m_VibrateManager.IsSupported();
        }
    }
}