using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public class AorTextLocalizing : MonoBehaviour
    {
        /// <summary>
        /// 本地化字段名称
        /// </summary>
        [SerializeField]
        private string m_LocalizingKeyName;
        public string LocalizingKeyName
        {
            set
            {
                // 确保手动设置不同的KeyName时触发Text内容刷新
                if(m_LocalizingKeyName != value)
                {
                    m_LocalizingKeyName = value;
                    RefreshTextValue();
                }
            }
            get
            {
                return m_LocalizingKeyName;
            }
        }

        /// <summary>
        /// 本地化字体标记
        /// </summary>
        [SerializeField]
        private string m_LocalizingFontMark;
        public string LocalizingFontMark
        {
            get
            {
                return m_LocalizingFontMark;
            }
        }

        /// <summary>
        /// 本地化字段名称对应的内容
        /// </summary>
        private string m_LocalizingValue;
        public string LocalizingValue
        {
            get
            {
                return m_LocalizingValue;
            }
        }

        /// <summary>
        /// 当前显示状态下的多语言类型，如果是隐藏状态，则是上一次显示时的多语言类型
        /// </summary>
        [SerializeField]
        private GameDefinitions.Language m_Language;
        public GameDefinitions.Language Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                m_Language = value;
            }
        }

        /// <summary>
        /// 是否开启text位置跟随配置表格动态调整功能
        /// </summary>
        private bool m_OpenPosOffset = true;
        public bool OpenPosOffset
        {
            get
            {
                return m_OpenPosOffset;
            }
            set
            {
                m_OpenPosOffset = value;
            }
        }

        /// <summary>
        /// 自身文本对象(Text)
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 自身文本对象(TextMeshProUGUI)
        /// </summary>
        private TextMeshProUGUI m_TextMeshProUGUI;

        /// <summary>
        /// 初始化
        /// </summary>
        void Start()
        {
            GameMainRoot.Event.Subscribe(GameEventCmd.TextLocalizingRefresh, this, RefreshTextValue);
            m_Text = transform.GetComponent<Text>();
            m_TextMeshProUGUI = transform.GetComponent<TextMeshProUGUI>();
            if (m_Text == null && m_TextMeshProUGUI == null)
            {
                Log.Fatal("TextLocalizing 无有效的Text/TextMeshProUGUI组件。");
            }
            // 确保初始化时触发Text内容刷新
            RefreshTextValue();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        void OnDestroy()
        {
            GameMainRoot.Event.Unsubscribe(GameEventCmd.TextLocalizingRefresh, this, RefreshTextValue);
        }

        /// <summary>
        /// 刷新文本内容
        /// </summary>
        private void RefreshTextValue(object sender = null, object userData = null, EventParams e = null)
        {
            if (sender != null && sender != (object)GameMainRoot.Localization) return;
            
            if (!string.IsNullOrEmpty(m_LocalizingKeyName))
            {
                m_LocalizingValue = GameMainRoot.Lua.LuaLocalizingCSEventDelegate(m_LocalizingKeyName);
                if(m_Text != null) m_Text.text = m_LocalizingValue;
                if(m_TextMeshProUGUI != null) m_TextMeshProUGUI.text = m_LocalizingValue;
            }
        }
    }

}


