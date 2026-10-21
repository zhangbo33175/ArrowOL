/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUIManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI 模块门面管理器 - 业务便捷方法转发（partial）
 *            收敛 UI/场景组件的对外能力，外部 UI 相关业务调用统一走本管理器
 ***************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class HonorUIManager
    {
        //=========================================================================

        #region UI（UIComponent）

        //=========================================================================

        /// <summary>
        /// 异步打开界面（按 UIInfo）
        /// </summary>
        /// <param name="uiInfo">界面信息</param>
        public void OpenUIAsyncByInfo(UIInfo uiInfo)
        {
            UI.OpenUIAsyncByInfo(uiInfo);
        }

        /// <summary>
        /// 关闭界面（按 UIInfo）
        /// </summary>
        /// <param name="uiInfo">界面信息</param>
        /// <param name="rightNow">是否立即关闭，默认false（延迟淡出）</param>
        public void CloseUIByInfo(UIInfo uiInfo, bool rightNow = false)
        {
            UI.CloseUIByInfo(uiInfo, rightNow);
        }

        #endregion

        //=========================================================================

        #region 场景（SceneComponent）

        //=========================================================================

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="abPath">场景所在AB包路径</param>
        /// <param name="assetName">场景名称</param>
        /// <param name="overCallback">加载完成回调，可选</param>
        public void LoadSceneAsync(string abPath, string assetName, SceneLoadOverCallback overCallback = null)
        {
            Scene.LoadSceneAsync(abPath, assetName, overCallback);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="overCallback">卸载完成回调，可选</param>
        public void UnloadScene(string sceneName, SceneUnloadOverCallback overCallback = null)
        {
            Scene.UnloadScene(sceneName, overCallback);
        }

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

        /// <summary>
        /// 关闭全部指定类型的界面（转发自 UIComponent）
        /// </summary>
        /// <param name="uiType">界面类型</param>
        /// <param name="rightNow">是否立即关闭，默认false（延迟淡出）</param>
        public void CloseAllUIs(UIType uiType, bool rightNow = false)
        {
            UI.CloseAllUIs(uiType, rightNow);
        }

        /// <summary>
        /// 销毁场景根节点下全部动态生成的游戏对象（转发自 SceneComponent）
        /// </summary>
        /// <param name="root">根节点，默认null时使用场景根节点</param>
        public void DestroyAllSceneGOs(GameObject root = null)
        {
            Scene.DestroyAllSceneGOs(root);
        }

        /// <summary>
        /// 通过 LuaTable 异步打开界面（转发自 UIComponent）
        /// </summary>
        /// <param name="luaTable">界面参数表</param>
        /// <param name="luaParams">附加 Lua 参数，可选</param>
        /// <param name="overCallback">打开完成回调，可选</param>
        public void OpenUIAsyncByLuaTable(LuaTable luaTable, LuaTable luaParams = null, UILoadOverCallback overCallback = null)
        {
            UI.OpenUIAsyncByLuaTable(luaTable, luaParams, overCallback);
        }

        /// <summary>
        /// 通过 LuaTable 同步添加界面（转发自 UIComponent）
        /// </summary>
        /// <param name="luaTable">界面参数表</param>
        /// <param name="parent">实例化父节点</param>
        /// <param name="luaParams">附加 Lua 参数，可选</param>
        /// <returns>界面根游戏对象</returns>
        public GameObject AddUISyncByLuaTable(LuaTable luaTable, Transform parent, LuaTable luaParams = null)
        {
            return UI.AddUISyncByLuaTable(luaTable, parent, luaParams);
        }

        /// <summary>
        /// 通过 LuaTable 关闭界面（转发自 UIComponent）
        /// </summary>
        /// <param name="luaTable">界面参数表</param>
        /// <param name="rightNow">是否立即关闭，默认false</param>
        public void CloseUIByLuaTable(LuaTable luaTable, bool rightNow = false)
        {
            UI.CloseUIByLuaTable(luaTable, rightNow);
        }

        /// <summary>
        /// 按游戏对象关闭界面（转发自 UIComponent）
        /// </summary>
        /// <param name="uiGO">界面根游戏对象</param>
        /// <param name="rightNow">是否立即关闭，默认false</param>
        public void CloseUIByGO(GameObject uiGO, bool rightNow = false)
        {
            UI.CloseUIByGO(uiGO, rightNow);
        }

        /// <summary>
        /// 将游戏对象加入待卸载 UI 列表（转发自 UIComponent）
        /// </summary>
        /// <param name="go">待卸载的游戏对象</param>
        public void AddToUnloadUIList(GameObject go)
        {
            UI.AddToUnloadUIList(go);
        }

        /// <summary>
        /// 淡出加载界面并关闭（转发自 UIComponent）
        /// </summary>
        public void FadeOutLoadingViewThenClose()
        {
            UI.FadeOutLoadingViewThenClose();
        }

        /// <summary>
        /// 遮罩层界面引用计数 +1（转发自 UIComponent）
        /// </summary>
        /// <param name="name">遮罩层名称</param>
        public void AddUIMaskLayerRef(string name)
        {
            UI.AddUIMaskLayerRef(name);
        }

        /// <summary>
        /// 遮罩层界面引用计数 -1（转发自 UIComponent）
        /// </summary>
        public void SubUIMaskLayerRef()
        {
            UI.SubUIMaskLayerRef();
        }

        /// <summary>
        /// 启动协程（转发自 LauncherComponent）
        /// </summary>
        /// <param name="routine">协程迭代器</param>
        /// <returns>协程句柄</returns>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return Launcher.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止指定协程（转发自 LauncherComponent）
        /// </summary>
        /// <param name="routine">协程句柄</param>
        public void StopCoroutine(Coroutine routine)
        {
            Launcher.StopCoroutine(routine);
        }

        /// <summary>
        /// 停止全部协程（转发自 LauncherComponent）
        /// </summary>
        public void StopAllCoroutines()
        {
            Launcher.StopAllCoroutines();
        }

        /// <summary>
        /// 加载多语言字体（自动适配，转发自 UIComponent）
        /// </summary>
        public void LoadFonts()
        {
            UI.LoadFonts();
        }

        /// <summary>
        /// 卸载多语言字体（转发自 UIComponent）
        /// </summary>
        /// <param name="rightNow">是否立即卸载，默认false</param>
        public void UnloadFonts(bool rightNow = false)
        {
            UI.UnloadFonts(rightNow);
        }

        /// <summary>
        /// 刷新 UI 字体（语言切换后调用，转发自 UIComponent）
        /// </summary>
        /// <param name="ui">目标界面根节点，默认null表示刷新全部</param>
        public void RefreshFontsForUI(GameObject ui = null)
        {
            UI.RefreshFontsForUI(ui);
        }

        /// <summary>
        /// 通过 LuaTable 同步打开 UI（转发自 UIComponent）
        /// </summary>
        /// <param name="luaTable">Lua 配置表</param>
        /// <param name="luaParams">传递参数，可选</param>
        /// <returns>UI 实例对象</returns>
        public GameObject OpenUISyncByLuaTable(LuaTable luaTable, LuaTable luaParams = null)
        {
            return UI.OpenUISyncByLuaTable(luaTable, luaParams);
        }

        /// <summary>
        /// 通过 LuaTable 异步追加子 UI 到指定父节点（转发自 UIComponent）
        /// </summary>
        /// <param name="luaTable">Lua 配置表</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">参数，可选</param>
        /// <param name="overCallback">完成回调，可选</param>
        public void AddUIAsyncByLuaTable(LuaTable luaTable, Transform parent, LuaTable luaParams = null, UILoadOverCallback overCallback = null)
        {
            UI.AddUIAsyncByLuaTable(luaTable, parent, luaParams, overCallback);
        }

        #endregion
    }
}
