namespace Honor.Runtime
{
    using DG.Tweening;
#if EASY_TOUCH_ENABLE
    using HedgehogTeam.EasyTouch;
#endif
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public sealed partial class SceneCameraActor : MonoBehaviour
    {
        /// <summary>
        /// 场景相机
        /// </summary>
        private Camera m_Camera;
        public Camera Camera
        {
            get
            {
                return m_Camera;
            }
        }

        /// <summary>
        /// Touch组件
        /// </summary>
        private TouchComponent m_TouchComponent;

#if EASY_TOUCH_ENABLE
        /// <summary>
        /// 手势操作器(2D相机)
        /// </summary>
        private Gestures2D m_Gestures2D;

        /// <summary>
        /// 手势操作器(3D相机)
        /// </summary>
        private Gestures3D m_Gestures3D;

#endif
        /// <summary>
        /// 场景相机初始目标位置
        /// </summary>
        private Vector3 m_OriginalTargetPosition;

        /// <summary>
        /// 场景相机初始目标角度
        /// </summary>
        private Quaternion m_OriginalTargetRotation;

        /// <summary>
        /// 场景相机初始目标尺寸(2D/3D)
        /// </summary>
        private float m_OriginalTargetSizeOrField;

        /// <summary>
        /// 场景相机向初始目标递进的持续时间
        /// </summary>
        private float m_OriginalTargetDuration;

        /// <summary>
        /// 禁用触摸引用计数
        /// 等于0时可用，大于0时禁用
        /// </summary>
        private int m_ForbidenEasyTouchRef;

        /// <summary>
        /// 历史触摸可用性备份
        /// </summary>
        private bool m_HistoryEasyTouchEnabledBak;

        /// <summary>
        /// 相机移动是否可以被手势中断
        /// </summary>
        private bool m_MoveCanInterruptByGestures;

        /// <summary>
        /// 相机缩放是否可以被手势中断
        /// </summary>
        private bool m_ScaleCanInterruptByGestures;

        void Awake()
        {
            m_TouchComponent = GameComponentsGroup.GetComponent<TouchComponent>();
            if (m_TouchComponent == null)
            {
                Log.Fatal("Touch component 无效。");
                return;
            }

            m_Camera = GetComponent<Camera>();
            if(m_Camera == null)
            {
                Log.Fatal("Camera 无效。");
                return;
            }
        }

        void Start()
        {
#if EASY_TOUCH_ENABLE
            m_Gestures2D = m_TouchComponent.Gestures2D;
            m_Gestures3D = m_TouchComponent.Gestures3D;
#endif
        }

        void OnEnable()
        {


        }

        void OnDisable()
        {

        }

        /// <summary>
        /// 初始化相机行动器
        /// </summary>
        /// <param name="originalPosition">初始场景相机位置</param>
        /// <param name="originalRotation">初始场景相机角度</param>
        /// <param name="originalSizeOrField">初始场景相机的尺寸</param>
        public void Init(Vector3 originalPosition, Quaternion originalRotation, float originalSizeOrField)
        {
            // 初始化场景相机位置
            m_Camera.transform.position = originalPosition;

            // 初始化场景相机角度
            m_Camera.transform.rotation = originalRotation;

            // 初始化2D场景相机尺寸
            m_Camera.orthographicSize = originalSizeOrField;

            // 初始化3D场景相机尺寸
            m_Camera.fieldOfView = originalSizeOrField;

            // 场景相机初始目标位置
            m_OriginalTargetPosition = originalPosition;

            // 场景相机初始目标角度
            m_OriginalTargetRotation = originalRotation;

            // 场景相机初始目标尺寸
            m_OriginalTargetSizeOrField = originalSizeOrField;

            // 场景相机向初始目标递进的持续时间
            m_OriginalTargetDuration = 0;

        }

        /// <summary>
        /// 初始化相机行动器（动画）
        /// </summary>
        /// <param name="originalPosition">初始场景相机位置</param>
        /// <param name="originalRotation">初始场景相机角度</param>
        /// <param name="originalSizeOrField">初始场景相机的尺寸</param>
        /// <param name="targetPosition">目标相机位置</param>
        /// <param name="targetRotation">目标相机角度</param>
        /// <param name="targetSizeOrField">目标相机尺寸</param>
        /// <param name="targetDuration">初始场景相机向目标参数递进的动画持续时间</param>
        /// <param name="canInterruptByGestures">动画是否可以被打断</param>
        /// <param name="overCallback">动画结束回调</param>
        public void InitWithAnimation(Vector3 originalPosition, Quaternion originalRotation, float originalSizeOrField, Vector3 targetPosition, Quaternion targetRotation, float targetSizeOrField, float targetDuration = 2f, bool canInterruptByGestures = false, Action overCallback = null)
        {
            // 初始化场景相机位置
            m_Camera.transform.position = originalPosition;

            // 初始化场景相机角度
            m_Camera.transform.rotation = originalRotation;

            // 初始化2D场景相机尺寸
            m_Camera.orthographicSize = originalSizeOrField;

            // 初始化3D场景相机尺寸
            m_Camera.fieldOfView = originalSizeOrField;

            // 场景相机初始目标位置
            m_OriginalTargetPosition = targetPosition;

            // 场景相机初始目标角度
            m_OriginalTargetRotation = targetRotation;

            // 场景相机初始目标尺寸
            m_OriginalTargetSizeOrField = targetSizeOrField;

            // 场景相机向初始目标递进的持续时间
            m_OriginalTargetDuration = targetDuration;

            // 开始出场动画
            PlayAnimationToTarget(m_OriginalTargetDuration, m_OriginalTargetPosition, m_OriginalTargetRotation, m_OriginalTargetSizeOrField, canInterruptByGestures, overCallback);

        }

        /// <summary>
        /// 播放场景相机向目标递进的动画
        /// </summary>
        /// <param name="duration">动画持续时间</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="targetRotation">目标角度</param>
        /// <param name="targetSizeOrField">目标尺寸</param>
        /// <param name="canInterruptByGestures">动画是否可以被打断</param>
        /// <param name="overCallback">动画结束回调</param>
        public void PlayAnimationToTarget(float duration, Vector3 targetPosition, Quaternion targetRotation, float targetSizeOrField, bool canInterruptByGestures = false, Action overCallback = null)
        {
            MoveTo(duration, targetPosition, canInterruptByGestures);
            RotateTo(duration, targetRotation);
            ScaleTo(duration, targetSizeOrField, canInterruptByGestures);

            DOTween.Kill(GameDOTweenTypes.CameraAnimation);
            DOTween.Sequence().AppendInterval(duration).OnComplete(() =>
            {
                if (overCallback != null)
                {
                    overCallback();
                }

            }).id = GameDOTweenTypes.CameraAnimation;

        }

        /// <summary>
        /// 播放场景相机向目标位置递进的动画
        /// </summary>
        /// <param name="duration">动画持续时间</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="canInterruptByGestures">动画是否可以被手势打断</param>
        /// <param name="overCallback">动画结束回调</param>
        public void MoveTo(float duration, Vector3 targetPosition, bool canInterruptByGestures = false, Action overCallback = null)
        {
            if (DOTween.IsTweening(GameDOTweenTypes.CameraMoveAnimation))
            {
                if (!m_MoveCanInterruptByGestures)
                {
                    SubForbidenEasyTouchRef();
                }
            }

            m_MoveCanInterruptByGestures = canInterruptByGestures;

            if (!m_MoveCanInterruptByGestures)
            {
                AddFobidenEasyTouchRef();
            }
#if EASY_TOUCH_ENABLE
            m_Gestures2D.ResetSwipe();
            m_Gestures3D.ResetSwipe();
#endif

            DOTween.Kill(GameDOTweenTypes.CameraMoveAnimation);
            DOTween.Sequence().Append(DOTween.To(() => m_Camera.transform.position, pos => m_Camera.transform.position = pos, targetPosition, duration)).
            OnComplete(() =>
            {
                if (!m_MoveCanInterruptByGestures)
                {
                    SubForbidenEasyTouchRef();
                }
                if (overCallback != null)
                {
                    overCallback();
                }
            }).id = GameDOTweenTypes.CameraMoveAnimation;
        }

        /// <summary>
        /// 播放场景相机向目标角度递进的动画
        /// </summary>
        /// <param name="duration">动画持续时间</param>
        /// <param name="targetRotation">目标角度</param>
        /// <param name="overCallback">动画结束回调</param>
        public void RotateTo(float duration, Quaternion targetRotation, Action overCallback = null)
        {
            DOTween.Kill(GameDOTweenTypes.CameraRotateAnimation);
            DOTween.Sequence().Append(DOTween.To(() => m_Camera.transform.rotation, rotate => m_Camera.transform.rotation = rotate, targetRotation.eulerAngles, duration)).
            OnComplete(() =>
            {
                if (overCallback != null)
                {
                    overCallback();
                }
            }).id = GameDOTweenTypes.CameraRotateAnimation;
        }

        /// <summary>
        /// 播放场景相机向目标尺寸递进的动画
        /// </summary>
        /// <param name="duration">动画持续时间</param>
        /// <param name="targetOrthographicSizeOrField">目标尺寸</param>
        /// <param name="canInterruptByGestures">动画是否可以被手势打断</param>
        /// <param name="overCallback">动画结束回调</param>
        public void ScaleTo(float duration, float targetOrthographicSizeOrField, bool canInterruptByGestures = false, Action overCallback = null)
        {
            if (DOTween.IsTweening(GameDOTweenTypes.CameraScaleAnimation))
            {
                if (!m_ScaleCanInterruptByGestures)
                {
                    SubForbidenEasyTouchRef();
                }
            }

            m_ScaleCanInterruptByGestures = canInterruptByGestures;

            if (!m_ScaleCanInterruptByGestures)
            {
                AddFobidenEasyTouchRef();
            }
#if EASY_TOUCH_ENABLE
            m_Gestures2D.ResetPinch();
            m_Gestures3D.ResetPinch();
#endif
            DOTween.Kill(GameDOTweenTypes.CameraScaleAnimation);
            if(m_Camera.orthographic)
            {
                DOTween.Sequence().Append(DOTween.To(() => m_Camera.orthographicSize, size => m_Camera.orthographicSize = size, targetOrthographicSizeOrField, duration)).
                OnComplete(() =>
                {
                    if (!m_ScaleCanInterruptByGestures)
                    {
                        SubForbidenEasyTouchRef();
                    }
                    if (overCallback != null)
                    {
                        overCallback();
                    }
                }).id = GameDOTweenTypes.CameraScaleAnimation;
            }
            else
            {
                DOTween.Sequence().Append(DOTween.To(() => m_Camera.fieldOfView, size => m_Camera.fieldOfView = size, targetOrthographicSizeOrField, duration)).
                OnComplete(() =>
                {
                    if (!m_ScaleCanInterruptByGestures)
                    {
                        SubForbidenEasyTouchRef();
                    }
                    if (overCallback != null)
                    {
                        overCallback();
                    }
                }).id = GameDOTweenTypes.CameraScaleAnimation;
            }

        }

        /// <summary>
        /// 增加禁用触摸的引用计数
        /// </summary>
        private void AddFobidenEasyTouchRef()
        {
#if EASY_TOUCH_ENABLE
            if (m_ForbidenEasyTouchRef == 0)
            {
                m_HistoryEasyTouchEnabledBak = EasyTouch.GetEnabled();
            }
            m_ForbidenEasyTouchRef++;
            if (m_ForbidenEasyTouchRef > 0)
            {
                EasyTouch.SetEnabled(false);
            }
#endif
        }

        /// <summary>
        /// 减少禁用触摸的引用计数
        /// </summary>
        private void SubForbidenEasyTouchRef()
        {
#if EASY_TOUCH_ENABLE
            m_ForbidenEasyTouchRef--;
            if (m_ForbidenEasyTouchRef == 0)
            {
                EasyTouch.SetEnabled(m_HistoryEasyTouchEnabledBak);
            }
            else if (m_ForbidenEasyTouchRef < 0)
            {
                Log.Error("SceneCameraActor.SubEasyTouchRef m_ForbidenEasyTouchRef 小于0 无效。");
            }
#endif
        }


    }

}


