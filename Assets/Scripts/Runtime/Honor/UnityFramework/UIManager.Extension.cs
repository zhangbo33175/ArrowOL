using GameLib;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
     /// <summary>
    /// 针对UIManager的扩展
    /// </summary>
    public sealed partial class UIManager
    {
        /// <summary>
        /// [屏幕遮罩UI] 遮罩UI（常驻内存）
        /// </summary>
        private AorUIMaskLayerBehaviour m_MaskLayerUI;

        /// <summary>
        /// 与设计分辨率比屏幕的缩放比例
        /// </summary>
        private float m_ScreenScale;

        public AorUIMaskLayerBehaviour MaskUI
        {
            get { return m_MaskLayerUI; }
        }

        /// <summary>
        /// [屏幕遮罩UI] 遮罩UI引用计数
        /// </summary>
        private int m_MaskLayerUIRefCount;

        public int MaskLayerUIRefCount
        {
            set { m_MaskLayerUIRefCount = value; }
            get { return m_MaskLayerUIRefCount; }
        }

        /// <summary>
        /// MaskLayer界面ref+1
        /// </summary>
        public void AddUIMakLayerRef(string name)
        {
            m_MaskLayerUIRefCount++;
            if (m_MaskLayerUIRefCount > 0)
            {
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
        /// MaskLayer界面ref-1
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
        /// 关闭UIMaskLayer(异常情况下，立即关闭)
        /// </summary>
        public void CloseUIMaskLayer()
        {
            Log.Error("UIMaskLayer 异常情况 使用CloseUIMaskLayer 关闭 ：{0}", m_MaskLayerUIRefCount);
            m_MaskLayerUIRefCount = 0;
            m_MaskLayerUI.SetVisible(false);
        }

        /// <summary>
        /// 获取与设计分辨率比的屏幕缩放比例
        /// </summary>
        /// <returns></returns>
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
    }
}