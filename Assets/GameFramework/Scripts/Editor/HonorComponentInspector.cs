using UnityEditor;

namespace Honor.Editor
{
    /// <summary>
    /// 编辑器扩展基类（Honor 框架所有自定义 Inspector 的父类）
    /// 作用：提供编译监听、预制体判断、Inspector 刷新等通用编辑器能力
    /// 所有组件的 Inspector 都应继承此类
    /// </summary>
    public class HonorComponentInspector : UnityEditor.Editor
    {
        /// <summary>
        /// 标记是否正在编译
        /// </summary>
        private bool m_IsCompiling = false;

        /// <summary>
        /// 重写 Inspector 绘制逻辑
        /// 监听 Unity 编译状态，驱动编译开始/完成事件
        /// </summary>
        public override void OnInspectorGUI()
        {
            // 监听：从编译中 → 编译结束
            if (m_IsCompiling && !EditorApplication.isCompiling)
            {
                m_IsCompiling = false;
                OnCompileComplete();
            }
            // 监听：从未编译 → 开始编译
            else if (!m_IsCompiling && EditorApplication.isCompiling)
            {
                m_IsCompiling = true;
                OnCompileStart();
            }

            // 持续刷新面板
            Repaint();
        }

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
        /// <param name="obj">目标物体</param>
        protected bool IsPrefabInHierarchy(UnityEngine.Object obj)
        {
            if (obj == null)
                return false;

            // 不是预制体源文件 → 就是场景实例
            return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.Regular;
        }
    }
}