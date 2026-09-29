/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  VibrateManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   震动管理器 - 组合震动链式播放、类型映射分部实现
 ***************************************************************/
#if NICEVIBRATIONS_ENABLE
using DG.Tweening;
using Lofelt.NiceVibrations;
using System;
#endif
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 震动管理器（逻辑实现分部类）
    /// 功能：处理震动组合的链式播放、预设类型映射，基于 Nice Vibrations 插件实现
    /// </summary>
    public sealed partial class VibrateManager
    {
        /// <summary>
        /// 递归播放【自定义连续震动组合】中的下一个片段
        /// 实现多段震动按顺序连续播放
        /// </summary>
        /// <param name="groupName">组合名称</param>
        /// <param name="clipIndex">当前播放的片段索引</param>
        private void PlayCustomChain(string groupName, int clipIndex)
        {
            List<VibrateInfo> clips = m_CustomClipLibrary[groupName];

            // 索引越界则整条链结束
            if (clipIndex >= clips.Count)
            {
                return;
            }

            VibrateInfo clip = clips[clipIndex];
            // 播放当前片段，结束后自动衔接下一段
            PlayCustom(clip.Intensity, clip.Sharpness, clip.PreDuration, clip.Duration, () =>
            {
                PlayCustomChain(groupName, clipIndex + 1);
            });
        }

        /// <summary>
        /// 递归播放【点震动/短促震动组合】中的下一个片段
        /// </summary>
        /// <param name="groupName">组合名称</param>
        /// <param name="clipIndex">当前播放的片段索引</param>
        private void PlayEmphasisChain(string groupName, int clipIndex)
        {
            List<VibrateInfo> clips = m_EmphasisClipLibrary[groupName];

            if (clipIndex >= clips.Count)
            {
                return;
            }

            VibrateInfo clip = clips[clipIndex];
            PlayEmphasis(clip.Intensity, clip.Sharpness, clip.PreDuration, clip.Duration, () =>
            {
                PlayEmphasisChain(groupName, clipIndex + 1);
            });
        }

#if NICEVIBRATIONS_ENABLE
        /// <summary>
        /// 将框架内部震动类型 映射为 Nice Vibrations 插件的预设类型
        /// </summary>
        /// <param name="type">内部震动类型枚举</param>
        /// <returns>插件对应的预设震动类型</returns>
        private HapticPatterns.PresetType ToPresetType(VibrateType type)
        {
            return type switch
            {
                VibrateType.Selection    => HapticPatterns.PresetType.Selection,
                VibrateType.Success      => HapticPatterns.PresetType.Success,
                VibrateType.Warning      => HapticPatterns.PresetType.Warning,
                VibrateType.Failure      => HapticPatterns.PresetType.Failure,
                VibrateType.LightImpact  => HapticPatterns.PresetType.LightImpact,
                VibrateType.MediumImpact => HapticPatterns.PresetType.MediumImpact,
                VibrateType.HeavyImpact  => HapticPatterns.PresetType.HeavyImpact,
                VibrateType.RigidImpact  => HapticPatterns.PresetType.RigidImpact,
                VibrateType.SoftImpact   => HapticPatterns.PresetType.SoftImpact,
                _                        => HapticPatterns.PresetType.None,
            };
        }
#endif
    }
}
