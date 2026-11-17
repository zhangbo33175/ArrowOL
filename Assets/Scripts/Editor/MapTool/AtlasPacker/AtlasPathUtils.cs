/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasPathUtils.cs
 * author:    云毅
 * created:   2026
 * descrip:   Atlas 打包通用工具（POT 对齐等）
 ***************************************************************/

using System.IO;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Atlas 打包通用工具
    /// 路径换算已统一收敛到 MapToolPathHelper，本类只保留与图集打包相关的尺寸/名字工具
    /// </summary>
    internal static class AtlasPathUtils
    {
        /// <summary>
        /// 计算下一个 2 的幂（POT 对齐）
        /// </summary>
        public static int NextPOT(int value)
        {
            if (value <= 0) return 1;
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }
    }
}
