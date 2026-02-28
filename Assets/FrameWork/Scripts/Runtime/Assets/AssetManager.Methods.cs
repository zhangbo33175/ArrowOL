using System;
using System.Collections.Generic;
using System.IO;
using GameLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Honor.Runtime
{
    /// <summary>
    /// AssetUtils 
    /// </summary>
    public partial class AssetManager
    {
        /// <summary>
        /// Assets字符串长度
        /// </summary>
        private readonly int s_AssetsStringLength = "Assets".Length;

        /// <summary>
        /// 获取资源Assets开头的路径
        /// EditorMode下使用
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public string GetAssetRelativeFullPath(string typeName, string abPath, string assetName)
        {
            string absoluteFullPath = GetAssetAbsoluteFullPath(typeName, abPath, assetName);
            string relativeFullPath = absoluteFullPath.Substring(Application.dataPath.Length - s_AssetsStringLength,
                absoluteFullPath.Length - (Application.dataPath.Length - s_AssetsStringLength));
            return relativeFullPath;
        }

        /// <summary>
        /// 获取资源的绝对路径
        /// EditorMode下使用
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public string GetAssetAbsoluteFullPath(string typeName, string abPath, string assetName)
        {
            // 如果assetName不直接在abPath下则需要计算出准确的fileFullPath
            string abFullPath = AorTxt.Format("{0}{1}",
                Application.dataPath.Substring(0, Application.dataPath.Length - s_AssetsStringLength), abPath);
            // 如果是一个文件则直接返回即可
            string tryFileName = AorTxt.Format("{0}{1}", abFullPath, GetAssetSuffix(typeName));
            if (File.Exists(tryFileName))
            {
                return tryFileName;
            }
            else // 如果不是文件，则开始在该目录下进行搜索
            {
                string[] fileFullPaths = Directory.GetFiles(abFullPath,
                    AorTxt.Format("{0}{1}", assetName, GetAssetSuffix(typeName)), SearchOption.AllDirectories);
                if (fileFullPaths.Length == 0)
                {
                    return null;
                }
                else if (fileFullPaths.Length != 1)
                {
                    return null;
                }

                return fileFullPaths[0].Replace('\\', '/');
            }
        }

        /// <summary>
        /// 获取Asset唯一标识路径
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public string GetAssetPath(string typeName, string abPath, string assetName)
        {
            return AorTxt.Format("{0}/{1}{2}", abPath, assetName, GetAssetSuffix(typeName));
        }

        /// <summary>
        /// 判断资源文件是否存在
        /// 对打入atlas的图片无法判断，图片请用AtlasLoadMgr
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="abPath"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public bool IsFileExist(string typeName, string abPath, string assetName)
        {
            if (m_EditorResourceMode)
            {
                string absoluteFullPath = GetAssetAbsoluteFullPath(typeName, abPath, assetName);
                if (string.IsNullOrEmpty(absoluteFullPath))
                {
                    return false;
                }

                return File.Exists(absoluteFullPath);
            }
            else
            {
                return m_LoadABComponent.IsABExist(abPath);
            }
        }

        /// <summary>
        /// 获取资源文件的后缀名
        /// </summary>
        /// <param name="typeName">资源的类型名称</param>
        /// <returns></returns>
        public string GetAssetSuffix(string typeName)
        {
            string suffix = string.Empty;
            if (GameConfig.instance.m_EditorResourceMode)
            {
                if (Enum.TryParse(typeof(GameDefinitions.AssetType), typeName, out object tmpAssetType))
                {
                    suffix = GameDefinitions.AssetSuffix[(GameDefinitions.AssetType)tmpAssetType];
                }
            }

            return suffix;
        }

        /// <summary>
        /// 检查路径下是否存在
        /// </summary>
        /// <returns></returns>
        public bool CheckAssetPath<T>(Dictionary<string, T> list, string path)
        {
            if (list.ContainsKey(path))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public Object GetLoadAssetSync(string game_Object, string abPath, string assetName)
        {
            return m_LoadAssetComponent.Load_Asset_Sync(game_Object, abPath, assetName);
        }

        /// <summary>
        /// 获取ab包在磁盘上的加载路径
        /// </summary>
        /// <param name="formatPath">ab格式化路径</param>
        /// <param name="path">ab资源路径</param>
        /// <param name="origin">ab资源来源</param>
        public void GetABLoadPathOnDisk(string formatPath, out string path, out ResourceType origin)
        {
            // 优先检查读写区域的资源是否存在，如果存在则加载读写区域的资源，否则加载只读区域
            string filePersistentPath = GamePathUtils.AssetBundleAB.Persistent.GetFileFullPath(formatPath);
            if (File.Exists(filePersistentPath))
            {
                path = filePersistentPath;
                origin = ResourceType.Persistent;
            }
            else
            {
                path = GamePathUtils.AssetBundleAB.Streaming.GetFileFullPath(formatPath);
                origin = ResourceType.Streaming;
            }
        }
    }
}