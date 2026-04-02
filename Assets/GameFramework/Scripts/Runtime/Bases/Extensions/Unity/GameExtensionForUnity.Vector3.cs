using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Honor.Runtime
{
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 获取屏幕射线检测到的3D对象的transform
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Transform GetRaycastHit3DTransform(this Vector3 origin, Camera camera)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(origin);
            if (Physics.Raycast(ray, out hit))
            {
                return hit.collider.transform;
            }
            return null;
        }

    }


}


