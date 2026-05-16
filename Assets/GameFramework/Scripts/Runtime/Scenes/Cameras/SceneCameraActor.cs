/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneCameraActor.cs
 * author:  云毅
 * created:
 * descrip:   场景相机控制器 - 统一管理主相机位移/旋转/缩放/动画/触摸控制
 ***************************************************************/

namespace Honor.Runtime
{
    using DG.Tweening;
#if EASY_TOUCH_ENABLE
    using HedgehogTeam.EasyTouch;
#endif
    using System;
    using UnityEngine;

    /// <summary>
    /// 场景相机控制器
    /// 功能：统一管理主相机的位置、旋转、缩放、过渡动画、触摸控制
    /// 支持2D/3D相机，自带动画打断、触摸屏蔽机制
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed partial class SceneCameraActor : MonoBehaviour
    {
        //=========================================================================
        #region 成员变量
        //=========================================================================

        /// <summary>
        /// 场景主相机
        /// </summary>
        private Camera m_Camera;
        public Camera Camera => m_Camera;

        /// <summary>
        /// 触摸管理组件
        /// </summary>
        private TouchComponent m_TouchComponent;

#if EASY_TOUCH_ENABLE
        /// <summary>
        /// 2D手势控制器
        /// </summary>
        private Gestures2D m_Gestures2D;

        /// <summary>
        /// 3D手势控制器
        /// </summary>
        private Gestures3D m_Gestures3D;
#endif

        /// <summary>
        /// 相机初始目标位置
        /// </summary>
        private Vector3 m_OriginalTargetPosition;

        /// <summary>
        /// 相机初始目标旋转
        /// </summary>
        private Quaternion m_OriginalTargetRotation;

        /// <summary>
        /// 相机初始目标尺寸（正交大小/视野）
        /// </summary>
        private float m_OriginalTargetSizeOrField;

        /// <summary>
        /// 相机恢复初始状态的动画时长
        /// </summary>
        private float m_OriginalTargetDuration;

        /// <summary>
        /// 禁止触摸引用计数
        /// >0：禁止触摸   =0：恢复触摸
        /// </summary>
        private int m_ForbidenEasyTouchRef;

        /// <summary>
        /// 禁止触摸前的原始触摸状态备份
        /// </summary>
        private bool m_HistoryEasyTouchEnabledBak;

        /// <summary>
        /// 移动动画是否可被手势打断
        /// </summary>
        private bool m_MoveCanInterruptByGestures;

        /// <summary>
        /// 缩放动画是否可被手势打断
        /// </summary>
        private bool m_ScaleCanInterruptByGestures;

        #endregion

        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 初始化：获取相机与触摸组件
        /// </summary>
        private void Awake()
        {
            m_TouchComponent = GameComponentsGroup.GetComponent<TouchComponent>();
            if (m_TouchComponent == null)
            {
                Log.Fatal("Touch component 无效。");
                return;
            }

            m_Camera = GetComponent<Camera>();
            if (m_Camera == null)
            {
                Log.Fatal("Camera 无效。");
                return;
            }
        }

        /// <summary>
        /// 启动：绑定手势控制器
        /// </summary>
        private void Start()
        {
#if EASY_TOUCH_ENABLE
            m_Gestures2D = m_TouchComponent.Gestures2D;
            m_Gestures3D = m_TouchComponent.Gestures3D;
#endif
        }

        private void OnEnable() { }
        private void OnDisable() { }

        #endregion

        //=========================================================================
        #region 初始化相机
        //=========================================================================

        /// <summary>
        /// 初始化相机（无动画）
        /// </summary>
        /// <param name="originalPosition">初始位置</param>
        /// <param name="originalRotation">初始旋转</param>
        /// <param name="originalSizeOrField">初始大小/视野</param>
        public void Init(Vector3 originalPosition, Quaternion originalRotation, float originalSizeOrField)
        {
            m_Camera.transform.position = originalPosition;
            m_Camera.transform.rotation = originalRotation;
            m_Camera.orthographicSize = originalSizeOrField;
            m_Camera.fieldOfView = originalSizeOrField;

            m_OriginalTargetPosition = originalPosition;
            m_OriginalTargetRotation = originalRotation;
            m_OriginalTargetSizeOrField = originalSizeOrField;
            m_OriginalTargetDuration = 0;
        }

        /// <summary>
        /// 初始化相机（带动画过渡）
        /// </summary>
        public void InitWithAnimation(
            Vector3 originalPosition,
            Quaternion originalRotation,
            float originalSizeOrField,
            Vector3 targetPosition,
            Quaternion targetRotation,
            float targetSizeOrField,
            float targetDuration = 2f,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            // 设置相机起始状态
            m_Camera.transform.position = originalPosition;
            m_Camera.transform.rotation = originalRotation;
            m_Camera.orthographicSize = originalSizeOrField;
            m_Camera.fieldOfView = originalSizeOrField;

            // 记录目标状态
            m_OriginalTargetPosition = targetPosition;
            m_OriginalTargetRotation = targetRotation;
            m_OriginalTargetSizeOrField = targetSizeOrField;
            m_OriginalTargetDuration = targetDuration;

            // 播放入场动画
            PlayAnimationToTarget(
                m_OriginalTargetDuration,
                m_OriginalTargetPosition,
                m_OriginalTargetRotation,
                m_OriginalTargetSizeOrField,
                canInterruptByGestures,
                overCallback);
        }

        #endregion

        //=========================================================================
        #region 相机动画
        //=========================================================================

        /// <summary>
        /// 播放相机综合动画（位移+旋转+缩放）
        /// </summary>
        public void PlayAnimationToTarget(
            float duration,
            Vector3 targetPosition,
            Quaternion targetRotation,
            float targetSizeOrField,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            MoveTo(duration, targetPosition, canInterruptByGestures);
            RotateTo(duration, targetRotation);
            ScaleTo(duration, targetSizeOrField, canInterruptByGestures);

            // 动画结束回调
            DOTween.Kill(GameDOTweenTypes.CameraAnimation);
            DOTween.Sequence()
                .AppendInterval(duration)
                .OnComplete(() => overCallback?.Invoke())
                .id = GameDOTweenTypes.CameraAnimation;
        }

        /// <summary>
        /// 相机移动动画
        /// </summary>
        public void MoveTo(
            float duration,
            Vector3 targetPosition,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            // 如果正在播放不可中断动画，先恢复触摸
            if (DOTween.IsTweening(GameDOTweenTypes.CameraMoveAnimation) && !m_MoveCanInterruptByGestures)
                SubForbidenEasyTouchRef();

            m_MoveCanInterruptByGestures = canInterruptByGestures;

            // 不可中断则禁用触摸
            if (!m_MoveCanInterruptByGestures)
                AddFobidenEasyTouchRef();

#if EASY_TOUCH_ENABLE
            m_Gestures2D.ResetSwipe();
            m_Gestures3D.ResetSwipe();
#endif

            // 执行移动动画
            DOTween.Kill(GameDOTweenTypes.CameraMoveAnimation);
            DOTween.Sequence()
                .Append(DOTween.To(
                    () => m_Camera.transform.position,
                    pos => m_Camera.transform.position = pos,
                    targetPosition,
                    duration))
                .OnComplete(() =>
                {
                    if (!m_MoveCanInterruptByGestures)
                        SubForbidenEasyTouchRef();
                    overCallback?.Invoke();
                })
                .id = GameDOTweenTypes.CameraMoveAnimation;
        }

        /// <summary>
        /// 相机旋转动画
        /// </summary>
        public void RotateTo(float duration, Quaternion targetRotation, Action overCallback = null)
        {
            DOTween.Kill(GameDOTweenTypes.CameraRotateAnimation);
            DOTween.Sequence()
                .Append(DOTween.To(
                    () => m_Camera.transform.rotation.eulerAngles,
                    rotate => m_Camera.transform.rotation = Quaternion.Euler(rotate),
                    targetRotation.eulerAngles,
                    duration))
                .OnComplete(() => overCallback?.Invoke())
                .id = GameDOTweenTypes.CameraRotateAnimation;
        }

        /// <summary>
        /// 相机缩放动画（2D正交大小 / 3D视野）
        /// </summary>
        public void ScaleTo(
            float duration,
            float targetOrthographicSizeOrField,
            bool canInterruptByGestures = false,
            Action overCallback = null)
        {
            if (DOTween.IsTweening(GameDOTweenTypes.CameraScaleAnimation) && !m_ScaleCanInterruptByGestures)
                SubForbidenEasyTouchRef();

            m_ScaleCanInterruptByGestures = canInterruptByGestures;

            if (!m_ScaleCanInterruptByGestures)
                AddFobidenEasyTouchRef();

#if EASY_TOUCH_ENABLE
            m_Gestures2D.ResetPinch();
            m_Gestures3D.ResetPinch();
#endif

            DOTween.Kill(GameDOTweenTypes.CameraScaleAnimation);

            // 2D相机
            if (m_Camera.orthographic)
            {
                DOTween.Sequence()
                    .Append(DOTween.To(
                        () => m_Camera.orthographicSize,
                        size => m_Camera.orthographicSize = size,
                        targetOrthographicSizeOrField,
                        duration))
                    .OnComplete(() =>
                    {
                        if (!m_ScaleCanInterruptByGestures)
                            SubForbidenEasyTouchRef();
                        overCallback?.Invoke();
                    })
                    .id = GameDOTweenTypes.CameraScaleAnimation;
            }
            // 3D相机
            else
            {
                DOTween.Sequence()
                    .Append(DOTween.To(
                        () => m_Camera.fieldOfView,
                        fov => m_Camera.fieldOfView = fov,
                        targetOrthographicSizeOrField,
                        duration))
                    .OnComplete(() =>
                    {
                        if (!m_ScaleCanInterruptByGestures)
                            SubForbidenEasyTouchRef();
                        overCallback?.Invoke();
                    })
                    .id = GameDOTweenTypes.CameraScaleAnimation;
            }
        }

        #endregion

        //=========================================================================
        #region 触摸控制
        //=========================================================================

        /// <summary>
        /// 增加禁止触摸引用（禁用触摸）
        /// </summary>
        private void AddFobidenEasyTouchRef()
        {
#if EASY_TOUCH_ENABLE
            if (m_ForbidenEasyTouchRef == 0)
                m_HistoryEasyTouchEnabledBak = EasyTouch.GetEnabled();
            
            m_ForbidenEasyTouchRef++;
            if (m_ForbidenEasyTouchRef > 0)
                EasyTouch.SetEnabled(false);
#endif
        }

        /// <summary>
        /// 减少禁止触摸引用（恢复触摸）
        /// </summary>
        private void SubForbidenEasyTouchRef()
        {
#if EASY_TOUCH_ENABLE
            m_ForbidenEasyTouchRef--;
            if (m_ForbidenEasyTouchRef == 0)
                EasyTouch.SetEnabled(m_HistoryEasyTouchEnabledBak);
            else if (m_ForbidenEasyTouchRef < 0)
                Log.Error("SceneCameraActor 禁止触摸引用计数异常 < 0");
#endif
        }

        #endregion
    }
}