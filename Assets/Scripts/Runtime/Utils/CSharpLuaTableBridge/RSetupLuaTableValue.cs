using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace GameLib
{
    public static class RSetupLuaTableValue
    {
        public static void RScreenToWorldPoint(Camera camera,Vector3 screenPosition,LuaTable worldPosition)
        {
            var screenPoint = new Vector3(screenPosition.x,screenPosition.y,screenPosition.z);
            var result = camera.ScreenToWorldPoint(screenPoint);
            worldPosition.Set("x",result.x);
            worldPosition.Set("y",result.y);
            worldPosition.Set("z",result.z);
        }

        public static float GetTimeDeltaTime()
        {
            return Time.deltaTime;
        }

        public static float GetRealtimeSinceStartup()
        {
            return Time.realtimeSinceStartup;
        }

        #region 是否点击到了UI
        private static List<RaycastResult> _isPointerOverUIObjectResult;
        public static bool IsPointerOverUIObject() {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _isPointerOverUIObjectResult ??= new List<RaycastResult>();
            _isPointerOverUIObjectResult.Clear();
            EventSystem.current.RaycastAll(eventData, _isPointerOverUIObjectResult);
            return _isPointerOverUIObjectResult.Count > 0 && _isPointerOverUIObjectResult.Exists(ui=>ui.gameObject && ui.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
        #endregion

        #region 返回DateTime相关的对象
        public class RDateTimeWarpYMDHMS
        {
            public int Year;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;
        }

        /// <summary>
        /// 一个公用的容器，Lua端获取之后要立刻建一个table把数据拷贝出去
        /// </summary>
        private static readonly RDateTimeWarpYMDHMS _dateTimeCommonData = new();
        
        /// <summary>
        /// 获取一个年月日时分秒的结构
        /// 从19791100添加秒数
        /// 根据本地时间戳获取本地年月日时分秒信息
        /// </summary>
        /// <returns></returns>
        public static RDateTimeWarpYMDHMS GetDateTime1970AddSeconds(int seconds)
        {
            var dateTime = new System.DateTime(1970, 1, 1).AddSeconds(seconds);
            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;
            
            return _dateTimeCommonData;
        }

        /// <summary>
        /// 获取当前时区的年月日时分秒信息
        /// </summary>
        /// <param name="isFromServer"></param>
        /// <param name="utc0SecondsFromServer"></param>
        /// <returns></returns>
        public static RDateTimeWarpYMDHMS GetLocalTimeInfo(bool isFromServer,double utc0SecondsFromServer)
        {
            var dateTime = !isFromServer
                ? System.DateTime.Now
                : new System.DateTime(1970, 1, 1).AddSeconds(utc0SecondsFromServer).ToLocalTime();

            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;
            
            return _dateTimeCommonData;
        }

        /// <summary>
        /// 获取当前时区的时间戳
        /// </summary>
        /// <param name="isFromServer"></param>
        /// <param name="utc0SecondsFromServer"></param>
        /// <returns></returns>
        public static double GetLocalTimeStamp(bool isFromServer,double utc0SecondsFromServer)
        {
            var dateTime = !isFromServer
                ? System.DateTime.Now
                : new System.DateTime(1970, 1, 1).AddSeconds(utc0SecondsFromServer).ToLocalTime();

            return (dateTime - new System.DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///  推送那儿的本地推送时间戳
        /// </summary>
        /// <param name="localTimeStamp"></param>
        /// <returns></returns>
        public static double GetUniversalTimeStamp(double localTimeStamp)
        {
            var date = new System.DateTime(1970, 1, 1).AddSeconds(localTimeStamp);
            return (date.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 生成的周期性推送的时间戳
        /// </summary>
        /// <param name="localTime"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static double GetPeriodicNotificationEndTimestamp(double localTime)
        {
            var localDate = new System.DateTime(1970, 1, 1).AddSeconds(localTime);
            return (new System.DateTime(localDate.Year, localDate.Month, localDate.Day).AddDays(7) -
                    new System.DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static double GetPeriodicNotificationLocalDayDate(double localTime, int delayDays,int hours,int minutes)
        {
            var localDate = new System.DateTime(1970, 1, 1).AddSeconds(localTime);
            var localDayDate = new System.DateTime(localDate.Year, localDate.Month, localDate.Day, 0, 0, 0);
            return (localDayDate.AddDays(delayDays).AddHours(hours).AddMinutes(minutes).ToUniversalTime() -
                    new System.DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取本地时区上指定年月日时分秒信息的时间戳
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double GetLocalTimeStampByParams(int year, int month, int day, int hour, int minute, int second)
        {
            var dateTime = new System.DateTime(year, month, day, hour, minute, second,System.DateTimeKind.Local)
                           - 
                           new System.DateTime(1970,1,1);
            return dateTime.TotalSeconds;
        }

        /// <summary>
        /// 获取UTC0时间戳（通过本地系统时间推算，本地系统时间无法避免作弊）
        /// </summary>
        /// <returns></returns>
        public static double GetUTC0TimeStamp()
        {
            return (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取UTC0时区的年月日时分秒信息
        /// </summary>
        /// <param name="isFromServer"></param>
        /// <param name="utc0SecondsFromServer"></param>
        /// <returns></returns>
        public static RDateTimeWarpYMDHMS GetUTC0TimeInfo(bool isFromServer,double utc0SecondsFromServer)
        {
            var dateTime = !isFromServer
                ? System.DateTime.UtcNow
                : new System.DateTime(1970, 1, 1).AddSeconds(utc0SecondsFromServer);
            
            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;
            
            return _dateTimeCommonData;
        }

        /// <summary>
        /// 根据UTC0时间戳获取UTC0年月日时分秒信息
        /// </summary>
        /// <param name="utc0TimeStamp"></param>
        /// <returns></returns>
        public static RDateTimeWarpYMDHMS GetUTC0TimeInfoByUTC0TimeStamp(double utc0TimeStamp)
        {
            var dateTime = new System.DateTime(1970,1,1).AddSeconds(utc0TimeStamp);
            
            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;
            
            return _dateTimeCommonData;
        }

        /// <summary>
        /// 获取UTC0时区上指定年月日时分秒信息的时间戳
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double GetUTC0TimeStampByParams(int year, int month, int day, int hour, int minute, int second)
        {
            var dateTime = new System.DateTime(year, month, day, hour, minute, second)
                           -
                           new System.DateTime(1970, 1, 1);
            return dateTime.TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转换成格式化字符串
        /// </summary>
        /// <param name="format"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static string GetTimeStringByTimeStamp(string format,double timeStamp)
        {
            return new System.DateTime(1970, 1, 1).AddSeconds(timeStamp).ToString(format);
        }
        #endregion
    }
}