/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI组件（分部类）
 *            提供遮罩层管理、引用计数、屏幕缩放等核心UI接口
 ***************************************************************/

using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// UI管理组件（分部类）
    /// 提供遮罩层、引用计数、屏幕适配等对外接口
    /// </summary>
    public sealed partial class UIComponent
    {
        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// [屏幕UI] 遮罩UI（常驻内存）
        /// </summary>
        public AorUIMaskLayerBehaviour MaskLayerUI
        {
            get { return m_UIManager.MaskUI; }
        }

        /// <summary>
        /// [屏幕UI] 遮罩UI引用计数
        /// </summary>
        public int MaskLayerUIRefCount
        {
            get { return m_UIManager.MaskLayerUIRefCount; }
        }
        #endregion

        #region 公共方法
        //=========================================================================
        // 公共方法
        //=========================================================================
        /// <summary>
        /// 遮罩层界面引用计数 +1
        /// </summary>
        public void AddUIMaskLayerRef(string name)
        {
            m_UIManager.AddUIMakLayerRef(name);
        }

        /// <summary>
        /// 遮罩层界面引用计数 -1
        /// </summary>
        public void SubUIMaskLayerRef()
        {
            m_UIManager.SubUIMaskLayerRef();
        }

        /// <summary>
        /// 获取遮罩层当前可见状态
        /// </summary>
        public bool IsUIMaskLayerVisible()
        {
            return MaskLayerUI.IsVisible();
        }

        /// <summary>
        /// 强制关闭遮罩层（异常情况下立即关闭）
        /// </summary>
        public void CloseUIMaskLayer()
        {
            m_UIManager.CloseUIMaskLayer();
        }

        /// <summary>
        /// 获取当前屏幕适配缩放比例
        /// </summary>
        public float GetScreenScale()
        {
            return m_UIManager.GetScreenScale();
        }
        #endregion
    }
}