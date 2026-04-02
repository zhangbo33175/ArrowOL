using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {

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
            if(m_EnableOver)
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

            LuaTable[] ownLuaEnvs = OwnLuaEnvs;
            if(ownLuaEnvs != null)
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
            if(ownLuaClasses != null)
            {
                for (int index = 0; index < ownLuaClasses.Length; index++)
                {
                    if (ownLuaClasses[index] != null)
                    {
                        ownLuaClasses[index].Dispose();
                    }
                }
            }

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
        /// 附加式Awake调用
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
            else if(m_PatternType == PatternType.MVVM)
            {
                index = (int)MVVMPatternType.View;
                if (string.IsNullOrEmpty(LuaScriptNames[index]))
                {
                    Log.Error("LuaBehaviour (MVVM Mode) LuaScriptName 为空。");
                    return;
                }
            }

            int subPatternTypeTotalNum = 1;
            OwnLuaEnvs = new LuaTable[subPatternTypeTotalNum];
            OwnLuaClasses = new LuaTable[m_PatternType == PatternType.None ? (int)NonePatternType.TotalNum : (int)MVVMPatternType.TotalNum];
            LuaAwakes = new Action[subPatternTypeTotalNum];
            LuaOnEnables = new Action[subPatternTypeTotalNum];
            LuaStarts = new Action[subPatternTypeTotalNum];
            LuaProcs = new Action[subPatternTypeTotalNum];
            LuaOnDisables = new Action[subPatternTypeTotalNum];
            LuaOnDestroys = new Action[subPatternTypeTotalNum];

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
            if(m_UseCollider2DLifeCycles)
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
        /// 附加式OnEnable调用
        /// </summary>
        public void OnEnableAppended(bool isAuto = false)
        {
            // 自动调用进入 或 已经完成调用情况下的手动调用进入
            if(isAuto || !m_EnableOver)
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

        ///// <summary>
        ///// 父节点变化回调
        ///// </summary>
        //private void OnTransformParentChanged()
        //{

        //}

        /// <summary>
        /// 获取所有直系孩子节点（不包括自身）
        /// 直系包括相邻和不相邻的首个LuaBehaviour子节点
        /// </summary>
        /// <returns></returns>
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
        /// 调用Lua层关闭方法
        /// </summary>
        public void CallLuaClose()
        {
            if (ValidLuaClass != null)
            {
                ValidLuaClass.Get("Close", out LuaFunction closeFunc);
                if(closeFunc != null)
                {
                    closeFunc.Action(ValidLuaClass);
                }
            }
        }

    }
}


