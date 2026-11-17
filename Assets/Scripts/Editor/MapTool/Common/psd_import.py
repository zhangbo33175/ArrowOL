"""
PSD 地图导入解析脚本（V3：三级资源目录 + 八层归类 + 像素尺寸/拉伸落盘）

用法:
    python psd_import.py <psd_file> <chapter_dir> <shared_dir> <json_dir> [level_id]

参数:
    psd_file    - PSD/PSB 文件路径
    chapter_dir - 当前章节专属资源目录（Chapter{id}）
    shared_dir  - 全局共享资源目录（Shared，跨章节复用，便于打图集）
    json_dir    - import_result.json 输出目录（Map 根目录）
    level_id    - 当前关卡ID（可选，缺省 1）

目录规划（与 C# MapLayerCatalog 对齐）:
    全局共享  <shared_dir>/<Layer>/                 跨章节复用
    章节共享  <chapter_dir>/Shared/<Layer>/          同章节跨关卡复用（章节公共资源）
    关卡专属  <chapter_dir>/Level{lv}/<Layer>/    仅当前关卡使用

分层（对齐截图 L0–L6 功能分桶 + 气泡置顶，与 C# MapLayerCatalog 一致）:
    Background / Greenery / Road / Buildings / Props / Characters / Foreground / Bubbles

输出:
    - 每个可见图层导出为单独 PNG（组不导出，只导出实际图层）
    - 三级去重: 全局 Shared → 章节 Shared → 既有关卡目录 → 当前关卡目录
    - import_result.json 携带 PSD 像素坐标/宽高/拉伸/排序，供编辑器与运行时 1:1 还原
"""
import sys
import os
import json
import shutil
import hashlib

try:
    from psd_tools import PSDImage
except ImportError:
    PSDImage = None


# ---------------------------------------------------------------------------
# 分层归类：类型键 ↔ 目录名（必须与 C# MapLayerCatalog 保持一致）
# ---------------------------------------------------------------------------
LAYER_DIRS = {
    "background": "Background",
    "greenery": "Greenery",
    "road": "Road",
    "building": "Buildings",
    "prop": "Props",
    "character": "Characters",
    "foreground": "Foreground",
    "bubble": "Bubbles",
}

SORT_ORDER = {
    "background": 0,
    "greenery": 100,
    "road": 200,
    "building": 300,
    "prop": 400,
    "character": 500,
    "foreground": 600,
    "bubble": 700,
}


def md5_of_file(path):
    """计算文件 MD5（用于跨关卡/跨章节资源复用判定）。"""
    if not os.path.exists(path):
        return None
    try:
        h = hashlib.md5()
        with open(path, "rb") as f:
            for chunk in iter(lambda: f.read(65536), b""):
                h.update(chunk)
        return h.hexdigest()
    except Exception:
        return None


def classify_layer(name, parent_group=""):
    """
    按图层名前缀 → 父组关键词 → 图层名关键词 的优先级判定分层类型键。

    :param name: 图层名
    :param parent_group: 父组名（PSD 分组）
    :return: 类型键（background/greenery/road/building/prop/character/foreground/bubble）
    """
    n = (name or "").lower()
    g = (parent_group or "").lower()

    # 1. 图层名前缀优先
    if n.startswith("bg_") or n in ("bg_color", "background"):
        return "background"
    if n.startswith("road_"):
        return "road"
    if n.startswith("grass_") or n.startswith("plant_"):
        return "greenery"
    if n.startswith("building_"):
        return "building"
    if n.startswith("char_") or n.startswith("cat_"):
        return "character"
    if n.startswith("eff_") or n.startswith("effect_") or n.startswith("fg_"):
        return "foreground"
    if n.startswith("bubble_"):
        return "bubble"
    if n.startswith("prop_"):
        return "prop"

    # 2. 父组名关键词判断
    if "road" in g or "道路" in g or "路面" in g or "车道" in g \
            or "斑马线" in g or "路灯" in g or "路牌" in g or "市政" in g:
        return "road"
    if "ground" in g or "草地" in g or "绿植" in g or "植物" in g or "花草" in g \
            or "灌木" in g or "绿化" in g or "树" in g or "花" in g:
        return "greenery"
    if "building" in g or "建筑" in g:
        return "building"
    if "char" in g or "人物" in g or "npc" in g or "cat" in g or "猫" in g:
        return "character"
    if "eff" in g or "特效" in g or "光效" in g or "fx" in g or "前景" in g:
        return "foreground"
    if "bubble" in g or "气泡" in g or "对话" in g:
        return "bubble"
    if "prop" in g or "物件" in g or "装饰" in g:
        return "prop"

    # 3. 图层名关键词判断
    if "road" in n or "道路" in n or "路面" in n or "车道" in n:
        return "road"
    if "grass" in n or "草" in n or "plant" in n or "绿植" in n or "花草" in n \
            or "花" in n or "灌木" in n or "树" in n:
        return "greenery"
    if "building" in n or "建筑" in n or "house" in n:
        return "building"
    if "char" in n or "npc" in n or "cat" in n or "猫" in n:
        return "character"
    if "eff" in n or "特效" in n or "光效" in n or "fg" in n:
        return "foreground"
    if "bubble" in n or "气泡" in n:
        return "bubble"

    # 4. 默认归道具层
    return "prop"


def safe_file_name(name):
    """图层名清洗为合法文件名。"""
    return (name or "layer").replace(" ", "_").replace("/", "_") \
        .replace("\\", "_").replace("拷贝", "copy").replace("组", "group")


def collect_existing(chapter_dir, shared_dir):
    """
    扫描既有资源：返回 {类型键: {文件名(去扩展名): (绝对路径, 来源)}}，
    覆盖全局 Shared / 章节 Shared / 既有关卡目录；来源标识 global/chapter/level。

    :param chapter_dir: 章节目录
    :param shared_dir: 全局共享目录
    :return: dict[type_key] -> {file_stem: (abs_path, source)}
    """
    existing = {k: {} for k in LAYER_DIRS}
    shared_norm = shared_dir.replace("\\", "/")
    chapter_shared_norm = os.path.join(chapter_dir, "Shared").replace("\\", "/")

    for root in (shared_dir, chapter_dir):
        if not os.path.isdir(root):
            continue
        for dirpath, _dirs, files in os.walk(root):
            dir_name = os.path.basename(dirpath)
            type_key = next((k for k, v in LAYER_DIRS.items() if v == dir_name), None)
            if type_key is None:
                continue
            norm_dir = dirpath.replace("\\", "/")
            if norm_dir.startswith(shared_norm):
                source = "global"
            elif norm_dir.startswith(chapter_shared_norm):
                source = "chapter"
            else:
                source = "level"
            for f in files:
                if not f.lower().endswith(".png"):
                    continue
                stem = os.path.splitext(f)[0]
                existing[type_key].setdefault(stem, (os.path.join(dirpath, f), source))
    return existing


def main():
    if len(sys.argv) < 5:
        print("Usage: python psd_import.py <psd_file> <chapter_dir> <shared_dir> <json_dir> [level_id]")
        sys.exit(1)

    psd_path = sys.argv[1]
    chapter_dir = sys.argv[2]
    shared_dir = sys.argv[3]
    json_dir = sys.argv[4]
    level_id = sys.argv[5] if len(sys.argv) >= 6 and sys.argv[5] else "1"

    if not os.path.exists(psd_path):
        print(f"PSD file not found: {psd_path}")
        sys.exit(1)

    if PSDImage is None:
        print("ERROR: psd_tools 未安装，请先执行: pip install psd-tools")
        sys.exit(2)

    # 三级目录创建
    def tier_dirs(base_dir, layer_type):
        return os.path.join(base_dir, LAYER_DIRS[layer_type])

    chapter_shared_root = os.path.join(chapter_dir, "Shared")
    chapter_level_root = os.path.join(chapter_dir, f"Level{level_id}")
    for root in (shared_dir, chapter_shared_root, chapter_level_root):
        for layer in LAYER_DIRS.values():
            os.makedirs(os.path.join(root, layer), exist_ok=True)
    os.makedirs(json_dir, exist_ok=True)

    # 既有资源索引（全局 Shared / 章节 Shared / 既有关卡）
    existing = collect_existing(chapter_dir, shared_dir)

    psd = PSDImage.open(psd_path)
    print(f"PSD size: {psd.width} x {psd.height}")

    result = {
        "mapWidth": psd.width,
        "mapHeight": psd.height,
        "backgroundName": "",
        "backgroundPath": "",
        "objects": [],
    }

    # ---- 1. 查找纯背景图层 ----
    bg_layer = {"layer": None}

    def find_bg_layer(layer):
        if bg_layer["layer"] is not None:
            return
        if not layer.is_group():
            name = layer.name.lower()
            if name.startswith("bg_") or name == "bg_color" or name == "background":
                bg_layer["layer"] = layer
                return
        for child in layer:
            find_bg_layer(child)

    for layer in psd:
        find_bg_layer(layer)

    if bg_layer["layer"] is not None:
        bg_image = bg_layer["layer"].composite()
        if bg_image:
            psd_base_name = os.path.splitext(os.path.basename(psd_path))[0]
            bg_filename = f"bg_{psd_base_name}.png"
            bg_dir = tier_dirs(chapter_level_root, "background")
            bg_path = os.path.join(bg_dir, bg_filename)
            bg_image.save(bg_path)
            result["backgroundName"] = bg_layer["layer"].name
            result["backgroundPath"] = bg_path.replace("\\", "/")
            print(f"Background exported: {bg_layer['layer'].name} -> Level{level_id}/Background/{bg_filename}")
    else:
        print("Warning: No bg_* layer found, background will be empty (transparent)")

    # ---- 2. 递归处理所有可见图层 ----
    exported_names = set()

    def find_existing_resource(safe_name, layer_type):
        """
        三级去重查找：全局 Shared → 章节 Shared → 既有关卡目录。
        :return: (来源标识, 绝对路径或 None)；来源 global/chapter/level
        """
        hit = existing.get(layer_type, {}).get(safe_name)
        if hit:
            hit_path, source = hit
            return source, hit_path
        return None, None

    def promote_to_chapter_shared(source, src_path, layer_type, safe_name):
        """
        章节公共资源提升：仅命中既有关卡目录（level）时，把图片复制到章节 Shared/<Layer>/，
        供同章节后续关卡复用，便于后续打图集；已在 global/chapter 的直接复用原路径。
        :return: 最终引用路径
        """
        if source != "level":
            return src_path
        shared_target_dir = tier_dirs(chapter_shared_root, layer_type)
        dst_path = os.path.join(shared_target_dir, f"{safe_name}.png")
        try:
            if not os.path.exists(dst_path):
                shutil.copyfile(src_path, dst_path)
            existing.setdefault(layer_type, {})[safe_name] = (dst_path, "chapter")
            print(f"  Promote to chapter shared: {safe_name}.png -> Shared/{LAYER_DIRS[layer_type]}/")
            return dst_path
        except Exception as e:
            print(f"  Promote failed: {safe_name}: {type(e).__name__}: {e}")
            return src_path

    def process_layer(layer, parent_group=""):
        if not layer.visible:
            return

        if layer.is_group():
            for child in layer:
                process_layer(child, layer.name)
            return

        layer_name = layer.name
        safe_name = safe_file_name(layer_name)
        layer_type = classify_layer(layer_name, parent_group)

        # 背景图层已单独导出，跳过
        if layer_type == "background":
            print(f"  Skip background layer: {layer_name}")
            return

        # 第 1 重：同 PSD 内同名去重
        if safe_name in exported_names:
            _, existing_path = find_existing_resource(safe_name, layer_type)
            append_object(layer, layer_name, safe_name, layer_type, existing_path)
            print(f"  Skip duplicate: {layer_name} (same PSD)")
            return

        # 第 2 重：跨 PSD / 跨关卡去重；跨关卡复用的图片提升到章节公共目录
        source, existing_path = find_existing_resource(safe_name, layer_type)
        if source:
            exported_names.add(safe_name)
            reused_path = promote_to_chapter_shared(source, existing_path, layer_type, safe_name)
            append_object(layer, layer_name, safe_name, layer_type, reused_path)
            print(f"  Skip {source}: {layer_name} -> {LAYER_DIRS[layer_type]}/{safe_name}.png (reuse)")
            return

        # 第 3 重：新资源 → 关卡专属目录
        target_dir = tier_dirs(chapter_level_root, layer_type)
        png_path = os.path.join(target_dir, f"{safe_name}.png")
        try:
            image = layer.composite()
            if image is None:
                print(f"  Skip {layer_name}: composite() returned None")
                return
            image.save(png_path)
            exported_names.add(safe_name)
            existing.setdefault(layer_type, {})[safe_name] = (png_path, "level")
            print(f"  Exported: {layer_name} -> Level{level_id}/{LAYER_DIRS[layer_type]}/{safe_name}.png ({layer.size})")
        except Exception as e:
            print(f"  Skip {layer_name}: {type(e).__name__}: {e}")
            return

        append_object(layer, layer_name, safe_name, layer_type, png_path)

    def append_object(layer, clean_name, safe_name, layer_type, png_path):
        """组装单条物件记录（PSD 像素坐标/宽高/拉伸/排序落盘）。"""
        result["objects"].append({
            "name": clean_name,
            "layerName": layer.name,
            "type": layer_type,
            "layerType": layer_type,
            "sortOrder": SORT_ORDER[layer_type],
            "x": layer.left,
            "y": layer.top,
            "width": layer.width,
            "height": layer.height,
            "pixelWidth": layer.width,
            "pixelHeight": layer.height,
            "stretch": False,
            "pngPath": (png_path or "").replace("\\", "/"),
        })

    for layer in psd:
        process_layer(layer)

    # ---- 3. 输出 JSON ----
    json_path = os.path.join(json_dir, "import_result.json")
    with open(json_path, "w", encoding="utf-8") as f:
        json.dump(result, f, ensure_ascii=False, indent=2)

    print(f"\nDone!")
    print(f"Chapter dir: {chapter_dir}")
    print(f"Shared dir: {shared_dir}")
    print(f"Level: Level{level_id}")
    print(f"Background: {result['backgroundName']}")
    print(f"Objects: {len(result['objects'])}")
    print(f"JSON: {json_path}")


if __name__ == "__main__":
    main()
