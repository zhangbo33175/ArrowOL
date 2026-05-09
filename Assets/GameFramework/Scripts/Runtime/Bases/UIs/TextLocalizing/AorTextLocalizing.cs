using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 文本多语言本地化组件
    /// 功能：自动根据当前语言加载对应文本，支持UGUI Text与TextMeshProUGUI
    /// 监听语言切换事件，自动刷新显示内容
    /// </summary>
    public class AorTextLocalizing : MonoBehaviour
    {
        /// <summary>
        /// 多语言配置表中的关键字（Key）
        /// </summary>
        [SerializeField]
        private string m_LocalizingKeyName;
        
        /// <summary>
        /// 多语言关键字，设置时自动刷新文本
        /// </summary>
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
        /// 多语言字体标记（用于区分不同语言字体）
        /// </summary>
        [SerializeField]
        private string m_LocalizingFontMark;
        
        /// <summary>
        /// 多语言字体标记（只读）
        /// </summary>
        public string LocalizingFontMark
        {
            get
            {
                return m_LocalizingFontMark;
            }
        }

        /// <summary>
        /// 多语言关键字对应的实际显示文本内容
        /// </summary>
        private string m_LocalizingValue;
        
        /// <summary>
        /// 当前加载的本地化文本内容（只读）
        /// </summary>
        public string LocalizingValue
        {
            get
            {
                return m_LocalizingValue;
            }
        }

        /// <summary>
        /// 当前使用的语言类型
        /// </summary>
        [SerializeField]
        private GameDefinitions.Language m_Language;
        
        /// <summary>
        /// 当前语言类型，可读写
        /// </summary>
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
        /// 是否开启文本位置根据表格动态偏移功能
        /// </summary>
        private bool m_OpenPosOffset = true;
        
        /// <summary>
        /// 文本位置动态偏移开关
        /// </summary>
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
        /// UGUI 文本组件
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// TextMeshPro 文本组件
        /// </summary>
        private TextMeshProUGUI m_TextMeshProUGUI;

        /// <summary>
        /// 初始化：注册多语言刷新事件，获取文本组件
        /// </summary>
        void Start()
        {
            // 监听多语言刷新全局事件
            GameMainRoot.Event.Subscribe(GameEventCmd.TextLocalizingRefresh, this, RefreshTextValue);
            
            // 获取自身文本组件
            m_Text = transform.GetComponent<Text>();
            m_TextMeshProUGUI = transform.GetComponent<TextMeshProUGUI>();

            // 无文本组件时输出错误日志
            if (m_Text == null && m_TextMeshProUGUI == null)
            {
                Log.Fatal("TextLocalizing 无有效的Text/TextMeshProUGUI组件。");
            }
            
            // 初始化完成后立即刷新文本
            RefreshTextValue();
        }

        /// <summary>
        /// 销毁时：注销多语言事件监听，防止内存泄漏
        /// </summary>
        void OnDestroy()
        {
            GameMainRoot.Event.Unsubscribe(GameEventCmd.TextLocalizingRefresh, this, RefreshTextValue);
        }

        /// <summary>
        /// 刷新本地化文本内容
        /// 从多语言系统获取对应Key的文本，并赋值给Text/TMP组件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="userData">用户数据</param>
        /// <param name="e">事件参数</param>
        private void RefreshTextValue(object sender = null, object userData = null, EventParams e = null)
        {
            // 仅响应多语言管理类发送的刷新事件
            if (sender != null && sender != (object)GameMainRoot.Localization) 
                return;
            
            // 根据Key获取多语言文本并赋值
            if (!string.IsNullOrEmpty(m_LocalizingKeyName))
            {
                m_LocalizingValue = GameMainRoot.Lua.LuaLocalizingCSEventDelegate(m_LocalizingKeyName);
                
                // 同步到文本组件
                if(m_Text != null) 
                    m_Text.text = m_LocalizingValue;
                
                if(m_TextMeshProUGUI != null) 
                    m_TextMeshProUGUI.text = m_LocalizingValue;
            }
        }
    }
}