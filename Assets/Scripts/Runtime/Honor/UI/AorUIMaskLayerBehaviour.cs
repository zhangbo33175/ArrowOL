using Honor.Runtime;
using UnityEngine;

namespace GameLib
{
    public class AorUIMaskLayerBehaviour: MonoBehaviour
    {
        private const float maskTimeOut = 50.0f;
        private float tempTime = 0f;

        private void Awake()
        {
            tempTime = 0f;
        }

        private void OnEnable()
        {
            tempTime = 0f;
        }

        private void OnDisable()
        {
            tempTime = 0f; 
        }

        private void Update()
        {
            tempTime += Time.deltaTime;
            if (tempTime >= maskTimeOut)
            {
                tempTime = 0f;
                GameMainRoot.UI.CloseUIMaskLayer();
            }
        }

        /// <summary>
        /// 可见性设置
        /// </summary>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// 获取可见性
        /// </summary>
        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }
    }
}