/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneManager.Util.cs
 * author:  云毅
 * created:
 * descrip:   场景管理器 - 工具方法分部类（列表查找、移除、状态判断）
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 场景管理器（工具方法分部类）
    /// 提供：场景列表查找、移除、状态判断的通用工具
    /// </summary>
    public sealed partial class SceneManager
    {
        //=========================================================================
        #region 列表工具方法
        //=========================================================================

        /// <summary>
        /// 从场景列表中移除指定的场景记录
        /// 特殊处理：卸载列表只匹配场景名，其他列表匹配 abPath + assetName
        /// </summary>
        /// <param name="list">目标列表</param>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">场景资源名</param>
        /// <returns>是否成功移除</returns>
        private bool RemoveAssetNamesFromList(List<List<string>> list, string abPath, string assetName)
        {
            if (list == null)
            {
                throw new GameException("Scene list 无效。");
            }

            // 如果是【卸载队列】：只按场景名称匹配（只存 assetName）
            if (list == m_UnloadingSceneAssetNames)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    throw new GameException("Scene assetName 无效。");
                }

                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][0]) && list[index][0] == assetName)
                    {
                        list.RemoveAt(index);
                        return true;
                    }
                }
            }
            // 如果是【预加载/加载中/已加载队列】：按 AB路径 + 场景名 匹配
            else
            {
                if (string.IsNullOrEmpty(abPath))
                {
                    throw new GameException("Scene abPath 无效。");
                }

                if (string.IsNullOrEmpty(assetName))
                {
                    throw new GameException("Scene assetName 无效。");
                }

                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][0]) && !string.IsNullOrEmpty(list[index][1]))
                    {
                        if (list[index][0] == abPath && list[index][1] == assetName)
                        {
                            list.RemoveAt(index);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 检查指定场景名称是否存在于列表中
        /// 卸载队列只查第0位，其他队列查第1位（assetName）
        /// </summary>
        /// <param name="list">目标列表</param>
        /// <param name="assetName">场景资源名</param>
        /// <returns>是否存在</returns>
        private bool IsAssetNameExistInList(List<List<string>> list, string assetName)
        {
            if (list == null)
            {
                throw new GameException("Scene list 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            // 卸载队列：存储格式 [assetName]
            if (list == m_UnloadingSceneAssetNames)
            {
                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][0]) && list[index][0] == assetName)
                    {
                        return true;
                    }
                }
            }
            // 其他队列：存储格式 [abPath, assetName]
            else
            {
                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][1]) && list[index][1] == assetName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}