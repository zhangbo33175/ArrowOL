using HedgehogTeam.EasyTouch;
using UnityEngine;

namespace GameLib
{
    public static class RTransformExtension
    {
        public static float GetPositionX(this Transform transform)
        {
            return transform.position.x;
        }
        
        public static float GetPositionY(this Transform transform)
        {
            return transform.position.y;
        }
        
        public static float GetPositionZ(this Transform transform)
        {
            return transform.position.z;
        }
        
        public static float GetPositionX(this BaseFinger transform)
        {
            return transform.position.x;
        }
        
        public static float GetPositionY(this BaseFinger transform)
        {
            return transform.position.y;
        }
        
        public static float GetDeltaPositionX(this BaseFinger finger)
        {
            return finger.deltaPosition.x;
        }
        
        public static float GetDeltaPositionY(this BaseFinger finger)
        {
            return finger.deltaPosition.y;
        }
    }
}