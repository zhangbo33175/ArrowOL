/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  MapObjectTemplateBase.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图物体模板基类 - 所有物件/建筑/人物预制体的基类
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime.Map
{
    /// <summary>
    /// 地图物体模板基类
    /// 所有物件/建筑/人物预制体都挂载此脚本，用于统一管理贴图、动画、交互等
    /// </summary>
    public class MapObjectTemplateBase : MonoBehaviour
    {
        [Header("===== 基础信息 =====")]
        [Tooltip("模板ID")]
        public string templateId;

        [Tooltip("模板名称")]
        public string templateName;

        [Tooltip("物体类型")]
        public MapObjectType objectType = MapObjectType.Prop;

        [Header("===== 渲染设置 =====")]
        [Tooltip("精灵渲染器")]
        public SpriteRenderer spriteRenderer;

        [Tooltip("渲染层级排序")]
        public int sortOrder = 0;

        [Tooltip("是否显示阴影")]
        public bool shadowEnabled = true;

        [Header("===== 动画设置 =====")]
        [Tooltip("动画控制器")]
        public Animator animator;

        [Tooltip("默认动画名称")]
        public string defaultAnimName = "Idle";

        [Tooltip("是否循环播放")]
        public bool loopAnim = true;

        [Tooltip("动画播放速度")]
        public float animSpeed = 1.0f;

        [Header("===== 交互设置 =====")]
        [Tooltip("是否可交互")]
        public bool interactable = true;

        [Tooltip("交互类型")]
        public InteractType interactType = InteractType.None;

        [Tooltip("对话气泡内容")]
        [TextArea]
        public string dialogBubble = "";

        [Header("===== 物理设置 =====")]
        [Tooltip("碰撞体")]
        public BoxCollider2D collider;

        [Tooltip("是否有碰撞")]
        public bool collisionEnabled = true;

        [Header("===== 收集物设置 =====")]
        [Tooltip("是否为收集物")]
        public bool isCollectible = false;

        [Tooltip("收集物重生时间（秒）")]
        public float respawnTime = 0f;

        /// <summary>
        /// 替换贴图
        /// </summary>
        public void SetSprite(Sprite newSprite)
        {
            if (spriteRenderer != null && newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
                Debug.Log($"[MapObjectTemplate] 替换贴图: {templateName} -> {newSprite.name}");
            }
        }

        /// <summary>
        /// 替换动画控制器
        /// </summary>
        public void SetAnimatorController(RuntimeAnimatorController newController)
        {
            if (animator != null && newController != null)
            {
                animator.runtimeAnimatorController = newController;
                Debug.Log($"[MapObjectTemplate] 替换动画: {templateName} -> {newController.name}");
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        public void PlayAnimation(string animName)
        {
            if (animator != null && !string.IsNullOrEmpty(animName))
            {
                animator.Play(animName);
                Debug.Log($"[MapObjectTemplate] 播放动画: {templateName} -> {animName}");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            // 自动获取组件
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (animator == null)
                animator = GetComponent<Animator>();

            if (collider == null)
                collider = GetComponent<BoxCollider2D>();

            // 设置排序层级
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = sortOrder;
            }
        }
    }

    /// <summary>
    /// 地图物体类型
    /// </summary>
    public enum MapObjectType
    {
        /// <summary>建筑</summary>
        Building = 0,
        /// <summary>人物</summary>
        Character = 1,
        /// <summary>物件</summary>
        Prop = 2
    }

    /// <summary>
    /// 交互类型
    /// </summary>
    public enum InteractType
    {
        /// <summary>无</summary>
        None = 0,
        /// <summary>点击</summary>
        Click = 1,
        /// <summary>对话</summary>
        Talk = 2,
        /// <summary>进入</summary>
        Enter = 3,
        /// <summary>收集</summary>
        Collect = 4
    }
}
