using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using XLua;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class VibrateComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

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
        /// 播放最简单的振动
        /// </summary>
        public void Play()
        {
            m_VibrateManager.Play();
        }

        /// <summary>
        /// 播放不同类型的振动
        /// </summary>
        /// <param name="type">振动类型</param>
        public void Play(VibrateType type)
        {
            m_VibrateManager.Play(type);
        }

        /// <summary>
        /// 播放自定义振动
        /// </summary>
        /// <param name="intensity">强度（0~1）</param>
        /// <param name="sharpness">感知度（0~1）</param>
        /// <param name="preDuration">前奏空闲持续时间（>=0）</param>
        /// <param name="duration">持续时间（>=0）</param>
        public void PlayCustom(float intensity, float sharpness, float preDuration = 0f, float duration = 0f)
        {
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
        /// 播放自定义振动组合
        /// </summary>
        /// <param name="luaTable">表格信息</param>
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
        /// 播放点振动
        /// </summary>
        /// <param name="amplitude">振幅（0~1）</param>
        /// <param name="frequency">频率（0~1）</param>
        /// <param name="preDuration">前奏空闲持续时间（>=0）</param>
        /// <param name="interval">间隔时间（>=0）</param>
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
        /// 播放点振动组合
        /// </summary>
        /// <param name="luaTable">表格信息</param>
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
        /// 终止所有振动
        /// </summary>
        public void StopAll()
        {
            m_VibrateManager.StopAll();
        }

        /// <summary>
        /// 设置开关
        /// </summary>
        /// <param name="enable">开关</param>
        public void SetEnable(bool enable)
        {
            m_VibrateManager.SetEnable(enable);
        }

        /// <summary>
        /// 获取开关
        /// </summary>
        public bool GetEnable()
        {
            return m_VibrateManager.GetEnable();
        }

        /// <summary>
        /// 判断是否支持振动
        /// </summary>
        /// <returns></returns>
        public bool IsSupported()
        {
            return m_VibrateManager.IsSupported();
        }

    }
}


