using System;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// 菊花等待界面ref+1
        /// </summary>
        public void AddWaitingRef()
        {
            m_UIManager.AddWaitingRef();
        }

        /// <summary>
        /// 菊花等待界面ref-1
        /// </summary>
        public void SubWaitingRef()
        {
            m_UIManager.SubWaitingRef();
        }

        /// <summary>
        /// 获取菊花等待界面的可见性
        /// </summary>
        public bool IsWaitingVisible()
        {
            return m_UIManager.IsWaitingVisible();
        }

        /// <summary>
        /// 设置菊花等待界面显示文本
        /// </summary>
        /// <param name="text">文本内容</param>
        public void SetWaitingDescText(string text)
        {
            m_UIManager.SetWaitingDescText(text);
        }
        
        /// <summary>
        /// 传入挂载UIWebGLWebView组件的节点，
        /// 在WebGL平台上面直接删除UIWebGLWebView组件的挂载节点
        /// 在Google/iOS平台上面从节点上面卸载UIWebGLWebView组件，删除网页显示
        /// </summary>
        public void CloseWebView(GameObject closeView)
        {
            m_UIManager.CloseWebView(closeView);
        }

        /// <summary>
        /// 显示流程切换过渡进入效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            m_UIManager.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示流程切换过渡退出效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            m_UIManager.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示飘字
        /// </summary>
        /// <param name="text">飘字内容</param>
        /// <param name="duration">飘字持续时间</param>
        /// <param name="blockUITouches">是否阻塞UI触摸</param>
        /// <param name="overCallback">结束回调</param>
        public void ShowFloatWords(string text, float duration = 0.5f, bool blockUITouches = false, Action overCallback = null)
        {
            m_UIManager.ShowFloatWords(text, duration, blockUITouches, overCallback);
        }

        /// <summary>
        /// 展示Splash闪屏界面
        /// </summary>
        /// <param name="durationOverCallback">延迟结束外部回调</param>
        /// <returns></returns>
        public UILauncherView ShowSplash(Action durationOverCallback = null)
        {
            return m_UIManager.ShowSplash(durationOverCallback);
        }

        /// <summary>
        /// 展示App大版本更新提示界面
        /// </summary>
        /// <returns></returns>
        public UIAppDownloadBehaviour ShowAppDownload(bool showCloseButton = true)
        {
            return m_UIManager.ShowAppDownload(showCloseButton);
        }

        /// <summary>
        /// 展示GDPR界面
        /// </summary>
        /// <param name="inGame">是否为游戏中重复进入</param>
        /// <param name="overButtonClickedCallback">over按钮点击外部回调</param>
        /// <returns></returns>
        public UIGDPRBehaviour ShowGDPR(bool inGame = false, Action overButtonClickedCallback = null)
        {
            return m_UIManager.ShowGDPR(inGame, overButtonClickedCallback);
        }

        /// <summary>
        /// 展示loading界面
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        /// <returns></returns>
        public UILauncherLoadingView ShowLoading(UILauncherLoadingView.LoadingMode loadingMode)
        {
            return m_UIManager.ShowLoading(loadingMode);
        }

        /// <summary>
        /// 隐藏loading界面
        /// </summary>
        /// <param name="uiLauncher">待隐藏的ui</param>
        public void HideLoading(UILauncherLoadingView uiLauncher)
        {
            m_UIManager.HideLoading(uiLauncher);
        }
        
        /// <summary>
        /// 展示App应用内评价
        /// </summary>
        /// <returns></returns>
        public UIAppReviewBehaviour ShowAppReview()
        {
            return m_UIManager.ShowAppReview();
        }

        /// <summary>
        /// 展示App应用内反馈
        /// </summary>
        /// <param name="starNum">评星数量</param>
        /// <param name="locationDescForDot">打点位置描述</param>
        /// <returns></returns>
        public UIAppFeedbackBehaviour ShowAppFeedback(int starNum = 0, string locationDescForDot = null)
        {
            return m_UIManager.ShowAppFeedback(starNum, locationDescForDot);
        }

    }

}


