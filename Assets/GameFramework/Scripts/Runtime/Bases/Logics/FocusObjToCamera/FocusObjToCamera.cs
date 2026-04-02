
using UnityEngine;

namespace Honor.Runtime
{
    public class FocusObjToCamera : MonoBehaviour
    {
        [Header("设置物体要朝向的相机，默认使用第一个场景相机")]
        public Camera FaceCamera;
        void Start()
        {
            if (FaceCamera == null)
            {
                FaceCamera = GameMainRoot.Scene.SceneCameras[0];
            }
        }

        void FixedUpdate()
        {
            if (FaceCamera != null)
            {
                transform.LookAt(transform.position + FaceCamera.transform.rotation * Vector3.forward, FaceCamera.transform.rotation * Vector3.up);
            }
        }

    }
}

