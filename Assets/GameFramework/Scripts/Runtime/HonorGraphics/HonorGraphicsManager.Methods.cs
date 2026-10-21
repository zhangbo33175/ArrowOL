/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorGraphicsManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   图形模块门面管理器 - 业务便捷方法转发（partial）
 *            收敛着色器/相机震动/图形工具对外能力，外部图形调用统一走本管理器
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class HonorGraphicsManager
    {
        //=========================================================================

        #region 着色器管理（ShaderManager）

        //=========================================================================

        /// <summary>
        /// 查找着色器（带缓存）
        /// </summary>
        /// <param name="shaderName">着色器名称</param>
        /// <returns>着色器对象</returns>
        public Shader FindShader(string shaderName)
        {
            return ShaderManager.Find(shaderName);
        }

        /// <summary>
        /// 预热一批着色器（提前编译，避免运行时卡顿）
        /// </summary>
        /// <param name="shaderNames">着色器名称集合</param>
        public void PrewarmShaders(IEnumerable<string> shaderNames)
        {
            ShaderManager.Prewarm(shaderNames);
        }

        /// <summary>
        /// 清空着色器缓存
        /// </summary>
        public void ClearShaderCache()
        {
            ShaderManager.ClearCache();
        }

        #endregion

        //=========================================================================

        #region 相机震动（CameraShake）

        //=========================================================================

        /// <summary>
        /// 相机综合震动（位置+旋转）
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="duration">震动时长（秒）</param>
        /// <param name="posStrength">位置震动强度，默认0.3</param>
        /// <param name="rotStrength">旋转震动强度，默认1</param>
        public void ShakeCamera(Camera camera, float duration, float posStrength = 0.3f, float rotStrength = 1f)
        {
            CameraShake.Shake(camera, duration, posStrength, rotStrength);
        }

        /// <summary>
        /// 相机位置震动
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="duration">震动时长（秒）</param>
        /// <param name="strength">位置震动强度，默认0.3</param>
        public void ShakeCameraPosition(Camera camera, float duration, float strength = 0.3f)
        {
            CameraShake.ShakePosition(camera, duration, strength);
        }

        /// <summary>
        /// 相机旋转震动
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="duration">震动时长（秒）</param>
        /// <param name="strength">旋转震动强度，默认1</param>
        /// <param name="vibrato">震动次数，默认30</param>
        public void ShakeCameraRotation(Camera camera, float duration, float strength = 1f, int vibrato = 30)
        {
            CameraShake.ShakeRotation(camera, duration, strength, vibrato);
        }

        #endregion

        //=========================================================================

        #region 图形工具（GraphicsUtils）

        //=========================================================================

        /// <summary>
        /// 获取角色根节点 Transform
        /// </summary>
        /// <param name="trans">任意子节点</param>
        /// <returns>角色根节点</returns>
        public Transform GetRoleRootTransform(Transform trans)
        {
            return GraphicsUtils.GetRoleRootTransform(trans);
        }
        
        #endregion
    }
}
