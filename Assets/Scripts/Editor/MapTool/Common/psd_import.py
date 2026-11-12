"""
PSD 地图导入解析脚本（V2：三级资源目录 + 跨PSD去重）
用法: python psd_import.py <psd_file> <chapter_dir> <shared_dir> <json_dir>

参数:
  psd_file    - PSD/PSB 文件路径
  chapter_dir - 当前章节专属资源目录（Chapter{id}）
  shared_dir  - 全局共享资源目录（Shared）
  json_dir    - import_result.json 输出目录（Map根目录）

输出:
  - 每个图层导出为单独 PNG 文件（组不导出，只导出实际图层）
  - 三级去重检查: Shared → Chapter → 新导出
  - import_result.json 包含所有图层信息
"""
import sys
import json
import os
from psd_tools import PSDImage

def main():
    if len(sys.argv) < 5:
        print("Usage: python psd_import.py <psd_file> <chapter_dir> <shared_dir> <json_dir>")
        sys.exit(1)

    psd_path = sys.argv[1]
    chapter_dir = sys.argv[2]
    shared_dir = sys.argv[3]
    json_dir = sys.argv[4]

    if not os.path.exists(psd_path):
        print(f"PSD file not found: {psd_path}")
        sys.exit(1)

    # 创建目录
    os.makedirs(chapter_dir, exist_ok=True)
    os.makedirs(shared_dir, exist_ok=True)
    os.makedirs(json_dir, exist_ok=True)

    # 按类型创建子目录（章节层 + 共享层）
    type_dirs_chapter = {
        "building": os.path.join(chapter_dir, "Building"),
        "character": os.path.join(chapter_dir, "Character"),
        "prop": os.path.join(chapter_dir, "Prop"),
        "cat": os.path.join(chapter_dir, "Cat"),
        "background": os.path.join(chapter_dir, "Background"),
    }
    type_dirs_shared = {
        "building": os.path.join(shared_dir, "Building"),
        "character": os.path.join(shared_dir, "Character"),
        "prop": os.path.join(shared_dir, "Prop"),
        "cat": os.path.join(shared_dir, "Cat"),
    }
    for d in list(type_dirs_chapter.values()) + list(type_dirs_shared.values()):
        os.makedirs(d, exist_ok=True)

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
            # 背景图导出到章节 Background 子目录，按PSD文件名命名
            psd_base_name = os.path.splitext(os.path.basename(psd_path))[0]
            bg_filename = f"bg_{psd_base_name}.png"
            bg_path = os.path.join(type_dirs_chapter["background"], bg_filename)
            bg_image.save(bg_path)
            result["backgroundName"] = bg_layer.name
            result["backgroundPath"] = bg_path.replace("\\", "/")
            print(f"Background exported: {bg_layer.name} -> Background/{bg_filename}")
    else:
        print("Warning: No bg_* layer found, background will be empty (transparent)")

    # 2. 处理所有图层（递归）
    exported_names = set()  # 同PSD内已导出的文件名（去重）

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

    def find_existing_resource(safe_name, layer_type):
        """
        三级去重检查，返回资源路径或 None
        检查顺序: Shared → Chapter
        """
        # 第1级: 检查全局共享层 Shared/
        shared_dir_for_type = type_dirs_shared.get(layer_type)
        if shared_dir_for_type:
            shared_path = os.path.join(shared_dir_for_type, f"{safe_name}.png")
            if os.path.exists(shared_path):
                return ("shared", shared_path.replace("\\", "/"))

        # 第2级: 检查当前章节层 Chapter{id}/
        chapter_dir_for_type = type_dirs_chapter.get(layer_type)
        if chapter_dir_for_type:
            chapter_path = os.path.join(chapter_dir_for_type, f"{safe_name}.png")
            if os.path.exists(chapter_path):
                return ("chapter", chapter_path.replace("\\", "/"))

        return (None, None)

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

        # 第1重: 同PSD内同名去重
        if safe_name in exported_names:
            print(f"  Skip duplicate: {layer_name} (same PSD)")
            # 仍然加入物件列表，路径指向已存在的文件
            _, existing_path = find_existing_resource(safe_name, layer_type)
            result["objects"].append({
                "name": clean_name,
                "layerName": layer_name,
                "type": layer_type,
                "x": layer.left,
                "y": layer.top,
                "width": layer.width,
                "height": layer.height,
                "pngPath": existing_path
            })
            return

        # 第2重: 跨PSD去重（Shared → Chapter 检查）
        source, existing_path = find_existing_resource(safe_name, layer_type)

        if source == "shared":
            print(f"  Skip shared: {layer_name} -> {layer_type}/{safe_name}.png (reuse Shared)")
            result["objects"].append({
                "name": clean_name,
                "layerName": layer_name,
                "type": layer_type,
                "x": layer.left,
                "y": layer.top,
                "width": layer.width,
                "height": layer.height,
                "pngPath": existing_path
            })
            exported_names.add(safe_name)
            return

        if source == "chapter":
            print(f"  Skip chapter: {layer_name} -> {layer_type}/{safe_name}.png (reuse Chapter)")
            result["objects"].append({
                "name": clean_name,
                "layerName": layer_name,
                "type": layer_type,
                "x": layer.left,
                "y": layer.top,
                "width": layer.width,
                "height": layer.height,
                "pngPath": existing_path
            })
            exported_names.add(safe_name)
            return

        # 第3重: 都不存在 → 导出到章节目录
        target_dir = type_dirs_chapter.get(layer_type, chapter_dir)
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
    json_path = os.path.join(json_dir, "import_result.json")
    with open(json_path, "w", encoding="utf-8") as f:
        json.dump(result, f, ensure_ascii=False, indent=2)

    print(f"\nDone!")
    print(f"Chapter dir: {chapter_dir}")
    print(f"Shared dir: {shared_dir}")
    print(f"Background: {result['backgroundName']}")
    print(f"Objects: {len(result['objects'])}")
    print(f"JSON: {json_path}")

if __name__ == "__main__":
    main()
