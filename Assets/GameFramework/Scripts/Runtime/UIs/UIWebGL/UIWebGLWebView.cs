using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WEBGL && WEBVIEW_ENABLE
using Vuplex.WebView;
#endif

namespace Honor.Runtime
{
    /// <summary>
    /// 泛型事件参数，用于传递简单数据
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// 事件携带的值
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="val">要传递的数据</param>
        public EventArgs(T val) => Value = val;
    }

    /// <summary>
    /// 加载进度事件参数（带类型、进度、消息）
    /// </summary>
    /// <typeparam name="T">进度类型</typeparam>
    public class LoadEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 构造加载进度参数
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="progress">0~1 进度</param>
        /// <param name="message">附加消息</param>
        public LoadEventArgs(T type, float progress, string message = "")
        {
            Type = type;
            Progress = progress;
            Message = message;
        }

        /// <summary>
        /// 加载进度（0 ~ 1）
        /// </summary>
        public readonly float Progress;

        /// <summary>
        /// 加载事件类型
        /// </summary>
        public readonly T Type;

        /// <summary>
        /// 加载消息（错误/状态）
        /// </summary>
        public readonly string Message;
    }

    /// <summary>
    /// WebView 统一管理组件
    /// 自动适配 WebGL / Android / iOS 平台
    /// </summary>
    [DisallowMultipleComponent]
    public class UIWebGLWebView : MonoBehaviour
    {
        /// <summary>
        /// 初始加载 URL
        /// </summary>
        [Tooltip("初始加载的网页地址")]
        [GameTitle("Initial Url")]
        public string InitialUrl;

        /// <summary>
        /// WebGL Vuplex WebView 是否初始化完成
        /// </summary>
        private bool m_WebGLVuplexViewInitialized;

        /// <summary>
        /// 获取 WebGL Vuplex WebView 初始化状态
        /// </summary>
        public bool WebGLVuplexViewInitialized => m_WebGLVuplexViewInitialized;

#if WEBVIEW_ENABLE
#if UNITY_WEBGL
        /// <summary>
        /// Vuplex WebView 实例（WebGL 平台）
        /// </summary>
        private CanvasWebViewPrefab m_WebView;

        public CanvasWebViewPrefab WebView
        {
            get => m_WebView;
            set => m_WebView = value;
        }
#else
        /// <summary
        /// UniWebView 实例（原生平台）
        /// </summary>
        private UniWebView m_WebView;

        public UniWebView WebView
        {
            get => m_WebView;
            set => m_WebView = value;
        }
#endif
#endif

        /// <summary>
        /// 加载进度变化事件
        /// </summary>
        public event EventHandler<LoadEventArgs<string>> OnLoadProgressChanged;

        /// <summary>
        /// 网页消息抛出事件
        /// </summary>
        public event EventHandler<EventArgs<string>> OnMessageEmitted;

#if WEBVIEW_ENABLE
        /// <summary>
        /// 原生平台消息接收事件
        /// </summary>
        public event EventHandler<EventArgs<UniWebViewMessage>> OnMessageReceived;
#endif

        private void Awake()
        {
#if UNITY_WEBGL
            Debug.Log("CreateVuplexWebView !!!");
#else
            Debug.Log("CreateUniWebView !!!");
#endif
        }

        private void OnDestroy()
        {
            // 清理逻辑
        }

        /// <summary>
        /// WebGL 平台 Vuplex WebView 初始化完成回调
        /// </summary>
        public void OnWebGLVuplexViewInitialized(object sender = null, object userData = null, EventParams e = null)
        {
#if UNITY_WEBGL
            if (m_WebGLVuplexViewInitialized) return;

            var vuplexWebView = m_WebView;
            vuplexWebView.WebView.CloseRequested += (_, _) =>
            {
                Debug.Log("CloseRequested");
            };

            vuplexWebView.WebView.LoadProgressChanged += (_, eventArgs) =>
            {
                OnLoadProgressChanged?.Invoke(this,
                    new LoadEventArgs<string>(eventArgs.Type.ToString(), eventArgs.Progress));
            };

            vuplexWebView.WebView.MessageEmitted += (_, eventArgs) =>
            {
                Debug.Log($"MessageEmitted: {eventArgs.Value}");
                OnMessageEmitted?.Invoke(this, new EventArgs<string>(eventArgs.Value));
            };

            vuplexWebView.WebView.UrlChanged += (_, eventArgs) =>
            {
                Debug.Log($"UrlChanged: {eventArgs.Url}");
                OnMessageReceived?.Invoke(this,
                    new EventArgs<UniWebViewMessage>(new UniWebViewMessage(eventArgs.Url)));
            };

            m_WebGLVuplexViewInitialized = true;
#endif
        }

        /// <summary>
        /// 加载指定 URL
        /// </summary>
        /// <param name="url">网页地址</param>
        public void LoadUrl(string url = null)
        {
#if WEBVIEW_ENABLE
#if UNITY_WEBGL
            if (!string.IsNullOrEmpty(url) && m_WebView?.WebView != null)
            {
                m_WebView.WebView.LoadUrl(url);
            }
#else
            if (m_WebView != null && !string.IsNullOrEmpty(url))
            {
                m_WebView.Load(url);
                m_WebView.Show(true);
            }
#endif
#endif
        }

        /// <summary>
        /// 直接加载 HTML 内容
        /// </summary>
        /// <param name="context">HTML 字符串</param>
        public void LoadHtml(string context)
        {
#if UNITY_WEBGL
            if (!string.IsNullOrEmpty(context) && m_WebView?.WebView != null)
            {
                m_WebView.WebView.LoadHtml(context);
            }
#endif
        }

        /// <summary>
        /// 创建原生平台 UniWebView
        /// </summary>
        public void CreateUniWebView()
        {
#if !UNITY_WEBGL && WEBVIEW_ENABLE
            var uniWebView = transform.GetOrAddComponent<UniWebView>();
            m_WebView = uniWebView;
            uniWebView.ReferenceRectTransform = transform.GetComponent<RectTransform>();

            uniWebView.OnPageStarted += (sender, _) =>
            {
                OnLoadProgressChanged?.Invoke(sender, new LoadEventArgs<string>("Started", 0f));
            };

            uniWebView.OnPageFinished += (sender, _, _) =>
            {
                OnLoadProgressChanged?.Invoke(sender, new LoadEventArgs<string>("Finished", 1f));
            };

            uniWebView.OnPageErrorReceived += (sender, code, msg) =>
            {
                OnLoadProgressChanged?.Invoke(sender, new LoadEventArgs<string>("Failed", 0f, msg));
            };

            uniWebView.OnPageProgressChanged += (sender, progress) =>
            {
                OnLoadProgressChanged?.Invoke(sender, new LoadEventArgs<string>("Progress", progress));
            };

            uniWebView.OnMessageReceived += (sender, message) =>
            {
                Debug.Log($"OnMessageReceived: {message.RawMessage}");
                OnMessageReceived?.Invoke(sender, new EventArgs<UniWebViewMessage>(message));
            };
#endif
        }

        /// <summary>
        /// 向网页发送消息
        /// </summary>
        /// <param name="context">消息内容</param>
        public void PostMessage(string context)
        {
#if UNITY_WEBGL && WEBVIEW_ENABLE
            if (m_WebView != null)
            {
                m_WebView.WebView.PostMessage(context);
            }
#endif
        }

        /// <summary>
        /// 执行 JavaScript 脚本
        /// </summary>
        /// <param name="javaScript">JS 代码</param>
        public void ExecuteJavaScript(string javaScript)
        {
#if WEBVIEW_ENABLE
#if UNITY_WEBGL
            if (!string.IsNullOrEmpty(javaScript) && m_WebView?.WebView != null)
            {
                m_WebView.WebView.ExecuteJavaScript(javaScript);
            }
#else
            if (m_WebView != null)
            {
                m_WebView.EvaluateJavaScript(javaScript);
            }
#endif
#endif
        }

        /// <summary>
        /// 重新加载页面（原生平台）
        /// </summary>
        public void Reload()
        {
#if WEBVIEW_ENABLE && !UNITY_WEBGL
            m_WebView?.Reload();
#endif
        }
    }
}