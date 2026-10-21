/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorGraphicsManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   图形模块门面管理器 - 分类能力说明（partial）
 *            明确本分类各静态工具的能力与访问方式，供调用方按需使用
 ***************************************************************/

namespace Honor.Runtime
{
    public sealed partial class HonorGraphicsManager
    {
        //=========================================================================

        #region 分类能力说明

        //=========================================================================

        // 本分类均为静态工具类，本门面作为统一转发入口，各工具能力说明：
        //
        // [ShaderManager] 着色器管理：Find（查找带缓存）/ Prewarm（预热）/ ClearCache（清空缓存）
        //   经本门面：FindShader / PrewarmShaders / ClearShaderCache
        //
        // [CameraShake] 相机震动：Shake（综合）/ ShakePosition（位置）/ ShakeRotation（旋转）
        //   经本门面：ShakeCamera / ShakeCameraPosition / ShakeCameraRotation
        //
        // [GraphicsUtils] 图形工具：GetRoleRootTransform（角色根节点）/ ScreenRatio（屏幕比例）
        //   经本门面：GetRoleRootTransform；ScreenRatio 依赖 EScreenType 枚举，直接静态调用

        #endregion
    }
}
