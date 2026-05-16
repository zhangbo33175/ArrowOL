/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PlaySoundInfo.cs
 * author:  云毅
 * created:
 * descrip:   声音播放信息实体类 —— 存储单次播放的全部数据，用于跟踪与控制
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 声音播放信息（实体类）
    /// 作用：存储一次声音播放的所有信息，用于管理器内部跟踪、暂停、恢复、停止
    /// </summary>
    public sealed class PlaySoundInfo
    {
        /// <summary>
        /// 声音唯一序列ID（用于定位、停止、暂停）
        /// </summary>
        private int m_SerialID;

        /// <summary>
        /// 声音所属分组（BGM/音效/UI等）
        /// </summary>
        private SoundGroup m_SoundGroup;

        /// <summary>
        /// 播放参数（音量、循环、优先级、音调、空间混合等）
        /// </summary>
        private PlaySoundParams m_PlaySoundParams;

        /// <summary>
        /// 播放扩展信息外壳（位置、绑定对象等额外数据）
        /// </summary>
        private PlaySoundInfoShell m_PlaySoundInfoShell;

        /// <summary>
        /// 默认构造：清空所有数据
        /// </summary>
        public PlaySoundInfo()
        {
            Clear();
        }

        /// <summary>
        /// 声音唯一ID（只读）
        /// </summary>
        public int SerialID => m_SerialID;

        /// <summary>
        /// 所属声音组（只读）
        /// </summary>
        public SoundGroup SoundGroup => m_SoundGroup;

        /// <summary>
        /// 播放参数（只读）
        /// </summary>
        public PlaySoundParams PlaySoundParams => m_PlaySoundParams;

        /// <summary>
        /// 扩展信息外壳（位置/绑定对象等）
        /// </summary>
        public PlaySoundInfoShell PlaySoundInfoShell => m_PlaySoundInfoShell;

        /// <summary>
        /// 创建播放信息实例（静态工厂方式，规范且安全）
        /// </summary>
        public static PlaySoundInfo Create(int serialID, SoundGroup soundGroup, PlaySoundParams playSoundParams,
            PlaySoundInfoShell playSoundInfoShell)
        {
            PlaySoundInfo playSoundInfo = new PlaySoundInfo();
            playSoundInfo.m_SerialID = serialID;
            playSoundInfo.m_SoundGroup = soundGroup;
            playSoundInfo.m_PlaySoundParams = playSoundParams;
            playSoundInfo.m_PlaySoundInfoShell = playSoundInfoShell;
            return playSoundInfo;
        }

        /// <summary>
        /// 清空数据（对象池复用专用）
        /// </summary>
        public void Clear()
        {
            m_SerialID = 0;
            m_SoundGroup = null;
            m_PlaySoundParams = null;
            m_PlaySoundInfoShell = null;
        }
    }
}