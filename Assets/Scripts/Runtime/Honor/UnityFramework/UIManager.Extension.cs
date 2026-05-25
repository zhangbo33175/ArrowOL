/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI管理器扩展（遮罩层管理）
 *            实现遮罩引用计数、自动创建、显示/隐藏、异常关闭功能
 ***************************************************************/

using GameLib;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// UI管理器扩展（针对遮罩层管理）
    /// </summary>
    public sealed partial class UIManager
    {
        #region 私有字段
        //=========================================================================
        // 私有字段
        //=========================================================================
        /// <summary>
        /// [屏幕遮罩UI] 遮罩UI实例（常驻内存）
        /// </summary>
        private AorUIMaskLayerBehaviour m_MaskLayerUI;

        /// <summary>
        /// 设计分辨率与屏幕的缩放比例
        /// </summary>
        private float m_ScreenScale;

        /// <summary>
        /// 遮罩层引用计数
        /// </summary>
        private int m_MaskLayerUIRefCount;
        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 遮罩UI实例（对外只读）
        /// </summary>
        public AorUIMaskLayerBehaviour MaskUI
        {
            get { return m_MaskLayerUI; }
        }

        /// <summary>
        /// 遮罩层引用计数（对外可读写）
        /// </summary>
        public int MaskLayerUIRefCount
        {
            set { m_MaskLayerUIRefCount = value; }
            get { return m_MaskLayerUIRefCount; }
        }
        #endregion

        #region 公共方法
        //=========================================================================
        // 公共方法
        //=========================================================================
        /// <summary>
        /// 遮罩层引用计数 +1
        /// 自动创建遮罩UI并显示
        /// </summary>
        public void AddUIMakLayerRef(string name)
        {
            m_MaskLayerUIRefCount++;
            
            if (m_MaskLayerUIRefCount > 0)
            {
                // 未创建则同步创建遮罩层
                if (m_MaskLayerUI == null)
                {
                    UIInfo uiInfo = new UIInfo()
                    {
                        UIType = UIType.Screen,
                        ABPath = "Assets/Res/Prefabs/UIs/UIMaskLayer",
                        AssetName = "UIMaskLayer",
                        IsModal = false,
                        ZOrder = 8000,
                        Priority = -1,
                        CloseOnEscapeKeyUp = false,
                        BlockingMask = "Everything",
                        BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                        MultiTypeTextCompsCoexist = false,
                        LuaParams = null,
                        OverCallback = null,
                    };
                    
                    GameObject maskLayerUIGO = OpenUISyncByInfo(uiInfo);
                    if (maskLayerUIGO == null)
                    {
                        Log.Error("maskLayerUIGO 无效。");
                        return;
                    }

                    m_MaskLayerUI = maskLayerUIGO.GetComponent<AorUIMaskLayerBehaviour>();
                    if (m_MaskLayerUI == null)
                    {
                        Log.Error("UIMaskLayer 无效。");
                        return;
                    }

                    m_PermanentUIs.Add(maskLayerUIGO);
                }

#if UNITY_EDITOR
                m_MaskLayerUI.gameObject.name = $"UIMaskLayer[{name}]";
#endif
                Log.Info("Add UIMaskLayer[{0}] RefCount: {1}", name, MaskLayerUIRefCount);
                m_MaskLayerUI.SetVisible(true);
            }
        }

        /// <summary>
        /// 遮罩层引用计数 -1
        /// 计数为0时自动隐藏
        /// </summary>
        public void SubUIMaskLayerRef()
        {
            m_MaskLayerUIRefCount--;
            Log.Info("Sub UIMaskLayer RefCount: {0}", MaskLayerUIRefCount);
            
            if (m_MaskLayerUIRefCount < 0)
            {
                Log.Error("MaskLayer 引用计数不可为负数，请检查引用计数的加减调用。当前引用计数为：{0}", m_MaskLayerUIRefCount);
                m_MaskLayerUIRefCount = 0;
                m_MaskLayerUI.SetVisible(false);
            }
            else if (m_MaskLayerUIRefCount == 0)
            {
                m_MaskLayerUI.SetVisible(false);
            }
        }

        /// <summary>
        /// 强制关闭遮罩层（异常情况下立即关闭）
        /// 重置引用计数并隐藏
        /// </summary>
        public void CloseUIMaskLayer()
        {
            Log.Error("UIMaskLayer 异常情况 使用CloseUIMaskLayer 关闭 ：{0}", m_MaskLayerUIRefCount);
            m_MaskLayerUIRefCount = 0;
            m_MaskLayerUI.SetVisible(false);
        }

        /// <summary>
        /// 获取设计分辨率与屏幕的缩放比例
        /// </summary>
        public float GetScreenScale()
        {
            if (m_ScreenScale <= 0)
            {
                Vector2 screenDesignedResolution = m_ScreenUICanvasScaler.referenceResolution;
                Vector2 gameView = Util.GameViewSize();
                m_ScreenScale = gameView.y / screenDesignedResolution.y;
            }

            return m_ScreenScale;
        }
        #endregion
    }
}