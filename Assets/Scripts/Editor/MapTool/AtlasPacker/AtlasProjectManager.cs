/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasProjectManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   项目管理器（保存、加载、新建）
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 项目管理器
    /// 负责项目的保存、加载、新建等操作
    /// </summary>
    internal class AtlasProjectManager
    {
        // UI 元素引用（由外部传入）
        public DropdownField PopupMaxWidth { get; set; }
        public DropdownField PopupMaxHeight { get; set; }
        public IntegerField IntBorderPadding { get; set; }
        public IntegerField IntShapePadding { get; set; }
        public TextField TxtDataFilename { get; set; }
        public TextField TxtTextureFile { get; set; }

        // 数据
        public List<SpriteItemInfo> Items { get; set; }

        // 委托：项目变更后的回调
        public Action OnProjectLoaded { get; set; }
        public Action OnProjectSaved { get; set; }

        /// <summary>
        /// 新建项目
        /// </summary>
        public void NewProject()
        {
            // 清空所有数据
            Items?.Clear();

            // 重置 UI 到默认值
            if (PopupMaxWidth != null) PopupMaxWidth.value = "1024";
            if (PopupMaxHeight != null) PopupMaxHeight.value = "1024";
            if (IntBorderPadding != null) IntBorderPadding.value = 2;
            if (IntShapePadding != null) IntShapePadding.value = 2;
            if (TxtDataFilename != null) TxtDataFilename.value = "atlas";
            if (TxtTextureFile != null) TxtTextureFile.value = "atlas.png";

            OnProjectLoaded?.Invoke();
        }

        /// <summary>
        /// 保存项目到文件
        /// </summary>
        public void SaveProject(string path)
        {
            var data = new AtlasProjectData();

            // 从 UI 读取设置
            if (PopupMaxWidth != null) data.maxWidth = PopupMaxWidth.value;
            if (PopupMaxHeight != null) data.maxHeight = PopupMaxHeight.value;
            if (IntBorderPadding != null) data.borderPadding = IntBorderPadding.value;
            if (IntShapePadding != null) data.shapePadding = IntShapePadding.value;
            if (TxtDataFilename != null) data.dataFilename = TxtDataFilename.value;
            if (TxtTextureFile != null) data.textureFile = TxtTextureFile.value;

            // 保存图片列表
            if (Items != null)
            {
                data.sprites = new string[Items.Count];
                for (int i = 0; i < Items.Count; i++)
                {
                    data.sprites[i] = Items[i].Path;
                }
            }

            // 写入文件
            var io = new AtlasProjectIO();
            io.Save(path, data);

            OnProjectSaved?.Invoke();
        }

        /// <summary>
        /// 从文件加载项目
        /// </summary>
        public void LoadProject(string path)
        {
            var io = new AtlasProjectIO();
            var data = io.Load(path);
            if (data == null) return;

            // 应用设置到 UI（带空值保护）
            if (PopupMaxWidth != null && !string.IsNullOrEmpty(data.maxWidth))
                PopupMaxWidth.value = data.maxWidth;
            if (PopupMaxHeight != null && !string.IsNullOrEmpty(data.maxHeight))
                PopupMaxHeight.value = data.maxHeight;
            if (IntBorderPadding != null)
                IntBorderPadding.value = data.borderPadding;
            if (IntShapePadding != null)
                IntShapePadding.value = data.shapePadding;
            if (TxtDataFilename != null && !string.IsNullOrEmpty(data.dataFilename))
                TxtDataFilename.value = data.dataFilename;
            if (TxtTextureFile != null && !string.IsNullOrEmpty(data.textureFile))
                TxtTextureFile.value = data.textureFile;

            // 加载图片列表（需要外部处理，这里只触发回调）
            OnProjectLoaded?.Invoke();
        }

        /// <summary>
        /// 获取当前项目设置的数据结构（用于外部读取）
        /// </summary>
        public AtlasProjectData GetCurrentData()
        {
            var data = new AtlasProjectData();

            if (PopupMaxWidth != null) data.maxWidth = PopupMaxWidth.value;
            if (PopupMaxHeight != null) data.maxHeight = PopupMaxHeight.value;
            if (IntBorderPadding != null) data.borderPadding = IntBorderPadding.value;
            if (IntShapePadding != null) data.shapePadding = IntShapePadding.value;
            if (TxtDataFilename != null) data.dataFilename = TxtDataFilename.value;
            if (TxtTextureFile != null) data.textureFile = TxtTextureFile.value;

            if (Items != null)
            {
                data.sprites = new string[Items.Count];
                for (int i = 0; i < Items.Count; i++)
                {
                    data.sprites[i] = Items[i].Path;
                }
            }

            return data;
        }
    }
}
