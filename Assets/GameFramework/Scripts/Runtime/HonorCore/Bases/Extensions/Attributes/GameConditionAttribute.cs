/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameConditionAttribute.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏框架条件显示特性 - 编辑器Inspector面板动态显示/隐藏字段
 *            根据布尔变量值控制属性可见性，支持隐藏/变灰两种模式
 ***************************************************************/

using System;
using UnityEngine;

namespace Honor.Runtime
{
    #region 游戏框架条件显示特性
    /// <summary>
    /// 游戏框架条件显示特性
    /// </summary>
    /// <remarks>Honor 框架编辑器扩展专用，用于在 Inspector 面板根据布尔变量值动态显示/隐藏字段</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class GameConditionAttribute : PropertyAttribute
    {
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 条件判断布尔变量名称
        /// </summary>
        /// <remarks>必须为当前类中的 bool 类型成员变量</remarks>
        public string ConditionBoolean { get; private set; }

        /// <summary>
        /// 不满足条件时的显示模式
        /// </summary>
        /// <remarks>true=隐藏字段，false=字段变灰不可编辑</remarks>
        public bool Hidden { get; private set; }

        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 条件显示特性构造函数
        /// </summary>
        /// <param name="conditionBoolean">用于判断的布尔变量名称</param>
        /// <remarks>默认不满足条件时字段变灰（不隐藏）</remarks>
        public GameConditionAttribute(string conditionBoolean)
        {
            ConditionBoolean = conditionBoolean;
            Hidden = false;
        }

        /// <summary>
        /// 条件显示特性构造函数
        /// </summary>
        /// <param name="conditionBoolean">用于判断的布尔变量名称</param>
        /// <param name="hideInInspector">不满足条件时是否在Inspector中隐藏</param>
        public GameConditionAttribute(string conditionBoolean, bool hideInInspector)
        {
            ConditionBoolean = conditionBoolean;
            Hidden = hideInInspector;
        }
    }
    #endregion
}