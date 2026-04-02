using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        /// <summary>
        /// 记录当前选中的Item
        /// </summary>
        private VisualElement _selectedItem;

        /// <summary>
        /// 记录当前选中的Item
        /// </summary>
        private string _selectedImagePath;

        /// <summary>
        /// 
        /// </summary>
        private readonly float _previewBoxWidth = 120f;

        private readonly float _previewBoxHeight = 140f;
        private int _textFontSize = 14;

        private float _previewSize = 40f; // 图片预览尺寸
        private readonly Color _selectedBgColor = new Color(0.2f, 0.6f, 1f, 0.2f); // 选中背景色
        private readonly Color _normalBgColor = new Color(0.9f, 0.9f, 0.9f, 0.1f); // 正常背景色

        /// <summary>
        /// 
        /// </summary>
        private void OnSetIconList()
        {
            DrawIconPreviewList();
        }
       /// <summary>
        /// 更具地址生成图片信息
        /// </summary>
        /// <param name="folderPath"></param>
        private void GetIconPath(string folderPath)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    folderPath = EditorUtility.OpenFolderPanel("选择图片文件夹", Application.dataPath, "");
                    if (string.IsNullOrEmpty(folderPath)) return;
                }

                _selectedImagePath = folderPath;
                ClearAllItemElements();
                string[] imageExtensions = new[] { ".png", ".jpg", ".jpeg", ".tga", ".bmp" };
                string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => imageExtensions.Contains(System.IO.Path.GetExtension(file).ToLower()))
                    .ToArray();

                if (imageFiles.Length == 0)
                {
                    EditorUtility.DisplayDialog("提示", "选中的文件夹中未找到图片文件！", "确定");
                    return;
                }

                foreach (string imagePath in imageFiles)
                {
                    try
                    {
                        // 读取图片文件为Texture2D（用于预览）
                        byte[] fileData = File.ReadAllBytes(imagePath);
                        Texture2D tex = new Texture2D(2, 2);
                        tex.LoadImage(fileData); // 自动适配图片尺寸

                        // 创建图片数据对象
                        m_IconList.Add(new IconData
                        {
                            m_IconName = Path.GetFileNameWithoutExtension(imagePath), // 不含后缀的文件名
                            m_IconAssetPath = imagePath,
                            m_IconTexture = tex,
                            m_IocnIsLoaded = false // 默认未选中
                        });
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"加载图片失败：{imagePath} \n错误：{ex.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("异常", $"生成Item失败：{e.Message}", "确定");
                Debug.LogError($"OnItemBtnClicked异常：{e}");
            }
        }
        /// <summary>
        /// 绘制ICON预览列表
        /// </summary>
        /// <summary>
        /// 绘制ICON预览列表（修复重叠，正确自动换行）
        /// </summary>
        private void DrawIconPreviewList()
        {
            // 滚动视图
            m_IconListView = EditorGUILayout.BeginScrollView(m_IconListView, GUILayout.Height(420));

            // 计算一行能放多少个
            float windowWidth = this.position.width - 25;
            int itemsPerRow = Mathf.Max(1, Mathf.FloorToInt(windowWidth / _previewBoxWidth));

            // 开始垂直布局（整个列表）
            GUILayout.BeginVertical();

            int currentCount = 0;
            foreach (var data in m_IconList)
            {
                // 【关键】每一行开头，新建一行
                if (currentCount % itemsPerRow == 0)
                {
                    GUILayout.BeginVertical(); // 新行
                }
                // 绘制一个Item
                CreateIconItem(data, currentCount + 1);
                currentCount++;

                // 【关键】每一行结尾，结束行
                if (currentCount % itemsPerRow == 0)
                {
                    GUILayout.EndVertical();
                }
            }

            // 最后如果还有没闭合的行，强制闭合
            if (currentCount % itemsPerRow != 0)
            {
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        
 

        /// <summary>
        /// 绘制单个ICON Item（支持点击选中，适配截图样式）
        /// </summary>
        /// <param name="data">ICON预览数据</param>
        /// <param name="index">ITEM索引</param>
        /// <summary>
        /// 绘制单个ICON Item（修复重叠，固定尺寸）
        /// </summary>
   /// <summary>
/// 绘制单个ICON Item（40*40图标 + 文字横向布局）
/// </summary>
        /// <summary>
        /// 绘制单个ICON Item（40*40图标 + 文字横向布局，无报错版）
        /// </summary>
        private void CreateIconItem(IconData data, int index)
        {
            // 最安全的写法：固定宽度 + 固定高度
            Rect itemRect = GUILayoutUtility.GetRect(10000, 50);

            // 背景
            Color bg = _selectedIndex == index ? _selectedBgColor : _normalBgColor;
            EditorGUI.DrawRect(itemRect, bg);

            // 图标 40*40 居中
            Rect iconRect = new Rect(
                itemRect.x + 8,
                itemRect.y + 5,
                40, 40
            );

            // 绘制图标
            if (data.m_IconTexture != null)
            {
                GUI.DrawTexture(iconRect, data.m_IconTexture, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.DrawRect(iconRect, Color.gray);
                GUI.Label(iconRect, "?", new GUIStyle { alignment = TextAnchor.MiddleCenter });
            }

            // 选中红色边框
            if (_selectedIndex == index)
            {
                EditorGUI.DrawRect(new Rect(iconRect.x, iconRect.y, iconRect.width, 2), Color.red);
                EditorGUI.DrawRect(new Rect(iconRect.x, iconRect.y, 2, iconRect.height), Color.red);
                EditorGUI.DrawRect(new Rect(iconRect.xMax - 2, iconRect.y, 2, iconRect.height), Color.red);
                EditorGUI.DrawRect(new Rect(iconRect.x, iconRect.yMax - 2, iconRect.width, 2), Color.red);
            }

            // 文字在图标右侧（横向布局）
            Rect labelRect = new Rect(
                iconRect.xMax + 10,
                itemRect.y,
                itemRect.width - iconRect.width - 20,
                itemRect.height
            );

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 13,
                normal = { textColor = Color.white }
            };
            GUI.Label(labelRect, data.m_IconName, labelStyle);

            // 点击事件
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && itemRect.Contains(e.mousePosition))
            {
                _selectedIndex = index;
                ChooseIconItem(data);
                e.Use();
                Repaint();
            }
        }

        /// <summary>
        /// 更具选择添加预制体
        /// </summary>
        /// <param name="data"></param>
        private void ChooseIconItem(IconData data)
        {
            Debug.Log($"选中Icon图：{data.m_IconName}");
            if (string.IsNullOrEmpty(data.m_IconAssetPath))
            {
                return;
            }

            GameObject prefabPath = Utils.ReplaceImageInPrefab(Utils.GetIconItem(), "BG", data.m_IconAssetPath, false,Type.LoadingIcon);
            if (prefabPath != null)
            {
                AddPrefabToMap(prefabPath, Vector2.zero);
            }
        }

        /// <summary>
        /// 清空所有Item元素
        /// </summary>
        private void ClearAllItemElements()
        {
            if (m_IconList == null) return;
            m_IconList.Clear();
        }
    }
}