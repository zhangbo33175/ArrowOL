/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorPlaySoundInfo.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音播放信息实体类 —— 存储单次播放的全部数据，用于跟踪与控制
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 声音播放信息（实体类）
    /// 作用：存储一次声音播放的所有信息，用于管理器内部跟踪、暂停、恢复、停止
    /// </summary>
    public sealed class HonorPlaySoundInfo
    {
        /// <summary>
        /// 声音唯一序列ID（用于定位、停止、暂停）
        /// </summary>
        private int m_SerialID;

        /// <summary>
        /// 声音所属分组（BGM/音效/UI等）
        /// </summary>
        private HonorSoundCategory m_HonorSoundCategory;

        /// <summary>
        /// 播放参数（音量、循环、优先级、音调、空间混合等）
        /// </summary>
        private HonorPlaySoundParams m_HonorPlaySoundParams;

        /// <summary>
        /// 播放扩展信息外壳（位置、绑定对象等额外数据）
        /// </summary>
        private HonorPlaySoundHandle m_HonorPlaySoundHandle;

        /// <summary>
        /// 默认构造：清空所有数据
        /// </summary>
        public HonorPlaySoundInfo()
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
        public HonorSoundCategory HonorSoundCategory => m_HonorSoundCategory;

        /// <summary>
        /// 播放参数（只读）
        /// </summary>
        public HonorPlaySoundParams HonorPlaySoundParams => m_HonorPlaySoundParams;

        /// <summary>
        /// 扩展信息外壳（位置/绑定对象等）
        /// </summary>
        public HonorPlaySoundHandle HonorPlaySoundHandle => m_HonorPlaySoundHandle;

        /// <summary>
        /// 创建播放信息实例（静态工厂方式，规范且安全）
        /// </summary>
        public static HonorPlaySoundInfo Create(int serialID, HonorSoundCategory HonorSoundCategory, HonorPlaySoundParams HonorPlaySoundParams,
            HonorPlaySoundHandle HonorPlaySoundHandle)
        {
            HonorPlaySoundInfo HonorPlaySoundInfo = new HonorPlaySoundInfo();
            HonorPlaySoundInfo.m_SerialID = serialID;
            HonorPlaySoundInfo.m_HonorSoundCategory = HonorSoundCategory;
            HonorPlaySoundInfo.m_HonorPlaySoundParams = HonorPlaySoundParams;
            HonorPlaySoundInfo.m_HonorPlaySoundHandle = HonorPlaySoundHandle;
            return HonorPlaySoundInfo;
        }

        /// <summary>
        /// 清空数据（对象池复用专用）
        /// </summary>
        public void Clear()
        {
            m_SerialID = 0;
            m_HonorSoundCategory = null;
            m_HonorPlaySoundParams = null;
            m_HonorPlaySoundHandle = null;
        }
    }
}