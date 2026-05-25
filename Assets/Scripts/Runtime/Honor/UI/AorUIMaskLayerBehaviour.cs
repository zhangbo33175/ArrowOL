/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorUIMaskLayerBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI遮罩层自动关闭组件
 *            超时自动关闭遮罩，提供可见性控制接口
 ***************************************************************/

using Honor.Runtime;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// UI遮罩层自动关闭组件
    /// 功能：超时自动关闭遮罩，提供可见性控制接口
    /// </summary>
    public class AorUIMaskLayerBehaviour : MonoBehaviour
    {
        #region 常量 & 私有字段
        //=========================================================================
        // 常量 & 私有字段
        //=========================================================================
        /// <summary>
        /// 遮罩自动关闭超时时间
        /// </summary>
        private const float maskTimeOut = 50.0f;

        /// <summary>
        /// 计时临时变量
        /// </summary>
        private float tempTime = 0f;
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================
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
        #endregion

        #region 公共方法
        //=========================================================================
        // 公共方法
        //=========================================================================
        /// <summary>
        /// 设置可见性
        /// </summary>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// 获取当前可见状态
        /// </summary>
        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }
        #endregion
    }
}