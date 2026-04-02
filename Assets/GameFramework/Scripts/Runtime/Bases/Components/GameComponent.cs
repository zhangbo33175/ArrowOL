
using UnityEngine;

namespace Honor.Runtime
{
    public abstract class GameComponent : MonoBehaviour
    {
        /// <summary>
        /// 第一时间注册游戏框架组件
        /// </summary>
        protected virtual void Awake()
        {
            GameComponentsGroup.RegisterComponent(this);
        }
    }
}


