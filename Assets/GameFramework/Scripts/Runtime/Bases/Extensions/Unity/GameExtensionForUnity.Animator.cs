/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameExtensionForUnity.cs
 * author:    云毅  
 * created:   2026   2026
 * descrip:   Unity Animator 组件扩展方法，提供安全、高性能、带参数校验的动画控制接口
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    #region Animator 动画控制器扩展方法
    //=========================================================================
    // Animator 动画控制器扩展方法
    //=========================================================================
    /// <summary>
    /// Unity Animator 组件扩展方法
    /// 提供安全、带校验、高性能的动画参数设置，防止因参数不存在导致的报错
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        #region 基础参数校验方法
        /// <summary>
        /// 检查 Animator 是否包含指定名称和类型的参数
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="type">参数类型</param>
        /// <returns>存在返回 true，不存在返回 false</returns>
        public static bool HasParameterOfType(this Animator animator, string paramName, AnimatorControllerParameterType type)
        {
            if (string.IsNullOrEmpty(paramName) || animator == null)
                return false;

            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (AnimatorControllerParameter param in parameters)
            {
                if (param.name == paramName && param.type == type)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 如果参数存在，则将其哈希值加入列表（用于高性能校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramHash">参数哈希值（输出）</param>
        /// <param name="type">参数类型</param>
        /// <param name="paramList">参数哈希值缓存列表</param>
        public static void AddAnimatorParameterIfExists(this Animator animator, string paramName, out int paramHash, AnimatorControllerParameterType type, HashSet<int> paramList)
        {
            paramHash = -1;

            if (string.IsNullOrEmpty(paramName))
                return;

            paramHash = Animator.StringToHash(paramName);

            if (animator.HasParameterOfType(paramName, type))
                paramList.Add(paramHash);
        }

        /// <summary>
        /// 如果参数存在，则将其名称加入列表
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="paramList">参数名称缓存列表</param>
        public static void AddAnimatorParameterIfExists(this Animator animator, string paramName, AnimatorControllerParameterType type, HashSet<string> paramList)
        {
            if (animator.HasParameterOfType(paramName, type))
                paramList.Add(paramName);
        }
        #endregion

        #region 直接设置（无缓存校验）
        /// <summary>
        /// 直接设置 Bool 参数（无校验，性能最高，需确保参数存在）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        public static void UpdateAnimatorBool(this Animator animator, string paramName, bool value)
        {
            animator.SetBool(paramName, value);
        }

        /// <summary>
        /// 直接设置 Int 参数（无校验，性能最高，需确保参数存在）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        public static void UpdateAnimatorInteger(this Animator animator, string paramName, int value)
        {
            animator.SetInteger(paramName, value);
        }

        /// <summary>
        /// 直接设置 Float 参数（无校验，性能最高，需确保参数存在）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        public static void UpdateAnimatorFloat(this Animator animator, string paramName, float value)
        {
            animator.SetFloat(paramName, value);
        }
        #endregion

        #region 哈希值 + 缓存列表（高性能）
        /// <summary>
        /// 安全设置 Bool 参数（使用哈希 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramHash">参数哈希值</param>
        /// <param name="value">参数值</param>
        /// <param name="paramList">参数哈希缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        /// <returns>设置成功返回 true</returns>
        public static bool UpdateAnimatorBool(this Animator animator, int paramHash, bool value, HashSet<int> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramHash))
                return false;

            animator.SetBool(paramHash, value);
            return true;
        }

        /// <summary>
        /// 安全设置 Trigger 参数（使用哈希 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramHash">参数哈希值</param>
        /// <param name="paramList">参数哈希缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        /// <returns>设置成功返回 true</returns>
        public static bool UpdateAnimatorTrigger(this Animator animator, int paramHash, HashSet<int> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramHash))
                return false;

            animator.SetTrigger(paramHash);
            return true;
        }

        /// <summary>
        /// 安全设置 Float 参数（使用哈希 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramHash">参数哈希值</param>
        /// <param name="value">参数值</param>
        /// <param name="paramList">参数哈希缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        /// <returns>设置成功返回 true</returns>
        public static bool UpdateAnimatorFloat(this Animator animator, int paramHash, float value, HashSet<int> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramHash))
                return false;

            animator.SetFloat(paramHash, value);
            return true;
        }

        /// <summary>
        /// 安全设置 Int 参数（使用哈希 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramHash">参数哈希值</param>
        /// <param name="value">参数值</param>
        /// <param name="paramList">参数哈希缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        /// <returns>设置成功返回 true</returns>
        public static bool UpdateAnimatorInteger(this Animator animator, int paramHash, int value, HashSet<int> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramHash))
                return false;

            animator.SetInteger(paramHash, value);
            return true;
        }
        #endregion

        #region 字符串 + 缓存列表
        /// <summary>
        /// 安全设置 Bool 参数（使用字符串 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="paramList">参数名称缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        public static void UpdateAnimatorBool(this Animator animator, string paramName, bool value, HashSet<string> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramName))
                return;

            animator.SetBool(paramName, value);
        }

        /// <summary>
        /// 安全设置 Trigger 参数（使用字符串 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramList">参数名称缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        public static void UpdateAnimatorTrigger(this Animator animator, string paramName, HashSet<string> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramName))
                return;

            animator.SetTrigger(paramName);
        }

        /// <summary>
        /// 安全设置 Float 参数（使用字符串 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="paramList">参数名称缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        public static void UpdateAnimatorFloat(this Animator animator, string paramName, float value, HashSet<string> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramName))
                return;

            animator.SetFloat(paramName, value);
        }

        /// <summary>
        /// 安全设置 Int 参数（使用字符串 + 缓存列表校验）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="paramList">参数名称缓存列表</param>
        /// <param name="performCheck">是否执行校验</param>
        public static void UpdateAnimatorInteger(this Animator animator, string paramName, int value, HashSet<string> paramList, bool performCheck = true)
        {
            if (performCheck && !paramList.Contains(paramName))
                return;

            animator.SetInteger(paramName, value);
        }
        #endregion

        #region 实时检查（安全但性能较低，适合调试）
        /// <summary>
        /// 检查参数存在后再设置 Bool（安全，适合调试）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        public static void UpdateAnimatorBoolIfExists(this Animator animator, string paramName, bool value)
        {
            if (animator.HasParameterOfType(paramName, AnimatorControllerParameterType.Bool))
                animator.SetBool(paramName, value);
        }

        /// <summary>
        /// 检查参数存在后再设置 Trigger（安全，适合调试）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        public static void UpdateAnimatorTriggerIfExists(this Animator animator, string paramName)
        {
            if (animator.HasParameterOfType(paramName, AnimatorControllerParameterType.Trigger))
                animator.SetTrigger(paramName);
        }

        /// <summary>
        /// 检查参数存在后再设置 Float（安全，适合调试）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        public static void UpdateAnimatorFloatIfExists(this Animator animator, string paramName, float value)
        {
            if (animator.HasParameterOfType(paramName, AnimatorControllerParameterType.Float))
                animator.SetFloat(paramName, value);
        }

        /// <summary>
        /// 检查参数存在后再设置 Int（安全，适合调试）
        /// </summary>
        /// <param name="animator">目标动画器</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        public static void UpdateAnimatorIntegerIfExists(this Animator animator, string paramName, int value)
        {
            if (animator.HasParameterOfType(paramName, AnimatorControllerParameterType.Int))
                animator.SetInteger(paramName, value);
        }
        #endregion
    }
    #endregion
}