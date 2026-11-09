# -*- coding: utf-8 -*-
import re, sys
files = [
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\GameCore\GameCoreManagerData.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\GameCoreManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\GameManager\GameManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\GameCore\GameCoreManagerEffect.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\GameCore\GameCoreManagerLevelFinish.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\GameCore\GameCoreManagerUseItem.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameEffectManager\GameEffectManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\MapManager\MapManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameCoreManager\PlayGameplayCycle.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\SectionManager\SectionManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\UIStackManager\UIStackManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\MainPlayingManager\MainPlayingManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\ItemManager\ItemManager.lua.txt',
 r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\GameMainUIPushManager\GameMainUIPushManager.lua.txt',
]
def bal(p):
    s = open(p,'rb').read().decode('utf-8-sig')
    s = re.sub(r'--\[\[.*?\]\]', '', s, flags=re.S)
    lines=[]
    for ln in s.splitlines():
        ln = re.sub(r'--.*$','',ln)
        ln = re.sub(r'"(?:[^"\\]|\\.)*"','""',ln)
        lines.append(ln)
    code='\n'.join(lines)
    o=len(re.findall(r'\b(if|for|while|function|do)\b',code))
    c=len(re.findall(r'\bend\b',code))
    fd=len(re.findall(r'\bfor\b[^\n]*\bdo\b',code))
    wd=len(re.findall(r'\bwhile\b[^\n]*\bdo\b',code))
    return o,c,fd,wd,o-c
for f in files:
    o,c,fd,wd,net = bal(f)
    status = "OK" if net==fd+wd else "BAD"
    print(status, f.split('\\')[-1], "opens=",o,"closes=",c,"for_do=",fd,"while_do=",wd,"net=",net)
