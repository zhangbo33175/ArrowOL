using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架条件显示特性（Honor 框架编辑器扩展专用）
    /// 用于在 Inspector 面板根据布尔变量值，动态显示/隐藏字段
    /// 用法：[GameConditionAttribute("布尔变量名")] 或 [GameCondition("布尔变量名")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class GameConditionAttribute : PropertyAttribute
    {
        /// <summary>
        /// 用于判断的布尔变量名称（必须是当前类中的 bool 成员）
        /// </summary>
        public string ConditionBoolean { get; private set; }

        /// <summary>
        /// 不满足条件时是否隐藏（true=隐藏，false=变灰不可编辑）
        /// </summary>
        public bool Hidden { get; private set; }

        /// <summary>
        /// 构造函数 - 默认不满足条件时变灰（不隐藏）
        /// </summary>
        /// <param name="conditionBoolean">用于判断的布尔变量名称</param>
        public GameConditionAttribute(string conditionBoolean)
        {
            ConditionBoolean = conditionBoolean;
            Hidden = false;
        }

        /// <summary>
        /// 构造函数 - 自定义不满足条件时的行为
        /// </summary>
        /// <param name="conditionBoolean">用于判断的布尔变量名称</param>
        /// <param name="hideInInspector">不满足条件时是否隐藏</param>
        public GameConditionAttribute(string conditionBoolean, bool hideInInspector)
        {
            ConditionBoolean = conditionBoolean;
            Hidden = hideInInspector;
        }
    }
}