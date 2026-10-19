/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  VibrateManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   震动管理器 - 基于 Nice Vibrations，支持预设/自定义/组合/Lua震动
 ***************************************************************/
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
        /// 构造函数：初始化两类震动组合的缓存字典
        /// </summary>
        public VibrateManager()
        {
            m_CustomClipLibrary   = new Dictionary<string, List<VibrateInfo>>();
            m_EmphasisClipLibrary = new Dictionary<string, List<VibrateInfo>>();
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
            HapticPatterns.PlayPreset(ToPresetType(type));
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
                    }).stringId = GameDOTweenTypes.CustomVibrateSustainTween;
                }).stringId = GameDOTweenTypes.CustomVibrateDelayTween;
            }
#endif
        }

        /// <summary>
        /// 播放自定义震动组合（从Lua配置表读取）
        /// 支持多段震动按顺序自动播放，首次播放后按组合名缓存
        /// </summary>
        /// <param name="luaTable">Lua配置表</param>
        public void PlayCustomGroup(LuaTable luaTable)
        {
            luaTable.Get("Name", out string groupName);

            if (!m_CustomClipLibrary.TryGetValue(groupName, out List<VibrateInfo> customClips))
            {
                customClips = LoadClips(luaTable, ReadCustomClip);
                m_CustomClipLibrary.Add(groupName, customClips);
            }

            PlayCustomChain(groupName, 0);
        }

        /// <summary>
        /// 播放点震动（短促、冲击型震动，适合点击/打击反馈）
        /// </summary>
        public void PlayEmphasis(float amplitude, float frequency, float preDuration, float interval, Action overCallback)
        {
#if NICEVIBRATIONS_ENABLE
            if (GetEnable())
            {
                DOTween.Sequence().AppendInterval(preDuration).AppendCallback(() => {
                    HapticPatterns.PlayEmphasis(amplitude, frequency);
                    DOTween.Sequence().AppendInterval(interval).AppendCallback(() => {
                        overCallback?.Invoke();
                    }).stringId = GameDOTweenTypes.EmphasisVibrateSustainTween;
                }).stringId = GameDOTweenTypes.EmphasisVibrateDelayTween;
            }
#endif
        }

        /// <summary>
        /// 播放点震动组合（Lua配置表驱动）
        /// </summary>
        public void PlayEmphasisGroup(LuaTable luaTable)
        {
            luaTable.Get("Name", out string groupName);

            if (!m_EmphasisClipLibrary.TryGetValue(groupName, out List<VibrateInfo> emphasisClips))
            {
                emphasisClips = LoadClips(luaTable, ReadEmphasisClip);
                m_EmphasisClipLibrary.Add(groupName, emphasisClips);
            }

            PlayEmphasisChain(groupName, 0);
        }

        /// <summary>
        /// 从Lua配置表顺序读取所有震动片段，通用解析逻辑（自定义/点震动共用）
        /// </summary>
        /// <param name="groupTable">组合根表</param>
        /// <param name="clipReader">单条片段的字段读取委托</param>
        /// <returns>解析得到的片段列表</returns>
        private static List<VibrateInfo> LoadClips(LuaTable groupTable, Func<LuaTable, VibrateInfo> clipReader)
        {
            var clips = new List<VibrateInfo>();
            int clipSeq = 1;
            LuaTable clipTable = null;
            groupTable.Get(AorTxt.Format("Vibrate{0}", clipSeq), out clipTable);

            while (clipTable != null)
            {
                clips.Add(clipReader(clipTable));
                clipSeq++;
                clipTable = null;
                groupTable.Get(AorTxt.Format("Vibrate{0}", clipSeq), out clipTable);
            }

            return clips;
        }

        /// <summary>
        /// 读取单条自定义连续震动片段
        /// </summary>
        private static VibrateInfo ReadCustomClip(LuaTable clipTable)
        {
            clipTable.Get("Intensity",   out float intensity);
            clipTable.Get("Sharpness",   out float sharpness);
            clipTable.Get("PreDuration", out float preDuration);
            clipTable.Get("Duration",    out float duration);
            return new VibrateInfo(intensity, sharpness, preDuration, duration);
        }

        /// <summary>
        /// 读取单条点震动片段
        /// </summary>
        private static VibrateInfo ReadEmphasisClip(LuaTable clipTable)
        {
            clipTable.Get("Amplitude",   out float amplitude);
            clipTable.Get("Frequency",   out float frequency);
            clipTable.Get("PreDuration", out float preDuration);
            clipTable.Get("Interval",    out float interval);
            return new VibrateInfo(amplitude, frequency, preDuration, interval);
        }

        /// <summary>
        /// 停止所有震动（包括DOTween延迟+NV震动）
        /// </summary>
        public void StopAll()
        {
            // 停止所有延迟/计时动画
            DOTween.Kill(GameDOTweenTypes.CustomVibrateDelayTween);
            DOTween.Kill(GameDOTweenTypes.CustomVibrateSustainTween);
            DOTween.Kill(GameDOTweenTypes.EmphasisVibrateDelayTween);
            DOTween.Kill(GameDOTweenTypes.EmphasisVibrateSustainTween);

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
