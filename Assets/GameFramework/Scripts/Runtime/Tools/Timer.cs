using System;

namespace Honor.Runtime
{
    public static class Timer
    {
        /// <summary>
        /// 获取当前UTC0时间戳
        /// 以秒为单位（临时的，以后使用服务器时间）
        /// </summary>
        /// <returns>返回秒数</returns>
        public static int GetUTC0TimeStamp()
        {
            return (int)Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        /// <summary>
        /// 获取本地时区的时间戳
        /// 以秒为单位（根据UTC0时间计算本地时区时间）
        /// </summary>
        /// <param name="utc0Seconds">UTC0时间(一般为服务器的时间戳)，不传时表示通过本地系统时间推算（本地系统时间无法避免作弊）</param>
        /// <returns></returns>
        public static int GetLocalTimeStamp(int utc0Seconds = 0)
        {
            DateTime dateTime = utc0Seconds == 0 ? DateTime.Now : new DateTime(1970, 1, 1).AddSeconds(utc0Seconds).ToLocalTime();
            return (int)Math.Floor((dateTime - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        /// <summary>
        /// 获取DateTime日期类型数据
        /// </summary>
        /// <param name="seconds">总秒数</param>
        /// <returns></returns>
        public static DateTime GetDateTime(int seconds)
        {
            DateTime dateTime = default(DateTime);
            if (seconds > 0)
            {
                dateTime = new DateTime(1970, 1, 1);
                dateTime = dateTime.AddSeconds(seconds);
            }
            return dateTime;
        }
    }
}


