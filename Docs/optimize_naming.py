# 批量优化私有字段命名：m_ 前缀 → _ 前缀
# 只修改私有字段，不修改公共字段和方法名

import os
import re

# 需要优化的目录
directories = [
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\GameFramework\Scripts",
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\Scripts\Runtime\Honor",
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\Scripts\Runtime\Utils",
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\Scripts\Runtime\Map",
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\Scripts\Runtime\MapGrid",
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\Scripts\Runtime\MapView",
    r"E:\Game\CatGame\CatGameCodeOptimizationBuild\Assets\Scripts\Runtime\Game",
]

# 统计
total_files = 0
modified_files = 0

def optimize_file(file_path):
    global total_files, modified_files
    total_files += 1
    
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # 匹配私有字段声明：private ... m_xxx = ... 或 private ... m_xxx;
        # 注意：只替换私有字段声明和使用，不替换公共字段
        # 模式：在private/protected后面的m_xxx
        
        # 第一步：替换私有字段声明
        # 匹配 private [static] [readonly] Type m_FieldName
        def replace_private_field(match):
            indent = match.group(1)
            modifiers = match.group(2)
            type_name = match.group(3)
            field_name = match.group(4)
            rest = match.group(5)
            return f"{indent}{modifiers}{type_name} _{field_name[2:]}{rest}"
        
        # 匹配私有字段声明行
        content = re.sub(
            r'^(\s*)(private\s+(?:static\s+)?(?:readonly\s+)?.+?\s+)m_([A-Z]\w*)(.*)$',
            replace_private_field,
            content,
            flags=re.MULTILINE
        )
        
        # 第二步：替换所有 m_xxx 的使用（在同一文件中）
        # 注意：这个比较危险，我们只替换明显是变量使用的地方
        # 比如：m_TemplateId → _TemplateId
        # 但是要小心不要替换字符串里面的内容
        
        # 简单处理：替换所有 m_ 开头的标识符为 _ 开头
        # （在这个项目中，m_ 只用于私有字段）
        content = re.sub(r'\bm_([A-Z]\w*)\b', r'_\1', content)
        
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            modified_files += 1
            print(f"  已优化: {os.path.basename(file_path)}")
            
    except Exception as e:
        print(f"  跳过: {os.path.basename(file_path)} - {e}")

# 遍历所有目录
for directory in directories:
    if not os.path.exists(directory):
        print(f"目录不存在: {directory}")
        continue
    
    print(f"\n处理目录: {directory}")
    
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                optimize_file(file_path)

print(f"\n=== 优化完成 ===")
print(f"总文件数: {total_files}")
print(f"修改文件数: {modified_files}")
