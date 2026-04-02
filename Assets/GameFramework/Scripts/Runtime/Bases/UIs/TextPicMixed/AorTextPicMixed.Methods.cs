
#if UIEXTENSION_ENABLE
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Honor.Runtime
{
    public partial class AorTextPicMixed : TextPic
    {
        /// <summary>
        /// 采集Icon信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="abPath">AB路径</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="scaleXY">缩放比例</param>
        /// <param name="offsetX">偏移量X</param>
        /// <param name="offsetY">偏移量Y</param>
        /// <returns>是否采集成功</returns>
        private bool CollectIcon(List<IconName> iconList, string name, string abPath, string assetName, float scaleXY, float offsetX, float offsetY)
        {
            int index = iconList.FindIndex((icon) => { if (icon.name.Equals(name)) { return true; } return false; });
            if (index < 0)
            {
#if UNITY_EDITOR
                Sprite sprite = Application.isPlaying ? GameMainRoot.Asset.LoadAssetSync("Sprite", abPath, assetName) as Sprite : (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(GetSpriteRelativeFullPathInEditor(abPath, assetName), typeof(Sprite));
#else
                Sprite sprite = Root.Asset.LoadAssetSync("Sprite", abPath, assetName) as Sprite;
#endif
                if (sprite)
                {
                    if (Application.isPlaying)
                    {
                        m_SpriteListOnPlaying.Add(sprite);
                    }
                    Vector2 scale = new Vector2(scaleXY, scaleXY);
                    Vector2 offset = new Vector2(offsetX, offsetY);
                    iconList.Add(new IconName() { name = name, sprite = sprite, offset = offset, scale = scale });
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取Editor下Sprite加载路径
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        private string GetSpriteRelativeFullPathInEditor(string abPath, string assetName)
        {
            // 如果assetName不直接在abPath下则需要计算出准确的fileFullPath
            string abFullPath = AorTxt.Format("{0}{1}", Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), abPath);
            // 如果不是文件，则开始在该目录下进行搜索
            string fullFilePath = AorTxt.Format("{0}.png", abFullPath);
            if (!File.Exists(fullFilePath))
            {
                if(Directory.Exists(abFullPath))
                {
                    string[] fileFullPaths = Directory.GetFiles(abFullPath, AorTxt.Format("{0}.png", assetName), SearchOption.AllDirectories);
                    if (fileFullPaths.Length == 0)
                    {
                        return null;
                    }
                    else if (fileFullPaths.Length != 1)
                    {
                        return null;
                    }
                    fullFilePath = fileFullPaths[0].Replace('\\', '/');
                    string relativeFullPath = fullFilePath.Substring(Application.dataPath.Length - "Assets".Length, fullFilePath.Length - (Application.dataPath.Length - "Assets".Length));
                    return relativeFullPath;
                }
            }
            return null;
        }

    }

}

#endif
