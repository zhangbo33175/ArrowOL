--=====================================================================================================
-- descrip:   设置数值管理器
--=====================================================================================================

---@class SettingDataManager @设置数值管理器
local SettingDataManager = class("SettingDataManager")

---构造函数
---@type fun(args:table):void
---@param args table @自定义参数
function SettingDataManager:ctor(args)

end

---创建函数
---@type fun(args:table):SettingDataMgr
---@param args table @自定义参数
---@return SettingDataMgr @区域金币管理器
function SettingDataManager:Create(args)
    local obj = SettingDataManager.new(args)
    return obj
end

---获取存档中记录的设置数据
---@type fun(settingType:ESettingType):boolean
---@param settingType ESettingType @Definitions定义的设置类型
---@return boolean @查询的设置类型当前是否开启，返回true表示开启
function SettingDataManager:GetSettingState(settingType)
    return SaveRootData.GameData.settingInfo[settingType]
end

---修改存档中记录的设置数据
---@type fun(settingType:ESettingType,newSetting:boolean):void
---@param settingType ESettingType @Definitions定义的设置类型
---@param newSetting boolean @新的开关开启状态，传入true，表示对应的开关关闭
function SettingDataManager:SetSettingNewState(settingType, newSetting)
    SaveRootData.GameData.settingInfo[settingType] = newSetting
    SaveIO.GameData.Save_settingInfo()
    -- 发送通知，关闭或者开启当前系统音乐
    if settingType == ESettingType.Music then
        SoundMgr:OnBGMCloseStateChanged(settingType, newSetting)
    end
    if settingType == ESettingType.Sound then
        SoundMgr:OnSoundCloseStateChanged(settingType, newSetting)
    end
end

--- 使用系统实际的推送权限状态矫正存档中的推送权限
function SettingDataManager:CorrectNotificationState()
    local allow = not self:GetSettingState(ESettingType.Notification)
    if allow then

    end
end
return SettingDataManager