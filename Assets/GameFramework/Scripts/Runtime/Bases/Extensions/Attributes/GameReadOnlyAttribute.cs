using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 可以在组件成员头用[HonorReadOnly]的方式标注
    /// 被该标签修饰的属性值将不可编辑
    /// </summary>
    public class GameReadOnlyAttribute : PropertyAttribute
    {
    }
}


