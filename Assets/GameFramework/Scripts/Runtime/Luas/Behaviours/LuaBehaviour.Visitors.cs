/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviour.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaBehaviour - 属性、公共访问器、字段定义
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {
        //=========================================================================
        // 序列化配置字段
        //=========================================================================

        #region Serialized Fields

        /// <summary>
        /// 按设计模式在 MVVM / None 两套存储之间取值（仅 MVVM 走 mvvm 分支，其余走 none 分支）
        /// </summary>
        /// <typeparam name="T">存储值类型</typeparam>
        /// <param name="mvvmBranch">MVVM 模式取值委托</param>
        /// <param name="noneBranch">None 及其它模式取值委托</param>
        /// <returns>对应设计模式下的存储值</returns>
        private T ResolveByPattern<T>(Func<T> mvvmBranch, Func<T> noneBranch)
        {
            return m_PatternType == PatternType.MVVM ? mvvmBranch() : noneBranch();
        }

        /// <summary>
        /// 按设计模式在 MVVM / None 两套存储之间赋值
        /// </summary>
        /// <param name="mvvmBranch">MVVM 模式赋值委托</param>
        /// <param name="noneBranch">None 及其它模式赋值委托</param>
        private void AssignByPattern(Action mvvmBranch, Action noneBranch)
        {
            if (m_PatternType == PatternType.MVVM)
            {
                mvvmBranch();
            }
            else
            {
                noneBranch();
            }
        }

        /// <summary>
        /// 设计模式类型
        /// </summary>
        [SerializeField]
        private PatternType m_PatternType;

        /// <summary>
        /// 预制体类型
        /// </summary>
        [SerializeField]
        private PrefabType m_PrefabType;
        public PrefabType PrefabType
        {
            get
            {
                return m_PrefabType;
            }
        }

        /// <summary>
        /// Lua 脚本公共名称（MVVM 模式）
        /// </summary>
        public string LuaScriptCommonName
        {
            get
            {
                return ResolveByPattern(() => m_LuaScriptCommonNameMVVM, () => string.Empty);
            }
        }

        /// <summary>
        /// Lua 脚本名称集合
        /// </summary>
        public List<string> LuaScriptNames
        {
            get
            {
                return ResolveByPattern(() => m_LuaScriptNamesMVVM, () => m_LuaScriptNamesNone);
            }
        }

        /// <summary>
        /// Lua 父类脚本名称集合
        /// </summary>
        public List<string> LuaSuperScriptNames
        {
            get
            {
                return ResolveByPattern(() => m_LuaSuperScriptNamesMVVM, () => m_LuaSuperScriptNamesNone);
            }
        }

        /// <summary>
        /// 是否启用 Proc 逻辑更新
        /// </summary>
        [SerializeField]
        private bool m_UseProc;
        public bool UseProc
        {
            set
            {
                m_UseProc = value;
            }
            get
            {
                return m_UseProc;
            }
        }

        /// <summary>
        /// 是否使用遮罩层
        /// </summary>
        [SerializeField]
        private bool m_MaskLayer;
        public bool MaskLayer
        {
            get
            {
                return m_MaskLayer;
            }
        }

        /// <summary>
        /// 是否使用关闭背景层
        /// </summary>
        [SerializeField]
        private bool m_BottomCloseLayer;
        public bool BottomCloseLayer
        {
            get
            {
                return m_BottomCloseLayer;
            }
        }

        /// <summary>
        /// 是否注入 2D 碰撞事件
        /// </summary>
        [SerializeField]
        private bool m_UseCollider2DLifeCycles;
        public bool UseCollider2DLifeCycles
        {
            get
            {
                return m_UseCollider2DLifeCycles;
            }
        }

        /// <summary>
        /// 是否注入 3D 碰撞事件
        /// </summary>
        [SerializeField]
        private bool m_UseCollider3DLifeCycles;
        public bool UseCollider3DLifeCycles
        {
            get
            {
                return m_UseCollider3DLifeCycles;
            }
        }

        /// <summary>
        /// 是否注入 2D 触发事件
        /// </summary>
        [SerializeField]
        private bool m_UseTrigger2DLifeCycles;
        public bool UseTrigger2DLifeCycles
        {
            get
            {
                return m_UseTrigger2DLifeCycles;
            }
        }

        /// <summary>
        /// 是否注入 3D 触发事件
        /// </summary>
        [SerializeField]
        private bool m_UseTrigger3DLifeCycles;
        public bool UseTrigger3DLifeCycles
        {
            get
            {
                return m_UseTrigger3DLifeCycles;
            }
        }

        /// <summary>
        /// 是否使用打开动画
        /// </summary>
        [SerializeField]
        private bool m_OpenAnimation;
        public bool OpenAnimation
        {
            set
            {
                m_OpenAnimation = value;
            }
            get
            {
                return m_OpenAnimation;
            }
        }

        /// <summary>
        /// 是否使用关闭动画
        /// </summary>
        [SerializeField]
        private bool m_CloseAnimation;
        public bool CloseAnimation
        {
            set
            {
                m_CloseAnimation = value;
            }
            get
            {
                return m_CloseAnimation;
            }
        }

        /// <summary>
        /// 是否绘制射线检测目标 Gizmo
        /// </summary>
        [SerializeField]
        private bool m_ShowRaycastTargetsGizmos;
        public bool ShowRaycastTargetsGizmos
        {
            get
            {
                return m_ShowRaycastTargetsGizmos;
            }
        }

        /// <summary>
        /// Gizmo 绘制颜色
        /// </summary>
        private Color m_RaycastTargetsGizmosColor = new Color(1.0f, 0.47f, 0.0f, 1.0f);

        /// <summary>
        /// 脚本作者
        /// </summary>
        [SerializeField]
        private string m_LuaAuthorName;

        /// <summary>
        /// 脚本描述
        /// </summary>
        [SerializeField]
        private string m_LuaDescript;

        /// <summary>
        /// 注入对象集合
        /// </summary>
        [SerializeField]
        private List<LuaInjection> m_Injections;
        public List<LuaInjection> Injections
        {
            get
            {
                return m_Injections;
            }
        }

        /// <summary>
        /// Lua 脚本名称集合
        /// </summary>
        private LuaTable m_LuaParams;

        public LuaTable LuaParams
        {
            set
            {
                m_LuaParams = value;
            }
            get
            {
                return m_LuaParams;
            }
        }

        /// <summary>
        /// Lua 独立环境表访问器（按设计模式返回对应环境）
        /// </summary>
        public LuaTable lua
        {
            get
            {
                return ResolveByPattern(
                    () => m_OwnLuaEnvsMVVM[(int)MVVMPatternType.View],
                    () => m_OwnLuaEnvsNone[(int)NonePatternType.Default]);
            }
        }

        /// <summary>
        /// 标准模式（None）下的 Lua Class 实例
        /// </summary>
        public LuaTable luaClass
        {
            get
            {
                return m_PatternType == PatternType.None ? m_OwnLuaClassesNone[(int)NonePatternType.Default] : null;
            }
        }

        /// <summary>
        /// MVVM 模式下的 View 层 Lua Class 实例
        /// </summary>
        public LuaTable luaClassView
        {
            get
            {
                return m_PatternType == PatternType.MVVM ? m_OwnLuaClassesMVVM[(int)MVVMPatternType.View] : null;
            }
        }

        /// <summary>
        /// MVVM 模式下的 ViewModel 层 Lua Class 实例
        /// </summary>
        public LuaTable luaClassViewModel
        {
            get
            {
                return m_PatternType == PatternType.MVVM ? m_OwnLuaClassesMVVM[(int)MVVMPatternType.ViewModel] : null;
            }
        }

        /// <summary>
        /// 可用的 Lua Class（优先标准模式，其次 MVVM View 层）
        /// </summary>
        public LuaTable ValidLuaClass
        {
            get
            {
                return luaClass != null ? luaClass : luaClassView;
            }
        }

        /// <summary>
        /// Lua 独立环境集合
        /// </summary>
        public LuaTable[] OwnLuaEnvs
        {
            set
            {
                AssignByPattern(() => m_OwnLuaEnvsMVVM = value, () => m_OwnLuaEnvsNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_OwnLuaEnvsMVVM, () => m_OwnLuaEnvsNone);
            }
        }

        /// <summary>
        /// Lua Class 集合
        /// </summary>
        public LuaTable[] OwnLuaClasses
        {
            set
            {
                AssignByPattern(() => m_OwnLuaClassesMVVM = value, () => m_OwnLuaClassesNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_OwnLuaClassesMVVM, () => m_OwnLuaClassesNone);
            }
        }

        #endregion

        //=========================================================================
        // 生命周期回调访问器
        //=========================================================================

        #region Lifecycle Accessors

        /// <summary>
        /// Lua 生命周期：Awake 回调数组（按设计模式转发到对应存储）
        /// </summary>
        public Action[] LuaAwakes
        {
            set
            {
                AssignByPattern(() => m_LuaAwakesMVVM = value, () => m_LuaAwakesNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_LuaAwakesMVVM, () => m_LuaAwakesNone);
            }
        }

        /// <summary>
        /// Lua 生命周期：OnEnable 回调数组
        /// </summary>
        public Action[] LuaOnEnables
        {
            set
            {
                AssignByPattern(() => m_LuaOnEnablesMVVM = value, () => m_LuaOnEnablesNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_LuaOnEnablesMVVM, () => m_LuaOnEnablesNone);
            }
        }

        /// <summary>
        /// Lua 生命周期：Start 回调数组
        /// </summary>
        public Action[] LuaStarts
        {
            set
            {
                AssignByPattern(() => m_LuaStartsMVVM = value, () => m_LuaStartsNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_LuaStartsMVVM, () => m_LuaStartsNone);
            }
        }

        /// <summary>
        /// Lua 自定义逻辑：Proc（逻辑帧更新）回调数组
        /// </summary>
        public Action[] LuaProcs
        {
            set
            {
                AssignByPattern(() => m_LuaProcsMVVM = value, () => m_LuaProcsNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_LuaProcsMVVM, () => m_LuaProcsNone);
            }
        }

        /// <summary>
        /// Lua 生命周期：OnDisable 回调数组
        /// </summary>
        public Action[] LuaOnDisables
        {
            set
            {
                AssignByPattern(() => m_LuaOnDisablesMVVM = value, () => m_LuaOnDisablesNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_LuaOnDisablesMVVM, () => m_LuaOnDisablesNone);
            }
        }

        /// <summary>
        /// Lua 生命周期：OnDestroy 回调数组
        /// </summary>
        public Action[] LuaOnDestroys
        {
            set
            {
                AssignByPattern(() => m_LuaOnDestroysMVVM = value, () => m_LuaOnDestroysNone = value);
            }
            get
            {
                return ResolveByPattern(() => m_LuaOnDestroysMVVM, () => m_LuaOnDestroysNone);
            }
        }

        #endregion

        // ==============================================
        // 碰撞/触发组件
        // ==============================================
        #region 碰撞/触发组件

        /// <summary>
        /// 2D 碰撞事件转发组件
        /// </summary>
        [SerializeField]
        private Collider2DLifeCyclesBehaviour m_Collider2DLifeCyclesBehaviour;

        /// <summary>
        /// 2D 碰撞事件转发组件（只读）
        /// </summary>
        public Collider2DLifeCyclesBehaviour Collider2DLifeCyclesBehaviour
        {
            get
            {
                return m_Collider2DLifeCyclesBehaviour;
            }
        }

        /// <summary>
        /// 3D 碰撞事件转发组件
        /// </summary>
        [SerializeField]
        private Collider3DLifeCyclesBehaviour m_Collider3DLifeCyclesBehaviour;

        /// <summary>
        /// 3D 碰撞事件转发组件（只读）
        /// </summary>
        public Collider3DLifeCyclesBehaviour Collider3DLifeCyclesBehaviour
        {
            get
            {
                return m_Collider3DLifeCyclesBehaviour;
            }
        }

        /// <summary>
        /// 2D 触发事件转发组件
        /// </summary>
        [SerializeField]
        private Trigger2DLifeCyclesBehaviour m_Trigger2DLifeCyclesBehaviour;

        /// <summary>
        /// 2D 触发事件转发组件（只读）
        /// </summary>
        public Trigger2DLifeCyclesBehaviour Trigger2DLifeCyclesBehaviour
        {
            get
            {
                return m_Trigger2DLifeCyclesBehaviour;
            }
        }

        /// <summary>
        /// 3D 触发事件转发组件
        /// </summary>
        [SerializeField]
        private Trigger3DLifeCyclesBehaviour m_Trigger3DLifeCyclesBehaviour;

        /// <summary>
        /// 3D 触发事件转发组件（只读）
        /// </summary>
        public Trigger3DLifeCyclesBehaviour Trigger3DLifeCyclesBehaviour
        {
            get
            {
                return m_Trigger3DLifeCyclesBehaviour;
            }
        }

        #endregion

        // ==============================================
        // 内部组件
        // ==============================================
        #region 内部组件

        /// <summary>
        /// UI 组件引用
        /// </summary>
        private UIComponent m_UIComponent;

        /// <summary>
        /// Lua 组件引用
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 生命周期标记
        /// </summary>
        private bool m_AwakeOver;

        /// <summary>
        /// 生命周期标记：OnEnable 是否已完成
        /// </summary>
        private bool m_EnableOver;

        /// <summary>
        /// 生命周期标记：Start 是否已完成
        /// </summary>
        private bool m_StartOver;

        /// <summary>
        /// Gizmo 绘制缓存
        /// </summary>
        private Vector3[] m_RaycastTargetWorldCornersOnDrawGizmos = new Vector3[4];

        #endregion
    }
}
