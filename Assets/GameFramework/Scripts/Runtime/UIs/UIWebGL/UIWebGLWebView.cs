using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_WEBGL && WEBVIEW_ENABLE
using Vuplex.WebView;
#endif

namespace Honor.Runtime
{
    /// <summary>
    /// A generic version of System.EventArgs.
    /// </summary>
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; private set; }

        public EventArgs(T val) => Value = val;
    }

    /// <summary>
    /// A generic version of System.EventArgs.
    /// </summary>
    public class LoadEventArgs<T> : EventArgs
    {
        public LoadEventArgs(T type, float progress, string message = "")
        {
            Type = type;
            Progress = progress;
            Message = message;
        }

        /// <summary>
        /// The estimated load progress, normalized to a float between 0 and 1.
        /// </summary>
        public readonly float Progress;

        /// <summary>
        /// The load progress event type.
        /// </summary>
        public readonly T Type;

        /// <summary>
        /// The load message.
        /// </summary>
        public readonly string Message;
    }

    [DisallowMultipleComponent]
    public class UIWebGLWebView : MonoBehaviour
    {
        [Tooltip("Input Url")]
        [GameTitle("Initial Url")]
        public string InitialUrl;
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        private bool m_WebGLVuplexViewInitialized = false;
        public bool WebGLVuplexViewInitialized
        {
            get => m_WebGLVuplexViewInitialized;
        }

        /// <summary>
        /// 当前展开的WebView
        /// </summary>
#if WEBVIEW_ENABLE
#if UNITY_WEBGL
        private CanvasWebViewPrefab m_WebView;
        public CanvasWebViewPrefab WebView
        { 
            set { m_WebView = value; }
            get { return m_WebView; }
        }
#else
        private UniWebView m_WebView;
        public UniWebView WebView
        {
            set { m_WebView = value; }
            get { return m_WebView; }
        }
#endif
#endif

        /// <summary>
        /// 载入事件
        /// </summary>
        public event EventHandler<LoadEventArgs<string>> OnLoadProgressChanged;

        /// <summary>
        /// 消息事件
        /// </summary>
        public event EventHandler<EventArgs<string>> OnMessageEmitted;

#if WEBVIEW_ENABLE
        /// <summary>
        /// UniWebView
        /// </summary>
        public event EventHandler<EventArgs<UniWebViewMessage>> OnMessageReceived;
#endif

        void Awake ()
        {
#if UNITY_WEBGL
            Debug.Log("CreateVuplexWebView !!!");
            // 删除CreateVuplexWebView接口,在UIManager统一处理
#else
            Debug.Log("CreateUniWebView !!!");
          //  CreateUniWebView();
#endif
        }

        private void OnDestroy()
        {
#if UNITY_WEBGL
#endif
        }

        /// <summary>
        /// webgl平台CanvasWebViewPrefab初始化完成回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userData"></param>
        /// <param name="e"></param>
        public void OnWebGLVuplexViewInitialized(object sender = null, object userData = null, EventParams e = null)
        {
#if UNITY_WEBGL
            if (m_WebGLVuplexViewInitialized == false)
            {
                var vuplexWebView = m_WebView as CanvasWebViewPrefab;
                vuplexWebView.WebView.CloseRequested += (sender, eventArgs) =>
                {
                    Debug.Log("CloseRequested");
                };

                vuplexWebView.WebView.LoadProgressChanged += (sender, eventArgs) =>
                {
                    OnLoadProgressChanged(sender, new LoadEventArgs<string>(eventArgs.Type.ToString(), eventArgs.Progress));
                };

                vuplexWebView.WebView.MessageEmitted += (sender, eventArgs) =>
                {
                    Debug.Log(AorTxt.Format("MessageEmitted {0}", eventArgs.Value));
                    OnMessageEmitted(sender, new EventArgs<string>(eventArgs.Value));
                };

                vuplexWebView.WebView.UrlChanged += (sender, eventArgs) =>
                {
                    Debug.Log(AorTxt.Format("UrlChanged {0}", eventArgs.Url));
                    OnMessageReceived(sender, new EventArgs<UniWebViewMessage>(new UniWebViewMessage(eventArgs.Url)));
                };

#if UNITY_WEBGL && !UNITY_EDITOR
                var webGLWebView = vuplexWebView.WebView as WebGLWebView;
                if (webGLWebView.CanAccessIFrameContent()) {
                    Debug.Log("The iframe content can be accessed 👍");
                }
#endif
                m_WebGLVuplexViewInitialized = true;
            }
#endif
        }

        /// <summary>
        /// 载入地址
        /// </summary>
        /// <param name="url"></param>
        public void LoadUrl(string url = null)
        {
#if WEBVIEW_ENABLE
#if UNITY_WEBGL
            if (!string.IsNullOrEmpty(url))
            {
                if (m_WebView != null && m_WebView.WebView != null)
                {
                    m_WebView.WebView.LoadUrl(url);
                }
            }
#else
            var webView = m_WebView as UniWebView;
            if (!string.IsNullOrEmpty(url))
            {
                webView.Load(url);
                webView.Show(true);
            }
#endif
#endif
        }

        /// <summary>
        /// 载入内容
        /// </summary>
        /// <param name="context"></param>
        public void LoadHtml(string context)
        {
#if UNITY_WEBGL
            if (!string.IsNullOrEmpty(context))
            {
                if (m_WebView != null && m_WebView.WebView != null)
                {
                    m_WebView.WebView.LoadHtml(context);
                }
            }
#else

#endif
        }


        /// <summary>
        /// 创建UniWebView
        /// </summary>
        public void CreateUniWebView()
        {
#if !UNITY_WEBGL && WEBVIEW_ENABLE
            var uniWebView = transform.GetOrAddComponent<UniWebView>();
            m_WebView = uniWebView;

            uniWebView.ReferenceRectTransform = transform.GetComponent<RectTransform>();

            uniWebView.OnPageStarted += (sender, url) =>
            {
                OnLoadProgressChanged(sender, new LoadEventArgs<string>("Started", 0f));
            };

            uniWebView.OnPageFinished += (sender, statusCode, url) =>
            {
                OnLoadProgressChanged(sender, new LoadEventArgs<string>("Finished", 1f));
            };

            uniWebView.OnPageErrorReceived += (sender, errorCode, errorMessage) =>
            {
                OnLoadProgressChanged(sender, new LoadEventArgs<string>("Failed", 0f, errorMessage));
            };

            uniWebView.OnPageProgressChanged += (sender, progress) =>
            {
                OnLoadProgressChanged(sender, new LoadEventArgs<string>("Progress", progress));
            };

            uniWebView.OnMessageReceived += (sender, message) =>
            {
                Debug.Log(Txt.Format("OnMessageReceived {0}", message.RawMessage));
                OnMessageReceived(sender, new EventArgs<UniWebViewMessage>(message));
            };

            //LoadUrl(InitialUrl);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void PostMessage(string context)
        {
#if UNITY_WEBGL
            var webView = m_WebView as CanvasWebViewPrefab;
            webView.WebView.PostMessage(context);
#else
            // UniWebView 不支持
#endif
        }

        /// <summary>
        /// 执行js脚本
        /// </summary>
        /// <param name="javaScript"></param>
        public void ExecuteJavaScript(string javaScript)
        {
#if WEBVIEW_ENABLE
#if UNITY_WEBGL

#if UNITY_WEBGL && !UNITY_EDITOR
            //WebGLWebView.ExecuteJavaScriptLocally(javaScript);
#endif
            if (javaScript != null)
            {
                if (m_WebView != null && m_WebView.WebView != null)
                {
                    m_WebView.WebView.ExecuteJavaScript(javaScript);
                }
            }
#else
            if (m_WebView != null)
            {
                m_WebView.EvaluateJavaScript(javaScript);
            }
#endif
#endif
        }

        public void Reload()
        {
#if WEBVIEW_ENABLE
#if !UNITY_WEBGL
            if (m_WebView != null)
            {
                m_WebView.Reload();
            }
#else
            Log.Debug("UNITY_WEBGL 不支持这个接口");
#endif
#endif
        }

#if WEBVIEW_ENABLE
        public void Show(bool fade = false, UniWebViewTransitionEdge edge = UniWebViewTransitionEdge.None,
            float duration = 0.4f, Action completionHandler = null)
        {
#if !UNITY_WEBGL
            // 统一接口，禁用
/*
            var webView = m_WebView as UniWebView;
            webView.Show(fade, edge, duration, completionHandler);
*/
#endif
        }
#endif

#if WEBVIEW_ENABLE
        public void Hide(bool fade = false, UniWebViewTransitionEdge edge = UniWebViewTransitionEdge.None,
                float duration = 0.4f, Action completionHandler = null)
        {
#if !UNITY_WEBGL
            // 统一接口，禁用
/*
            var webView = m_WebView as UniWebView;
            webView.Hide(fade, edge, duration, completionHandler);
*/
#endif
        }
#endif


    }

}
