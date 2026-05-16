/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameExtensionForUnity.Spine.cs
 * author:    云毅
 * created:   2025
 * descrip:   Spine动画扩展方法 - 动态换装、Slot皮肤图片替换
 ***************************************************************/
#if SPINE_ENABLE
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
#endif
using UnityEngine;



namespace Honor.Runtime
{
    /// <summary>
    /// Spine 动画扩展方法
    /// <para>提供动态换装、Slot 图片替换等通用功能</para>
    /// <para>依赖 SPINE_ENABLE 宏开关</para>
    /// </summary>
    public static partial class GameExtensionForUnity
    {
#if SPINE_ENABLE
        #region Spine 换装扩展
        //=========================================================================
        // Spine 插槽皮肤替换、动态换装功能
        //=========================================================================
        /// <summary>
        /// 替换 Spine 动画中指定 Slot 的皮肤图片
        /// </summary>
        /// <param name="skeletonAnimation">目标 SkeletonAnimation 组件</param>
        /// <param name="slotName">插槽名称</param>
        /// <param name="findSpriteName">需要替换的附件名称</param>
        /// <param name="sprite">新的图片资源</param>
        /// <param name="skinLayerName">皮肤层名称</param>
        public static void ChangeSlotSkinSprite(this SkeletonAnimation skeletonAnimation, string slotName,
            string findSpriteName, Sprite sprite, string skinLayerName)
        {
            if (skeletonAnimation == null) return;

            SpinChangeSlotSkinSprite(
                skeletonAnimation.Skeleton,
                skeletonAnimation.skeletonDataAsset,
                skeletonAnimation.AnimationState,
                slotName,
                findSpriteName,
                sprite,
                skinLayerName
            );
        }

        /// <summary>
        /// 替换 UI 下 Spine 动画中指定 Slot 的皮肤图片
        /// </summary>
        /// <param name="skeletonGraphic">目标 SkeletonGraphic 组件</param>
        /// <param name="slotName">插槽名称</param>
        /// <param name="findSpriteName">需要替换的附件名称</param>
        /// <param name="sprite">新的图片资源</param>
        /// <param name="skinLayerName">皮肤层名称</param>
        public static void ChangeSlotSkinSprite(this SkeletonGraphic skeletonGraphic, string slotName,
            string findSpriteName, Sprite sprite, string skinLayerName)
        {
            if (skeletonGraphic == null) return;

            skeletonGraphic.allowMultipleCanvasRenderers = true;

            SpinChangeSlotSkinSprite(
                skeletonGraphic.Skeleton,
                skeletonGraphic.skeletonDataAsset,
                skeletonGraphic.AnimationState,
                slotName,
                findSpriteName,
                sprite,
                skinLayerName
            );
        }

        /// <summary>
        /// 内部通用实现：替换 Spine Slot 皮肤图片
        /// </summary>
        /// <param name="skeleton">Spine Skeleton 对象</param>
        /// <param name="skeletonDataAsset">Spine 数据资源</param>
        /// <param name="animationState">动画状态机</param>
        /// <param name="slotName">插槽名称</param>
        /// <param name="findSpriteName">需要替换的附件名称</param>
        /// <param name="sprite">新的图片资源</param>
        /// <param name="skinLayerName">皮肤层名称</param>
        private static void SpinChangeSlotSkinSprite(
            Skeleton skeleton,
            SkeletonDataAsset skeletonDataAsset,
            Spine.AnimationState animationState,
            string slotName,
            string findSpriteName,
            Sprite sprite,
            string skinLayerName
        )
        {
            if (skeleton == null || skeletonDataAsset == null || animationState == null)
                return;

            if (string.IsNullOrEmpty(slotName) || string.IsNullOrEmpty(findSpriteName) || sprite == null)
                return;

            SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
            Skin skin = skeletonData.FindSkin(skinLayerName);
            SlotData slotData = skeletonData.FindSlot(slotName);

            if (skin == null || slotData == null)
                return;

            Attachment templateAttachment = skin.GetAttachment(slotData.Index, findSpriteName);
            if (templateAttachment == null)
                return;

            Material sourceMaterial = templateAttachment.GetMaterial();
            Attachment clonedAttachment = templateAttachment.GetRemappedClone(sprite, sourceMaterial);

            skin.SetAttachment(slotData.Index, findSpriteName, clonedAttachment);

            skeleton.SetSkin(skin);
            skeleton.SetSlotsToSetupPose();
            animationState.Apply(skeleton);
        }
        #endregion
#endif
    }
}