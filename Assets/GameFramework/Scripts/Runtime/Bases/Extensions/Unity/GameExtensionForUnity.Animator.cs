using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// Animator中是否包含指定的参数
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasParameterOfType(this Animator animator, string name, AnimatorControllerParameterType type)
        {
            if (string.IsNullOrEmpty(name)) { return false; }
            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (AnimatorControllerParameter currParam in parameters)
            {
                if (currParam.type == type && currParam.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 向外部传入的parameterList中添加参数名称
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameter"></param>
        /// <param name="type"></param>
        /// <param name="parameterList"></param>
        public static void AddAnimatorParameterIfExists(Animator animator, string parameterName, out int parameter, AnimatorControllerParameterType type, HashSet<int> parameterList)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                parameter = -1;
                return;
            }

            parameter = Animator.StringToHash(parameterName);

            if (animator.HasParameterOfType(parameterName, type))
            {
                parameterList.Add(parameter);
            }
        }

        /// <summary>
        /// 向外部传入的parameterList中添加参数名称
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="type"></param>
        /// <param name="parameterList"></param>
        public static void AddAnimatorParameterIfExists(Animator animator, string parameterName, AnimatorControllerParameterType type, HashSet<string> parameterList)
        {
            if (animator.HasParameterOfType(parameterName, type))
            {
                parameterList.Add(parameterName);
            }
        }

        /// <summary>
        /// 设置Animator的Boolean值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public static void UpdateAnimatorBool(Animator animator, string parameterName, bool value)
        {
            animator.SetBool(parameterName, value);
        }


        /// <summary>
        /// 设置Animator的Integer值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public static void UpdateAnimatorInteger(Animator animator, string parameterName, int value)
        {
            animator.SetInteger(parameterName, value);
        }

        /// <summary>
        /// 设置Animator的Float值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="performSanityCheck"></param>
        public static void UpdateAnimatorFloat(Animator animator, string parameterName, float value, bool performSanityCheck = true)
        {
            animator.SetFloat(parameterName, value);
        }

        /// <summary>
        /// 设置Animator的Boolean值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <param name="parameterList"></param>
        /// <param name="performSanityCheck"></param>
        /// <returns></returns>
        public static bool UpdateAnimatorBool(Animator animator, int parameter, bool value, HashSet<int> parameterList, bool performSanityCheck = true)
        {
            if (performSanityCheck && !parameterList.Contains(parameter))
            {
                return false;
            }
            animator.SetBool(parameter, value);
            return true;
        }

        /// <summary>
        /// 设置Animator的Trigger
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameter"></param>
        /// <param name="parameterList"></param>
        public static bool UpdateAnimatorTrigger(Animator animator, int parameter, HashSet<int> parameterList, bool performSanityCheck = true)
        {
            if (performSanityCheck && !parameterList.Contains(parameter))
            {
                return false;
            }
            animator.SetTrigger(parameter);
            return true;
        }

        /// <summary>
        /// 设置Animator的Float值
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameter">Parameter name.</param>
        /// <param name="value">Value.</param>
        public static bool UpdateAnimatorFloat(Animator animator, int parameter, float value, HashSet<int> parameterList, bool performSanityCheck = true)
        {
            if (performSanityCheck && !parameterList.Contains(parameter))
            {
                return false;
            }
            animator.SetFloat(parameter, value);
            return true;
        }

        /// <summary>
        /// 设置Animator的Integer值
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameter">Parameter name.</param>
        /// <param name="value">Value.</param>
        public static bool UpdateAnimatorInteger(Animator animator, int parameter, int value, HashSet<int> parameterList, bool performSanityCheck = true)
        {
            if (performSanityCheck && !parameterList.Contains(parameter))
            {
                return false;
            }
            animator.SetInteger(parameter, value);
            return true;
        }

        // <summary>
        /// 设置Animator的Boolean值
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void UpdateAnimatorBool(Animator animator, string parameterName, bool value, HashSet<string> parameterList, bool performSanityCheck = true)
        {
            if (parameterList.Contains(parameterName))
            {
                animator.SetBool(parameterName, value);
            }
        }

        /// <summary>
        /// 设置Animator的Trigger
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterList"></param>
        public static void UpdateAnimatorTrigger(Animator animator, string parameterName, HashSet<string> parameterList, bool performSanityCheck = true)
        {
            if (parameterList.Contains(parameterName))
            {
                animator.SetTrigger(parameterName);
            }
        }

        /// <summary>
        /// 设置Animator的Float值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="parameterList"></param>
        /// <param name="performSanityCheck"></param>
		public static void UpdateAnimatorFloat(Animator animator, string parameterName, float value, HashSet<string> parameterList, bool performSanityCheck = true)
        {
            if (parameterList.Contains(parameterName))
            {
                animator.SetFloat(parameterName, value);
            }
        }

        /// <summary>
        /// 设置Animator的Integer值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="parameterList"></param>
        /// <param name="performSanityCheck"></param>
        public static void UpdateAnimatorInteger(Animator animator, string parameterName, int value, HashSet<string> parameterList, bool performSanityCheck = true)
        {
            if (parameterList.Contains(parameterName))
            {
                animator.SetInteger(parameterName, value);
            }
        }

        /// <summary>
        /// 设置Animator的Boolean值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="performSanityCheck"></param>
        public static void UpdateAnimatorBoolIfExists(Animator animator, string parameterName, bool value, bool performSanityCheck = true)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Bool))
            {
                animator.SetBool(parameterName, value);
            }
        }

        /// <summary>
        /// 设置Animator的Trigger
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        public static void UpdateAnimatorTriggerIfExists(Animator animator, string parameterName, bool performSanityCheck = true)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Trigger))
            {
                animator.SetTrigger(parameterName);
            }
        }

        /// <summary>
        /// 设置Animator的Float值
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="performSanityCheck"></param>
        public static void UpdateAnimatorFloatIfExists(Animator animator, string parameterName, float value, bool performSanityCheck = true)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Float))
            {
                animator.SetFloat(parameterName, value);
            }
        }

        /// <summary>
        /// 设置Animator的Trigger
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        public static void UpdateAnimatorIntegerIfExists(Animator animator, string parameterName, int value, bool performSanityCheck = true)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Int))
            {
                animator.SetInteger(parameterName, value);
            }
        }

    }
}


