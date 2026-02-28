using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Honor.Runtime
{
    public partial class UIComponent
    {
        /// <summary>
        /// 模态ui心跳
        /// </summary>
        private void UpdateCurModalUI()
        {
            if (m_CurModalUI != null && m_CurModalUI.LuaBehaviour != null && m_CurModalUI.LuaBehaviour.UseProc && m_CurModalUI.gameObject.activeSelf)
            {
                m_CurModalUI.LuaBehaviour.Proc();
            }
        }

        /// <summary>
        /// 模态ui信息队列
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
                    // 当存在回调函数时按照异步进行弹出处理
                    if (modalUIInfo.OverCallback != null)
                    {
                        OpenUIAsyncByInfo(modalUIInfo);
                    }
                    else  // 当不存在回调函数时按照同步进行弹出处理
                    {
                        OpenUISyncByInfo(modalUIInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 非模态ui心跳
        /// </summary>
        private void UpdateUnModalUIList()
        {
            for (int index = 0; index < m_UnModalUIList.Count; index++)
            {
                UIFlagBehaviour ui = m_UnModalUIList[index];
                if (ui.LuaBehaviour != null && ui.LuaBehaviour.UseProc && ui.gameObject.activeSelf)
                {
                    ui.LuaBehaviour.Proc();
                }
            }
        }

        /// <summary>
        /// 场景ui心跳
        /// </summary>
        private void UpdateSceneUIList()
        {
            for (int index = 0; index < m_SceneUIList.Count; index++)
            {
                UIFlagBehaviour ui = m_SceneUIList[index];
                if (ui.LuaBehaviour != null && ui.LuaBehaviour.UseProc && ui.gameObject.activeSelf)
                {
                    ui.LuaBehaviour.Proc();
                }
            }
        }

        /// <summary>
        /// 附加ui心跳
        /// 针对默认嵌入在UI中或Append追加到UI上的对象
        /// </summary>
        /// <param name="uiType"></param>
        private void UpdateSubUIList(UIType uiType)
        {
            for (int index = 0; index < m_SubUIList[uiType].Count; index++)
            {
                UIFlagBehaviour ui = m_SubUIList[uiType][index];
                if (ui.LuaBehaviour != null && ui.LuaBehaviour.UseProc && ui.gameObject.activeSelf)
                {
                    ui.LuaBehaviour.Proc();
                }
            }
        }

        /// <summary>
        /// 销毁ui
        /// </summary>
        private void UpdateDestroyUIList()
        {
            int count = 0;
            for (int index = m_UnloadUIList.Count - 1; index >= 0; index--)
            {
                // 销毁UI（内部）
                InnerDestroyUIByFlag(m_UnloadUIList[index]);
                if (++count >= m_DestroyMaxNumPerFrame)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 添加WebGLInput组件
        /// </summary>
        /// <param name="go">UI对象</param>
        private void AddWebGLInput(GameObject go)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var inputFields = go.GetComponentsInChildren<InputField>(true);
            foreach (var inputField in inputFields)
            {
                if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                {
                    inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                }
            }
#endif
        }

        /// <summary>
        /// 添加Canvas相关组件
        /// </summary>
        /// <param name="go">UI对象</param>
        /// <param name="uiInfo">UI信息</param>
        private void AddCanvas(GameObject go, UIInfo uiInfo)
        {
            Canvas canvas = go.AddComponent<Canvas>();
            if (canvas != null)
            {
                // 以Inherit继承的方式设置当前canvas的pixelPerfect属性值
                //canvas.pixelPerfect = true;
                //canvas.overridePixelPerfect = true;
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
        /// 添加FlagBehaviour组件
        /// </summary>
        /// <param name="go">UI对象</param>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
        private UIFlagBehaviour AddFlagBehaviour(GameObject go, UIInfo uiInfo)
        {
            UIFlagBehaviour uiFlagBehaviour = go.GetOrAddComponent<UIFlagBehaviour>();
            uiFlagBehaviour.LuaBehaviour = go.GetComponent<LuaBehaviour>();
            uiFlagBehaviour.PrefabInstanceGOBehaviour = go.GetComponent<PrefabInstanceGOBehaviour>();
            uiFlagBehaviour.UIInfo = uiInfo;
            return uiFlagBehaviour;
        }

        /// <summary>
        /// 为所有子节点添加FlagBehaviour组件
        /// 为所有默认挂载的孩子LuaBehaviour节点追加Flag
        /// </summary>
        /// <param name="go">UI对象</param>
        /// <returns></returns>
        private List<UIFlagBehaviour> AddChildrenFlagBehaviours(GameObject go)
        {
            List<UIFlagBehaviour> childrenFlagBehaviours = new List<UIFlagBehaviour>();
            List<LuaBehaviour> childrenLuaBehaviour = new List<LuaBehaviour>();
            go.GetComponentsInChildren(true, childrenLuaBehaviour);
            childrenLuaBehaviour.Remove(go.GetComponent<UIFlagBehaviour>().LuaBehaviour);
            foreach (var child in childrenLuaBehaviour)
            {
                if (child.PrefabType == PrefabType.UI)
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
        /// UI异步创建完毕回调
        /// </summary>
        /// <param name="go">UI对象</param>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="prefabObject">预制体对象</param>
        private void DoUICreateOverCallbackOnAsync(GameObject go, UIInfo uiInfo, LoadPrefabData prefabObject)
        {
            if (uiInfo.OverCallback != null)
            {
                LuaTable validLuaClass = null;
                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                if (luaBehaviour != null)
                {
                    validLuaClass = luaBehaviour.ValidLuaClass;
                }
                uiInfo.OverCallback(prefabObject.ABPath, prefabObject.AssetName, validLuaClass, go);
            }
        }

        /// <summary>
        /// 检查按键弹起事件
        /// </summary>
        private void CheckKeysUp()
        {
            if (m_BlockAllUIsKeyUpSwitch)
            {
                return;
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (LuaManager.Instance != null && LuaManager.Instance.LuaKeysUpFromCSEventDelegate != null)
                {
                    List<LuaBehaviour> luaBehaviours = new List<LuaBehaviour>();
                    if (m_CurModalUI != null)
                    {
                        luaBehaviours.Add(m_CurModalUI.LuaBehaviour);
                    }
                    if(m_UnModalUIList.Count > 0)
                    {
                        m_UnModalUIList.ForEach((ui) =>
                        {
                            if(ui.LuaBehaviour != null)
                            {
                                luaBehaviours.Add(ui.LuaBehaviour);
                            }
                        });
                    }
                    if (luaBehaviours.Count > 0)
                    {
                        luaBehaviours.Sort((luaBehaviour1, luaBehaviour2) => {
                            return luaBehaviour1.GetComponent<UIFlagBehaviour>().UIInfo.ZOrder - luaBehaviour2.GetComponent<UIFlagBehaviour>().UIInfo.ZOrder;
                        });
                        bool closeOnEscapeKeyUp = luaBehaviours[luaBehaviours.Count - 1].GetComponent<UIFlagBehaviour>().UIInfo.CloseOnEscapeKeyUp;
                        if(closeOnEscapeKeyUp)
                        {
                            luaBehaviours[luaBehaviours.Count - 1].CallLuaClose();
                        }
                        LuaManager.Instance.LuaKeysUpFromCSEventDelegate(KeyCode.Escape);
                    }
                }
            }
        }

        /// <summary>
        /// 获取待检查对象子节点中与目标对象匹配的GameObject
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <param name="targetFlagBehaviour">目标对象</param>
        private GameObject GetMatchedGOGameObject(UIFlagBehaviour objectForChecking, UIFlagBehaviour targetFlagBehaviour)
        {
            GameObject result = null;
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && behaviour.UIInfo.IsAppend == targetFlagBehaviour.UIInfo.IsAppend && behaviour.gameObject == targetFlagBehaviour.gameObject)
                {
                    result = behaviour.gameObject;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取待检查对象子节点中与目标UI信息匹配的GameObject
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <returns></returns>
        private GameObject GetMatchedUIInfoGameObject(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            GameObject result = null;
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo))
                {
                    result = behaviour.gameObject;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取待检查对象子节点中所有与目标UI信息匹配的GameObject
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <returns></returns>
        private List<GameObject> GetAllMatchedUIInfoGameObjects(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            List<GameObject> result = new List<GameObject>();
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            // 剔除所有作为其他已经收集到的ui子节点的ui对象
            uiBehaviours.RemoveAll((behaviour) => {
                foreach (var behaviourForChecking in uiBehaviours)
                {
                    if (behaviour != behaviourForChecking && behaviour.transform.IsChildOf(behaviourForChecking.transform))
                    {
                        return true;
                    }
                }
                return false;
            });
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo))
                {
                    result.Add(behaviour.gameObject);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取待检查对象子节点中所有非模态UI的GameObject
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <returns></returns>
        private List<GameObject> GetAllMatchedUnModalGameObjects(UIFlagBehaviour objectForChecking)
        {
            List<GameObject> result = new List<GameObject>();
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            // 剔除所有作为其他已经收集到的ui节点的子节点的ui对象
            uiBehaviours.RemoveAll((behaviour) => {
                foreach (var behaviourForChecking in uiBehaviours)
                {
                    if (behaviour != behaviourForChecking && behaviour.transform.IsChildOf(behaviourForChecking.transform))
                    {
                        return true;
                    }
                }
                return false;
            });
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && !behaviour.UIInfo.IsAppend && !behaviour.UIInfo.IsModal)
                {
                    result.Add(behaviour.gameObject);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取待检查对象子节点中所有场景UI的GameObject
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <returns></returns>
        private List<GameObject> GetAllMatchedSceneUIGameObjects(UIFlagBehaviour objectForChecking)
        {
            List<GameObject> result = new List<GameObject>();
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            // 剔除所有作为其他已经收集到的ui节点的子节点的ui对象
            uiBehaviours.RemoveAll((behaviour) => {
                foreach (var behaviourForChecking in uiBehaviours)
                {
                    if (behaviour != behaviourForChecking && behaviour.transform.IsChildOf(behaviourForChecking.transform))
                    {
                        return true;
                    }
                }
                return false;
            });
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && !behaviour.UIInfo.IsAppend && behaviour.UIInfo.UIType == UIType.Scene)
                {
                    result.Add(behaviour.gameObject);
                }
            }
            return result;
        }

        /// <summary>
        /// 从模态UI信息队列中移除匹配的目标UI信息
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        private void RemoveMatchedUIInfoFromModalUIInfoList(UIInfo targetUIInfo)
        {
            int index = m_ModalUIInfoList.FindIndex((uiInfo) => { return uiInfo.Equals(targetUIInfo); });
            if (index >= 0)
            {
                m_ModalUIInfoList.RemoveAt(index);
            }
        }

        /// <summary>
        /// 从模态UI信息队列中移除所有匹配的目标UI信息
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        private void RemoveAllMatchedUIInfosFromModalUIInfoList(UIInfo targetUIInfo)
        {
            m_ModalUIInfoList.RemoveAll((uiInfo) => { return uiInfo.Equals(targetUIInfo); });
        }

        /// <summary>
        /// 获取待检查对象子节点中与目标UI信息匹配的有效GameObject
        /// 有效GameObject：非UnloadUIList中的对象
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <returns></returns>
        private GameObject GetMatchedUIInfoValidGameObject(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            GameObject result = null;
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo))
                {
                    if (!m_UnloadUIList.Contains(behaviour))
                    {
                        result = behaviour.gameObject;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取待检查对象子节点中所有与目标UI信息匹配的有效GameObject
        /// 有效GameObject：非UnloadUIList中的对象
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <returns></returns>
        private List<GameObject> GetAllMatchedUIInfoValidGameObjects(UIFlagBehaviour objectForChecking, UIInfo targetUIInfo)
        {
            List<GameObject> result = new List<GameObject>();
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && behaviour.UIInfo.Equals(targetUIInfo))
                {
                    if (!m_UnloadUIList.Contains(behaviour))
                    {
                        result.Add(behaviour.gameObject);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取待检查对象子节点中所有与目标UI类型匹配的有效GameObject
        /// 有效GameObject：非UnloadUIList中的对象
        /// </summary>
        /// <param name="objectForChecking">待检查对象根节点</param>
        /// <param name="targetUIType">目标UI信息中的UI类型</param>
        /// <param name="isAppend">是否为附加UI</param>
        /// <returns></returns>
        private List<GameObject> GetAllMatchedUITypeValidGameObjects(UIFlagBehaviour objectForChecking, UIType targetUIType, bool isAppend)
        {
            List<GameObject> result = new List<GameObject>();
            List<UIFlagBehaviour> uiBehaviours = new List<UIFlagBehaviour>();
            uiBehaviours.AddRange(objectForChecking.GetComponentsInChildren<UIFlagBehaviour>(true));
            foreach (var behaviour in uiBehaviours)
            {
                if (behaviour.UIInfo != null && behaviour.UIInfo.IsAppend == isAppend && behaviour.UIInfo.UIType == targetUIType)
                {
                    if (!m_UnloadUIList.Contains(behaviour))
                    {
                        result.Add(behaviour.gameObject);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据GameObject关闭UI（内部）
        /// </summary>
        /// <param name="go">UI对象</param>
        /// <param name="rightNowDestroy">是否立即销毁</param>
        private void InnerCloseUIByGO(GameObject go, bool rightNowDestroy)
        {
            if (go == null)
            {
                return;
            }
            PrefabInstanceGOBehaviour prefabInstanceGOBehaviour = go.GetComponent<PrefabInstanceGOBehaviour>();
            if (prefabInstanceGOBehaviour != null)
            {
                UIFlagBehaviour flagBehaviour = go.GetComponent<UIFlagBehaviour>();

                if (rightNowDestroy)
                {
                    InnerDestroyUIByFlag(flagBehaviour, true);
                }
                else
                {
                    if (flagBehaviour.LuaBehaviour != null)
                    {
                        flagBehaviour.LuaBehaviour.CallLuaClose();
                    }
                    else
                    {
                        AddFlagToUnloadUIList(flagBehaviour);
                    }
                }
            }
            else
            {
                AssetManager.Instance.m_LoadPrefabComponent.Destroy(go, rightNowDestroy);
            }
        }

        /// <summary>
        /// 根据UIFlagBehaviour销毁UI（内部）
        /// </summary>
        /// <param name="flagBehaviour">UIFlagBehaviour通用组件</param>
        /// <param name="destroyImmediate">是否立即销毁（当前帧马上销毁）</param>
        private void InnerDestroyUIByFlag(UIFlagBehaviour flagBehaviour, bool destroyImmediate = false)
        {
            if(flagBehaviour && flagBehaviour.PrefabInstanceGOBehaviour != null)
            {
                flagBehaviour.PrefabInstanceGOBehaviour.RightNowDestroyOnAsset = true;
            }

            if (m_CurModalUI == flagBehaviour)
            {
                m_CurModalUI = null;
            }
            else
            {
                if (m_UnModalUIList.Contains(flagBehaviour))
                {
                    m_UnModalUIList.Remove(flagBehaviour);
                }
                else if (m_SceneUIList.Contains(flagBehaviour))
                {
                    m_SceneUIList.Remove(flagBehaviour);
                }
            }
            foreach(var itr in m_SubUIList)
            {
                if(m_SubUIList[itr.Key].Contains(flagBehaviour))
                {
                    m_SubUIList[itr.Key].Remove(flagBehaviour);
                    break;
                }
            }

            m_UnloadUIList.Remove(flagBehaviour);

            // 通知父节点（由近及远的顺序通知父节点）
            if (flagBehaviour && flagBehaviour.LuaBehaviour)
            {
                if(flagBehaviour.UIInfo != null && flagBehaviour.UIInfo.IsAppend)
                {
                    List<UIFlagBehaviour> parentFlags = new List<UIFlagBehaviour>();
                    flagBehaviour.GetComponentsInParent(true, parentFlags);
                    parentFlags.Remove(flagBehaviour);
                    parentFlags.Sort((parent1, parent2) => { return parent2.transform.GetRouteNum() - parent1.transform.GetRouteNum(); });
                    foreach (var parentFlag in parentFlags)
                    {
                        if (parentFlag.LuaBehaviour != null)
                        {
                            LuaTable masterLuaClass = parentFlag.LuaBehaviour.ValidLuaClass;
                            if (masterLuaClass != null)
                            {
                                masterLuaClass.Get("OnAddedUIDestroyed", out LuaFunction func);
                                if (func != null) func.Action(masterLuaClass, flagBehaviour.LuaBehaviour.ValidLuaClass);
                            }
                        }
                    }
                }
            }
            
            if(flagBehaviour && !flagBehaviour.FollowParentDestroy)
            {
                if (destroyImmediate)
                {
                    GameObject.DestroyImmediate(flagBehaviour.gameObject);
                }
                else
                {
                    GameObject.Destroy(flagBehaviour.gameObject);
                }
            }
        }

        /// <summary>
        /// 刷新文本组件参数
        /// </summary>
        /// <param name="go">指定对象</param>
        /// <param name="fontDatas">字体数据集合</param>
        private void RefreshTextComponentsAdaptationParams(GameObject go, List<LocalizationFontData> fontDatas)
        {
            //m_LocalizationComponent.GetFontDatas(m_LocalizationComponent.RuntimeLastLanguage, out List<LocalizationFontData> lastFontDatas);

            // 启用TextLocalizing脚本检测
            if (m_CheckTextLocalizings)
            {
                List<MaskableGraphic> graphics = new List<MaskableGraphic>();
                graphics.AddRange(go.GetComponentsInChildren<Text>());
                graphics.AddRange(go.GetComponentsInChildren<TextMeshProUGUI>());
                foreach (var graphic in graphics)
                {
                    AorTextLocalizing aorTextLocalizing = graphic.GetComponent<AorTextLocalizing>();
                    if (aorTextLocalizing == null)
                    {
                        Log.Error($"发现组件 TextLocalizing 缺失，请检查：{graphic.transform.GetRoute()}。");
                    }
                }
            }

            List<string> fontDataMarks = new List<string>();
            fontDatas.ForEach((data)=> { fontDataMarks.Add(data.Mark); });

            UIInfo uiInfo = go.GetComponent<UIFlagBehaviour>().UIInfo;
            AorTextLocalizing[] textLocalizingComponents = go.GetComponentsInChildren<AorTextLocalizing>(true);

            for (int tlcIndex = 0; tlcIndex < textLocalizingComponents.Length; tlcIndex++)
            {
                AorTextLocalizing aorTextLocalizingComponent = textLocalizingComponents[tlcIndex];
                int markTargetIndex = fontDataMarks.FindIndex((mark)=> { return mark == aorTextLocalizingComponent.LocalizingFontMark; });
                markTargetIndex = markTargetIndex < 0 ? 0 : markTargetIndex;

                Text textComponent = aorTextLocalizingComponent.GetComponent<Text>();
                TextMeshProUGUI tmpComponent = aorTextLocalizingComponent.GetComponent<TextMeshProUGUI>();

                LocalizationFontData targetFontData = fontDatas[markTargetIndex];
                UnityEngine.Object targetFont = Fonts[markTargetIndex];
                UnityEngine.Object oldFont = markTargetIndex < m_LastFonts.Count ? m_LastFonts[markTargetIndex] : null;
                GameDefinitions.AssetType targetFontType = (GameDefinitions.AssetType)Enum.Parse(typeof(GameDefinitions.AssetType), fontDatas[markTargetIndex].FontType);
                float targetFontSizeScaleRatio = fontDatas[markTargetIndex].FontSizeScaleRatio;
                Vector2 targetPositionOffset = Vector2.zero; //fontDatas[markTargetIndex].TextPositionOffset;

                float actualFontScaleRatio = 1.0f;
                Vector2 actualPositionOffset = Vector2.zero;
                //if (oldFont != null && oldFont != targetFont)
                //{
                //    // 根据前次字体类型计算出目标字体类型下的fontSize缩放比例
                //    actualFontScaleRatio = targetFontSizeScaleRatio / lastFontDatas[markTargetIndex].FontSizeScaleRatio;
                //}
                if (aorTextLocalizingComponent.Language != GameDefinitions.Language.Unspecified && aorTextLocalizingComponent.Language != LocalizationManager.Instance().Language)
                {
                    LocalizationManager.Instance().GetFontDatas(aorTextLocalizingComponent.Language, out List<LocalizationFontData> lastFontDatas);
                    if (lastFontDatas != null)
                    {
                        actualFontScaleRatio = targetFontSizeScaleRatio / lastFontDatas[markTargetIndex].FontSizeScaleRatio;
                        actualPositionOffset = targetPositionOffset - Vector2.zero;//lastFontDatas[markTargetIndex].TextPositionOffset;
                    }
                }
                else if (aorTextLocalizingComponent.Language == GameDefinitions.Language.Unspecified)
                {
                    actualFontScaleRatio = targetFontSizeScaleRatio;
                    actualPositionOffset = targetPositionOffset;
                }

                if (targetFontType == GameDefinitions.AssetType.Font)
                {
                    if (!uiInfo.MultiTypeTextCompsCoexist)
                    {
                        if(tmpComponent)
                        {
                            tmpComponent.enabled = false;
                        }
                    }
                    if(textComponent)
                    {
                        textComponent.enabled = true;
                        textComponent.font = (Font)targetFont;
                        if (aorTextLocalizingComponent.OpenPosOffset)
                        {
                            textComponent.fontSize = (int)System.Math.Round(textComponent.fontSize * actualFontScaleRatio);
                            textComponent.resizeTextMinSize = (int)System.Math.Round(textComponent.resizeTextMinSize * actualFontScaleRatio);
                            textComponent.resizeTextMaxSize = (int)System.Math.Round(textComponent.resizeTextMaxSize * actualFontScaleRatio);
                        }
                        aorTextLocalizingComponent.Language = LocalizationManager.Instance().Language;
                        // 校正坐标
                        if (aorTextLocalizingComponent.OpenPosOffset)
                        {
                            textComponent.rectTransform.anchoredPosition += actualPositionOffset;
                        }
                    }
                }
                else if (targetFontType == GameDefinitions.AssetType.FontTMP)
                {
                    if (!uiInfo.MultiTypeTextCompsCoexist)
                    {
                        if (textComponent)
                        {
                            textComponent.enabled = false;
                        }
                    }
                    if(tmpComponent)
                    {
                        tmpComponent.enabled = true;

                        // 先找到目标字体材质并进行加载
                        string contactFlag = "___";
                        Material targetFontMaterial = null;
                        if (tmpComponent.fontSharedMaterial.name.Contains(contactFlag))
                        {
                            int idx = tmpComponent.fontSharedMaterial.name.LastIndexOf(contactFlag);
                            if (idx > 0)
                            {
                                string targetFontNameSuffix = tmpComponent.fontSharedMaterial.name.Substring(idx + contactFlag.Length).Replace("(Instance)", string.Empty).Replace(" ", string.Empty);
                                if (!string.IsNullOrEmpty(targetFontNameSuffix))
                                {
                                    targetFontMaterial = (Material)AssetManager.Instance.LoadAssetSync("Material", targetFontData.ABPath, $"{targetFont.name}{contactFlag}{targetFontNameSuffix}");
                                }
                            }
                        }

                        // font赋值会导致fontMaterial发生变化
                        tmpComponent.font = (TMP_FontAsset)targetFont;

                        // 如果找到了目标字体材质则进行替换
                        if (targetFontMaterial)
                        {
                            tmpComponent.fontMaterial = targetFontMaterial;
                            AssetManager.Instance.UnloadAsset(targetFontMaterial);
                        }
                        if (aorTextLocalizingComponent.OpenPosOffset)
                        {
                            tmpComponent.fontSize = (int)System.Math.Round(tmpComponent.fontSize * actualFontScaleRatio);
                            tmpComponent.fontSizeMin = (int)System.Math.Round(tmpComponent.fontSizeMin * actualFontScaleRatio);
                            tmpComponent.fontSizeMax = (int)System.Math.Round(tmpComponent.fontSizeMax * actualFontScaleRatio);
                        }
                        aorTextLocalizingComponent.Language = LocalizationManager.Instance().Language;
                        if (aorTextLocalizingComponent.OpenPosOffset)
                        {
                            tmpComponent.rectTransform.anchoredPosition += actualPositionOffset;
                        }
                    }
                }
                
            }
        }
    }
}