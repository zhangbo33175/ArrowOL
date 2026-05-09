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
        /// 全局等待菊花（Loading）界面 引用计数 +1
        /// 计数 > 0 时自动创建并显示等待界面
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
        /// 全局等待菊花（Loading）界面 引用计数 -1
        /// 计数 == 0 时隐藏等待界面
        /// </summary>
        public void SubWaitingRef()
        {
            m_WaitingUIRefCount--;
            if (m_WaitingUIRefCount < 0)
            {
                Log.Error("WaitingUI 引用计数不可为负数，请检查引用计数的加减调用。当前引用计数为：{0}", m_WaitingUIRefCount);
                m_WaitingUIRefCount = 0;
            }
            if (m_WaitingUIRefCount == 0)
            {
                _mConnectionWaitingUIConnection.SetVisible(false);
            }
        }

        /// <summary>
        /// 获取全局等待菊花界面是否可见
        /// </summary>
        /// <returns>是否可见</returns>
        public bool IsWaitingVisible()
        {
            if (_mConnectionWaitingUIConnection != null)
            {
                return _mConnectionWaitingUIConnection.IsVisible();
            }
            return false;
        }

        /// <summary>
        /// 设置等待界面的描述文本
        /// </summary>
        /// <param name="text">显示文本</param>
        public void SetWaitingDescText(string text)
        {
            if (_mConnectionWaitingUIConnection != null)
            {
                _mConnectionWaitingUIConnection.SetWaitingDescText(text);
            }
        }

        /// <summary>
        /// 在游戏内打开 WebView 网页视图
        /// 各平台自动适配（WebGL / Android / iOS）
        /// </summary>
        /// <param name="adaptRectTransform">用于适配大小的 RectTransform</param>
        /// <param name="openUrl">初始加载 URL，可为 null</param>
        /// <param name="loadOverCallback">WebView 初始化完成回调</param>
#if WEBVIEW_ENABLE
        public async void OpenWebView(RectTransform adaptRectTransform, string openUrl = null, WebViewLoadOverCallback loadOverCallback = null)
        {
#if UNITY_WEBGL
            var vuplexWebView = CanvasWebViewPrefab.Instantiate();
            vuplexWebView.transform.parent = m_WebUICanvas.transform;

            var localRectTransform = adaptRectTransform;
            var rectTransform = vuplexWebView.transform.GetComponent<RectTransform>();
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                (Screen.width - localRectTransform.rect.width * localRectTransform.localScale.x) * localRectTransform.pivot.x,
                localRectTransform.rect.width * localRectTransform.localScale.x);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                (Screen.height - localRectTransform.rect.height * localRectTransform.localScale.y) * localRectTransform.pivot.y,
                localRectTransform.rect.height * localRectTransform.localScale.y);

            vuplexWebView.InitialUrl = null;
            await vuplexWebView.WaitUntilInitialized();
            
            UIWebGLWebView newView = vuplexWebView.transform.GetOrAddComponent<UIWebGLWebView>();
            newView.WebView = vuplexWebView;
            newView.OnWebGLVuplexViewInitialized();
            
            loadOverCallback?.Invoke(newView);
            if (openUrl != null)
            {
                newView.LoadUrl(openUrl);
            }
#else
            if (adaptRectTransform != null)
            {
                UIWebGLWebView newView = adaptRectTransform.transform.GetOrAddComponent<UIWebGLWebView>();
                newView.CreateUniWebView();
                loadOverCallback?.Invoke(newView);
                if (openUrl != null)
                {
                    newView.LoadUrl(openUrl);
                }
            }
#endif
        }
#endif

        /// <summary>
        /// 关闭并销毁 WebView
        /// WebGL：直接销毁克隆对象
        /// 原生平台：销毁组件
        /// </summary>
        /// <param name="closeView">要关闭的 WebView 对象</param>
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
                UIWebGLWebView webView = closeView.GetComponent<UIWebGLWebView>();
                if (webView != null)
                {
                    UnityEngine.Object.DestroyImmediate(webView);
                }
            }
#endif
        }

        /// <summary>
        /// 播放流程切换的入场转场动画（黑屏/白屏过渡）
        /// </summary>
        /// <param name="forceOver">是否立即完成，不播动画</param>
        /// <param name="duration">动画持续时间</param>
        /// <param name="blockRaycast">动画期间是否阻断射线（触摸）</param>
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
                m_TransitionUI.EnterOver();
            else
                m_TransitionUI.Enter();
        }

        /// <summary>
        /// 播放流程切换的退场转场动画
        /// </summary>
        /// <param name="forceOver">是否立即完成</param>
        /// <param name="duration">动画时长</param>
        /// <param name="blockRaycast">是否阻断触摸</param>
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
                m_TransitionUI.ExitOver();
            else
                m_TransitionUI.Exit();
        }

        /// <summary>
        /// 显示屏幕中央飘字提示（通用提示）
        /// </summary>
        /// <param name="text">提示内容</param>
        /// <param name="duration">显示时长</param>
        /// <param name="blockUITouches">是否阻断触摸</param>
        /// <param name="overCallback">动画结束回调</param>
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
        /// 显示启动 Splash 界面
        /// </summary>
        /// <param name="durationOverCallback">动画结束回调</param>
        /// <returns>启动界面控制组件</returns>
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
        /// 显示 App 大版本更新弹窗
        /// </summary>
        /// <param name="showCloseButton">是否显示关闭按钮</param>
        /// <returns>更新界面控制组件</returns>
        public UIAppDownloadBehaviour ShowAppDownload(bool showCloseButton)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
                ABPath = "aa",
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
        /// 显示 GDPR 隐私政策弹窗
        /// </summary>
        /// <param name="inGame">是否游戏内再次打开</param>
        /// <param name="overButtonClickedCallback">确认按钮回调</param>
        /// <returns>GDPR 控制组件</returns>
        public UIGDPRBehaviour ShowGDPR(bool inGame, Action overButtonClickedCallback)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
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
        /// 显示 Loading 界面（预加载、切换场景等）
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        /// <returns>Loading 控制组件</returns>
        public UILauncherLoadingView ShowLoading(UILauncherLoadingView.LoadingMode loadingMode)
        {
            string abPath = string.Empty;
            string assetName = string.Empty;
            int ZOrder = 0;

            switch (loadingMode)
            {
                case UILauncherLoadingView.LoadingMode.Preload:
                    abPath = GameMainRoot.Procedure.UIPreloadABPath;
                    assetName = GameMainRoot.Procedure.UIPreloadAssetName;
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
                    BlockingObjects = BlockingObjects.None,
                    MultiTypeTextCompsCoexist = false,
                    LuaParams = null,
                    OverCallback = null,
                };
                GameObject uiGO = GameMainRoot.UI.OpenUISyncByInfo(uiInfo);
                UILauncherLoadingView view = uiGO.GetComponent<UILauncherLoadingView>();
                view.SetLoadingMode(loadingMode);
                return view;
            }

            return null;
        }

        /// <summary>
        /// 隐藏 Loading 界面
        /// </summary>
        /// <param name="uiLauncher">Loading 界面组件</param>
        public void HideLoading(UILauncherLoadingView uiLauncher)
        {
            if (uiLauncher != null)
                uiLauncher.SetVisible(false);
        }

        /// <summary>
        /// 打开应用内评分（App Review）弹窗
        /// </summary>
        /// <returns>评分界面组件</returns>
        public UIAppReviewBehaviour ShowAppReview()
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
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
            return uiGO.GetComponent<UIAppReviewBehaviour>();
        }

        /// <summary>
        /// 打开应用内反馈（Feedback）界面
        /// </summary>
        /// <param name="starNum">评分星级</param>
        /// <param name="locationDescForDot">埋点描述</param>
        /// <returns>反馈界面组件</returns>
        public UIAppFeedbackBehaviour ShowAppFeedback(int starNum, string locationDescForDot)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = UIType.Screen,
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
            UIAppFeedbackBehaviour behaviour = uiGO.GetComponent<UIAppFeedbackBehaviour>();
            behaviour.StarNum = starNum;
            behaviour.LocationDescForDot = locationDescForDot;
            return behaviour;
        }
    }
}