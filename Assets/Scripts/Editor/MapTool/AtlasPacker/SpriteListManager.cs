/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SpriteListManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   Sprite 列表管理（添加、删除、加载文件夹）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Sprite 列表管理器
    /// 负责图片列表的添加、删除、加载文件夹等操作
    /// </summary>
    internal class SpriteListManager
    {
        // 列表数据
        public List<SpriteItemInfo> Items { get; private set; } = new List<SpriteItemInfo>();

        // UI 元素引用（由外部传入）
        public VisualElement ListContainer { get; set; }
        public Label CountLabel { get; set; }

        // 委托：路径转换
        public Func<string, string> FullToAssetPath { get; set; }
        public Func<string, string> AssetToFullPath { get; set; }

        // 委托：选中回调
        public Action<int> OnItemSelected { get; set; }

        /// <summary>
        /// 添加单个 sprite 项（只负责数据，UI 由外部刷新）
        /// </summary>
        public void AddSpriteItem(string assetPath)
        {
            string fileName = Path.GetFileName(assetPath);
            string fullPath = AssetToFullPath?.Invoke(assetPath);
            long fileSize = 0;
            if (File.Exists(fullPath))
            {
                fileSize = new FileInfo(fullPath).Length;
            }

            var item = new SpriteItemInfo
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                Path = assetPath,
                Size = $"{fileSize / 1024} KB"
            };

            Items.Add(item);
        }

        /// <summary>
        /// 按文件名排序（对齐 TexturePacker 右侧 Sprites 列表规则）：
        /// 字符串字典序（忽略大小写），同前缀归组、主图在前 copy 后缀跟随；
        /// 稳定排序，保留同名重复项的原有相对顺序。
        /// </summary>
        public void SortByName()
        {
            if (Items == null || Items.Count <= 1) return;

            var ordered = Items
                .OrderBy(item => Path.GetFileName(item.Path), StringComparer.OrdinalIgnoreCase)
                .ToList();

            Items.Clear();
            Items.AddRange(ordered);
        }

        /// <summary>
        /// 从文件夹加载所有图片
        /// 优先用磁盘直读（解决 ...Sprite 等特殊文件夹名问题），更可靠
        /// </summary>
        public void LoadSpritesFromFolder(string folderPath)
        {
            // 先用磁盘直读（更可靠，不受特殊文件夹名影响）
            LoadFromDisk(folderPath);

            // 如果磁盘直读结果也为空，再尝试 AssetDatabase.FindAssets
            if (Items.Count == 0)
            {
                string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });
                foreach (string guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(assetPath)) continue;

                    // 跳过非图片文件
                    string ext = Path.GetExtension(assetPath).ToLower();
                    if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".psd")
                        continue;

                    // 避免重复添加
                    bool exists = Items.Exists(x => x.Path == assetPath);
                    if (!exists)
                    {
                        AddSpriteItem(assetPath);
                    }
                }
            }

            // 磁盘直读 / AssetDatabase 两条路径都加载完成后，统一按文件名排序
            SortByName();
        }

        /// <summary>
        /// 从磁盘直接读取文件夹里的图片文件
        /// 解决 AssetDatabase.FindAssets 对特殊文件夹名（如 ...Sprite）搜索不到的问题
        /// </summary>
        private void LoadFromDisk(string folderPath)
        {
            // 把 Asset 路径转成磁盘完整路径
            string fullPath = AssetToFullPath?.Invoke(folderPath);
            if (string.IsNullOrEmpty(fullPath))
            {
                Debug.LogWarning($"路径转换失败: {folderPath}");
                return;
            }

            fullPath = fullPath.Replace('\\', '/');
            if (!Directory.Exists(fullPath))
            {
                Debug.LogWarning($"文件夹不存在: {fullPath}");
                return;
            }

            // 支持的图片扩展名
            string[] extensions = { "*.png", "*.jpg", "*.jpeg", "*.psd" };

            foreach (string ext in extensions)
            {
                string[] files = Directory.GetFiles(fullPath, ext, SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    // 跳过 .meta 文件
                    if (file.EndsWith(".meta")) continue;

                    // 把磁盘路径转成 Asset 路径
                    string assetPath = FullToAssetPath?.Invoke(file);
                    if (string.IsNullOrEmpty(assetPath)) continue;

                    // 避免重复添加
                    bool exists = Items.Exists(x => x.Path == assetPath);
                    if (!exists)
                    {
                        AddSpriteItem(assetPath);
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定索引的项（只负责数据，UI 由外部刷新）
        /// </summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Items.Count) return;
            Items.RemoveAt(index);
        }

        /// <summary>
        /// 清空所有项（只负责数据，UI 由外部刷新）
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// 刷新列表 UI
        /// </summary>
        public void RefreshListUI()
        {
            if (ListContainer == null) return;

            ListContainer.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                var itemElement = new VisualElement();
                itemElement.AddToClassList("sprite-item");
                itemElement.userData = i;

                // 名称
                var nameLabel = new Label(item.Name);
                nameLabel.AddToClassList("sprite-name");
                itemElement.Add(nameLabel);

                // 尺寸
                var sizeLabel = new Label(item.Size);
                sizeLabel.AddToClassList("sprite-size");
                itemElement.Add(sizeLabel);

                // 点击事件
                int index = i;
                itemElement.RegisterCallback<ClickEvent>(evt =>
                {
                    OnItemSelected?.Invoke(index);
                });

                ListContainer.Add(itemElement);
            }

            // 更新计数
            if (CountLabel != null)
            {
                CountLabel.text = $"{Items.Count} sprites";
            }
        }

        /// <summary>
        /// 获取指定索引的项
        /// </summary>
        public SpriteItemInfo GetAt(int index)
        {
            if (index < 0 || index >= Items.Count) return null;
            return Items[index];
        }
    }
}
