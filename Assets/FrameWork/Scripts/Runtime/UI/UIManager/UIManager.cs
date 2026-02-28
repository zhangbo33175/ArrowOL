using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public delegate void UILoadOverCallback(string abPath, string assetName, LuaTable luaClass, GameObject obj);

    public sealed partial class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// 异步打开UI界面
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="luaParams">lua参数</param>
        /// <param name="overCallback">加载完成回调</param>
        /// <returns></returns>
        public void OpenUIAsyncByLuaTable(LuaTable luaTable, LuaTable luaParams = null,UILoadOverCallback overCallback = null)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.OpenUIAsyncByLuaTable luaTable 无效。");
                return;
            }

            OpenUIAsyncByInfo(GenerateUIInfo(luaTable, luaParams, overCallback));
        }

        /// <summary>
        /// 异步打开UI界面
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
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

            m_UIComponent.OpenUIAsyncByInfo(uiInfo);
        }

        /// <summary>
        /// 同步打开UI界面
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="luaParams">lua参数</param>
        /// <returns></returns>
        public GameObject OpenUISyncByLuaTable(LuaTable luaTable, LuaTable luaParams = null)
        {
            if (luaTable == null)
            {
                Log.Error("UIComponent.OpenUISyncByLuaTable luaTable 无效。");
                return null;
            }

            return OpenUISyncByInfo(GenerateUIInfo(luaTable, luaParams));
        }

        /// <summary>
        /// 同步打开UI界面
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
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

            return m_UIComponent.OpenUISyncByInfo(uiInfo);
        }

        /// <summary>
        /// 异步追加UI到主体UI上
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="parent">指定的父对象（必须为UI主体下的对象节点）</param>
        /// <param name="luaParams">lua参数</param>
        /// <param name="overCallback">异步回调函数</param>
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
        /// 异步追加UI到主体UI上
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="parent">指定的父对象（必须为UI主体下的对象节点）</param>
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

            m_UIComponent.AddUIAsyncByInfo(uiInfo, parent);
        }

        /// <summary>
        /// 同步追加UI到主体UI上
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="parent">指定的父对象（必须为UI主体下的对象节点）</param>
        /// <param name="luaParams">lua参数</param>
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
        /// 同步追加UI到主体UI上
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="parent">指定的父对象（必须为UI主体下的对象节点）</param>
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

            return m_UIComponent.AddUISyncByInfo(uiInfo, parent);
        }

        /// <summary>
        /// 关闭UI界面
        /// </summary>
        /// <param name="uiGO">UI对象</param>
        /// <param name="rightNow">马上</param>
        /// <returns></returns>
        public void CloseUIByGO(GameObject uiGO, bool rightNow = false)
        {
            if (uiGO == null)
            {
                Log.Error("UIComponent.CloseUIByGO uiGO 无效。");
                return;
            }

            m_UIComponent.CloseUIByGO(uiGO, rightNow);
        }

        /// <summary>
        /// 关闭UI界面
        /// 检查到任意一个匹配UI即触发关闭并返回
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="rightNow">马上</param>
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
        /// 关闭UI界面
        /// 检查到任意一个匹配UI即触发关闭并返回
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="rightNow">马上</param>
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

            m_UIComponent.CloseUIByInfo(uiInfo, rightNow);
        }

        /// <summary>
        /// 关闭UI界面
        /// 检查到所有匹配UI，统一触发关闭并返回
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="rightNow">马上</param>
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
        /// 关闭UI界面
        /// 检查到所有匹配UI，统一触发关闭并返回
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="rightNow">马上</param>
        /// <returns></returns>
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

            m_UIComponent.CloseUIsByInfo(uiInfo, rightNow);
        }

        /// <summary>
        /// 移除主体UI上的追加UI
        /// </summary>
        /// <param name="uiGO">UI对象</param>
        /// <param name="rightNow">马上</param>
        public void RemoveUIByGO(GameObject uiGO, bool rightNow = false)
        {
            if (uiGO == null)
            {
                Log.Error("UIComponent.RemoveUIByGO uiGO 无效。");
                return;
            }

            m_UIComponent.RemoveUIByGO(uiGO, rightNow);
        }

        /// <summary>
        /// 获取UI界面
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <returns></returns>
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
        /// 获取UI界面
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
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

            return m_UIComponent.GetUIByInfo(uiInfo);
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <returns></returns>
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
        /// 获取UI界面集合
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <param name="uis">ui集合</param>
        /// <returns></returns>
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
        /// 获取UI界面集合
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
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

            return m_UIComponent.GetUIsByInfo(uiInfo);
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <param name="uis">ui集合</param>
        /// <returns></returns>
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

            m_UIComponent.GetUIsByInfo(uiInfo, uis);
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="uiType">UI界面类型</param>
        /// <returns></returns>
        public GameObject[] GetUIsByUIType(UIType uiType, bool isAppend)
        {
            return m_UIComponent.GetUIsByUIType(uiType, isAppend);
        }

        /// <summary>
        /// 获取UI界面集合
        /// </summary>
        /// <param name="uiType">UI界面类型</param>
        /// <param name="uis">ui集合</param>
        public void GetUIsByUIType(UIType uiType, bool isAppend, List<GameObject> uis)
        {
            if (uis == null)
            {
                Log.Error("UIComponent.GetUIsByUIType uis 无效。");
                return;
            }

            m_UIComponent.GetUIsByUIType(uiType, isAppend, uis);
        }

        /// <summary>
        /// 关闭所有模态UI界面
        /// </summary>
        /// <param name="rightNow">马上</param>
        public void CloseAllModalUIs(bool rightNow = false)
        {
            m_UIComponent.CloseAllModalUIs(rightNow);
        }

        /// <summary>
        /// 关闭所有非模态UI界面
        /// </summary>
        /// <param name="rightNow">马上</param>
        public void CloseAllUnModalUIs(bool rightNow = false)
        {
            m_UIComponent.CloseAllUnModalUIs(rightNow);
        }

        /// <summary>
        /// 关闭所有UI界面
        /// </summary>
        /// <param name="uiType">UI界面类型</param>
        /// <param name="rightNow">马上</param>
        public void CloseAllUIs(UIType uiType, bool rightNow = false)
        {
            m_UIComponent.CloseAllUIs(uiType, rightNow);
        }

        /// <summary>
        /// 将UI加入到UI卸载列表（非追加式UI）
        /// </summary>
        /// <param name="go">GameObject</param>
        public void AddToUnloadUIList(GameObject go)
        {
            if (go == null)
            {
                Log.Error("UIComponent.AddToUnloadUIList go 无效。");
                return;
            }

            m_UIComponent.AddFlagToUnloadUIList(go.GetComponent<UIFlagBehaviour>());
        }

        /// <summary>
        /// 判断指定UI界面是否存在于模态UI集合中
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <returns></returns>
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
        /// 判断指定UI界面是否存在于模态UI集合中
        /// </summary>
        /// <param name="uiInfo">UI信息</param>
        /// <returns></returns>
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

            return m_UIComponent.IsUIExistInModalUIs(uiInfo);
        }

        /// <summary>
        /// 追加场景UI相机
        /// </summary>
        /// <param name="camera">场景UI相机</param>
        /// <returns>场景UI相机索引值</returns>
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
        /// 移除场景UI相机
        /// </summary>
        /// <param name="camera">场景UI相机</param>
        /// <returns>是否移除成功</returns>
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
        /// 根据索引值移除场景UI相机
        /// </summary>
        /// <param name="index">场景UI相机索引值</param>
        /// <returns>是否移除成功</returns>
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
        /// 获取场景UI相机索引值
        /// </summary>
        /// <param name="camera">场景UI相机</param>
        /// <returns>场景UI相机索引值</returns>
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
        /// 获取场景UI相机
        /// </summary>
        /// <param name="index">场景UI相机索引值</param>
        /// <returns>场景UI相机</returns>
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
        /// 设置场景UI相机是否可用
        /// </summary>
        /// <param name="enabled">可用</param>
        /// <param name="index">索引值</param>
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
        /// 追加屏幕UI相机
        /// </summary>
        /// <param name="camera">屏幕UI相机</param>
        /// <returns>屏幕UI相机索引值</returns>
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
        /// 移除屏幕UI相机
        /// </summary>
        /// <param name="camera">屏幕UI相机</param>
        /// <returns>是否移除成功</returns>
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
        /// 根据索引值移除屏幕UI相机
        /// </summary>
        /// <param name="index">屏幕UI相机索引值</param>
        /// <returns>是否移除成功</returns>
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
        /// 获取屏幕UI相机索引值
        /// </summary>
        /// <param name="camera">屏幕UI相机</param>
        /// <returns>屏幕UI相机索引值</returns>
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
        /// 获取屏幕UI相机
        /// </summary>
        /// <param name="index">屏幕UI相机索引值</param>
        /// <returns>屏幕UI相机</returns>
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
        /// 设置屏幕UI相机是否可用
        /// </summary>
        /// <param name="enabled">可用</param>
        /// <param name="index">索引值</param>
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
        /// 加载字体资源集合
        /// </summary>
        public void LoadFonts()
        {
            if (!LocalizationManager.Instance().AutoFontAdapt)
            {
                return;
            }

            if (m_UIComponent.Fonts.Count == 0)
            {
                LocalizationManager.Instance().GetFontDatas(LocalizationManager.Instance().Language,
                    out List<LocalizationFontData> fontDatas);

                if (fontDatas == null || fontDatas.Count == 0)
                {
                    Log.Error("UIComponent.LoadMainFont fontDatas 无效。");
                    return;
                }

                foreach (var data in fontDatas)
                {
                    m_UIComponent.Fonts.Add(AssetManager.Instance.LoadAssetSync(data.FontType, data.ABPath, data.AssetName));
                }
            }

            return;
        }

        /// <summary>
        /// 卸载字体资源
        /// </summary>
        /// <param name="rightNow">马上</param>
        public void UnloadFonts(bool rightNow = false)
        {
            if (!LocalizationManager.Instance().AutoFontAdapt)
            {
                return;
            }

            foreach (var obj in m_UIComponent.Fonts)
            {
                AssetManager.Instance.UnloadAsset(obj, null, rightNow);
            }

            m_UIComponent.Fonts.Clear();
        }

        /// <summary>
        /// 刷新当前语言的字体到UI
        /// 当打开UI时使用（不建议频繁调用）
        /// </summary>
        /// <param name="ui">ui（不传入时表示刷新当前所有ui）</param>
        public void RefreshFontsForUI(GameObject ui = null)
        {
            if (!LocalizationManager.Instance().AutoFontAdapt)
            {
                return;
            }

            if (m_UIComponent.Fonts.Count > 0)
            {
                LocalizationManager.Instance().GetFontDatas(LocalizationManager.Instance().Language,
                    out List<LocalizationFontData> fontDatas);

                if (fontDatas == null || fontDatas.Count == 0)
                {
                    Log.Error("UIComponent.RefreshFontsForUI fontDatas 无效。");
                    return;
                }

                m_UIComponent.SetFont(fontDatas, ui);
            }

            return;
        }

        /// <summary>
        /// 刷新屏幕宽高适比例配阀值
        /// </summary>
        public void RefreshScreenMatchValue()
        {
            if ((Screen.width >= Screen.height && (Screen.width * 1.0f / Screen.height <= 16.0f / 10)) ||
                (Screen.width < Screen.height && (Screen.width * 1.0f / Screen.height >= 3.0f / 4)))
            {
                ScreenWidthHeightMatchValue = ConfigManager.Instance.GetFloat("PadScreenMatchValue");
                // 重新计算刘海位置
                m_UIComponent.InitBangsSize();
            }
        }

        public override void Dispose()
        {
            
        }
    }
}