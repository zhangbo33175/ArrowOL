using UnityEngine;

namespace Honor.Runtime
{
    public sealed class UIFlagBehaviour : MonoBehaviour
    {
        /// <summary>
        /// LuaBehaviour组件
        /// </summary>
        public LuaBehaviour LuaBehaviour;

        /// <summary>
        /// Prefab实例GO通用组件
        /// </summary>
        public PrefabInstanceGOBehaviour PrefabInstanceGOBehaviour;

        /// <summary>
        /// UI信息
        /// </summary>
        public UIInfo UIInfo;

        /// <summary>
        /// 跟随父级销毁
        /// 仅对GameObject.Destroy进行控制，UI列表及父子Close逻辑不受影响。（默认不做赋值）
        /// </summary>
        public bool FollowParentDestroy;

    }

}


