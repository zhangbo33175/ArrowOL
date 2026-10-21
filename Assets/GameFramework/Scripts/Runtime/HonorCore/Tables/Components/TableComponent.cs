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
    public sealed partial class TableComponent 
    {
        //=========================================================================
        // 生命周期函数
        //=========================================================================
        #region 生命周期

        /// <summary>
        /// 初始化组件（C#层无业务逻辑）
        /// <para>由门面管理器（HonorCoreManager）阶段2 统一调用，表格实际解析在 Lua 层</para>
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 释放组件（C#层无业务资源，实际表格数据由Lua层持有；实例置空由门面统一处理）
        /// </summary>
        public void Dispose()
        {
            // C# 层无业务资源，Lua 侧持有实际表格数据；实例置空由门面统一处理
        }

        #endregion
    }
}