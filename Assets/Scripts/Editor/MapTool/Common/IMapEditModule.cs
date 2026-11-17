/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  IMapEditModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器编辑模块统一契约（初始化 / 显隐 / 激活），
 *            主窗口按 Tab→模块注册表驱动，不再逐个模块硬编码切换分支
 ***************************************************************/

using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器编辑模块统一契约
    /// 场景 / 物品元件 / 迷雾等并列模式面板均实现本接口，
    /// 主窗口只依赖该契约完成装配与切换，模块内部实现互不感知。
    /// </summary>
    public interface IMapEditModule
    {
        /// <summary>
        /// 初始化模块：查找元素并绑定事件
        /// </summary>
        /// <param name="root">模块根节点</param>
        void Init(VisualElement root);

        /// <summary>
        /// 显示模块
        /// </summary>
        void Show();

        /// <summary>
        /// 隐藏模块
        /// </summary>
        void Hide();
    }
}
