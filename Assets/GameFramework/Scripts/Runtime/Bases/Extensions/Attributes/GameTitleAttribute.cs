using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 可以在组件成员头用[HonorTitle("XXXXX")]的方式标注
    /// 根据类名的定义格式决定了使用时的标注格式
    /// </summary>
    public class GameTitleAttribute : PropertyAttribute
    {
        public string Title { get; private set; }
        public GameTitleAttribute(string name)
        {
            Title = name;
        }
    }
}


