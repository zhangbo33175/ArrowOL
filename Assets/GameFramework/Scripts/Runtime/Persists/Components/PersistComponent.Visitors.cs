/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PersistComponent.Fields.cs
 * author:    云毅
 * created:
 * descrip:   持久化组件 - 字段与属性定义
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 持久化存储组件 - 字段与属性模块
    /// </summary>
    public sealed partial class PersistComponent : GameComponent
    {
        //=========================================================================
        #region 私有字段
        //=========================================================================

        /// <summary>
        /// WebGL 专用文件片段存储管理器
        /// </summary>
        private FileFragmentForWebGLManager m_FileFragmentForWebGLManager = null;

        /// <summary>
        /// 常规平台文件片段存储管理器
        /// </summary>
        private FileFragmentManager m_FileFragmentManager = null;

        /// <summary>
        /// PlayerPrefs 存储管理器
        /// </summary>
        private PlayerPrefsManager m_PlayerPrefsManager = null;

        #endregion

        //=========================================================================
        #region 公开属性
        //=========================================================================

        /// <summary>
        /// 获取 WebGL 专用文件片段存储管理器
        /// </summary>
        public FileFragmentForWebGLManager FileFragmentForWebGLManager
        {
            get { return m_FileFragmentForWebGLManager; }
        }

        /// <summary>
        /// 获取常规平台文件片段存储管理器
        /// </summary>
        public FileFragmentManager FileFragmentManager
        {
            get { return m_FileFragmentManager; }
        }

        /// <summary>
        /// 获取 PlayerPrefs 存储管理器
        /// </summary>
        public PlayerPrefsManager PlayerPrefsManager
        {
            get { return m_PlayerPrefsManager; }
        }

        #endregion

        //=========================================================================
        #region 公共方法
        //=========================================================================

        /// <summary>
        /// 获取指定分类下的存储条目数量
        /// </summary>
        /// <param name="wayType">存储方式</param>
        /// <param name="classifyName">分类名称</param>
        /// <returns>条目数量</returns>
        public int Count(PersistWayType wayType, string classifyName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.Count(classifyName);
#else
                return m_FileFragmentManager.Count(classifyName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.Count(classifyName);
            }

            return 0;
        }

        #endregion
    }
}