/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaInjection.cs
 * author:    云毅
 *created:   2026
 * descrip:   Lua 注入配置核心类，支持 Inspector 序列化、数组、组件自动绑定
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 层 GameObject/组件/变量 注入对象
    /// 自定义序列化类必须添加 [System.Serializable]
    /// </summary>
    [System.Serializable]
    public class LuaInjection
    {
        //=========================================================================
        // 注入类型枚举
        //=========================================================================
        /// <summary>
        /// 注入对象类型定义（实际存储类型）
        /// </summary>
        public enum InjectionType
        {
            GameObject                      = 0,
            LuaBehaviour                    = 1,
            Int32                           = 2,
            Float                           = 3,
            String                          = 4,
            Boolean                         = 5,
            SpriteRenderer                  = 6,
            Tilemap                         = 7,
            UI_Text                         = 8,
            UI_Text_TextMeshPro             = 9,
            UI_TextPicMixed                 = 10,
            UI_Image                        = 11,
            UI_Button                       = 12,
            UI_Scrollbar                    = 13,
            UI_ScrollRect                   = 14,
            UI_Toggle                       = 15,
            UI_Slider                       = 16,
            UI_Dropdown                     = 17,
            UI_InputField                   = 18,
            UI_Tree                         = 19,
            Canvas                          = 20,
            Camera                          = 21,
            ParticleSystem                  = 22,
            Light                           = 23,
            UI_ListView                     = 24,
            UI_SwitchButton                 = 25,
            UI_Dropdown_TextMeshPro         = 26,
            UI_InputField_TextMeshPro       = 27,
            Transform                       = 28,
            RectTransform                   = 29,
        }

        //=========================================================================
        // 静态映射表
        //=========================================================================
        /// <summary>
        /// 注入类型 -> Lua 层类型字符串映射
        /// </summary>
        public static readonly string[] LuaInjectionType =
        {
            "UnityEngine.GameObject",
            "Honor.Runtime.LuaBehaviour",
            "number",
            "number",
            "string",
            "boolean",
            "UnityEngine.SpriteRenderer",
            "UnityEngine.Tilemaps.Tilemap",
            "UnityEngine.UI.Text",
            "TMPro.TextMeshProUGUI",
            "Honor.Runtime.TextPicMixed",
            "UnityEngine.UI.Image",
            "UnityEngine.UI.Button",
            "UnityEngine.UI.Scrollbar",
            "UnityEngine.UI.ScrollRect",
            "UnityEngine.UI.Toggle",
            "UnityEngine.UI.Slider",
            "UnityEngine.UI.Dropdown",
            "UnityEngine.UI.InputField",
            "Honor.Runtime.Tree",
            "UnityEngine.Canvas",
            "UnityEngine.Camera",
            "UnityEngine.ParticleSystem",
            "UnityEngine.Light",
            "Honor.Runtime.ListView",
            "Honor.Runtime.SwitchButton",
            "TMPro.TMP_Dropdown",
            "TMPro.TMP_InputField",
            "UnityEngine.Transform",
            "UnityEngine.RectTransform",
        };

        /// <summary>
        /// 编辑器显示用类型名称
        /// </summary>
        public static readonly string[] DisplayInjectionTypeString =
        {
            "Int32",
            "Float",
            "String",
            "Boolean",
            "GameObject",
            "Transform",
            "RectTransform",
            "LuaBehaviour",
            "Canvas",
            "Camera",
            "SpriteRenderer",
            "ParticleSystem",
            "Light",
            "Tilemap",
            "UI_Text",
            "UI_TextPicMixed",
            "UI_Image",
            "UI_Button",
            "UI_Scrollbar",
            "UI_ScrollRect",
            "UI_Toggle",
            "UI_Slider",
            "UI_Dropdown",
            "UI_InputField",
            "UI_ListView",
            "UI_Tree",
            "UI_SwitchButton",
            "TextMeshPro/UI_Text",
            "TextMeshPro/UI_Dropdown",
            "TextMeshPro/UI_InputField",
        };

        /// <summary>
        /// 显示索引 -> 实际类型索引 映射
        /// </summary>
        public static readonly Dictionary<int, int> InjectionTypeDisplayToRealMapping = new()
        {
            { 0, (int)InjectionType.Int32 },
            { 1, (int)InjectionType.Float },
            { 2, (int)InjectionType.String },
            { 3, (int)InjectionType.Boolean },
            { 4, (int)InjectionType.GameObject },
            { 5, (int)InjectionType.Transform },
            { 6, (int)InjectionType.RectTransform },
            { 7, (int)InjectionType.LuaBehaviour },
            { 8, (int)InjectionType.Canvas },
            { 9, (int)InjectionType.Camera },
            { 10, (int)InjectionType.SpriteRenderer },
            { 11, (int)InjectionType.ParticleSystem },
            { 12, (int)InjectionType.Light },
            { 13, (int)InjectionType.Tilemap },
            { 14, (int)InjectionType.UI_Text },
            { 15, (int)InjectionType.UI_TextPicMixed },
            { 16, (int)InjectionType.UI_Image },
            { 17, (int)InjectionType.UI_Button },
            { 18, (int)InjectionType.UI_Scrollbar },
            { 19, (int)InjectionType.UI_ScrollRect },
            { 20, (int)InjectionType.UI_Toggle },
            { 21, (int)InjectionType.UI_Slider },
            { 22, (int)InjectionType.UI_Dropdown },
            { 23, (int)InjectionType.UI_InputField },
            { 24, (int)InjectionType.UI_ListView },
            { 25, (int)InjectionType.UI_Tree },
            { 26, (int)InjectionType.UI_SwitchButton },
            { 27, (int)InjectionType.UI_Text_TextMeshPro },
            { 28, (int)InjectionType.UI_Dropdown_TextMeshPro },
            { 29, (int)InjectionType.UI_InputField_TextMeshPro },
        };

        /// <summary>
        /// 实际类型索引 -> 显示索引 映射
        /// </summary>
        public static readonly Dictionary<int, int> InjectionTypeRealToDisplayMapping = new()
        {
            { (int)InjectionType.Int32, 0 },
            { (int)InjectionType.Float, 1 },
            { (int)InjectionType.String, 2 },
            { (int)InjectionType.Boolean, 3 },
            { (int)InjectionType.GameObject, 4 },
            { (int)InjectionType.Transform, 5 },
            { (int)InjectionType.RectTransform, 6 },
            { (int)InjectionType.LuaBehaviour, 7 },
            { (int)InjectionType.Canvas, 8 },
            { (int)InjectionType.Camera, 9 },
            { (int)InjectionType.SpriteRenderer, 10 },
            { (int)InjectionType.ParticleSystem, 11 },
            { (int)InjectionType.Light, 12 },
            { (int)InjectionType.Tilemap, 13 },
            { (int)InjectionType.UI_Text, 14 },
            { (int)InjectionType.UI_TextPicMixed, 15 },
            { (int)InjectionType.UI_Image, 16 },
            { (int)InjectionType.UI_Button, 17 },
            { (int)InjectionType.UI_Scrollbar, 18 },
            { (int)InjectionType.UI_ScrollRect, 19 },
            { (int)InjectionType.UI_Toggle, 20 },
            { (int)InjectionType.UI_Slider, 21 },
            { (int)InjectionType.UI_Dropdown, 22 },
            { (int)InjectionType.UI_InputField, 23 },
            { (int)InjectionType.UI_ListView, 24 },
            { (int)InjectionType.UI_Tree, 25 },
            { (int)InjectionType.UI_SwitchButton, 26 },
            { (int)InjectionType.UI_Text_TextMeshPro, 27 },
            { (int)InjectionType.UI_Dropdown_TextMeshPro, 28 },
            { (int)InjectionType.UI_InputField_TextMeshPro, 29 },
        };

        //=========================================================================
        // 序列化基础字段
        //=========================================================================
        /// <summary>
        /// 注释说明（编辑器用）
        /// </summary>
        public string Comment;

        /// <summary>
        /// 注入对象类型
        /// </summary>
        public InjectionType InjectionTypeName;

        /// <summary>
        /// 注入变量名（Lua 层使用）
        /// </summary>
        public string Name;

        /// <summary>
        /// 是否为数组注入
        /// </summary>
        public bool IsArray;

        //=========================================================================
        // 普通注入数据
        //=========================================================================
        /// <summary>
        /// 注入 Unity 对象
        /// </summary>
        public Object Obj;

        /// <summary>
        /// 基础类型变量值
        /// </summary>
        public string Variant;

        /// <summary>
        /// 辅助配置信息
        /// </summary>
        public string InfoEx;

        /// <summary>
        /// 是否启用扩展信息
        /// </summary>
        public bool ExtendsEnabled;

        /// <summary>
        /// 扩展信息（# 分隔）
        /// </summary>
        public string Extends;

        //=========================================================================
        // 数组注入数据
        //=========================================================================
        /// <summary>
        /// 数组元素对象集合
        /// </summary>
        public List<Object> ElementsObjs;

        /// <summary>
        /// 数组元素变量值集合
        /// </summary>
        public List<string> ElementsVariants;

        /// <summary>
        /// 数组元素辅助信息集合
        /// </summary>
        public List<string> ElementsInfoExs;

        /// <summary>
        /// 数组元素扩展启用标记
        /// </summary>
        public List<bool> ElementsExtendsEnableds;

        /// <summary>
        /// 数组元素扩展信息集合
        /// </summary>
        public List<string> ElementsExtends;

        //=========================================================================
        // 扩展信息工具方法
        //=========================================================================
        /// <summary>
        /// 获取当前扩展信息分段数量
        /// </summary>
        public int GetExtendsCount()
        {
            return string.IsNullOrEmpty(Extends) ? 0 : Extends.Split('#').Length;
        }

        /// <summary>
        /// 获取指定索引的扩展信息
        /// </summary>
        public string GetExtendsInfo(int index)
        {
            if (string.IsNullOrEmpty(Extends))
                return null;
            
            string[] infos = Extends.Split('#');
            return index >= 0 && index < infos.Length ? infos[index] : null;
        }

        /// <summary>
        /// 获取数组指定元素的扩展信息段数
        /// </summary>
        public int GetExtendsCountAtElementIndex(int elementIndex)
        {
            if (elementIndex < 0 || elementIndex >= ElementsExtends.Count)
                return -1;
            
            var str = ElementsExtends[elementIndex];
            return string.IsNullOrEmpty(str) ? 0 : str.Split('#').Length;
        }

        /// <summary>
        /// 获取数组指定元素的指定段扩展信息
        /// </summary>
        public string GetExtendsInfoAtElementIndex(int elementIndex, int index)
        {
            if (elementIndex < 0 || elementIndex >= ElementsExtends.Count)
                return null;

            var str = ElementsExtends[elementIndex];
            if (string.IsNullOrEmpty(str))
                return null;

            string[] infos = str.Split('#');
            return index >= 0 && index < infos.Length ? infos[index] : null;
        }
    }
}