import re

p = r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts\Game\GameMain\ManagerAndHelper\Manager\SectionManager\SectionManager.lua.txt'
s = open(p, encoding='utf-8').read()

# 目标枚举字面量 = ESectionState. + "Exploring"
target = "ESectionState.Explor" + "ing"   # = ESectionState.Exploring

# 归一化所有错误变体（ExploRing/ExplorIng/Exploriting/Explor 无括号等）
s = re.sub(r'ESectionState\.Explor\w*', target, s)

# 修复 209 行缺失的右括号：SetState(sectionId, <target>\n    end  -> 补 )
s = s.replace(target + '\n    end', target + ')\n    end')

open(p, 'w', encoding='utf-8').write(s)

# 验证
out = open(p, encoding='utf-8').read()
hits = re.findall(r'ESectionState\.\w+', out)
print("枚举引用:", sorted(set(hits)))
