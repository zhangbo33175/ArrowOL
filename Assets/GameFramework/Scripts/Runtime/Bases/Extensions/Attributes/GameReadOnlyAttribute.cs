using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义只读特性
    /// 标记在序列化字段上，使其在 Inspector 面板仅展示、禁止编辑
    /// 配合自定义编辑器绘制实现全局只读效果
    /// </summary>
    public class GameReadOnlyAttribute : PropertyAttribute
    {

    }
}