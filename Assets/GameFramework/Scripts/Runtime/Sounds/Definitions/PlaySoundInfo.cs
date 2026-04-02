namespace Honor.Runtime
{
    public sealed class PlaySoundInfo
    {
        private int m_SerialID;
        private SoundGroup m_SoundGroup;
        private PlaySoundParams m_PlaySoundParams;
        private PlaySoundInfoShell m_PlaySoundInfoShell;

        public PlaySoundInfo()
        {
            m_SerialID = 0;
            m_SoundGroup = null;
            m_PlaySoundParams = null;
            m_PlaySoundInfoShell = null;
        }

        public int SerialID
        {
            get
            {
                return m_SerialID;
            }
        }

        public SoundGroup SoundGroup
        {
            get
            {
                return m_SoundGroup;
            }
        }

        public PlaySoundParams PlaySoundParams
        {
            get
            {
                return m_PlaySoundParams;
            }
        }

        public object PlaySoundInfoShell
        {
            get
            {
                return m_PlaySoundInfoShell;
            }
        }

        public static PlaySoundInfo Create(int serialID, SoundGroup soundGroup, PlaySoundParams playSoundParams, PlaySoundInfoShell playSoundInfoShell)
        {
            PlaySoundInfo playSoundInfo = new PlaySoundInfo();
            playSoundInfo.m_SerialID = serialID;
            playSoundInfo.m_SoundGroup = soundGroup;
            playSoundInfo.m_PlaySoundParams = playSoundParams;
            playSoundInfo.m_PlaySoundInfoShell = playSoundInfoShell;
            return playSoundInfo;
        }

        public void Clear()
        {
            m_SerialID = 0;
            m_SoundGroup = null;
            m_PlaySoundParams = null;
            m_PlaySoundInfoShell = null;
        }
    }
}


