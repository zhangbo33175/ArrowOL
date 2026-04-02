using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {
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
        /// Lua脚本公有名称
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
        /// Lua脚本名称集合
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
        /// Lua父类脚本名称集合
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
        /// 使用Proc
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
        /// 使用前后遮罩层
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
        /// 使用关闭背景层
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
        /// 注入Collider2D生命周期函数
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
        /// 注入Collider3D生命周期函数
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
        /// 注入Trigger2D生命周期函数
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
        /// 注入Trigger3D生命周期函数
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
        /// 使用打开动画
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
        /// 使用关闭动画
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
        /// 绘制Gizmo信息(RaycastTargets)
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
        /// 绘制Gizmo信息颜色(RaycastTargets)
        /// </summary>
        private Color m_RaycastTargetsGizmosColor = new Color(1.0f, 0.47f, 0.0f, 1.0f);

        /// <summary>
        /// Lua脚本作者名称
        /// </summary>
        [SerializeField]
        private string m_LuaAuthorName;

        /// <summary>
        /// Lua脚本描述信息
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
        /// lua自定义传入参数
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
        /// 公开暴露的cs层的lua环境
        /// 可以在lua层中通过cs.lua来访问当前挂载的Lua环境
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
        /// 公开暴露的cs层的lua脚本class环境（设计模式：默认）
        /// </summary>
        public LuaTable luaClass
        {
            get
            {
                return m_PatternType == PatternType.None ? m_OwnLuaClassesNone[(int)NonePatternType.Default] : null;
            }
        }

        /// <summary>
        /// 公开暴露的cs层的lua脚本ViewClass环境（设计模式：MVVM）
        /// </summary>
        public LuaTable luaClassView
        {
            get
            {
                return m_PatternType == PatternType.MVVM ? m_OwnLuaClassesMVVM[(int)MVVMPatternType.View] : null;
            }
        }

        /// <summary>
        /// 公开暴露的cs层的lua脚本ViewModelClass环境（设计模式：MVVM）
        /// </summary>
        public LuaTable luaClassViewModel
        {
            get
            {
                return m_PatternType == PatternType.MVVM ? m_OwnLuaClassesMVVM[(int)MVVMPatternType.ViewModel] : null;
            }
        }

        /// <summary>
        /// 获取公开暴露的cs层有效的lua脚本class环境
        /// </summary>
        public LuaTable ValidLuaClass
        {
            get
            {
                return luaClass != null ? luaClass : luaClassView;
            }
        }

        /// <summary>
        /// Lua脚本独立环境集合
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
        /// Lua脚本class环境集合
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

        /// <summary>
        /// Lua自定义生命周期函数：Awake
        /// </summary>
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

        /// <summary>
        /// Lua自定义生命周期函数：OnEnable
        /// </summary>
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

        /// <summary>
        /// Lua自定义生命周期函数：Start
        /// </summary>
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

        /// <summary>
        /// Lua自定义生命周期函数：Proc
        /// </summary>
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

        /// <summary>
        /// Lua自定义生命周期函数：OnDisable
        /// </summary>
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

        /// <summary>
        /// Lua自定义生命周期函数：Destroy
        /// </summary>
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

        /// <summary>
        /// Collider2D碰撞盒生命周期函数组件
        /// </summary>
        [SerializeField]
        private Collider2DLifeCyclesBehaviour m_Collider2DLifeCyclesBehaviour;
        public Collider2DLifeCyclesBehaviour Collider2DLifeCyclesBehaviour
        {
            get
            {
                return m_Collider2DLifeCyclesBehaviour;
            }
        }

        /// <summary>
        /// Collider3D碰撞盒生命周期函数组件
        /// </summary>
        [SerializeField]
        private Collider3DLifeCyclesBehaviour m_Collider3DLifeCyclesBehaviour;
        public Collider3DLifeCyclesBehaviour Collider3DLifeCyclesBehaviour
        {
            get
            {
                return m_Collider3DLifeCyclesBehaviour;
            }
        }

        /// <summary>
        /// Trigger2D触发器生命周期函数组件
        /// </summary>
        [SerializeField]
        private Trigger2DLifeCyclesBehaviour m_Trigger2DLifeCyclesBehaviour;
        public Trigger2DLifeCyclesBehaviour Trigger2DLifeCyclesBehaviour
        {
            get
            {
                return m_Trigger2DLifeCyclesBehaviour;
            }
        }


        /// <summary>
        /// Trigger3D触发器生命周期函数组件
        /// </summary>
        [SerializeField]
        private Trigger3DLifeCyclesBehaviour m_Trigger3DLifeCyclesBehaviour;
        public Trigger3DLifeCyclesBehaviour Trigger3DLifeCyclesBehaviour
        {
            get
            {
                return m_Trigger3DLifeCyclesBehaviour;
            }
        }

        /// <summary>
        /// UI组件
        /// </summary>
        private UIComponent m_UIComponent;

        /// <summary>
        /// Lua组件
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 标记Awake是否调用结束
        /// </summary>
        private bool m_AwakeOver;

        /// <summary>
        /// 标记启动时Enable是否调用结束
        /// </summary>
        private bool m_EnableOver;

        /// <summary>
        /// 标记Start是否调用结束
        /// </summary>
        private bool m_StartOver;

        /// <summary>
        /// Gizmos绘制过程中RaycastTargets对象的四角坐标
        /// </summary>
        private Vector3[] m_RaycastTargetWorldCornersOnDrawGizmos = new Vector3[4];

    }
}


