using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 物体始终朝向相机（广告牌效果）
    /// 常用于：角色血条、头顶名称、3D UI、特效等需要始终面向相机的物体
    /// </summary>
    public class FocusObjToCamera : MonoBehaviour
    {
        [Header("指定朝向的目标相机，不指定则自动使用场景默认相机")] public Camera FaceCamera;

        /// <summary>
        /// 初始化：自动获取默认场景相机
        /// </summary>
        private void Start()
        {
            // 若未指定相机，则自动使用场景管理的第一个相机
            if (FaceCamera == null)
            {
                if (GameMainRoot.Scene != null && GameMainRoot.Scene.SceneCameras?.Count > 0)
                {
                    FaceCamera = GameMainRoot.Scene.SceneCameras[0];
                }
            }
        }

        /// <summary>
        /// 每帧更新物体旋转，确保始终朝向相机
        /// 使用 LateUpdate 保证在相机移动之后执行，防止画面抖动
        /// </summary>
        private void LateUpdate()
        {
            // 相机无效时不执行
            if (FaceCamera == null)
                return;

            // 让物体朝向相机（标准广告牌旋转算法）
            transform.LookAt(
                transform.position + FaceCamera.transform.rotation * Vector3.forward,
                FaceCamera.transform.rotation * Vector3.up
            );
        }
    }
}