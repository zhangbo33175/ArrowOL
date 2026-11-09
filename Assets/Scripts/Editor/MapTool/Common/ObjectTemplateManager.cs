/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  ObjectTemplateManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   物体模板管理器 - 统一管理所有物品元件模板
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 物体模板管理器（单例模式）
    /// 统一管理建筑、人物、物件三类地图元素模板
    /// 所有模板数据从Excel配置文件动态加载，不硬编码
    /// </summary>
    public class ObjectTemplateManager
    {
        #region 单例

        private static ObjectTemplateManager _instance;

        /// <summary>
        /// 单例实例（懒加载）
        /// </summary>
        public static ObjectTemplateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ObjectTemplateManager();
                }
                return _instance;
            }
        }

        #endregion

        #region 字段

        /// <summary>
        /// 模板字典（Key: 模板ID, Value: 模板对象）
        /// </summary>
        private readonly Dictionary<string, ObjectTemplate> _templates = new Dictionary<string, ObjectTemplate>();

        /// <summary>
        /// 默认配置文件相对路径（相对于项目根目录）
        /// </summary>
        private const string DefaultConfigRelativePath = @"Docs\Designs\Excels\Tables\AlTables\Item\TableTemplate.xlsm";

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化模板管理器
        /// 从默认配置文件加载所有模板数据
        /// </summary>
        public void Initialize()
        {
            _templates.Clear();

            // 计算默认配置文件完整路径
            string defaultConfigPath = Path.Combine(Application.dataPath, "..", DefaultConfigRelativePath);
            defaultConfigPath = Path.GetFullPath(defaultConfigPath);

            // 检查文件是否存在
            if (!File.Exists(defaultConfigPath))
            {
                Debug.LogError($"[ObjectTemplateManager] 默认配置文件不存在: {defaultConfigPath}");
                return;
            }

            // 从配置文件加载
            LoadFromFile(defaultConfigPath);

            Debug.Log($"[ObjectTemplateManager] 初始化完成，共加载 {_templates.Count} 个模板");
        }

        /// <summary>
        /// 从指定配置文件加载模板数据
        /// </summary>
        /// <param name="filePath">Excel配置文件绝对路径（.xlsm格式）</param>
        public void LoadFromFile(string filePath)
        {
            // 参数校验
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogWarning("[ObjectTemplateManager] 配置文件路径为空");
                return;
            }

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[ObjectTemplateManager] 配置文件不存在: {filePath}");
                return;
            }

            // 从Excel读取模板数据
            List<ObjectTemplate> templates = TemplateConfigReader.ReadFromExcel(filePath);

            if (templates == null || templates.Count == 0)
            {
                Debug.LogWarning($"[ObjectTemplateManager] 配置文件中没有读取到模板数据: {filePath}");
                return;
            }

            // 清空旧数据，加载新数据
            _templates.Clear();
            foreach (var template in templates)
            {
                AddTemplate(template);
            }

            Debug.Log($"[ObjectTemplateManager] 从配置文件加载完成: {Path.GetFileName(filePath)}，共 {templates.Count} 个模板");
        }

        /// <summary>
        /// 从所有类型的配置表 Excel 文件批量加载最新数据
        /// </summary>
        public void LoadAllConfigTables()
        {
            _templates.Clear();

            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string tableDir = Path.Combine(projectRoot, MapToolConstants.ConfigTableRootDir);

            // 所有类型的配置表文件
            string[] tableFiles = new[]
            {
                MapToolConstants.TableBuildingFileName,
                MapToolConstants.TableCharacterFileName,
                MapToolConstants.TablePropFileName,
                MapToolConstants.TableCatFileName,
                MapToolConstants.TableBgFileName,
            };

            int totalLoaded = 0;
            foreach (var fileName in tableFiles)
            {
                string filePath = Path.Combine(tableDir, fileName);
                if (!File.Exists(filePath)) continue;

                List<ObjectTemplate> templates = TemplateConfigReader.ReadFromExcel(filePath);
                if (templates == null) continue;

                foreach (var t in templates)
                {
                    AddTemplate(t);
                    totalLoaded++;
                }
            }

            Debug.Log($"[ObjectTemplateManager] 已从所有配置表重新加载，共 {totalLoaded} 个模板");
        }

        #endregion

        #region 模板增删改

        /// <summary>
        /// 添加或更新模板
        /// </summary>
        /// <param name="template">模板对象</param>
        public void AddTemplate(ObjectTemplate template)
        {
            // 空值校验
            if (template == null)
            {
                Debug.LogWarning("[ObjectTemplateManager] 模板对象为空，跳过添加");
                return;
            }

            if (string.IsNullOrEmpty(template.TemplateId))
            {
                Debug.LogWarning("[ObjectTemplateManager] 模板ID不能为空，跳过添加");
                return;
            }

            _templates[template.TemplateId] = template;
        }

        /// <summary>
        /// 清空所有模板
        /// </summary>
        public void ClearAll()
        {
            int removedCount = _templates.Count;
            _templates.Clear();
            Debug.Log($"[ObjectTemplateManager] 已清空所有模板，共移除 {removedCount} 个");
        }

        /// <summary>
        /// 原子性地更新模板ID（同时更新字典key和对象属性）
        /// </summary>
        /// <param name="template">要更新的模板对象</param>
        /// <param name="newId">新的模板ID</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateTemplateId(ObjectTemplate template, string newId)
        {
            if (template == null || string.IsNullOrEmpty(newId))
                return false;

            string oldId = template.TemplateId;
            if (oldId == newId)
                return true;

            // 从字典中移除旧 key
            if (!_templates.ContainsKey(oldId))
            {
                Debug.LogWarning($"[ObjectTemplateManager] 更新ID失败，旧key不存在: '{oldId}'");
                return false;
            }

            _templates.Remove(oldId);

            // 更新对象属性
            template.TemplateId = newId;

            // 以新 key 重新添加到字典
            _templates[newId] = template;

            return true;
        }

        /// <summary>
        /// 根据ID删除模板
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <returns>是否删除成功</returns>
        public bool RemoveTemplate(string templateId)
        {
            if (string.IsNullOrEmpty(templateId))
                return false;

            Debug.Log($"[ObjectTemplateManager] 尝试删除模板: ID='{templateId}' (长度={templateId.Length})");
            Debug.Log($"[ObjectTemplateManager] 当前字典中的键数量: {_templates.Count}");
            foreach (var key in _templates.Keys)
            {
                Debug.Log($"[ObjectTemplateManager] 字典键: '{key}' (长度={key.Length})");
            }

            if (_templates.ContainsKey(templateId))
            {
                _templates.Remove(templateId);
                Debug.Log($"[ObjectTemplateManager] 已删除模板: {templateId}");
                return true;
            }

            Debug.LogWarning($"[ObjectTemplateManager] 未找到要删除的模板: {templateId}");
            return false;
        }

        #endregion

        #region 模板查询

        /// <summary>
        /// 根据ID获取模板
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <returns>模板对象，未找到返回null</returns>
        public ObjectTemplate GetTemplateById(string templateId)
        {
            if (string.IsNullOrEmpty(templateId))
                return null;

            ObjectTemplate template;
            if (_templates.TryGetValue(templateId, out template))
                return template;

            Debug.LogWarning($"[ObjectTemplateManager] 未找到模板: {templateId}");
            return null;
        }

        /// <summary>
        /// 根据名称获取模板（模糊匹配 TemplateName）
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns>模板对象，未找到返回 null</returns>
        public ObjectTemplate GetTemplateByName(string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
                return null;

            foreach (var t in _templates.Values)
            {
                if (t.TemplateName == templateName)
                    return t;
            }
            return null;
        }

        /// <summary>
        /// 获取所有模板列表
        /// </summary>
        /// <returns>所有模板列表的副本</returns>
        public List<ObjectTemplate> GetAllTemplates()
        {
            return new List<ObjectTemplate>(_templates.Values);
        }

        /// <summary>
        /// 按物体类型筛选模板
        /// </summary>
        /// <param name="objectType">物体类型</param>
        /// <returns>对应类型的模板列表</returns>
        public List<ObjectTemplate> GetTemplatesByType(MapObjectType objectType)
        {
            List<ObjectTemplate> result = new List<ObjectTemplate>();
            foreach (var template in _templates.Values)
            {
                if (template.Type == objectType)
                    result.Add(template);
            }
            return result;
        }

        /// <summary>
        /// 获取当前模板总数
        /// </summary>
        public int TemplateCount
        {
            get { return _templates.Count; }
        }

        #endregion

        #region JSON 持久化（编辑器内保存，不直接写 Excel）

        /// <summary>
        /// 将所有模板保存为 JSON 文件（用于编辑器内修改的持久化）
        /// </summary>
        /// <param name="jsonPath">JSON 文件绝对路径</param>
        public void SaveToJson(string jsonPath)
        {
            try
            {
                var list = new List<ObjectTemplate>(_templates.Values);
                string json = JsonUtility.ToJson(new TemplateListWrapper { templates = list }, true);
                File.WriteAllText(jsonPath, json, System.Text.Encoding.UTF8);
                Debug.Log($"[ObjectTemplateManager] JSON 保存完成: {jsonPath}，共 {list.Count} 个模板");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ObjectTemplateManager] JSON 保存失败: {e.Message}");
            }
        }

        /// <summary>
        /// 从 JSON 文件加载模板（覆盖当前内存数据）
        /// </summary>
        /// <param name="jsonPath">JSON 文件绝对路径</param>
        /// <returns>加载的模板数量</returns>
        public int LoadFromJson(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return 0;

            try
            {
                string json = File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
                var wrapper = JsonUtility.FromJson<TemplateListWrapper>(json);
                if (wrapper?.templates == null)
                    return 0;

                _templates.Clear();
                foreach (var t in wrapper.templates)
                {
                    if (!string.IsNullOrEmpty(t.TemplateId))
                        _templates[t.TemplateId] = t;
                }

                Debug.Log($"[ObjectTemplateManager] JSON 加载完成: {jsonPath}，共 {_templates.Count} 个模板");
                return _templates.Count;
            }
            catch (Exception e)
            {
                Debug.LogError($"[ObjectTemplateManager] JSON 加载失败: {e.Message}");
                return 0;
            }
        }

        /// <summary>
        /// JSON 序列化包装类（JsonUtility 不支持直接序列化 List）
        /// </summary>
        [System.Serializable]
        private class TemplateListWrapper
        {
            public List<ObjectTemplate> templates;
        }

        #endregion
    }
}
