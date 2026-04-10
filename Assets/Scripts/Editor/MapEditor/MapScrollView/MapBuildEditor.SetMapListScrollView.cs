using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        /// <summary>
        /// 存储读取到的所有地图配置
        /// </summary>
        private List<MapAssetData> _mapConfigs = new List<MapAssetData>();

        /// <summary>
        /// 选中的列表项索引
        /// </summary>
        private int _selectedIndex = -1;
        /// <summary>
        /// 核心方法：在ScrollView中绘制MapConfigData列表
        /// </summary>
        private void OnSetMapJsonList()
        {
            m_MapListView = GUILayout.BeginScrollView(m_MapListView, 
                GUILayout.MinHeight(260), GUILayout.Height(260));

            GUILayout.BeginVertical();

            if (m_TableMainLevelsList.Count == 0)
            {
                GUILayout.Space(20);
                GUILayout.Label("暂无地图配置数据", EditorStyles.centeredGreyMiniLabel);
            }
            else
            {
                for (int i = 0; i < m_TableMainLevelsList.Count; i++)
                {
                   GUILayout.BeginVertical("Box");
                   DrawClickableListItem(m_TableMainLevelsList[i], i);
                   GUILayout.EndVertical(); 
                   GUILayout.Space(2);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// 可点击的列表项
        /// </summary>
        private void DrawClickableListItem(TableMainLevels tableMainLevels, int index)
        {
            if (_selectedIndex == index)
            {
                GUILayout.BeginHorizontal("Box");
            }
            else
            {
                GUILayout.BeginHorizontal();
            }

            if (GUILayout.Button($"{tableMainLevels.LevelID}", EditorStyles.label, GUILayout.Width(165)))
            {
                _selectedIndex = index;
                ChooseItem(tableMainLevels);
            }
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableMainLevels"></param>
        private void ChooseItem(TableMainLevels tableMainLevels)
        {
            try
            {
                GameObject prefabPath = Utils.ReplaceImageInPrefab(Utils.GetMap(), "RawImage", tableMainLevels.Level_bg, false,Type.LoadBackground);
                if (prefabPath!=null)
                {
                    LoadMapPrefab(prefabPath, () => { SetPreviewRect(); }); 
                }
                m_TableMainLevels = tableMainLevels;
                mapName = tableMainLevels.LevelID;
                mapId = tableMainLevels.ID;
                m_IsInitIconList = false;
                SetIconItem();
                Debug.Log($"选中地图：{tableMainLevels.LevelID}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}