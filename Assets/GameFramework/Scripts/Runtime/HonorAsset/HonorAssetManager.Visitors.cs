/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorAssetManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源模块门面管理器 - 便捷状态与访问（partial）
 *            提供门面就绪状态等便捷属性，提高调用可读性
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class HonorAssetManager
    {
        //=========================================================================

        #region 组件引用

        //=========================================================================

        /// <summary>
        /// 资源管理组件（AB包、资源同步/异步加载、卸载、场景、预制体实例化）
        /// </summary>
        public AssetComponent Asset { get; private set; }

        #endregion

        //=========================================================================

        #region 便捷状态与访问

        //=========================================================================

        /// <summary>
        /// 资源组件是否已就绪
        /// </summary>
        public bool IsAssetReady => Asset != null;

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

        /// <summary>
        /// 资源严格校验开关（双向，转发自 AssetComponent）
        /// </summary>
        public bool StrictCheck
        {
            get { return Asset != null && Asset.StrictCheck; }
            set { if (Asset != null) Asset.StrictCheck = value; }
        }

        /// <summary>
        /// 全局 AB 包依赖关系映射表（转发自 AssetComponent）
        /// </summary>
        public Dictionary<string, string[]> DependsDataList => Asset != null ? Asset.DependsDataList : null;

        /// <summary>
        /// 底层资源加载管理器实例（转发自 AssetComponent）
        /// </summary>
        public AssetLoadManager AssetLoadManager => Asset != null ? Asset.AssetLoadManager : null;

        /// <summary>
        /// 资源组件根节点（Lua 绑定/动态物体挂载父节点，转发自 AssetComponent.transform）
        /// </summary>
        public Transform AssetRootTransform => Asset != null ? Asset.transform : null;

        #endregion
    }
}
