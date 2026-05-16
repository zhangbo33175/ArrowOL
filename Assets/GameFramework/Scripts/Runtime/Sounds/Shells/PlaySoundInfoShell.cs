/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlaySoundInfoShell.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音播放信息外壳 - 存储音频播放额外扩展数据（3D位置等）
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音播放信息外壳（扩展数据类）
    /// 作用：存储播放声音时的额外数据，目前主要用于 3D 音效的世界坐标
    /// </summary>
    public sealed class PlaySoundInfoShell
    {
        //=========================================================================
        // 私有成员变量
        //=========================================================================
        #region 私有成员变量
        
        /// <summary>
        /// 3D 声音所在的世界坐标
        /// </summary>
        private Vector3 m_WorldPosition;

        #endregion


        //=========================================================================
        // 公共属性
        //=========================================================================
        #region 公共属性
        
        /// <summary>
        /// 获取声音的世界位置（只读）
        /// </summary>
        public Vector3 WorldPosition
        {
            get => m_WorldPosition;
        }

        #endregion


        //=========================================================================
        // 构造函数
        //=========================================================================
        #region 构造函数
        
        /// <summary>
        /// 默认构造，位置归零
        /// </summary>
        public PlaySoundInfoShell()
        {
            m_WorldPosition = Vector3.zero;
        }

        #endregion


        //=========================================================================
        // 公共方法
        //=========================================================================
        #region 公共方法
        
        /// <summary>
        /// 静态创建方法（工厂模式）
        /// </summary>
        /// <param name="worldPosition">声音世界坐标</param>
        /// <returns>声音信息外壳实例</returns>
        public static PlaySoundInfoShell Create(Vector3 worldPosition)
        {
            PlaySoundInfoShell shell = new PlaySoundInfoShell();
            shell.m_WorldPosition = worldPosition;
            return shell;
        }

        /// <summary>
        /// 清空数据（对象池复用）
        /// </summary>
        public void Clear()
        {
            m_WorldPosition = Vector3.zero;
        }

        #endregion
    }
}