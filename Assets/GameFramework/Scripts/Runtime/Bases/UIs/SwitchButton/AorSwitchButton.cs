using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义开关切换按钮 (基于UGUI Toggle扩展)
    /// 具备固定样式，不实现开关切换动画效果，不处理Navigation导航排版逻辑
    /// 同时支持UGUI文本与TextMeshPro文本显示
    /// </summary>
    public class AorSwitchButton : Toggle
    {
        /// <summary>
        /// UGUI 文本标签，用于显示开关文字
        /// </summary>
        [SerializeField]
        private Text m_Label;
        
        /// <summary>
        /// 外部访问UGUI文本标签
        /// </summary>
        public Text Label
        {
            get { return m_Label; }
            set { m_Label = value; }
        }

        /// <summary>
        /// TextMeshPro 文本标签，用于显示开关文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_TMPLabel;
        
        /// <summary>
        /// 外部访问TextMeshPro文本标签
        /// </summary>
        public TextMeshProUGUI TMPLabel
        {
            set { m_TMPLabel = value; }
            get { return m_TMPLabel; }
        }

        /// <summary>
        /// 重写Awake方法，初始化开关基础配置
        /// </summary>
        protected override void Awake()
        {
            // 调用父类Awake逻辑
            base.Awake();
            
            // 禁用UGUI默认的状态过渡动画（颜色/缩放/精灵切换）
            transition = Transition.None;
            // 禁用Toggle自带的切换过渡效果
            toggleTransition = ToggleTransition.None;
        }
    }
}