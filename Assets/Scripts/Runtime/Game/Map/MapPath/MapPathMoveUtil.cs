/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapPathMoveUtil.cs
 * author:    云毅
 * created:   2026
 * descrip:   路径移动工具
 *            基于 DOTween Sequence 的分段路径移动，支持跨区拦截与续走
 ***************************************************************/

using UnityEngine;
using DG.Tweening;
using XLua;
using System.Collections.Generic;

namespace GameLib
{
    /// <summary>
    /// 段级拦截谓词（Lua 注入）
    /// 传入目标点，返回是否拦截该段
    /// </summary>
    [CSharpCallLua]
    public delegate bool SectionBlockDelegate(Vector3 toPoint);

    /// <summary>
    /// 路径移动工具（静态全量实现，含跨区 gating）
    /// 构建 DOTween Sequence 驱动 Transform 沿路径点匀速移动
    /// </summary>
    [LuaCallCSharp]
    public static class MapPathMoveUtil
    {
        #region 常量

        /// <summary>基础移动速度（像素/秒）</summary>
        public const float BASE_MOVE_PX_PER_SEC = 80f;

        /// <summary>路径调试日志开关</summary>
        public static bool MAP_PATH_DEBUG_PRINT = true;

        #endregion

        #region 内部状态

        /// <summary>路径移动内部状态</summary>
        private class PathState
        {
            /// <summary>目标 Transform</summary>
            public Transform trans;
            /// <summary>路径点数组</summary>
            public Vector3[] points;
            /// <summary>单位速度（像素/秒）</summary>
            public float unitSpeed;
            /// <summary>是否循环巡逻</summary>
            public bool isLoop;
            /// <summary>标签（日志标识）</summary>
            public string label;
            /// <summary>下一段索引</summary>
            public int nextIndex;
            /// <summary>段级拦截谓词</summary>
            public SectionBlockDelegate shouldBlock;
        }

        /// <summary>Sequence → 状态映射表</summary>
        private static Dictionary<Sequence, PathState> _states = new Dictionary<Sequence, PathState>();

        #endregion

        #region 公开接口

        /// <summary>
        /// 构建路径移动 Sequence（含 gating：被拦段不 append，停在边界）
        /// </summary>
        /// <param name="trans">目标 Transform</param>
        /// <param name="pathPoints">路径点数组</param>
        /// <param name="speed">速度倍率（0 或负数视为 1）</param>
        /// <param name="isLoop">是否循环巡逻</param>
        /// <param name="label">日志标签</param>
        /// <param name="shouldBlock">段级拦截谓词（可选）</param>
        /// <returns>DOTween Sequence（可用于后续 ResumePath / Kill）</returns>
        public static Sequence BuildLocalMoveSeq(Transform trans, Vector3[] pathPoints,
            float speed, bool isLoop, string label, SectionBlockDelegate shouldBlock = null)
        {
            if (trans == null || pathPoints == null || pathPoints.Length < 1) return null;

            float speedMul = speed > 0 ? speed : 1f;
            float unitSpeed = BASE_MOVE_PX_PER_SEC * speedMul;

            var state = new PathState
            {
                trans = trans,
                points = pathPoints,
                unitSpeed = unitSpeed,
                isLoop = isLoop,
                label = label,
                nextIndex = 0,
                shouldBlock = shouldBlock
            };

            Sequence seq = DOTween.Sequence();
            _states[seq] = state;
            AppendReachable(seq, state);
            return seq;
        }

        /// <summary>
        /// 续走（解锁后重新评估追加可达段）
        /// </summary>
        /// <param name="seq">目标 Sequence（BuildLocalMoveSeq 返回值）</param>
        public static void ResumePath(Sequence seq)
        {
            if (seq == null || !seq.IsActive()) return;
            if (_states.TryGetValue(seq, out PathState state))
            {
                AppendReachable(seq, state);
                seq.Play();
            }
        }

        /// <summary>
        /// 销毁 Sequence 并清理状态
        /// </summary>
        /// <param name="seq">目标 Sequence</param>
        public static void Kill(Sequence seq)
        {
            if (seq == null) return;
            if (seq.IsActive()) seq.Kill();
            _states.Remove(seq);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 追加可达段（从 nextIndex 起，遇拦即停）
        /// 循环模式：到达终点后重置 nextIndex 到 0
        /// </summary>
        private static void AppendReachable(Sequence seq, PathState state)
        {
            Vector3 from = state.trans.localPosition;
            while (state.nextIndex < state.points.Length)
            {
                Vector3 to = state.points[state.nextIndex];

                // 段级拦截：被拦则停在当前位置，等待解锁后 ResumePath
                if (state.shouldBlock != null && state.shouldBlock(to))
                {
                    Debug.Log($"[跨区] 分区拦截，角色停在边界段{state.nextIndex + 1}");
                    break;
                }

                float dist = Vector3.Distance(from, to);
                float dur = dist / state.unitSpeed;
                Tweener tw = state.trans.DOLocalMove(to, dur).SetEase(Ease.Linear);

                if (MAP_PATH_DEBUG_PRINT)
                {
                    int seg = state.nextIndex + 1;
                    tw.OnComplete(() => Debug.Log(
                        $"[路径调试] {state.label} 到达段{seg}/{state.points.Length} " +
                        $"实际({state.trans.localPosition.x:F0},{state.trans.localPosition.y:F0})"));
                }

                seq.Append(tw);
                from = to;
                state.nextIndex++;
            }

            // 循环模式：到达终点后重置索引
            if (state.nextIndex >= state.points.Length && state.isLoop)
            {
                state.nextIndex = 0;
            }
        }

        #endregion
    }
}
