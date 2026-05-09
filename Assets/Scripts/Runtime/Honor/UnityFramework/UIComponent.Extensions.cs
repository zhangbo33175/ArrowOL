using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIComponent
    {
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

        /// <summary>
        /// MaskLayer界面ref+1
        /// </summary>
        public void AddUIMaskLayerRef(string name)
        {
            m_UIManager.AddUIMakLayerRef(name);
        }

        /// <summary>
        /// MaskLayer界面ref-1
        /// </summary>
        public void SubUIMaskLayerRef()
        {
            m_UIManager.SubUIMaskLayerRef();
        }

        /// <summary>
        /// 获取MaskLayer界面的可见性
        /// </summary>
        public bool IsUIMaskLayerVisible()
        {
            return MaskLayerUI.IsVisible();
        }

        /// <summary>
        /// 关闭UIMaskLayer(异常情况下，立即关闭)
        /// </summary>
        public void CloseUIMaskLayer()
        {
            m_UIManager.CloseUIMaskLayer();
        }

        /// <summary>
        /// 获取屏幕缩放比例
        /// </summary>
        /// <returns></returns>
        public float GetScreenScale()
        {
            return m_UIManager.GetScreenScale();
        }
    }
}