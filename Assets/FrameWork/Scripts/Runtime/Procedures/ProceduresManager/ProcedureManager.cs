using System;
using System.Collections.Generic;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class ProcedureManager : Singleton<ProcedureManager>
    {
        private void Start()
        {
            m_RuntimeProcedureRecordInfos = new List<string>();

            // 根据可使用流程类型名称集合进行所有流程实例化
            ProcedureState[] procedures = new ProcedureState[m_ProcedureTypeNames.Length];
            for (int i = 0; i < m_ProcedureTypeNames.Length; i++)
            {
                Type procedureType = Type.GetType(m_ProcedureTypeNames[i]);
                if (procedureType == null)
                {
                    Log.Error("无法找到 procedure 类型 '{0}'.", m_ProcedureTypeNames[i]);
                    return;
                }

                procedures[i] = (ProcedureState)Activator.CreateInstance(procedureType);
                if (procedures[i] == null)
                {
                    Log.Error("无法创建 procedure 实例 '{0}'.", m_ProcedureTypeNames[i]);
                    return;
                }

                // 记录启动入口流程实例化
                if (m_EntryProcedureTypeName == m_ProcedureTypeNames[i])
                {
                    GameConfig.instance.m_EntryProcedure = procedures[i];
                }
            }

            if (GameConfig.instance.m_EntryProcedure == null)
            {
                Log.Error("入口 procedure 无效。");
                return;
            }

            // 用已经创建完毕的流程集合来初始化流程管理器
            GameConfig.instance.m_ProcedureStateMachine = new ProcedureStateMachine(this, procedures);

            // 开始启动入口流程
            StartProcedure(GameConfig.instance.m_EntryProcedure.GetType());
        }


        private void Update()
        {
            if (GameConfig.instance.m_ProcedureStateMachine != null)
            {
                GameConfig.instance.m_ProcedureStateMachine.Update();
            }
        }

        private void OnDestroy()
        {
            // 禁止在OnDestroy中间接调用destroy接口
            //if (m_ProcedureStateMachine != null)
            //{
            //    m_ProcedureStateMachine.Clear();
            //}
        }

        /// <summary>
        /// 初始化流程的lua绑定
        /// </summary>
        public void InitLuaBindings()
        {
            var procedures = GameConfig.instance.m_ProcedureStateMachine.GetAllStates();
            for (int i = 0; i < m_ProcedureTypeNames.Length; i++)
            {
                string[] words = m_ProcedureTypeNames[i].Split('.');
                string luaScriptName = words[words.Length - 1];
                if (LuaScriptWhiteNameList.Contains(luaScriptName))
                {
                    ((ProcedureState)procedures[i]).InitLuaBindings(luaScriptName);
                }
            }
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        private void StartProcedure(Type procedureType)
        {
            if (GameConfig.instance.m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }

            GameConfig.instance.m_ProcedureStateMachine.Start(procedureType);
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(Type procedureType)
        {
            if (GameConfig.instance.m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }

            return GameConfig.instance.m_ProcedureStateMachine.HasState(procedureType);
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
        public ProcedureState GetProcedure(Type procedureType)
        {
            if (GameConfig.instance.m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }

            return (ProcedureState)GameConfig.instance.m_ProcedureStateMachine.GetState(procedureType);
        }

        /// <summary>
        /// 记录运行时流程信息
        /// </summary>
        /// <param name="procedureTypeName"></param>
        /// <param name="time"></param>
        public void RecordRuntimeProcedureInfos(string procedureTypeName, float time)
        {
            if (Application.isPlaying)
            {
                m_RuntimeProcedureRecordInfos.Add(AorTxt.Format("{0}\t（{1:N2}秒）", procedureTypeName, time));
            }
        }

        public override void Dispose()
        {
        }
    }
}