/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviour.Properties.cs
 * author:    云毅
 * created:   2026 2025
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
                switch(m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaScriptCommonNameMVVM;
                    case PatternType.None: return string.Empty;
                    default:return string.Empty;
                }
            }
        }

        /// <summary>
        /// Lua 脚本名称集合
        /// </summary>
        public List<string> LuaScriptNames
        {
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaScriptNamesMVVM;
                    case PatternType.None: return m_LuaScriptNamesNone;
                    default: return m_LuaScriptNamesNone;
                }
            }
        }

        /// <summary>
        /// Lua 父类脚本名称集合
        /// </summary>
        public List<string> LuaSuperScriptNames
        {
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaSuperScriptNamesMVVM;
                    case PatternType.None: return m_LuaSuperScriptNamesNone;
                    default: return m_LuaSuperScriptNamesNone;
                }
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
        /// Lua 父类脚本名称集合
        /// </summary>
        public LuaTable lua
        {
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_OwnLuaEnvsMVVM[(int)MVVMPatternType.View];
                    case PatternType.None: return m_OwnLuaEnvsNone[(int)NonePatternType.Default];
                    default: return m_OwnLuaEnvsNone[(int)NonePatternType.Default];
                }
            }
        }

        /// <summary>
        /// 是否启用 Proc 逻辑更新
        /// </summary>
        public LuaTable luaClass
        {
            get
            {
                return m_PatternType == PatternType.None ? m_OwnLuaClassesNone[(int)NonePatternType.Default] : null;
            }
        }

        /// <summary>
        /// 是否使用遮罩层
        /// </summary>
        public LuaTable luaClassView
        {
            get
            {
                return m_PatternType == PatternType.MVVM ? m_OwnLuaClassesMVVM[(int)MVVMPatternType.View] : null;
            }
        }

        /// <summary>
        /// 是否使用关闭动画
        /// </summary>
        public LuaTable luaClassViewModel
        {
            get
            {
                return m_PatternType == PatternType.MVVM ? m_OwnLuaClassesMVVM[(int)MVVMPatternType.ViewModel] : null;
            }
        }

        /// <summary>
        /// 是否绘制射线检测目标 Gizmo
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
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_OwnLuaEnvsMVVM = value; break;
                    case PatternType.None: m_OwnLuaEnvsNone = value; break;
                    default: m_OwnLuaEnvsNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_OwnLuaEnvsMVVM;
                    case PatternType.None: return m_OwnLuaEnvsNone;
                    default: return m_OwnLuaEnvsNone;
                }
            }
        }

        /// <summary>
        /// Lua Class 集合
        /// </summary>
        public LuaTable[] OwnLuaClasses
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_OwnLuaClassesMVVM = value; break;
                    case PatternType.None: m_OwnLuaClassesNone = value; break;
                    default: m_OwnLuaClassesNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_OwnLuaClassesMVVM;
                    case PatternType.None: return m_OwnLuaClassesNone;
                    default: return m_OwnLuaClassesNone;
                }
            }
        }

        #endregion

        //=========================================================================
        // 生命周期回调访问器
        //=========================================================================

        #region Lifecycle Accessors

        public Action[] LuaAwakes
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_LuaAwakesMVVM = value; break;
                    case PatternType.None: m_LuaAwakesNone = value; break;
                    default: m_LuaAwakesNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaAwakesMVVM;
                    case PatternType.None: return m_LuaAwakesNone;
                    default: return m_LuaAwakesNone;
                }
            }
        }

        public Action[] LuaOnEnables
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_LuaOnEnablesMVVM = value; break;
                    case PatternType.None: m_LuaOnEnablesNone = value; break;
                    default: m_LuaOnEnablesNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaOnEnablesMVVM;
                    case PatternType.None: return m_LuaOnEnablesNone;
                    default: return m_LuaOnEnablesNone;
                }
            }
        }

        public Action[] LuaStarts
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_LuaStartsMVVM = value; break;
                    case PatternType.None: m_LuaStartsNone = value; break;
                    default: m_LuaStartsNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaStartsMVVM;
                    case PatternType.None: return m_LuaStartsNone;
                    default: return m_LuaStartsNone;
                }
            }
        }

        public Action[] LuaProcs
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_LuaProcsMVVM = value; break;
                    case PatternType.None: m_LuaProcsNone = value; break;
                    default: m_LuaProcsNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaProcsMVVM;
                    case PatternType.None: return m_LuaProcsNone;
                    default: return m_LuaProcsNone;
                }
            }
        }

        public Action[] LuaOnDisables
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_LuaOnDisablesMVVM = value; break;
                    case PatternType.None: m_LuaOnDisablesNone = value; break;
                    default: m_LuaOnDisablesNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaOnDisablesMVVM;
                    case PatternType.None: return m_LuaOnDisablesNone;
                    default: return m_LuaOnDisablesNone;
                }
            }
        }

        public Action[] LuaOnDestroys
        {
            set
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: m_LuaOnDestroysMVVM = value; break;
                    case PatternType.None: m_LuaOnDestroysNone = value; break;
                    default: m_LuaOnDestroysNone = value; break;
                }
            }
            get
            {
                switch (m_PatternType)
                {
                    case PatternType.MVVM: return m_LuaOnDestroysMVVM;
                    case PatternType.None: return m_LuaOnDestroysNone;
                    default: return m_LuaOnDestroysNone;
                }
            }
        }

        #endregion

        // ==============================================
        // 碰撞/触发组件
        // ==============================================
        [SerializeField]
        private Collider2DLifeCyclesBehaviour m_Collider2DLifeCyclesBehaviour;
        public Collider2DLifeCyclesBehaviour Collider2DLifeCyclesBehaviour
        {
            get
            {
                return m_Collider2DLifeCyclesBehaviour;
            }
        }

        [SerializeField]
        private Collider3DLifeCyclesBehaviour m_Collider3DLifeCyclesBehaviour;
        public Collider3DLifeCyclesBehaviour Collider3DLifeCyclesBehaviour
        {
            get
            {
                return m_Collider3DLifeCyclesBehaviour;
            }
        }

        [SerializeField]
        private Trigger2DLifeCyclesBehaviour m_Trigger2DLifeCyclesBehaviour;
        public Trigger2DLifeCyclesBehaviour Trigger2DLifeCyclesBehaviour
        {
            get
            {
                return m_Trigger2DLifeCyclesBehaviour;
            }
        }

        [SerializeField]
        private Trigger3DLifeCyclesBehaviour m_Trigger3DLifeCyclesBehaviour;
        public Trigger3DLifeCyclesBehaviour Trigger3DLifeCyclesBehaviour
        {
            get
            {
                return m_Trigger3DLifeCyclesBehaviour;
            }
        }

        // ==============================================
        // 内部组件
        // ==============================================
        private UIComponent m_UIComponent;
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 生命周期标记
        /// </summary>
        private bool m_AwakeOver;

        private bool m_EnableOver;
        private bool m_StartOver;

        /// <summary>
        /// Gizmo 绘制缓存
        /// </summary>
        private Vector3[] m_RaycastTargetWorldCornersOnDrawGizmos = new Vector3[4];
    }
}