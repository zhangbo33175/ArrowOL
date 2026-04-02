using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Honor.Runtime
{
    // 自定义开关切换按钮，具有固定的样式。不实现开关切换时的动画，不实现Navigation中的排版
    public class AorSwitchButton : Toggle
    {
        [SerializeField]
        private Text m_Label;
        public Text Label
        {
            get { return m_Label; }
            set { m_Label = value; }
        }

        [SerializeField]
        private TextMeshProUGUI m_TMPLabel;
        public TextMeshProUGUI TMPLabel
        {
            set { m_TMPLabel = value; }
            get { return m_TMPLabel; }
        }

        protected override void Awake()
        {
            base.Awake();
            // 设置无切换状态过渡效果
            transition = Transition.None;
            toggleTransition = ToggleTransition.None;
        }

    }
}