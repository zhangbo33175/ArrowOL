/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  RSetupLuaTableValue.cs
 * author:    云毅
 * created:   2026
 * descrip:   Lua ↔ C# 交互工具类
 *            提供坐标转换、时间计算、UI检测、时间戳转换等通用接口
 *            专供 Lua 脚本调用，实现跨语言功能互通
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace GameLib
{
    /// <summary>
    /// Lua ↔ C# 交互工具类
    /// 提供：屏幕坐标转换、时间获取、UI检测、时间戳/日期转换、本地/UTC时间计算
    /// 专供 Lua 脚本调用，实现跨语言功能互通
    /// </summary>
    public static class RSetupLuaTableValue
    {
        #region 坐标转换
        //=========================================================================
        // 坐标转换
        //=========================================================================
        /// <summary>
        /// 屏幕坐标转世界坐标（结果写入LuaTable）
        /// </summary>
        /// <param name="camera">相机</param>
        /// <param name="screenPosition">屏幕坐标</param>
        /// <param name="worldPosition">Lua表（用于接收x/y/z）</param>
        public static void RScreenToWorldPoint(Camera camera, Vector3 screenPosition, LuaTable worldPosition)
        {
            Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, screenPosition.z);
            Vector3 result = camera.ScreenToWorldPoint(screenPoint);

            // 把结果写入Lua表
            worldPosition.Set("x", result.x);
            worldPosition.Set("y", result.y);
            worldPosition.Set("z", result.z);
        }
        #endregion

        #region 时间获取
        //=========================================================================
        // 时间获取
        //=========================================================================
        /// <summary>
        /// 获取帧间隔时间 deltaTime
        /// </summary>
        public static float GetTimeDeltaTime()
        {
            return Time.deltaTime;
        }

        /// <summary>
        /// 获取游戏启动总时间
        /// </summary>
        public static float GetRealtimeSinceStartup()
        {
            return Time.realtimeSinceStartup;
        }
        #endregion

        #region UI 检测
        //=========================================================================
        // UI 检测
        //=========================================================================
        private static List<RaycastResult> _isPointerOverUIObjectResult;

        /// <summary>
        /// 判断鼠标是否点击在UI上（检测UI层）
        /// </summary>
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            _isPointerOverUIObjectResult ??= new List<RaycastResult>();
            _isPointerOverUIObjectResult.Clear();
            EventSystem.current.RaycastAll(eventData, _isPointerOverUIObjectResult);

            // 检测是否存在 UI 层的物体
            return _isPointerOverUIObjectResult.Count > 0 &&
                   _isPointerOverUIObjectResult.Exists(ui =>
                       ui.gameObject && ui.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
        #endregion

        #region 日期时间工具
        //=========================================================================
        // 日期时间工具
        //=========================================================================
        /// <summary>
        /// 年月日时分秒包装类
        /// Lua获取时间的通用数据结构
        /// </summary>
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
        /// 静态公用容器（Lua端获取后需立刻复制数据）
        /// 减少GC，复用对象
        /// </summary>
        private static readonly RDateTimeWarpYMDHMS _dateTimeCommonData = new();

        /// <summary>
        /// 从1970-01-01加上秒数，获取年月日时分秒
        /// </summary>
        public static RDateTimeWarpYMDHMS GetDateTime1970AddSeconds(int seconds)
        {
            DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(seconds);
            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;

            return _dateTimeCommonData;
        }

        /// <summary>
        /// 获取本地时区时间（支持服务器时间戳）
        /// </summary>
        public static RDateTimeWarpYMDHMS GetLocalTimeInfo(bool isFromServer, double utc0SecondsFromServer)
        {
            DateTime dateTime = !isFromServer
                ? DateTime.Now
                : new DateTime(1970, 1, 1).AddSeconds(utc0SecondsFromServer).ToLocalTime();

            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;

            return _dateTimeCommonData;
        }

        /// <summary>
        /// 获取本地时间戳（1970年起）
        /// </summary>
        public static double GetLocalTimeStamp(bool isFromServer, double utc0SecondsFromServer)
        {
            DateTime dateTime = !isFromServer
                ? DateTime.Now
                : new DateTime(1970, 1, 1).AddSeconds(utc0SecondsFromServer).ToLocalTime();

            return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 本地时间戳 → UTC0时间戳
        /// </summary>
        public static double GetUniversalTimeStamp(double localTimeStamp)
        {
            DateTime date = new DateTime(1970, 1, 1).AddSeconds(localTimeStamp);
            return (date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取周期性推送结束时间戳（7天后）
        /// </summary>
        public static double GetPeriodicNotificationEndTimestamp(double localTime)
        {
            DateTime localDate = new DateTime(1970, 1, 1).AddSeconds(localTime);
            return (new DateTime(localDate.Year, localDate.Month, localDate.Day).AddDays(7)
                    - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取指定延迟天数/时分的本地日期时间戳（推送用）
        /// </summary>
        public static double GetPeriodicNotificationLocalDayDate(double localTime, int delayDays, int hours,
            int minutes)
        {
            DateTime localDate = new DateTime(1970, 1, 1).AddSeconds(localTime);
            DateTime localDayDate = new DateTime(localDate.Year, localDate.Month, localDate.Day, 0, 0, 0);

            return (localDayDate.AddDays(delayDays).AddHours(hours).AddMinutes(minutes).ToUniversalTime()
                    - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 根据年月日时分秒 获取本地时间戳
        /// </summary>
        public static double GetLocalTimeStampByParams(int year, int month, int day, int hour, int minute, int second)
        {
            TimeSpan dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local)
                                - new DateTime(1970, 1, 1);
            return dateTime.TotalSeconds;
        }

        /// <summary>
        /// 获取当前UTC0时间戳（防本地时间作弊）
        /// </summary>
        public static double GetUTC0TimeStamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 获取UTC0时间信息
        /// </summary>
        public static RDateTimeWarpYMDHMS GetUTC0TimeInfo(bool isFromServer, double utc0SecondsFromServer)
        {
            DateTime dateTime = !isFromServer
                ? DateTime.UtcNow
                : new DateTime(1970, 1, 1).AddSeconds(utc0SecondsFromServer);

            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;

            return _dateTimeCommonData;
        }

        /// <summary>
        /// 根据UTC0时间戳 获取UTC0时间信息
        /// </summary>
        public static RDateTimeWarpYMDHMS GetUTC0TimeInfoByUTC0TimeStamp(double utc0TimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(utc0TimeStamp);

            _dateTimeCommonData.Year = dateTime.Year;
            _dateTimeCommonData.Month = dateTime.Month;
            _dateTimeCommonData.Day = dateTime.Day;
            _dateTimeCommonData.Hour = dateTime.Hour;
            _dateTimeCommonData.Minute = dateTime.Minute;
            _dateTimeCommonData.Second = dateTime.Second;

            return _dateTimeCommonData;
        }

        /// <summary>
        /// 根据年月日时分秒 获取UTC0时间戳
        /// </summary>
        public static double GetUTC0TimeStampByParams(int year, int month, int day, int hour, int minute, int second)
        {
            TimeSpan dateTime = new DateTime(year, month, day, hour, minute, second)
                                - new DateTime(1970, 1, 1);
            return dateTime.TotalSeconds;
        }

        /// <summary>
        /// 时间戳 → 自定义格式化字符串
        /// </summary>
        public static string GetTimeStringByTimeStamp(string format, double timeStamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timeStamp).ToString(format);
        }
        #endregion
    }
}