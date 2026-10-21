/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorCoreManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   核心模块门面管理器 - 业务便捷方法转发（partial）
 *            收敛各核心组件对外能力，外部核心业务调用统一走本管理器
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class HonorCoreManager
    {
        //=========================================================================

        #region 事件（EventComponent）

        //=========================================================================

        /// <summary>
        /// 触发全局事件（异步）
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="cmd">事件命令</param>
        /// <param name="objects">事件参数（键值对），可选</param>
        public void Fire(object sender, GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            Event.Fire(sender, cmd, objects);
        }

        /// <summary>
        /// 订阅全局事件
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">订阅者标识</param>
        /// <param name="handler">事件回调</param>
        public void Subscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            Event.Subscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 取消订阅全局事件
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">订阅者标识</param>
        /// <param name="handler">事件回调</param>
        public void Unsubscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            Event.Unsubscribe(cmd, userData, handler);
        }

        #endregion

        //=========================================================================

        #region 音频（SoundComponent）

        //=========================================================================

        /// <summary>
        /// 播放音频（通过 LuaTable 参数）
        /// </summary>
        /// <param name="luaTable">音频播放参数表</param>
        /// <returns>音频序列号</returns>
        public int PlaySound(LuaTable luaTable)
        {
            return Sound.PlaySound(luaTable);
        }

        /// <summary>
        /// 停止指定音频
        /// </summary>
        /// <param name="serialID">音频序列号</param>
        /// <returns>停止成功返回true</returns>
        public bool StopSound(int serialID)
        {
            return Sound.StopSound(serialID);
        }

        /// <summary>
        /// 设置音频组音量
        /// </summary>
        /// <param name="newVolume">新音量（0~1）</param>
        /// <param name="groupName">音频组名称</param>
        public void SetAllSoundVolume(float newVolume, string groupName)
        {
            Sound.SetAllSoundVolume(newVolume, groupName);
        }

        #endregion

        //=========================================================================

        #region 配置（ConfigComponent）

        //=========================================================================

        /// <summary>
        /// 加载所有配置
        /// </summary>
        public void LoadConfigs()
        {
            Config.LoadConfigs();
        }

        /// <summary>
        /// 读取配置布尔值
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否读取平台配置，默认false</param>
        /// <returns>配置值</returns>
        public bool GetConfigBool(string configName, bool onPlatform = false)
        {
            return Config.GetBool(configName, onPlatform);
        }

        /// <summary>
        /// 读取配置整数值
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否读取平台配置，默认false</param>
        /// <returns>配置值</returns>
        public int GetConfigInt(string configName, bool onPlatform = false)
        {
            return Config.GetInt(configName, onPlatform);
        }

        /// <summary>
        /// 读取配置字符串值
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否读取平台配置，默认false</param>
        /// <returns>配置值</returns>
        public string GetConfigString(string configName, bool onPlatform = false)
        {
            return Config.GetString(configName, onPlatform);
        }

        #endregion

        //=========================================================================

        #region 震动（VibrateComponent）

        //=========================================================================

        /// <summary>
        /// 播放默认震动
        /// </summary>
        public void VibratePlay()
        {
            Vibrate.Play();
        }

        /// <summary>
        /// 停止所有震动
        /// </summary>
        public void VibrateStopAll()
        {
            Vibrate.StopAll();
        }

        #endregion

        //=========================================================================

        #region 计时器（TimerComponent）

        //=========================================================================

        /// <summary>
        /// 添加倒计时器
        /// </summary>
        /// <param name="time">倒计时长（秒）</param>
        /// <param name="del">倒计时结束回调</param>
        /// <param name="obj">关联游戏对象</param>
        /// <param name="owner">计时器归属标识</param>
        /// <returns>计时器对象</returns>
        public TimerCounter AddTimerCounter(float time, Action<string> del, GameObject obj, string owner = "")
        {
            return Timer.AddTimerCounter(time, del, obj, owner);
        }

        /// <summary>
        /// 移除指定归属的计时器
        /// </summary>
        /// <param name="owner">计时器归属标识</param>
        public void RemoveTimer(string owner)
        {
            Timer.RemoveTimer(owner);
        }

        #endregion

        //=========================================================================

        #region 多语言（LocalizationComponent）

        //=========================================================================

        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="language">目标语言</param>
        /// <param name="launchSet">是否启动时设置，默认false</param>
        public void SetLanguage(GameDefinitions.Language language, bool launchSet = false)
        {
            Localization.SetLanguage(language, launchSet);
        }

        /// <summary>
        /// 读取本地化文本
        /// </summary>
        /// <param name="keyName">文本键名</param>
        /// <returns>本地化文本</returns>
        public string GetLocalizationText(string keyName)
        {
            return Localization.GetDefaultData(keyName);
        }

        #endregion

        //=========================================================================

        #region 持久化（PersistComponent）

        //=========================================================================

        /// <summary>
        /// 按持久化方式保存数据
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        public void SavePersist(PersistWayType wayType)
        {
            Persist.Save(wayType);
        }

        #endregion

        //=========================================================================

        #region Lua（LuaComponent）

        //=========================================================================

        /// <summary>
        /// 初始化 Lua 环境
        /// </summary>
        public void InitLuaEnv()
        {
            Lua.InitLuaEnv();
        }

        /// <summary>
        /// 重置 Lua 环境
        /// </summary>
        public void ResetLua()
        {
            Lua.ResetLua();
        }

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

        /// <summary>
        /// 同步触发全局事件（转发自 EventComponent）
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="cmd">事件命令</param>
        /// <param name="objects">事件参数（键值对），可选</param>
        public void FireNow(object sender, GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            Event.FireNow(sender, cmd, objects);
        }

        /// <summary>
        /// 读取配置浮点值（转发自 ConfigComponent）
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="onPlatform">是否读取平台配置，默认false</param>
        /// <returns>配置值</returns>
        public float GetConfigFloat(string configName, bool onPlatform = false)
        {
            return Config.GetFloat(configName, onPlatform);
        }

        /// <summary>
        /// 获取当前构建平台名称（转发自 ConfigComponent）
        /// </summary>
        /// <returns>平台名称：iOS / WebGL / Android / Amazon</returns>
        public string GetCurBuildPlatformName()
        {
            return Config.GetCurBuildPlatformName();
        }

        /// <summary>
        /// 读取本地化默认文本（转发自 LocalizationComponent）
        /// </summary>
        /// <param name="keyName">文本键名</param>
        /// <returns>本地化文本</returns>
        public string GetDefaultData(string keyName)
        {
            return Localization.GetDefaultData(keyName);
        }

        /// <summary>
        /// 加载默认语言列表（转发自 LocalizationComponent）
        /// </summary>
        public void LoadDefaultLanguages()
        {
            Localization.LoadDefaultLanguages();
        }

        /// <summary>
        /// 初始化当前语言（转发自 LocalizationComponent）
        /// </summary>
        public void InitCurLanguage()
        {
            Localization.InitCurLanguage();
        }

        /// <summary>
        /// 加载默认多语言数据表（转发自 LocalizationComponent）
        /// </summary>
        public void LoadDefaultDatas()
        {
            Localization.LoadDefaultDatas();
        }

        /// <summary>
        /// 加载字体数据（转发自 LocalizationComponent）
        /// </summary>
        public void LoadFontDatas()
        {
            Localization.LoadFontDatas();
        }

        /// <summary>
        /// 获取指定语言对应的字体列表（转发自 LocalizationComponent）
        /// </summary>
        /// <param name="language">目标语言</param>
        /// <param name="fontDatas">字体列表输出</param>
        public void GetFontData(GameDefinitions.Language language, out List<LocalizationFontData> fontDatas)
        {
            Localization.GetFontData(language, out fontDatas);
        }

        /// <summary>
        /// 清理 Lua 环境（转发自 LuaComponent）
        /// </summary>
        public void Clear()
        {
            Lua.Clear();
        }

        /// <summary>
        /// 初始化 Lua 配置（转发自 LuaComponent）
        /// </summary>
        public void InitLuaConfigs()
        {
            Lua.InitLuaConfigs();
        }

        /// <summary>
        /// 确保 Lua 脚本 AB 路径已枚举（转发自 LuaComponent）
        /// </summary>
        public void EnsureLuaScriptPathsBuilt()
        {
            Lua.EnsureLuaScriptPathsBuilt();
        }

        /// <summary>
        /// 待预取 Lua 脚本模块数量（转发自 LuaComponent）
        /// </summary>
        public int LuaPrefetchModuleCount => Lua != null ? Lua.LuaPrefetchModuleCount : 0;

        /// <summary>
        /// 预取下一个 Lua 脚本模块（转发自 LuaComponent）
        /// </summary>
        /// <returns>仍有待预取模块返回true</returns>
        public bool PrefetchNextLuaModule()
        {
            return Lua.PrefetchNextLuaModule();
        }

        /// <summary>
        /// 构建 Lua 预加载执行计划（转发自 LuaComponent）
        /// </summary>
        public void BuildLuaPreloadExecutionPlan()
        {
            Lua.BuildLuaPreloadExecutionPlan();
        }

        /// <summary>
        /// 执行下一个 Lua 预加载模块（转发自 LuaComponent）
        /// </summary>
        /// <returns>仍有待执行模块返回true</returns>
        public bool ExecuteNextLuaPreloadModule()
        {
            return Lua.ExecuteNextLuaPreloadModule();
        }

        /// <summary>
        /// 执行 Lua 入口主体（转发自 LuaComponent）
        /// </summary>
        /// <returns>执行成功返回true</returns>
        public bool ExecuteStartGameBody()
        {
            return Lua.ExecuteStartGameBody();
        }

        /// <summary>
        /// 绑定框架级 Lua 回调（转发自 LuaComponent）
        /// </summary>
        public void InitLuaBindings()
        {
            Lua.InitLuaBindings();
        }

        /// <summary>
        /// 绑定游戏级 Lua 回调（转发自 LuaComponent）
        /// </summary>
        public void InitGameLuaBindings()
        {
            Lua.InitGameLuaBindings();
        }

        /// <summary>
        /// 播放音频（按资源参数，转发自 SoundComponent）
        /// </summary>
        /// <param name="abPath">音频所在 AB 包路径</param>
        /// <param name="assetName">音频资源名称</param>
        /// <param name="soundGroupName">音频组名称</param>
        /// <param name="playSoundParams">播放参数，可选</param>
        /// <param name="worldPosition">世界坐标（3D 音效），可选</param>
        /// <returns>音频序列号</returns>
        public int PlaySound(string abPath, string assetName, string soundGroupName,
            PlaySoundParams playSoundParams = null, Vector3 worldPosition = default)
        {
            return Sound.PlaySound(abPath, assetName, soundGroupName, playSoundParams, worldPosition);
        }

        /// <summary>
        /// 带淡出时间停止指定音频（转发自 SoundComponent）
        /// </summary>
        /// <param name="serialID">音频序列号</param>
        /// <param name="fadeOutSeconds">淡出时长（秒）</param>
        /// <returns>停止成功返回true</returns>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            return Sound.StopSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 停止指定音频组全部声音（转发自 SoundComponent）
        /// </summary>
        /// <param name="groupName">音频组名称</param>
        /// <returns>停止成功返回true</returns>
        public bool StopGroupSound(string groupName)
        {
            return Sound.StopGroupSound(groupName);
        }

        /// <summary>
        /// 设置震动总开关（转发自 VibrateComponent）
        /// </summary>
        /// <param name="enable">是否启用震动</param>
        public void SetEnable(bool enable)
        {
            Vibrate.SetEnable(enable);
        }

        /// <summary>
        /// 判断持久化数据是否存在（转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <returns>存在返回true</returns>
        public bool HasItem(PersistWayType wayType, string classifyName, string itemName)
        {
            return Persist.HasItem(wayType, classifyName, itemName);
        }

        /// <summary>
        /// 读取持久化字符串（无默认值，转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <returns>字符串值</returns>
        public string GetString(PersistWayType wayType, string classifyName, string itemName)
        {
            return Persist.GetString(wayType, classifyName, itemName);
        }

        /// <summary>
        /// 读取持久化字符串（带默认值，转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串值</returns>
        public string GetString(PersistWayType wayType, string classifyName, string itemName, string defaultValue)
        {
            return Persist.GetString(wayType, classifyName, itemName, defaultValue);
        }

        /// <summary>
        /// 写入持久化字符串（转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <param name="value">字符串值</param>
        public void SetString(PersistWayType wayType, string classifyName, string itemName, string value)
        {
            Persist.SetString(wayType, classifyName, itemName, value);
        }

        /// <summary>
        /// 读取持久化整数（无默认值，转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <returns>整数值</returns>
        public int GetInt(PersistWayType wayType, string classifyName, string itemName)
        {
            return Persist.GetInt(wayType, classifyName, itemName);
        }

        /// <summary>
        /// 写入持久化整数（转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <param name="value">整数值</param>
        public void SetInt(PersistWayType wayType, string classifyName, string itemName, int value)
        {
            Persist.SetInt(wayType, classifyName, itemName, value);
        }

        /// <summary>
        /// 读取持久化布尔值（无默认值，转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <returns>布尔值</returns>
        public bool GetBool(PersistWayType wayType, string classifyName, string itemName)
        {
            return Persist.GetBool(wayType, classifyName, itemName);
        }

        /// <summary>
        /// 读取持久化布尔值（带默认值，转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>布尔值</returns>
        public bool GetBool(PersistWayType wayType, string classifyName, string itemName, bool defaultValue)
        {
            return Persist.GetBool(wayType, classifyName, itemName, defaultValue);
        }

        /// <summary>
        /// 写入持久化布尔值（转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        /// <param name="itemName">键名</param>
        /// <param name="value">布尔值</param>
        public void SetBool(PersistWayType wayType, string classifyName, string itemName, bool value)
        {
            Persist.SetBool(wayType, classifyName, itemName, value);
        }

        /// <summary>
        /// 保存全部持久化数据（转发自 PersistComponent，与既有 SavePersist 并存）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        public void Save(PersistWayType wayType)
        {
            Persist.Save(wayType);
        }

        /// <summary>
        /// 按分类保存持久化数据（转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名</param>
        public void Save(PersistWayType wayType, string classifyName)
        {
            Persist.Save(wayType, classifyName);
        }

        /// <summary>
        /// 清空指定方式的全部持久化数据（转发自 PersistComponent）
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        public void RemoveAllItems(PersistWayType wayType)
        {
            Persist.RemoveAllItems(wayType);
        }

        /// <summary>
        /// 播放默认震动（转发自 VibrateComponent）
        /// </summary>
        public void Play()
        {
            Vibrate.Play();
        }

        /// <summary>
        /// 播放指定类型震动（转发自 VibrateComponent）
        /// </summary>
        /// <param name="type">震动类型</param>
        public void Play(VibrateType type)
        {
            Vibrate.Play(type);
        }

        /// <summary>
        /// 当前设备是否支持震动（转发自 VibrateComponent）
        /// </summary>
        /// <returns>支持返回true</returns>
        public bool IsSupported()
        {
            return Vibrate.IsSupported();
        }

        /// <summary>
        /// 获取震动开关状态（转发自 VibrateComponent）
        /// </summary>
        /// <returns>启用返回true</returns>
        public bool GetEnable()
        {
            return Vibrate.GetEnable();
        }

        #endregion
    }
}
