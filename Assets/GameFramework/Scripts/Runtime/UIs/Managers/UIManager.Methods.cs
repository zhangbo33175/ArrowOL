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
        /// <param name="uiType">UI 类型（屏幕/场景）</param>
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

        /// <summary>
        /// 为 WebGL 平台自动添加输入框兼容组件
        /// 仅在 WebGL 非编辑器环境生效
        /// </summary>
        /// <param name="go">UI 对象</param>
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
        /// 配置排序层级、射线拦截、覆盖排序等
        /// </summary>
        /// <param name="go">UI 对象</param>
        /// <param name="uiInfo">UI 信息</param>
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
        /// 绑定 Lua 脚本、预制体实例、UI 配置
        /// </summary>
        /// <param name="go">UI 对象</param>
        /// <param name="uiInfo">UI 信息</param>
        /// <returns>UIFlagBehaviour 实例</returns>
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
        /// 只处理挂载了 LuaBehaviour 且类型为 UI 的子节点
        /// </summary>
        /// <param name="go">根 UI 对象</param>
        /// <returns>子节点标记列表</returns>
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
        /// 触发外部传入的回调并传递 Lua 实例
        /// </summary>
        /// <param name="go">UI 对象</param>
        /// <param name="uiInfo">UI 信息</param>
        /// <param name="prefabObject">预制体资源对象</param>
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

        /// <summary>
        /// 检测全局按键抬起事件
        /// 当前仅处理返回键（ESC），用于关闭顶层 UI
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

                    // 收集所有可响应返回键的 UI
                    if (m_CurModalUI != null && m_CurModalUI.LuaBehaviour != null)
                        luaBehaviours.Add(m_CurModalUI.LuaBehaviour);

                    foreach (var ui in m_UnModalUIList)
                    {
                        if (ui != null && ui.LuaBehaviour != null)
                            luaBehaviours.Add(ui.LuaBehaviour);
                    }

                    if (luaBehaviours.Count > 0)
                    {
                        // 按 ZOrder 从低到高排序
                        luaBehaviours.Sort((a, b) =>
                        {
                            if (a == null || b == null) return 0;
                            var flagA = a.GetComponent<UIFlagBehaviour>();
                            var flagB = b.GetComponent<UIFlagBehaviour>();
                            if (flagA == null || flagB == null || flagA.UIInfo == null || flagB.UIInfo == null) return 0;
                            return flagA.UIInfo.ZOrder - flagB.UIInfo.ZOrder;
                        });

                        // 关闭最顶层可关闭 UI
                        var topLua = luaBehaviours[luaBehaviours.Count - 1];
                        if (topLua != null)
                        {
                            var topFlag = topLua.GetComponent<UIFlagBehaviour>();
                            if (topFlag != null && topFlag.UIInfo != null && topFlag.UIInfo.CloseOnEscapeKeyUp)
                            {
                                topFlag.LuaBehaviour?.CallLuaClose();
                            }
                        }

                        // 派发按键事件到 Lua
                        luaComponent.LuaKeysUpFromCSEventDelegate(KeyCode.Escape);
                    }
                }
            }
        }

        /// <summary>
        /// 根据目标 Flag 查找匹配的 UI 对象
        /// </summary>
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

        /// <summary>
        /// 根据 UIInfo 查找单个匹配的 UI 对象
        /// </summary>
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

        /// <summary>
        /// 根据 UIInfo 查找所有匹配的根 UI（去重子对象）
        /// </summary>
        private List<GameObject> GetAllMatchedUIInfoGameObjects(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            List<GameObject> result = new List<GameObject>();
            if (objectForChecking == null || targetUIInfo == null) return result;

            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            objectForChecking.GetComponentsInChildren(true, uiBehaviours);

            // 剔除子对象，只保留顶层父 UI
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

        /// <summary>
        /// 获取所有非模态、非追加的 UI
        /// </summary>
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

        /// <summary>
        /// 获取所有场景 UI（非追加）
        /// </summary>
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

        /// <summary>
        /// 从模态队列移除单个匹配 UI
        /// </summary>
        private void RemoveMatchedUIInfoFromModalUIInfoList(UIInfo targetUIInfo)
        {
            if (targetUIInfo == null) return;
            int index = m_ModalUIInfoList.FindIndex(uiInfo => uiInfo != null && uiInfo.Equals(targetUIInfo));
            if (index >= 0)
                m_ModalUIInfoList.RemoveAt(index);
        }

        /// <summary>
        /// 从模态队列移除所有匹配 UI
        /// </summary>
        private void RemoveAllMatchedUIInfosFromModalUIInfoList(UIInfo targetUIInfo)
        {
            if (targetUIInfo == null) return;
            m_ModalUIInfoList.RemoveAll(uiInfo => uiInfo != null && uiInfo.Equals(targetUIInfo));
        }

        /// <summary>
        /// 获取有效（未被销毁）的单个匹配 UI
        /// </summary>
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

        /// <summary>
        /// 获取所有有效（未被销毁）匹配 UI
        /// </summary>
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

        /// <summary>
        /// 根据 UI 类型获取所有有效 UI
        /// </summary>
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

        /// <summary>
        /// 内部关闭 UI（统一入口）
        /// 支持立即销毁/等待动画销毁
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
        /// 从所有管理列表移除，并通知父 UI
        /// </summary>
        private void InnerDestroyUIByFlag(UIFlagBehaviour flagBehaviour, bool destroyImmediate = false)
        {
            if (flagBehaviour == null) return;

            if (flagBehaviour.PrefabInstanceGOBehaviour != null)
            {
                flagBehaviour.PrefabInstanceGOBehaviour.RightNowDestroyOnAsset = true;
            }

            // 从管理容器移除
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

            // 追加 UI 销毁 → 通知父 UI
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

            // 销毁对象
            if (!flagBehaviour.FollowParentDestroy && flagBehaviour.gameObject != null)
            {
                if (destroyImmediate)
                    GameObject.DestroyImmediate(flagBehaviour.gameObject);
                else
                    GameObject.Destroy(flagBehaviour.gameObject);
            }
        }

        /// <summary>
        /// 刷新所有文本组件（字体、大小、位置、多语言适配）
        /// 支持 Unity 原生 Text 与 TMP 文本
        /// </summary>
        private void RefreshTextComponentsAdaptationParams(GameObject go, List<LocalizationFontData> fontDatas)
        {
            if (go == null || fontDatas == null || fontDatas.Count == 0) return;

            // 多语言检测：检查是否挂载 TextLocalizing
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

                // 计算缩放偏移
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

                // 原生 Text
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
                // TMP 文本
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
    }
}