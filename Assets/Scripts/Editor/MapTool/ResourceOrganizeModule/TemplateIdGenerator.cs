/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TemplateIdGenerator.cs
 * author:    云毅
 * created:   2026
 * descrip:   模板ID生成器 - 按类型分段生成ID，删除后自动重排序
 ***************************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Honor.Editor.MapTool.ResourceOrganize
{
    /// <summary>
    /// 模板ID生成器：按类型分段生成ID（Character 1xxx、Building 2xxx、Prop 3xxx）
    /// 删除模板后自动重排序补全ID
    /// </summary>
    public static class TemplateIdGenerator
    {
        // ==================== ID 分段定义 ====================

        /// <summary>人物类型 ID 起始值</summary>
        private const int CharacterIdStart = 1000;
        /// <summary>建筑类型 ID 起始值</summary>
        private const int BuildingIdStart = 2000;
        /// <summary>物件类型 ID 起始值</summary>
        private const int PropIdStart = 3000;
        /// <summary>猫猫类型 ID 起始值</summary>
        private const int CatIdStart = 4000;

        /// <summary>
        /// 获取指定类型的下一个可用ID
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>下一个可用的ID字符串</returns>
        public static string GetNextId(MapObjectType type)
        {
            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();
            int maxId = GetMaxIdOfType(allTemplates, type);
            int nextId = maxId + 1;
            return nextId.ToString();
        }

        /// <summary>
        /// 删除模板后，对同类型的ID进行重排序补全
        /// </summary>
        /// <param name="type">需要重排序的物体类型</param>
        public static void ReorderIds(MapObjectType type)
        {
            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();
            var typeTemplates = allTemplates
                .Where(t => t.Type == type)
                .OrderBy(t =>
                {
                    int id;
                    return int.TryParse(t.TemplateId, out id) ? id : int.MaxValue;
                })
                .ToList();

            int idCounter = GetIdStartValue(type);
            foreach (var template in typeTemplates)
            {
                int oldId;
                int.TryParse(template.TemplateId, out oldId);
                if (oldId != idCounter)
                {
                    // 通过管理器原子性更新ID（同步字典key和对象属性）
                    ObjectTemplateManager.Instance.UpdateTemplateId(template, idCounter.ToString());
                    Debug.Log($"ID 重排序: {oldId} -> {idCounter} ({template.TemplateName})");
                }
                idCounter++;
            }
        }

        // ==================== 私有辅助方法 ====================

        /// <summary>
        /// 获取指定类型的最大ID
        /// </summary>
        private static int GetMaxIdOfType(List<ObjectTemplate> templates, MapObjectType type)
        {
            var typeIds = templates
                .Where(t => t.Type == type)
                .Select(t =>
                {
                    int id;
                    return int.TryParse(t.TemplateId, out id) ? id : 0;
                })
                .ToList();

            return typeIds.Count > 0 ? typeIds.Max() : GetIdStartValue(type) - 1;
        }

        /// <summary>
        /// 获取指定类型的ID起始值
        /// </summary>
        private static int GetIdStartValue(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Character: return CharacterIdStart;
                case MapObjectType.Building: return BuildingIdStart;
                case MapObjectType.Prop: return PropIdStart;
                case MapObjectType.Cat: return CatIdStart;
                default: return 1000;
            }
        }
    }
}
