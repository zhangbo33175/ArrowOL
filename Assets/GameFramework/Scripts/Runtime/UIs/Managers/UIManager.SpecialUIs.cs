using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if WEBVIEW_ENABLE
using Vuplex.WebView;
#endif
using XLua;
using static UnityEngine.UI.GraphicRaycaster;

namespace Honor.Runtime
{
    public sealed partial class UIManager
    {
        /// <summary>
        /// 菊花等待界面ref+1
        /// </summary>
        public void AddWaitingRef()
        {
            m_WaitingUIRefCount++;
            if (m_WaitingUIRefCount > 0)
            {
                if (_mConnectionWaitingUIConnection == null)
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
                        BlockingObjects = BlockingObjects.None,
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
                    _mConnectionWaitingUIConnection = waitingUIGO.GetComponent<UIConnectionWaitingView>();
                    if (_mConnectionWaitingUIConnection == null)
                    {
                        Log.Error("WaitingUI 无效。");
                        return;
                    }
                    m_PermanentUIs.Add(waitingUIGO);
                }
                _mConnectionWaitingUIConnection.SetVisible(true);
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
                _mConnectionWaitingUIConnection.SetVisible(false);
            }
        }

        /// <summary>
        /// 获取菊花等待界面的可见性
        /// </summary>
        public bool IsWaitingVisible()
        {
            if (_mConnectionWaitingUIConnection != null)
            {
                return _mConnectionWaitingUIConnection.IsVisible();
            }
            return false;
        }

        /// <summary>
        /// 设置菊花等待界面显示文本
        /// </summary>
        /// <param name="text"></param>
        public void SetWaitingDescText(string text)
        {
            if (_mConnectionWaitingUIConnection != null)
            {
                _mConnectionWaitingUIConnection.SetWaitingDescText(text);
            }
        }

        /// <summary>
        /// 在游戏内打开webview
        /// </summary>
        /// <param name="viewRect">打开的webview挂载的节点</param>
        /// <param name="loadOverCallback">webview创建后的回调函数</param>
        /// <param name="openUrl">要打开的url链接，可为null</param>
#if WEBVIEW_ENABLE
        public async void OpenWebView(RectTransform adaptRectTransform, string openUrl = null, WebViewLoadOverCallback loadOverCallback = null)
        {
#if UNITY_WEBGL
            // 创建新的预制体，并挂载在m_WebUICanvas下
            var vuplexWebView = CanvasWebViewPrefab.Instantiate();
            vuplexWebView.transform.parent = m_WebUICanvas.transform;

            // 处理打开的网页的大小和位置
            var localRectTransform = adaptRectTransform;
            var rectTransform = vuplexWebView.transform.GetComponent<RectTransform>();
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                (Screen.width - localRectTransform.rect.width * localRectTransform.localScale.x) * localRectTransform.pivot.x,
                localRectTransform.rect.width * localRectTransform.localScale.x);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                (Screen.height - localRectTransform.rect.height * localRectTransform.localScale.y) * localRectTransform.pivot.y,
                localRectTransform.rect.height * localRectTransform.localScale.y);

            // 统一在回调函数中打开url，或者html，或者js
            vuplexWebView.InitialUrl = null;

            // Load a URL once the prefab finishes initializing
            await vuplexWebView.WaitUntilInitialized();
            // 挂载统一逻辑处理的脚本
            UIWebGLWebView newView = vuplexWebView.transform.GetOrAddComponent<UIWebGLWebView>();
            newView.WebView = vuplexWebView;
            newView.OnWebGLVuplexViewInitialized();
            // 调用回调，通知UI webview节点创建并初始化完毕
            if (loadOverCallback != null)
            {
                loadOverCallback(newView);
            }
            if (openUrl != null)
            {
                newView.LoadUrl(openUrl);
            }
#else
            if (adaptRectTransform != null)
            {
                // 在需要显示网页的节点上面挂载统一的网页处理UI
                UIWebGLWebView newView = adaptRectTransform.transform.GetOrAddComponent<UIWebGLWebView>();
                // 添加网页显示界面
                newView.CreateUniWebView();
                // 调用回调，通知UIWebGLWebView节点创建并初始化完毕
                if (loadOverCallback != null)
                {
                    loadOverCallback(newView);
                }
                if (openUrl != null)
                {
                    newView.LoadUrl(openUrl);
                }
            }
#endif
        }
#endif

        /// <summary>
        /// 在WebGL平台上面，直接删除运行过程中Clone的CanvasWebViewPrefab
        /// 在Google/iOS上面，卸载显示网页的节点上面挂载的UIWebGLWebView组件
        /// </summary>
        public void CloseWebView(GameObject closeView)
        {
#if UNITY_WEBGL
            for (int i = 0; i < m_WebUICanvas.transform.childCount; i++)
            {
                var transform = m_WebUICanvas.transform.GetChild(i);
                if (transform.gameObject == closeView)
                {
                    UnityEngine.Object.DestroyImmediate(transform.gameObject);
                    break;
                }
            }
#else
            if (closeView != null)
            {
                UIWebGLWebView webView = closeView.transform.GetComponent<UIWebGLWebView>();
                if (webView != null)
                {
                    UnityEngine.Object.DestroyImmediate(webView);
                }
            }

#endif
        }

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
                    ABPath = GameMainRoot.Procedure.UITransitionABPath,
                    AssetName = GameMainRoot.Procedure.UITransitionAssetName,
                    IsModal = false,
                    ZOrder = GameConstants.ProcedureTransitionUIZOrder,
                    Priority = 0,
                    CloseOnEscapeKeyUp = false,
                    BlockingMask = "Everything",
                    BlockingObjects = BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject transitionUIGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
                if (transitionUIGO == null)
                {
                    Log.Error("TransitionUIGO 无效。");
                    return;
                }
                m_TransitionUI = transitionUIGO.GetComponent<UILauncherLogoView>();
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
                    ABPath = GameMainRoot.Procedure.UITransitionABPath,
                    AssetName = GameMainRoot.Procedure.UITransitionAssetName,
                    IsModal = false,
                    ZOrder = GameConstants.ProcedureTransitionUIZOrder,
                    Priority = 0,
                    CloseOnEscapeKeyUp = false,
                    BlockingMask = "Everything",
                    BlockingObjects = BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject transitionUIGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
                if (transitionUIGO == null)
                {
                    Log.Error("TransitionUIGO 无效。");
                    return;
                }
                m_TransitionUI = transitionUIGO.GetComponent<UILauncherLogoView>();
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
                BlockingObjects = BlockingObjects.None,
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
        public UILauncherView ShowSplash(Action durationOverCallback)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
                ABPath = GameMainRoot.Procedure.UISplashABPath,
                AssetName = GameMainRoot.Procedure.UISplashAssetName,
                IsModal = false,
                ZOrder = GameConstants.SplashUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
            UILauncherView uiLauncherView = uiGO.GetComponent<UILauncherView>();
            uiLauncherView.DurationOverCallback = durationOverCallback;
            return uiLauncherView;
        }

        /// <summary>
        /// 展示App大版本更新提示界面
        /// </summary>
        /// <returns></returns>
        public UIAppDownloadBehaviour ShowAppDownload(bool showCloseButton)
        {
            // 打开大版本更新提示界面
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
                ABPath = "aa",//GameMainRoot.Hotfix.UIAppDownloadABPath,
                AssetName = "aa",
                IsModal = false,
                ZOrder = GameConstants.AppDownloadUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
            UIAppDownloadBehaviour uiAppDownloadBehaviour = uiGO.GetComponent<UIAppDownloadBehaviour>();
            uiAppDownloadBehaviour.InitView(showCloseButton);
            return uiAppDownloadBehaviour;
        }

        /// <summary>
        /// 展示GDPR界面
        /// </summary>
        /// <param name="inGame">是否为游戏中重复进入</param>
        /// <param name="overButtonClickedCallback">over按钮点击外部回调</param>
        /// <returns></returns>
        public UIGDPRBehaviour ShowGDPR(bool inGame, Action overButtonClickedCallback)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
               /* ABPath = Root.SDK.UIGDPRABPath,
                AssetName = Root.SDK.UIGDPRAssetName,*/
                IsModal = false,
                ZOrder = GameConstants.GDPRUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
            UIGDPRBehaviour uiGDPRBehaviour = uiGO.GetComponent<UIGDPRBehaviour>();
            uiGDPRBehaviour.InGame = inGame;
            uiGDPRBehaviour.OnOverButtonClickedCallback = overButtonClickedCallback;
            return uiGDPRBehaviour;
        }

        /// <summary>
        /// 展示loading界面
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        /// <returns></returns>
        public UILauncherLoadingView ShowLoading(UILauncherLoadingView.LoadingMode loadingMode)
        {
            string abPath = string.Empty;
            string assetName = string.Empty;
            int ZOrder = 0;
            switch(loadingMode)
            { 
                case UILauncherLoadingView.LoadingMode.Preload: abPath = GameMainRoot.Procedure.UIPreloadABPath; assetName = GameMainRoot.Procedure.UIPreloadAssetName; ZOrder = GameConstants.PreloadUIZOrder; break;
                default: Log.Error("UIManager.ShowLoading 参数错误。"); break;
            }

            if(!string.IsNullOrEmpty(abPath) && !string.IsNullOrEmpty(assetName))
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
                    BlockingObjects = BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
                UILauncherLoadingView uiLauncherLoadingView = uiGO.GetComponent<UILauncherLoadingView>();
                uiLauncherLoadingView.SetLoadingMode(loadingMode);
                return uiLauncherLoadingView;
            }

            return null;
        }

        /// <summary>
        /// 隐藏loading界面
        /// </summary>
        /// <param name="uiLauncher">待隐藏的ui</param>
        /// <returns></returns>
        public void HideLoading(UILauncherLoadingView uiLauncher)
        {
            uiLauncher.SetVisible(false);
        }
        /// <summary>
        /// 展示App应用内评价
        /// </summary>
        /// <returns></returns>
        public UIAppReviewBehaviour ShowAppReview()
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
               /* ABPath = Root.SDK.UIAppReviewABPath,
                AssetName = Root.SDK.UIAppReviewAssetName,*/
                IsModal = false,
                ZOrder = GameConstants.AppReviewUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
            UIAppReviewBehaviour uiAppReviewBehaviour = uiGO.GetComponent<UIAppReviewBehaviour>();
            return uiAppReviewBehaviour;
        }

        /// <summary>
        /// 展示App应用内反馈
        /// </summary>
        /// <param name="starNum">评星数量</param>
        /// <param name="locationDescForDot">打点位置描述</param>
        /// <returns></returns>
        public UIAppFeedbackBehaviour ShowAppFeedback(int starNum, string locationDescForDot)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
              /*  ABPath = Root.SDK.UIAppFeedbackABPath,
                AssetName = Root.SDK.UIAppFeedbackAssetName,*/
                IsModal = false,
                ZOrder = GameConstants.AppFeedbackUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };
            GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
            UIAppFeedbackBehaviour uiAppFeedbackBehaviour = uiGO.GetComponent<UIAppFeedbackBehaviour>();
            uiAppFeedbackBehaviour.StarNum = starNum;
            uiAppFeedbackBehaviour.LocationDescForDot = locationDescForDot;
            return uiAppFeedbackBehaviour;
        }

    }

}


