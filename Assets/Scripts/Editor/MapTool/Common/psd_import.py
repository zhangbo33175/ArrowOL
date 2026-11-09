"""
PSD 地图导入解析脚本
用法: python psd_import.py <psd_file_path> <output_dir>
输出:
  - 每个图层导出为单独 PNG 文件（组不导出，只导出实际图层）
  - import_result.json 包含所有图层信息
"""
import sys
import json
import os
from psd_tools import PSDImage

def main():
    if len(sys.argv) < 3:
        print("Usage: python psd_import.py <psd_file> <output_dir>")
        sys.exit(1)

    psd_path = sys.argv[1]
    output_dir = sys.argv[2]

    if not os.path.exists(psd_path):
        print(f"PSD file not found: {psd_path}")
        sys.exit(1)

    os.makedirs(output_dir, exist_ok=True)

    psd = PSDImage.open(psd_path)
    print(f"PSD size: {psd.width} x {psd.height}")

    result = {
        "mapWidth": psd.width,
        "mapHeight": psd.height,
        "backgroundName": "",
        "backgroundPath": "",
        "objects": []
    }

    # 1. 找纯背景图层（bg_开头，或者叫 bg_color / background 的图层）
    #    不用整张 PSD 合成图，避免和拆分的物件重叠
    bg_layer = None

    def find_bg_layer(layer):
        nonlocal bg_layer
        if bg_layer is not None:
            return
        if not layer.is_group():
            name = layer.name.lower()
            if name.startswith("bg_") or name == "bg_color" or name == "background":
                bg_layer = layer
                return
        for child in layer:
            find_bg_layer(child)

    for layer in psd:
        find_bg_layer(layer)

    if bg_layer is not None:
        bg_image = bg_layer.composite()
        if bg_image:
            bg_path = os.path.join(output_dir, "_background.png")
            bg_image.save(bg_path)
            result["backgroundName"] = bg_layer.name
            result["backgroundPath"] = bg_path.replace("\\", "/")
            print(f"Background exported from layer: {bg_layer.name}")
    else:
        print("Warning: No bg_* layer found, background will be empty (transparent)")

    # 2. 处理所有图层（递归）
    name_counter = {}  # 同名文件计数，避免覆盖
    exported_names = set()  # 已导出的文件名（去重）

    # 按类型创建子目录
    type_dirs = {
        "building": os.path.join(output_dir, "Building"),
        "character": os.path.join(output_dir, "Character"),
        "prop": os.path.join(output_dir, "Prop"),
        "cat": os.path.join(output_dir, "Cat"),
        "background": os.path.join(output_dir, "Background"),
    }
    for d in type_dirs.values():
        os.makedirs(d, exist_ok=True)

    def get_unique_name(base_name):
        if base_name not in name_counter:
            name_counter[base_name] = 0
            return base_name
        name_counter[base_name] += 1
        return f"{base_name}_{name_counter[base_name]}"

    def get_type_by_name(name, parent_group_name=""):
        """根据图层名和父组名自动判断类型"""
        name_lower = name.lower()
        group_lower = parent_group_name.lower()

        # 1. 图层名前缀优先
        if name_lower.startswith("bg_"):
            return "background"
        if name_lower.startswith("building_"):
            return "building"
        if name_lower.startswith("char_"):
            return "character"
        if name_lower.startswith("prop_"):
            return "prop"
        if name_lower.startswith("cat_"):
            return "cat"

        # 2. 父组名关键词判断
        if "building" in group_lower or "建筑" in group_lower:
            return "building"
        if "char" in group_lower or "人物" in group_lower or "npc" in group_lower:
            return "character"
        if "cat" in group_lower or "猫" in group_lower:
            return "cat"
        if "prop" in group_lower or "物件" in group_lower:
            return "prop"

        # 3. 图层名关键词判断
        if "building" in name_lower or "建筑" in name_lower or "house" in name_lower:
            return "building"
        if "char" in name_lower or "npc" in name_lower:
            return "character"
        if "cat" in name_lower or "猫" in name_lower:
            return "cat"

        # 4. 默认当作物件
        return "prop"

    def process_layer(layer, parent_group_name=""):
        if not layer.visible:
            return

        # 组不导出，只递归处理子图层
        if layer.is_group():
            for child in layer:
                process_layer(child, layer.name)
            return

        layer_name = layer.name
        safe_name = layer_name.replace(" ", "_").replace("/", "_").replace("拷贝", "copy").replace("组", "group")

        # 判断类型（图层名 + 父组名）
        layer_type = get_type_by_name(layer_name, parent_group_name)
        clean_name = layer_name

        # 跳过纯背景图层（背景已经用 bg_xxx 图层了）
        if layer_type == "background":
            print(f"  Skip background layer: {layer_name}")
            return

        # 同名去重：只导出第一张
        if safe_name in exported_names:
            print(f"  Skip duplicate: {layer_name} (already exported)")
            # 仍然加入物件列表，但路径指向已导出的文件
            existing_path = os.path.join(type_dirs.get(layer_type, output_dir), f"{safe_name}.png")
            result["objects"].append({
                "name": clean_name,
                "layerName": layer_name,
                "type": layer_type,
                "x": layer.left,
                "y": layer.top,
                "width": layer.width,
                "height": layer.height,
                "pngPath": existing_path.replace("\\", "/")
            })
            return

        # 导出 PNG（按类型放到对应子目录）
        target_dir = type_dirs.get(layer_type, output_dir)
        png_path = os.path.join(target_dir, f"{safe_name}.png")

        try:
            image = layer.composite()
            if image is None:
                print(f"  Skip {layer_name}: composite() returned None")
                return
            image.save(png_path)
            exported_names.add(safe_name)
            print(f"  Exported: {layer_name} -> {layer_type}/{safe_name}.png ({layer.size})")
        except Exception as e:
            print(f"  Skip {layer_name}: {type(e).__name__}: {e}")
            return

        result["objects"].append({
            "name": clean_name,
            "layerName": layer_name,
            "type": layer_type,
            "x": layer.left,
            "y": layer.top,
            "width": layer.width,
            "height": layer.height,
            "pngPath": png_path.replace("\\", "/")
        })

    for layer in psd:
        process_layer(layer)

    # 输出 JSON
    json_path = os.path.join(output_dir, "import_result.json")
    with open(json_path, "w", encoding="utf-8") as f:
        json.dump(result, f, ensure_ascii=False, indent=2)

    print(f"\nDone! Exported to: {output_dir}")
    print(f"Background: {result['backgroundName']}")
    print(f"Objects: {len(result['objects'])}")
    print(f"JSON: {json_path}")

if __name__ == "__main__":
    main()
