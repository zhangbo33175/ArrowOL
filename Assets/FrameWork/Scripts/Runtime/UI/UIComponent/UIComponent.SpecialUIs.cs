using System;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class UIComponent
    {
        #region 菊花等待
        /// <summary>
        /// 菊花等待界面ref+1
        /// </summary>
        public void AddWaitingRef()
        {
            m_WaitingUIRefCount++;
            if (m_WaitingUIRefCount > 0)
            {
                if (m_WaitingUI == null)
                {
                    UIInfo uiInfo = new UIInfo()
                    {
                        UIType = UIType.Screen,
                        ABPath = m_WaitingUIABPath,
                        AssetName = m_WaitingUIAssetName,
                        IsModal = false,
                        ZOrder = GameConstants.WaitingUIZOrder,
                        Priority = -1,
                        CloseOnEscapeKeyUp = false,
                        BlockingMask = "Everything",
                        BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                        MultiTypeTextCompsCoexist = false,
                        LuaParams = null,
                        OverCallback = null,
                    };
                    GameObject waitingUIGO = OpenUISyncByInfo(uiInfo);
                    if (waitingUIGO == null)
                    {
                        Log.Error("WaitingUIGO 无效。");
                        return;
                    }

                    m_WaitingUI = waitingUIGO.GetComponent<UIWaitingBehaviour>();
                    if (m_WaitingUI == null)
                    {
                        Log.Error("WaitingUI 无效。");
                        return;
                    }

                    m_PermanentUIs.Add(waitingUIGO);
                }

                m_WaitingUI.SetVisible(true);
            }
        }

        /// <summary>
        /// 菊花等待界面ref-1
        /// </summary>
        public void SubWaitingRef()
        {
            m_WaitingUIRefCount--;
            if (m_WaitingUIRefCount < 0)
            {
                Log.Error("WaitingUI 引用计数不可为负数，请检查引用计数的加减调用。当前引用计数为：{0}", m_WaitingUIRefCount);
                m_WaitingUIRefCount = 0; // 修正为0 , 防止引用计数为负数
            }

            if (m_WaitingUIRefCount == 0)
            {
                m_WaitingUI.SetVisible(false);
            }
        }

        /// <summary>
        /// 获取菊花等待界面的可见性
        /// </summary>
        public bool IsWaitingVisible()
        {
            if (m_WaitingUI != null)
            {
                return m_WaitingUI.IsVisible();
            }

            return false;
        }

        /// <summary>
        /// 设置菊花等待界面显示文本
        /// </summary>
        /// <param name="text"></param>
        public void SetWaitingDescText(string text)
        {
            if (m_WaitingUI != null)
            {
                m_WaitingUI.SetWaitingDescText(text);
            }
        }
        #endregion

        /// <summary>
        /// 显示流程切换过渡进入效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            if (m_TransitionUI == null)
            {
                UIInfo uiInfo = new UIInfo()
                {
                    UIType = UIType.Screen,
                    ABPath = ProcedureManager.Instance.UITransitionABPath,
                    AssetName = ProcedureManager.Instance.UITransitionAssetName,
                    IsModal = false,
                    ZOrder = GameConstants.ProcedureTransitionUIZOrder,
                    Priority = 0,
                    CloseOnEscapeKeyUp = false,
                    BlockingMask = "Everything",
                    BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject transitionUIGO = UIManager.Instance.OpenUISyncByInfo(uiInfo);
                if (transitionUIGO == null)
                {
                    Log.Error("TransitionUIGO 无效。");
                    return;
                }

                m_TransitionUI = transitionUIGO.GetComponent<UITransitionBehaviour>();
                if (m_TransitionUI == null)
                {
                    Log.Error("TransitionUI 无效。");
                    return;
                }

                m_PermanentUIs.Add(transitionUIGO);
            }

            m_TransitionUI.EnterDuration = duration;
            m_TransitionUI.BlockRaycastOnEntering = blockRaycast;

            if (forceOver)
            {
                m_TransitionUI.EnterOver();
            }
            else
            {
                m_TransitionUI.Enter();
            }
        }

        /// <summary>
        /// 显示流程切换过渡退出效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            if (m_TransitionUI == null)
            {
                UIInfo uiInfo = new UIInfo()
                {
                    UIType = UIType.Screen,
                    ABPath = ProcedureManager.Instance.UITransitionABPath,
                    AssetName = ProcedureManager.Instance.UITransitionAssetName,
                    IsModal = false,
                    ZOrder = GameConstants.ProcedureTransitionUIZOrder,
                    Priority = 0,
                    CloseOnEscapeKeyUp = false,
                    BlockingMask = "Everything",
                    BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject transitionUIGO = UIManager.Instance.OpenUISyncByInfo(uiInfo);
                if (transitionUIGO == null)
                {
                    Log.Error("TransitionUIGO 无效。");
                    return;
                }

                m_TransitionUI = transitionUIGO.GetComponent<UITransitionBehaviour>();
                if (m_TransitionUI == null)
                {
                    Log.Error("TransitionUI 无效。");
                    return;
                }

                m_PermanentUIs.Add(transitionUIGO);
            }

            m_TransitionUI.ExitDuration = duration;
            m_TransitionUI.BlockRaycastOnExiting = blockRaycast;

            if (forceOver)
            {
                m_TransitionUI.ExitOver();
            }
            else
            {
                m_TransitionUI.Exit();
            }
        }

        /// <summary>
        /// 显示飘字
        /// </summary>
        /// <param name="text">飘字内容</param>
        /// <param name="duration">飘字持续时间</param>
        /// <param name="blockUITouches">是否阻塞UI触摸</param>
        /// <param name="overCallback">结束回调</param>
        public void ShowFloatWords(string text, float duration, bool blockUITouches = false, Action overCallback = null)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
                ABPath = m_FloatWordsUIABPath,
                AssetName = m_FloatWordsUIAssetName,
                IsModal = false,
                ZOrder = GameConstants.FloatWordsUIZOrder,
                Priority = -1,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = (string abPath, string assetName, LuaTable luaClass, GameObject uiGO) =>
                {
                    UIFloatWordsBehaviour floatWordsBehaviour = uiGO.GetComponent<UIFloatWordsBehaviour>();
                    if (floatWordsBehaviour.WordsText != null)
                    {
                        floatWordsBehaviour.WordsText.text = text;
                    }

                    if (floatWordsBehaviour.WordsTextTMP != null)
                    {
                        floatWordsBehaviour.WordsTextTMP.text = text;
                    }

                    floatWordsBehaviour.Duration = duration == 0 ? m_FloatWordsDuration : duration;
                    floatWordsBehaviour.BlockUITouches = blockUITouches;
                    floatWordsBehaviour.OverCallback = overCallback;
                },
            };
            OpenUIAsyncByInfo(uiInfo);
        }

        /// <summary>
        /// 展示Splash闪屏界面
        /// </summary>
        /// <param name="durationOverCallback">延迟结束回调</param>
        /// <returns></returns>
        public UISplashBehaviour ShowSplash(Action durationOverCallback)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
                ABPath = ProcedureManager.Instance.UISplashABPath,
                AssetName = ProcedureManager.Instance.UISplashAssetName,
                IsModal = false,
                ZOrder = GameConstants.SplashUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = UIManager.Instance.OpenUISyncByInfo(uiInfo);
            UISplashBehaviour uiSplashBehaviour = uiGO.GetComponent<UISplashBehaviour>();
            uiSplashBehaviour.DurationOverCallback = durationOverCallback;
            return uiSplashBehaviour;
        }

        /// <summary>
        /// 展示App大版本更新提示界面
        /// </summary>
        /// <returns></returns>
        public UIAppDownloadBehaviour ShowAppDownload(bool showCloseButton)
        {
            // 打开大版本更新提示界面
            /*UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
                ABPath = Root.Hotfix.UIAppDownloadABPath,
                AssetName = Root.Hotfix.UIAppDownloadAssetName,
                IsModal = false,
                ZOrder = Constants.AppDownloadUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = Root.UI.OpenUISyncByInfo(uiInfo);
            UIAppDownloadBehaviour uiAppDownloadBehaviour = uiGO.GetComponent<UIAppDownloadBehaviour>();
            uiAppDownloadBehaviour.InitView(showCloseButton);
            return uiAppDownloadBehaviour;*/
            return null;
        }
        

        /// <summary>
        /// 展示loading界面
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        /// <returns></returns>
        public UILoadingBehaviour ShowLoading(UILoadingBehaviour.LoadingMode loadingMode)
        {
            string abPath = string.Empty;
            string assetName = string.Empty;
            int ZOrder = 0;
            switch (loadingMode)
            {
                case UILoadingBehaviour.LoadingMode.Preload:
                    abPath = ProcedureManager.Instance.UIPreloadABPath;
                    assetName =ProcedureManager.Instance.UIPreloadAssetName;
                    ZOrder = GameConstants.PreloadUIZOrder;
                    break;
                default:
                    Log.Error("UIManager.ShowLoading 参数错误。");
                    break;
            }

            if (!string.IsNullOrEmpty(abPath) && !string.IsNullOrEmpty(assetName))
            {
                UIInfo uiInfo = new UIInfo()
                {
                    UIType = UIType.Screen,
                    ABPath = abPath,
                    AssetName = assetName,
                    IsModal = false,
                    ZOrder = ZOrder,
                    Priority = 0,
                    CloseOnEscapeKeyUp = false,
                    BlockingMask = "Everything",
                    BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject uiGO = UIManager.Instance.OpenUISyncByInfo(uiInfo);
                UILoadingBehaviour uiLoadingBehaviour = uiGO.GetComponent<UILoadingBehaviour>();
                uiLoadingBehaviour.SetLoadingMode(loadingMode);
                return uiLoadingBehaviour;
            }

            return null;
        }

        /// <summary>
        /// 隐藏loading界面
        /// </summary>
        /// <param name="ui">待隐藏的ui</param>
        /// <returns></returns>
        public void HideLoading(UILoadingBehaviour ui)
        {
            ui.SetVisible(false);
        }
        
    }
}