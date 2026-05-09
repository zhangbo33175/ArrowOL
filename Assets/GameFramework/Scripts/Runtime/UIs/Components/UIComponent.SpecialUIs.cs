using System;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// 等待菊花（Loading）引用计数 +1
        /// </summary>
        public void AddWaitingRef()
        {
            m_UIManager.AddWaitingRef();
        }

        /// <summary>
        /// 等待菊花（Loading）引用计数 -1
        /// </summary>
        public void SubWaitingRef()
        {
            m_UIManager.SubWaitingRef();
        }

        /// <summary>
        /// 获取等待菊花（Loading）是否正在显示
        /// </summary>
        /// <returns>是否可见</returns>
        public bool IsWaitingVisible()
        {
            return m_UIManager.IsWaitingVisible();
        }

        /// <summary>
        /// 设置等待菊花的描述文本
        /// </summary>
        /// <param name="text">显示文本</param>
        public void SetWaitingDescText(string text)
        {
            m_UIManager.SetWaitingDescText(text);
        }

        /// <summary>
        /// 关闭 WebView 网页视图（跨平台兼容）
        /// WebGL：直接删除节点
        /// Android/iOS：卸载组件、关闭网页
        /// </summary>
        /// <param name="closeView">挂载 UIWebGLWebView 的节点</param>
        public void CloseWebView(GameObject closeView)
        {
            m_UIManager.CloseWebView(closeView);
        }

        /// <summary>
        /// 显示流程切换的进入转场动画（黑屏/遮罩）
        /// </summary>
        /// <param name="forceOver">是否强制立即结束</param>
        /// <param name="duration">动画持续时间</param>
        /// <param name="blockRaycast">是否阻挡射线触摸</param>
        public void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            m_UIManager.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示流程切换的退出转场动画
        /// </summary>
        /// <param name="forceOver">是否强制立即结束</param>
        /// <param name="duration">动画持续时间</param>
        /// <param name="blockRaycast">是否阻挡射线触摸</param>
        public void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            m_UIManager.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示屏幕飘字（提示文字）
        /// </summary>
        /// <param name="text">飘字内容</param>
        /// <param name="duration">显示时长</param>
        /// <param name="blockUITouches">是否阻塞UI触摸</param>
        /// <param name="overCallback">动画结束回调</param>
        public void ShowFloatWords(string text, float duration = 0.5f, bool blockUITouches = false, Action overCallback = null)
        {
            m_UIManager.ShowFloatWords(text, duration, blockUITouches, overCallback);
        }

        /// <summary>
        /// 显示启动闪屏界面（Splash）
        /// </summary>
        /// <param name="durationOverCallback">延时结束回调</param>
        /// <returns>闪屏视图实例</returns>
        public UILauncherView ShowSplash(Action durationOverCallback = null)
        {
            return m_UIManager.ShowSplash(durationOverCallback);
        }

        /// <summary>
        /// 显示 App 大版本更新弹窗
        /// </summary>
        /// <param name="showCloseButton">是否显示关闭按钮</param>
        /// <returns>更新界面行为组件</returns>
        public UIAppDownloadBehaviour ShowAppDownload(bool showCloseButton = true)
        {
            return m_UIManager.ShowAppDownload(showCloseButton);
        }

        /// <summary>
        /// 显示 GDPR 隐私政策弹窗
        /// </summary>
        /// <param name="inGame">是否游戏内重复打开</param>
        /// <param name="overButtonClickedCallback">确认按钮回调</param>
        /// <returns>GDPR界面行为组件</returns>
        public UIGDPRBehaviour ShowGDPR(bool inGame = false, Action overButtonClickedCallback = null)
        {
            return m_UIManager.ShowGDPR(inGame, overButtonClickedCallback);
        }

        /// <summary>
        /// 显示 Loading 加载界面
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        /// <returns>Loading视图实例</returns>
        public UILauncherLoadingView ShowLoading(UILauncherLoadingView.LoadingMode loadingMode)
        {
            return m_UIManager.ShowLoading(loadingMode);
        }

        /// <summary>
        /// 隐藏 Loading 加载界面
        /// </summary>
        /// <param name="uiLauncher">需要隐藏的Loading实例</param>
        public void HideLoading(UILauncherLoadingView uiLauncher)
        {
            m_UIManager.HideLoading(uiLauncher);
        }

        /// <summary>
        /// 打开应用商店评分弹窗
        /// </summary>
        /// <returns>评分界面行为组件</returns>
        public UIAppReviewBehaviour ShowAppReview()
        {
            return m_UIManager.ShowAppReview();
        }

        /// <summary>
        /// 打开应用内反馈界面
        /// </summary>
        /// <param name="starNum">默认星级</param>
        /// <param name="locationDescForDot">埋点描述</param>
        /// <returns>反馈界面行为组件</returns>
        public UIAppFeedbackBehaviour ShowAppFeedback(int starNum = 0, string locationDescForDot = null)
        {
            return m_UIManager.ShowAppFeedback(starNum, locationDescForDot);
        }
    }
}