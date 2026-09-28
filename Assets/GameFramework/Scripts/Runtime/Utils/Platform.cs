/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  Platform.cs
 * author:  云毅
 * created:
 * descrip:   运行平台工具类 - 统一判断当前运行环境（编辑器/Android/iOS/Windows）与编译宏
 * 优化记录: 由旧版 HonorUtils.Platform 迁移，统一命名空间与编码规范，补齐宏开关说明
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 运行平台工具类
    /// 功能：集中判断当前运行平台、内置编译宏开关，供业务层统一使用
    /// 所有平台判断都应优先走本工具，避免散落的 Application.platform 判断
    /// </summary>
    public static class Platform
    {
        #region 基础平台判断

        /// <summary>
        /// 是否编辑器环境（Windows/OSX 编辑器）
        /// </summary>
        public static bool IsEditor
        {
            get
            {
                return Application.platform == RuntimePlatform.WindowsEditor
                    || Application.platform == RuntimePlatform.OSXEditor;
            }
        }

        /// <summary>
        /// 是否 Android 真机
        /// </summary>
        public static bool IsAndroid
        {
            get { return Application.platform == RuntimePlatform.Android; }
        }

        /// <summary>
        /// 是否 iOS 真机
        /// </summary>
        public static bool IsIPhone
        {
            get { return Application.platform == RuntimePlatform.IPhonePlayer; }
        }

        /// <summary>
        /// 是否 Flash 平台（历史保留，当前 Unity 已不支持）
        /// </summary>
        public static bool IsFlash
        {
            get { return Application.platform == RuntimePlatform.FlashPlayer; }
        }

        /// <summary>
        /// 是否 Windows 独立播放器
        /// </summary>
        public static bool IsWindows
        {
            get { return Application.platform == RuntimePlatform.WindowsPlayer; }
        }

        /// <summary>
        /// 是否 Windows 平台（编辑器 + 独立播放器）
        /// </summary>
        public static bool IsWindowsPlatform
        {
            get { return Application.platform == RuntimePlatform.WindowsEditor || IsWindows; }
        }

        #endregion

        #region 编译宏开关

        /// <summary>
        /// 是否发布模式（需在 Player Settings 定义 BUILD_MODE 宏）
        /// </summary>
        public static bool BUILD_MODE
        {
            get
            {
#if BUILD_MODE
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// 是否无 Honor 模式（需在 Player Settings 定义 NOHONOR 宏）
        /// </summary>
        public static bool NOHONOR
        {
            get
            {
#if NOHONOR
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// 是否特定版本分支（需在 Player Settings 定义 _BANSHU 宏）
        /// </summary>
        public static bool _BANSHU
        {
            get
            {
#if _BANSHU
                return true;
#else
                return false;
#endif
            }
        }

        #endregion
    }
}
