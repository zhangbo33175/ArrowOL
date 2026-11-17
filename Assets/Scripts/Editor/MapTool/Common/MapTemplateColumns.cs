/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapTemplateColumns.cs
 * author:    云毅
 * created:   2026
 * descrip:   模板配置表列名唯一口径（表头第2行）。读写两侧共用本文件，
 *            新增/改名列只改这一处，避免 Reader / Writer 各写一份常量表而漂移。
 ***************************************************************/

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 模板配置表列名常量（列名字符串即 Excel 第2行表头，改动需与策划表同步）
    /// </summary>
    public static class MapTemplateColumns
    {
        #region 标识与展示

        /// <summary>模板ID</summary>
        public const string ColumnTemplateId = "TemplateId";
        /// <summary>模板名称</summary>
        public const string ColumnTemplateName = "TemplateName";
        /// <summary>资源名（决定物件类型归属）</summary>
        public const string ColumnResName = "ResName";
        /// <summary>图片路径</summary>
        public const string ColumnImagePath = "ImagePath";
        /// <summary>显示层级（八层键）</summary>
        public const string ColumnDisplayLayer = "DisplayLayer";
        /// <summary>排序值（层内槽位）</summary>
        public const string ColumnSortOrder = "SortOrder";

        #endregion

        #region 动画

        /// <summary>待机动画名</summary>
        public const string ColumnAnimName = "AnimName";
        /// <summary>是否循环播放</summary>
        public const string ColumnIsLoop = "Loop";
        /// <summary>循环重启延迟（秒）</summary>
        public const string ColumnLoopRestartDelay = "LoopRestartDelay";
        /// <summary>移动动画名</summary>
        public const string ColumnMoveAnimName = "MoveAnimName";
        /// <summary>移动速度</summary>
        public const string ColumnSpeed = "Speed";

        #endregion

        #region 交互与碰撞

        /// <summary>是否可交互</summary>
        public const string ColumnIsInteractable = "Interactable";
        /// <summary>交互类型</summary>
        public const string ColumnInteractType = "InteractType";
        /// <summary>是否启用碰撞</summary>
        public const string ColumnHasCollision = "CollisionEnabled";
        /// <summary>朝向</summary>
        public const string ColumnFacingDirection = "FacingDirection";
        /// <summary>是否启用投影</summary>
        public const string ColumnShadowEnabled = "ShadowEnabled";

        #endregion

        #region 气泡与收集

        /// <summary>对话气泡文案</summary>
        public const string ColumnDialogBubble = "DialogBubble";
        /// <summary>气泡ID</summary>
        public const string ColumnBubbleId = "BubbleId";
        /// <summary>是否可收集</summary>
        public const string ColumnIsCollectible = "IsCollectible";
        /// <summary>重生时间（秒）</summary>
        public const string ColumnRespawnTime = "RespawnTime";

        #endregion

        #region 交通与移动

        /// <summary>交通类别</summary>
        public const string ColumnTrafficCategory = "TrafficCategory";
        /// <summary>是否启用 Y 轴排序</summary>
        public const string ColumnYSortEnabled = "YSortEnabled";
        /// <summary>Y 轴排序偏移</summary>
        public const string ColumnYSortOffset = "YSortOffset";
        /// <summary>是否可移动</summary>
        public const string ColumnIsMovable = "IsMovable";
        /// <summary>出图是否隐藏</summary>
        public const string ColumnHideWhenOffMap = "HideWhenOffMap";

        #endregion

        #region 传送

        /// <summary>是否传送门</summary>
        public const string ColumnIsTeleport = "IsTeleport";
        /// <summary>目标场景</summary>
        public const string ColumnTeleportTargetScene = "TeleportTargetScene";
        /// <summary>目标格子 X</summary>
        public const string ColumnTeleportTargetX = "TeleportTargetX";
        /// <summary>目标格子 Y</summary>
        public const string ColumnTeleportTargetY = "TeleportTargetY";

        #endregion
    }
}
