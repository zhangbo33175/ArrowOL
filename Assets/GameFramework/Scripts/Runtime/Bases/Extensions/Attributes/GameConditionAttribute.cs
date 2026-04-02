using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 可以在组件成员头用[HonorConditionAttribute("XXXXX")]的方式标注
    /// XXXX为当前类中Boolean类型的成员变量
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class GameConditionAttribute : PropertyAttribute
    {
        public string ConditionBoolean = "";
        public bool Hidden = false;

        public GameConditionAttribute(string conditionBoolean)
        {
            this.ConditionBoolean = conditionBoolean;
            this.Hidden = false;
        }

        public GameConditionAttribute(string conditionBoolean, bool hideInInspector)
        {
            this.ConditionBoolean = conditionBoolean;
            this.Hidden = hideInInspector;
        }
    }
}


