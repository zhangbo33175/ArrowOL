/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherLoadingView.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器加载界面（热更新 + 预加载）
 *            预加载进度展示、热更下载进度、按钮控制、多语言、事件监听
 *            调用链：ShowLoading(模式) → SetLoadingMode → 事件(LoadProgress/Hotfix*) → RefreshViews
 ***************************************************************/

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器加载界面（热更新 + 预加载）
    /// 功能：预加载进度展示、热更下载进度、按钮控制、多语言、事件监听
    /// 调用链：
    ///   1) 外部通过 <see cref="GameMainRoot.UI.ShowLoading(UILauncherLoadingView.LoadingMode)"/> 打开界面，
    ///      内部调用 <see cref="SetLoadingMode"/> 初始化显示；
    ///   2) 界面在 <see cref="OnEnable"/> 订阅 LoadProgress / HotfixProgress 等事件；
    ///   3) 进度变化时回调进入 <see cref="RefreshViews"/> 统一刷新界面。
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
        // 说明：每个文案同时预留 UGUI(Text) 与 TextMeshPro(TMP) 两套引用，按实际预制体接线取其一；
        //       当前 Preload 预制体仅接线 进度条(Slider) 与 进度TMP文本，按钮/字节/文件/描述为热更模式预留。
        //=========================================================================

        /// <summary>
        /// 开始游戏按钮（热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("开始游戏按钮（热更模式）")]
        private Button m_StartButton;

        /// <summary>
        /// 开始按钮文字（UGUI Text）
        /// </summary>
        [SerializeField]
        [Tooltip("开始按钮文字（UGUI Text）")]
        private Text m_StartButtonText;

        /// <summary>
        /// 开始按钮文字（TextMeshProUGUI）
        /// </summary>
        [SerializeField]
        [Tooltip("开始按钮文字（TextMeshProUGUI）")]
        private TextMeshProUGUI m_StartButtonTextTMP;

        /// <summary>
        /// 重试下载按钮（热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("重试下载按钮（热更模式）")]
        private Button m_RetryButton;

        /// <summary>
        /// 重试按钮文字（UGUI Text）
        /// </summary>
        [SerializeField]
        [Tooltip("重试按钮文字（UGUI Text）")]
        private Text m_RetryButtonText;

        /// <summary>
        /// 重试按钮文字（TMP）
        /// </summary>
        [SerializeField]
        [Tooltip("重试按钮文字（TextMeshProUGUI）")]
        private TextMeshProUGUI m_RetryButtonTextTMP;

        /// <summary>
        /// 关闭/退出按钮（热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("关闭/退出按钮（热更模式）")]
        private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文字（UGUI Text）
        /// </summary>
        [SerializeField]
        [Tooltip("关闭按钮文字（UGUI Text）")]
        private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文字（TMP）
        /// </summary>
        [SerializeField]
        [Tooltip("关闭按钮文字（TextMeshProUGUI）")]
        private TextMeshProUGUI m_CloseButtonTextTMP;

        /// <summary>
        /// 进度条
        /// </summary>
        [SerializeField]
        [Tooltip("加载进度条")]
        private Slider m_ProgressSlider;

        /// <summary>
        /// 进度百分比文本（UGUI Text）
        /// </summary>
        [SerializeField]
        [Tooltip("进度百分比文本（UGUI Text）")]
        private Text m_ProgressText;

        /// <summary>
        /// 进度百分比文本（TMP）
        /// </summary>
        [SerializeField]
        [Tooltip("进度百分比文本（TextMeshProUGUI）")]
        private TextMeshProUGUI m_ProgressTextTMP;

        /// <summary>
        /// 下载字节数文本（UGUI Text，热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("下载字节数文本（UGUI Text，热更模式）")]
        private Text m_BytesNumText;

        /// <summary>
        /// 下载字节数文本（TMP，热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("下载字节数文本（TextMeshProUGUI，热更模式）")]
        private TextMeshProUGUI m_BytesNumTextTMP;

        /// <summary>
        /// 下载文件数文本（UGUI Text，热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("下载文件数文本（UGUI Text，热更模式）")]
        private Text m_FileNumText;

        /// <summary>
        /// 下载文件数文本（TMP，热更模式）
        /// </summary>
        [SerializeField]
        [Tooltip("下载文件数文本（TextMeshProUGUI，热更模式）")]
        private TextMeshProUGUI m_FileNumTextTMP;

        /// <summary>
        /// 描述文本（UGUI Text）
        /// </summary>
        [SerializeField]
        [Tooltip("描述文本（UGUI Text）")]
        private Text m_DescText;

        /// <summary>
        /// 描述文本（TMP）
        /// </summary>
        [SerializeField]
        [Tooltip("描述文本（TextMeshProUGUI）")]
        private TextMeshProUGUI m_DescTextTMP;
        #endregion

        #region 切换过渡配置（Inspector 可调）
        //=========================================================================
        // 切换过渡配置（进入/退出，可在 Inspector 框式行中调整）
        // 说明：进入过渡在加载界面显示时淡入，退出过渡在隐藏时淡出，使启动衔接更丝滑。
        //=========================================================================

        /// <summary>
        /// 进入式-切换过渡：是否启用淡入动画
        /// </summary>
        [SerializeField]
        [Tooltip("进入式-切换过渡：显示加载界面时是否播放淡入动画")]
        private bool m_EnterTransitionEnabled = true;

        /// <summary>
        /// 进入过渡时长（秒）
        /// </summary>
        [SerializeField]
        [Tooltip("进入过渡时长（秒）")]
        private float m_EnterTransitionDuration = 0.5f;

        /// <summary>
        /// 退出式-切换过渡：是否启用淡出动画
        /// </summary>
        [SerializeField]
        [Tooltip("退出式-切换过渡：隐藏加载界面时是否播放淡出动画")]
        private bool m_ExitTransitionEnabled = true;

        /// <summary>
        /// 退出过渡时长（秒）
        /// </summary>
        [SerializeField]
        [Tooltip("退出过渡时长（秒）")]
        private float m_ExitTransitionDuration = 0.5f;

        /// <summary>
        /// 进入式-切换过渡：是否启用淡入动画
        /// </summary>
        public bool EnterTransitionEnabled
        {
            set => m_EnterTransitionEnabled = value;
            get => m_EnterTransitionEnabled;
        }

        /// <summary>
        /// 进入过渡时长（秒）
        /// </summary>
        public float EnterTransitionDuration
        {
            set => m_EnterTransitionDuration = value;
            get => m_EnterTransitionDuration;
        }

        /// <summary>
        /// 退出式-切换过渡：是否启用淡出动画
        /// </summary>
        public bool ExitTransitionEnabled
        {
            set => m_ExitTransitionEnabled = value;
            get => m_ExitTransitionEnabled;
        }

        /// <summary>
        /// 退出过渡时长（秒）
        /// </summary>
        public float ExitTransitionDuration
        {
            set => m_ExitTransitionDuration = value;
            get => m_ExitTransitionDuration;
        }
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
        /// 描述内容（外部通过该属性注入最新描述，RefreshViews 统一应用）
        /// </summary>
        public string DescContent
        {
            set => m_DescContent = value;
        }

        /// <summary>
        /// 描述内容缓存
        /// </summary>
        private string m_DescContent;

        /// <summary>
        /// 过渡动画所用的 CanvasGroup（运行时按需获取/创建，控制整界淡入淡出）
        /// </summary>
        private CanvasGroup m_CanvasGroup;
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================

        /// <summary>
        /// 唤醒：仅获取预制体上预挂的 CanvasGroup（用于整界淡入淡出过渡）
        /// 说明：不在 Awake/OnEnable 中运行时 AddComponent 创建——对象经 Instantiate 创建时，
        ///       其生命周期回调期间 AddComponent 会失败（返回 null），导致淡出动画崩溃；
        ///       因此 CanvasGroup 必须在预制体上预挂，代码只做判空读取。
        /// </summary>
        private void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// 启用时注册加载进度与热更相关事件，并播放进入过渡（淡入）
        /// 说明：先重置进度为 0，避免界面出现时沿用预制体 Slider 默认值（m_Value=1）或上次残留，
        ///       保证进度条从头开始增长。
        /// </summary>
        private void OnEnable()
        {
            // 重置进度显示：界面出现时进度条从 0 开始
            SetProgress(0f);

            GameMainRoot.Event.Subscribe(GameEventCmd.LoadProgress, this, OnLoadProgressEventCallback);
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixProgress, this, OnHotfixProgressEventCallback);
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixAllOver, this, OnHotfixAllOverEventCallback);
            GameMainRoot.Event.Subscribe(GameEventCmd.HotfixError, this, OnHotfixErrorEventCallback);

            // 显示界面时播放进入淡入动画（首次打开与重新显示均生效）
            PlayEnterTransition();
        }

        /// <summary>
        /// 禁用时注销加载进度与热更相关事件
        /// </summary>
        private void OnDisable()
        {
            GameMainRoot.Event.Unsubscribe(GameEventCmd.LoadProgress, this, OnLoadProgressEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixProgress, this, OnHotfixProgressEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixAllOver, this, OnHotfixAllOverEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.HotfixError, this, OnHotfixErrorEventCallback);
        }

        /// <summary>
        /// 启动时按平台与多语言设置各按钮文本
        /// 说明：仅当对应按钮文本组件已接线时才获取多语言文案；
        ///       当前 Preload 预制体未接线热更按钮，直接跳过，避免在语言尚未初始化时
        ///       触发 LocalizationManager 的“language 无效”异常。
        /// </summary>
        private void Start()
        {
            // 开始按钮
            if (m_StartButtonText != null || m_StartButtonTextTMP != null)
                SetText(m_StartButtonText, m_StartButtonTextTMP,
                    IsWebGL() ? "Start" : GameMainRoot.Localization.GetDefaultData("Hotfix_StartButton_Text"));

            // 重试按钮
            if (m_RetryButtonText != null || m_RetryButtonTextTMP != null)
                SetText(m_RetryButtonText, m_RetryButtonTextTMP,
                    IsWebGL() ? "Retry" : GameMainRoot.Localization.GetDefaultData("Hotfix_RetryButton_Text"));

            // 关闭按钮
            if (m_CloseButtonText != null || m_CloseButtonTextTMP != null)
                SetText(m_CloseButtonText, m_CloseButtonTextTMP,
                    IsWebGL() ? "Close" : GameMainRoot.Localization.GetDefaultData("Hotfix_CloseButton_Text"));
        }
        #endregion

        #region 公共接口
        //=========================================================================
        // 公共接口
        //=========================================================================

        /// <summary>
        /// 设置加载模式（预加载 / 热更），并按模式初始化界面元素显隐
        /// </summary>
        /// <param name="loadingMode">加载模式</param>
        public void SetLoadingMode(LoadingMode loadingMode)
        {
            m_LoadingMode = loadingMode;

            switch (m_LoadingMode)
            {
                case LoadingMode.Preload:
                    // 预加载模式：仅展示进度条与描述，隐藏热更相关控件
                    SetButtonsVisible(false, false, false);
                    SetViewsActive(true,
                        m_ProgressSlider?.gameObject, m_ProgressText?.gameObject, m_ProgressTextTMP?.gameObject,
                        m_DescText?.gameObject, m_DescTextTMP?.gameObject);
                    SetViewsActive(false,
                        m_BytesNumText?.gameObject, m_BytesNumTextTMP?.gameObject,
                        m_FileNumText?.gameObject, m_FileNumTextTMP?.gameObject);
                    RefreshViews(string.Empty, 0f);
                    break;

                case LoadingMode.HotfixLauncher:
                case LoadingMode.HotfixIncreaser:
                    // 热更模式：展示进度条、描述、字节/文件数与操作按钮
                    SetButtonsVisible(true, true, true);
                    SetViewsActive(true,
                        m_ProgressSlider?.gameObject, m_ProgressText?.gameObject, m_ProgressTextTMP?.gameObject,
                        m_DescText?.gameObject, m_DescTextTMP?.gameObject,
                        m_BytesNumText?.gameObject, m_BytesNumTextTMP?.gameObject,
                        m_FileNumText?.gameObject, m_FileNumTextTMP?.gameObject);
                    RefreshViews(string.Empty, 0f, GameDefinitions.DownloadStep.Idle, 0, 0, 0, 0);
                    break;
            }
        }

        /// <summary>
        /// 统一刷新界面显示（预加载与热更共用入口）
        /// </summary>
        /// <param name="descContent">描述内容</param>
        /// <param name="progress">进度值 0~1</param>
        /// <param name="downloadStep">当前下载步骤（热更模式）</param>
        /// <param name="curFileNum">已下载文件数</param>
        /// <param name="totalFileNum">待下载文件总数</param>
        /// <param name="curBytes">已下载字节数</param>
        /// <param name="totalBytes">待下载总字节数</param>
        public void RefreshViews(string descContent, float progress,
            GameDefinitions.DownloadStep downloadStep = GameDefinitions.DownloadStep.None, int curFileNum = 0,
            int totalFileNum = 0, float curBytes = 0f, float totalBytes = 0f)
        {
            if (m_LoadingMode == LoadingMode.HotfixLauncher || m_LoadingMode == LoadingMode.HotfixIncreaser)
            {
                RefreshHotfixModeViews(descContent, progress, downloadStep, curFileNum, totalFileNum, curBytes,
                    totalBytes);
            }
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
        /// 显示时播放进入淡入动画；隐藏时播放退出淡出动画（未启用过渡则立即显示/隐藏）
        /// </summary>
        /// <param name="visible">true=显示，false=隐藏</param>
        public void SetVisible(bool visible)
        {
            if (visible)
            {
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true); // 触发 OnEnable → 播放进入过渡
                else
                    PlayEnterTransition();      // 已激活则直接播放进入过渡
            }
            else
            {
                PlayExitTransition();
            }
        }
        #endregion

        #region UI按钮点击事件
        //=========================================================================
        // UI按钮点击事件（热更模式按钮，由 Unity 事件或代码注册调用；当前 Preload 不显示）
        //=========================================================================

        /// <summary>
        /// 开始按钮点击
        /// </summary>
        public void OnStartButtonClicked()
        {
            // 热更完成进入游戏入口（预留）
        }

        /// <summary>
        /// 重试按钮点击
        /// </summary>
        public void OnRetryButtonClicked()
        {
            // 下载失败重试入口（预留）
        }

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        public void OnCloseButtonClicked()
        {
            // 关闭/退出入口（预留）
        }
        #endregion

        #region 私有刷新/设置方法
        //=========================================================================
        // 私有刷新/设置方法
        //=========================================================================

        /// <summary>
        /// 热更模式下刷新界面：按下载步骤切换描述文本与按钮显隐
        /// </summary>
        /// <param name="descContent">描述内容</param>
        /// <param name="progress">进度值0~1</param>
        /// <param name="downloadStep">当前下载步骤</param>
        /// <param name="curFileNum">已下载文件数</param>
        /// <param name="totalFileNum">待下载文件总数</param>
        /// <param name="curBytes">已下载字节数</param>
        /// <param name="totalBytes">待下载总字节数</param>
        private void RefreshHotfixModeViews(string descContent, float progress,
            GameDefinitions.DownloadStep downloadStep, int curFileNum, int totalFileNum, float curBytes,
            float totalBytes)
        {
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

            switch (downloadStep)
            {
                case GameDefinitions.DownloadStep.Idle:
                    SetProgressDescContent(textReadyDownload);
                    SetButtonsVisible(true, false, true);
                    break;

                case GameDefinitions.DownloadStep.Processing:
                    if (string.IsNullOrEmpty(descContent))
                        SetProgressDescContent(descContent);
                    else
                        SetProgressDescContent(AorTxt.Format("{0} [{1:N2}M/{2:N2}M] {3:N0}%", textDownloading,
                            curBytes / 1024f / 1024f, totalBytes / 1024f / 1024f, progress * 100));
                    SetButtonsVisible(false, false, false);
                    break;

                case GameDefinitions.DownloadStep.AllOver:
                    SetProgressDescContent(textDownloadAllOver);
                    SetButtonsVisible(false, false, false);
                    break;

                case GameDefinitions.DownloadStep.Skip:
                    SetProgressDescContent(textDownloadFailedToSkip);
                    SetButtonsVisible(false, false, false);
                    break;

                case GameDefinitions.DownloadStep.Error:
                    SetProgressDescContent(textDownloadError);
                    SetButtonsVisible(false, true, false);
                    break;
            }
        }

        /// <summary>
        /// 进度条平滑动画ID（避免与其他过渡动画冲突）
        /// </summary>
        private const string ProgressTweenId = "__UILauncherLoadingView_ProgressTween__";

        /// <summary>
        /// 进度条平滑动画时长（秒）：把底层的跳变进度转为平滑滑行，使加载过程更丝滑
        /// </summary>
        private const float ProgressSmoothDuration = 0.35f;

        /// <summary>
        /// 设置进度条与百分比（带平滑动画，避免直接跳变）
        /// 说明：底层进度事件可能大步跳变（重活集中在结尾），直接赋值会让进度条"走一次就跳满"；
        ///       这里改为向目标进度平滑滑动，并在动画过程中同步百分比文字，保证滑块与文字一致。
        /// </summary>
        /// <param name="progress">目标进度值 0~1</param>
        private void SetProgress(float progress)
        {
            m_ProgressValue = progress;

            if (m_ProgressSlider != null)
            {
                // 平滑过渡到目标进度；连续事件到达时取消上一次动画，向最新目标滑动
                DOTween.Kill(ProgressTweenId);
                m_ProgressSlider.DOValue(progress, ProgressSmoothDuration)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true) // 不受游戏暂停影响
                    .SetId(ProgressTweenId)
                    .OnUpdate(() => RefreshProgressText(m_ProgressSlider.value));
            }
            else
            {
                RefreshProgressText(progress);
            }
        }

        /// <summary>
        /// 刷新进度百分比文字（与滑块当前值同步）
        /// </summary>
        /// <param name="progress">进度值 0~1</param>
        private void RefreshProgressText(float progress)
        {
            SetText(m_ProgressText, m_ProgressTextTMP, AorTxt.Format("{0:N0}%", progress * 100));
        }

        /// <summary>
        /// 设置进度描述文本
        /// </summary>
        /// <param name="descContent">描述内容</param>
        private void SetProgressDescContent(string descContent)
        {
            SetText(m_ProgressText, m_ProgressTextTMP, descContent);
        }

        /// <summary>
        /// 设置文件数量显示
        /// </summary>
        /// <param name="curNum">已下载文件数</param>
        /// <param name="totalNum">待下载文件总数</param>
        private void SetFileNums(int curNum, int totalNum)
        {
            m_CurFileNum = curNum;
            m_TotalFileNum = totalNum;
            SetText(m_FileNumText, m_FileNumTextTMP, AorTxt.Format("({0} / {1})", m_CurFileNum, m_TotalFileNum));
        }

        /// <summary>
        /// 设置字节数显示（MB）
        /// </summary>
        /// <param name="curNum">已下载字节数</param>
        /// <param name="totalNum">待下载总字节数</param>
        private void SetBytesNums(float curNum, float totalNum)
        {
            m_CurBytes = curNum;
            m_TotalBytes = totalNum;
            SetText(m_BytesNumText, m_BytesNumTextTMP,
                AorTxt.Format("({0:N2} / {1:N2}MB) ", m_CurBytes / 1024f / 1024f, m_TotalBytes / 1024f / 1024f));
        }

        /// <summary>
        /// 设置通用描述文本
        /// </summary>
        /// <param name="descContent">描述内容</param>
        private void SetDescContent(string descContent)
        {
            m_DescContent = descContent;
            SetText(m_DescText, m_DescTextTMP, m_DescContent);
        }

        /// <summary>
        /// 设置三个按钮显隐
        /// </summary>
        /// <param name="startButtonVisible">开始按钮显隐</param>
        /// <param name="retryButtonVisible">重试按钮显隐</param>
        /// <param name="closeButtonVisible">关闭按钮显隐</param>
        private void SetButtonsVisible(bool startButtonVisible, bool retryButtonVisible, bool closeButtonVisible)
        {
            if (m_StartButton != null) m_StartButton.gameObject.SetActive(startButtonVisible);
            if (m_RetryButton != null) m_RetryButton.gameObject.SetActive(retryButtonVisible);
            if (m_CloseButton != null) m_CloseButton.gameObject.SetActive(closeButtonVisible);
        }

        /// <summary>
        /// 批量设置控件显隐（判空安全，用于消除重复的空引用判断）
        /// </summary>
        /// <param name="active">显隐状态</param>
        /// <param name="views">目标控件 GameObject（可为空元素）</param>
        private static void SetViewsActive(bool active, params GameObject[] views)
        {
            if (views == null) return;
            foreach (GameObject view in views)
                if (view != null) view.SetActive(active);
        }

        /// <summary>
        /// 同时设置 UGUI 与 TMP 两套文本（按实际接线取其一）
        /// </summary>
        /// <param name="uguiText">UGUI 文本（可为空）</param>
        /// <param name="tmpText">TMP 文本（可为空）</param>
        /// <param name="content">文本内容</param>
        private static void SetText(Text uguiText, TextMeshProUGUI tmpText, string content)
        {
            if (uguiText != null) uguiText.text = content;
            if (tmpText != null) tmpText.text = content;
        }

        /// <summary>
        /// 是否 WebGL 平台（用于文本差异化；当前未启用，保留平台判断入口）
        /// </summary>
        private bool IsWebGL()
        {
            return Application.platform == RuntimePlatform.WebGLPlayer;
        }
        #endregion

        #region 切换过渡动画
        //=========================================================================
        // 切换过渡动画（进入淡入 / 退出淡出，带缓动曲线，使启动衔接更丝滑）
        //=========================================================================

        /// <summary>
        /// 播放进入过渡：整界面淡入（0 → 1）
        /// 未启用进入过渡、或预制体未预挂 CanvasGroup 时，立即置为完全不透明（不播放动画）
        /// </summary>
        private void PlayEnterTransition()
        {
            DOTween.Kill(GameDOTweenTypes.LauncherLoadingViewTween);

            CanvasGroup cg = GetCanvasGroup();
            if (!m_EnterTransitionEnabled || cg == null || !gameObject.activeSelf)
            {
                if (cg != null) cg.alpha = 1f;
                return;
            }

            cg.alpha = 0f;
            cg.DOFade(1f, Mathf.Max(0.01f, m_EnterTransitionDuration))
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true) // 不受游戏暂停影响
                .SetId(GameDOTweenTypes.LauncherLoadingViewTween);
        }

        /// <summary>
        /// 播放退出过渡：整界面淡出（1 → 0），结束后隐藏物体
        /// 未启用退出过渡、或预制体未预挂 CanvasGroup 时，立即隐藏（不播放动画）
        /// </summary>
        private void PlayExitTransition()
        {
            CanvasGroup cg = GetCanvasGroup();
            if (!m_ExitTransitionEnabled || cg == null || !gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                return;
            }

            DOTween.Kill(GameDOTweenTypes.LauncherLoadingViewTween);

            cg.DOFade(0f, Mathf.Max(0.01f, m_ExitTransitionDuration))
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true) // 不受游戏暂停影响
                .SetId(GameDOTweenTypes.LauncherLoadingViewTween)
                .OnComplete(() => gameObject.SetActive(false));
        }

        /// <summary>
        /// 获取预制体上预挂的 CanvasGroup（惰性缓存读取，不做运行时创建）
        /// </summary>
        private CanvasGroup GetCanvasGroup()
        {
            if (m_CanvasGroup == null)
                m_CanvasGroup = GetComponent<CanvasGroup>();
            return m_CanvasGroup;
        }
        #endregion

        #region 事件回调
        //=========================================================================
        // 事件回调
        //=========================================================================

        /// <summary>
        /// 事件：加载进度回调（预加载 / 通用进度）
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
        /// 事件：热更出错（预留：可按 fileName 处理失败提示）
        /// </summary>
        private void OnHotfixErrorEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            string fileName = e.GetString("fileName");
            Log.Error("热更下载出错，文件：{0}", fileName);
        }
        #endregion
    }
}
