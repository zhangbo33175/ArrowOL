
using System.Collections.Generic;
using UnityEngine;
#if UIEXTENSION_ENABLE
using UnityEngine.UI.Extensions;

namespace Honor.Runtime
{
    [ExecuteInEditMode]
    public partial class AorTextPicMixed : TextPic
    {
        /// <summary>
        /// 文本中图片标识起始符
        /// </summary>
        private const string IDENTIFIERS_START = "#__";

        /// <summary>
        /// 文本中图片标识结束符
        /// </summary>
        private const string IDENTIFIERS_END = "__#";

        /// <summary>
        /// 文本中图片标识分隔符
        /// </summary>
        private const char SEPARATOR = '&';

        /// <summary>
        /// 运行时动态加载Sprite集合
        /// </summary>
        private List<Sprite> m_SpriteListOnPlaying = new List<Sprite>();

        /// <summary>
        /// 重写text文本读写接口
        /// </summary>
        public override string text
        {
            get { return m_Text; }
            set { base.text = value; ParseTextOnResDefFormat(); ParseTextOnDetailFormat(); }
        }

        void Start()
        {
            base.Start();
            ParseTextOnResDefFormat();
            ParseTextOnDetailFormat();
            //text = "Round192#__Assets/Game/Textures/AppIcons/Android&Foreground432&1&0&0__##__Sprite_Image_Header1&1&0&0__#";
        }

        void OnDestroy()
        {
            base.OnDestroy();
            m_SpriteListOnPlaying.ForEach((sprite) =>
            {
                GameMainRoot.Asset.UnloadAsset(sprite);
            });
            m_SpriteListOnPlaying.Clear();
        }

    }

}
#else
using UnityEngine.UI;
    namespace Honor.Runtime
    {
        [ExecuteInEditMode]
        public class AorTextPicMixed : Text
        { 
    
        }
    }
#endif


