using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 时间戳工具类
    /// 提供 UTC0 时间戳、本地时间戳、秒数转 DateTime 的常用封装
    /// 注意：本地时间仅作临时过渡，正式环境务必使用服务器时间戳
    /// </summary>
    public static class Timer
    {
        /// <summary>
        /// 获取当前 UTC0 时间戳（1970-01-01 至今的秒数）
        /// 临时方案：后续需统一改为服务器下发时间戳
        /// </summary>
        /// <returns>UTC0 时间戳（秒）</returns>
        public static int GetUTC0TimeStamp()
        {
            return (int)Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        /// <summary>
        /// 获取本地时区时间戳（基于 UTC0 换算）
        /// </summary>
        /// <param name="utc0Seconds">服务器 UTC0 时间戳（秒），默认0=用本地系统时间（易作弊，仅开发/离线用）</param>
        /// <returns>本地时区时间戳（秒）</returns>
        public static int GetLocalTimeStamp(int utc0Seconds = 0)
        {
            DateTime dateTime = utc0Seconds == 0 
                ? DateTime.Now 
                : new DateTime(1970, 1, 1).AddSeconds(utc0Seconds).ToLocalTime();

            return (int)Math.Floor((dateTime - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        /// <summary>
        /// 将秒级时间戳转为 DateTime（UTC0）
        /// </summary>
        /// <param name="seconds">1970-01-01 至今的秒数</param>
        /// <returns>UTC0 对应的 DateTime，seconds≤0 返回默认值</returns>
        public static DateTime GetDateTime(int seconds)
        {
            if (seconds <= 0)
                return default;

            return new DateTime(1970, 1, 1).AddSeconds(seconds);
        }
    }
}