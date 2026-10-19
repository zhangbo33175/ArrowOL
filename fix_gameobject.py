import os
import re

root_dir = r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets"

count = 0

for root, dirs, files in os.walk(root_dir):
    for file in files:
        if not file.endswith('.cs'):
            continue
            
        path = os.path.join(root, file)
        try:
            with open(path, 'r', encoding='utf-8') as f:
                content = f.read()
        except:
            try:
                with open(path, 'r', encoding='utf-16') as f:
                    content = f.read()
            except:
                continue
        
        # 修复：UnityEngine.gameObject → UnityEngine.GameObject
        new_content = re.sub(r'UnityEngine\.gameObject', 'UnityEngine.GameObject', content)
        
        # 同时修复 List<UnityEngine.gameObject> → List<UnityEngine.GameObject>
        new_content = re.sub(r'UnityEngine\.gameObject>', 'UnityEngine.GameObject>', new_content)
        
        if content != new_content:
            with open(path, 'w', encoding='utf-8', newline='') as f:
                f.write(new_content)
            count += 1

print(f"Updated {count} files!")
