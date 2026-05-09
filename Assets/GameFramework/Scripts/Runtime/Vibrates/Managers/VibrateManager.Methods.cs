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
        /// <param name="name">组合名称</param>
        /// <param name="index">当前播放的片段索引</param>
        private void StartCustomGroupItem(string name, int index)
        {
            // 如果索引未超出列表范围，继续播放
            if (index < m_CustomVibratesGroup[name].Count)
            {
                VibrateInfo info = m_CustomVibratesGroup[name][index];
                // 播放当前片段，结束后自动播放下一个（递归）
                PlayCustom(info.Intensity, info.Sharpness, info.PreDuration, info.Duration, () =>
                {
                    StartCustomGroupItem(name, index + 1);
                });
            }
        }

        /// <summary>
        /// 递归播放【点震动/短促震动组合】中的下一个片段
        /// </summary>
        /// <param name="name">组合名称</param>
        /// <param name="index">当前播放的片段索引</param>
        private void StartEmphasisGroupItem(string name, int index)
        {
            // 如果索引未超出列表范围，继续播放
            if (index < m_EmphasisVibratesGroup[name].Count)
            {
                VibrateInfo info = m_EmphasisVibratesGroup[name][index];
                // 播放当前片段，结束后自动播放下一个（递归）
                PlayEmphasis(info.Intensity, info.Sharpness, info.PreDuration, info.Duration, () =>
                {
                    StartEmphasisGroupItem(name, index + 1);
                });
            }
        }

#if NICEVIBRATIONS_ENABLE
        /// <summary>
        /// 将框架内部震动类型 映射为 Nice Vibrations 插件的预设类型
        /// </summary>
        /// <param name="type">内部震动类型枚举</param>
        /// <returns>插件对应的预设震动类型</returns>
        private HapticPatterns.PresetType GetHapticType(VibrateType type)
        {
            HapticPatterns.PresetType hapticType = HapticPatterns.PresetType.None;
            switch (type)
            {
                case VibrateType.Selection:        hapticType = HapticPatterns.PresetType.Selection; break;
                case VibrateType.Success:          hapticType = HapticPatterns.PresetType.Success; break;
                case VibrateType.Warning:          hapticType = HapticPatterns.PresetType.Warning; break;
                case VibrateType.Failure:          hapticType = HapticPatterns.PresetType.Failure; break;
                case VibrateType.LightImpact:      hapticType = HapticPatterns.PresetType.LightImpact; break;
                case VibrateType.MediumImpact:     hapticType = HapticPatterns.PresetType.MediumImpact; break;
                case VibrateType.HeavyImpact:      hapticType = HapticPatterns.PresetType.HeavyImpact; break;
                case VibrateType.RigidImpact:      hapticType = HapticPatterns.PresetType.RigidImpact; break;
                case VibrateType.SoftImpact:       hapticType = HapticPatterns.PresetType.SoftImpact; break;
                default:                           hapticType = HapticPatterns.PresetType.None; break;
            }
            return hapticType;
        }
#endif
    }
}