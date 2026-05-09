namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架全局常量定义
    /// 统一管理版本、UI层级、持久化键值等全局不变的配置
    /// 禁止写入，仅用于读取常量
    /// </summary>
    public static partial class GameConstants
    {
        /// <summary>
        /// 游戏支持的最低版本号
        /// </summary>
        public const string MinGameVersion = "0.0.1";

        /// <summary>
        /// Honor 游戏框架版本号
        /// </summary>
        public const string HonorVersion = "1.1.0";

        /// <summary>
        /// 全局系统级 UI 层级（ZOrder）定义
        /// 层级数值越大，显示越靠前
        /// </summary>
        public const int LaunchUIZOrder = 32755; // 启动UI

        public const int SplashUIZOrder = 32756; // 闪屏UI
        public const int HotfixUIZOrder = 32757; // 热更新UI
        public const int HotfixErrorUIZOrder = 32758; // 热更新错误提示UI
        public const int WebGLUIZOrder = 32759; // WebGL专用UI
        public const int PreloadUIZOrder = 32760; // 预加载UI
        public const int AppDownloadUIZOrder = 32761; // 大版本更新提示UI
        public const int WaitingUIZOrder = 32762; // 加载等待菊花UI
        public const int ProcedureTransitionUIZOrder = 32763; // 流程切换过渡UI
        public const int GDPRUIZOrder = 32764; // GDPR隐私授权UI
        public const int AppReviewUIZOrder = 32765; // 应用内评价UI
        public const int AppFeedbackUIZOrder = 32766; // 应用内反馈UI
        public const int FloatWordsUIZOrder = 32767; // 顶部飘字UI（最高优先级）

        /// <summary>
        /// 持久化（存储）相关常量
        /// 统一管理所有存储路径、分类名、Key值
        /// </summary>
        public static class Persist
        {
            /// <summary>
            /// 通用持久化配置
            /// 存储：版本、语言、设备信息等基础数据
            /// </summary>
            public static class Common
            {
                public const PersistWayType WayType = PersistWayType.FileFragment;
                public const string ClassifyName = "CommonInfos";

                public static class ItemKey
                {
                    public const string Version = "Version"; // 游戏版本
                    public const string Language = "Language"; // 语言设置
                }
            }

            /// <summary>
            /// IAP 内购持久化配置
            /// 存储：订单、订阅、支付记录、分层信息等
            /// </summary>
            public static class IAP
            {
                public const PersistWayType WayType = PersistWayType.FileFragment;
                public const string ClassifyName = "IAPInfos";

                public static class ItemKey
                {
                    public const string OrderingStates = "OrderingStates"; // 进行中订单
                    public const string NonConsumeDotDatas = "NonConsumeDotDatas"; // 非消耗品数据
                    public const string SubscriptionDotDatas = "SubscriptionDotDatas"; // 订阅数据
                    public const string SubscriptionOrderTableIDs = "SubscriptionOrderTableIDs"; // 订阅订单映射
                    public const string SubscriptionInBuyID = "SubscriptionInBuyID"; // 购买中的订阅
                    public const string FinishedOrderIDs = "FinishedOrderIDs"; // 已完成订单
                    public const string FinishedFailedOrderIDs = "FinishedFailedOrderIDs"; // 已失败订单
                    public const string FinishedOrderFirstTime = "FinishedOrderFirstTime"; // 首次支付时间
                    public const string FinishedOrderLatestTime = "FinishedOrderLatestTime"; // 最近支付时间
                    public const string FinishedOrderLatestTimeInterval = "FinishedOrderLatestTimeInterval"; // 最近支付间隔
                    public const string FinishedOrderTotalCount = "FinishedOrderTotalCount"; // 总支付次数
                    public const string FinishedOrderTotalMoney = "FinishedOrderTotalMoney"; // 总支付金额（USD）
                    public const string FinishedUserLayerInfo = "FinishedUserLayerInfo"; // 用户分层信息
                    public const string ThirdOrderingStates = "ThirdOrderingStates"; // 第三方进行中订单
                    public const string ThirdFinishOrderIDs = "ThirdFinishOrderIDs"; // 第三方完成订单
                    public const string ThirdFailedOrderIDs = "ThirdErrorOrderIDs"; // 第三方失败订单
                    public const string AmazonOrderingStates = "AmazonOrderingStates"; // Amazon进行中订单
                    public const string AmazonFinishOrderIDs = "AmazonFinishOrderIDs"; // Amazon完成订单
                    public const string AmazonFailedOrderIDs = "AmazonFailedOrderIDs"; // Amazon失败订单
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
                    public const string GDPROver = "GDPROver"; // GDPR流程是否完成
                    public const string GDPRNeedFlagFromNet = "GDPRNeedFlagFromNet"; // 云端是否需要GDPR
                    public const string HasUserConsent = "HasUserConsent"; // 用户授权状态
                    public const string IsSell = "IsSell"; // CCPA 隐私选项
                    public const string IsAgeReachStandard = "IsAgeReachStandard"; // COPPA 年龄认证
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
                    public const string BannerILRDCount = "BannerILRDCount"; // Banner广告展示次数
                    public const string BannerILRDRevenue = "BannerILRDRevenue"; // Banner广告收益
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
                    public const string IsAuthorized = "IsAuthorized"; // 用户是否已授权
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
                    public const string IsServerConversionDatas = "IsServerConversionDatas"; // 是否获取服务端归因
                    public const string ServerConversionDatas = "ServerConversionDatas"; // 服务端归因数据
                }
            }
        }
    }
}