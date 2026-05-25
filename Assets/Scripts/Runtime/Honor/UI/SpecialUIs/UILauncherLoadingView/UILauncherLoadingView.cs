/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherLoadingView.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器加载界面（热更新 + 预加载）
 *            热更下载进度、预加载进度、按钮控制、多语言、事件监听
 ***************************************************************/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器加载界面（热更新 + 预加载）
    /// 功能：热更下载进度展示、预加载进度、按钮控制、多语言、事件监听
    /// </summary>
    public sealed class UILauncherLoadingView : MonoBehaviour
    {
        #region 枚举定义
        //=========================================================================
        // 枚举定义
        //=========================================================================
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
            /// 游戏内预加载模式
            /// </summary>
            Preload,
        }
        #endregion

        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        [SerializeField] private Button m_StartButton;

        /// <summary>
        /// 开始按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_StartButtonText;

        /// <summary>
        /// 开始按钮文字（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_StartButtonTextTMP;

        /// <summary>
        /// 重试下载按钮
        /// </summary>
        [SerializeField] private Button m_RetryButton;

        /// <summary>
        /// 重试按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_RetryButtonText;

        /// <summary>
        /// 重试按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_RetryButtonTextTMP;

        /// <summary>
        /// 关闭/退出按钮
        /// </summary>
        [SerializeField] private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_CloseButtonTextTMP;

        /// <summary>
        /// 进度条
        /// </summary>
        [SerializeField] private Slider m_ProgressSlider;

        /// <summary>
        /// 进度百分比文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_ProgressText;

        /// <summary>
        /// 进度百分比文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_ProgressTextTMP;

        /// <summary>
        /// 下载字节数文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_BytesNumText;

        /// <summary>
        /// 下载字节数文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_BytesNumTextTMP;

        /// <summary>
        /// 下载文件数文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_FileNumText;

        /// <summary>
        /// 下载文件数文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_FileNumTextTMP;

        /// <summary>
        /// 描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_DescText;

        /// <summary>
        /// 描述文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_DescTextTMP;
        #endregion

        #region 私有字段 & 公共属性
        //=========================================================================
        // 私有字段 & 公共属性
        //=========================================================================
        /// <summary>
        /// 当前加载模式
        /// </summary>
        private LoadingMode m_LoadingMode;

        /// <summary>
        /// 进度值 0~1
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
        /// 描述内容
        /// </summary>
        private string m_DescContent;

        /// <summary>
        /// 描述内容
        /// </summary>
        public string DescContent
        {
            set => m_DescContent = value;
        }
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================
        private void OnEnable()
        {
            // 注册事件：加载进度
            GameMainRoot.Event.Subscribe(GameEventCmd.LoadProgress, this, OnLoadProgressEventCallback);
            // 注册事件：热更进度
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixProgress, this, OnHotfixProgressEventCallback);
            // 注册事件：热更完成
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixAllOver, this, OnHotfixAllOverEventCallback);
            // 注册事件：热更错误
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixError, this, OnHotfixErrorEventCallback);
        }

        private void OnDisable()
        {
            // 注销事件
            GameMainRoot.Event.Unsubscribe(GameEventCmd.LoadProgress, this, OnLoadProgressEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixProgress, this, OnHotfixProgressEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixAllOver, this, OnHotfixAllOverEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixError, this, OnHotfixErrorEventCallback);
        }

        private void Start()
        {
            // 多语言按钮文本（WebGL特殊处理）
            if (m_StartButtonText != null)
                m_StartButtonText.text =
                    IsWebGL() ? "Start" : GameMainRoot.Localization.GetDefaultData("Hotfix_StartButton_Text");
            if (m_StartButtonTextTMP != null)
                m_StartButtonTextTMP.text =
                    IsWebGL() ? "Start" : GameMainRoot.Localization.GetDefaultData("Hotfix_StartButton_Text");
            if (m_RetryButtonText != null)
                m_RetryButtonText.text =
                    IsWebGL() ? "Retry" : GameMainRoot.Localization.GetDefaultData("Hotfix_RetryButton_Text");
            if (m_RetryButtonTextTMP != null)
                m_RetryButtonTextTMP.text =
                    IsWebGL() ? "Retry" : GameMainRoot.Localization.GetDefaultData("Hotfix_RetryButton_Text");
            if (m_CloseButtonText != null)
                m_CloseButtonText.text =
                    IsWebGL() ? "Close" : GameMainRoot.Localization.GetDefaultData("Hotfix_CloseButton_Text");
            if (m_CloseButtonTextTMP != null)
                m_CloseButtonTextTMP.text =
                    IsWebGL() ? "Close" : GameMainRoot.Localization.GetDefaultData("Hotfix_CloseButton_Text");
        }
        #endregion

        #region 公共控制方法
        //=========================================================================
        // 公共控制方法
        //=========================================================================
        /// <summary>
        /// 设置加载模式（热更/预加载）
        /// </summary>
        public void SetLoadingMode(LoadingMode loadingMode)
        {
            m_LoadingMode = loadingMode;

            // 预加载模式
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
            // 热更新模式
            else if (m_LoadingMode == LoadingMode.HotfixLauncher || m_LoadingMode == LoadingMode.HotfixIncreaser)
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
        /// 统一刷新界面显示
        /// </summary>
        public void RefreshViews(string descContent, float progress,
            GameDefinitions.DownloadStep downloadStep = GameDefinitions.DownloadStep.None, int curFileNum = 0,
            int totalFileNum = 0, float curBytes = 0f, float totalBytes = 0f)
        {
            // 热更新模式
            if (m_LoadingMode == LoadingMode.HotfixLauncher || m_LoadingMode == LoadingMode.HotfixIncreaser)
            {
                // 多语言提示文本
                string textReadyDownload = IsWebGL()
                    ? "Ready to download."
                    : GameMainRoot.Localization.GetDefaultData("Hotfix_Ready_Download_Text");
                string textDownloading = IsWebGL()
                    ? "Downloading: "
                    : GameMainRoot.Localization.GetDefaultData("Hotfix_Downloading_Text");
                string textDownloadAllOver = IsWebGL()
                    ? "It's downloaded."
                    : GameMainRoot.Localization.GetDefaultData("Hotfix_Download_AllOver_Text");
                string textDownloadFailedToSkip = IsWebGL()
                    ? "Download failed, ready to enter the game."
                    : GameMainRoot.Localization.GetDefaultData("Hotfix_Download_Failed_To_Skip_Text");
                string textDownloadError = IsWebGL()
                    ? "Download abnormal!Please try again."
                    : GameMainRoot.Localization.GetDefaultData("Hotfix_Download_Error_Text");

                SetProgress(progress);

                // 根据下载状态切换显示
                if (downloadStep == GameDefinitions.DownloadStep.Idle)
                {
                    SetProgressDescContent(textReadyDownload);
                    SetButtonsVisible(true, false, true);
                }
                else if (downloadStep == GameDefinitions.DownloadStep.Processing)
                {
                    if (string.IsNullOrEmpty(descContent))
                        SetProgressDescContent(descContent);
                    else
                        SetProgressDescContent(AorTxt.Format("{0} [{1:N2}M/{2:N2}M] {3:N0}%", textDownloading,
                            curBytes / 1024f / 1024f, totalBytes / 1024f / 1024f, progress * 100));

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
            // 预加载模式
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
        /// 设置界面显隐
        /// </summary>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
        #endregion

        #region UI按钮点击事件
        //=========================================================================
        // UI按钮点击事件
        //=========================================================================
        /// <summary>
        /// 开始按钮点击
        /// </summary>
        public void OnStartButtonClicked()
        {
        }

        /// <summary>
        /// 重试按钮点击
        /// </summary>
        public void OnRetryButtonClicked()
        {
        }

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        public void OnCloseButtonClicked()
        {
        }
        #endregion

        #region 私有刷新/设置方法
        //=========================================================================
        // 私有刷新/设置方法
        //=========================================================================
        /// <summary>
        /// 设置进度条与百分比
        /// </summary>
        private void SetProgress(float progress)
        {
            m_ProgressValue = progress;
            if (m_ProgressSlider != null) m_ProgressSlider.value = m_ProgressValue;
            if (m_ProgressText != null) m_ProgressText.text = AorTxt.Format("{0:N0}%", m_ProgressValue * 100);
            if (m_ProgressTextTMP != null) m_ProgressTextTMP.text = AorTxt.Format("{0:N0}%", m_ProgressValue * 100);
        }

        /// <summary>
        /// 设置进度描述文本
        /// </summary>
        private void SetProgressDescContent(string descContent)
        {
            if (m_ProgressText != null) m_ProgressText.text = descContent;
            if (m_ProgressTextTMP != null) m_ProgressTextTMP.text = descContent;
        }

        /// <summary>
        /// 设置文件数量显示
        /// </summary>
        private void SetFileNums(int curNum, int totalNum)
        {
            m_CurFileNum = curNum;
            m_TotalFileNum = totalNum;
            if (m_FileNumText != null) m_FileNumText.text = AorTxt.Format("({0} / {1})", m_CurFileNum, m_TotalFileNum);
            if (m_FileNumTextTMP != null)
                m_FileNumTextTMP.text = AorTxt.Format("({0} / {1})", m_CurFileNum, m_TotalFileNum);
        }

        /// <summary>
        /// 设置字节数显示（MB）
        /// </summary>
        private void SetBytesNums(float curNum, float totalNum)
        {
            m_CurBytes = curNum;
            m_TotalBytes = totalNum;
            if (m_BytesNumText != null)
                m_BytesNumText.text = AorTxt.Format("({0:N2} / {1:N2}MB) ", m_CurBytes / 1024f / 1024f,
                    m_TotalBytes / 1024f / 1024f);
            if (m_BytesNumTextTMP != null)
                m_BytesNumTextTMP.text = AorTxt.Format("({0:N2} / {1:N2}MB) ", m_CurBytes / 1024f / 1024f,
                    m_TotalBytes / 1024f / 1024f);
        }

        /// <summary>
        /// 设置通用描述文本
        /// </summary>
        private void SetDescContent(string descContent)
        {
            m_DescContent = descContent;
            if (m_DescText != null) m_DescText.text = m_DescContent;
            if (m_DescTextTMP != null) m_DescTextTMP.text = m_DescContent;
        }

        /// <summary>
        /// 设置三个按钮显隐
        /// </summary>
        private void SetButtonsVisible(bool startButtonVisible, bool retryButtonVisible, bool closeButtonVisible)
        {
            if (m_StartButton != null) m_StartButton.gameObject.SetActive(startButtonVisible);
            if (m_RetryButton != null) m_RetryButton.gameObject.SetActive(retryButtonVisible);
            if (m_CloseButton != null) m_CloseButton.gameObject.SetActive(closeButtonVisible);
        }

        /// <summary>
        /// 是否WebGL平台（用于文本差异化）
        /// </summary>
        private bool IsWebGL()
        {
            return false;
        }
        #endregion

        #region 事件回调
        //=========================================================================
        // 事件回调
        //=========================================================================
        /// <summary>
        /// 事件：加载进度回调
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
        /// 事件：热更进度回调
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

            RefreshViews(descContent, progress, GameDefinitions.DownloadStep.Processing, curFileNum, totalFileNum,
                curBytes, totalBytes);
            Log.Info("当前下载进度：{0:N2}%，当前已下载：{1:N2}MB，待下载：{2:N2}MB。", progress * 100, curBytes / 1024f / 1024f,
                totalBytes / 1024f / 1024f);
        }

        /// <summary>
        /// 事件：热更全部完成
        /// </summary>
        private void OnHotfixAllOverEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            RefreshViews(string.Empty, 1.0f, GameDefinitions.DownloadStep.AllOver, m_CurFileNum, m_TotalFileNum,
                m_CurBytes, m_TotalBytes);
            GameMainRoot.Event.Fire(this, GameEventCmd.FlowPermit);
            Log.Info("下载完毕！准备进入游戏。");
        }

        /// <summary>
        /// 事件：热更出错
        /// </summary>
        private void OnHotfixErrorEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            string fileName = e.GetString("fileName");
        }
        #endregion
    }
}