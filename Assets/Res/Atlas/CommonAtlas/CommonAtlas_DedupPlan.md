# CommonAtlas 重复图片去重对照方案

> 生成时间：2026-11-13
> 范围：UIs 预制体（`Assets/Res/Prefabs/UIs`）、文件夹二（`Assets/Res/Textures/PicsForEditor/Common/Common`）、文件夹三（`Assets/Res/Atlas/CommonAtlas/...Sprite`）

## 1. 结论摘要

- 文件夹二 37 张 png 与文件夹三 `...Sprite` 下同名图 **MD5 全部一致（37/37）**，是纯重复副本；本次"替换"不涉及换图，本质是去重搬家。
- 预制体精灵引用格式：`m_Sprite: {fileID: 21300000, guid: <纹理GUID>, type: 3}`，单精灵不带 PathID。
- 引用分类：**预制体 + Lua 都引用 17 张**、**仅预制体 1 张**、**仅 Lua 15 张**、**都未引用 4 张**。
- 文件夹三的 37 张切片 png **当前没有 `.meta`（未被 Unity 导入），没有 GUID**；直接删文件夹二会导致预制体 GUID 引用与 Lua 路径引用同时悬空。

## 2. 引用分类

### 2.1 预制体 + Lua 都引用（17）

| 名称 | 尺寸 | 纹理 GUID | 引用预制体 |
|---|---|---|---|
| btn_b | 280x184 | a18b008bfdd2b6448a87f753d8fb9aa2 | UIMainStateView; UILevelFailedView |
| btn_g | 280x184 | 6e9bb5c6d0bbaed42b9b68d465c99fd5 | UILoginView; UIMainStateView; UILevelFailedView; UILevelVictoryView |
| close | 87x87 | 7cf6393d38b93fd4daef19a834f43639 | UISettingView |
| CollectIcon | 128x128 | c37fa6a744bfc084491d4c9635eb2a0c | UIMainStateView |
| home | 59x55 | 3cc51203f27e04d4c8e38be0bf156252 | UILevelFailedView |
| home_puzzle_frame | 503x520 | 3870ee09e38d2ab4c882f7e6d3a9fe42 | SectionItem |
| icon_fish | 64x64 | c7f79989ec2c6544e9120a28e7f39ddf | FishItem |
| icon_home | 98x98 | 0bc6ec9a3540929439919a3b49688a18 | UILevelVictoryView |
| icon_search | 98x98 | 6c1d433f064bdb745ba2e472a0af3a61 | UIMainPlaying |
| icon_set | 62x64 | 16d7d49cf56a4784e9f426486050b7d3 | UIMainPlaying; UIMainStateView |
| level | 47x47 | 04ea05ce89449d84299e370ded51efa0 | UIMainPlaying |
| num_panel | 64x62 | a163cc7e16354014381d289ad4216d2f | UIMainPlaying |
| PhotographIcon | 128x128 | 75cc743487d24fd49b1138e2f8134359 | UIMainStateView |
| profile_cat_1 | 144x145 | fd488ae7e5e77704493929c77f8fa80c | UIMainStateView |
| profile_frame_1 | 150x151 | a220249ca6479944aa5a070e80c49d93 | UIMainStateView |
| safe_panel | 24x24 | 6e8e7f3f13900ad44a7edcf79abb84e3 | UIMainPlaying |
| sign_r | 44x44 | 1892a4d8d4f394344a7a0b135f138cdf | UIMainStateView |

### 2.2 仅预制体引用（1）

| 名称 | 尺寸 | 纹理 GUID | 引用预制体 |
|---|---|---|---|
| icon_fish_2 | 50x44 | c47e5888218e7da4b91e7c5a415d8114 | FishItem |

### 2.3 仅 Lua 运行时加载（15）

| 名称 | 尺寸 | 纹理 GUID |
|---|---|---|
| btn_p | 280x184 | c3ccd35214a72d14690ce4718d548149 |
| btn_r | 280x184 | 9e3db01fe05d9514daf4e63f1c0f997b |
| checkInIcon | 96x101 | 3f3b10d186d01bc468d3862cd1649dca |
| Icon_Advertising | 68x71 | f64aec9307ea78046b59e2fc1d479881 |
| icon_cat | 92x92 | 54413c8cff5d98f4b8cc657eed135279 |
| IconBg | 36x36 | 5fdda0c03dcd43641b1b1286142f27e0 |
| loa_bar | 62x67 | 03ab8650cde27a4478ac1c29a5d3256d |
| loa_pro | 40x41 | 888e89ff60519944cb58cc972dd24655 |
| puzzle | 544x605 | 39c75bee674888e4b96d9f64d7705511 |
| set_panel | 180x181 | 18a8e4c0c34cb1f4494c42e3fafe0013 |
| settingIcon | 60x64 | 4d11300194b320a4383cc2915eab7202 |
| sign_y | 44x44 | 7fd89e2748d12d640aacaa1bf93647dd |
| Tile | 357x70 | dc6569c0e841b6949b1c4811c291a708 |
| turntableIcon | 96x102 | 7788f1a01c65c1e4a932232e6ff489c2 |
| userIcon | 116x116 | 00a29d08bd4c9474786f9f944702d3f6 |

### 2.4 都未引用（4，可安全删除重复副本）

| 名称 | 尺寸 | 纹理 GUID |
|---|---|---|
| btn_g_1 | 280x184 | d5d5b2347fe2f9e45be56fcf15d48e55 |
| circle | 132x132 | 7b62fc82f4a585245ad7483894ec6baf |
| combo_good | 185x58 | 0b00d50c2c56eab41b2d2dd4e7254a32 |
| icon_roll | 98x98 | 906d3dc4d4fc9ae4092beede6daeed00 |

> 已全量扫描 `LuaScripts` 目录，上述 4 张无任何名字/路径引用；UIs 目录之外的预制体/场景/材质/资产也未引用文件夹二的 GUID。

## 3. 运行时依赖位置

- Lua 资源定义：`Assets/LuaScripts/Config/LoadResDefs/LoadResDefs_1.lua.txt`，共 32 条，`ABPath = Assets/Res/Textures/PicsForEditor/Common/Common/<名>`。
- AB 打包配置：`Assets/LuaScripts/Config/LuaJson/ABConfigs.json`，ID 8「通用界面图片」`Path = Assets/Res/Textures/PicsForEditor/Common/Common`。
- 文件夹二 `.meta` 关键字段：`textureType: 8`（Sprite）、`spriteMode: 1`（Single）、`isReadable: 1`、`spritePixelsToUnits: 100`、`spritePivot: {0.5, 0.5}`；`assetBundleName` 按文件夹二路径编码 bundle 名。
- `CommonAtlasTex.png`（合图）与 `CommonAtlas.txt`（打包描述）当前**未被任何预制体/场景/图集资产引用**，项目内也没有 `.spriteatlas` 资产。

## 4. 迁移到文件夹三必须同步改动的清单

1. **Editor 导入生成 meta**：让 Unity 为 `...Sprite` 下 37 张生成 `.meta`，导入设置与文件夹二保持一致（Sprite Single、PPU 100、Pivot 居中、isReadable、透明）。
2. **GUID 处理（二选一）**：
   - 方案甲（保 GUID）：把文件夹二 `.meta` 复制到文件夹三并保留原 `guid`，预制体 `m_Sprite` 无需改 GUID；
   - 方案乙（换新 GUID）：Editor 导入后取得新 GUID，批量替换预制体 `m_Sprite` 中的旧 guid。
3. **Lua 路径改写**：`LoadResDefs_1.lua.txt` 32 条 `ABPath` 改到文件夹三目录，`AssetName` 不变。
4. **AB 配置改写**：`ABConfigs.json` ID 8 的 `Path` 改为文件夹三目录（或走 ResDef 导出工具重新生成）。
5. **bundle 名重建**：迁移后 `.meta` 的 `assetBundleName` 需按新目录重新生成，避免旧 bundle 名残留。
6. **确认目录可被打包**：`...Sprite` 为特殊目录名，需在 Editor 确认 Unity 正常导入、未被忽略，且不会被图集工具二次覆盖。
7. **删除文件夹二**：备份后删除对应 png + `.meta`。
8. **Editor 复核**：预制体图标不丢失、Lua 运行时加载正常、AB 打包路径与体积正确。

## 5. 可选执行策略

- **策略 A 整体迁移**：按第 4 节清单把 37 张（或选定子集）迁到文件夹三，改 prefab / Lua / ABConfigs / meta 后删文件夹二。风险最高，必须在 Editor 内逐步复核。
- **策略 B 就地覆盖（零引用风险）**：用文件夹三同名字节覆盖文件夹二 png，保留原 meta、路径与 AB 打包，不改引用、不删图。适合先统一资源来源，后续再做目录搬迁。
- **策略 C 仅清理无引用项**：只删除 2.4 节 4 张无任何引用的重复副本，其余保留；风险最低。

## 6. 待确认事项

- 文件夹三 `...Sprite` 是否确定作为运行时资源目录（而非仅 TexturePacker 工程源切片）。
- 迁移范围：仅 18 张预制体引用、全部 37 张、还是仅 4 张无引用。
- 合图 `CommonAtlasTex.png` 后续是否走 SpriteAtlas 接入运行时（本次未处理）。
