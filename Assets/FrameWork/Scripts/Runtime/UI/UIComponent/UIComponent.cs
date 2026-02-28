using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// UI组件
    /// </summary>
    public partial class UIComponent
    {
          /// <summary>
        /// 异步打开UI界面
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
        public void OpenUIAsyncByInfo(UIInfo uiInfo)
        {
            if (uiInfo.UIType == UIType.Screen)
            {
                if (uiInfo.IsModal)
                {
                    if (!m_BlockModalUIsSwitch && m_CurModalUI == null)
                    {
                        AssetManager.Instance.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams, (LoadPrefabData prefabObject, GameObject go) =>
                        {
                            AddWebGLInput(go);
                            AddCanvas(go, uiInfo);
                            m_CurModalUI = AddFlagBehaviour(go, uiInfo);
                            m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                            UIManager.Instance.RefreshFontsForUI(go);
                            DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
                        });
                    }
                    else
                    {
                        m_ModalUIInfoList.Add(uiInfo);
                        m_ModalUIInfoList.Sort((ui1, ui2) => { return ui1.Priority - ui2.Priority; });
                    }
                }
                else
                {
                    AssetManager.Instance.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams, (LoadPrefabData prefabObject, GameObject go) =>
                    {
                        AddWebGLInput(go);
                        AddCanvas(go, uiInfo);
                        m_UnModalUIList.Add(AddFlagBehaviour(go, uiInfo));
                        m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                        UIManager.Instance.RefreshFontsForUI(go);
                        DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
                    });
                }
            }
            else
            {
                AssetManager.Instance.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, m_SceneUICanvas.transform, uiInfo.LuaParams, (LoadPrefabData prefabObject, GameObject go) =>
                {
                    AddWebGLInput(go);
                    AddCanvas(go, uiInfo);
                    m_SceneUIList.Add(AddFlagBehaviour(go, uiInfo));
                    m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                    UIManager.Instance.RefreshFontsForUI(go);
                    DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
                });
            }
            return;
        }

        /// <summary>
        /// 同步打开UI界面
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
        public GameObject OpenUISyncByInfo(UIInfo uiInfo)
        {
            if (uiInfo.UIType == UIType.Screen)
            {
                if (uiInfo.IsModal)
                {
                    if (!m_BlockModalUIsSwitch && m_CurModalUI == null)
                    {
                        GameObject go = AssetManager.Instance.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams);
                        AddWebGLInput(go);
                        AddCanvas(go, uiInfo);
                        m_CurModalUI = AddFlagBehaviour(go, uiInfo);
                        m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                        UIManager.Instance.RefreshFontsForUI(go);
                        return m_CurModalUI.gameObject;
                    }
                    else
                    {
                        m_ModalUIInfoList.Add(uiInfo);
                        m_ModalUIInfoList.Sort((ui1, ui2) => { return ui1.Priority - ui2.Priority; });
                        return null;
                    }
                }
                else
                {
                    GameObject go = AssetManager.Instance.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams);
                    AddWebGLInput(go);
                    AddCanvas(go, uiInfo);
                    m_UnModalUIList.Add(AddFlagBehaviour(go, uiInfo));
                    m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                    UIManager.Instance.RefreshFontsForUI(go);
                    return go;
                }
            }
            else
            {
                GameObject go = AssetManager.Instance.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, m_SceneUICanvas.transform, uiInfo.LuaParams);
                AddWebGLInput(go);
                AddCanvas(go, uiInfo);       
                m_SceneUIList.Add(AddFlagBehaviour(go, uiInfo));
                m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                UIManager.Instance.RefreshFontsForUI(go);
                return go;
            }
        }

        /// <summary>
        /// 异步追加UI到主体UI上（追加式UI）
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="parent">指定的父对象（必须为UI主体下的对象节点）</param>
        public void AddUIAsyncByInfo(UIInfo uiInfo, Transform parent)
        {
            AssetManager.Instance.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, parent, uiInfo.LuaParams, (LoadPrefabData prefabObject, GameObject go) =>
            {
                uiInfo.IsAppend = true;
                AddWebGLInput(go);
                m_SubUIList[uiInfo.UIType].Add(AddFlagBehaviour(go, uiInfo));
                m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                UIManager.Instance.RefreshFontsForUI(go);
                DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
            });
        }

        /// <summary>
        /// 同步追加UI到主体UI上（追加式UI）
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="parent">指定的父对象（必须为UI主体下的对象节点）</param>
        public GameObject AddUISyncByInfo(UIInfo uiInfo, Transform parent)
        {
            GameObject go = AssetManager.Instance.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, parent, uiInfo.LuaParams);
            uiInfo.IsAppend = true;
            AddWebGLInput(go);
            m_SubUIList[uiInfo.UIType].Add(AddFlagBehaviour(go, uiInfo));
            m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
            UIManager.Instance.RefreshFontsForUI(go);
            return go;
        }

        /// <summary>
        /// 关闭UI界面
        /// </summary>
        /// <param name="targetGO">目标UI对象</param>
        /// <param name="rightNow">马上</param>
        /// <returns></returns>
        public void CloseUIByGO(GameObject targetGO, bool rightNow)
        {
            if (targetGO == null) return;
            if (m_PermanentUIs.Contains(targetGO)) return;

            GameObject result = null;
            UIFlagBehaviour targetFlag = targetGO.GetComponent<UIFlagBehaviour>();
            if(targetFlag != null && targetFlag.UIInfo != null)
            {
                if (targetFlag.UIInfo.UIType == UIType.Screen)
                {
                    if (m_CurModalUI != null)
                    {
                        result = GetMatchedGOGameObject(m_CurModalUI, targetFlag);
                    }
                    if (result == null)
                    {
                        foreach (var unModalUI in m_UnModalUIList)
                        {
                            result = GetMatchedGOGameObject(unModalUI, targetFlag);
                            if (result != null)
                            {
                                break;
                            }
                        }
                    }
                }
                else if(targetFlag.UIInfo.UIType == UIType.Scene)
                {
                    foreach (var sceneUI in m_SceneUIList)
                    {
                        result = GetMatchedGOGameObject(sceneUI, targetFlag);
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
                // 根据GameObject销毁UI（内部）
                InnerCloseUIByGO(result, rightNow);
            }
        }

        /// <summary>
        /// 关闭UI界面
        /// 检查到任意一个匹配UI即触发关闭并返回
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <param name="rightNow">马上</param>
        /// <returns></returns>
        public void CloseUIByInfo(UIInfo targetUIInfo, bool rightNow)
        {
            if (targetUIInfo == null) return;
            
            GameObject result = null;

            if (targetUIInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    result = GetMatchedUIInfoGameObject(m_CurModalUI, targetUIInfo);
                }
                if (result == null)
                {
                    foreach (var unModalUI in m_UnModalUIList)
                    {
                        result = GetMatchedUIInfoGameObject(unModalUI, targetUIInfo);
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
                if (result == null)
                {
                    RemoveMatchedUIInfoFromModalUIInfoList(targetUIInfo);
                }
            }
            else if (targetUIInfo.UIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                {
                    result = GetMatchedUIInfoGameObject(sceneUI, targetUIInfo);
                    if (result != null)
                    {
                        break;
                    }
                }
            }

            // 确认当前需要移除的ui并非常住内存ui
            if (result != null)
            {
                if (m_PermanentUIs.Contains(result.gameObject))
                {
                    result = null;
                }
            }

            // 根据GameObject销毁UI（内部）
            InnerCloseUIByGO(result, rightNow);

        }

        /// <summary>
        /// 关闭UI界面
        /// 检查到所有匹配UI，统一触发关闭并返回
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <param name="rightNow">马上</param>
        /// <returns></returns>
        public void CloseUIsByInfo(UIInfo targetUIInfo, bool rightNow)
        {
            if (targetUIInfo == null) return;
            
            List<GameObject> result = new List<GameObject>();

            if (targetUIInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    result.AddRange(GetAllMatchedUIInfoGameObjects(m_CurModalUI, targetUIInfo));
                }
                foreach (var unModalUI in m_UnModalUIList)
                {
                    result.AddRange(GetAllMatchedUIInfoGameObjects(unModalUI, targetUIInfo));
                }
                RemoveAllMatchedUIInfosFromModalUIInfoList(targetUIInfo);
            }
            else if (targetUIInfo.UIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                {
                    result.AddRange(GetAllMatchedUIInfoGameObjects(sceneUI, targetUIInfo));
                }
            }

            result.RemoveAll((go) => {return m_PermanentUIs.Contains(go);});

            if (result.Count > 0)
            {
                // 根据GameObject销毁UI（内部）
                result.ForEach((go) => { InnerCloseUIByGO(go, rightNow); });
            }

        }

        /// <summary>
        /// 移除主体UI上的追加UI
        /// </summary>
        /// <param name="uiGO">UI对象</param>
        /// <param name="rightNow">马上</param>
        public void RemoveUIByGO(GameObject uiGO, bool rightNow)
        {
            CloseUIByGO(uiGO, rightNow);
        }

        /// <summary>
        /// 获取UI界面
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <returns></returns>
        public GameObject GetUIByInfo(UIInfo targetUIInfo)
        {
            if (targetUIInfo == null) return null;

            GameObject result = null;

            if (targetUIInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    result = GetMatchedUIInfoValidGameObject(m_CurModalUI, targetUIInfo);
                    if (result != null) return result;
                }
                foreach (var unModalUI in m_UnModalUIList)
                {
                    result = GetMatchedUIInfoValidGameObject(unModalUI, targetUIInfo);
                    if (result != null) return result;
                }
            }
            else if (targetUIInfo.UIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                {
                    result = GetMatchedUIInfoValidGameObject(sceneUI, targetUIInfo);
                    if (result != null) return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <returns></returns>
        public GameObject[] GetUIsByInfo(UIInfo targetUIInfo)
        {
            List<GameObject> results = new List<GameObject>();
            GetUIsByInfo(targetUIInfo, results);
            return results.ToArray();
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="targetUIInfo">目标UI信息</param>
        /// <param name="result">ui集合</param>
        /// <returns></returns>
        public void GetUIsByInfo(UIInfo targetUIInfo, List<GameObject> result)
        {
            if (targetUIInfo == null) return;
            if (result == null) return;

            result.Clear();

            if (targetUIInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    result.AddRange(GetAllMatchedUIInfoValidGameObjects(m_CurModalUI, targetUIInfo));
                }
                foreach (var unModalUI in m_UnModalUIList)
                {
                    result.AddRange(GetAllMatchedUIInfoValidGameObjects(unModalUI, targetUIInfo));
                }
            }
            else if (targetUIInfo.UIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                {
                    result.AddRange(GetAllMatchedUIInfoValidGameObjects(sceneUI, targetUIInfo));
                }
            }
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="targetUIType">目标UI界面类型</param>
        /// <param name="isAppend">是否为追加式UI</param>
        /// <returns></returns>
        public GameObject[] GetUIsByUIType(UIType targetUIType, bool isAppend)
        {
            List<GameObject> result = new List<GameObject>();
            GetUIsByUIType(targetUIType, isAppend, result);
            return result.ToArray();
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="targetUIType">目标UI界面类型</param>
        /// <param name="result">ui集合</param>
        public void GetUIsByUIType(UIType targetUIType, bool isAppend, List<GameObject> result)
        {
            if (result == null) return;

            result.Clear();

            if (targetUIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    result.AddRange(GetAllMatchedUITypeValidGameObjects(m_CurModalUI, targetUIType, isAppend));
                }
                foreach (var unModalUI in m_UnModalUIList)
                {
                    result.AddRange(GetAllMatchedUITypeValidGameObjects(unModalUI, targetUIType, isAppend));
                }
            }
            else if (targetUIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                {
                    result.AddRange(GetAllMatchedUITypeValidGameObjects(sceneUI, targetUIType, isAppend));
                }
            }
        }

        /// <summary>
        /// 关闭所有模态UI界面（非追加式UI）
        /// 从非追加式节点（即父节点）驱动子节点的Close
        /// </summary>
        /// <param name="rightNow">马上</param>
        public void CloseAllModalUIs(bool rightNow)
        {
            if (m_CurModalUI != null)
            {
                InnerCloseUIByGO(m_CurModalUI.gameObject, rightNow);
            }
            m_ModalUIInfoList.Clear();
        }

        /// <summary>
        /// 关闭所有非模态UI界面（非追加式UI）
        /// 从非追加式节点（即父节点）驱动子节点的Close
        /// </summary>
        /// <param name="rightNow">马上</param>
        public void CloseAllUnModalUIs(bool rightNow)
        {
            List<GameObject> result = new List<GameObject>();

            // UIType.Screen
            if (m_CurModalUI != null)
            {
                result.AddRange(GetAllMatchedUnModalGameObjects(m_CurModalUI));
            }
            foreach (var unModalUI in m_UnModalUIList)
            {
                result.AddRange(GetAllMatchedUnModalGameObjects(unModalUI));
            }
            
            result.RemoveAll((cmp) => { return m_PermanentUIs.Contains(cmp.gameObject); });

            if (result.Count > 0)
            {
                // 根据GameObject销毁UI（内部）
                result.ForEach((go) => { InnerCloseUIByGO(go, rightNow); });
            }
        }

        /// <summary>
        /// 关闭所有场景UI界面（非追加式UI）
        /// 从非追加式节点（即父节点）驱动子节点的Close
        /// </summary>
        /// <param name="rightNow">马上</param>
        public void CloseAllSceneUIs(bool rightNow)
        {
            List<GameObject> result = new List<GameObject>();

            // UIType.Scene
            foreach (var sceneUI in m_SceneUIList)
            {
                result.AddRange(GetAllMatchedSceneUIGameObjects(sceneUI));
            }

            result.RemoveAll((cmp) => { return m_PermanentUIs.Contains(cmp.gameObject); });

            if (result.Count > 0)
            {
                // 根据GameObject销毁UI（内部）
                result.ForEach((go) => { InnerCloseUIByGO(go, rightNow); });
            }
        }

        /// <summary>
        /// 关闭所有UI界面（非追加式UI）
        /// 从非追加式节点（即父节点）驱动子节点的Close
        /// </summary>
        /// <param name="uiType">UI界面类型</param>
        /// <param name="rightNow">马上</param>
        public void CloseAllUIs(UIType uiType, bool rightNow)
        {
            if (uiType == UIType.Screen)
            {
                CloseAllModalUIs(rightNow);
                CloseAllUnModalUIs(rightNow);
            }
            else
            {
                CloseAllSceneUIs(rightNow);
            }
        }

        /// <summary>
        /// 将UI加入到UI卸载列表
        /// </summary>
        /// <param name="flagBehaviour">UIFlagBehaviour组件</param>
        public void AddFlagToUnloadUIList(UIFlagBehaviour flagBehaviour)
        {
            if(flagBehaviour != null)
            {
                if (!m_UnloadUIList.Contains(flagBehaviour))
                {
                    m_UnloadUIList.Add(flagBehaviour);
                }
            }
        }

        /// <summary>
        /// 判断指定UI界面是否存在于模态UI集合中（非追加式UI）
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
        public bool IsUIExistInModalUIs(UIInfo uiInfo)
        {
            if (uiInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    if (m_CurModalUI.UIInfo.ABPath == uiInfo.ABPath && m_CurModalUI.UIInfo.AssetName == uiInfo.AssetName)
                    {
                        if (!m_UnloadUIList.Contains(m_CurModalUI))
                        {
                            return true;
                        }
                    }
                }
                foreach (UIInfo ui in m_ModalUIInfoList)
                {
                    if (ui.ABPath == uiInfo.ABPath && ui.AssetName == uiInfo.AssetName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="fontDatas">字体数据集合</param>
        /// <param name="ui">ui（当ui为null时表示APP当前所有ui的全局替换）</param>
        public void SetFont(List<LocalizationFontData> fontDatas, GameObject ui)
        {
            // 当前所有ui的Font方案全局替换
            if (ui == null)
            {
                // 菊花等待界面（屏幕UI）
                if (m_WaitingUI != null)
                {
                    RefreshTextComponentsAdaptationParams(m_WaitingUI.gameObject, fontDatas);
                }

                // 流程切换过渡界面（屏幕UI）
                if (m_TransitionUI != null)
                {
                    RefreshTextComponentsAdaptationParams(m_TransitionUI.gameObject, fontDatas);
                }

                // 模态界面（屏幕UI）
                if (m_CurModalUI != null)
                {
                    RefreshTextComponentsAdaptationParams(m_CurModalUI.gameObject, fontDatas);
                }

                // 非模态界面（屏幕UI）
                if (m_UnModalUIList != null)
                {
                    foreach (var unModalUI in m_UnModalUIList)
                    {
                        RefreshTextComponentsAdaptationParams(unModalUI.gameObject, fontDatas);
                    }
                }

                // 场景UI界面（场景UI）
                if (m_SceneUIList != null)
                {
                    foreach (var sceneUI in m_SceneUIList)
                    {
                        RefreshTextComponentsAdaptationParams(sceneUI.gameObject, fontDatas);
                    }
                }
            }
            else // 当前指定ui的Font替换
            {
                RefreshTextComponentsAdaptationParams(ui, fontDatas);
            }

            // 备份UI字体集合
            m_LastFonts.Clear();
            m_LastFonts.AddRange(m_Fonts);
            
        }

        /// <summary>
        /// 刷新屏幕宽高适比例配阀值
        /// </summary>
        /// <param name="matchValue">屏幕宽高适比例配阀值</param>
        public void RefreshScreenMatchValue(float matchValue)
        {
            m_ScreenUICanvasScaler.matchWidthOrHeight = matchValue;
        }

        /// <summary>
        /// 根据Screen、Canvas和安全区域大小来计算刘海区域大小
        /// </summary>
        public void InitBangsSize()
        {
            Vector2 screenCanvasSize = m_ScreenUICanvas.rectTransform().sizeDelta;
            Log.Info($"屏幕分辨率： {Screen.width} X {Screen.height}");
            Log.Info($"屏幕画布尺寸： {screenCanvasSize.x} X {screenCanvasSize.y}，屏幕画布缩放比例：{(screenCanvasSize.x / Screen.width).ToString("f2")}");
            m_UIBangsSize = Vector2.zero;
            if (Screen.width >= Screen.height)
            {
                // 横屏显示
                if (Screen.width > Screen.safeArea.width)
                {
                    // 有刘海
                    m_UIBangsSize = new Vector2(Screen.width - Screen.safeArea.width, Screen.height);
#if UNITY_IOS
                    m_UIBangsSize.x *= 0.5f;
#elif UNITY_EDITOR
                    if (m_UIBangsSize.x >= 100)
                    {
                        m_UIBangsSize.x *= 0.5f;
                    }
#endif
                }
            }
            else
            {
                // 竖屏显示
                if (Screen.height > Screen.safeArea.height)
                {
                    // 有刘海
                    m_UIBangsSize = new Vector2(Screen.width, Screen.height - Screen.safeArea.height);
#if UNITY_IOS
                    m_UIBangsSize.y *= 0.5f;
#elif UNITY_EDITOR
                    if (m_UIBangsSize.y >= 100)
                    {
                        m_UIBangsSize.y *= 0.5f;
                    }
#endif
                }
            }
            Log.Info($"刘海分辨率：{m_UIBangsSize.x} X {m_UIBangsSize.y}");

            // 从屏幕尺寸转为canvas尺寸
            if (Screen.width > 0)
            {
                m_UIBangsSize *= screenCanvasSize.x / Screen.width;
            }
            Log.Info($"刘海画布尺寸：{m_UIBangsSize.x} X {m_UIBangsSize.y}");
        }

        /// <summary>
        /// 检测屏幕方向是否发生改变，当发生改变时发送事件通知
        /// </summary>
        private void CheckScreenOrientationState()
        {
            // 当设置为自动旋转时才有可能会旋转屏幕
            if (UIManager.Instance != null && UIManager.Instance.CheckOrientationState)
            {
                if (Screen.autorotateToPortrait || Screen.autorotateToPortraitUpsideDown || Screen.autorotateToLandscapeLeft || Screen.autorotateToLandscapeRight)
                {
                    if (m_ScreenOrientation != Screen.orientation)
                    {
                        Dictionary<string, object> orientationParam = new Dictionary<string, object>();
                        orientationParam["LastOrientation"] = (int)m_ScreenOrientation;
                        orientationParam["CurOrientation"] = (int)Screen.orientation;
                        orientationParam["BangsWidth"] = m_UIBangsSize.x;
                        orientationParam["BangsHeight"] = m_UIBangsSize.y;
                        EventManager.Instance.DispatchEvent(this, EventCmd.ScreenOrientationChanged, orientationParam);
                        m_ScreenOrientation = Screen.orientation;
                    }
                }
            }
        }
    }
}