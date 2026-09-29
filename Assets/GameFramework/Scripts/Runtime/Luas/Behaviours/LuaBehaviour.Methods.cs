/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviour.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaBehaviour - 注入初始化 & Gizmos 绘制
 ***************************************************************/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// LuaBehaviour - 注入初始化 & Gizmos 绘制（分部类实现）
    /// </summary>
    /// <remarks>
    /// 本文件承载两部分能力：
    /// 1. Lua 独立环境初始化与变量注入（InitLuaEnv）；
    /// 2. 编辑器 Gizmos 调试绘制（OnDrawGizmos）。
    /// 全部逻辑与对外行为与原实现保持一致。
    /// </remarks>
    public partial class LuaBehaviour : MonoBehaviour
    {
        //=========================================================================
        // Lua 环境初始化与注入
        //=========================================================================
        #region Lua Env Initialize & Injection

        /// <summary>
        /// 初始化 Lua 独立环境并执行变量注入
        /// </summary>
        /// <param name="index">Lua 脚本独立环境数组下标</param>
        /// <remarks>
        /// 流程：
        /// 1. 为指定下标创建独立 Lua Table，并以全局表为元表（__index 指向 Env.Global）；
        /// 2. 向独立环境注入自身引用（lua / cs）；
        /// 3. 遍历所有注入项，按注入类型（GameObject / LuaBehaviour / 组件与基础类型）写入 Lua 环境。
        /// </remarks>
        private void InitLuaEnv(int index)
        {
            BuildOwnLuaEnv(index);

            // 临时容器（每轮注入前清空复用，避免频繁分配）
            List<Object> objs = new List<Object>();
            List<string> variants = new List<string>();
            List<object> keys = new List<object>();
            List<string> infoExs = new List<string>();

            // 遍历所有注入项
            for (int injectionIndex = 0; injectionIndex < m_Injections.Count; injectionIndex++)
            {
                LuaInjection injection = m_Injections[injectionIndex];
                bool isArray = injection.IsArray;
                LuaTable luaEnv = isArray ? m_LuaComponent.Env.NewTable() : OwnLuaEnvs[index];

                objs.Clear();
                variants.Clear();
                keys.Clear();
                infoExs.Clear();

                FillInjectionElements(injection, isArray, objs, variants, keys, infoExs);

                // 按注入类型分发到对应处理器
                if (injection.InjectionTypeName == LuaInjection.InjectionType.GameObject)
                {
                    ApplyGameObjectInjection(luaEnv, objs, keys, infoExs);
                }
                else if (injection.InjectionTypeName == LuaInjection.InjectionType.LuaBehaviour)
                {
                    if (ApplyLuaBehaviourInjection(injection, isArray, luaEnv, objs, keys, infoExs))
                    {
                        return;
                    }
                }
                else
                {
                    ApplyComponentInjection(injection, luaEnv, objs, keys, variants);
                }

                if (isArray)
                {
                    OwnLuaEnvs[index].Set(injection.Name, luaEnv);
                }
            }
        }

        /// <summary>
        /// 创建当前下标的独立Lua环境：以全局表为元表并注入自身引用
        /// </summary>
        /// <param name="index">Lua 脚本独立环境数组下标</param>
        private void BuildOwnLuaEnv(int index)
        {
            // 实例化 Lua 独立环境
            OwnLuaEnvs[index] = m_LuaComponent.Env.NewTable();
            LuaTable meta = m_LuaComponent.Env.NewTable();
            meta.Set("__index", m_LuaComponent.Env.Global);
            OwnLuaEnvs[index].SetMetaTable(meta);
            meta.Dispose();

            // 注入自身环境
            OwnLuaEnvs[index].Set("lua", OwnLuaEnvs[index]);
            OwnLuaEnvs[index].Set("cs", this);
        }

        /// <summary>
        /// 按数组/单条方式填充注入元素到临时容器
        /// </summary>
        /// <param name="injection">注入项数据</param>
        /// <param name="isArray">是否为数组注入</param>
        /// <param name="objs">对象列表输出</param>
        /// <param name="variants">基础类型值列表输出</param>
        /// <param name="keys">键列表输出</param>
        /// <param name="infoExs">附加信息列表输出</param>
        private void FillInjectionElements(LuaInjection injection, bool isArray, List<Object> objs, List<string> variants,
            List<object> keys, List<string> infoExs)
        {
            // 填充数据：数组注入按元素逐个填充，普通注入填充单条
            if (isArray)
            {
                for (int idx = 0; idx < injection.ElementsObjs.Count; idx++)
                {
                    objs.Add(injection.ElementsObjs[idx]);
                    variants.Add(injection.ElementsVariants[idx]);
                    keys.Add(idx + 1);
                    infoExs.Add(injection.ElementsInfoExs[idx]);
                }
            }
            else
            {
                objs.Add(injection.Obj);
                variants.Add(injection.Variant);
                keys.Add(injection.Name);
                infoExs.Add(injection.InfoEx);
            }
        }

        /// <summary>
        /// 注入GameObject：按组件名取组件，未指定时直接注入GameObject
        /// </summary>
        /// <param name="luaEnv">目标Lua环境</param>
        /// <param name="objs">对象列表</param>
        /// <param name="keys">键列表</param>
        /// <param name="infoExs">附加信息列表</param>
        private void ApplyGameObjectInjection(LuaTable luaEnv, List<Object> objs, List<object> keys, List<string> infoExs)
        {
            // 游戏对象注入：按组件名取组件，未指定组件名时直接注入 GameObject
            for (int idx = 0; idx < objs.Count; idx++)
            {
                if (objs[idx])
                {
                    if (!string.IsNullOrEmpty(infoExs[idx]))
                    {
                        string[] tidyName = infoExs[idx].Split('.');
                        luaEnv.Set(keys[idx], ((GameObject)objs[idx]).GetComponent(tidyName[tidyName.Length - 1]));
                    }
                    else
                    {
                        luaEnv.Set(keys[idx], (GameObject)objs[idx]);
                    }
                }
            }
        }

        /// <summary>
        /// 注入LuaBehaviour：临时激活触发生命周期后注入其luaClass环境
        /// </summary>
        /// <param name="injection">注入项数据</param>
        /// <param name="isArray">是否为数组注入</param>
        /// <param name="luaEnv">目标Lua环境</param>
        /// <param name="objs">对象列表</param>
        /// <param name="keys">键列表</param>
        /// <param name="infoExs">附加信息列表</param>
        /// <returns>是否因注入自身而中止整个初始化流程</returns>
        private bool ApplyLuaBehaviourInjection(LuaInjection injection, bool isArray, LuaTable luaEnv, List<Object> objs,
            List<object> keys, List<string> infoExs)
        {
            // LuaBehaviour 注入：注入其 luaClass 环境，注入前临时激活以触发生命周期回调
            for (int idx = 0; idx < objs.Count; idx++)
            {
                if (!objs[idx]) continue;

                LuaBehaviour luaBehaviour = ((LuaBehaviour)objs[idx]);
                if (luaBehaviour == this)
                {
                    Log.Error("禁止将自身 LuaBehaviour 组件作为注入参数注入脚本！");
                    return true;
                }

                int luaScriptIndex = luaBehaviour.LuaScriptNames.IndexOf(infoExs[idx]);
                if (luaScriptIndex >= 0)
                {
                    bool useTmpParent = false;
                    Transform realParent = luaBehaviour.transform.parent;
                    if (!realParent.gameObject.activeInHierarchy)
                    {
                        luaBehaviour.transform.SetParent(GameMainRoot.Asset.transform, false);
                        useTmpParent = true;
                    }
                    if (!luaBehaviour.gameObject.activeSelf)
                    {
                        luaBehaviour.gameObject.SetActive(true);
                        luaBehaviour.gameObject.SetActive(false);
                    }

                    luaBehaviour.AwakeAppended();
                    luaBehaviour.OnEnableAppended();

                    if (useTmpParent)
                    {
                        luaBehaviour.transform.SetParent(realParent, false);
                    }

                    LuaTable luaTable = luaBehaviour.OwnLuaClasses[luaScriptIndex];
                    if (luaTable != null)
                    {
                        luaEnv.Set(keys[idx], luaTable);
                    }
                    else
                    {
                        if (isArray)
                        {
                            Log.Error("注入对象 {0}[{1}] 还未初始化luaClass脚本 {2} ！", injection.Name, (int)keys[idx], infoExs[idx]);
                        }
                        else
                        {
                            Log.Error("注入对象 {0} 还未初始化luaClass脚本 {1} ！", injection.Name, infoExs[idx]);
                        }
                    }
                }
                else
                {
                    if (isArray)
                    {
                        Log.Error("注入对象 {0}[{1}] 没有找到合法的lua脚本！", injection.Name, (int)keys[idx]);
                    }
                    else
                    {
                        Log.Error("注入对象 {0} 没有找到合法的lua脚本！", injection.Name);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 注入组件与基础类型：按注入类型转换后写入Lua环境
        /// </summary>
        /// <param name="injection">注入项数据</param>
        /// <param name="luaEnv">目标Lua环境</param>
        /// <param name="objs">对象列表</param>
        /// <param name="keys">键列表</param>
        /// <param name="variants">基础类型值列表</param>
        private void ApplyComponentInjection(LuaInjection injection, LuaTable luaEnv, List<Object> objs, List<object> keys,
            List<string> variants)
        {
            // 组件 & 基础类型注入：按注入类型转换为对应 Unity / 基础类型后写入 Lua 环境
            for (int idx = 0; idx < objs.Count; idx++)
            {
                switch (injection.InjectionTypeName)
                {
                    case LuaInjection.InjectionType.Float:
                        luaEnv.Set(keys[idx], System.Convert.ChangeType(variants[idx], System.Type.GetType("System.Single")));
                        break;
                    case LuaInjection.InjectionType.Transform:
                    case LuaInjection.InjectionType.RectTransform:
                    case LuaInjection.InjectionType.SpriteRenderer:
                    case LuaInjection.InjectionType.Tilemap:
                    case LuaInjection.InjectionType.Canvas:
                    case LuaInjection.InjectionType.Camera:
                    case LuaInjection.InjectionType.ParticleSystem:
                    case LuaInjection.InjectionType.Light:
                        ApplySceneComponentInjection(injection, luaEnv, objs, keys, idx);
                        break;
                    case LuaInjection.InjectionType.UI_Text:
                    case LuaInjection.InjectionType.UI_Text_TextMeshPro:
                    case LuaInjection.InjectionType.UI_Dropdown_TextMeshPro:
                    case LuaInjection.InjectionType.UI_InputField_TextMeshPro:
                    case LuaInjection.InjectionType.UI_TextPicMixed:
                    case LuaInjection.InjectionType.UI_Image:
                    case LuaInjection.InjectionType.UI_Button:
                    case LuaInjection.InjectionType.UI_Scrollbar:
                    case LuaInjection.InjectionType.UI_ScrollRect:
                    case LuaInjection.InjectionType.UI_Toggle:
                    case LuaInjection.InjectionType.UI_Slider:
                    case LuaInjection.InjectionType.UI_Dropdown:
                    case LuaInjection.InjectionType.UI_InputField:
                    case LuaInjection.InjectionType.UI_Tree:
                    case LuaInjection.InjectionType.UI_ListView:
                    case LuaInjection.InjectionType.UI_SwitchButton:
                        ApplyUIComponentInjection(injection, luaEnv, objs, keys, idx);
                        break;
                    default:
                        luaEnv.Set(keys[idx], System.Convert.ChangeType(variants[idx], System.Type.GetType(AorTxt.Format("System.{0}", injection.InjectionTypeName.ToString()))));
                        break;
                }
            }
        }

        /// <summary>
        /// 场景类组件（Transform/RectTransform/Renderer/Canvas/Camera等）类型转换并写入Lua环境
        /// </summary>
        /// <param name="injection">注入项数据</param>
        /// <param name="luaEnv">目标Lua环境</param>
        /// <param name="objs">对象列表</param>
        /// <param name="keys">键列表</param>
        /// <param name="idx">当前元素下标</param>
        private void ApplySceneComponentInjection(LuaInjection injection, LuaTable luaEnv, List<Object> objs, List<object> keys, int idx)
        {
            switch (injection.InjectionTypeName)
            {
                case LuaInjection.InjectionType.Transform:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Transform)objs[idx]);
                    break;
                case LuaInjection.InjectionType.RectTransform:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (RectTransform)objs[idx]);
                    break;
                case LuaInjection.InjectionType.SpriteRenderer:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (SpriteRenderer)objs[idx]);
                    break;
                case LuaInjection.InjectionType.Tilemap:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (UnityEngine.Tilemaps.Tilemap)objs[idx]);
                    break;
                case LuaInjection.InjectionType.Canvas:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Canvas)objs[idx]);
                    break;
                case LuaInjection.InjectionType.Camera:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Camera)objs[idx]);
                    break;
                case LuaInjection.InjectionType.ParticleSystem:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (ParticleSystem)objs[idx]);
                    break;
                case LuaInjection.InjectionType.Light:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Light)objs[idx]);
                    break;
            }
        }

        /// <summary>
        /// UI类组件（Text/Image/Button/控件等）类型转换并写入Lua环境
        /// </summary>
        /// <param name="injection">注入项数据</param>
        /// <param name="luaEnv">目标Lua环境</param>
        /// <param name="objs">对象列表</param>
        /// <param name="keys">键列表</param>
        /// <param name="idx">当前元素下标</param>
        private void ApplyUIComponentInjection(LuaInjection injection, LuaTable luaEnv, List<Object> objs, List<object> keys, int idx)
        {
            switch (injection.InjectionTypeName)
            {
                case LuaInjection.InjectionType.UI_Text:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Text)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Text_TextMeshPro:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (TextMeshProUGUI)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Dropdown_TextMeshPro:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (TMP_Dropdown)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_InputField_TextMeshPro:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (TMP_InputField)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_TextPicMixed:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (AorTextPicMixed)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Image:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Image)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Button:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Button)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Scrollbar:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Scrollbar)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_ScrollRect:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (ScrollRect)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Toggle:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Toggle)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Slider:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Slider)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Dropdown:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (Dropdown)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_InputField:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (InputField)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_Tree:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (AorTree)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_ListView:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (AorListView)objs[idx]);
                    break;
                case LuaInjection.InjectionType.UI_SwitchButton:
                    if (objs[idx] != null) luaEnv.Set(keys[idx], (AorSwitchButton)objs[idx]);
                    break;
            }
        }

        #endregion

        //=========================================================================
        // Gizmos 绘制
        //=========================================================================
        #region Gizmos

        /// <summary>
        /// 编辑器 Gizmos 绘制：显示所有 RaycastTarget 对象的包围框（调试用）
        /// </summary>
        /// <remarks>
        /// 仅在 m_ShowRaycastTargetsGizmos 开启时生效，
        /// 遍历子物体 MaskableGraphic，对开启 raycastTarget 的对象绘制世界坐标矩形框。
        /// </remarks>
        private void OnDrawGizmos()
        {
            if (m_ShowRaycastTargetsGizmos)
            {
                // 绘制 RaycastTargets 对象
                MaskableGraphic[] graphics = GetComponentsInChildren<MaskableGraphic>();
                foreach (MaskableGraphic graphic in graphics)
                {
                    if (graphic.raycastTarget)
                    {
                        RectTransform rectTransform = graphic.transform as RectTransform;
                        rectTransform.GetWorldCorners(m_RaycastTargetWorldCornersOnDrawGizmos);
                        Gizmos.color = m_RaycastTargetsGizmosColor;
                        for (int i = 0; i < 4; i++)
                        {
                            Gizmos.DrawLine(m_RaycastTargetWorldCornersOnDrawGizmos[i], m_RaycastTargetWorldCornersOnDrawGizmos[(i + 1) % 4]);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
