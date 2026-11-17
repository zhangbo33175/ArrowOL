/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasProjectIO.cs
 * author:    云毅
 * created:   2026
 * descrip:   项目文件读写（.atlasproj）
 ***************************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 项目文件数据结构
    /// </summary>
    [Serializable]
    internal class AtlasProjectData
    {
        // 版本号
        public string version = "1.0";

        // 输出设置
        public string dataFormat = "Unity3D";
        public string dataFilename;
        public string textureFormat = "PNG";
        public string textureFile;
        public int pngOptLevel = 2;
        public int dpi = 72;
        public bool premultiplyAlpha = false;
        public string imageFormat = "RGBA8888";
        public string ditheringAlgorithm = "NearestNeighbour";
        public string texturePath;

        // 尺寸设置
        public string maxWidth;
        public string maxHeight;
        public int fixedWidth = 0;
        public int fixedHeight = 0;
        public string sizeConstraints = "POT";
        public bool forceSquare = false;
        public float scale = 1f;
        public string scaleMode = "Smooth";

        // 布局设置
        public string layoutAlgorithm = "MaxRects";
        public string heuristicStrategy = "Best";
        public string packMode = "Best";
        public int borderPadding;
        public int shapePadding;
        public int innerPadding = 0;
        public int extrude = 0;
        public int divisorX = 1;
        public int divisorY = 1;
        public bool allowRotation = false;
        public bool autoAlias = true;
        public string trimMode = "None";
        public int trimThreshold = 1;
        public bool multipack = false;
        public bool reduceBorderArtifacts = false;
        public bool shapeOutlines = false;
        public bool heuristicMask = false;
        public bool forceWordAligned = false;
        public bool trimSpriteNames = false;

        // 图片列表
        public string[] sprites;
    }

    /// <summary>
    /// 项目文件读写器
    /// </summary>
    internal class AtlasProjectIO
    {
        /// <summary>
        /// 保存项目到文件
        /// </summary>
        public void Save(string path, AtlasProjectData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 从文件加载项目
        /// </summary>
        public AtlasProjectData Load(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                string json = File.ReadAllText(path);
                var project = JsonUtility.FromJson<AtlasProjectData>(json);
                return project;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
