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
    public sealed partial class TableComponent : GameComponent
    {
        //=========================================================================
        // 生命周期函数
        //=========================================================================
        #region 生命周期

        /// <summary>
        /// 初始化组件（C#层无业务逻辑）
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // C# 层目前无运行时逻辑
            // 1. 编辑器下的 Inspector 扩展逻辑已移除
            // 2. 策划表解析、VBA 导出逻辑全部转移至 Lua 层
            // 3. 运行时表格读取也由 Lua 统一管理
        }

        /// <summary>
        /// 启动（C#层无业务逻辑）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 销毁（C#层无业务逻辑）
        /// </summary>
        private void OnDestroy()
        {
        }

        #endregion
    }
}