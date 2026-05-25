/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EGameMode.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏运行环境枚举
 *            区分开发模式 / 发布模式，用于环境控制
 ***************************************************************/

namespace GameLib
{
    /// <summary>
    /// 游戏运行模式枚举
    /// </summary>
    public enum EGameMode
    {
        /// <summary>
        /// 未设置任何模式（默认值）
        /// </summary>
        ENone,

        /// <summary>
        /// 开发者调试模式
        /// </summary>
        EDevelopMode,

        /// <summary>
        /// 正式发布模式
        /// </summary>
        EPublishMode
    }
}