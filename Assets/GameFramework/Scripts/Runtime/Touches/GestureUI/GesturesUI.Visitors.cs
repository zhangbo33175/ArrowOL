#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed partial class GesturesUI : MonoBehaviour
    {
        /// <summary>
        /// UI相机在UI相机列表中的index
        /// </summary>
        [SerializeField]
        [HonorTitle("UI相机索引值")]
        private int m_UICameraIndex;
        public int UICameraIndex
        {
            set
            {
                if (value < 0 || value >= Root.UI.ScreenUICameras.Count)
                {
                    Log.Fatal("UICameraIndex 无效。");
                    return;
                }
                m_UICameraIndex = value;
                m_UICamera = Root.UI.ScreenUICameras[m_UICameraIndex];
            }
            get
            {
                return m_UICameraIndex;
            }
        }

        /// <summary>
        /// 总开关
        /// </summary>
        [SerializeField]
        [HonorTitle("总开关 (EnableSwitch)")]
        private bool m_EnableSwitch;
        public bool EnableSwitch
        {
            set
            {
                m_EnableSwitch = value;
                enabled = m_EnableSwitch;
            }
            get
            {
                return m_EnableSwitch;
            }
        }

        /// <summary>
        ///【UI】UI开关
        /// </summary>
        [SerializeField]
        [HonorTitle("UI开关 (UISwitch)")]
        private bool m_UISwitch;
        public bool UISwitch
        {
            set
            {
                m_UISwitch = value;
            }
            get
            {
                return m_UISwitch;
            }
        }

        /// <summary>
        ///【选中】选中开关
        /// </summary>
        [SerializeField]
        [HonorTitle("选中开关 (SelectSwitch)")]
        private bool m_SelectSwitch;
        public bool SelectSwitch
        {
            set
            {
                m_SelectSwitch = value;
            }
            get
            {
                return m_SelectSwitch;
            }
        }

        /// <summary>
        ///【拖拽】拖拽开关
        /// </summary>
        [SerializeField]
        [HonorTitle("拖拽开关 (DragSwitch)")]
        private bool m_DragSwitch;
        public bool DragSwitch
        {
            set
            {
                m_DragSwitch = value;
            }
            get
            {
                return m_DragSwitch;
            }
        }

        /// <summary>
        /// 常选中模式
        /// 常选中模式：具体流程：选中->开始拖拽->拖拽中->结束拖拽->切换目标->结束选中
        /// 非常选中模式：具体流程：选中->开始拖拽->拖拽中->结束拖拽->结束选中
        /// </summary>
        [SerializeField]
        [HonorTitle("常选中模式（SelectHoldMode）")]
        private bool m_SelectHoldMode;
        public bool SelectHoldMode
        {
            set
            {
                m_SelectHoldMode = value;
            }
            get
            {
                return m_SelectHoldMode;
            }
        }

        /// <summary>
        /// 常选中模式下选中反弹
        /// 常选中模式下：当对象处于被选中状态时，再次点击可以将选中状态反弹回未选中状态
        /// </summary>
        [SerializeField]
        [HonorTitle("常选中模式下选中反弹机制（SelectReboundInSelectHoldMode）")]
        private bool m_SelectReboundInSelectHoldMode;
        public bool SelectReboundInSelectHoldMode
        {
            set
            {
                m_SelectReboundInSelectHoldMode = value;
            }
            get
            {
                return m_SelectReboundInSelectHoldMode;
            }
        }

        /// <summary>
        /// 常选中模式下拖拽结束后反弹
        /// 常选中模式下：当对象拖拽结束时，可以将选中状态反弹回未选中状态
        /// </summary>
        [SerializeField]
        [HonorTitle("常选中模式下拖拽结束后反弹机制（SelectReboundAfterDragEndInSelectHoldMode）")]
        private bool m_SelectReboundAfterDragEndInSelectHoldMode;
        public bool SelectReboundAfterDragEndInSelectHoldMode
        {
            set
            {
                m_SelectReboundAfterDragEndInSelectHoldMode = value;
            }
            get
            {
                return m_SelectReboundAfterDragEndInSelectHoldMode;
            }
        }

        /// <summary>
        /// 选中对象时所需按压等待时长
        /// </summary>
        [SerializeField]
        [HonorTitle("选中对象时所需按压等待时长 (PressTimeOfSelectingObj)")]
        private float m_PressTimeOfSelectingObj = 0f;
        public float PressTimeOfSelectingObj
        {
            set
            {
                m_PressTimeOfSelectingObj = value;
            }
            get
            {
                return m_PressTimeOfSelectingObj;
            }
        }

        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名集合
        /// 必须按照响应优先级从高到底的顺序设置
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名 (SelectingTypes)")]
        private List<string> m_SelectingTypes;
        public List<string> SelectingTypes
        {
            set
            {
                m_SelectingTypes = value;
            }
            get
            {
                return m_SelectingTypes;
            }
        }

        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名-在自身中查询"
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名-在自身中查询 (FindSelectingTypesOnSelf)")]
        private bool m_FindSelectingTypesOnSelf = false;
        public bool FindSelectingTypesOnSelf
        {
            set
            {
                m_FindSelectingTypesOnSelf = value;
            }
            get
            {
                return m_FindSelectingTypesOnSelf;
            }
        }


        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名-在父对象中查询
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名-在父对象中查询 (FindSelectingTypesOnParent)")]
        private bool m_FindSelectingTypesOnParent = false;
        public bool FindSelectingTypesOnParent
        {
            set
            {
                m_FindSelectingTypesOnParent = value;
            }
            get
            {
                return m_FindSelectingTypesOnParent;
            }
        }

        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名-在子对象中查询
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名-在子对象中查询 (FindSelectingTypesOnChildren)")]
        private bool m_FindSelectingTypesOnChildren = false;
        public bool FindSelectingTypesOnChildren
        {
            set
            {
                m_FindSelectingTypesOnChildren = value;
            }
            get
            {
                return m_FindSelectingTypesOnChildren;
            }
        }

        /// <summary>
        /// Lua组件
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// UI相机
        /// </summary>
        private Camera m_UICamera;
        public Camera UICamera
        {
            get
            {
                return m_UICamera;
            }
        }

        /// <summary>
        /// 当前选中的对象
        /// </summary>
        private GameObject m_SelectedObj;

        /// <summary>
        /// 当前选中的对象的Lua类型
        /// </summary>
        private string m_SelectedObjType;

        /// <summary>
        /// 当前选中的对象的手势
        /// </summary>
        private Gesture m_SelectedObjGesture;

        /// <summary>
        /// 是否可以进入反弹状态
        /// </summary>
        private bool m_CanEnterSelectReboundInSelectHoldMode = true;

        /// <summary>
        /// 正否正在拖拽中
        /// </summary>
        private bool m_IsDraging = false;

        /// <summary>
        /// 本轮拖拽状态
        /// </summary>
        private DragState m_CurDragStateOnThisRound = DragState.None;

        /// <summary>
        /// 上一次拖拽Obj手势所在世界坐标
        /// </summary>
        private Vector3 m_LastWorldPosition = Vector3.zero;

    }
}

#endif