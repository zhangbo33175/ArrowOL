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
    public sealed partial class VibrateManager
    {
        /// <summary>
        /// 开始播放自定义振动组合条目
        /// </summary>
        /// <param name="name">归属组合名称</param>
        /// <param name="index">条目index</param>
        private void StartCustomGroupItem(string name, int index)
        {
            if (index < m_CustomVibratesGroup[name].Count)
            {
                VibrateInfo info = m_CustomVibratesGroup[name][index];
                PlayCustom(info.Intensity, info.Sharpness, info.PreDuration, info.Duration, () =>
                {
                    StartCustomGroupItem(name, index + 1);
                });
            }
        }

        /// <summary>
        /// 开始播放点振动组合条目
        /// </summary>
        /// <param name="name">归属组合名称</param>
        /// <param name="index">条目index</param>
        private void StartEmphasisGroupItem(string name, int index)
        {
            if (index < m_EmphasisVibratesGroup[name].Count)
            {
                VibrateInfo info = m_EmphasisVibratesGroup[name][index];
                PlayEmphasis(info.Intensity, info.Sharpness, info.PreDuration, info.Duration, () =>
                {
                    StartEmphasisGroupItem(name, index + 1);
                });
            }
        }

#if NICEVIBRATIONS_ENABLE
        /// <summary>
        /// 获取插件级HapticType
        /// </summary>
        /// <param name="type">振动类型</param>
        /// <returns></returns>
        private HapticPatterns.PresetType GetHapticType(VibrateType type)
        {
            HapticPatterns.PresetType hapticType = HapticPatterns.PresetType.None;
            switch (type)
            {
                case VibrateType.Selection: hapticType = HapticPatterns.PresetType.Selection; break;
                case VibrateType.Success: hapticType = HapticPatterns.PresetType.Success; break;
                case VibrateType.Warning: hapticType = HapticPatterns.PresetType.Warning; break;
                case VibrateType.Failure: hapticType = HapticPatterns.PresetType.Failure; break;
                case VibrateType.LightImpact: hapticType = HapticPatterns.PresetType.LightImpact; break;
                case VibrateType.MediumImpact: hapticType = HapticPatterns.PresetType.MediumImpact; break;
                case VibrateType.HeavyImpact: hapticType = HapticPatterns.PresetType.HeavyImpact; break;
                case VibrateType.RigidImpact: hapticType = HapticPatterns.PresetType.RigidImpact; break;
                case VibrateType.SoftImpact: hapticType = HapticPatterns.PresetType.SoftImpact; break;
                default: hapticType = HapticPatterns.PresetType.None; break;
            }
            return hapticType;
        }
#endif
    }
}


