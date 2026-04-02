using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class TableComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            // C#层暂无逻辑，存在Inspector定制界面逻辑，全部转移到Lua层与Excel的VBA逻辑

        }

        private void Start()
        {

        }

        private void OnDestroy()
        {

        }

    }

}


