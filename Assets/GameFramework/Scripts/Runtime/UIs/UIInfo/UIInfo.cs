using System.Collections.Generic;
using UnityEngine;
using XLua;
using static UnityEngine.UI.GraphicRaycaster;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 打开所需的配置信息数据类
    /// 用于统一封装 UI 的路径、层级、模态、遮罩、回调等参数
    /// </summary>
    public class UIInfo
    {
        /// <summary>
        /// 默认构造函数，初始化所有 UI 参数默认值
        /// </summary>
        public UIInfo()
        {
            UIType = UIType.None;
            ABPath = null;
            AssetName = null;
            IsAppend = false;
            IsModal = false;
            ZOrder = 0;
            Priority = -1;
            CloseOnEscapeKeyUp = false;
            BlockingMaskValue = -1;
            BlockingMask = "Everything";
            BlockingObjects = BlockingObjects.None;
            MultiTypeTextCompsCoexist = true;
            LuaParams = null;
            OverCallback = null;
        }

        /// <summary>
        /// UI 界面类型（Screen / World / 其他）
        /// </summary>
        public UIType UIType { set; get; }

        /// <summary>
        /// UI 所在的 AssetBundle 路径
        /// </summary>
        public string ABPath { set; get; }

        /// <summary>
        /// UI 预制体名称
        /// </summary>
        public string AssetName { set; get; }

        /// <summary>
        /// 是否为附加型 UI（依附于父界面，不单独管理）
        /// </summary>
        public bool IsAppend { set; get; }

        /// <summary>
        /// 是否为模态窗口（会阻塞下层交互）
        /// </summary>
        public bool IsModal { set; get; }

        /// <summary>
        /// UI 显示层级（Sorting Order）
        /// </summary>
        public int ZOrder { set; get; }

        /// <summary>
        /// 模态 UI 优先级，优先级高的后关闭、先显示
        /// </summary>
        public int Priority { set; get; }

        /// <summary>
        /// 按下手机返回键 / ESC 键时是否自动关闭
        /// </summary>
        public bool CloseOnEscapeKeyUp { set; get; }

        /// <summary>
        /// 阻塞射线检测的层级掩码（最终计算值）
        /// </summary>
        public LayerMask BlockingMaskValue { set; get; }

        /// <summary>
        /// 阻塞层级名称（如：Everything / UI / Character），自动转为 LayerMask
        /// </summary>
        public string BlockingMask
        {
            set
            {
                List<string> layerMaskNames = new List<string>(value.Split('|'));
                if (layerMaskNames.Contains("Everything"))
                {
                    return;
                }
                else if (layerMaskNames.Contains("Nothing"))
                {
                    BlockingMaskValue = 0;
                }
                else
                {
                    BlockingMaskValue = LayerMask.GetMask(layerMaskNames.ToArray());
                }
            }
        }

        /// <summary>
        /// 射线阻塞对象类型（2D / 3D / 无）
        /// </summary>
        public BlockingObjects BlockingObjects { set; get; }

        /// <summary>
        /// 是否允许 Text 和 TextMeshPro 组件共存
        /// </summary>
        public bool MultiTypeTextCompsCoexist { set; get; }

        /// <summary>
        /// 传递给 Lua 脚本的自定义参数表
        /// </summary>
        public LuaTable LuaParams { set; get; }

        /// <summary>
        /// UI 异步加载并实例化完成后的回调
        /// </summary>
        public UILoadOverCallback OverCallback { set; get; }

        /// <summary>
        /// 判断两个 UIInfo 是否等价（根据类型、路径、名称、附加状态）
        /// </summary>
        /// <param name="uiInfo">待比较的 UIInfo</param>
        /// <returns>是否相等</returns>
        public bool Equals(UIInfo uiInfo)
        {
            return UIType == uiInfo.UIType 
                   && ABPath == uiInfo.ABPath 
                   && AssetName == uiInfo.AssetName 
                   && IsAppend == uiInfo.IsAppend;
        }
    }
}