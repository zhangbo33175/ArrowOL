/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  ExportConfigData.cs
 * author:    云毅
 * created:   2026
 * descrip:   元件配置导出数据类
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 元件配置导出数据
    /// </summary>
    [System.Serializable]
    public class ExportConfigData
    {
        public string version;
        public string exportTime;
        public List<ExportTemplateItem> templates = new List<ExportTemplateItem>();
    }

    /// <summary>
    /// 导出模板项
    /// </summary>
    [System.Serializable]
    public class ExportTemplateItem
    {
        public string templateId;
        public string templateName;
        public string type;
        public string imagePath;
        public string animName;
        public bool loop;
        public float speed;
        public bool interactable;
        public string interactType;
        public int sortOrder;
        public bool collisionEnabled;
        public string dialogBubble;
        public string facingDirection;
        public bool shadowEnabled;
        public bool isCollectible;
        public float respawnTime;
    }
}
