using DG.Tweening;
#if NICEVIBRATIONS_ENABLE
using Lofelt.NiceVibrations;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 震动管理器（核心逻辑实现）
    /// 基于 Nice Vibrations 插件，提供：预设震动、自定义连续震动、点震动、组合震动、全局开关控制
    /// 支持 Lua 配置表驱动，适合游戏技能/UI/打击震动反馈
    /// </summary>
    public sealed partial class VibrateManager
    {
        /// <summary>
        /// 构造函数：初始化震动组合字典
        /// </summary>
        public VibrateManager()
        {
            m_CustomVibratesGroup = new Dictionary<string, List<VibrateInfo>>();
            m_EmphasisVibratesGroup = new Dictionary<string, List<VibrateInfo>>();
        }

        /// <summary>
        /// 播放默认简单震动（强震动3秒）
        /// </summary>
        public void Play()
        {
#if NICEVIBRATIONS_ENABLE
            HapticPatterns.PlayConstant(1, 1, 3);
#endif
        }

        /// <summary>
        /// 播放内置预设类型震动（成功/失败/重击/轻击等）
        /// </summary>
        /// <param name="type">震动类型</param>
        public void Play(VibrateType type)
        {
#if NICEVIBRATIONS_ENABLE
            HapticPatterns.PlayPreset(GetHapticType(type));
#endif
        }

        /// <summary>
        /// 播放自定义连续震动
        /// </summary>
        /// <param name="intensity">强度</param>
        /// <param name="sharpness">尖锐度</param>
        /// <param name="preDuration">延迟时间</param>
        /// <param name="duration">持续时间</param>
        /// <param name="overCallback">结束回调</param>
        public void PlayCustom(float intensity, float sharpness, float preDuration, float duration, Action overCallback)
        {
#if NICEVIBRATIONS_ENABLE
            if (GetEnable())
            {
                // 延迟后播放震动
                DOTween.Sequence().AppendInterval(preDuration).AppendCallback(() => {
                    HapticPatterns.PlayConstant(intensity, sharpness, duration);
                    // 震动结束后触发回调
                    DOTween.Sequence().AppendInterval(duration).AppendCallback(() => {
                        overCallback?.Invoke();
                    }).stringId = DOTweenTypes.CustomVibrateDuration;
                }).stringId = DOTweenTypes.CustomVibratePreDuration;
            }
#endif
        }

        /// <summary>
        /// 播放自定义震动组合（从Lua配置表读取）
        /// 支持多段震动按顺序自动播放
        /// </summary>
        /// <param name="luaTable">Lua配置表</param>
        public void PlayCustomGroup(LuaTable luaTable)
        {
            luaTable.Get("Name", out string name);

            // 第一次播放时缓存震动组合
            if (!m_CustomVibratesGroup.ContainsKey(name))
            {
                m_CustomVibratesGroup.Add(name, new List<VibrateInfo>());

                int index = 1;
                LuaTable vibrateLuabTable = null;
                luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);

                // 循环读取所有震动片段
                while (vibrateLuabTable != null)
                {
                    vibrateLuabTable.Get("Intensity", out float intensity);
                    vibrateLuabTable.Get("Sharpness", out float sharpness);
                    vibrateLuabTable.Get("PreDuration", out float preDuration);
                    vibrateLuabTable.Get("Duration", out float duration);

                    m_CustomVibratesGroup[name].Add(new VibrateInfo(intensity, sharpness, preDuration, duration));

                    index++;
                    vibrateLuabTable = null;
                    luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);
                }
            }

            // 开始顺序播放组合
            StartCustomGroupItem(name, 0);
        }

        /// <summary>
        /// 播放点震动（短促、冲击型震动，适合点击/打击反馈）
        /// </summary>
        public void PlayEmphasis(float amplitude, float frequency, float preDuration, float interval,
            Action overCallback)
        {
#if NICEVIBRATIONS_ENABLE
            if (GetEnable())
            {
                DOTween.Sequence().AppendInterval(preDuration).AppendCallback(() => {
                    HapticPatterns.PlayEmphasis(amplitude, frequency);
                    DOTween.Sequence().AppendInterval(interval).AppendCallback(() => {
                        overCallback?.Invoke();
                    }).stringId = DOTweenTypes.EmphasisVibrateDuration;
                }).stringId = DOTweenTypes.EmphasisVibratePreDuration;
            }
#endif
        }

        /// <summary>
        /// 播放点震动组合（Lua配置表驱动）
        /// </summary>
        public void PlayEmphasisGroup(LuaTable luaTable)
        {
            luaTable.Get("Name", out string name);

            if (!m_EmphasisVibratesGroup.ContainsKey(name))
            {
                m_EmphasisVibratesGroup.Add(name, new List<VibrateInfo>());

                int index = 1;
                LuaTable vibrateLuabTable = null;
                luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);

                while (vibrateLuabTable != null)
                {
                    vibrateLuabTable.Get("Amplitude", out float amplitude);
                    vibrateLuabTable.Get("Frequency", out float frequency);
                    vibrateLuabTable.Get("PreDuration", out float preDuration);
                    vibrateLuabTable.Get("Interval", out float interval);

                    m_EmphasisVibratesGroup[name].Add(new VibrateInfo(amplitude, frequency, preDuration, interval));

                    index++;
                    vibrateLuabTable = null;
                    luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);
                }
            }

            // 开始播放点震动组合
            StartEmphasisGroupItem(name, 0);
        }

        /// <summary>
        /// 停止所有震动（包括DOTween延迟+NV震动）
        /// </summary>
        public void StopAll()
        {
            // 停止所有延迟/计时动画
            DOTween.Kill(GameDOTweenTypes.CustomVibratePreDuration);
            DOTween.Kill(GameDOTweenTypes.CustomVibrateDuration);
            DOTween.Kill(GameDOTweenTypes.EmphasisVibratePreDuration);
            DOTween.Kill(GameDOTweenTypes.EmphasisVibrateDuration);

#if NICEVIBRATIONS_ENABLE
            HapticController.Stop();
#endif
        }

        /// <summary>
        /// 设置全局震动开关
        /// </summary>
        public void SetEnable(bool enable)
        {
            if (!enable)
            {
                StopAll();
            }

#if NICEVIBRATIONS_ENABLE
            HapticController.hapticsEnabled = enable;
#endif
        }

        /// <summary>
        /// 获取震动开关状态
        /// </summary>
        public bool GetEnable()
        {
#if NICEVIBRATIONS_ENABLE
            return HapticController.hapticsEnabled;
#else
            return false;
#endif
        }

        /// <summary>
        /// 当前设备是否支持震动
        /// </summary>
        public bool IsSupported()
        {
#if NICEVIBRATIONS_ENABLE
            return DeviceCapabilities.isVersionSupported;
#else
            return false;
#endif
        }
    }
}