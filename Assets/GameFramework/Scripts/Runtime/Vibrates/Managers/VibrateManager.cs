
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
    public sealed partial class VibrateManager
    {
        /// <summary>
        /// 初始化振动管理器的新实例
        /// </summary>
        public VibrateManager()
        {
            m_CustomVibratesGroup = new Dictionary<string, List<VibrateInfo>>();
            m_EmphasisVibratesGroup = new Dictionary<string, List<VibrateInfo>>();
        }

        /// <summary>
        /// 播放最简单的振动
        /// </summary>
        public void Play()
        {
#if NICEVIBRATIONS_ENABLE
            HapticPatterns.PlayConstant(1, 1, 3);
#endif
        }

        /// <summary>
        /// 播放不同类型的振动
        /// </summary>
        /// <param name="type">振动类型</param>
        public void Play(VibrateType type)
        {
#if NICEVIBRATIONS_ENABLE
            HapticPatterns.PlayPreset(GetHapticType(type));
#endif
        }

        /// <summary>
        /// 播放自定义振动
        /// </summary>
        /// <param name="intensity">强度（0-1）</param>
        /// <param name="sharpness">感知度（0-1）</param>
        /// <param name="preDuration">前奏空闲持续时间（>0）</param>
        /// <param name="duration">持续时间（>0）</param>
        public void PlayCustom(float intensity, float sharpness, float preDuration, float duration, Action overCallback)
        {
#if NICEVIBRATIONS_ENABLE
            if (GetEnable())
            {
                DOTween.Sequence().AppendInterval(preDuration).AppendCallback(() => {
                    HapticPatterns.PlayConstant(intensity, sharpness, duration);
                    DOTween.Sequence().AppendInterval(duration).AppendCallback(() => {
                        if (overCallback != null)
                        {
                            overCallback();
                        }
                    }).stringId = DOTweenTypes.CustomVibrateDuration;
                }).stringId = DOTweenTypes.CustomVibratePreDuration;
            }
#endif
        }

        /// <summary>
        /// 播放自定义振动组合
        /// </summary>
        /// <param name="luaTable">LuaTable</param>
        public void PlayCustomGroup(LuaTable luaTable)
        {
            luaTable.Get("Name", out string name);

            if (!m_CustomVibratesGroup.ContainsKey(name))
            {
                m_CustomVibratesGroup.Add(name, new List<VibrateInfo>());

                int index = 1;
                LuaTable vibrateLuabTable = null;
                luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);
                while (vibrateLuabTable != null)
                {
                    float intensity = 0f;
                    float sharpness = 0f;
                    float preDuration = 0f;
                    float duration = 0f;
                    int type = 0;

                    vibrateLuabTable.Get("Intensity", out intensity);
                    vibrateLuabTable.Get("Sharpness", out sharpness);
                    vibrateLuabTable.Get("PreDuration", out preDuration);
                    vibrateLuabTable.Get("Duration", out duration);
                    m_CustomVibratesGroup[name].Add(new VibrateInfo(intensity, sharpness, preDuration, duration));

                    index++;
                    vibrateLuabTable = null;
                    luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);
                }
            }
            // 开始播放振动组合
            StartCustomGroupItem(name, 0);
        }

        /// <summary>
        /// 播放点振动
        /// </summary>
        /// <param name="amplitude">振幅（0~1）</param>
        /// <param name="frequency">频率（0~1）</param>
        /// <param name="preDuration">前奏空闲持续时间（>=0）</param>
        /// <param name="interval">间隔时间（>=0）</param>
        public void PlayEmphasis(float amplitude, float frequency, float preDuration, float interval, Action overCallback)
        {
#if NICEVIBRATIONS_ENABLE
            if (GetEnable())
            {
                DOTween.Sequence().AppendInterval(preDuration).AppendCallback(() => {
                    HapticPatterns.PlayEmphasis(amplitude, frequency);
                    DOTween.Sequence().AppendInterval(interval).AppendCallback(() => {
                        if (overCallback != null)
                        {
                            overCallback();
                        }
                    }).stringId = DOTweenTypes.EmphasisVibrateDuration;
                }).stringId = DOTweenTypes.EmphasisVibratePreDuration;
            }
#endif
        }

        /// <summary>
        /// 播放点振动组合
        /// </summary>
        /// <param name="luaTable">LuaTable</param>
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
                    float amplitude = 0f;
                    float frequency = 0f;
                    float preDuration = 0f;
                    float interval = 0f;

                    vibrateLuabTable.Get("Amplitude", out amplitude);
                    vibrateLuabTable.Get("Frequency", out frequency);
                    vibrateLuabTable.Get("PreDuration", out preDuration);
                    vibrateLuabTable.Get("Interval", out interval);
                    m_EmphasisVibratesGroup[name].Add(new VibrateInfo(amplitude, frequency, preDuration, interval));

                    index++;
                    vibrateLuabTable = null;
                    luaTable.Get(AorTxt.Format("Vibrate{0}", index), out vibrateLuabTable);
                }
            }
            // 开始播放点振动组合
            StartEmphasisGroupItem(name, 0);
        }

        /// <summary>
        /// 结束所有振动
        /// </summary>
        public void StopAll()
        {
            DOTween.Kill(GameDOTweenTypes.CustomVibratePreDuration);
            DOTween.Kill(GameDOTweenTypes.CustomVibrateDuration);
            DOTween.Kill(GameDOTweenTypes.EmphasisVibratePreDuration);
            DOTween.Kill(GameDOTweenTypes.EmphasisVibrateDuration);
#if NICEVIBRATIONS_ENABLE
            HapticController.Stop();
#endif
        }

        /// <summary>
        /// 设置开关
        /// </summary>
        /// <param name="enable">开关</param>
        public void SetEnable(bool enable)
        {
            if(!enable)
            {
                DOTween.Kill(GameDOTweenTypes.CustomVibratePreDuration);
                DOTween.Kill(GameDOTweenTypes.CustomVibrateDuration);
                DOTween.Kill(GameDOTweenTypes.EmphasisVibratePreDuration);
                DOTween.Kill(GameDOTweenTypes.EmphasisVibrateDuration);
            }
#if NICEVIBRATIONS_ENABLE
            HapticController.hapticsEnabled = enable;
#endif
        }

        /// <summary>
        /// 获取开关
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
        /// 判断是否支持振动
        /// </summary>
        /// <returns></returns>
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


