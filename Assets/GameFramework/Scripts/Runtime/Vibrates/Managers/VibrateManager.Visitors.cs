
using System;
using System.Collections.Generic;
#if NICEVIBRATIONS_ENABLE
using Lofelt.NiceVibrations;
#endif
namespace Honor.Runtime
{
    public sealed partial class VibrateManager
    {
        /// <summary>
        /// 自定义振动组合
        /// </summary>
        private readonly Dictionary<string, List<VibrateInfo>> m_CustomVibratesGroup;

        /// <summary>
        /// 点振动组合
        /// </summary>
        private readonly Dictionary<string, List<VibrateInfo>> m_EmphasisVibratesGroup;

#if NICEVIBRATIONS_ENABLE
        /// <summary>
        /// 整体触觉输出水平,默认值是1，跟ClipLevel搭配使用
        /// </summary>
        public float OutputLevel
        {
            get { return HapticController.outputLevel; }
            set { HapticController.outputLevel = value; }
        }
#endif


#if NICEVIBRATIONS_ENABLE
        /// <summary>
        /// 加载剪辑的级别默认值是1，跟OutputLevel搭配使用
        /// </summary>
        public float ClipLevel 
        {
            get { return HapticController.clipLevel; }
            set { HapticController.clipLevel = value; }
        }
#endif
    }
}


