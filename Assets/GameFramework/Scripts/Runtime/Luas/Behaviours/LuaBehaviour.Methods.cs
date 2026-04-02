using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 初始化Lua环境
        /// </summary>
        /// <param name="index">Lua脚本独立环境数组下标</param>
        private void InitLuaEnv(int index)
        {
            // 实例化Lua脚本的独立环境
            OwnLuaEnvs[index] = m_LuaComponent.Env.NewTable();
            LuaTable meta = m_LuaComponent.Env.NewTable();
            meta.Set("__index", m_LuaComponent.Env.Global);
            OwnLuaEnvs[index].SetMetaTable(meta);
            meta.Dispose();

            // 向Lua脚本独立环境中注入lua对象（自身lua环境为lua）
            OwnLuaEnvs[index].Set("lua", OwnLuaEnvs[index]);
            OwnLuaEnvs[index].Set("cs", this);

            // 向Lua脚本独立环境中注入所有必需对象
            List<Object> objs = new List<Object>();
            List<string> variants = new List<string>();
            List<object> keys = new List<object>();
            List<string> infoExs = new List<string>();

            for (int injectionIndex = 0; injectionIndex < m_Injections.Count; injectionIndex++)
            {
                LuaInjection injection = m_Injections[injectionIndex];
                bool isArray = injection.IsArray;
                LuaTable luaEnv = isArray ? m_LuaComponent.Env.NewTable() : OwnLuaEnvs[index];

                objs.Clear();
                variants.Clear();
                keys.Clear();
                infoExs.Clear();

                if (injection.IsArray)
                {
                    for(int idx = 0; idx < injection.ElementsObjs.Count; idx++)
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

                if (injection.InjectionTypeName == LuaInjection.InjectionType.GameObject)
                {
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
                else if (injection.InjectionTypeName == LuaInjection.InjectionType.LuaBehaviour)
                {
                    for (int idx = 0; idx < objs.Count; idx++)
                    {
                        if (objs[idx])
                        {
                            LuaBehaviour luaBehaviour = ((LuaBehaviour)objs[idx]);
                            if (luaBehaviour == this)
                            {
                                Log.Error("禁止将自身 LuaBehaviour 组件作为注入参数注入脚本！");
                                return;
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
                                    // 保证GameObject active一次，ObjInfo才能触发Awake，未Awake的脚本不能触发OnDestroy，不触发Awake和OnDestroy的情况下引用计数会出错
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
                                    if(isArray)
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
                    }
                }
                else
                {
                    for (int idx = 0; idx < objs.Count; idx++)
                    {
                        switch (injection.InjectionTypeName)
                        {
                            case LuaInjection.InjectionType.Transform: if (objs[idx] != null) luaEnv.Set(keys[idx], (Transform)objs[idx]); break;
                            case LuaInjection.InjectionType.RectTransform: if (objs[idx] != null) luaEnv.Set(keys[idx], (RectTransform)objs[idx]); break;
                            case LuaInjection.InjectionType.SpriteRenderer: if (objs[idx] != null) luaEnv.Set(keys[idx], (SpriteRenderer)objs[idx]); break;
                            case LuaInjection.InjectionType.Tilemap: if (objs[idx] != null) luaEnv.Set(keys[idx], (UnityEngine.Tilemaps.Tilemap)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Text: if (objs[idx] != null) luaEnv.Set(keys[idx], (Text)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Text_TextMeshPro: if (objs[idx] != null) luaEnv.Set(keys[idx], (TextMeshProUGUI)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Dropdown_TextMeshPro: if (objs[idx] != null) luaEnv.Set(keys[idx], (TMP_Dropdown)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_InputField_TextMeshPro: if (objs[idx] != null) luaEnv.Set(keys[idx], (TMP_InputField)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_TextPicMixed: if (objs[idx] != null) luaEnv.Set(keys[idx], (AorTextPicMixed)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Image: if (objs[idx] != null) luaEnv.Set(keys[idx], (Image)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Button: if (objs[idx] != null) luaEnv.Set(keys[idx], (Button)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Scrollbar: if (objs[idx] != null) luaEnv.Set(keys[idx], (Scrollbar)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_ScrollRect: if (objs[idx] != null) luaEnv.Set(keys[idx], (ScrollRect)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Toggle: if (objs[idx] != null) luaEnv.Set(keys[idx], (Toggle)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Slider: if (objs[idx] != null) luaEnv.Set(keys[idx], (Slider)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Dropdown: if (objs[idx] != null) luaEnv.Set(keys[idx], (Dropdown)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_InputField: if (objs[idx] != null) luaEnv.Set(keys[idx], (InputField)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_Tree: if (objs[idx] != null) luaEnv.Set(keys[idx], (AorTree)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_ListView: if (objs[idx] != null) luaEnv.Set(keys[idx], (AorListView)objs[idx]); break;
                            case LuaInjection.InjectionType.UI_SwitchButton: if (objs[idx] != null) luaEnv.Set(keys[idx], (AorSwitchButton)objs[idx]); break;
                            case LuaInjection.InjectionType.Canvas: if (objs[idx] != null) luaEnv.Set(keys[idx], (Canvas)objs[idx]); break;
                            case LuaInjection.InjectionType.Camera: if (objs[idx] != null) luaEnv.Set(keys[idx], (Camera)objs[idx]); break;
                            case LuaInjection.InjectionType.ParticleSystem: if (objs[idx] != null) luaEnv.Set(keys[idx], (ParticleSystem)objs[idx]); break;
                            case LuaInjection.InjectionType.Light: if (objs[idx] != null) luaEnv.Set(keys[idx], (Light)objs[idx]); break;
                            case LuaInjection.InjectionType.Float: luaEnv.Set(keys[idx], System.Convert.ChangeType(variants[idx], System.Type.GetType("System.Single"))); break;
                            default: luaEnv.Set(keys[idx], System.Convert.ChangeType(variants[idx], System.Type.GetType(AorTxt.Format("System.{0}", injection.InjectionTypeName.ToString())))); break;
                        }
                    }
                }

                if(isArray)
                {
                    OwnLuaEnvs[index].Set(injection.Name, luaEnv);
                }
            }
        }

        /// <summary>
        /// Gizmo绘制
        /// </summary>
        private void OnDrawGizmos()
        {
            if(m_ShowRaycastTargetsGizmos)
            {
                // 绘制RaycastTargets对象
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

    }
}


