/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameConstants.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏框架全局常量配置中心
 *            统一管理版本号、UI层级、持久化存储键值等全局静态常量
 ***************************************************************/

namespace Honor.Runtime
{
    #region 游戏框架全局常量定义
    /// <summary>
    /// 游戏框架全局常量定义
    /// </summary>
    /// <remarks>
    /// 统一管理版本、UI层级、持久化键值等全局不变的配置
    /// 静态只读常量，禁止写入，仅用于全局读取
    /// </remarks>
    public static partial class GameConstants
    {
        //=========================================================================
        // 版本常量
        //=========================================================================
        /// <summary>
        /// 游戏支持的最低版本号
        /// </summary>
        public const string MinGameVersion = "0.0.1";

        /// <summary>
        /// Honor 游戏框架版本号
        /// </summary>
        public const string HonorVersion = "1.1.0";

        //=========================================================================
        // UI 层级常量
        //=========================================================================
        /// <summary>
        /// 全局系统级 UI 层级（ZOrder）
        /// </summary>
        /// <remarks>层级数值越大，UI显示越靠前</remarks>
        public const int LaunchUIZOrder = 32755;                 // 启动UI
        public const int SplashUIZOrder = 32756;                // 闪屏UI
        public const int HotfixUIZOrder = 32757;                // 热更新UI
        public const int HotfixErrorUIZOrder = 32758;           // 热更新错误提示UI
        public const int WebGLUIZOrder = 32759;                 // WebGL专用UI
        public const int PreloadUIZOrder = 32760;               // 预加载UI
        public const int AppDownloadUIZOrder = 32761;           // 大版本更新提示UI
        public const int WaitingUIZOrder = 32762;               // 加载等待菊花UI
        public const int ProcedureTransitionUIZOrder = 32763;   // 流程切换过渡UI
        public const int GDPRUIZOrder = 32764;                   // GDPR隐私授权UI
        public const int AppReviewUIZOrder = 32765;             // 应用内评价UI
        public const int AppFeedbackUIZOrder = 32766;           // 应用内反馈UI
        public const int FloatWordsUIZOrder = 32767;             // 顶部飘字UI（最高优先级）

        //=========================================================================
        // 持久化常量
        //=========================================================================
        /// <summary>
        /// 持久化存储相关常量
        /// </summary>
        /// <remarks>统一管理所有存储方式、分类名称、数据Key</remarks>
        public static class Persist
        {
            /// <summary>
            /// 通用持久化配置
            /// </summary>
            /// <remarks>存储版本、语言、设备信息等基础公共数据</remarks>
            public static class Common
            {
                /// <summary>
                /// 持久化存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;
                
                /// <summary>
                /// 存储分类名称
                /// </summary>
                public const string ClassifyName = "CommonInfos";

                /// <summary>
                /// 通用数据存储键值
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 游戏版本号存储键
                    /// </summary>
                    public const string Version = "Version";
                    
                    /// <summary>
                    /// 语言设置存储键
                    /// </summary>
                    public const string Language = "Language";
                }
            }

            /// <summary>
            /// IAP 内购持久化配置
            /// </summary>
            /// <remarks>存储订单、订阅、支付记录、用户分层等数据</remarks>
            public static class IAP
            {
                /// <summary>
                /// 持久化存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;
                
                /// <summary>
                /// 存储分类名称
                /// </summary>
                public const string ClassifyName = "IAPInfos";

                /// <summary>
                /// IAP数据存储键值
                /// </summary>
                public static class ItemKey
                {
                    public const string OrderingStates = "OrderingStates";
                    public const string NonConsumeDotDatas = "NonConsumeDotDatas";
                    public const string SubscriptionDotDatas = "SubscriptionDotDatas";
                    public const string SubscriptionOrderTableIDs = "SubscriptionOrderTableIDs";
                    public const string SubscriptionInBuyID = "SubscriptionInBuyID";
                    public const string FinishedOrderIDs = "FinishedOrderIDs";
                    public const string FinishedFailedOrderIDs = "FinishedFailedOrderIDs";
                    public const string FinishedOrderFirstTime = "FinishedOrderFirstTime";
                    public const string FinishedOrderLatestTime = "FinishedOrderLatestTime";
                    public const string FinishedOrderLatestTimeInterval = "FinishedOrderLatestTimeInterval";
                    public const string FinishedOrderTotalCount = "FinishedOrderTotalCount";
                    public const string FinishedOrderTotalMoney = "FinishedOrderTotalMoney";
                    public const string FinishedUserLayerInfo = "FinishedUserLayerInfo";
                    public const string ThirdOrderingStates = "ThirdOrderingStates";
                    public const string ThirdFinishOrderIDs = "ThirdFinishOrderIDs";
                    public const string ThirdFailedOrderIDs = "ThirdErrorOrderIDs";
                    public const string AmazonOrderingStates = "AmazonOrderingStates";
                    public const string AmazonFinishOrderIDs = "AmazonFinishOrderIDs";
                    public const string AmazonFailedOrderIDs = "AmazonFailedOrderIDs";
                }
            }

            /// <summary>
            /// GDPR 隐私合规持久化配置
            /// </summary>
            public static class GDPR
            {
                public const PersistWayType WayType = PersistWayType.FileFragment;
                public const string ClassifyName = "GDPRInfos";

                public static class ItemKey
                {
                    public const string GDPROver = "GDPROver";
                    public const string GDPRNeedFlagFromNet = "GDPRNeedFlagFromNet";
                    public const string HasUserConsent = "HasUserConsent";
                    public const string IsSell = "IsSell";
                    public const string IsAgeReachStandard = "IsAgeReachStandard";
                }
            }

            /// <summary>
            /// MAX 广告相关持久化配置
            /// </summary>
            public static class MAX
            {
                public const PersistWayType WayType = PersistWayType.FileFragment;
                public const string ClassifyName = "MAXInfos";

                public static class ItemKey
                {
                    public const string BannerILRDCount = "BannerILRDCount";
                    public const string BannerILRDRevenue = "BannerILRDRevenue";
                }
            }

            /// <summary>
            /// 权限申请相关持久化配置
            /// </summary>
            public static class Permission
            {
                public const PersistWayType WayType = PersistWayType.FileFragment;
                public const string ClassifyName = "PermissionInfos";

                public static class ItemKey
                {
                    public const string IsAuthorized = "IsAuthorized";
                }
            }

            /// <summary>
            /// AF（AppsFlyer）归因相关持久化配置
            /// </summary>
            public static class AF
            {
                public const PersistWayType WayType = PersistWayType.FileFragment;
                public const string ClassifyName = "AFInfos";

                public static class ItemKey
                {
                    public const string IsServerConversionDatas = "IsServerConversionDatas";
                    public const string ServerConversionDatas = "ServerConversionDatas";
                }
            }
        }
    }
    #endregion
}