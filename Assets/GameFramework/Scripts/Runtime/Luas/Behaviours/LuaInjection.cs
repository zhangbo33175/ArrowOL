using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua层GO注入对象
    /// 对于自定义的非Unity对象如果需要序列化处理则必须添加[System.Serializable]标签
    /// </summary>
    [System.Serializable]
    public class LuaInjection
    {
        /// <summary>
        /// 注入对象类型定义（Real，序列化数据）
        /// </summary>
        public enum InjectionType
        {
            GameObject                          = 0,
            LuaBehaviour                        = 1,
            Int32                               = 2,
            Float                               = 3,
            String                              = 4,
            Boolean                             = 5,
            SpriteRenderer                      = 6,
            Tilemap                             = 7,
            UI_Text                             = 8,
            UI_Text_TextMeshPro                 = 9,
            UI_TextPicMixed                     = 10,
            UI_Image                            = 11,
            UI_Button                           = 12,
            UI_Scrollbar                        = 13,
            UI_ScrollRect                       = 14,
            UI_Toggle                           = 15,
            UI_Slider                           = 16,
            UI_Dropdown                         = 17,
            UI_InputField                       = 18,
            UI_Tree                             = 19,
            Canvas                              = 20,
            Camera                              = 21,
            ParticleSystem                      = 22,
            Light                               = 23,
            UI_ListView                         = 24,
            UI_SwitchButton                     = 25,
            UI_Dropdown_TextMeshPro             = 26,
            UI_InputField_TextMeshPro           = 27,
            Transform                           = 28,
            RectTransform                       = 29,
        }

        /// <summary>
        /// 注入对象类型到Lua层的映射类型(Real，序列化数据)
        /// </summary>
        public static string[] LuaInjectionType = {
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
        /// 注入对象类型字符串描述数组(Display，显示数据)
        /// </summary>
        public static string[] DisplayInjectionTypeString = {
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
        /// 注入对象类型字段匹配（显示序列到实际序列的注入类型映射）
        /// </summary>
        public static Dictionary<int, int> InjectionTypeDisplayToRealMapping = new Dictionary<int, int>()
        {
            {0,     (int)InjectionType.Int32},
            {1,     (int)InjectionType.Float},
            {2,     (int)InjectionType.String},
            {3,     (int)InjectionType.Boolean},
            {4,     (int)InjectionType.GameObject},
            {5,     (int)InjectionType.Transform},
            {6,     (int)InjectionType.RectTransform},
            {7,     (int)InjectionType.LuaBehaviour},
            {8,     (int)InjectionType.Canvas},
            {9,     (int)InjectionType.Camera},
            {10,    (int)InjectionType.SpriteRenderer},
            {11,    (int)InjectionType.ParticleSystem},
            {12,    (int)InjectionType.Light},
            {13,    (int)InjectionType.Tilemap},
            {14,    (int)InjectionType.UI_Text},
            {15,    (int)InjectionType.UI_TextPicMixed},
            {16,    (int)InjectionType.UI_Image},
            {17,    (int)InjectionType.UI_Button},
            {18,    (int)InjectionType.UI_Scrollbar},
            {19,    (int)InjectionType.UI_ScrollRect},
            {20,    (int)InjectionType.UI_Toggle},
            {21,    (int)InjectionType.UI_Slider},
            {22,    (int)InjectionType.UI_Dropdown},
            {23,    (int)InjectionType.UI_InputField},
            {24,    (int)InjectionType.UI_ListView},
            {25,    (int)InjectionType.UI_Tree},
            {26,    (int)InjectionType.UI_SwitchButton},
            {27,    (int)InjectionType.UI_Text_TextMeshPro},
            {28,    (int)InjectionType.UI_Dropdown_TextMeshPro},
            {29,    (int)InjectionType.UI_InputField_TextMeshPro},
        };

        /// <summary>
        /// 注入对象类型字段匹配（实际序列到显示序列的注入类型映射）
        /// </summary>
        public static Dictionary<int, int> InjectionTypeRealToDisplayMapping = new Dictionary<int, int>()
        {
            {(int)InjectionType.Int32,                          0},
            {(int)InjectionType.Float,                          1},
            {(int)InjectionType.String,                         2},
            {(int)InjectionType.Boolean,                        3},
            {(int)InjectionType.GameObject,                     4},
            {(int)InjectionType.Transform,                      5},
            {(int)InjectionType.RectTransform,                  6},
            {(int)InjectionType.LuaBehaviour,                   7},
            {(int)InjectionType.Canvas,                         8},
            {(int)InjectionType.Camera,                         9},
            {(int)InjectionType.SpriteRenderer,                 10},
            {(int)InjectionType.ParticleSystem,                 11},
            {(int)InjectionType.Light,                          12},
            {(int)InjectionType.Tilemap,                        13},
            {(int)InjectionType.UI_Text,                        14},
            {(int)InjectionType.UI_TextPicMixed,                15},
            {(int)InjectionType.UI_Image,                       16},
            {(int)InjectionType.UI_Button,                      17},
            {(int)InjectionType.UI_Scrollbar,                   18},
            {(int)InjectionType.UI_ScrollRect,                  19},
            {(int)InjectionType.UI_Toggle,                      20},
            {(int)InjectionType.UI_Slider,                      21},
            {(int)InjectionType.UI_Dropdown,                    22},
            {(int)InjectionType.UI_InputField,                  23},
            {(int)InjectionType.UI_ListView,                    24},
            {(int)InjectionType.UI_Tree,                        25},
            {(int)InjectionType.UI_SwitchButton,                26},
            {(int)InjectionType.UI_Text_TextMeshPro,            27},
            {(int)InjectionType.UI_Dropdown_TextMeshPro,        28},
            {(int)InjectionType.UI_InputField_TextMeshPro,      29},
        };

        /// <summary>
        /// 注入对象注释信息
        /// </summary>
        public string Comment;

        /// <summary>
        /// 注入对象类型
        /// </summary>
        public InjectionType InjectionTypeName;

        /// <summary>
        /// 注入对象名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 是否为数组
        /// </summary>
        public bool IsArray;

        // -----------------------------------------------------------------------------

        /// <summary>
        /// 注入对象（Unity对象时有效）
        /// </summary>
        public Object Obj;

        /// <summary>
        /// 注入变量（基础类型时有效）
        /// </summary>
        public string Variant;

        /// <summary>
        /// 注入辅助信息
        /// </summary>
        public string InfoEx;

        /// <summary>
        /// 启用扩展信息集合
        /// </summary>
        public bool ExtendsEnabled;

        /// <summary>
        /// 扩展信息集合
        /// 格式：XXX#YYY#ZZZ
        /// </summary>
        public string Extends;

        // -----------------------------------------------------------------------------

        /// <summary>
        /// 注入的数组元素对象（自身为数组时有效）
        /// </summary>
        public List<Object> ElementsObjs;

        /// <summary>
        /// 注入的数组元素变量（自身为数组时有效）
        /// </summary>
        public List<string> ElementsVariants;

        /// <summary>
        /// 注入的数组元素辅助信息（自身为数组时有效）
        /// </summary>
        public List<string> ElementsInfoExs;

        /// <summary>
        /// 数组元素的启用扩展信息集合（自身为数组时有效）
        /// </summary>
        public List<bool> ElementsExtendsEnableds;

        /// <summary>
        /// 数组元素的扩展信息集合（自身为数组时有效）
        /// 格式：XXX#YYY#ZZZ
        /// </summary>
        public List<string> ElementsExtends;

        // --------------------------------------------------------------------------------

        /// <summary>
        /// 获取扩展信息的段数
        /// </summary>
        /// <returns></returns>
        public int GetExtendsCount()
        {
            return Extends.Split('#').Length;
        }

        /// <summary>
        /// 获取扩展信息的指定段的内容
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetExtendsInfo(int index)
        {
            string[] infos = Extends.Split('#');
            if (index >= 0 && index < infos.Length)
            {
                return infos[index];
            }
            return null;
        }

        /// <summary>
        /// 获取数组元素的扩展信息集合中指定索引位置的信息段数
        /// </summary>
        /// <returns></returns>
        public int GetExtendsCountAtElementIndex(int elementIndex)
        {
            if(elementIndex >= 0 && elementIndex < ElementsExtends.Count)
            {
                return ElementsExtends[elementIndex].Split('#').Length;
            }
            return -1;
        }

        /// <summary>
        /// 获取扩展信息的指定段的内容
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetExtendsInfoAtElementIndex(int elementIndex, int index)
        {
            if (elementIndex >= 0 && elementIndex < ElementsExtends.Count)
            {
                string[] infos = ElementsExtends[elementIndex].Split('#');
                if (index >= 0 && index < infos.Length)
                {
                    return infos[index];
                }
            }
            return null;
        }

    }

}


