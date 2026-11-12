/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SpriteAtlasCreator.cs
 * author:    云毅
 * created:   2026
 * descrip:   Unity Sprite Atlas 资产创建工具
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Sprite Atlas 创建工具
    /// 负责创建和配置 Unity Sprite Atlas 资产
    /// </summary>
    internal static class SpriteAtlasCreator
    {
        /// <summary>
        /// 创建或更新 Sprite Atlas 资产
        /// </summary>
        /// <param name="atlasPath">Atlas 资产路径（Assets/...）</param>
        /// <param name="folderPath">包含图片的文件夹路径</param>
        public static void CreateOrUpdate(string atlasPath, string folderPath)
        {
            // 确保目录存在
            string atlasDir = Path.GetDirectoryName(atlasPath);
            if (!Directory.Exists(atlasDir))
            {
                Directory.CreateDirectory(atlasDir);
                AssetDatabase.Refresh();
            }

            // 创建 Atlas
            var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
            if (atlas == null)
            {
                var newAtlas = new SpriteAtlas();
                AssetDatabase.CreateAsset(newAtlas, atlasPath);
                atlas = newAtlas;
            }

            // 设置包含的文件夹
            var objs = new List<Object>();
            var folderObj = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            if (folderObj != null) objs.Add(folderObj);

            var serializedAtlas = new SerializedObject(atlas);
            var includeProp = serializedAtlas.FindProperty("m_IncludeObjects");
            if (includeProp != null)
            {
                includeProp.ClearArray();
                for (int i = 0; i < objs.Count; i++)
                {
                    includeProp.InsertArrayElementAtIndex(i);
                    includeProp.GetArrayElementAtIndex(i).objectReferenceValue = objs[i];
                }
            }
            serializedAtlas.ApplyModifiedProperties();

            EditorUtility.SetDirty(atlas);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 在系统文件夹中显示指定文件
        /// </summary>
        public static void ShowInFolder(string assetPath)
        {
            string fullPath = Path.GetFullPath(assetPath);
            EditorUtility.RevealInFinder(fullPath);
        }
    }
}
