
namespace Honor.Runtime
{
    public static partial class GameConstants
    {
        /// <summary>
        /// 最小游戏版本号
        /// </summary>
        public const string MinGameVersion = "0.0.1";

        /// <summary>
        /// Honor框架版本号
        /// </summary>
        public const string HonorVersion = "1.1.0";

        /// <summary>
        /// 启动UI默认层级ZOrder
        /// </summary>
        public const int LaunchUIZOrder = 32755;

        /// <summary>
        /// 闪屏UI默认层级ZOrder
        /// </summary>
        public const int SplashUIZOrder = 32756;

        /// <summary>
        /// 热更新UI默认层级ZOrder
        /// </summary>
        public const int HotfixUIZOrder = 32757;

        /// <summary>
        /// 热更新错误提示UI默认层级ZOrder
        /// </summary>
        public const int HotfixErrorUIZOrder = 32758;

        /// <summary>
        /// WebGL-UI默认层级ZOrder
        /// </summary>
        public const int WebGLUIZOrder = 32759;

        /// <summary>
        /// 预加载UI默认层级ZOrder
        /// </summary>
        public const int PreloadUIZOrder = 32760;

        /// <summary>
        /// 大版本更新提示UI默认层级ZOrder
        /// </summary>
        public const int AppDownloadUIZOrder = 32761;

        /// <summary>
        /// 菊花等待UI默认层级ZOrder
        /// </summary>
        public const int WaitingUIZOrder = 32762;

        /// <summary>
        /// 流程切换过渡UI默认层级ZOrder
        /// </summary>
        public const int ProcedureTransitionUIZOrder = 32763;

        /// <summary>
        /// GDPRUI默认层级ZOrder
        /// </summary>
        public const int GDPRUIZOrder = 32764;

        /// <summary>
        /// 应用内评价UI默认层级ZOrder
        /// </summary>
        public const int AppReviewUIZOrder = 32765;

        /// <summary>
        /// 应用内反馈UI默认层级ZOrder
        /// </summary>
        public const int AppFeedbackUIZOrder = 32766;

        /// <summary>
        /// 飘字UI默认层级ZOrder
        /// </summary>
        public const int FloatWordsUIZOrder = 32767;

        /// <summary>
        /// 持久化常量
        /// </summary>
        public static class Persist
        {
            /// <summary>
            /// 持久化-通用常量
            /// </summary>
            public static class Common
            {
                /// <summary>
                /// 持久化-通用常量-存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;

                /// <summary>
                /// 持久化-通用常量-类别名称
                /// </summary>
                public const string ClassifyName = "CommonInfos";

                /// <summary>
                /// 持久化-通用常量-条目键
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 持久化-通用常量-条目键-版本
                    /// </summary>
                    public const string Version = "Version";

                    /// <summary>
                    /// 持久化-通用常量-条目键-语言
                    /// </summary>
                    public const string Language = "Language";
                }

            }

            /// <summary>
            /// 持久化-IAP常量
            /// </summary>
            public static class IAP
            {
                /// <summary>
                /// 持久化-IAP常量-存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;

                /// <summary>
                /// 持久化-IAP常量-类别名称
                /// </summary>
                public const string ClassifyName = "IAPInfos";

                /// <summary>
                /// 持久化-IAP常量-条目键
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 持久化-IAP常量-条目键-正在进行中的订单状态集合
                    /// </summary>
                    public const string OrderingStates = "OrderingStates";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-
                    /// </summary>
                    public const string NonConsumeDotDatas = "NonConsumeDotDatas";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-正在进行中的订单状态集合
                    /// </summary>
                    public const string SubscriptionDotDatas = "SubscriptionDotDatas";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-订阅订单ID和TableID的对应关系
                    /// </summary>
                    public const string SubscriptionOrderTableIDs = "SubscriptionOrderTableIDs";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-正在购买中的订阅TableID
                    /// </summary>
                    public const string SubscriptionInBuyID = "SubscriptionInBuyID";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-已经完成的订单ID集合
                    /// </summary>
                    public const string FinishedOrderIDs = "FinishedOrderIDs";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-已经完成的失败订单ID集合
                    /// </summary>
                    public const string FinishedFailedOrderIDs = "FinishedFailedOrderIDs";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-首次订单完成时间
                    /// </summary>
                    public const string FinishedOrderFirstTime = "FinishedOrderFirstTime";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-最近订单完成时间
                    /// </summary>
                    public const string FinishedOrderLatestTime = "FinishedOrderLatestTime";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-最近支付距离上一次支付时间间隔（秒数）
                    /// </summary>
                    public const string FinishedOrderLatestTimeInterval = "FinishedOrderLatestTimeInterval";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-累积完成订单次数
                    /// </summary>
                    public const string FinishedOrderTotalCount = "FinishedOrderTotalCount";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-累积完成订单金额（统一按照美元）
                    /// </summary>
                    public const string FinishedOrderTotalMoney = "FinishedOrderTotalMoney";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-用户分层统计订单时间,新增
                    /// </summary>
                    public const string FinishedUserLayerInfo = "FinishedUserLayerInfo";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-正在进行中的第三方订单状态集合,新增
                    /// </summary>
                    public const string ThirdOrderingStates = "ThirdOrderingStates";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-完成的第三方订单ID集合,新增
                    /// </summary>
                    public const string ThirdFinishOrderIDs = "ThirdFinishOrderIDs";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-失败的第三方订单ID集合,新增
                    /// </summary>
                    public const string ThirdFailedOrderIDs = "ThirdErrorOrderIDs";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-正在进行中的Amazon订单状态集合,新增
                    /// </summary>
                    public const string AmazonOrderingStates = "AmazonOrderingStates";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-完成的Amazon订单ID集合,新增
                    /// </summary>
                    public const string AmazonFinishOrderIDs = "AmazonFinishOrderIDs";

                    /// <summary>
                    /// 持久化-IAP常量-条目键-失败的Amazon集合,新增
                    /// </summary>
                    public const string AmazonFailedOrderIDs = "AmazonFailedOrderIDs";

                }

            }

            /// <summary>
            /// 持久化-GDPR常量
            /// </summary>
            public static class GDPR
            {
                /// <summary>
                /// 持久化-GDPR常量-存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;

                /// <summary>
                /// 持久化-GDPR常量-类别名称
                /// </summary>
                public const string ClassifyName = "GDPRInfos";

                /// <summary>
                /// 持久化-GDPR常量-条目键
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 持久化-GDPR常量-条目键-GDPR是否已结束
                    /// </summary>
                    public const string GDPROver = "GDPROver";

                    /// <summary>
                    /// 持久化-GDPR常量-条目键-是否需要遵循GDPR的云端标记
                    /// </summary>
                    public const string GDPRNeedFlagFromNet = "GDPRNeedFlagFromNet";

                    /// <summary>
                    /// 持久化-GDPR常量-条目键-GDPR协议设置项
                    /// </summary>
                    public const string HasUserConsent = "HasUserConsent";

                    /// <summary>
                    /// 持久化-GDPR常量-条目键-CCPA协议设置项
                    /// </summary>
                    public const string IsSell = "IsSell";

                    /// <summary>
                    /// 持久化-GDPR常量-条目键-COPPA协议设置项
                    /// </summary>
                    public const string IsAgeReachStandard = "IsAgeReachStandard";

                }

            }

            /// <summary>
            /// 持久化-MAX常量
            /// </summary>
            public static class MAX
            {
                /// <summary>
                /// 持久化-MAX常量-存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;

                /// <summary>
                /// 持久化-MAX常量-类别名称
                /// </summary>
                public const string ClassifyName = "MAXInfos";

                /// <summary>
                /// 持久化-MAX常量-条目键
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 持久化-MAX常量-条目键-Banner同步间隔
                    /// </summary>
                    public const string BannerILRDCount = "BannerILRDCount";
                    
                    /// <summary>
                    /// 持久化-MAX常量-条目键-Banner的累计付费
                    /// </summary>
                    public const string BannerILRDRevenue = "BannerILRDRevenue";
                }

            }

            /// <summary>
            /// 持久化-Permission常量
            /// </summary>
            public static class Permission
            {
                /// <summary>
                /// 持久化-Permission常量-存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;

                /// <summary>
                /// 持久化-Permission常量-类别名称
                /// </summary>
                public const string ClassifyName = "PermissionInfos";

                /// <summary>
                /// 持久化-Permission常量-条目键
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 持久化-授予常量-条目键-用户授予是否已结束
                    /// </summary>
                    public const string IsAuthorized = "IsAuthorized";
                }
            }
            
            /// <summary>
            /// 持久化-Permission常量
            /// </summary>
            public static class AF
            {
                /// <summary>
                /// 持久化-AF常量-存储方式
                /// </summary>
                public const PersistWayType WayType = PersistWayType.FileFragment;

                /// <summary>
                /// 持久化-AF常量-类别名称
                /// </summary>
                public const string ClassifyName = "AFInfos";

                /// <summary>
                /// 持久化-AF常量-条目键
                /// </summary>
                public static class ItemKey
                {
                    /// <summary>
                    /// 持久化-授予常量-条目键-是否从服务器获取精准归因数据
                    /// </summary>
                    public const string IsServerConversionDatas = "IsServerConversionDatas";
                    
                    /// <summary>
                    /// 持久化-授予常量-条目键-从服务器获取精准归因数据
                    /// </summary>
                    public const string ServerConversionDatas = "ServerConversionDatas";
                }
            }
        }

    }
}


