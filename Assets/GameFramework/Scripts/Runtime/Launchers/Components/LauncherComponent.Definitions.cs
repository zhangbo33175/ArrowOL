/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LauncherComponent.Performance.cs
 * author:    云毅
 * created:   2026
 * descrip:   框架启动器 - 设备性能配置类（partial）
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 框架启动器 - 设备性能配置部分
    /// </summary>
    public sealed partial class LauncherComponent : GameComponent
    {
        /// <summary>
        /// 设备性能配置数据（用于自动检测设备性能等级）
        /// 可在 Inspector 中配置
        /// </summary>
        [System.Serializable]
        public class DevicePerformanceData
        {
            //=========================================================================
            // 公共字段
            //=========================================================================
            #region Public Fields
            /// <summary>
            /// CPU 核心数
            /// </summary>
            public int ProcessorCount;

            /// <summary>
            /// 高档设备 GPU 显存阈值（MB）
            /// </summary>
            public int GraphicsMemorySizeHighBase;

            /// <summary>
            /// 中档设备 GPU 显存阈值（MB）
            /// </summary>
            public int GraphicsMemorySizeMidBase;

            /// <summary>
            /// 高档设备 运行内存阈值（MB）
            /// </summary>
            public int SystemMemorySizeHighBase;

            /// <summary>
            /// 中档设备 运行内存阈值（MB）
            /// </summary>
            public int SystemMemorySizeMidBase;
            #endregion

            //=========================================================================
            // 构造函数
            //=========================================================================
            #region Constructor
            /// <summary>
            /// 构造设备性能数据
            /// </summary>
            /// <param name="processorCount">CPU核心数</param>
            /// <param name="graphicsMemorySizeHighBase">高档GPU显存阈值</param>
            /// <param name="graphicsMemorySizeMidBase">中档GPU显存阈值</param>
            /// <param name="systemMemorySizeHighBase">高档运行内存阈值</param>
            /// <param name="systemMemorySizeMidBase">中档运行内存阈值</param>
            public DevicePerformanceData(
                int processorCount, 
                int graphicsMemorySizeHighBase, 
                int graphicsMemorySizeMidBase, 
                int systemMemorySizeHighBase, 
                int systemMemorySizeMidBase)
            {
                ProcessorCount              = processorCount;
                GraphicsMemorySizeHighBase   = graphicsMemorySizeHighBase;
                GraphicsMemorySizeMidBase    = graphicsMemorySizeMidBase;
                SystemMemorySizeHighBase     = systemMemorySizeHighBase;
                SystemMemorySizeMidBase      = systemMemorySizeMidBase;
            }
            #endregion
        }
    }
}