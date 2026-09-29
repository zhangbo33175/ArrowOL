/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   Honor框架编辑器扩展基类，所有自定义Inspector的父类
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    #region Honor 框架编辑器扩展基类
    /// <summary>
    /// 编辑器扩展基类（Honor 框架所有自定义 Inspector 的父类）
    /// 作用：提供编译监听、预制体判断、Inspector 刷新等通用编辑器能力
    /// 所有组件的 Inspector 都应继承此类
    /// </summary>
    public class HonorComponentInspector : UnityEditor.Editor
    {
        #region 私有字段
        /// <summary>
        /// 缓存上一帧的编译状态，用于检测编译开始/完成的跳变沿
        /// </summary>
        private bool m_PreviousCompiling = false;
        #endregion

        #region 重写方法
        /// <summary>
        /// 重写 Inspector 绘制逻辑
        /// 监听 Unity 编译状态，驱动编译开始/完成事件
        /// </summary>
        public override void OnInspectorGUI()
        {
            SyncCompileState();

            // 持续刷新面板
            Repaint();
        }

        /// <summary>
        /// 对比当前编译状态与上一帧缓存，仅在状态发生跳变时触发对应回调
        /// </summary>
        private void SyncCompileState()
        {
            bool nowCompiling = EditorApplication.isCompiling;
            if (m_PreviousCompiling == nowCompiling)
            {
                return;
            }

            m_PreviousCompiling = nowCompiling;
            if (nowCompiling)
            {
                OnCompileStart();
            }
            else
            {
                OnCompileComplete();
            }
        }
        #endregion

        #region 保护虚方法
        /// <summary>
        /// 编译开始时触发（虚方法，子类可重写）
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成时触发（虚方法，子类可重写）
        /// </summary>
        protected virtual void OnCompileComplete()
        {
        }

        /// <summary>
        /// 判断物体是否是场景中的预制体（非预制体源文件）
        /// </summary>
        /// <param name="targetObject">目标物体</param>
        /// <returns>true：场景预制体实例 false：预制体源文件/空对象</returns>
        protected bool IsPrefabInHierarchy(UnityEngine.Object targetObject)
        {
            if (targetObject == null)
            {
                return false;
            }

            // 不是预制体源文件 → 就是场景实例
            return PrefabUtility.GetPrefabAssetType(targetObject) != PrefabAssetType.Regular;
        }
        #endregion
    }
    #endregion
}
