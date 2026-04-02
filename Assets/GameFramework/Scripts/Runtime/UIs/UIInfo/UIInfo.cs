using System.Collections.Generic;
using UnityEngine;
using XLua;
using static UnityEngine.UI.GraphicRaycaster;

namespace Honor.Runtime
{
    public class UIInfo
    {
        /// <summary>
        /// UI信息
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
        /// UI界面类型
        /// </summary>
        public UIType UIType { set; get; }

        /// <summary>
        /// AB路径
        /// </summary>
        public string ABPath { set; get; }

        /// <summary>
        /// Asset资源名称
        /// </summary>
        public string AssetName { set; get; }

        /// <summary>
        /// 是否为附加UI
        /// </summary>
        public bool IsAppend { set; get; }

        /// <summary>
        /// 是否为模态
        /// </summary>
        public bool IsModal { set; get; }

        /// <summary>
        /// 层级
        /// </summary>
        public int ZOrder { set; get; }

        /// <summary>
        /// 模态优先级
        /// </summary>
        public int Priority { set; get; }

        /// <summary>
        /// 原生返回按键抬起时是否关闭
        /// </summary>
        public bool CloseOnEscapeKeyUp { set; get; }

        /// <summary>
        /// 阻塞Raycaster的对象层
        /// </summary>
        public LayerMask BlockingMaskValue { set; get; }

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
        /// 阻塞Raycaster的对象类型
        /// </summary>
        public BlockingObjects BlockingObjects { set; get; }

        /// <summary>
        /// 多类型文本组件并存
        /// </summary>
        public bool MultiTypeTextCompsCoexist { set; get; }

        /// <summary>
        /// 自定义Lua传入参数
        /// </summary>
        public LuaTable LuaParams { set; get; }

        /// <summary>
        /// 异步加载完成回调
        /// </summary>
        public UILoadOverCallback OverCallback { set; get; }

        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="uiInfo"></param>
        /// <returns></returns>
        public bool Equals(UIInfo uiInfo)
        {
            return UIType == uiInfo.UIType && ABPath == uiInfo.ABPath && AssetName == uiInfo.AssetName &&
                   IsAppend == uiInfo.IsAppend;
        }
    }
}