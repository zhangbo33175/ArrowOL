/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PngAtlasEncoder.cs
 * author:    云毅
 * created:   2026
 * descrip:   Editor 端 RGBA8 PNG 编码器（复用 SharpZipLib zlib 流 + 逐行滤镜选优）
 ***************************************************************/

using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip.Compression;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 图集 PNG 优化编码器。
    /// 仅在 Editor 导出链路使用，不进入运行时包。
    /// 输出 8-bit / RGBA / 非交错 PNG，IDAT 采用 level 9 zlib 包装流，
    /// 每行在 5 种 PNG 滤镜中按残差绝对值和选最优；deflate 策略在
    /// Default / Filtered 之间择优取较小输出，进一步降低导出体积。
    /// </summary>
    internal static class PngAtlasEncoder
    {
        /// <summary>每像素字节数（RGBA8）。</summary>
        private const int BytesPerPixel = 4;

        /// <summary>deflate 压缩级别（9 = 最高压缩）。</summary>
        private const int CompressionLevel = 9;

        /// <summary>
        /// 把纹理编码为优化 PNG 字节流。
        /// </summary>
        /// <param name="tex">RGBA32 纹理（像素已做透明 RGB 清零）。</param>
        /// <returns>PNG 文件字节；编码失败时由调用方兜底回退 EncodeToPNG。</returns>
        public static byte[] Encode(Texture2D tex)
        {
            if (tex == null) throw new ArgumentNullException(nameof(tex));
            Color32[] pixels = tex.GetPixels32();
            return Encode(pixels, tex.width, tex.height);
        }

        /// <summary>
        /// 把 RGBA 像素数组编码为优化 PNG 字节流。
        /// </summary>
        /// <param name="pixels">Texture2D 行序像素（第 0 行为纹理底部）。</param>
        /// <param name="width">图集宽度。</param>
        /// <param name="height">图集高度。</param>
        public static byte[] Encode(Color32[] pixels, int width, int height)
        {
            if (pixels == null) throw new ArgumentNullException(nameof(pixels));
            if (width <= 0 || height <= 0) throw new ArgumentException("图集尺寸必须大于 0");

            int rowBytes = width * BytesPerPixel;

            // 1. 整理为 PNG 扫描线行序（顶到底）。Texture2D 像素数组为底到顶，需翻转 Y。
            byte[][] rawRows = BuildRawRows(pixels, width, height, rowBytes);

            // 2. 逐行滤镜选优，拼接滤波流（每行前 1 字节滤镜类型）。
            byte[] filtered = BuildFilteredStream(rawRows, rowBytes);

            // 3. zlib 包装流压缩滤波流（PNG IDAT 格式）。
            //    同一滤波流分别用 Default / Filtered 策略压缩，取较小输出；
            //    两种策略均为无损，Filtered 更贴合 PNG 滤波残差（小重复值），
            //    对高熵数据回退 Default，保证不劣化。
            byte[] idatDefault = DeflateZlib(filtered, CompressionLevel, DeflateStrategy.Default);
            byte[] idatFiltered = DeflateZlib(filtered, CompressionLevel, DeflateStrategy.Filtered);
            byte[] idat = idatFiltered.Length < idatDefault.Length ? idatFiltered : idatDefault;

            // 4. 组装 PNG：签名 + IHDR + IDAT + IEND。
            using (var ms = new MemoryStream())
            {
                ms.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, 0, 8);

                var ihdr = new byte[13];
                WriteBigEndian(ihdr, 0, width);
                WriteBigEndian(ihdr, 4, height);
                ihdr[8] = 8;    // bit depth = 8
                ihdr[9] = 6;    // color type = RGBA
                ihdr[10] = 0;   // compression method = deflate
                ihdr[11] = 0;   // filter method = adaptive
                ihdr[12] = 0;   // interlace = none
                WriteChunk(ms, "IHDR", ihdr);

                WriteChunk(ms, "IDAT", idat);
                WriteChunk(ms, "IEND", Array.Empty<byte>());

                return ms.ToArray();
            }
        }

        /// <summary>把 Texture2D 像素数组翻转为顶到底的原始扫描线。</summary>
        private static byte[][] BuildRawRows(Color32[] pixels, int width, int height, int rowBytes)
        {
            var rows = new byte[height][];
            for (int y = 0; y < height; y++)
            {
                var row = new byte[rowBytes];
                int srcY = height - 1 - y;
                for (int x = 0; x < width; x++)
                {
                    Color32 c = pixels[srcY * width + x];
                    int o = x * BytesPerPixel;
                    row[o] = c.r;
                    row[o + 1] = c.g;
                    row[o + 2] = c.b;
                    row[o + 3] = c.a;
                }
                rows[y] = row;
            }
            return rows;
        }

        /// <summary>逐行在 5 种滤镜中选残差绝对值和最小者，输出滤波流。</summary>
        private static byte[] BuildFilteredStream(byte[][] rawRows, int rowBytes)
        {
            int height = rawRows.Length;
            var filtered = new byte[height * (1 + rowBytes)];
            var prev = new byte[rowBytes];
            int pos = 0;

            for (int y = 0; y < height; y++)
            {
                byte[] row = rawRows[y];
                ChooseBestFilter(row, prev, rowBytes, out int filter, out byte[] filteredRow);

                filtered[pos++] = (byte)filter;
                Buffer.BlockCopy(filteredRow, 0, filtered, pos, rowBytes);
                pos += rowBytes;

                prev = row;
            }
            return filtered;
        }

        /// <summary>对单行尝试 5 种 PNG 滤镜，选出残差绝对值和最小的一种。</summary>
        private static void ChooseBestFilter(byte[] row, byte[] prev, int rowBytes,
            out int bestFilter, out byte[] bestRow)
        {
            bestFilter = 0;
            bestRow = null;
            long bestSum = long.MaxValue;

            for (int filter = 0; filter < 5; filter++)
            {
                var buf = new byte[rowBytes];
                long sum = 0;

                for (int i = 0; i < rowBytes; i++)
                {
                    byte a = i >= BytesPerPixel ? row[i - BytesPerPixel] : (byte)0;
                    byte b = prev[i];
                    byte c = i >= BytesPerPixel ? prev[i - BytesPerPixel] : (byte)0;

                    int predictor = filter switch
                    {
                        0 => 0,                                  // None
                        1 => a,                                  // Sub
                        2 => b,                                  // Up
                        3 => (a + b) >> 1,                       // Average
                        4 => PaethPredictor(a, b, c),            // Paeth
                        _ => 0
                    };

                    int residual = (row[i] - predictor) & 0xFF;
                    buf[i] = (byte)residual;
                    sum += residual < 128 ? residual : 256 - residual;
                }

                if (sum < bestSum)
                {
                    bestSum = sum;
                    bestFilter = filter;
                    bestRow = buf;
                }
            }
        }

        /// <summary>Paeth 预测器：在左 a、上 b、左上 c 中取最接近 p=a+b-c 的一个。</summary>
        private static int PaethPredictor(int a, int b, int c)
        {
            int p = a + b - c;
            int pa = Math.Abs(p - a);
            int pb = Math.Abs(p - b);
            int pc = Math.Abs(p - c);

            if (pa <= pb && pa <= pc) return a;
            if (pb <= pc) return b;
            return c;
        }

        /// <summary>用 SharpZipLib 输出 zlib 包装流（含 zlib 头与 Adler32 尾），匹配 PNG IDAT。</summary>
        /// <param name="data">待压缩滤波流。</param>
        /// <param name="level">deflate 级别（0..9）。</param>
        /// <param name="strategy">deflate 策略（Default / Filtered）。</param>
        private static byte[] DeflateZlib(byte[] data, int level, DeflateStrategy strategy)
        {
            // noZlibHeaderOrFooter: false —— PNG IDAT 是 RFC1950 zlib 流，不是 raw deflate。
            var deflater = new Deflater(level, false);
            deflater.SetStrategy(strategy);
            deflater.SetInput(data, 0, data.Length);

            var buffer = new byte[16384];
            using (var ms = new MemoryStream())
            {
                // 排空输入缓冲
                while (!deflater.IsNeedingInput)
                {
                    int count = deflater.Deflate(buffer, 0, buffer.Length);
                    if (count > 0) ms.Write(buffer, 0, count);
                }

                deflater.Finish();

                // 排空剩余压缩输出
                while (!deflater.IsFinished)
                {
                    int count = deflater.Deflate(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        ms.Write(buffer, 0, count);
                    }
                    else
                    {
                        break;
                    }
                }

                return ms.ToArray();
            }
        }

        /// <summary>写入一个 PNG chunk：长度 + 类型 + 数据 + CRC32（CRC 覆盖类型与数据）。</summary>
        private static void WriteChunk(Stream stream, string type, byte[] data)
        {
            var lengthField = new byte[4];
            WriteBigEndian(lengthField, 0, data.Length);
            stream.Write(lengthField, 0, 4);

            byte[] typeBytes = Encoding.ASCII.GetBytes(type);
            stream.Write(typeBytes, 0, 4);

            if (data.Length > 0) stream.Write(data, 0, data.Length);

            var crc = new Crc32();
            crc.Update(typeBytes);
            crc.Update(data);
            uint checksum = (uint)crc.Value;

            var crcField = new byte[4];
            crcField[0] = (byte)(checksum >> 24);
            crcField[1] = (byte)(checksum >> 16);
            crcField[2] = (byte)(checksum >> 8);
            crcField[3] = (byte)checksum;
            stream.Write(crcField, 0, 4);
        }

        /// <summary>把 int 以大端序写入字节数组指定偏移。</summary>
        private static void WriteBigEndian(byte[] buffer, int offset, int value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }
    }
}
