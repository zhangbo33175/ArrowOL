/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIManager.Core.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI 管理器 - 核心逻辑实现
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class UIManager
    {
        #region 构造与初始化
        /// <summary>
        /// UI 管理器构造函数
        /// 初始化所有组件、根节点、分辨率适配、UI 容器、默认状态等
        /// </summary>
        public UIManager(AssetComponent assetComponent, LocalizationComponent localizationComponent, UIComponent uiComponent,
                         List<Camera> screenUICameras, List<Camera> sceneUICameras,
                         Canvas screenUICanvas, Canvas sceneUICanvas,
                         Vector2 screenDesignedResolution, float screenWidthHeightMatchValue,
                         int destroyMaxNumPerFrame,
                         bool checkTextLocalizings,
                         string waitingUIABPath, string waitingUIAssetName,
                         string floatWordsUIABPath, string floatWordsUIAssetName, float floatWordsDuration)
        {
            m_AssetComponent = assetComponent;
            m_LocalizationComponent = localizationComponent;
            m_UIComponent = uiComponent;
            m_ScreenUICameras = screenUICameras;
            m_SceneUICameras = sceneUICameras;
            m_ScreenUICanvas = screenUICanvas;
            m_SceneUICanvas = sceneUICanvas;
            m_DestroyMaxNumPerFrame = destroyMaxNumPerFrame;
            m_CheckTextLocalizings = checkTextLocalizings;
            m_WaitingUIABPath = waitingUIABPath;
            m_WaitingUIAssetName = waitingUIAssetName;
            m_FloatWordsUIABPath = floatWordsUIABPath;
            m_FloatWordsUIAssetName = floatWordsUIAssetName;
            m_FloatWordsDuration = floatWordsDuration;

            // 获取并校验屏幕 UI 画布适配组件
            m_ScreenUICanvasScaler = m_ScreenUICanvas.GetComponent<CanvasScaler>();
            if (m_ScreenUICanvasScaler == null)
            {
                Log.Fatal("Screen UI Canvas Scaler 无效。");
                return;
            }
            // 设置屏幕 UI 分辨率适配
            m_ScreenUICanvasScaler.referenceResolution = screenDesignedResolution;
            m_ScreenUICanvasScaler.matchWidthOrHeight = screenWidthHeightMatchValue;

            // 获取并校验屏幕 UI 射线投射组件
            m_ScreenUIGraphicRaycaster = m_ScreenUICanvas.GetComponent<GraphicRaycaster>();
            if (m_ScreenUIGraphicRaycaster == null)
            {
                Log.Fatal("Screen UI GraphicRaycaster 无效。");
                return;
            }

            // 获取并校验屏幕 UI 画布组组件
            m_ScreenUICanvasGroup = m_ScreenUICanvas.GetComponent<CanvasGroup>();
            if (m_ScreenUICanvasGroup == null)
            {
                Log.Fatal("Screen UI CanvasGroup 无效。");
                return;
            }

            // 获取并校验场景 UI 画布适配组件
            m_SceneUICanvasScaler = m_SceneUICanvas.GetComponent<CanvasScaler>();
            if (m_SceneUICanvasScaler == null)
            {
                Log.Fatal("Scene UI Canvas Scaler 无效。");
                return;
            }

            // 获取并校验场景 UI 射线投射组件
            m_SceneUIGraphicRaycaster = m_SceneUICanvas.GetComponent<GraphicRaycaster>();
            if (m_SceneUIGraphicRaycaster == null)
            {
                Log.Fatal("Scene UI GraphicRaycaster 无效。");
                return;
            }

            // 获取并校验场景 UI 画布组组件
            m_SceneUICanvasGroup = m_SceneUICanvas.GetComponent<CanvasGroup>();
            if (m_SceneUICanvasGroup == null)
            {
                Log.Fatal("Scene UI CanvasGroup 无效。");
                return;
            }

            // 初始化所有 UI 管理容器
            m_Fonts = new List<Object>();
            m_LastFonts = new List<Object>();
            m_BlockModalUIsSwitch = false;
            m_BlockAllUIsKeyUpSwitch = false;
            m_PermanentUIs = new List<GameObject>();
            m_CurModalUI = null;
            m_ModalUIInfoList = new List<UIInfo>();
            m_UnModalUIList = new List<UIFlagBehaviour>();
            m_SceneUIList = new List<UIFlagBehaviour>();
            m_SubUIList = new Dictionary<UIType, List<UIFlagBehaviour>>();
            m_SubUIList[UIType.Screen] = new List<UIFlagBehaviour>();
            m_SubUIList[UIType.Scene] = new List<UIFlagBehaviour>();
            m_UnloadUIList = new List<UIFlagBehaviour>();
        }
        #endregion

        #region 生命周期更新
        /// <summary>
        /// UI 管理器帧更新（心跳）
        /// 按优先级更新所有 UI 生命周期、队列、销毁、输入、屏幕状态
        /// </summary>
        public void Update()
        {
            UpdateCurModalUI();
            UpdateModalUIInfoList();
            UpdateUnModalUIList();
            UpdateSubUIList(UIType.Screen);
            UpdateSceneUIList();
            UpdateSubUIList(UIType.Scene);
            UpdateDestroyUIList();
            CheckKeysUp();
            CheckScreenOrientationState();
        }
        #endregion

        #region 打开 UI（异步/同步）
        /// <summary>
        /// 异步打开 UI 界面（根据 UI 信息）
        /// 自动区分屏幕/场景、模态/非模态，支持队列与优先级
        /// </summary>
        public void OpenUIAsyncByInfo(UIInfo uiInfo)
        {
            if (uiInfo.UIType == UIType.Screen)
            {
                if (uiInfo.IsModal)
                {
                    // 无阻塞且当前无模态 UI，直接打开
                    if (!m_BlockModalUIsSwitch && m_CurModalUI == null)
                    {
                        m_AssetComponent.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams, (PrefabObject prefabObject, GameObject go) =>
                        {
                            AddWebGLInput(go);
                            AddCanvas(go, uiInfo);
                            m_CurModalUI = AddFlagBehaviour(go, uiInfo);
                            m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                            m_UIComponent.RefreshFontsForUI(go);
                            DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
                        });
                    }
                    else
                    {
                        // 加入模态队列并按优先级排序
                        m_ModalUIInfoList.Add(uiInfo);
                        m_ModalUIInfoList.Sort((ui1, ui2) => { return ui1.Priority - ui2.Priority; });
                    }
                }
                else
                {
                    // 异步加载非模态 UI
                    m_AssetComponent.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams, (PrefabObject prefabObject, GameObject go) =>
                    {
                        AddWebGLInput(go);
                        AddCanvas(go, uiInfo);
                        m_UnModalUIList.Add(AddFlagBehaviour(go, uiInfo));
                        m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                        m_UIComponent.RefreshFontsForUI(go);
                        DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
                    });
                }
            }
            else
            {
                // 异步加载场景 UI
                m_AssetComponent.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, m_SceneUICanvas.transform, uiInfo.LuaParams, (PrefabObject prefabObject, GameObject go) =>
                {
                    AddWebGLInput(go);
                    AddCanvas(go, uiInfo);
                    m_SceneUIList.Add(AddFlagBehaviour(go, uiInfo));
                    m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                    m_UIComponent.RefreshFontsForUI(go);
                    DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
                });
            }
        }

        /// <summary>
        /// 同步打开 UI 界面（根据 UI 信息）
        /// 立即加载并返回 UI 对象，阻塞执行
        /// </summary>
        public GameObject OpenUISyncByInfo(UIInfo uiInfo)
        {
            if (uiInfo.UIType == UIType.Screen)
            {
                if (uiInfo.IsModal)
                {
                    if (!m_BlockModalUIsSwitch && m_CurModalUI == null)
                    {
                        GameObject go = m_AssetComponent.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams);
                        AddWebGLInput(go);
                        AddCanvas(go, uiInfo);
                        m_CurModalUI = AddFlagBehaviour(go, uiInfo);
                        m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                        m_UIComponent.RefreshFontsForUI(go);
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
                    GameObject go = m_AssetComponent.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, m_ScreenUICanvas.transform, uiInfo.LuaParams);
                    AddWebGLInput(go);
                    AddCanvas(go, uiInfo);
                    m_UnModalUIList.Add(AddFlagBehaviour(go, uiInfo));
                    m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                    m_UIComponent.RefreshFontsForUI(go);
                    return go;
                }
            }
            else
            {
                GameObject go = m_AssetComponent.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, m_SceneUICanvas.transform, uiInfo.LuaParams);
                AddWebGLInput(go);
                AddCanvas(go, uiInfo);       
                m_SceneUIList.Add(AddFlagBehaviour(go, uiInfo));
                m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                m_UIComponent.RefreshFontsForUI(go);
                return go;
            }
        }
        #endregion

        #region 追加子 UI（异步/同步）
        /// <summary>
        /// 异步追加子 UI 到指定父节点（附加式 UI）
        /// 不独立管理生命周期，跟随父 UI
        /// </summary>
        public void AddUIAsyncByInfo(UIInfo uiInfo, Transform parent)
        {
            m_AssetComponent.LoadPrefabAsync(uiInfo.ABPath, uiInfo.AssetName, parent, uiInfo.LuaParams, (PrefabObject prefabObject, GameObject go) =>
            {
                uiInfo.IsAppend = true;
                AddWebGLInput(go);
                m_SubUIList[uiInfo.UIType].Add(AddFlagBehaviour(go, uiInfo));
                m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
                m_UIComponent.RefreshFontsForUI(go);
                DoUICreateOverCallbackOnAsync(go, uiInfo, prefabObject);
            });
        }

        /// <summary>
        /// 同步追加子 UI 到指定父节点（附加式 UI）
        /// </summary>
        public GameObject AddUISyncByInfo(UIInfo uiInfo, Transform parent)
        {
            GameObject go = m_AssetComponent.LoadPrefabSync(uiInfo.ABPath, uiInfo.AssetName, parent, uiInfo.LuaParams);
            uiInfo.IsAppend = true;
            AddWebGLInput(go);
            m_SubUIList[uiInfo.UIType].Add(AddFlagBehaviour(go, uiInfo));
            m_SubUIList[uiInfo.UIType].AddRange(AddChildrenFlagBehaviours(go));
            m_UIComponent.RefreshFontsForUI(go);
            return go;
        }
        #endregion

        #region 关闭 UI（单个/批量）
        /// <summary>
        /// 根据 GameObject 关闭 UI
        /// 自动跳过常驻 UI，支持立即/延时关闭
        /// </summary>
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
                            if (result != null) break;
                        }
                    }
                }
                else if(targetFlag.UIInfo.UIType == UIType.Scene)
                {
                    foreach (var sceneUI in m_SceneUIList)
                    {
                        result = GetMatchedGOGameObject(sceneUI, targetFlag);
                        if (result != null) break;
                    }
                }
                InnerCloseUIByGO(result, rightNow);
            }
        }

        /// <summary>
        /// 根据 UI 信息关闭单个 UI
        /// 找到第一个匹配项立即关闭并返回
        /// </summary>
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
                        if (result != null) break;
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
                    if (result != null) break;
                }
            }

            if (result != null && m_PermanentUIs.Contains(result)) result = null;
            InnerCloseUIByGO(result, rightNow);
        }

        /// <summary>
        /// 根据 UI 信息关闭所有匹配的 UI
        /// 批量关闭，全部匹配后统一执行
        /// </summary>
        public void CloseUIsByInfo(UIInfo targetUIInfo, bool rightNow)
        {
            if (targetUIInfo == null) return;
            
            List<GameObject> result = new List<GameObject>();

            if (targetUIInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                    result.AddRange(GetAllMatchedUIInfoGameObjects(m_CurModalUI, targetUIInfo));
                
                foreach (var unModalUI in m_UnModalUIList)
                    result.AddRange(GetAllMatchedUIInfoGameObjects(unModalUI, targetUIInfo));
                
                RemoveAllMatchedUIInfosFromModalUIInfoList(targetUIInfo);
            }
            else if (targetUIInfo.UIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                    result.AddRange(GetAllMatchedUIInfoGameObjects(sceneUI, targetUIInfo));
            }

            result.RemoveAll(go => m_PermanentUIs.Contains(go));
            result.ForEach(go => InnerCloseUIByGO(go, rightNow));
        }

        /// <summary>
        /// 移除追加式 UI（同关闭 UI）
        /// </summary>
        public void RemoveUIByGO(GameObject uiGO, bool rightNow)
        {
            CloseUIByGO(uiGO, rightNow);
        }
        #endregion

        #region 获取 UI
        /// <summary>
        /// 根据 UI 信息获取单个 UI 对象
        /// </summary>
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
        /// 根据 UI 信息获取所有匹配的 UI 数组
        /// </summary>
        public GameObject[] GetUIsByInfo(UIInfo targetUIInfo)
        {
            List<GameObject> results = new List<GameObject>();
            GetUIsByInfo(targetUIInfo, results);
            return results.ToArray();
        }

        /// <summary>
        /// 根据 UI 信息获取所有匹配的 UI（列表版）
        /// </summary>
        public void GetUIsByInfo(UIInfo targetUIInfo, List<GameObject> result)
        {
            if (targetUIInfo == null || result == null) return;
            result.Clear();

            if (targetUIInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                    result.AddRange(GetAllMatchedUIInfoValidGameObjects(m_CurModalUI, targetUIInfo));
                
                foreach (var unModalUI in m_UnModalUIList)
                    result.AddRange(GetAllMatchedUIInfoValidGameObjects(unModalUI, targetUIInfo));
            }
            else if (targetUIInfo.UIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                    result.AddRange(GetAllMatchedUIInfoValidGameObjects(sceneUI, targetUIInfo));
            }
        }

        /// <summary>
        /// 根据 UI 类型获取所有 UI 数组
        /// </summary>
        public GameObject[] GetUIsByUIType(UIType targetUIType, bool isAppend)
        {
            List<GameObject> result = new List<GameObject>();
            GetUIsByUIType(targetUIType, isAppend, result);
            return result.ToArray();
        }

        /// <summary>
        /// 根据 UI 类型获取所有 UI（列表版）
        /// </summary>
        public void GetUIsByUIType(UIType targetUIType, bool isAppend, List<GameObject> result)
        {
            if (result == null) return;
            result.Clear();

            if (targetUIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                    result.AddRange(GetAllMatchedUITypeValidGameObjects(m_CurModalUI, targetUIType, isAppend));
                
                foreach (var unModalUI in m_UnModalUIList)
                    result.AddRange(GetAllMatchedUITypeValidGameObjects(unModalUI, targetUIType, isAppend));
            }
            else if (targetUIType == UIType.Scene)
            {
                foreach (var sceneUI in m_SceneUIList)
                    result.AddRange(GetAllMatchedUITypeValidGameObjects(sceneUI, targetUIType, isAppend));
            }
        }
        #endregion

        #region 批量关闭
        /// <summary>
        /// 关闭所有模态 UI（屏幕 UI）
        /// 清空当前模态与等待队列
        /// </summary>
        public void CloseAllModalUIs(bool rightNow)
        {
            if (m_CurModalUI != null)
                InnerCloseUIByGO(m_CurModalUI.gameObject, rightNow);
            
            m_ModalUIInfoList.Clear();
        }

        /// <summary>
        /// 关闭所有非模态 UI（屏幕 UI）
        /// </summary>
        public void CloseAllUnModalUIs(bool rightNow)
        {
            List<GameObject> result = new List<GameObject>();

            if (m_CurModalUI != null)
                result.AddRange(GetAllMatchedUnModalGameObjects(m_CurModalUI));
            
            foreach (var unModalUI in m_UnModalUIList)
                result.AddRange(GetAllMatchedUnModalGameObjects(unModalUI));
            
            result.RemoveAll(cmp => m_PermanentUIs.Contains(cmp));
            result.ForEach(go => InnerCloseUIByGO(go, rightNow));
        }

        /// <summary>
        /// 关闭所有场景 UI
        /// </summary>
        public void CloseAllSceneUIs(bool rightNow)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (var sceneUI in m_SceneUIList)
                result.AddRange(GetAllMatchedSceneUIGameObjects(sceneUI));
            
            result.RemoveAll(cmp => m_PermanentUIs.Contains(cmp));
            result.ForEach(go => InnerCloseUIByGO(go, rightNow));
        }

        /// <summary>
        /// 关闭指定类型的所有 UI
        /// </summary>
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
        #endregion

        #region 工具与状态
        /// <summary>
        /// 将 UI 加入待卸载列表
        /// 用于分帧销毁，避免卡顿
        /// </summary>
        public void AddFlagToUnloadUIList(UIFlagBehaviour flagBehaviour)
        {
            if(flagBehaviour != null && !m_UnloadUIList.Contains(flagBehaviour))
                m_UnloadUIList.Add(flagBehaviour);
        }

        /// <summary>
        /// 判断指定 UI 是否存在于模态 UI 体系中
        /// </summary>
        public bool IsUIExistInModalUIs(UIInfo uiInfo)
        {
            if (uiInfo.UIType == UIType.Screen)
            {
                if (m_CurModalUI != null)
                {
                    if (m_CurModalUI.UIInfo.ABPath == uiInfo.ABPath && m_CurModalUI.UIInfo.AssetName == uiInfo.AssetName)
                    {
                        return !m_UnloadUIList.Contains(m_CurModalUI);
                    }
                }
                foreach (UIInfo ui in m_ModalUIInfoList)
                {
                    if (ui.ABPath == uiInfo.ABPath && ui.AssetName == uiInfo.AssetName)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 全局/指定 UI 替换字体
        /// 支持多语言字体切换，自动刷新所有文本
        /// </summary>
        public void SetFont(List<LocalizationFontData> fontDatas, GameObject ui)
        {
            if (ui == null)
            {
                // 全局刷新所有已打开 UI
                if (_mConnectionWaitingUIConnection != null)
                    RefreshTextComponentsAdaptationParams(_mConnectionWaitingUIConnection.gameObject, fontDatas);

                if (m_TransitionUI != null)
                    RefreshTextComponentsAdaptationParams(m_TransitionUI.gameObject, fontDatas);

                if (m_CurModalUI != null)
                    RefreshTextComponentsAdaptationParams(m_CurModalUI.gameObject, fontDatas);

                foreach (var unModalUI in m_UnModalUIList)
                    RefreshTextComponentsAdaptationParams(unModalUI.gameObject, fontDatas);

                foreach (var sceneUI in m_SceneUIList)
                    RefreshTextComponentsAdaptationParams(sceneUI.gameObject, fontDatas);
            }
            else
            {
                RefreshTextComponentsAdaptationParams(ui, fontDatas);
            }

            // 备份上一次字体
            m_LastFonts.Clear();
            m_LastFonts.AddRange(m_Fonts);
        }

        /// <summary>
        /// 刷新屏幕宽高匹配值
        /// 动态调整分辨率适配比例
        /// </summary>
        public void RefreshScreenMatchValue(float matchValue)
        {
            m_ScreenUICanvasScaler.matchWidthOrHeight = matchValue;
        }

        /// <summary>
        /// 初始化刘海屏安全区域尺寸
        /// 根据屏幕、画布、安全区域自动计算偏移
        /// </summary>
        public void InitBangsSize()
        {
            Vector2 screenCanvasSize = m_ScreenUICanvas.rectTransform().sizeDelta;
            Log.Info($"屏幕分辨率： {Screen.width} X {Screen.height}");
            Log.Info($"屏幕画布尺寸： {screenCanvasSize.x} X {screenCanvasSize.y}，缩放比例：{(screenCanvasSize.x / Screen.width):F2}");
            
            m_UIBangsSize = Vector2.zero;
            if (Screen.width >= Screen.height)
            {
                // 横屏
                if (Screen.width > Screen.safeArea.width)
                {
                    m_UIBangsSize = new Vector2(Screen.width - Screen.safeArea.width, Screen.height);
#if UNITY_IOS
                    m_UIBangsSize.x *= 0.5f;
#elif UNITY_EDITOR
                    if (m_UIBangsSize.x >= 100) m_UIBangsSize.x *= 0.5f;
#endif
                }
            }
            else
            {
                // 竖屏
                if (Screen.height > Screen.safeArea.height)
                {
                    m_UIBangsSize = new Vector2(Screen.width, Screen.height - Screen.safeArea.height);
#if UNITY_IOS
                    m_UIBangsSize.y *= 0.5f;
#elif UNITY_EDITOR
                    if (m_UIBangsSize.y >= 100) m_UIBangsSize.y *= 0.5f;
#endif
                }
            }
            Log.Info($"刘海分辨率：{m_UIBangsSize.x} X {m_UIBangsSize.y}");

            // 转为画布坐标系
            if (Screen.width > 0)
                m_UIBangsSize *= screenCanvasSize.x / Screen.width;
            
            Log.Info($"刘海画布尺寸：{m_UIBangsSize.x} X {m_UIBangsSize.y}");
        }

        /// <summary>
        /// 检测屏幕方向变化
        /// 变化时派发全局事件，用于 UI 自动适配横竖屏
        /// </summary>
        private void CheckScreenOrientationState()
        {
            if (m_UIComponent == null || !m_UIComponent.CheckOrientationState) return;

            if (Screen.autorotateToPortrait || Screen.autorotateToPortraitUpsideDown || 
                Screen.autorotateToLandscapeLeft || Screen.autorotateToLandscapeRight)
            {
                if (m_ScreenOrientation != Screen.orientation)
                {
                    var orientationParam = new Dictionary<string, object>
                    {
                        ["LastOrientation"] = (int)m_ScreenOrientation,
                        ["CurOrientation"] = (int)Screen.orientation,
                        ["BangsWidth"] = m_UIBangsSize.x,
                        ["BangsHeight"] = m_UIBangsSize.y
                    };
                    GameMainRoot.Event.Fire(this, GameEventCmd.ScreenOrientationChanged, orientationParam);
                    m_ScreenOrientation = Screen.orientation;
                }
            }
        }
        #endregion
    }
}