/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DevicePerformance.cs
 * author:    云毅
 * created:   2026
 * descrip:   设备性能检测工具类 - 自动判断硬件等级、设置Unity画质参数、
 *            提供性能等级对应UI显示颜色
 ***************************************************************/
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 设备硬件性能等级枚举
    //=========================================================================
    /// <summary>
    /// 设备硬件性能等级
    /// </summary>
    public enum DevicePerformanceLevel
    {
        /// <summary>
        /// 低端设备
        /// </summary>
        Low,
        
        /// <summary>
        /// 中端设备
        /// </summary>
        Mid,
        
        /// <summary>
        /// 高端设备
        /// </summary>
        High
    }

    //=========================================================================
    // 画面质量等级枚举
    //=========================================================================
    /// <summary>
    /// 画面质量等级（与设备性能对应）
    /// </summary>
    public enum QualityLevel
    {
        /// <summary>
        /// 低画质
        /// </summary>
        Low,
        
        /// <summary>
        /// 中画质
        /// </summary>
        Mid,
        
        /// <summary>
        /// 高画质
        /// </summary>
        High
    }

    //=========================================================================
    // 设备性能检测与画质设置工具类
    //=========================================================================
    /// <summary>
    /// 设备性能检测 & 自动画质设置工具类
    /// 功能：
    /// 1. 根据 CPU核心数 / 显存 / 内存 自动判断设备性能等级
    /// 2. 根据性能等级自动设置 Unity 画质参数
    /// 3. 提供不同等级对应的颜色显示
    /// </summary>
    public static class DevicePerformance
    {
        #region 设备性能等级判断
        /// <summary>
        /// 获取设备硬件性能评级（核心判断逻辑）
        /// 判断依据：显卡类型 → CPU核心数 → 显存 + 内存大小
        /// </summary>
        /// <returns>设备性能等级 Low/Mid/High</returns>
        public static DevicePerformanceLevel GetDevicePerformanceLevel()
        {
            // 英特尔集显 → 直接判定为低端
            if (SystemInfo.graphicsDeviceVendorID == 32902)
            {
                return DevicePerformanceLevel.Low;
            }
            else // NVIDIA / AMD 独立显卡
            {
                // 第一步：按 CPU 核心数判断（不同平台阈值不同）
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (SystemInfo.processorCount <= GameMainRoot.Launcher.EditorPerformance.ProcessorCount)
#elif UNITY_STANDALONE_OSX || UNITY_IOS
                if (SystemInfo.processorCount < Root.Launcher.iOSPerformance.ProcessorCount)
#elif UNITY_ANDROID
                if (SystemInfo.processorCount <= Root.Launcher.AndroidPerformance.ProcessorCount)
#endif
                {
                    // CPU 核心数不足 → 低端机
                    return DevicePerformanceLevel.Low;
                }
                else
                {
                    // 第二步：使用 显存 + 内存 综合判断中/高端
                    int graphicsMemorySize = SystemInfo.graphicsMemorySize;
                    int systemMemorySize = SystemInfo.systemMemorySize;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (graphicsMemorySize >= GameMainRoot.Launcher.EditorPerformance.GraphicsMemorySizeHighBase &&
                        systemMemorySize >= GameMainRoot.Launcher.EditorPerformance.SystemMemorySizeHighBase)
                    {
                        return DevicePerformanceLevel.High;
                    }
                    else if (graphicsMemorySize >= GameMainRoot.Launcher.EditorPerformance.GraphicsMemorySizeMidBase &&
                             systemMemorySize >= GameMainRoot.Launcher.EditorPerformance.SystemMemorySizeMidBase)
                    {
                        return DevicePerformanceLevel.Mid;
                    }
                    else
                    {
                        return DevicePerformanceLevel.Low;
                    }
#elif UNITY_STANDALONE_OSX || UNITY_IOS
                    if (graphicsMemorySize >= Root.Launcher.iOSPerformance.GraphicsMemorySizeHighBase && systemMemorySize >= Root.Launcher.iOSPerformance.SystemMemorySizeHighBase)
                    {
                        return DevicePerformanceLevel.High;
                    }
                    else if (graphicsMemorySize >= Root.Launcher.iOSPerformance.GraphicsMemorySizeMidBase && systemMemorySize >= Root.Launcher.iOSPerformance.SystemMemorySizeMidBase)
                    {
                        return DevicePerformanceLevel.Mid;
                    }
                    else
                    {
                        return DevicePerformanceLevel.Low;
                    }
#elif UNITY_ANDROID
                    if (graphicsMemorySize >= Root.Launcher.AndroidPerformance.GraphicsMemorySizeHighBase && systemMemorySize >= Root.Launcher.AndroidPerformance.SystemMemorySizeHighBase)
                    {
                        return DevicePerformanceLevel.High;
                    }
                    else if (graphicsMemorySize >= Root.Launcher.AndroidPerformance.GraphicsMemorySizeMidBase && systemMemorySize >= Root.Launcher.AndroidPerformance.SystemMemorySizeMidBase)
                    {
                        return DevicePerformanceLevel.Mid;
                    }
                    else
                    {
                        return DevicePerformanceLevel.Low;
                    }
#endif
                }
            }
        }
        #endregion

        #region 画质等级设置
        /// <summary>
        /// 根据设备性能自动设置 Unity 质量等级（直接使用项目内置画质配置）
        /// </summary>
        /// <param name="lowQuality">Low 对应画质等级</param>
        /// <param name="midQuality">Mid 对应画质等级</param>
        /// <param name="highQuality">High 对应画质等级</param>
        public static void ModifyQualityLevelsBasedOnPerformanceLevel(int lowQuality, int midQuality, int highQuality)
        {
            DevicePerformanceLevel level = GetDevicePerformanceLevel();
            switch (level)
            {
                case DevicePerformanceLevel.Low:
                    QualitySettings.SetQualityLevel(lowQuality, true);
                    break;
                case DevicePerformanceLevel.Mid:
                    QualitySettings.SetQualityLevel(midQuality, true);
                    break;
                case DevicePerformanceLevel.High:
                    QualitySettings.SetQualityLevel(highQuality, true);
                    break;
            }
        }

        /// <summary>
        /// 根据设备性能自动设置详细画质参数（抗锯齿、阴影、灯光等）
        /// </summary>
        public static void ModifyQualitySettingsBasedOnPerformanceLevel()
        {
            DevicePerformanceLevel level = GetDevicePerformanceLevel();
            switch (level)
            {
                case DevicePerformanceLevel.Low:
                    SetQualitySettings(QualityLevel.Low);
                    break;
                case DevicePerformanceLevel.Mid:
                    SetQualitySettings(QualityLevel.Mid);
                    break;
                case DevicePerformanceLevel.High:
                    SetQualitySettings(QualityLevel.High);
                    break;
            }
        }

        /// <summary>
        /// 设置具体画质参数（可自由定制低/中/高方案）
        /// 包含：抗锯齿、阴影、灯光、纹理、同步等核心性能参数
        /// </summary>
        /// <param name="qualityLevel">画质等级</param>
        public static void SetQualitySettings(QualityLevel qualityLevel)
        {
            switch (qualityLevel)
            {
                case QualityLevel.Low:
                    // 极致性能模式：关闭阴影、抗锯齿、软粒子、反射
                    QualitySettings.pixelLightCount = 2;
                    QualitySettings.globalTextureMipmapLimit = 1;
                    QualitySettings.antiAliasing = 0;
                    QualitySettings.softParticles = false;
                    QualitySettings.realtimeReflectionProbes = false;
                    QualitySettings.billboardsFaceCameraPosition = false;
                    QualitySettings.shadows = ShadowQuality.Disable;
                    QualitySettings.vSyncCount = 0;
                    break;
                case QualityLevel.Mid:
                    // 均衡模式：开启基础阴影、2倍抗锯齿
                    QualitySettings.pixelLightCount = 4;
                    QualitySettings.antiAliasing = 2;
                    QualitySettings.softParticles = false;
                    QualitySettings.realtimeReflectionProbes = true;
                    QualitySettings.billboardsFaceCameraPosition = true;
                    QualitySettings.shadows = ShadowQuality.HardOnly;
                    QualitySettings.vSyncCount = 2;
                    break;
                case QualityLevel.High:
                    // 画质模式：全特效开启
                    QualitySettings.pixelLightCount = 4;
                    QualitySettings.antiAliasing = 8;
                    QualitySettings.softParticles = true;
                    QualitySettings.realtimeReflectionProbes = true;
                    QualitySettings.billboardsFaceCameraPosition = true;
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.vSyncCount = 2;
                    break;
            }
        }
        #endregion

        #region 性能等级颜色获取
        /// <summary>
        /// 获取性能等级对应的显示颜色（用于UI展示）
        /// </summary>
        /// <param name="level">设备性能等级</param>
        /// <returns>等级对应UI颜色</returns>
        public static Color GetDevicePerformanceLevelColor(DevicePerformanceLevel level)
        {
            switch (level)
            {
                case DevicePerformanceLevel.High:
                    // 绿色
                    return new Color32(0x0, 0x82, 0x1A, 0xFF);
                case DevicePerformanceLevel.Mid:
                    // 黄色
                    return new Color32(0xFF, 0xEB, 0x29, 0xFF);
                case DevicePerformanceLevel.Low:
                    // 紫色
                    return new Color32(0x89, 0x0, 0xA4, 0xFF);
            }

            return Color.red;
        }
        #endregion
    }
}