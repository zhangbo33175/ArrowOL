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
    public partial class LuaBehaviour : MonoBehaviour
    {
        //=========================================================================
        // Unity 生命周期
        //=========================================================================
        #region Unity Lifecycle
        private void Awake()
        {
            if (!Application.isEditor)
            {
                m_ShowRaycastTargetsGizmos = false;
            }
        }

        private void OnEnable()
        {
            OnEnableAppended(true);
        }

        private IEnumerator Start()
        {
            while (!m_AwakeOver || !m_EnableOver)
            {
                yield return new WaitForEndOfFrame();
            }

            Action[] luaStarts = LuaStarts;
            if (luaStarts != null)
            {
                for (int index = 0; index < luaStarts.Length; index++)
                {
                    if (luaStarts[index] != null)
                    {
                        luaStarts[index]();
                    }
                }
            }

            m_StartOver = true;
        }

        public void Proc()
        {
            if (!m_StartOver)
            {
                return;
            }

            Action[] luaProcs = LuaProcs;
            if (luaProcs != null)
            {
                for (int index = 0; index < luaProcs.Length; index++)
                {
                    if (luaProcs[index] != null)
                    {
                        luaProcs[index]();
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (m_EnableOver)
            {
                Action[] luaOnDisables = LuaOnDisables;
                if (luaOnDisables != null)
                {
                    for (int index = 0; index < luaOnDisables.Length; index++)
                    {
                        if (luaOnDisables[index] != null)
                        {
                            luaOnDisables[index]();
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            // 执行销毁回调
            Action[] luaOnDestroys = LuaOnDestroys;
            if (luaOnDestroys != null)
            {
                for (int index = 0; index < luaOnDestroys.Length; index++)
                {
                    if (luaOnDestroys[index] != null)
                    {
                        luaOnDestroys[index]();
                    }
                }
            }

            // 释放 Lua 资源
            LuaTable[] ownLuaEnvs = OwnLuaEnvs;
            if (ownLuaEnvs != null)
            {
                for (int index = 0; index < ownLuaEnvs.Length; index++)
                {
                    if (ownLuaEnvs[index] != null)
                    {
                        ownLuaEnvs[index].Dispose();
                    }
                }
            }

            LuaTable[] ownLuaClasses = OwnLuaClasses;
            if (ownLuaClasses != null)
            {
                for (int index = 0; index < ownLuaClasses.Length; index++)
                {
                    if (ownLuaClasses[index] != null)
                    {
                        ownLuaClasses[index].Dispose();
                    }
                }
            }

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
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 每帧驱动 Lua 更新
        /// </summary>
        public void AwakeAppended()
        {
            if (m_AwakeOver) return;

            m_UIComponent = GameComponentsGroup.GetComponent<UIComponent>();
            if (m_UIComponent == null)
            {
                Log.Fatal("UI Component 无效。");
                return;
            }

            m_LuaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (m_LuaComponent == null)
            {
                Log.Fatal("Lua Component 无效。");
                return;
            }

            int index = -1;
            if (m_PatternType == PatternType.None)
            {
                index = (int)NonePatternType.Default;
                if (string.IsNullOrEmpty(LuaScriptNames[index]))
                {
                    Log.Error("LuaBehaviour (None Mode) LuaScriptName 为空。");
                    return;
                }
            }
            else if (m_PatternType == PatternType.MVVM)
            {
                index = (int)MVVMPatternType.View;
                if (string.IsNullOrEmpty(LuaScriptNames[index]))
                {
                    Log.Error("LuaBehaviour (MVVM Mode) LuaScriptName 为空。");
                    return;
                }
            }

            // 初始化数组
            int subPatternTypeTotalNum = 1;
            OwnLuaEnvs = new LuaTable[subPatternTypeTotalNum];
            OwnLuaClasses = new LuaTable[m_PatternType == PatternType.None ? (int)NonePatternType.TotalNum : (int)MVVMPatternType.TotalNum];
            LuaAwakes = new Action[subPatternTypeTotalNum];
            LuaOnEnables = new Action[subPatternTypeTotalNum];
            LuaStarts = new Action[subPatternTypeTotalNum];
            LuaProcs = new Action[subPatternTypeTotalNum];
            LuaOnDisables = new Action[subPatternTypeTotalNum];
            LuaOnDestroys = new Action[subPatternTypeTotalNum];

            // 创建 Lua 环境
            InitLuaEnv(index);
            OwnLuaClasses[index] = m_LuaComponent.LuaCreateLuaClassFromCSEventDelegate(OwnLuaEnvs[index], LuaScriptNames[index]);

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

            Action[] luaAwakes = LuaAwakes;
            if (luaAwakes != null)
            {
                for (int awakeIndex = 0; awakeIndex < luaAwakes.Length; awakeIndex++)
                {
                    if (luaAwakes[awakeIndex] != null)
                    {
                        luaAwakes[awakeIndex]();
                    }
                }
            }

            m_AwakeOver = true;
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
                    Action[] luaOnEnables = LuaOnEnables;
                    if (luaOnEnables != null)
                    {
                        for (int enableIndex = 0; enableIndex < luaOnEnables.Length; enableIndex++)
                        {
                            if (luaOnEnables[enableIndex] != null)
                            {
                                luaOnEnables[enableIndex]();
                            }
                        }
                    }

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
            allDirectChildren.Sort((child1, child2) => { return child1.transform.GetRouteNum() - child2.transform.GetRouteNum(); }); // 由近及远排列
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