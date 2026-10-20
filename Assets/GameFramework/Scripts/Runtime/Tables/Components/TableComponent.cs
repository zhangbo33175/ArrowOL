/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TableComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   表格配置组件 - 游戏策划表/Excel表管理载体组件
 *            C#仅做组件承载，实际逻辑全部由Lua层实现
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 表格配置组件（负责游戏策划表/Excel表的加载与管理）
    /// 归属：GameFramework 游戏核心组件
    /// 说明：C# 层仅作为组件载体，实际表格解析、VBA 生成、配置读取均转移至 Lua 层
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class TableComponent : Singleton<TableComponent>
    {
        //=========================================================================
        // 生命周期函数
        //=========================================================================
        #region 生命周期

        /// <summary>
        /// 初始化组件（C#层无业务逻辑）
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        #endregion

        /// <summary>
        /// 释放组件（C#层未实现，实际资源回收由Lua层驱动）
        /// </summary>
        /// <exception cref="System.NotImplementedException">始终抛出，提示调用方走Lua释放路径</exception>
        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}