using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
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
        public static byte[] GetBytesByBoolean(bool value)
        {
            byte[] buffer = new byte[1];
            GetBytesByBoolean(value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByBoolean(bool value, byte[] buffer)
        {
            GetBytesByBoolean(value, buffer, 0);
        }

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
        public static bool GetBoolean(byte[] value)
        {
            return BitConverter.ToBoolean(value, 0);
        }

        public static bool GetBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }
        #endregion

        #region 字符与字节数组互转
        /// <summary>
        /// 字符 → 字节数组
        /// </summary>
        public static byte[] GetBytesByChar(char value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort((short)value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByChar(char value, byte[] buffer)
        {
            GetBytesByShort((short)value, buffer, 0);
        }

        public static void GetBytesByChar(char value, byte[] buffer, int startIndex)
        {
            GetBytesByShort((short)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → 字符
        /// </summary>
        public static char GetChar(byte[] value)
        {
            return BitConverter.ToChar(value, 0);
        }

        public static char GetChar(byte[] value, int startIndex)
        {
            return BitConverter.ToChar(value, startIndex);
        }
        #endregion

        #region short / ushort 与字节数组互转
        /// <summary>
        /// short → 字节数组
        /// </summary>
        public static byte[] GetBytesByShort(short value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort(value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByShort(short value, byte[] buffer)
        {
            GetBytesByShort(value, buffer, 0);
        }

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
        public static short GetInt16(byte[] value)
        {
            return BitConverter.ToInt16(value, 0);
        }

        public static short GetInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToInt16(value, startIndex);
        }

        /// <summary>
        /// ushort → 字节数组
        /// </summary>
        public static byte[] GetBytesByUShort(ushort value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort((short)value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByUShort(ushort value, byte[] buffer)
        {
            GetBytesByShort((short)value, buffer, 0);
        }

        public static void GetBytesByUShort(ushort value, byte[] buffer, int startIndex)
        {
            GetBytesByShort((short)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → ushort
        /// </summary>
        public static ushort GetUInt16(byte[] value)
        {
            return BitConverter.ToUInt16(value, 0);
        }

        public static ushort GetUInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt16(value, startIndex);
        }
        #endregion

        #region int / uint 与字节数组互转
        /// <summary>
        /// int → 字节数组
        /// </summary>
        public static byte[] GetBytesByInt(int value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt(value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByInt(int value, byte[] buffer)
        {
            GetBytesByInt(value, buffer, 0);
        }

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
        public static int GetInt32(byte[] value)
        {
            return BitConverter.ToInt32(value, 0);
        }

        public static int GetInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToInt32(value, startIndex);
        }

        /// <summary>
        /// uint → 字节数组
        /// </summary>
        public static byte[] GetBytesByUInt(uint value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt((int)value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByUInt(uint value, byte[] buffer)
        {
            GetBytesByInt((int)value, buffer, 0);
        }

        public static void GetBytesByUInt(uint value, byte[] buffer, int startIndex)
        {
            GetBytesByInt((int)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → uint
        /// </summary>
        public static uint GetUInt32(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0);
        }

        public static uint GetUInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt32(value, startIndex);
        }
        #endregion

        #region long / ulong 与字节数组互转
        /// <summary>
        /// long → 字节数组
        /// </summary>
        public static byte[] GetBytesByLong(long value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong(value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByLong(long value, byte[] buffer)
        {
            GetBytesByLong(value, buffer, 0);
        }

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
        public static long GetInt64(byte[] value)
        {
            return BitConverter.ToInt64(value, 0);
        }

        public static long GetInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToInt64(value, startIndex);
        }

        /// <summary>
        /// ulong → 字节数组
        /// </summary>
        public static byte[] GetBytesByULong(ulong value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong((long)value, buffer, 0);
            return buffer;
        }

        public static void GetBytesByULong(ulong value, byte[] buffer)
        {
            GetBytesByLong((long)value, buffer, 0);
        }

        public static void GetBytesByULong(ulong value, byte[] buffer, int startIndex)
        {
            GetBytesByLong((long)value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → ulong
        /// </summary>
        public static ulong GetUInt64(byte[] value)
        {
            return BitConverter.ToUInt64(value, 0);
        }

        public static ulong GetUInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt64(value, startIndex);
        }
        #endregion

        #region float 与字节数组互转
        /// <summary>
        /// float → 字节数组（指针强转）
        /// </summary>
        public static unsafe byte[] GetBytesByFloat(float value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt(*(int*)&value, buffer, 0);
            return buffer;
        }

        public static unsafe void GetBytesByFloat(float value, byte[] buffer)
        {
            GetBytesByInt(*(int*)&value, buffer, 0);
        }

        public static unsafe void GetBytesByFloat(float value, byte[] buffer, int startIndex)
        {
            GetBytesByInt(*(int*)&value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → float
        /// </summary>
        public static float GetSingle(byte[] value)
        {
            return BitConverter.ToSingle(value, 0);
        }

        public static float GetSingle(byte[] value, int startIndex)
        {
            return BitConverter.ToSingle(value, startIndex);
        }
        #endregion

        #region double 与字节数组互转
        /// <summary>
        /// double → 字节数组（指针强转）
        /// </summary>
        public static unsafe byte[] GetBytesByDouble(double value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong(*(long*)&value, buffer, 0);
            return buffer;
        }

        public static unsafe void GetBytesByDouble(double value, byte[] buffer)
        {
            GetBytesByLong(*(long*)&value, buffer, 0);
        }

        public static unsafe void GetBytesByDouble(double value, byte[] buffer, int startIndex)
        {
            GetBytesByLong(*(long*)&value, buffer, startIndex);
        }

        /// <summary>
        /// 字节数组 → double
        /// </summary>
        public static double GetDouble(byte[] value)
        {
            return BitConverter.ToDouble(value, 0);
        }

        public static double GetDouble(byte[] value, int startIndex)
        {
            return BitConverter.ToDouble(value, startIndex);
        }
        #endregion

        #region 字符串与字节数组互转
        /// <summary>
        /// UTF8字符串 → 字节数组
        /// </summary>
        public static byte[] GetBytesByString(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 字节数组 → UTF8字符串
        /// </summary>
        public static string GetString(byte[] value)
        {
            if (value == null)
            {
                throw new Exception("Value 无效。");
            }
            return Encoding.UTF8.GetString(value);
        }

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
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }

        /// <summary>
        /// 字节数组 → 连续十六进制字符串
        /// </summary>
        public static string ToHex(this byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static string ToHex(this byte[] bytes, string format)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString(format));
            }
            return stringBuilder.ToString();
        }

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
        public static bool IsBase64String(string content)
        {
            return Regex.IsMatch(content, @"^[a-zA-Z0-9\+/]*={0,2}$");
        }

        /// <summary>
        /// 验证字节数组是否为 Base64 编码
        /// </summary>
        public static bool IsBase64Bytes(byte[] content)
        {
            return IsBase64String(Converter.GetString(content));
        }
        #endregion
    }
}