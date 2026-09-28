/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  DefLayer.cs
 * author:  云毅
 * created:
 * descrip:   游戏层级常量定义 - 统一管理所有业务 Layer 名称
 * 优化记录: 由旧版 HonorUtils.DefLayer 迁移，补齐每个层的用途说明
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏层级常量定义
    /// 功能：集中声明项目所有 Layer 名称，防止散落的字符串字面量
    /// 使用方式：gameObject.layer = LayerMask.NameToLayer(DefLayer.UI3D);
    /// </summary>
    public static class DefLayer
    {
        /// <summary>默认层</summary>
        public const string Default = "Default";

        /// <summary>透明特效层</summary>
        public const string TransparentFX = "TransparentFX";

        /// <summary>忽略射线层</summary>
        public const string IgnoreRaycast = "Ignore Raycast";

        /// <summary>水面层（用于水面、镜面）</summary>
        public const string Water = "Water";

        /// <summary>UGUI 界面层</summary>
        public const string UGUI = "UI";

        /// <summary>NGUI 层（历史保留，可占用）</summary>
        public const string NGUI = "NGUI";

        /// <summary>临时层（切换场景时旧场景显示在前）</summary>
        public const string Temp = "Temp";

        /// <summary>场景层</summary>
        public const string Scene = "Scene";

        /// <summary>战斗光照层</summary>
        public const string Light = "Light";

        /// <summary>场景特效阴影层</summary>
        public const string Shadow = "Shadow";

        /// <summary>游走地图障碍物层</summary>
        public const string MapObstacle = "MapObstacle";

        /// <summary>阴影投射层（按字符串调用）</summary>
        public const string ShadowCaster = "ShadowCaster";

        /// <summary>游走地图节点层</summary>
        public const string MapNode = "MapNode";

        /// <summary>游走地图地面层</summary>
        public const string MapGround = "MapGround";

        /// <summary>屏幕后处理层</summary>
        public const string PostEffect = "PostEffect";

        /// <summary>扭曲处理层</summary>
        public const string Distort = "Distort";

        /// <summary>隐藏层（防止摄像机近裁剪物件穿帮、技能隐藏角色）</summary>
        public const string Hide = "Hide";

        /// <summary>剧情 UI 层</summary>
        public const string StoryUI = "StoryUI";

        /// <summary>渲染纹理层（可占用）</summary>
        public const string RenderTex = "RenderTex";

        /// <summary>角色层（角色展示界面、战斗角色）</summary>
        public const string Role = "Role";

        /// <summary>背景层（可占用）</summary>
        public const string BackGround = "BackGround";

        /// <summary>UI 3D 层</summary>
        public const string UI3D = "UI3D";

        /// <summary>顶层 UI 层</summary>
        public const string TOPUI = "TopUI";

        /// <summary>弹窗模糊层</summary>
        public const string FocusUI = "FocusUI";

        /// <summary>角色面部层（多用于战斗外）</summary>
        public const string CharacterFace = "CharacterFace";

        /// <summary>角色身体层</summary>
        public const string CharacterBody = "CharacterBody";

        /// <summary>角色腿部层</summary>
        public const string CharacterLeg = "CharacterLeg";

        /// <summary>大地图事件节点层</summary>
        public const string BigMapEvent = "BigMapEvent";
    }
}
