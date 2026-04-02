using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class SceneManager
    {
        /// <summary>
        /// 准备预加载Scene资源信息
        /// 格式：<<abPath, assetName>>
        /// </summary>
        private readonly List<List<string>> m_PreLoadSceneAssetNames;
        public List<List<string>> PreLoadSceneAssetNames
        {
            get
            {
                return m_PreLoadSceneAssetNames;
            }
        }

        /// <summary>
        /// 正在加载中的Scene资源信息
        /// 格式：<<abPath, assetName>>
        /// </summary>
        private readonly List<List<string>> m_LoadingSceneAssetNames;
        public List<List<string>> LoadingSceneAssetNames
        {
            get
            {
                return m_LoadingSceneAssetNames;
            }
        }

        /// <summary>
        /// 已经加载的Scene资源信息
        /// 格式：<<abPath, assetName>>
        /// </summary>
        private readonly List<List<string>> m_LoadedSceneAssetNames;
        public List<List<string>> LoadedSceneAssetNames
        {
            get
            {
                return m_LoadedSceneAssetNames;
            }
        }

        /// <summary>
        /// 正在卸载中的Scene资源信息
        /// 格式：<<assetName>>
        /// </summary>
        private readonly List<List<string>> m_UnloadingSceneAssetNames;
        public List<List<string>> UnloadingSceneAssetNames
        {
            get
            {
                return m_UnloadingSceneAssetNames;
            }
        }

        /// <summary>
        /// Asset组件
        /// </summary>
        private AssetComponent m_AssetComponent;

    }
}


