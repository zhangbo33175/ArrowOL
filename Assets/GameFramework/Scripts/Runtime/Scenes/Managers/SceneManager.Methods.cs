using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class SceneManager
    {
        /// <summary>
        /// 从资源名称列表中移除资源名称
        /// </summary>
        /// <param name="list">资源名称列表</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset资源名称</param>
        private bool RemoveAssetNamesFromList(List<List<string>> list, string abPath, string assetName)
        {
            if (list == null)
            {
                throw new GameException("Scene list 无效。");
            }

            if (list == m_UnloadingSceneAssetNames)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    throw new GameException("Scene assetName 无效。");
                }

                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][0]))
                    {
                        if (list[index][0] == assetName)
                        {
                            list.RemoveAt(index);
                            return true;
                        }
                    }
                }
            }
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
        /// 指定资源是否存在于指定资源名称列表中
        /// </summary>
        /// <param name="list">资源名称列表</param>
        /// <param name="assetName">asset资源名称</param>
        /// <returns></returns>
        private bool IsAssetNameExistInList(List<List<string>> list, string assetName)
        {
            if (list == null)
            {
                throw new GameException("Scene list 无效。");
            }

            if (list == m_UnloadingSceneAssetNames)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    throw new GameException("Scene assetName 无效。");
                }

                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][0]))
                    {
                        if (list[index][0] == assetName)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    throw new GameException("Scene assetName 无效。");
                }

                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list[index][1]))
                    {
                        if (list[index][1] == assetName)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }

}


