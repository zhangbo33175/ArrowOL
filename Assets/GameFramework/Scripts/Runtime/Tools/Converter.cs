/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Converter.cs
 * author:    云毅
 * created:   2026
 * descrip:   通用数据转换工具类，提供单位互转、基础类型与字节数组互转、
 *            纹理转字节流、文件编码处理、Base64验证等功能
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    //=========================================================================
    // 通用数据转换工具类
    //=========================================================================
    /// <summary>
    /// 通用数据转换工具类（静态）
    /// 功能：
    /// 1. 像素 / 厘米 / 英寸单位互转
    /// 2. 基础类型与字节数组互转（bool/char/short/int/float/double/string）
    /// 3. 字节数组与十六进制字符串互转
    /// 4. 纹理转字节流（支持非可读纹理）
    /// 5. Base64 格式验证
    /// 6. UTF8无BOM文件批量转换
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// 英寸转厘米系数（1英寸=2.54厘米）
        /// </summary>
        private const float InchesToCentimeters = 2.54f;

        /// <summary>
        /// 厘米转英寸系数
        /// </summary>
        private const float CentimetersToInches = 1f / InchesToCentimeters;

        /// <summary>
        /// 当前系统字节序（小端/大端）
        /// </summary>
        public static bool IsLittleEndian => BitConverter.IsLittleEndian;

        /// <summary>
        /// 屏幕DPI（单位英寸像素数）
        /// 必须先设置才能使用单位转换功能
        /// </summary>
        public static float ScreenDpi { get; set; }

        #region 像素 / 厘米 / 英寸 转换
        /// <summary>
        /// 像素 → 厘米
        /// </summary>
        /// <param name="pixels">像素值</param>
        /// <returns>厘米值</returns>
        /// <exception cref="Exception">未设置ScreenDpi时抛出异常</exception>
        public static float GetCentimetersFromPixels(float pixels)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return InchesToCentimeters * pixels / ScreenDpi;
        }

        /// <summary>
        /// 厘米 → 像素
        /// </summary>
        /// <param name="centimeters">厘米值</param>
        /// <returns>像素值</returns>
        /// <exception cref="Exception">未设置ScreenDpi时抛出异常</exception>
        public static float GetPixelsFromCentimeters(float centimeters)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return CentimetersToInches * centimeters * ScreenDpi;
        }

        /// <summary>
        /// 像素 → 英寸
        /// </summary>
        /// <param name="pixels">像素值</param>
        /// <returns>英寸值</returns>
        /// <exception cref="Exception">未设置ScreenDpi时抛出异常</exception>
        public static float GetInchesFromPixels(float pixels)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return pixels / ScreenDpi;
        }

        /// <summary>
        /// 英寸 → 像素
        /// </summary>
        /// <param name="inches">英寸值</param>
        /// <returns>像素值</returns>
        /// <exception cref="Exception">未设置ScreenDpi时抛出异常</exception>
        public static float GetPixelsFromInches(float inches)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return inches * ScreenDpi;
        }
        #endregion

        #region 布尔值与字节数组互转
        /// <summary>
        /// 布尔值 → 字节数组
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByBoolean(bool value)
        {
            byte[] buffer = new byte[1];
            GetBytesByBoolean(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 布尔值写入字节数组
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByBoolean(bool value, byte[] buffer)
        {
            GetBytesByBoolean(value, buffer, 0);
        }

        /// <summary>
        /// 布尔值写入字节数组指定位置
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <exception cref="Exception">缓冲区无效或索引越界时抛出异常</exception>
        public static void GetBytesByBoolean(bool value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 1 > buffer.Length)
            {
                throw new Exception("startIndex 无效。");
            }

            buffer[startIndex] = value ? (byte)1 : (byte)0;
        }

        /// <summary>
        /// 字节数组 → 布尔值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>布尔值</returns>
        public static bool GetBoolean(byte[] value)
        {
            return BitConverter.ToBoolean(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → 布尔值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>布尔值</returns>
        public static bool GetBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }
        #endregion

        #region 字符与字节数组互转
        /// <summary>
        /// 字符 → 字节数组
        /// </summary>
        /// <param name="value">字符</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByChar(char value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort((short)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 字符写入字节数组
        /// </summary>
        /// <param name="value">字符</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByChar(char value, byte[] buffer)
        {
            GetBytesByShort((short)value, buffer, 0);
        }

        /// <summary>
        /// 字符写入字节数组指定位置
        /// </summary>
        /// <param name="value">字符</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        public static void GetBytesByChar(char value, byte[] buffer, int startIndex)
        {
            GetBytesByShort((short)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → 字符
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>字符</returns>
        public static char GetChar(byte[] value)
        {
            return BitConverter.ToChar(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → 字符
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>字符</returns>
        public static char GetChar(byte[] value, int startIndex)
        {
            return BitConverter.ToChar(value, startIndex);
        }
        #endregion

        #region short / ushort 与字节数组互转
        /// <summary>
        /// short → 字节数组
        /// </summary>
        /// <param name="value">short数值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByShort(short value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// short写入字节数组
        /// </summary>
        /// <param name="value">short数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByShort(short value, byte[] buffer)
        {
            GetBytesByShort(value, buffer, 0);
        }

        /// <summary>
        /// short写入字节数组指定位置
        /// </summary>
        /// <param name="value">short数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <exception cref="Exception">缓冲区无效或索引越界时抛出异常</exception>
        public static unsafe void GetBytesByShort(short value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 2 > buffer.Length)
            {
                throw new Exception("startIndex 无效。");
            }

            fixed (byte* valueRef = buffer)
            {
                *(short*)(valueRef + startIndex) = value;
            }
        }

        /// <summary>
        /// 字节数组 → short
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>short数值</returns>
        public static short GetInt16(byte[] value)
        {
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → short
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>short数值</returns>
        public static short GetInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToInt16(value, startIndex);
        }

        /// <summary>
        /// ushort → 字节数组
        /// </summary>
        /// <param name="value">ushort数值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByUShort(ushort value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort((short)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// ushort写入字节数组
        /// </summary>
        /// <param name="value">ushort数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByUShort(ushort value, byte[] buffer)
        {
            GetBytesByShort((short)value, buffer, 0);
        }

        /// <summary>
        /// ushort写入字节数组指定位置
        /// </summary>
        /// <param name="value">ushort数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        public static void GetBytesByUShort(ushort value, byte[] buffer, int startIndex)
        {
            GetBytesByShort((short)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → ushort
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>ushort数值</returns>
        public static ushort GetUInt16(byte[] value)
        {
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → ushort
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>ushort数值</returns>
        public static ushort GetUInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt16(value, startIndex);
        }
        #endregion

        #region int / uint 与字节数组互转
        /// <summary>
        /// int → 字节数组
        /// </summary>
        /// <param name="value">int数值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByInt(int value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// int写入字节数组
        /// </summary>
        /// <param name="value">int数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByInt(int value, byte[] buffer)
        {
            GetBytesByInt(value, buffer, 0);
        }

        /// <summary>
        /// int写入字节数组指定位置
        /// </summary>
        /// <param name="value">int数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <exception cref="Exception">缓冲区无效或索引越界时抛出异常</exception>
        public static unsafe void GetBytesByInt(int value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 4 > buffer.Length)
            {
                throw new Exception("Start index 无效。");
            }

            fixed (byte* valueRef = buffer)
            {
                *(int*)(valueRef + startIndex) = value;
            }
        }

        /// <summary>
        /// 字节数组 → int
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>int数值</returns>
        public static int GetInt32(byte[] value)
        {
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → int
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>int数值</returns>
        public static int GetInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToInt32(value, startIndex);
        }

        /// <summary>
        /// uint → 字节数组
        /// </summary>
        /// <param name="value">uint数值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByUInt(uint value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt((int)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// uint写入字节数组
        /// </summary>
        /// <param name="value">uint数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByUInt(uint value, byte[] buffer)
        {
            GetBytesByInt((int)value, buffer, 0);
        }

        /// <summary>
        /// uint写入字节数组指定位置
        /// </summary>
        /// <param name="value">uint数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        public static void GetBytesByUInt(uint value, byte[] buffer, int startIndex)
        {
            GetBytesByInt((int)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → uint
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>uint数值</returns>
        public static uint GetUInt32(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → uint
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>uint数值</returns>
        public static uint GetUInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt32(value, startIndex);
        }
        #endregion

        #region long / ulong 与字节数组互转
        /// <summary>
        /// long → 字节数组
        /// </summary>
        /// <param name="value">long数值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByLong(long value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// long写入字节数组
        /// </summary>
        /// <param name="value">long数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByLong(long value, byte[] buffer)
        {
            GetBytesByLong(value, buffer, 0);
        }

        /// <summary>
        /// long写入字节数组指定位置
        /// </summary>
        /// <param name="value">long数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <exception cref="Exception">缓冲区无效或索引越界时抛出异常</exception>
        public static unsafe void GetBytesByLong(long value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 8 > buffer.Length)
            {
                throw new Exception("Start index 无效。");
            }

            fixed (byte* valueRef = buffer)
            {
                *(long*)(valueRef + startIndex) = value;
            }
        }

        /// <summary>
        /// 字节数组 → long
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>long数值</returns>
        public static long GetInt64(byte[] value)
        {
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → long
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>long数值</returns>
        public static long GetInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToInt64(value, startIndex);
        }

        /// <summary>
        /// ulong → 字节数组
        /// </summary>
        /// <param name="value">ulong数值</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetBytesByULong(ulong value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong((long)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// ulong写入字节数组
        /// </summary>
        /// <param name="value">ulong数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static void GetBytesByULong(ulong value, byte[] buffer)
        {
            GetBytesByLong((long)value, buffer, 0);
        }

        /// <summary>
        /// ulong写入字节数组指定位置
        /// </summary>
        /// <param name="value">ulong数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        public static void GetBytesByULong(ulong value, byte[] buffer, int startIndex)
        {
            GetBytesByLong((long)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → ulong
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>ulong数值</returns>
        public static ulong GetUInt64(byte[] value)
        {
            return BitConverter.ToUInt64(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → ulong
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>ulong数值</returns>
        public static ulong GetUInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt64(value, startIndex);
        }
        #endregion

        #region float 与字节数组互转
        /// <summary>
        /// float → 字节数组（指针强转）
        /// </summary>
        /// <param name="value">float数值</param>
        /// <returns>转换后的字节数组</returns>
        public static unsafe byte[] GetBytesByFloat(float value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt(*(int*)&value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// float写入字节数组
        /// </summary>
        /// <param name="value">float数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static unsafe void GetBytesByFloat(float value, byte[] buffer)
        {
            GetBytesByInt(*(int*)&value, buffer, 0);
        }

        /// <summary>
        /// float写入字节数组指定位置
        /// </summary>
        /// <param name="value">float数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        public static unsafe void GetBytesByFloat(float value, byte[] buffer, int startIndex)
        {
            GetBytesByInt(*(int*)&value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → float
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>float数值</returns>
        public static float GetSingle(byte[] value)
        {
            return BitConverter.ToSingle(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → float
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>float数值</returns>
        public static float GetSingle(byte[] value, int startIndex)
        {
            return BitConverter.ToSingle(value, startIndex);
        }
        #endregion

        #region double 与字节数组互转
        /// <summary>
        /// double → 字节数组（指针强转）
        /// </summary>
        /// <param name="value">double数值</param>
        /// <returns>转换后的字节数组</returns>
        public static unsafe byte[] GetBytesByDouble(double value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong(*(long*)&value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// double写入字节数组
        /// </summary>
        /// <param name="value">double数值</param>
        /// <param name="buffer">目标字节数组</param>
        public static unsafe void GetBytesByDouble(double value, byte[] buffer)
        {
            GetBytesByLong(*(long*)&value, buffer, 0);
        }

        /// <summary>
        /// double写入字节数组指定位置
        /// </summary>
        /// <param name="value">double数值</param>
        /// <param name="buffer">目标字节数组</param>
        /// <param name="startIndex">起始索引</param>
        public static unsafe void GetBytesByDouble(double value, byte[] buffer, int startIndex)
        {
            GetBytesByLong(*(long*)&value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → double
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>double数值</returns>
        public static double GetDouble(byte[] value)
        {
            return BitConverter.ToDouble(value, 0);
        }

        /// <summary>
        /// 字节数组指定位置 → double
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>double数值</returns>
        public static double GetDouble(byte[] value, int startIndex)
        {
            return BitConverter.ToDouble(value, startIndex);
        }
        #endregion

        #region 字符串与字节数组互转
        /// <summary>
        /// UTF8字符串 → 字节数组
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>UTF8字节数组</returns>
        public static byte[] GetBytesByString(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 字节数组 → UTF8字符串
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>UTF8字符串</returns>
        /// <exception cref="Exception">字节数组为空时抛出异常</exception>
        public static string GetString(byte[] value)
        {
            if (value == null)
            {
                throw new Exception("Value 无效。");
            }
            return Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// 字节数组指定区间 → UTF8字符串
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">读取长度</param>
        /// <returns>UTF8字符串</returns>
        /// <exception cref="Exception">字节数组为空时抛出异常</exception>
        public static string GetString(byte[] value, int startIndex, int length)
        {
            if (value == null)
            {
                throw new Exception("Value 无效。");
            }

            return Encoding.UTF8.GetString(value, startIndex, length);
        }
        #endregion

        #region 字节数组 ↔ 十六进制字符串
        /// <summary>
        /// 单字节 → 十六进制字符串（00~FF）
        /// </summary>
        /// <param name="b">字节数据</param>
        /// <returns>两位十六进制字符串</returns>
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }

        /// <summary>
        /// 字节数组 → 连续十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHex(this byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 字节数组按自定义格式 → 十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="format">格式化字符串</param>
        /// <returns>格式化后的十六进制字符串</returns>
        public static string ToHex(this byte[] bytes, string format)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString(format));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 字节数组指定区间 → 十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="offset">起始偏移量</param>
        /// <param name="count">转换长度</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHex(this byte[] bytes, int offset, int count)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = offset; i < offset + count; ++i)
            {
                stringBuilder.Append(bytes[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region 文件编码处理
        /// <summary>
        /// 批量将目录下指定后缀文件转为 UTF8 无 BOM 编码
        /// </summary>
        /// <param name="dirName">目标目录</param>
        /// <param name="suffixInfos">文件后缀匹配符</param>
        public static void ToNoBOMUTF8(string dirName, string suffixInfos)
        {
            string[] fileFullPaths = Directory.GetFiles(dirName, suffixInfos, SearchOption.AllDirectories);
            for (int index = 0; index < fileFullPaths.Length; index++)
            {
                string content = File.ReadAllText(fileFullPaths[index]);
                File.WriteAllText(fileFullPaths[index], content, new System.Text.UTF8Encoding(false));
            }
        }
        #endregion

        #region 纹理转字节流
        /// <summary>
        /// 纹理转 PNG/JPG 字节流（支持非可读纹理）
        /// </summary>
        /// <param name="texture">纹理对象</param>
        /// <param name="isJpeg">是否为JPEG格式</param>
        /// <returns>纹理字节流</returns>
        public static byte[] GetTextureBytes(Texture2D texture, bool isJpeg)
        {
            try
            {
                return isJpeg ? texture.EncodeToJPG(100) : texture.EncodeToPNG();
            }
            catch (UnityException)
            {
                return GetTextureBytesFromCopy(texture, isJpeg);
            }
            catch (ArgumentException)
            {
                return GetTextureBytesFromCopy(texture, isJpeg);
            }

            return null;
        }

        /// <summary>
        /// 非可读纹理 → 拷贝为可读纹理 → 转字节流
        /// </summary>
        /// <param name="texture">非可读纹理</param>
        /// <param name="isJpeg">是否为JPEG格式</param>
        /// <returns>纹理字节流</returns>
        private static byte[] GetTextureBytesFromCopy(Texture2D texture, bool isJpeg)
        {
            Debug.LogWarning("Saving non-readable textures is slower than saving readable textures");

            Texture2D sourceTexReadable = null;
            RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height);
            RenderTexture activeRT = RenderTexture.active;

            try
            {
                Graphics.Blit(texture, rt);
                RenderTexture.active = rt;

                sourceTexReadable = new Texture2D(texture.width, texture.height, isJpeg ? TextureFormat.RGB24 : TextureFormat.RGBA32, false);
                sourceTexReadable.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0, false);
                sourceTexReadable.Apply(false, false);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UnityEngine.Object.DestroyImmediate(sourceTexReadable);
                return null;
            }
            finally
            {
                RenderTexture.active = activeRT;
                RenderTexture.ReleaseTemporary(rt);
            }

            try
            {
                return isJpeg ? sourceTexReadable.EncodeToJPG(100) : sourceTexReadable.EncodeToPNG();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(sourceTexReadable);
            }
        }
        #endregion

        #region Base64 验证
        /// <summary>
        /// 验证字符串是否符合 Base64 格式
        /// </summary>
        /// <param name="content">待验证字符串</param>
        /// <returns>是否为合法Base64格式</returns>
        public static bool IsBase64String(string content)
        {
            return Regex.IsMatch(content, @"^[a-zA-Z0-9\+/]*={0,2}$");
        }

        /// <summary>
        /// 验证字节数组是否为 Base64 编码
        /// </summary>
        /// <param name="content">待验证字节数组</param>
        /// <returns>是否为合法Base64编码</returns>
        public static bool IsBase64Bytes(byte[] content)
        {
            return IsBase64String(Converter.GetString(content));
        }
        #endregion
    }
}