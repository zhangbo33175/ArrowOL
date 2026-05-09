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
    /// 提供动态换装、Slot 图片替换等功能
    /// </summary>
    public static partial class GameExtensionForUnity
    {
#if SPINE_ENABLE
        /// <summary>
        /// 替换 Spine 动画中指定 Slot 的皮肤图片
        /// </summary>
        /// <param name="skeletonAnimation">SkeletonAnimation 组件</param>
        /// <param name="slotName">插槽名称</param>
        /// <param name="findSpriteName">要替换的附件名称</param>
        /// <param name="sprite">新图片</param>
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
        /// <param name="skeletonGraphic">SkeletonGraphic 组件</param>
        /// <param name="slotName">插槽名称</param>
        /// <param name="findSpriteName">要替换的附件名称</param>
        /// <param name="sprite">新图片</param>
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
        /// 内部实现：替换 Spine Slot 皮肤图片
        /// </summary>
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
#endif
    }
}