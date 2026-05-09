using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音播放信息外壳（扩展数据类）
    /// 作用：存储播放声音时的额外数据，目前主要用于 3D 音效的世界坐标
    /// </summary>
    public sealed class PlaySoundInfoShell
    {
        /// <summary>
        /// 3D 声音所在的世界坐标
        /// </summary>
        private Vector3 m_WorldPosition;

        /// <summary>
        /// 获取声音的世界位置（只读）
        /// </summary>
        public Vector3 WorldPosition
        {
            get => m_WorldPosition;
        }

        /// <summary>
        /// 默认构造，位置归零
        /// </summary>
        public PlaySoundInfoShell()
        {
            m_WorldPosition = Vector3.zero;
        }

        /// <summary>
        /// 静态创建方法（工厂模式）
        /// </summary>
        /// <param name="worldPosition">声音世界坐标</param>
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
    }
}