# -*- coding: utf-8 -*-
import os, re

root = r'E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\LuaScripts'
csfile = root + r'\Config\CSharpTypes.lua.txt'

# 已定义类型（---@class 名）
defined = set()
raw = open(csfile,'rb').read().decode('utf-8-sig')
for m in re.finditer(r'---@class\s+([^\s]+)', raw):
    defined.add(m.group(1))

# 扫描所有 .lua.txt 的 ---@type 标注
# 类型标注形式：---@type Name  或  ---@type fun(...)  或  ---@type Name[]
type_pat = re.compile(r'---@type\s+([^\s]+)')
usage = {}  # 类型名 -> [文件]
for dirpath, _, files in os.walk(root):
    for f in files:
        if not f.endswith('.lua.txt'):
            continue
        p = os.path.join(dirpath, f)
        try:
            s = open(p,'rb').read().decode('utf-8-sig')
        except Exception:
            continue
        for m in type_pat.finditer(s):
            t = m.group(1).strip()
            # 去掉 fun(...) / table[] 等后缀：只取标识符
            t = t.split('(')[0]
            t = re.sub(r'\[\]$','',t)
            if not t:
                continue
            # 过滤 Lua 内建/基元：不参与 class 定义
            if t in ('table','function','boolean','number','string','nil','any','true','false','void'):
                continue
            usage.setdefault(t, []).append(p)

# 未定义（红线）类型
missing = sorted(k for k in usage if k not in defined)
print("== 已定义数 ==", len(defined))
print("== 标注类型总数 ==", len(usage))
print("== 未定义（可能红线）类型数 ==", len(missing))
for t in missing:
    files = sorted(set(usage[t]))
    print(t, " <-- ", len(files), "处:", files[:3])
