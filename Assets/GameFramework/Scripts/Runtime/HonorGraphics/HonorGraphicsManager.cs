/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorGraphicsManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   图形模块门面管理器 - HonorGraphics 分类统一入口（主文件）
 *            统一管理图形工具（Shader/相机震动/特效），外部图形调用统一走本管理器
 *            业务转发方法见 HonorGraphicsManager.Methods.cs；分类说明见 HonorGraphicsManager.Visitors.cs
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 图形模块门面管理器（HonorGraphics 分类统一入口）
    /// 功能：统一转发图形能力（ShaderManager 着色器、CameraShake 相机震动、GraphicsUtils 图形工具）
    /// 外部调用：HonorGraphicsManager.Instance.FindShader(...)
    /// 说明：本分类均为静态工具类，本管理器作为统一入口封装，纯代码单例
    /// </summary>
    public sealed partial class HonorGraphicsManager : Singleton<HonorGraphicsManager>
    {
        //=========================================================================

        #region 生命周期

        //=========================================================================

        /// <summary>
        /// 初始化：本分类为静态工具类，无需实例引用
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// 释放：静态工具类无需清理
        /// </summary>
        public override void Dispose()
        {
        }

        #endregion
    }
}
