/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  VibrateManager.Define.cs
 * author:    云毅
 * created:   2026
 * descrip:   震动管理器 - 成员变量、属性定义分部类
 ***************************************************************/
using System;
using System.Collections.Generic;
#if NICEVIBRATIONS_ENABLE
using Lofelt.NiceVibrations;
#endif

namespace Honor.Runtime
{
    /// <summary>
    /// 震动管理器（变量分部类）
    /// 功能：基于 Nice Vibrations 插件的设备震动/触觉反馈核心管理
    /// 管理自定义震动组合、点震动组合、全局震动强度控制
    /// </summary>
    public sealed partial class VibrateManager
    {
        /// <summary>
        /// 自定义连续震动组合字典
        /// Key：组合名称 / Value：震动片段列表
        /// </summary>
        private readonly Dictionary<string, List<VibrateInfo>> m_CustomVibratesGroup;

        /// <summary>
        /// 短促点震动组合字典
        /// Key：组合名称 / Value：震动片段列表
        /// </summary>
        private readonly Dictionary<string, List<VibrateInfo>> m_EmphasisVibratesGroup;

#if NICEVIBRATIONS_ENABLE
        /// <summary>
        /// 全局触觉反馈输出强度（总音量）
        /// 默认 1，与 ClipLevel 相乘得到最终播放强度
        /// 范围：0 ~ 1
        /// </summary>
        public float OutputLevel
        {
            get => HapticController.outputLevel;
            set => HapticController.outputLevel = value;
        }

        /// <summary>
        /// 震动片段自身强度
        /// 默认 1，与 OutputLevel 相乘得到最终播放强度
        /// 范围：0 ~ 1
        /// </summary>
        public float ClipLevel
        {
            get => HapticController.clipLevel;
            set => HapticController.clipLevel = value;
        }
#endif
    }
}