/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.IconModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 图标列表管理模块（根据地图配置动态加载图标）
 ***************************************************************/

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 - 图标列表管理模块
    /// 负责根据地图配置，动态加载并绑定该地图可使用的图标列表
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        //=========================================================================
        // 图标数据绑定与加载
        //=========================================================================
        #region Icon Data Binding
        /// <summary>
        /// 根据当前选中的地图配置，初始化并绑定【该地图可使用的图标列表】
        /// 会先清空旧数据，再根据地图配置的 ItemID 从全局物品表中匹配图标
        /// </summary>
        /// <param name="tableMainLevelsEditor">当前选中的主关卡配置数据</param>
        public void SetIconItem(TableMainLevelsEditor tableMainLevelsEditor)
        {
            // 安全校验：传入的配置数据不能为空
            if (tableMainLevelsEditor == null)
            {
                UnityEngine.Debug.LogWarning("地图配置数据为空，无法加载图标列表！");
                return;
            }

            // 清空旧图标列表，避免历史数据残留
            m_IconList.Clear();

            // 从关卡配置中解析：该地图允许使用的物品 ID 数组
            int[] itemIds = Utils.GetIntList(tableMainLevelsEditor.CatItemID);

            // 安全校验：物品 ID 数组不能为空
            if (itemIds == null || itemIds.Length == 0)
            {
                UnityEngine.Debug.LogWarning("当前地图未配置任何可使用物品 ID！");
                return;
            }

            // 遍历地图配置的物品 ID，匹配全局物品表
            foreach (int currentId in itemIds)
            {
                // 从全局物品配置表中，找到 ID 匹配的物品
                var targetItem = m_TableItemsList.Find(item => item.ID == currentId);

                // 找到有效物品 → 转为编辑器图标数据
                if (targetItem != null)
                {
                    m_IconList.Add(new ItemIconDataEditor
                    {
                        m_ID = targetItem.ID,                  // 物品唯一ID
                        m_IconName = targetItem.Icon,          // 图标名称（用于显示）
                        m_IconAssetPath = targetItem.Icon,     // 图标资源路径（用于加载）
                        m_IsChoose = false,                    // 默认未选中状态
                        m_Type = RMapIconType.Dissipate        // 图标交互类型：默认选中消散
                    });
                }
            }
        }
        #endregion
    }
}