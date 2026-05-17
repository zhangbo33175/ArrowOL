/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIManager.Utils.cs
 * author:  云毅
 * created: 2026
 * descrip:   UI 管理器 - 私有工具方法与内部逻辑
 ***************************************************************/
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class UIManager
    {
        #region 帧更新逻辑
        /// <summary>
        /// 当前模态 UI 帧更新（心跳）
        /// 执行启用 UseProc 的 Lua 逻辑更新
        /// </summary>
        private void UpdateCurModalUI()
        {
            if (m_CurModalUI != null && m_CurModalUI.LuaBehaviour != null && m_CurModalUI.LuaBehaviour.UseProc && m_CurModalUI.gameObject.activeSelf)
            {
                m_CurModalUI.LuaBehaviour.Proc();
            }
        }

        /// <summary>
        /// 模态 UI 等待队列帧更新
        /// 阻塞状态下不处理；当前模态 UI 关闭后自动弹出下一个
        /// </summary>
        private void UpdateModalUIInfoList()
        {
            if (m_BlockModalUIsSwitch) return;

            if (m_CurModalUI == null)
            {
                if (m_ModalUIInfoList.Count > 0)
                {
                    UIInfo modalUIInfo = m_ModalUIInfoList[0];
                    m_ModalUIInfoList.RemoveAt(0);

                    // 有回调 → 异步打开
                    if (modalUIInfo.OverCallback != null)
                    {
                        OpenUIAsyncByInfo(modalUIInfo);
                    }
                    // 无回调 → 同步打开
                    else
                    {
                        OpenUISyncByInfo(modalUIInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 非模态 UI 列表帧更新
        /// </summary>
        private void UpdateUnModalUIList()
        {
            for (int index = 0; index < m_UnModalUIList.Count; index++)
            {
                UIFlagBehaviour ui = m_UnModalUIList[index];
                if (ui != null && ui.LuaBehaviour != null && ui.LuaBehaviour.UseProc && ui.gameObject.activeSelf)
                {
                    ui.LuaBehaviour.Proc();
                }
            }
        }

        /// <summary>
        /// 场景 UI 列表帧更新
        /// </summary>
        private void UpdateSceneUIList()
        {
            for (int index = 0; index < m_SceneUIList.Count; index++)
            {
                UIFlagBehaviour ui = m_SceneUIList[index];
                if (ui != null && ui.LuaBehaviour != null && ui.LuaBehaviour.UseProc && ui.gameObject.activeSelf)
                {
                    ui.LuaBehaviour.Proc();
                }
            }
        }

        /// <summary>
        /// 附加/子 UI 列表帧更新
        /// </summary>
        private void UpdateSubUIList(UIType uiType)
        {
            if (m_SubUIList == null || !m_SubUIList.ContainsKey(uiType)) return;

            for (int index = 0; index < m_SubUIList[uiType].Count; index++)
            {
                UIFlagBehaviour ui = m_SubUIList[uiType][index];
                if (ui != null && ui.LuaBehaviour != null && ui.LuaBehaviour.UseProc && ui.gameObject.activeSelf)
                {
                    ui.LuaBehaviour.Proc();
                }
            }
        }

        /// <summary>
        /// 分帧销毁待卸载 UI
        /// 限制每帧最大数量，避免卡顿
        /// </summary>
        private void UpdateDestroyUIList()
        {
            int count = 0;
            for (int index = m_UnloadUIList.Count - 1; index >= 0; index--)
            {
                if (index >= m_UnloadUIList.Count) continue;
                var flag = m_UnloadUIList[index];
                if (flag == null)
                {
                    m_UnloadUIList.RemoveAt(index);
                    continue;
                }

                InnerDestroyUIByFlag(flag);
                if (++count >= m_DestroyMaxNumPerFrame)
                {
                    break;
                }
            }
        }
        #endregion

        #region UI 创建辅助
        /// <summary>
        /// 为 WebGL 平台自动添加输入框兼容组件
        /// </summary>
        private void AddWebGLInput(GameObject go)
        {
            if (go == null) return;

#if UNITY_WEBGL && !UNITY_EDITOR
            var inputFields = go.GetComponentsInChildren<InputField>(true);
            foreach (var inputField in inputFields)
            {
                if (inputField != null && inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                {
                    inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                }
            }
#endif
        }

        /// <summary>
        /// 为新创建的 UI 添加 Canvas 相关组件
        /// </summary>
        private void AddCanvas(GameObject go, UIInfo uiInfo)
        {
            if (go == null || uiInfo == null) return;

            Canvas canvas = go.AddComponent<Canvas>();
            if (canvas != null)
            {
                canvas.overrideSorting = true;
                canvas.sortingLayerName = uiInfo.UIType == UIType.Scene ? "Default" : "UI";
                canvas.sortingOrder = uiInfo.ZOrder;
            }

            GraphicRaycaster graphicRaycaster = go.AddComponent<GraphicRaycaster>();
            if (graphicRaycaster != null)
            {
                graphicRaycaster.blockingMask = uiInfo.BlockingMaskValue;
                graphicRaycaster.blockingObjects = uiInfo.BlockingObjects;
            }

            go.AddComponent<CanvasGroup>();
        }

        /// <summary>
        /// 添加 UI 标记组件
        /// </summary>
        private UIFlagBehaviour AddFlagBehaviour(GameObject go, UIInfo uiInfo)
        {
            if (go == null) return null;

            UIFlagBehaviour uiFlagBehaviour = go.GetOrAddComponent<UIFlagBehaviour>();
            uiFlagBehaviour.LuaBehaviour = go.GetComponent<LuaBehaviour>();
            uiFlagBehaviour.PrefabInstanceGOBehaviour = go.GetComponent<PrefabInstanceGOBehaviour>();
            uiFlagBehaviour.UIInfo = uiInfo;
            return uiFlagBehaviour;
        }

        /// <summary>
        /// 为子对象自动添加 UI 标记
        /// </summary>
        private List<UIFlagBehaviour> AddChildrenFlagBehaviours(GameObject go)
        {
            List<UIFlagBehaviour> childrenFlagBehaviours = new List<UIFlagBehaviour>();
            if (go == null) return childrenFlagBehaviours;

            List<LuaBehaviour> childrenLuaBehaviour = new List<LuaBehaviour>();
            go.GetComponentsInChildren(true, childrenLuaBehaviour);

            var rootFlag = go.GetComponent<UIFlagBehaviour>();
            if (rootFlag != null)
            {
                childrenLuaBehaviour.Remove(rootFlag.LuaBehaviour);
            }

            foreach (var child in childrenLuaBehaviour)
            {
                if (child != null && child.PrefabType == PrefabType.UI)
                {
                    UIFlagBehaviour childFlagBehaviour = child.GetOrAddComponent<UIFlagBehaviour>();
                    childFlagBehaviour.LuaBehaviour = child;
                    childFlagBehaviour.PrefabInstanceGOBehaviour = null;
                    childFlagBehaviour.UIInfo = null;
                    childrenFlagBehaviours.Add(childFlagBehaviour);
                }
            }
            return childrenFlagBehaviours;
        }

        /// <summary>
        /// UI 异步创建完成回调
        /// </summary>
        private void DoUICreateOverCallbackOnAsync(GameObject go, UIInfo uiInfo, PrefabObject prefabObject)
        {
            if (uiInfo == null || uiInfo.OverCallback == null || go == null || prefabObject == null)
                return;

            LuaTable validLuaClass = null;
            LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
            if (luaBehaviour != null)
            {
                validLuaClass = luaBehaviour.ValidLuaClass;
            }
            uiInfo.OverCallback(prefabObject.AssetBundlePath, prefabObject.AssetName, validLuaClass, go);
        }
        #endregion

        #region 输入与按键
        /// <summary>
        /// 检测全局按键抬起事件
        /// </summary>
        private void CheckKeysUp()
        {
            if (m_BlockAllUIsKeyUpSwitch) return;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                LuaComponent luaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
                if (luaComponent != null && luaComponent.LuaKeysUpFromCSEventDelegate != null)
                {
                    List<LuaBehaviour> luaBehaviours = new List<LuaBehaviour>();

                    if (m_CurModalUI != null && m_CurModalUI.LuaBehaviour != null)
                        luaBehaviours.Add(m_CurModalUI.LuaBehaviour);

                    foreach (var ui in m_UnModalUIList)
                    {
                        if (ui != null && ui.LuaBehaviour != null)
                            luaBehaviours.Add(ui.LuaBehaviour);
                    }

                    if (luaBehaviours.Count > 0)
                    {
                        luaBehaviours.Sort((a, b) =>
                        {
                            if (a == null || b == null) return 0;
                            var flagA = a.GetComponent<UIFlagBehaviour>();
                            var flagB = b.GetComponent<UIFlagBehaviour>();
                            if (flagA == null || flagB == null || flagA.UIInfo == null || flagB.UIInfo == null) return 0;
                            return flagA.UIInfo.ZOrder - flagB.UIInfo.ZOrder;
                        });

                        var topLua = luaBehaviours[luaBehaviours.Count - 1];
                        if (topLua != null)
                        {
                            var topFlag = topLua.GetComponent<UIFlagBehaviour>();
                            if (topFlag != null && topFlag.UIInfo != null && topFlag.UIInfo.CloseOnEscapeKeyUp)
                            {
                                topFlag.LuaBehaviour?.CallLuaClose();
                            }
                        }

                        luaComponent.LuaKeysUpFromCSEventDelegate(KeyCode.Escape);
                    }
                }
            }
        }
        #endregion

        #region 匹配与查找工具
        private GameObject GetMatchedGOGameObject(UIFlagBehaviour objectForChecking, UIFlagBehaviour targetFlagBehaviour)
        {
            if (objectForChecking == null || targetFlagBehaviour == null) return null;

            GameObject result = null;
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour == null || behaviour.UIInfo == null || targetFlagBehaviour.UIInfo == null)
                    continue;

                if (behaviour.UIInfo.IsAppend == targetFlagBehaviour.UIInfo.IsAppend && behaviour.gameObject == targetFlagBehaviour.gameObject)
                {
                    result = behaviour.gameObject;
                    break;
                }
            }
            return result;
        }

        private GameObject GetMatchedUIInfoGameObject(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            if (objectForChecking == null || targetUIInfo == null) return null;

            GameObject result = null;
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo))
                {
                    result = behaviour.gameObject;
                    break;
                }
            }
            return result;
        }

        private List<GameObject> GetAllMatchedUIInfoGameObjects(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            List<GameObject> result = new List<GameObject>();
            if (objectForChecking == null || targetUIInfo == null) return result;

            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            uiBehaviours.RemoveAll(behaviour =>
            {
                if (behaviour == null) return true;
                foreach (var other in uiBehaviours)
                {
                    if (other == null) continue;
                    if (behaviour != other && behaviour.transform.IsChildOf(other.transform))
                        return true;
                }
                return false;
            });

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo))
                    result.Add(behaviour.gameObject);
            }
            return result;
        }

        private List<GameObject> GetAllMatchedUnModalGameObjects(UIFlagBehaviour objectForChecking)
        {
            List<GameObject> result = new List<GameObject>();
            if (objectForChecking == null) return result;

            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            uiBehaviours.RemoveAll(behaviour =>
            {
                if (behaviour == null) return true;
                foreach (var other in uiBehaviours)
                {
                    if (other == null) continue;
                    if (behaviour != other && behaviour.transform.IsChildOf(other.transform))
                        return true;
                }
                return false;
            });

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && !behaviour.UIInfo.IsAppend && !behaviour.UIInfo.IsModal)
                    result.Add(behaviour.gameObject);
            }
            return result;
        }

        private List<GameObject> GetAllMatchedSceneUIGameObjects(UIFlagBehaviour objectForChecking)
        {
            List<GameObject> result = new List<GameObject>();
            if (objectForChecking == null) return result;

            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            uiBehaviours.RemoveAll(behaviour =>
            {
                if (behaviour == null) return true;
                foreach (var other in uiBehaviours)
                {
                    if (other == null) continue;
                    if (behaviour != other && behaviour.transform.IsChildOf(other.transform))
                        return true;
                }
                return false;
            });

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && !behaviour.UIInfo.IsAppend && behaviour.UIInfo.UIType == UIType.Scene)
                    result.Add(behaviour.gameObject);
            }
            return result;
        }

        private void RemoveMatchedUIInfoFromModalUIInfoList(UIInfo targetUIInfo)
        {
            if (targetUIInfo == null) return;
            int index = m_ModalUIInfoList.FindIndex(uiInfo => uiInfo != null && uiInfo.Equals(targetUIInfo));
            if (index >= 0)
                m_ModalUIInfoList.RemoveAt(index);
        }

        private void RemoveAllMatchedUIInfosFromModalUIInfoList(UIInfo targetUIInfo)
        {
            if (targetUIInfo == null) return;
            m_ModalUIInfoList.RemoveAll(uiInfo => uiInfo != null && uiInfo.Equals(targetUIInfo));
        }

        private GameObject GetMatchedUIInfoValidGameObject(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            if (objectForChecking == null || targetUIInfo == null) return null;

            GameObject result = null;
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo) && !m_UnloadUIList.Contains(behaviour))
                {
                    result = behaviour.gameObject;
                    break;
                }
            }
            return result;
        }

        private List<GameObject> GetAllMatchedUIInfoValidGameObjects(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            List<GameObject> result = new List<GameObject>();
            if (objectForChecking == null || targetUIInfo == null) return result;

            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo) && !m_UnloadUIList.Contains(behaviour))
                    result.Add(behaviour.gameObject);
            }
            return result;
        }

        private List<GameObject> GetAllMatchedUITypeValidGameObjects(UIFlagBehaviour objectForChecking, UIType targetUIType, bool isAppend)
        {
            List<GameObject> result = new List<GameObject>();
            if (objectForChecking == null) return result;

            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour != null && behaviour.UIInfo != null && behaviour.UIInfo.IsAppend == isAppend && behaviour.UIInfo.UIType == targetUIType && !m_UnloadUIList.Contains(behaviour))
                    result.Add(behaviour.gameObject);
            }
            return result;
        }
        #endregion

        #region 销毁与关闭
        /// <summary>
        /// 内部关闭 UI（统一入口）
        /// </summary>
        private void InnerCloseUIByGO(GameObject go, bool rightNowDestroy)
        {
            if (go == null) return;

            PrefabInstanceGOBehaviour prefabInstance = go.GetComponent<PrefabInstanceGOBehaviour>();
            if (prefabInstance != null)
            {
                UIFlagBehaviour flag = go.GetComponent<UIFlagBehaviour>();
                if (flag == null) return;

                if (rightNowDestroy)
                {
                    InnerDestroyUIByFlag(flag, true);
                }
                else
                {
                    if (flag.LuaBehaviour != null)
                        flag.LuaBehaviour.CallLuaClose();
                    else
                        AddFlagToUnloadUIList(flag);
                }
            }
            else
            {
                GameMainRoot.Asset.PrefabLoadManager.Destroy(go, rightNowDestroy);
            }
        }

        /// <summary>
        /// 内部销毁 UI（真正执行销毁）
        /// </summary>
        private void InnerDestroyUIByFlag(UIFlagBehaviour flagBehaviour, bool destroyImmediate = false)
        {
            if (flagBehaviour == null) return;

            if (flagBehaviour.PrefabInstanceGOBehaviour != null)
            {
                flagBehaviour.PrefabInstanceGOBehaviour.RightNowDestroyOnAsset = true;
            }

            if (m_CurModalUI == flagBehaviour)
            {
                m_CurModalUI = null;
            }
            else
            {
                m_UnModalUIList.Remove(flagBehaviour);
                m_SceneUIList.Remove(flagBehaviour);
            }

            if (m_SubUIList != null)
            {
                foreach (var kvp in m_SubUIList)
                {
                    if (kvp.Value != null && kvp.Value.Contains(flagBehaviour))
                    {
                        kvp.Value.Remove(flagBehaviour);
                        break;
                    }
                }
            }

            m_UnloadUIList.Remove(flagBehaviour);

            if (flagBehaviour.LuaBehaviour != null && flagBehaviour.UIInfo != null && flagBehaviour.UIInfo.IsAppend)
            {
                List<UIFlagBehaviour> parentFlags = new List<UIFlagBehaviour>();
                flagBehaviour.GetComponentsInParent(true, parentFlags);
                parentFlags.Remove(flagBehaviour);
                parentFlags.Sort((a, b) =>
                {
                    if (a == null || b == null || a.transform == null || b.transform == null) return 0;
                    return b.transform.GetRouteNum() - a.transform.GetRouteNum();
                });

                foreach (var parent in parentFlags)
                {
                    if (parent == null || parent.LuaBehaviour == null) continue;

                    LuaTable master = parent.LuaBehaviour.ValidLuaClass;
                    if (master != null)
                    {
                        master.Get("OnAddedUIDestroyed", out LuaFunction func);
                        func?.Action(master, flagBehaviour.LuaBehaviour.ValidLuaClass);
                        func?.Dispose();
                    }
                }
            }

            if (!flagBehaviour.FollowParentDestroy && flagBehaviour.gameObject != null)
            {
                if (destroyImmediate)
                    GameObject.DestroyImmediate(flagBehaviour.gameObject);
                else
                    GameObject.Destroy(flagBehaviour.gameObject);
            }
        }
        #endregion

        #region 字体与多语言
        /// <summary>
        /// 刷新所有文本组件（字体、大小、位置、多语言适配）
        /// </summary>
        private void RefreshTextComponentsAdaptationParams(GameObject go, List<LocalizationFontData> fontDatas)
        {
            if (go == null || fontDatas == null || fontDatas.Count == 0) return;

            if (m_CheckTextLocalizings)
            {
                List<MaskableGraphic> graphics = new List<MaskableGraphic>();
                graphics.AddRange(go.GetComponentsInChildren<Text>());
                graphics.AddRange(go.GetComponentsInChildren<TextMeshProUGUI>());

                foreach (var g in graphics)
                {
                    if (g != null && !g.GetComponent<AorTextLocalizing>())
                    {
                        Log.Error($"多语言组件缺失：{g.transform.GetRoute()}");
                    }
                }
            }

            List<string> fontMarks = new List<string>();
            fontDatas.ForEach(data => { if(data != null) fontMarks.Add(data.Mark); });

            UIInfo uiInfo = go.GetComponent<UIFlagBehaviour>()?.UIInfo;
            if (uiInfo == null) return;

            AorTextLocalizing[] locals = go.GetComponentsInChildren<AorTextLocalizing>(true);
            foreach (var local in locals)
            {
                if (local == null) continue;

                int index = fontMarks.FindIndex(m => m == local.LocalizingFontMark);
                index = Mathf.Clamp(index, 0, fontDatas.Count - 1);

                Text text = local.GetComponent<Text>();
                TextMeshProUGUI tmp = local.GetComponent<TextMeshProUGUI>();

                LocalizationFontData fontData = fontDatas[index];
                if (fontData == null || index >= Fonts.Count) continue;

                UnityEngine.Object font = Fonts[index];
                if (!Enum.TryParse(fontData.FontType, out GameDefinitions.AssetType fontType)) continue;

                float scale = 1;
                Vector2 offset = Vector2.zero;
                if (local.Language != GameDefinitions.Language.Unspecified && local.Language != GameMainRoot.Localization.Language)
                {
                    m_LocalizationComponent.GetFontData(local.Language, out List<LocalizationFontData> lastFonts);
                    if (lastFonts != null && index < lastFonts.Count)
                    {
                        var lastData = lastFonts[index];
                        if (lastData != null && lastData.FontSizeScaleRatio > 0)
                            scale = fontData.FontSizeScaleRatio / lastData.FontSizeScaleRatio;
                    }
                }
                else if (local.Language == GameDefinitions.Language.Unspecified)
                {
                    scale = fontData.FontSizeScaleRatio;
                }

                if (fontType == GameDefinitions.AssetType.Font)
                {
                    if (!uiInfo.MultiTypeTextCompsCoexist && tmp) tmp.enabled = false;
                    if (text != null && font is Font unityFont)
                    {
                        text.enabled = true;
                        text.font = unityFont;
                        if (local.OpenPosOffset)
                        {
                            text.fontSize = Mathf.RoundToInt(text.fontSize * scale);
                            text.resizeTextMinSize = Mathf.RoundToInt(text.resizeTextMinSize * scale);
                            text.resizeTextMaxSize = Mathf.RoundToInt(text.resizeTextMaxSize * scale);
                            text.rectTransform.anchoredPosition += offset;
                        }
                        local.Language = GameMainRoot.Localization.Language;
                    }
                }
                else if (fontType == GameDefinitions.AssetType.FontTMP)
                {
                    if (!uiInfo.MultiTypeTextCompsCoexist && text) text.enabled = false;
                    if (tmp != null && font is TMP_FontAsset tmpFont)
                    {
                        tmp.enabled = true;
                        string sep = "___";
                        Material mat = null;

                        if (tmp.fontSharedMaterial != null && tmp.fontSharedMaterial.name.Contains(sep))
                        {
                            int i = tmp.fontSharedMaterial.name.LastIndexOf(sep);
                            if (i > 0)
                            {
                                string suffix = tmp.fontSharedMaterial.name.Substring(i + sep.Length).Replace("(Instance)", "").Replace(" ", "");
                                if (!string.IsNullOrEmpty(suffix) && !string.IsNullOrEmpty(fontData.ABPath))
                                {
                                    mat = (Material)m_AssetComponent.LoadAssetSync("Material", fontData.ABPath, $"{font.name}{sep}{suffix}");
                                }
                            }
                        }

                        tmp.font = tmpFont;
                        if (mat != null)
                        {
                            tmp.fontMaterial = mat;
                            m_AssetComponent.UnloadAsset(mat);
                        }

                        if (local.OpenPosOffset)
                        {
                            tmp.fontSize = Mathf.RoundToInt(tmp.fontSize * scale);
                            tmp.fontSizeMin = Mathf.RoundToInt(tmp.fontSizeMin * scale);
                            tmp.fontSizeMax = Mathf.RoundToInt(tmp.fontSizeMax * scale);
                            tmp.rectTransform.anchoredPosition += offset;
                        }
                        local.Language = GameMainRoot.Localization.Language;
                    }
                }
            }
        }
        #endregion
    }
}