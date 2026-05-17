/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIFlagBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI 标识行为组件 - 每个UI实例必备标记类
 ***************************************************************/
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 标识行为组件
    /// 挂载在所有 UI 实例上，用于标记、管理、关联 UI 所需的核心数据
    /// 是 UI 框架识别界面的核心标记
    /// </summary>
    public sealed class UIFlagBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Lua 逻辑脚本组件（XLua 绑定）
        /// </summary>
        public LuaBehaviour LuaBehaviour;

        /// <summary>
        /// 预制体实例化通用组件（资源加载、实例化管理）
        /// </summary>
        public PrefabInstanceGOBehaviour PrefabInstanceGOBehaviour;

        /// <summary>
        /// UI 配置信息（AB路径、名称、参数、回调等）
        /// </summary>
        public UIInfo UIInfo;

        /// <summary>
        /// 是否跟随父对象自动销毁
        /// 说明：仅控制 GameObject.Destroy，不影响 UI 管理器的关闭/队列逻辑
        /// </summary>
        public bool FollowParentDestroy;
    }
}