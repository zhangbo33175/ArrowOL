/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SpriteItemInfo.cs
 * author:    云毅
 * created:   2026
 * descrip:   精灵项数据结构
 ***************************************************************/

using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Sprite 项信息（数据结构）
    /// </summary>
    internal class SpriteItemInfo
    {
        /// <summary>
        /// 图片名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 资源路径（Assets/...）
        /// </summary>
        public string Path;

        /// <summary>
        /// 尺寸字符串（例如 "128x64"）
        /// </summary>
        public string Size;

        /// <summary>
        /// 在图集中的 X 坐标（从左上角开始）
        /// </summary>
        public int PosX;

        /// <summary>
        /// 在图集中的 Y 坐标（从左上角开始）
        /// </summary>
        public int PosY;

        /// <summary>
        /// 是否在图集中旋转 90° 放置（Allow rotation 开启时由排布算法决定）
        /// </summary>
        public bool Rotated;

        /// <summary>
        /// 是否被排除出图集（尺寸超图集上限、纹理加载失败等）
        /// </summary>
        public bool Excluded;

        /// <summary>
        /// 排除原因（仅 Excluded = true 时有值）
        /// </summary>
        public string ExclusionReason;

        /// <summary>
        /// 别名指向的主项（null = 主项/独立精灵；非 null = 像素重复，复用主项槽位，不单独占位）
        /// </summary>
        public SpriteItemInfo AliasOf;

        /// <summary>
        /// 是否为别名精灵（像素重复，与主项共享图集槽位）
        /// </summary>
        public bool IsAlias => AliasOf != null;

        /// <summary>
        /// 复位排布结果（坐标与旋转归零）
        /// 排布失败 / 被排除 / 重新排布前统一调用，避免各处各写三行赋值
        /// </summary>
        public void ResetPlacement()
        {
            PosX = 0;
            PosY = 0;
            Rotated = false;
        }

        /// <summary>
        /// 标记为被排除（记录原因并复位排布结果），排布失败/超限/加载失败统一走这里
        /// </summary>
        /// <param name="reason">排除原因（写入 ExclusionReason）</param>
        public void MarkExcluded(string reason)
        {
            Excluded = true;
            ExclusionReason = reason;
            ResetPlacement();
        }

        /// <summary>
        /// 清除排除标记（Excluded 与 ExclusionReason 必须成对清零，避免只清其中一个）
        /// </summary>
        public void ClearExclusion()
        {
            Excluded = false;
            ExclusionReason = null;
        }

        /// <summary>
        /// 标记为主项参与排布（清除排除标记并复位排布结果）
        /// </summary>
        public void MarkIncluded()
        {
            ClearExclusion();
            ResetPlacement();
        }
    }
}
