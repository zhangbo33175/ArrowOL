using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed class UILauncherLoadingView:MonoBehaviour
    {
        /// <summary>
        /// 加载模式
        /// </summary>
        public enum LoadingMode : byte
        {
            /// <summary>
            /// 启动阶段热更新加载模式
            /// </summary>
            HotfixLauncher,

            /// <summary>
            /// 增量阶段热更新加载模式
            /// </summary>
            HotfixIncreaser,

            /// <summary>
            /// 预加载加载模式
            /// </summary>
            Preload,
        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        [SerializeField]
        private Button m_StartButton;

        /// <summary>
        /// 开始按钮文字
        /// </summary>
        [SerializeField]
        private Text m_StartButtonText;

        /// <summary>
        /// 开始按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_StartButtonTextTMP;

        /// <summary>
        /// 重试按钮
        /// </summary>
        [SerializeField]
        private Button m_RetryButton;

        /// <summary>
        /// 重试按钮文字
        /// </summary>
        [SerializeField]
        private Text m_RetryButtonText;

        /// <summary>
        /// 重试按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_RetryButtonTextTMP;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField]
        private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文字
        /// </summary>
        [SerializeField]
        private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_CloseButtonTextTMP;

        /// <summary>
        /// 进度条
        /// </summary>
        [SerializeField]
        private Slider m_ProgressSlider;

        /// <summary>
        /// 进度文本
        /// </summary>
        [SerializeField]
        private Text m_ProgressText;

        /// <summary>
        /// 进度文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_ProgressTextTMP;

        /// <summary>
        /// 字节数文本
        /// </summary>
        [SerializeField]
        private Text m_BytesNumText;

        /// <summary>
        /// 字节数文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_BytesNumTextTMP;

        /// <summary>
        /// 文件数文本
        /// </summary>
        [SerializeField]
        private Text m_FileNumText;

        /// <summary>
        /// 文件数文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_FileNumTextTMP;

        /// <summary>
        /// 描述内容文本
        /// </summary>
        [SerializeField]
        private Text m_DescText;

        /// <summary>
        /// 描述内容文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_DescTextTMP;
        
        /// <summary>
        /// 加载模式
        /// </summary>
        private LoadingMode m_LoadingMode;

        /// <summary>
        /// 进度值0~1
        /// </summary>
        private float m_ProgressValue;

        /// <summary>
        /// 当前已下载总字节数
        /// </summary>
        private float m_CurBytes;

        /// <summary>
        /// 待下载总字节数
        /// </summary>
        private float m_TotalBytes;

        /// <summary>
        /// 当前已下载文件数量
        /// </summary>
        private int m_CurFileNum;

        /// <summary>
        /// 待下载文件总数
        /// </summary>
        private int m_TotalFileNum;

        /// <summary>
        /// 描述性内容
        /// </summary>
        private string m_DescContent;
        public string DescContent
        {
            set
            {
                m_DescContent = value;
            }
        }

        private void OnEnable()
        {
            // 注册事件监听-加载进度
            GameMainRoot.Event.Subscribe(GameEventCmd.LoadProgress, this, OnLoadProgressEventCallback);

            // 注册事件监听-热更进度
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixProgress, this, OnHotfixProgressEventCallback);

            // 注册事件监听-热更完毕
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixAllOver, this, OnHotfixAllOverEventCallback);

            // 注册事件监听-热更异常
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixError, this, OnHotfixErrorEventCallback);
        }

        private void OnDisable()
        {
            // 注销事件监听-加载进度
            GameMainRoot.Event.Unsubscribe(GameEventCmd.LoadProgress, this, OnLoadProgressEventCallback);

            // 注销事件监听-热更进度
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixProgress, this, OnHotfixProgressEventCallback);

            // 注销事件监听-热更完毕
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixAllOver, this, OnHotfixAllOverEventCallback);

            // 注销事件监听-热更异常
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixError, this, OnHotfixErrorEventCallback);
            
        }

        private void Start()
        {
            if (m_StartButtonText != null) m_StartButtonText.text = IsWebGL() ? "Start" : GameMainRoot.Localization.GetDefaultData("Hotfix_StartButton_Text");
            if (m_StartButtonTextTMP != null) m_StartButtonTextTMP.text = IsWebGL() ? "Start" : GameMainRoot.Localization.GetDefaultData("Hotfix_StartButton_Text");
            if (m_RetryButtonText != null) m_RetryButtonText.text = IsWebGL() ? "Retry" : GameMainRoot.Localization.GetDefaultData("Hotfix_RetryButton_Text");
            if (m_RetryButtonTextTMP != null) m_RetryButtonTextTMP.text = IsWebGL() ? "Retry" : GameMainRoot.Localization.GetDefaultData("Hotfix_RetryButton_Text");
            if (m_CloseButtonText != null) m_CloseButtonText.text = IsWebGL() ? "Close" : GameMainRoot.Localization.GetDefaultData("Hotfix_CloseButton_Text");
            if (m_CloseButtonTextTMP != null) m_CloseButtonTextTMP.text = IsWebGL() ? "Close" : GameMainRoot.Localization.GetDefaultData("Hotfix_CloseButton_Text");
        }

        /// <summary>
        /// 设置加载模式
        /// </summary>
        /// <param name="loadingMode">加载模式类型</param>
        public void SetLoadingMode(LoadingMode loadingMode)
        {
            m_LoadingMode = loadingMode;
            if (m_LoadingMode == LoadingMode.Preload)
            {
                SetButtonsVisible(false, false, false);
                if (m_ProgressSlider != null) m_ProgressSlider.gameObject.SetActive(true);
                if (m_ProgressText != null) m_ProgressText.gameObject.SetActive(true);
                if (m_ProgressTextTMP != null) m_ProgressTextTMP.gameObject.SetActive(true);
                if (m_DescText != null) m_DescText.gameObject.SetActive(true);
                if (m_DescTextTMP != null) m_DescTextTMP.gameObject.SetActive(true);
                if (m_BytesNumText != null) m_BytesNumText.gameObject.SetActive(false);
                if (m_BytesNumTextTMP != null) m_BytesNumTextTMP.gameObject.SetActive(false);
                if (m_FileNumText != null) m_FileNumText.gameObject.SetActive(false);
                if (m_FileNumTextTMP != null) m_FileNumTextTMP.gameObject.SetActive(false);
                RefreshViews(string.Empty, 0f);
            }
            else if (m_LoadingMode == LoadingMode.HotfixLauncher || 
                     m_LoadingMode == LoadingMode.HotfixIncreaser)
            {
                SetButtonsVisible(true, true, true);
                if (m_ProgressSlider != null) m_ProgressSlider.gameObject.SetActive(true);
                if (m_ProgressText != null) m_ProgressText.gameObject.SetActive(true);
                if (m_ProgressTextTMP != null) m_ProgressTextTMP.gameObject.SetActive(true);
                if (m_DescText != null) m_DescText.gameObject.SetActive(true);
                if (m_DescTextTMP != null) m_DescTextTMP.gameObject.SetActive(true);
                if (m_BytesNumText != null) m_BytesNumText.gameObject.SetActive(true);
                if (m_BytesNumTextTMP != null) m_BytesNumTextTMP.gameObject.SetActive(true);
                if (m_FileNumText != null) m_FileNumText.gameObject.SetActive(true);
                if (m_FileNumTextTMP != null) m_FileNumTextTMP.gameObject.SetActive(true);
                RefreshViews(string.Empty, 0f, GameDefinitions.DownloadStep.Idle, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="descContent">描述信息</param>
        /// <param name="progress">进度值</param>
        /// <param name="downloadStep">下载步骤</param>
        /// <param name="curFileNum">当前文件数量</param>
        /// <param name="totalFileNum">文件总数量</param>
        /// <param name="curBytes">当前字节数</param>
        /// <param name="totalBytes">总字节数</param>
        public void RefreshViews(string descContent, float progress, GameDefinitions.DownloadStep downloadStep = GameDefinitions.DownloadStep.None, int curFileNum = 0, int totalFileNum = 0, float curBytes = 0f, float totalBytes = 0f)
        {
            // 热更新-加载模式
            if (m_LoadingMode == LoadingMode.HotfixLauncher || 
                m_LoadingMode == LoadingMode.HotfixIncreaser)
            {
                string textReadyDownload = IsWebGL() ? "Ready to download." : GameMainRoot.Localization.GetDefaultData("Hotfix_Ready_Download_Text");
                string textDownloading = IsWebGL() ? "Downloading: " : GameMainRoot.Localization.GetDefaultData("Hotfix_Downloading_Text");
                string textDownloadAllOver = IsWebGL() ? "It's downloaded." : GameMainRoot.Localization.GetDefaultData("Hotfix_Download_AllOver_Text");
                string textDownloadFailedToSkip = IsWebGL() ? "Download failed, ready to enter the game." : GameMainRoot.Localization.GetDefaultData("Hotfix_Download_Failed_To_Skip_Text");
                string textDownloadError = IsWebGL() ? "Download abnormal!Please try again." : GameMainRoot.Localization.GetDefaultData("Hotfix_Download_Error_Text");

                SetProgress(progress);
                //SetFileNums(curFileNum, totalFileNum);
                //SetBytesNums(curBytes, totalBytes);

                if (downloadStep == GameDefinitions.DownloadStep.Idle)
                {
                    SetProgressDescContent(textReadyDownload);
                    SetButtonsVisible(true, false, true);
                }
                else if (downloadStep == GameDefinitions.DownloadStep.Processing)
                {
                    if (string.IsNullOrEmpty(descContent))
                    {
                        SetProgressDescContent(descContent);
                    }
                    else
                    {
                        SetProgressDescContent(AorTxt.Format("{0} [{1:N2}M/{2:N2}M] {3:N0}%", textDownloading, curBytes / 1024f / 1024f,totalBytes / 1024f / 1024f, progress * 100));
                    }
                    SetButtonsVisible(false, false, false);
                }
                else if (downloadStep == GameDefinitions.DownloadStep.AllOver)
                {
                    SetProgressDescContent(textDownloadAllOver);
                    SetButtonsVisible(false, false, false);
                }
                else if (downloadStep == GameDefinitions.DownloadStep.Skip)
                {
                    SetProgressDescContent(textDownloadFailedToSkip);
                    SetButtonsVisible(false, false, false);
                }
                else if (downloadStep == GameDefinitions.DownloadStep.Error)
                {
                    SetProgressDescContent(textDownloadError);
                    SetButtonsVisible(false, true, false);
                }
            }
            // 预加载-加载模式
            else if (m_LoadingMode == LoadingMode.Preload)
            {
                SetProgress(progress);
                SetFileNums(curFileNum, totalFileNum);
                SetBytesNums(curBytes, totalBytes);
                SetDescContent(descContent);
                SetButtonsVisible(false, false, false);
            }
        }

        /// <summary>
        /// 开始按钮回调
        /// </summary>
        public void OnStartButtonClicked()
        {
            
        }

        /// <summary>
        /// 重试按钮回调
        /// </summary>
        public void OnRetryButtonClicked()
        {
            
        }

        /// <summary>
        /// 关闭按钮回调
        /// </summary>
        public void OnCloseButtonClicked()
        {
            
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="progress">进度值0~1</param>
        private void SetProgress(float progress)
        {
            m_ProgressValue = progress;
            if (m_ProgressSlider != null) m_ProgressSlider.value = m_ProgressValue;
            if (m_ProgressText != null) m_ProgressText.text = AorTxt.Format("{0:N0}%", m_ProgressValue * 100);
            if (m_ProgressTextTMP != null) m_ProgressTextTMP.text = AorTxt.Format("{0:N0}%", m_ProgressValue * 100);
        }
        
        /// <summary>
        /// 设置进度描述内容
        /// </summary>
        /// <param name="descContent"></param>
        private void SetProgressDescContent(string descContent)
        {
            if (m_ProgressText != null) m_ProgressText.text = descContent;
            if (m_ProgressTextTMP != null) m_ProgressTextTMP.text = descContent;
        }

        /// <summary>
        /// 设置文件数
        /// </summary>
        /// <param name="curNum">当前文件数</param>
        /// <param name="totalNum">待下载文件总数</param>
        private void SetFileNums(int curNum, int totalNum)
        {
            m_CurFileNum = curNum;
            m_TotalFileNum = totalNum;
            if (m_FileNumText != null) m_FileNumText.text = AorTxt.Format("({0} / {1})", m_CurFileNum, m_TotalFileNum);
            if (m_FileNumTextTMP != null) m_FileNumTextTMP.text = AorTxt.Format("({0} / {1})", m_CurFileNum, m_TotalFileNum);
        }

        /// <summary>
        /// 设置字节总
        /// </summary>
        /// <param name="curNum">当前字节数</param>
        /// <param name="totalNum">待下载字节总数</param>
        private void SetBytesNums(float curNum, float totalNum)
        {
            m_CurBytes = curNum;
            m_TotalBytes = totalNum;
            if (m_BytesNumText != null) m_BytesNumText.text = AorTxt.Format("({0:N2} / {1:N2}MB) ", m_CurBytes / 1024f / 1024f, m_TotalBytes / 1024f / 1024f);
            if (m_BytesNumTextTMP != null) m_BytesNumTextTMP.text = AorTxt.Format("({0:N2} / {1:N2}MB) ", m_CurBytes / 1024f / 1024f, m_TotalBytes / 1024f / 1024f);
        }

        /// <summary>
        /// 设置描述内容
        /// </summary>
        /// <param name="descContent">描述内容</param>
        private void SetDescContent(string descContent)
        {
            m_DescContent = descContent;
            if(m_DescText != null) m_DescText.text = m_DescContent;
            if(m_DescTextTMP != null) m_DescTextTMP.text = m_DescContent;
        }

        /// <summary>
        /// 设置按钮可见性
        /// </summary>
        /// <param name="startButtonVisible">开始按钮是否可见</param>
        /// <param name="retryButtonVisible">重试按钮是否可见</param>
        /// <param name="closeButtonVisible">关闭按钮是否可见</param>
        private void SetButtonsVisible(bool startButtonVisible, bool retryButtonVisible, bool closeButtonVisible)
        {
            if (m_StartButton != null) m_StartButton.gameObject.SetActive(startButtonVisible);
            if (m_RetryButton != null) m_RetryButton.gameObject.SetActive(retryButtonVisible);
            if (m_CloseButton != null) m_CloseButton.gameObject.SetActive(closeButtonVisible);
        }

        /// <summary>
        /// 设置可见性
        /// </summary>
        /// <param name="visible">可见性</param>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// 事件监听回调-加载进度
        /// </summary>
        private void OnLoadProgressEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;

            float progress = e.GetFloat("progress");
            string descContent = e.GetString("descContent");
            RefreshViews(descContent, progress);
            Log.Info("当前加载进度：{0:N2}%。", progress * 100);
        }

        /// <summary>
        /// 事件监听回调-热更进度
        /// </summary>
        private void OnHotfixProgressEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            
            float progress = e.GetFloat("progress");
            float curBytes = e.GetFloat("curBytes");
            float totalBytes = e.GetFloat("totalBytes");
            int curFileNum = e.GetInt("curFileNum");
            int totalFileNum = e.GetInt("totalFileNum");
            string descContent = e.GetString("descContent");
            RefreshViews(descContent, progress, GameDefinitions.DownloadStep.Processing, curFileNum, totalFileNum, curBytes, totalBytes);
            Log.Info("当前下载进度：{0:N2}%，当前已下载：{1:N2}MB，待下载：{2:N2}MB。", progress * 100, curBytes / 1024f / 1024f, totalBytes / 1024f / 1024f);
        }

        /// <summary>
        /// 事件监听回调-热更完毕
        /// </summary>
        private void OnHotfixAllOverEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            RefreshViews(string.Empty, 1.0f, GameDefinitions.DownloadStep.AllOver, m_CurFileNum, m_TotalFileNum, m_CurBytes, m_TotalBytes);
            GameMainRoot.Event.Fire(this, GameEventCmd.FlowPermit);
            Log.Info("下载完毕！准备进入游戏。");
        }

        /// <summary>
        /// 事件监听回调-热更异常
        /// </summary>
        private void OnHotfixErrorEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;

            string fileName = e.GetString("fileName");
        }

        /// <summary>
        /// 是否为WebGL
        /// </summary>
        /// <returns></returns>
        private bool IsWebGL()
        {
            return false;
        }
    }

}


