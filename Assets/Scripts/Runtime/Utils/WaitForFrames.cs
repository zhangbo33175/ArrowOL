using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 协程等待指定帧数
    /// </summary>
    public class WaitForFrames : CustomYieldInstruction
    {
        private int _targetFrameCount;

        private int _numberOfFrames;

        public WaitForFrames(int numberOfFrames)
        {
            _numberOfFrames = numberOfFrames;
            _targetFrameCount = Time.frameCount + numberOfFrames;
        }

        public override bool keepWaiting => Time.frameCount < _targetFrameCount;

        public override void Reset() => _targetFrameCount = Time.frameCount + _numberOfFrames;
    }
}