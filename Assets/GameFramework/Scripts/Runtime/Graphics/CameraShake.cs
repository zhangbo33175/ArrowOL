/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  CameraShake.cs
 * author:  云毅
 * created:
 * descrip:   相机震动 - 基于 DOTween 的位置/旋转震动（打击感、场景震撼）
 * 优化记录: 由旧版 HonorGraphics.CameraShake 迁移，统一命名空间，补齐旋转震动与参数说明
 ***************************************************************/

using DG.Tweening;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 相机震动
    /// 功能：使用 DOTween 对相机施加带衰减的位置/旋转震动
    /// 使用：CameraShake.Shake(camera, 0.5f, 0.3f);
    /// </summary>
    public static class CameraShake
    {
        #region 常量

        /// <summary>震动 ID（用于打断上次震动）</summary>
        private const string ShakeId = "HonorCameraShake";

        #endregion

        #region 位置震动

        /// <summary>
        /// 相机位置震动
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="duration">震动时长（秒）</param>
        /// <param name="strength">震动强度（位置振幅）</param>
        /// <param name="vibrato">震动频率（越高越碎）</param>
        /// <param name="randomness">随机度 0~1（越高越不规则）</param>
        /// <param name="fadeOut">是否衰减结束</param>
        public static void ShakePosition(Camera camera, float duration, float strength = 0.3f,
            int vibrato = 30, float randomness = 90f, bool fadeOut = true)
        {
            if (camera == null)
            {
                return;
            }

            // 打断旧震动，避免叠加
            DOTween.Kill(ShakeId);

            Sequence sequence = DOTween.Sequence().SetId(ShakeId);
            sequence.Append(camera.transform.DOShakePosition(duration, strength, vibrato, randomness, fadeOut))
                    .OnComplete(() =>
                    {
                        // 震动结束归位（DOShake 会回到原点，这里仅兜底）
                        camera.transform.localPosition = Vector3.zero;
                    });
        }

        #endregion

        #region 旋转震动

        /// <summary>
        /// 相机旋转震动（带角速度衰减）
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="duration">震动时长（秒）</param>
        /// <param name="strength">震动强度（角度振幅）</param>
        /// <param name="vibrato">震动频率</param>
        public static void ShakeRotation(Camera camera, float duration, float strength = 1f, int vibrato = 30)
        {
            if (camera == null)
            {
                return;
            }

            DOTween.Kill(ShakeId);
            Sequence sequence = DOTween.Sequence().SetId(ShakeId);
            sequence.Append(camera.transform.DOShakeRotation(duration, new Vector3(0f, 0f, strength), vibrato, 90f, true))
                    .OnComplete(() =>
                    {
                        camera.transform.localRotation = Quaternion.identity;
                    });
        }

        #endregion

        #region 综合震动

        /// <summary>
        /// 位置 + 旋转综合震动（推荐使用）
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="duration">时长</param>
        /// <param name="posStrength">位置强度</param>
        /// <param name="rotStrength">旋转强度</param>
        public static void Shake(Camera camera, float duration, float posStrength = 0.3f, float rotStrength = 1f)
        {
            if (camera == null)
            {
                return;
            }

            DOTween.Kill(ShakeId);
            Sequence sequence = DOTween.Sequence().SetId(ShakeId);
            sequence.Append(camera.transform.DOShakePosition(duration, posStrength, 30, 90f, true))
                    .Join(camera.transform.DOShakeRotation(duration, new Vector3(0f, 0f, rotStrength), 30, 90f, true))
                    .OnComplete(() =>
                    {
                        camera.transform.localPosition = Vector3.zero;
                        camera.transform.localRotation = Quaternion.identity;
                    });
        }

        #endregion
    }
}
