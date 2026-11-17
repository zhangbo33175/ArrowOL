/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapAssetLoader.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器资源加载器（模板预制体 / Sprite / Texture 缓存 + 三级目录查找 + 纹理转 Sprite 回退）
 *            场景编辑模块只描述"要什么资源"，不再关心缓存、路径拼接与加载失败回退
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器资源加载器
    /// 统一承接 AssetDatabase 访问：模板预制体、Sprite、Texture 的带缓存加载，
    /// 以及「Sprite 资产缺失时用 Texture2D 现造 Sprite」的统一回退口径。
    /// 同一实例内缓存跨次复用，重建场景时调用 <see cref="ClearCaches"/> 失效。
    /// </summary>
    public sealed class MapAssetLoader
    {
        /// <summary>模板预制体缓存（类型 → 预制体）</summary>
        private readonly Dictionary<MapObjectType, GameObject> m_TemplatePrefabs = new Dictionary<MapObjectType, GameObject>();

        /// <summary>Sprite 资产缓存（资产路径 → Sprite，含加载失败的空值）</summary>
        private readonly Dictionary<string, Sprite> m_Sprites = new Dictionary<string, Sprite>();

        /// <summary>Texture 资产缓存（资产路径 → Texture2D，含加载失败的空值）</summary>
        private readonly Dictionary<string, Texture2D> m_Textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// 失效全部缓存（场景重建、资产替换后调用，避免引用旧资产）
        /// </summary>
        public void ClearCaches()
        {
            m_TemplatePrefabs.Clear();
            m_Sprites.Clear();
            m_Textures.Clear();
        }

        #region 模板预制体

        /// <summary>
        /// 按类型加载基础模板预制体（带缓存）
        /// </summary>
        /// <param name="type">物件类型（猫猫复用物件模板）</param>
        /// <returns>基础模板预制体，缺失返回 null</returns>
        public GameObject LoadTemplatePrefab(MapObjectType type)
        {
            if (m_TemplatePrefabs.TryGetValue(type, out GameObject cached))
                return cached;

            string path;
            switch (type)
            {
                case MapObjectType.Building:
                    path = MapToolConstants.BuildingBaseTemplatePrefab;
                    break;
                case MapObjectType.Character:
                    path = MapToolConstants.CharacterBaseTemplatePrefab;
                    break;
                default:
                    path = MapToolConstants.PropBaseTemplatePrefab;
                    break;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            m_TemplatePrefabs[type] = prefab;
            return prefab;
        }

        #endregion

        #region Sprite / Texture

        /// <summary>
        /// 按资产路径加载 Sprite（带缓存）
        /// </summary>
        /// <param name="assetPath">Assets 相对路径</param>
        /// <returns>Sprite，缺失返回 null</returns>
        public Sprite LoadSprite(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return null;
            if (m_Sprites.TryGetValue(assetPath, out Sprite cached))
                return cached;

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            m_Sprites[assetPath] = sprite;
            return sprite;
        }

        /// <summary>
        /// 按资产路径加载 Texture2D（带缓存）
        /// </summary>
        /// <param name="assetPath">Assets 相对路径</param>
        /// <returns>Texture2D，缺失返回 null</returns>
        public Texture2D LoadTexture(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return null;
            if (m_Textures.TryGetValue(assetPath, out Texture2D cached))
                return cached;

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            m_Textures[assetPath] = texture;
            return texture;
        }

        /// <summary>
        /// 按资产路径取 Sprite：优先 Sprite 资产，缺失时用 Texture2D 现造（带缓存）
        /// 统一「图片未设置为 Sprite 类型」时的回退口径，避免各处重复 Sprite.Create。
        /// </summary>
        /// <param name="assetPath">Assets 相对路径</param>
        /// <returns>可用 Sprite，缺失返回 null</returns>
        public Sprite LoadSpriteOrCreate(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return null;
            if (m_Sprites.TryGetValue(assetPath, out Sprite cached) && cached != null)
                return cached;

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite == null)
            {
                Texture2D texture = LoadTexture(assetPath);
                sprite = CreateSpriteFromTexture(texture);
            }

            m_Sprites[assetPath] = sprite;
            return sprite;
        }

        /// <summary>
        /// 用整张纹理现造一个居中锚点的 Sprite（不落盘，仅编辑器预览用）
        /// </summary>
        /// <param name="texture">源纹理</param>
        /// <returns>Sprite，纹理为空返回 null</returns>
        public static Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }

        #endregion

        #region 三级目录查找

        /// <summary>
        /// 按图片名在 Map 资源目录下查找 PNG（三级：关卡专属 → 章节公共 → 全局公共），并兼容旧单数目录名
        /// </summary>
        /// <param name="imageName">图片文件名（不含扩展名）</param>
        /// <param name="layerType">分层类型键（见 MapLayerCatalog）</param>
        /// <param name="chapterId">当前章节ID（可为空）</param>
        /// <param name="levelId">当前关卡ID（可为空）</param>
        /// <returns>Assets 相对路径，未找到返回 null</returns>
        public string FindMapTextureByLayer(string imageName, string layerType, string chapterId, string levelId)
        {
            if (string.IsNullOrEmpty(imageName)) return null;

            var candidates = new List<string>();

            // 第1级：当前关卡专属目录
            if (!string.IsNullOrEmpty(chapterId) && !string.IsNullOrEmpty(levelId))
                candidates.Add(MapLayerCatalog.GetLevelDir(chapterId, levelId, layerType));

            // 第2级：章节公共目录（同章节跨关卡复用）
            if (!string.IsNullOrEmpty(chapterId))
                candidates.Add(MapLayerCatalog.GetChapterSharedDir(chapterId, layerType));

            // 第3级：全局公共目录（跨章节复用）
            candidates.Add(MapLayerCatalog.GetGlobalSharedDir(layerType));

            foreach (string dir in candidates)
            {
                string assetPath = $"{dir}/{imageName}{MapToolConstants.ExtPng}";
                if (AssetDatabase.LoadAssetAtPath<Object>(assetPath) != null)
                    return assetPath;
            }

            // 兼容旧版单数目录名（Building/Character/Prop/Cat）
            string legacyDir = MapLayerCatalog.GetLayerDir(layerType) switch
            {
                MapLayerCatalog.DirBuildings => MapToolConstants.ResNameKeywordBuilding,
                MapLayerCatalog.DirCharacters => MapToolConstants.ResNameKeywordCharacter,
                MapLayerCatalog.DirProps => MapToolConstants.ResNameKeywordProp,
                string other => other,
            };
            if (!string.IsNullOrEmpty(chapterId))
            {
                string chapterLegacy = $"{MapToolConstants.MapTextureRoot}/Chapter{chapterId}/{legacyDir}/{imageName}{MapToolConstants.ExtPng}";
                if (AssetDatabase.LoadAssetAtPath<Object>(chapterLegacy) != null)
                    return chapterLegacy;
            }
            string sharedLegacy = $"{MapToolConstants.MapTextureRoot}/Shared/{legacyDir}/{imageName}{MapToolConstants.ExtPng}";
            if (AssetDatabase.LoadAssetAtPath<Object>(sharedLegacy) != null)
                return sharedLegacy;

            return null;
        }

        /// <summary>
        /// 自动在 Background 三级目录中查找背景图（bg_*.png）：关卡 → 章节公共 → 全局公共
        /// </summary>
        /// <param name="chapterId">当前章节ID（可为空）</param>
        /// <param name="levelId">当前关卡ID（可为空）</param>
        /// <returns>Assets 相对路径，未找到返回 null</returns>
        public string FindBackgroundImageByChapter(string chapterId, string levelId)
        {
            var bgDirs = new List<string>();
            if (!string.IsNullOrEmpty(chapterId) && !string.IsNullOrEmpty(levelId))
                bgDirs.Add(MapLayerCatalog.GetLevelDir(chapterId, levelId, MapLayerCatalog.Background));
            if (!string.IsNullOrEmpty(chapterId))
                bgDirs.Add(MapLayerCatalog.GetChapterSharedDir(chapterId, MapLayerCatalog.Background));
            bgDirs.Add(MapLayerCatalog.GetGlobalSharedDir(MapLayerCatalog.Background));

            foreach (string bgDir in bgDirs)
            {
                string fullPath = MapToolPathHelper.ToAbsolutePath(bgDir);
                if (!Directory.Exists(fullPath)) continue;

                string[] bgFiles = Directory.GetFiles(fullPath, $"{MapToolConstants.PrefixBg}*{MapToolConstants.ExtPng}");
                if (bgFiles.Length > 0)
                    return bgFiles[0].Replace('\\', '/');
            }
            return null;
        }

        #endregion
    }
}
