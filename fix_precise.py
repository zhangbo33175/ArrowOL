import os
import re

root_dir = r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets"

count = 0

for root, dirs, files in os.walk(root_dir):
    for file in files:
        if not file.endswith('.cs'):
            continue
            
        path = os.path.join(root, file)
        # 尝试不同编码
        try:
            with open(path, 'r', encoding='utf-8') as f:
                content = f.read()
        except:
            try:
                with open(path, 'r', encoding='utf-16') as f:
                    content = f.read()
            except:
                continue
        
        new_content = content
        
        # 1. 修复 UnityEngine.gameObject → UnityEngine.GameObject
        new_content = re.sub(r'UnityEngine\.gameObject', 'UnityEngine.GameObject', new_content)
        
        # 2. 修复 .GameObject → .gameObject (但不是类名，是属性)
        # 注意：Transform.GameObject → Transform.gameObject
        new_content = re.sub(r'\.GameObject', '.gameObject', new_content)
        
        # 3. 修复 HonorPathUtils.LuaScript.Game → LuaScript.Game (枚举值不应该被替换)
        # 这个已经是对的了
        
        # 4. 修复 HonorDefinitions.AssetType.gameObject → AssetType.GameObject
        new_content = re.sub(r'AssetType\.gameObject', 'AssetType.GameObject', new_content)
        
        # 5. 修复 LauncherComponent.HonorSpeed / HonorSpeedCache → GameSpeed / GameSpeedCache
        new_content = new_content.replace('.HonorSpeedCache', '.GameSpeedCache')
        new_content = new_content.replace('.HonorSpeed', '.GameSpeed')
        
        # 6. 修复 Util.HonorViewSize → Util.GameViewSize
        new_content = new_content.replace('Util.HonorViewSize', 'Util.GameViewSize')
        
        # 7. 修复 InjectionType.gameObject → InjectionType.GameObject
        new_content = re.sub(r'InjectionType\.gameObject', 'InjectionType.GameObject', new_content)
        
        if content != new_content:
            with open(path, 'w', encoding='utf-8', newline='') as f:
                f.write(new_content)
            count += 1

print(f"Updated {count} files!")
