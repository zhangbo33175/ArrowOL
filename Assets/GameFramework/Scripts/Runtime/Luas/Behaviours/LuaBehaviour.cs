/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   Lua 脚本生命周期调度核心组件，负责 C# <=> Lua 交互
 ***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// LuaBehaviour - Unity 生命周期调度部分
    /// </summary>
    /// <remarks>
    /// 本分部文件承载 MonoBehaviour 生命周期回调（Awake/OnEnable/Start/Proc/OnDisable/OnDestroy），
    /// 将 Unity 生命周期转发到对应 Lua 层方法，并在销毁时安全释放 Lua 环境。
    /// </remarks>
    public partial class LuaBehaviour : MonoBehaviour
    {
        //=========================================================================
        // Unity 生命周期
        //=========================================================================
        #region Unity Lifecycle

        /// <summary>
        /// 组件初始化：非编辑器环境下关闭 RaycastTarget Gizmos 绘制
        /// </summary>
        private void Awake()
        {
            if (!Application.isEditor)
            {
                m_ShowRaycastTargetsGizmos = false;
            }
        }

        /// <summary>
        /// 组件启用时附加触发 Lua OnEnable 回调
        /// </summary>
        private void OnEnable()
        {
            OnEnableAppended(true);
        }

        /// <summary>
        /// 协程 Start：等待 Awake/OnEnable 完成后，依次触发 Lua Start 回调
        /// </summary>
        private IEnumerator Start()
        {
            while (!m_AwakeOver || !m_EnableOver)
            {
                yield return new WaitForEndOfFrame();
            }

            InvokeAll(LuaStarts);

            m_StartOver = true;
        }

        /// <summary>
        /// 逻辑帧驱动：Start 完成后逐帧转发 Lua Proc 回调
        /// </summary>
        public void Proc()
        {
            if (!m_StartOver)
            {
                return;
            }

            InvokeAll(LuaProcs);
        }

        /// <summary>
        /// 组件禁用：转发 Lua OnDisable 回调（仅在已完成 OnEnable 后）
        /// </summary>
        private void OnDisable()
        {
            if (m_EnableOver)
            {
                InvokeAll(LuaOnDisables);
            }
        }

        /// <summary>
        /// 组件销毁：转发 Lua OnDestroy 回调，并释放所有 Lua 环境与类引用
        /// </summary>
        private void OnDestroy()
        {
            // 执行销毁回调
            InvokeAll(LuaOnDestroys);

            // 释放 Lua 资源
            DisposeAll(OwnLuaEnvs);
            DisposeAll(OwnLuaClasses);

            // 清空所有引用（防泄漏）
            LuaOnDestroys = null;
            LuaOnDisables = null;
            LuaProcs = null;
            LuaStarts = null;
            LuaOnEnables = null;
            LuaAwakes = null;
            OwnLuaClasses = null;
            OwnLuaEnvs = null;
            m_Injections = null;
        }

        /// <summary>
        /// 依次调用委托数组中所有非空委托（空数组直接忽略）
        /// </summary>
        /// <param name="callbacks">待调用的委托数组</param>
        private static void InvokeAll(Action[] callbacks)
        {
            if (callbacks == null)
            {
                return;
            }

            for (int i = 0; i < callbacks.Length; i++)
            {
                callbacks[i]?.Invoke();
            }
        }

        /// <summary>
        /// 释放数组中所有非空 LuaTable（空数组直接忽略）
        /// </summary>
        /// <param name="tables">待释放的 LuaTable 数组</param>
        private static void DisposeAll(LuaTable[] tables)
        {
            if (tables == null)
            {
                return;
            }

            for (int i = 0; i < tables.Length; i++)
            {
                tables[i]?.Dispose();
            }
        }
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 附加式 Awake 初始化：获取依赖组件、创建 Lua 环境并绑定生命周期回调
        /// </summary>
        public void AwakeAppended()
        {
            if (m_AwakeOver) return;

            if (!TryGetRequiredComponents()) return;

            int index = ResolveMainScriptIndex(out bool aborted);
            if (aborted) return;

            InitLuaArrays();

            // 创建 Lua 环境
            InitLuaEnv(index);
            OwnLuaClasses[index] = m_LuaComponent.LuaCreateLuaClassFromCSEventDelegate(OwnLuaEnvs[index], LuaScriptNames[index]);

            BindLifeCycleLuaFunctions(index);

            InvokeLuaAwakes();

            m_AwakeOver = true;
        }

        /// <summary>
        /// 获取AwakeAppended所需的UI与Lua组件，缺失时打印致命错误
        /// </summary>
        /// <returns>组件是否均有效</returns>
        private bool TryGetRequiredComponents()
        {
            m_UIComponent = GameComponentsGroup.GetComponent<UIComponent>();
            if (m_UIComponent == null)
            {
                Log.Fatal("UI Component 无效。");
                return false;
            }

            m_LuaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (m_LuaComponent == null)
            {
                Log.Fatal("Lua Component 无效。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据模式类型解析主脚本下标，并校验脚本名非空
        /// </summary>
        /// <param name="aborted">是否因脚本名为空而中止初始化</param>
        /// <returns>主脚本下标</returns>
        private int ResolveMainScriptIndex(out bool aborted)
        {
            aborted = false;
            int index = -1;
            if (m_PatternType == PatternType.None)
            {
                index = (int)NonePatternType.Default;
                if (string.IsNullOrEmpty(LuaScriptNames[index]))
                {
                    Log.Error("LuaBehaviour (None Mode) LuaScriptName 为空。");
                    aborted = true;
                }
            }
            else if (m_PatternType == PatternType.MVVM)
            {
                index = (int)MVVMPatternType.View;
                if (string.IsNullOrEmpty(LuaScriptNames[index]))
                {
                    Log.Error("LuaBehaviour (MVVM Mode) LuaScriptName 为空。");
                    aborted = true;
                }
            }

            return index;
        }

        /// <summary>
        /// 初始化Lua环境与各生命周期委托数组
        /// </summary>
        private void InitLuaArrays()
        {
            // 初始化数组
            int subPatternTypeTotalNum = 1;
            OwnLuaEnvs = new LuaTable[subPatternTypeTotalNum];
            OwnLuaClasses = new LuaTable[m_PatternType == PatternType.None ? (int)NonePatternType.TotalCount : (int)MVVMPatternType.TotalCount];
            LuaAwakes = new Action[subPatternTypeTotalNum];
            LuaOnEnables = new Action[subPatternTypeTotalNum];
            LuaStarts = new Action[subPatternTypeTotalNum];
            LuaProcs = new Action[subPatternTypeTotalNum];
            LuaOnDisables = new Action[subPatternTypeTotalNum];
            LuaOnDestroys = new Action[subPatternTypeTotalNum];
        }

        /// <summary>
        /// 绑定Lua必需生命周期函数及碰撞/触发器生命周期
        /// </summary>
        /// <param name="index">Lua 脚本独立环境数组下标</param>
        private void BindLifeCycleLuaFunctions(int index)
        {
            // 绑定Lua中必需的声明周期函数到C#
            OwnLuaEnvs[index].Get("Awake", out LuaAwakes[index]);
            OwnLuaEnvs[index].Get("OnEnable", out LuaOnEnables[index]);
            OwnLuaEnvs[index].Get("Start", out LuaStarts[index]);
            OwnLuaEnvs[index].Get("Proc", out LuaProcs[index]);
            OwnLuaEnvs[index].Get("OnDisable", out LuaOnDisables[index]);
            OwnLuaEnvs[index].Get("OnDestroy", out LuaOnDestroys[index]);

            // 绑定2D碰撞生命周期函数
            if (m_UseCollider2DLifeCycles)
            {
                Collider2DLifeCyclesBehaviour.LuaBinding(OwnLuaEnvs[index]);
            }

            // 绑定3D碰撞生命周期函数
            if (m_UseCollider3DLifeCycles)
            {
                Collider3DLifeCyclesBehaviour.LuaBinding(OwnLuaEnvs[index]);
            }

            // 绑定2D触发器生命周期函数
            if (m_UseTrigger2DLifeCycles)
            {
                Trigger2DLifeCyclesBehaviour.LuaBinding(OwnLuaEnvs[index]);
            }

            // 绑定3D触发器生命周期函数
            if (m_UseTrigger3DLifeCycles)
            {
                Trigger3DLifeCyclesBehaviour.LuaBinding(OwnLuaEnvs[index]);
            }
        }

        /// <summary>
        /// 依次调用已绑定的Lua Awake委托
        /// </summary>
        private void InvokeLuaAwakes()
        {
            InvokeAll(LuaAwakes);
        }

        /// <summary>
        /// 附加式 OnEnable 调用
        /// </summary>
        public void OnEnableAppended(bool isAuto = false)
        {
            if (isAuto || !m_EnableOver)
            {
                if (m_AwakeOver && enabled && gameObject.activeInHierarchy)
                {
                    InvokeAll(LuaOnEnables);

                    m_EnableOver = true;
                }
            }
        }

        /// <summary>
        /// 获取所有直系子节点 LuaBehaviour
        /// </summary>
        public List<LuaBehaviour> GetAllDirectChildren()
        {
            List<LuaBehaviour> allDirectChildren = new List<LuaBehaviour>();
            GetComponentsInChildren(true, allDirectChildren);
            allDirectChildren.Remove(this);
            allDirectChildren.Sort((a, b) => a.transform.GetRouteNum() - b.transform.GetRouteNum()); // 由近及远排列
            for (int index = allDirectChildren.Count - 1; index >= 0; index--)  // 反向（由远及近的片段），方便数组移除
            {
                var parent = allDirectChildren[index].transform.parent;
                while (parent != transform)
                {
                    if (parent.GetComponent<LuaBehaviour>() != null)
                    {
                        allDirectChildren.RemoveAt(index);
                        break;
                    }
                    parent = parent.parent;
                }
            }

            return allDirectChildren;
        }

        /// <summary>
        /// 调用 Lua 层 Close 方法
        /// </summary>
        public void CallLuaClose()
        {
            if (ValidLuaClass != null)
            {
                ValidLuaClass.Get("Close", out LuaFunction closeFunc);
                if (closeFunc != null)
                {
                    closeFunc.Action(ValidLuaClass);
                }
            }
        }
        #endregion
    }
}
