using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 核心管理组件
    /// 负责 UI 的同步/异步加载、关闭、管理、相机管理、多语言字体适配、刘海屏适配
    /// 基于 GameComponent 生命周期运行，是全局唯一的 UI 入口
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// 初始化：框架生命周期入口
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 初始化管理器、配置、依赖
            Initialize();
        }

        /// <summary>
        /// 启动：初始化刘海屏适配
        /// </summary>
        private void Start()
        {
            // 初始化刘海区域大小
            if (m_UIManager != null)
            {
                m_UIManager.InitBangsSize();
            }
        }

        /// <summary>
        /// 每帧更新 UI 管理器逻辑
        /// </summary>
        private void Update()
        {
            if (m_UIManager != null)
            {
                m_UIManager.Update();
            }
        }

        /// <summary>
        /// 销毁（预留）
        /// </summary>
        private void OnDestroy()
        {
        }

        /// <summary>
        /// 通过 LuaTable 异步打开 UI
        /// </summary>
        /// <param name="luaTable">Lua 配置表</param>
        /// <param name="luaParams">传递参数</param>
        /// <param name="overCallback">加载完成回调</param>
        public void OpenUIAsyncByLuaTable(LuaTable luaTable, LuaTable luaParams = null,
            UILoadOverCallback overCallback = null)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.OpenUIAsyncByLuaTable luaTable 无效。");
                return;
            }

            OpenUIAsyncByInfo(GenerateUIInfo(luaTable, luaParams, overCallback));
        }

        /// <summary>
        /// 通过 UIInfo 异步打开 UI
        /// </summary>
        /// <param name="uiInfo">UI 配置信息</param>
        public void OpenUIAsyncByInfo(UIInfo uiInfo)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.OpenUIAsyncByInfo uiInfo 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.OpenUIAsyncByInfo uiInfo.ABPath 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.OpenUIAsyncByInfo uiInfo.AssetName 无效。");
                return;
            }

            m_UIManager.OpenUIAsyncByInfo(uiInfo);
        }

        /// <summary>
        /// 通过 LuaTable 同步打开 UI
        /// </summary>
        /// <param name="luaTable">Lua 配置表</param>
        /// <param name="luaParams">传递参数</param>
        /// <returns>UI 实例对象</returns>
        public GameObject OpenUISyncByLuaTable(LuaTable luaTable, LuaTable luaParams = null)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.OpenUISyncByLuaTable luaTable 无效。");
                return null;
            }
            var  go = OpenUISyncByInfo(GenerateUIInfo(luaTable, luaParams));
            return OpenUISyncByInfo(GenerateUIInfo(luaTable, luaParams));
        }

        /// <summary>
        /// 通过 UIInfo 同步打开 UI
        /// </summary>
        /// <param name="uiInfo">UI 配置信息</param>
        /// <returns>UI 实例对象</returns>
        public GameObject OpenUISyncByInfo(UIInfo uiInfo)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.OpenUISyncByInfo uiInfo 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.OpenUISyncByInfo uiInfo.ABPath 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.OpenUISyncByInfo uiInfo.AssetName 无效。");
                return null;
            }

            return m_UIManager.OpenUISyncByInfo(uiInfo);
        }

        /// <summary>
        /// 通过 LuaTable 异步追加子 UI 到指定父节点
        /// </summary>
        /// <param name="luaTable">Lua 配置表</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">参数</param>
        /// <param name="overCallback">完成回调</param>
        public void AddUIAsyncByLuaTable(LuaTable luaTable, Transform parent, LuaTable luaParams = null,
            UILoadOverCallback overCallback = null)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.AddUIAsyncByLuaTable luaTable 无效。");
                return;
            }

            AddUIAsyncByInfo(GenerateUIInfo(luaTable, luaParams, overCallback), parent);
        }

        /// <summary>
        /// 通过 UIInfo 异步追加子 UI
        /// </summary>
        /// <param name="uiInfo">UI 信息</param>
        /// <param name="parent">父节点</param>
        public void AddUIAsyncByInfo(UIInfo uiInfo, Transform parent)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.AddUIAsyncByInfo uiInfo 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.AddUIAsyncByInfo uiInfo.ABPath 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.AddUIAsyncByInfo uiInfo.AssetName 无效。");
                return;
            }

            if (parent == null)
            {
                Log.Error("UIComponent.AddUIAsyncByInfo parent 无效。");
                return;
            }

            m_UIManager.AddUIAsyncByInfo(uiInfo, parent);
        }

        /// <summary>
        /// 通过 LuaTable 同步追加子 UI
        /// </summary>
        public GameObject AddUISyncByLuaTable(LuaTable luaTable, Transform parent, LuaTable luaParams = null)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.AddUISyncByLuaTable luaTable 无效。");
                return null;
            }

            return AddUISyncByInfo(GenerateUIInfo(luaTable, luaParams), parent);
        }

        /// <summary>
        /// 通过 UIInfo 同步追加子 UI
        /// </summary>
        public GameObject AddUISyncByInfo(UIInfo uiInfo, Transform parent)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.AddUISyncByInfo uiInfo 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.AddUISyncByInfo uiInfo.ABPath 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.AddUISyncByInfo uiInfo.AssetName 无效。");
                return null;
            }

            if (parent == null)
            {
                Log.Error("UIComponent.AddUISyncByInfo parent 无效。");
                return null;
            }

            return m_UIManager.AddUISyncByInfo(uiInfo, parent);
        }

        /// <summary>
        /// 通过 GameObject 关闭 UI
        /// </summary>
        /// <param name="uiGO">UI 对象</param>
        /// <param name="rightNow">是否立即关闭</param>
        public void CloseUIByGO(GameObject uiGO, bool rightNow = false)
        {
            if (uiGO == null)
            {
                Log.Error("UIComponent.CloseUIByGO uiGO 无效。");
                return;
            }

            m_UIManager.CloseUIByGO(uiGO, rightNow);
        }

        /// <summary>
        /// 通过 LuaTable 关闭 UI（匹配到一个即关闭）
        /// </summary>
        public void CloseUIByLuaTable(LuaTable luaTable, bool rightNow = false)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.CloseUIByLuaTable luaTable 无效。");
                return;
            }

            CloseUIByInfo(GenerateUIInfo(luaTable), rightNow);
        }

        /// <summary>
        /// 通过 UIInfo 关闭 UI（匹配到一个即关闭）
        /// </summary>
        public void CloseUIByInfo(UIInfo uiInfo, bool rightNow = false)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.CloseUIByInfo uiInfo 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.CloseUIByInfo uiInfo.ABPath 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.CloseUIByInfo uiInfo.AssetName 无效。");
                return;
            }

            m_UIManager.CloseUIByInfo(uiInfo, rightNow);
        }

        /// <summary>
        /// 通过 LuaTable 关闭所有匹配的 UI
        /// </summary>
        public void CloseUIsByLuaTable(LuaTable luaTable, bool rightNow = false)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.CloseUIsByLuaTable luaTable 无效。");
                return;
            }

            CloseUIsByInfo(GenerateUIInfo(luaTable), rightNow);
        }

        /// <summary>
        /// 通过 UIInfo 关闭所有匹配的 UI
        /// </summary>
        public void CloseUIsByInfo(UIInfo uiInfo, bool rightNow = false)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.CloseUIsByInfo uiInfo 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.CloseUIsByInfo uiInfo.ABPath 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.CloseUIsByInfo uiInfo.AssetName 无效。");
                return;
            }

            m_UIManager.CloseUIsByInfo(uiInfo, rightNow);
        }

        /// <summary>
        /// 移除子 UI（追加模式）
        /// </summary>
        public void RemoveUIByGO(GameObject uiGO, bool rightNow = false)
        {
            if (uiGO == null)
            {
                Log.Error("UIComponent.RemoveUIByGO uiGO 无效。");
                return;
            }

            m_UIManager.RemoveUIByGO(uiGO, rightNow);
        }

        /// <summary>
        /// 通过 LuaTable 获取 UI 对象
        /// </summary>
        public GameObject GetUIByLuaTable(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.GetUIByLuaTable luaTable 无效。");
                return null;
            }

            return GetUIByInfo(GenerateUIInfo(luaTable));
        }

        /// <summary>
        /// 通过 UIInfo 获取 UI 对象
        /// </summary>
        public GameObject GetUIByInfo(UIInfo uiInfo)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.GetUIByInfo uiInfo 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.GetUIByInfo uiInfo.ABPath 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.GetUIByInfo uiInfo.AssetName 无效。");
                return null;
            }

            return m_UIManager.GetUIByInfo(uiInfo);
        }

        /// <summary>
        /// 通过 LuaTable 获取一组 UI
        /// </summary>
        public GameObject[] GetUIsByLuaTable(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.GetUIsByLuaTable luaTable 无效。");
                return null;
            }

            return GetUIsByInfo(GenerateUIInfo(luaTable));
        }

        /// <summary>
        /// 通过 LuaTable 获取一组 UI（输出到 List）
        /// </summary>
        public void GetUIsByLuaTable(LuaTable luaTable, List<GameObject> uis)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.GetUIsByLuaTable luaTable 无效。");
                return;
            }

            if (uis == null)
            {
                Log.Error("UIComponent.GetUIsByLuaTable uis 无效。");
                return;
            }

            GetUIsByInfo(GenerateUIInfo(luaTable), uis);
        }

        /// <summary>
        /// 通过 UIInfo 获取一组 UI
        /// </summary>
        public GameObject[] GetUIsByInfo(UIInfo uiInfo)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.GetUIsByInfo uiInfo 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.GetUIsByInfo uiInfo.ABPath 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.GetUIsByInfo uiInfo.AssetName 无效。");
                return null;
            }

            return m_UIManager.GetUIsByInfo(uiInfo);
        }

        /// <summary>
        /// 通过 UIInfo 获取一组 UI（输出到 List）
        /// </summary>
        public void GetUIsByInfo(UIInfo uiInfo, List<GameObject> uis)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.GetUIsByInfo uiInfo 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.GetUIsByInfo uiInfo.ABPath 无效。");
                return;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.GetUIsByInfo uiInfo.AssetName 无效。");
                return;
            }

            if (uis == null)
            {
                Log.Error("UIComponent.GetUIsByInfo uis 无效。");
                return;
            }

            m_UIManager.GetUIsByInfo(uiInfo, uis);
        }

        /// <summary>
        /// 根据 UIType 获取一组 UI
        /// </summary>
        public GameObject[] GetUIsByUIType(UIType uiType, bool isAppend)
        {
            return m_UIManager.GetUIsByUIType(uiType, isAppend);
        }

        /// <summary>
        /// 根据 UIType 获取一组 UI（输出到 List）
        /// </summary>
        public void GetUIsByUIType(UIType uiType, bool isAppend, List<GameObject> uis)
        {
            if (uis == null)
            {
                Log.Error("UIComponent.GetUIsByUIType uis 无效。");
                return;
            }

            m_UIManager.GetUIsByUIType(uiType, isAppend, uis);
        }

        /// <summary>
        /// 关闭所有模态窗口
        /// </summary>
        public void CloseAllModalUIs(bool rightNow = false)
        {
            m_UIManager.CloseAllModalUIs(rightNow);
        }

        /// <summary>
        /// 关闭所有非模态窗口
        /// </summary>
        public void CloseAllUnModalUIs(bool rightNow = false)
        {
            m_UIManager.CloseAllUnModalUIs(rightNow);
        }

        /// <summary>
        /// 关闭指定类型的所有 UI
        /// </summary>
        public void CloseAllUIs(UIType uiType, bool rightNow = false)
        {
            m_UIManager.CloseAllUIs(uiType, rightNow);
        }

        /// <summary>
        /// 将 UI 标记为可卸载
        /// </summary>
        public void AddToUnloadUIList(GameObject go)
        {
            if (go == null)
            {
                Log.Error("UIComponent.AddToUnloadUIList go 无效。");
                return;
            }

            m_UIManager.AddFlagToUnloadUIList(go.GetComponent<UIFlagBehaviour>());
        }

        /// <summary>
        /// 判断 UI 是否存在于模态队列中
        /// </summary>
        public bool IsUIExistInModalUIs(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.IsUIExistInModalUIs luaTable 无效。");
                return false;
            }

            return IsUIExistInModalUIs(GenerateUIInfo(luaTable));
        }

        /// <summary>
        /// 判断 UI 是否存在于模态队列中
        /// </summary>
        public bool IsUIExistInModalUIs(UIInfo uiInfo)
        {
            if (uiInfo == null)
            {
                Log.Error("UIComponent.IsUIExistInModalUIs uiInfo 无效。");
                return false;
            }

            if (string.IsNullOrEmpty(uiInfo.ABPath))
            {
                Log.Error("UIComponent.IsUIExistInModalUIs uiInfo.ABPath 无效。");
                return false;
            }

            if (string.IsNullOrEmpty(uiInfo.AssetName))
            {
                Log.Error("UIComponent.IsUIExistInModalUIs uiInfo.AssetName 无效。");
                return false;
            }

            return m_UIManager.IsUIExistInModalUIs(uiInfo);
        }

        /// <summary>
        /// 添加场景 UI 相机
        /// </summary>
        public int AddSceneUICamera(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("UIComponent.AddSceneUICamera camera 无效。");
                return -1;
            }

            if (!m_SceneUICameras.Contains(camera))
            {
                m_SceneUICameras.Add(camera);
            }

            return m_SceneUICameras.FindIndex((obj) => { return obj == camera; });
        }

        /// <summary>
        /// 移除场景 UI 相机
        /// </summary>
        public bool RemoveSceneUICamera(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("UIComponent.RemoveSceneUICamera camera 无效。");
                return false;
            }

            return m_SceneUICameras.Remove(camera);
        }

        /// <summary>
        /// 根据索引移除场景 UI 相机
        /// </summary>
        public bool RemoveSceneUICameraByIndex(int index)
        {
            if (index < 0 || index >= m_SceneUICameras.Count)
            {
                Log.Error("UIComponent.RemoveSceneUICameraByIndex index 无效。");
                return false;
            }

            m_SceneUICameras.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// 获取场景 UI 相机索引
        /// </summary>
        public int GetSceneUICameraIndex(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("UIComponent.GetSceneUICameraIndex camera 无效。");
                return -1;
            }

            return m_SceneUICameras.FindIndex((obj) => { return obj == camera; });
        }

        /// <summary>
        /// 根据索引获取场景 UI 相机
        /// </summary>
        public Camera GetSceneUICamera(int index)
        {
            if (index < 0 || index >= m_SceneUICameras.Count)
            {
                Log.Error("UIComponent.GetSceneUICamera index 无效。");
                return null;
            }

            return m_SceneUICameras[index];
        }

        /// <summary>
        /// 设置场景相机启用状态
        /// </summary>
        public void SetSceneUICameraEnable(bool enabled, int index = -1)
        {
            if (index < -1 || index >= m_SceneUICameras.Count)
            {
                Log.Error("UIComponent.SetSceneUICameraEnable index 无效。");
            }

            if (index == -1)
            {
                m_SceneUICameras.ForEach(camera => camera.enabled = enabled);
            }
            else
            {
                GetSceneUICamera(index).enabled = enabled;
            }
        }

        /// <summary>
        /// 添加屏幕 UI 相机
        /// </summary>
        public int AddScreenUICamera(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("UIComponent.AddScreenUICamera camera 无效。");
                return -1;
            }

            if (!m_ScreenUICameras.Contains(camera))
            {
                m_ScreenUICameras.Add(camera);
            }

            return m_ScreenUICameras.FindIndex((obj) => { return obj == camera; });
        }

        /// <summary>
        /// 移除屏幕 UI 相机
        /// </summary>
        public bool RemoveScreenUICamera(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("UIComponent.RemoveScreenUICamera camera 无效。");
                return false;
            }

            return m_ScreenUICameras.Remove(camera);
        }

        /// <summary>
        /// 根据索引移除屏幕 UI 相机
        /// </summary>
        public bool RemoveScreenUICameraByIndex(int index)
        {
            if (index < 0 || index >= m_ScreenUICameras.Count)
            {
                Log.Error("UIComponent.RemoveScreenUICameraByIndex index 无效。");
                return false;
            }

            m_ScreenUICameras.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// 获取屏幕 UI 相机索引
        /// </summary>
        public int GetScreenUICameraIndex(Camera camera)
        {
            if (camera == null)
            {
                Log.Error("UIComponent.GetScreenUICameraIndex camera 无效。");
                return -1;
            }

            return m_ScreenUICameras.FindIndex((obj) => { return obj == camera; });
        }

        /// <summary>
        /// 根据索引获取屏幕 UI 相机
        /// </summary>
        public Camera GetScreenUICamera(int index)
        {
            if (index < 0 || index >= m_ScreenUICameras.Count)
            {
                Log.Error("UIComponent.GetScreenUICamera index 无效。");
                return null;
            }

            return m_ScreenUICameras[index];
        }

        /// <summary>
        /// 设置屏幕相机启用状态
        /// </summary>
        public void SetScreenUICameraEnable(bool enabled, int index = -1)
        {
            if (index < -1 || index >= m_ScreenUICameras.Count)
            {
                Log.Error("UIComponent.SetScreenUICameraEnable index 无效。");
            }

            if (index == -1)
            {
                m_ScreenUICameras.ForEach(camera => camera.enabled = enabled);
            }
            else
            {
                GetScreenUICamera(index).enabled = enabled;
            }
        }

        /// <summary>
        /// 加载多语言字体（自动适配）
        /// </summary>
        public void LoadFonts()
        {
            if (!m_LocalizationComponent.AutoFontAdapt)
            {
                return;
            }

            if (m_UIManager.Fonts.Count == 0)
            {
                m_LocalizationComponent.GetFontData(m_LocalizationComponent.Language,
                    out List<LocalizationFontData> fontDatas);

                if (fontDatas == null || fontDatas.Count == 0)
                {
                    Log.Error("UIComponent.LoadMainFont fontDatas 无效。");
                    return;
                }

                foreach (var data in fontDatas)
                {
                    m_UIManager.Fonts.Add(m_AssetComponent.LoadAssetSync(data.FontType, data.ABPath, data.AssetName));
                }
            }

            return;
        }

        /// <summary>
        /// 卸载多语言字体
        /// </summary>
        public void UnloadFonts(bool rightNow = false)
        {
            if (!m_LocalizationComponent.AutoFontAdapt)
            {
                return;
            }

            foreach (var obj in m_UIManager.Fonts)
            {
                m_AssetComponent.UnloadAsset(obj, null, rightNow);
            }

            m_UIManager.Fonts.Clear();
        }

        /// <summary>
        /// 刷新 UI 字体（语言切换后调用）
        /// </summary>
        public void RefreshFontsForUI(GameObject ui = null)
        {
            if (!m_LocalizationComponent.AutoFontAdapt)
            {
                return;
            }

            if (m_UIManager.Fonts.Count > 0)
            {
                m_LocalizationComponent.GetFontData(m_LocalizationComponent.Language,
                    out List<LocalizationFontData> fontDatas);

                if (fontDatas == null || fontDatas.Count == 0)
                {
                    Log.Error("UIComponent.RefreshFontsForUI fontDatas 无效。");
                    return;
                }

                m_UIManager.SetFont(fontDatas, ui);
            }

            return;
        }

        /// <summary>
        /// 刷新屏幕适配比例（平板/手机自适应）
        /// 自动根据宽高比切换适配模式，并重新计算刘海
        /// </summary>
        public void RefreshScreenMatchValue()
        {
            // 在非iOS设备上面屏幕宽高比超过一定数值则按照pad模式对屏幕适配阀值进行重新赋值
            if ((Screen.width >= Screen.height && (Screen.width * 1.0f / Screen.height <= 16.0f / 10)) ||
                (Screen.width < Screen.height && (Screen.width * 1.0f / Screen.height >= 3.0f / 4)))
            {
                ScreenWidthHeightMatchValue = m_ConfigComponent.GetFloat("PadScreenMatchValue");
                // 重新计算刘海位置
                m_UIManager.InitBangsSize();
            }
        }
    }
}