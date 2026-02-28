using System;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIManager
    {
         /// <summary>
        /// 菊花等待界面ref+1
        /// </summary>
        public void AddWaitingRef()
        {
            m_UIComponent.AddWaitingRef();
        }

        /// <summary>
        /// 菊花等待界面ref-1
        /// </summary>
        public void SubWaitingRef()
        {
            m_UIComponent.SubWaitingRef();
        }

        /// <summary>
        /// 获取菊花等待界面的可见性
        /// </summary>
        public bool IsWaitingVisible()
        {
            return m_UIComponent.IsWaitingVisible();
        }

        /// <summary>
        /// 设置菊花等待界面显示文本
        /// </summary>
        /// <param name="text">文本内容</param>
        public void SetWaitingDescText(string text)
        {
            m_UIComponent.SetWaitingDescText(text);
        }

        /// <summary>
        /// 在游戏内打开webview
        /// </summary>
        /// <param name="viewRect">打开的webview挂载的节点</param>
        /// <param name="loadOverCallback">webview创建后的回调函数</param>
        /// <param name="openUrl">要打开的url链接，可为null</param>
#if WEBVIEW_ENABLE
        public void OpenWebView(RectTransform viewRect, string openUrl = null, WebViewLoadOverCallback loadOverCallback = null)
        {
            m_UIManager.OpenWebView(viewRect, openUrl, loadOverCallback);
        }
#endif

        /// <summary>
        /// 传入挂载UIWebGLWebView组件的节点，
        /// 在WebGL平台上面直接删除UIWebGLWebView组件的挂载节点
        /// 在Google/iOS平台上面从节点上面卸载UIWebGLWebView组件，删除网页显示
        /// </summary>
        public void CloseWebView(GameObject closeView)
        {
           // m_UIComponent.CloseWebView(closeView);
        }

        /// <summary>
        /// 显示流程切换过渡进入效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            m_UIComponent.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示流程切换过渡退出效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            m_UIComponent.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
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
            m_UIComponent.ShowFloatWords(text, duration, blockUITouches, overCallback);
        }

        /// <summary>
        /// 展示Splash闪屏界面
        /// </summary>
        /// <param name="durationOverCallback">延迟结束外部回调</param>
        /// <returns></returns>
        public UISplashBehaviour ShowSplash(Action durationOverCallback = null)
        {
            return m_UIComponent.ShowSplash(durationOverCallback);
        }
        
        /// <summary>
        /// 展示loading界面
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        /// <returns></returns>
        public UILoadingBehaviour ShowLoading(UILoadingBehaviour.LoadingMode loadingMode)
        {
            return m_UIComponent.ShowLoading(loadingMode);
        }

        /// <summary>
        /// 隐藏loading界面
        /// </summary>
        /// <param name="ui">待隐藏的ui</param>
        public void HideLoading(UILoadingBehaviour ui)
        {
            m_UIComponent.HideLoading(ui);
        }
        
        /*/// <summary>
        /// 展示App应用内评价
        /// </summary>
        /// <returns></returns>
        public UIAppReviewBehaviour ShowAppReview()
        {
            return m_UIComponent.ShowAppReview();
        }

        /// <summary>
        /// 展示App应用内反馈
        /// </summary>
        /// <param name="starNum">评星数量</param>
        /// <param name="locationDescForDot">打点位置描述</param>
        /// <returns></returns>
        public UIAppFeedbackBehaviour ShowAppFeedback(int starNum = 0, string locationDescForDot = null)
        {
            return m_UIComponent.ShowAppFeedback(starNum, locationDescForDot);
        }*/
    }
}