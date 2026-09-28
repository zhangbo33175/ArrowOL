/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  HonorDef.cs
 * author:  云毅
 * created:
 * descrip:   游戏全局常量定义 - 通用前缀与 PlayerPrefs 存储键
 * 优化记录: 由旧版 HonorUtils.HonorDef 迁移，统一命名空间
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏全局常量定义
    /// 功能：集中声明跨模块使用的常量（如 PlayerPrefs 存储键枚举）
    /// </summary>
    public static class HonorDef
    {
        /// <summary>多分辨率适配后缀</summary>
        public const string multiDef = "_multi";

        /// <summary>
        /// PlayerPrefs 存储键枚举
        /// 说明：使用枚举名作为存储 Key，避免字符串拼写错误
        /// </summary>
        public enum PlayerPrefsKey
        {
            /// <summary>帧率</summary>
            PPK_FPS,

            /// <summary>分辨率</summary>
            PPK_Resolution,

            /// <summary>画质质量</summary>
            PPK_GraphicsQuality,

            /// <summary>CPU 型号</summary>
            PPK_CPU,

            /// <summary>GPU 型号</summary>
            PPK_GPU,

            /// <summary>OpenGL 版本</summary>
            PPK_OpenGLVersion,

            /// <summary>Android API Level</summary>
            PPK_AndroidAPILevel,

            /// <summary>设备性能档位</summary>
            PPK_DeviceLevel,

            /// <summary>客户端 Source 版本</summary>
            PPK_SourceVersion,
        }
    }
}
