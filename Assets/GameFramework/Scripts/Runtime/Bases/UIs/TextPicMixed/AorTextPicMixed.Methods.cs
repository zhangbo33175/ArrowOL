/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextPicMixed.Collect.cs
 * author:    云毅
 * created:   2026
 * descrip:   图文混排 - 图标采集与资源加载（partial）
 ***************************************************************/

#if UIEXTENSION_ENABLE
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Honor.Runtime
{
    public partial class AorTextPicMixed
    {
        //=========================================================================
        // 图标采集与资源加载
        //=========================================================================
        #region Method - 图标采集
        /// <summary>
        /// 采集并加载图标信息，加入图标列表
        /// 运行时：从AB包同步加载
        /// 编辑器：从AssetDatabase加载
        /// </summary>
        /// <param name="iconList">图标列表</param>
        /// <param name="name">文本中匹配到的原始标签名称</param>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">精灵资源名称</param>
        /// <param name="scaleXY">统一缩放值（X/Y相同）</param>
        /// <param name="offsetX">X偏移</param>
        /// <param name="offsetY">Y偏移</param>
        /// <returns>是否采集成功</returns>
        private bool CollectIcon(List<IconName> iconList, string name, string abPath, string assetName, float scaleXY,
            float offsetX, float offsetY)
        {
            // 检查图标是否已存在，避免重复添加
            int index = iconList.FindIndex((icon) =>
            {
                return icon.name.Equals(name);
            });

            if (index < 0)
            {
                Sprite sprite = null;

#if UNITY_EDITOR
                // 编辑器环境：运行时走AB加载，编辑模式直接从路径加载
                if (Application.isPlaying)
                {
                    sprite = GameMainRoot.Asset.LoadAssetSync("Sprite", abPath, assetName) as Sprite;
                }
                else
                {
                    string editorPath = GetSpriteRelativeFullPathInEditor(abPath, assetName);
                    sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(editorPath, typeof(Sprite));
                }
#else
                // 运行时环境：统一从AB包同步加载精灵
                sprite = GameMainRoot.Asset.LoadAssetSync("Sprite", abPath, assetName) as Sprite;
#endif

                if (sprite != null)
                {
                    // 运行模式：缓存加载的Sprite，用于销毁时释放
                    if (Application.isPlaying)
                    {
                        m_SpriteListOnPlaying.Add(sprite);
                    }

                    // 构造图标信息并加入列表
                    Vector2 scale = new Vector2(scaleXY, scaleXY);
                    Vector2 offset = new Vector2(offsetX, offsetY);
                    iconList.Add(new IconName()
                    {
                        name = name,
                        sprite = sprite,
                        offset = offset,
                        scale = scale
                    });
                }
                else
                {
                    // 加载失败，返回false
                    return false;
                }
            }

            return true;
        }
        #endregion

        //=========================================================================
        // 编辑器路径搜索
        //=========================================================================
        #region Method - 编辑器资源路径
        /// <summary>
        /// 编辑器专用：获取Sprite的完整资源路径
        /// 自动搜索目录下的图片，解决AB路径与实际文件路径不一致的问题
        /// </summary>
        /// <param name="abPath">AB包配置路径</param>
        /// <param name="assetName">资源名称</param>
        /// <returns>Unity可识别的资源相对路径</returns>
        private string GetSpriteRelativeFullPathInEditor(string abPath, string assetName)
        {
            // 拼接磁盘绝对路径（去掉Assets前缀）
            string rootPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
            string abFullPath = AorTxt.Format("{0}{1}", rootPath, abPath);

            // 先尝试直接拼接路径
            string fullFilePath = AorTxt.Format("{0}.png", abFullPath);

            // 文件不存在，则进入目录递归搜索
            if (!File.Exists(fullFilePath))
            {
                if (Directory.Exists(abFullPath))
                {
                    // 搜索所有子目录下的目标png文件
                    string[] fileFullPaths = Directory.GetFiles(
                        abFullPath,
                        AorTxt.Format("{0}.png", assetName),
                        SearchOption.AllDirectories
                    );

                    // 找不到 或 找到多个，都返回null
                    if (fileFullPaths.Length == 0 || fileFullPaths.Length != 1)
                    {
                        return null;
                    }

                    // 格式化为Unity可识别的相对路径
                    fullFilePath = fileFullPaths[0].Replace('\\', '/');
                    string relativeFullPath = fullFilePath.Substring(
                        rootPath.Length,
                        fullFilePath.Length - rootPath.Length
                    );

                    return relativeFullPath;
                }
            }

            return null;
        }
        #endregion
    }
}
#endif