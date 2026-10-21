/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUtilsManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   工具模块门面管理器 - 便捷状态与工具清单（partial）
 *            提供门面就绪状态与本分类通用工具类清单，明确各工具功能与访问方式
 ***************************************************************/

namespace Honor.Runtime
{
    public sealed partial class HonorUtilsManager
    {
        //=========================================================================

        #region 组件引用

        //=========================================================================

        /// <summary>
        /// 触摸输入组件（点击、手势、输入屏蔽；通过 Gestures2D/3D/UI 控制器提供完整能力）
        /// </summary>
        public TouchComponent Touch { get; private set; }

        #endregion

        //=========================================================================

        #region 便捷状态

        //=========================================================================

        /// <summary>
        /// 触摸组件是否已就绪
        /// </summary>
        public bool IsTouchReady => Touch != null;

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

#if EASY_TOUCH_ENABLE
        /// <summary>
        /// 2D 相机手势控制器（转发自 TouchComponent）
        /// </summary>
        public Gestures2D Gestures2D => Touch != null ? Touch.Gestures2D : null;

        /// <summary>
        /// 3D 相机手势控制器（转发自 TouchComponent）
        /// </summary>
        public Gestures3D Gestures3D => Touch != null ? Touch.Gestures3D : null;
#endif

        //=========================================================================

        #region 通用工具类清单（HonorUtils 分类下，静态方式访问）

        //=========================================================================

        // 说明：以下工具均为静态类，无需经本管理器实例化，直接静态调用即可。
        // 本区域仅用于明确本分类的功能清单，供调用方按需使用。

        /// <summary>
        /// [Tools] 日志工具：Log.Info / Log.Warning / Log.Error / Log.Fatal
        /// </summary>
        // public static class Log —— 静态调用 Log.xxx

        /// <summary>
        /// [Tools] 路径工具：GamePathUtils —— 游戏各资源/存档/日志路径常量
        /// </summary>
        // public static class GamePathUtils —— 静态调用 GamePathUtils.xxx

        /// <summary>
        /// [Tools] 加解密工具：AESEncrypt / Encryption / MD5Handler / GZip —— 数据加解密、校验、压缩
        /// </summary>
        // 静态调用 AESEncrypt.xxx / MD5Handler.xxx / GZip.xxx

        /// <summary>
        /// [Utils] UI 工具：UIUtils —— 通用 UI 布局/坐标转换/尺寸适配辅助
        /// </summary>
        // 静态调用 UIUtils.xxx

        /// <summary>
        /// [Utils] 定义工具：DefLayer / HonorDef —— 层级与全局常量定义
        /// </summary>
        // 静态调用 DefLayer.xxx / HonorDef.xxx

        /// <summary>
        /// [Utils] 平台工具：Platform —— 平台类型判断与平台相关能力
        /// </summary>
        // 静态调用 Platform.xxx

        /// <summary>
        /// [Utils] 容器工具：MyList / BetterList —— 高性能容器
        /// </summary>
        // 静态调用 MyList.xxx / BetterList.xxx

        /// <summary>
        /// [Utils] 补间工具：TweenAlpha / TweenPosition / TweenScale 等 —— NGUI 风格补间动画
        /// </summary>
        // 静态/实例调用 TweenAlpha.xxx 等

        #endregion

        #endregion
    }
}
