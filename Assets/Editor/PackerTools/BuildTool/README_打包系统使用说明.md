# Honor 打包系统使用说明（Assets/Editor/PackerTools/BuildTool）

> (c) copyright 2026 - 2030, Honor.Editor
> 适用工程：CatGame（Unity 2022.3.8f1）
> 本文档与打包代码同目录存放，随工具一起交付。

---

## 1. 系统能力总览

本系统在既有 `BuildPackage`（单平台 AB 打包，随编辑器活动目标）基础之上，提供一条完整的、可指定平台、可编排步骤的发布流水线：

| 能力 | 说明 |
|---|---|
| 多平台 AB 打包 | Android / iOS / WebGL / PC(StandaloneWindows64)，可选目标平台，输出到 `StreamingAssets/AssetBundles/{平台}` |
| res 资源打包 | 按既有配置分组打包 `Assets/Res`，生成 `respathtab.txt` |
| Lua 资源打包 | 收集 `Assets/LuaScripts` 下 `*.lua.txt / *.json / *.bytes`，按运行时约定命名打入 AB（可本地/AB 双模式兼容），加密可选（预留） |
| 合并单次打包 | `PackageAll`：res + lua 一次 `BuildAssetBundles`，产出**单一 manifest**（运行时 LuaComponent 才能在同一份 manifest 中扫到 lua bundle） |
| APK 打包 | Android，LZ4 / Development / keystore 签名 / versionCode |
| PC 打包 | StandaloneWindows64 exe |
| iOS 打包 | Xcode 工程（需 macOS 环境执行） |
| 流水线编排 | BuildOrder 订单树 + OrderType 注册表，单步异常即停 |
| 打包窗口 | Tab 窗口：平台选择 + 步骤勾选 + 进度 + 日志 |
| CI 入口 | `-executeMethod` 命令行，退出码 0/1 |

架构借鉴自参照工程（E:\资料\资料\新建文件夹\新建文件夹\Editor\BuildTool\）的 BuildOrder 流水线，但全部解耦重写：不依赖其 GameCfg/BuildPath/XLuaManager 实现。

---

## 2. 目录结构与新增文件

```
Assets/Editor/PackerTools/BuildTool/
├── BuildSystem/                     # 流水线核心（新增）
│   ├── BuildOrder.cs                # OrderType 枚举 + BuildOrder 基类（EditorPrefs 勾选持久化）
│   ├── BuildContext.cs              # 按类型存取的上下文容器
│   ├── IContextObject.cs            # 上下文对象标记接口
│   ├── BuildPlatformContext.cs      # 平台上下文（目标平台/输出目录/版本/签名/场景）
│   ├── BuildSettingsStore.cs        # 全局打包设置持久化（EditorPrefs, honor_build_ 前缀）
│   ├── BuildLog.cs                  # 统一日志（事件解耦转发给窗口）
│   └── BuildOrderHandler.cs         # 单例处理器：流水线定义 + 订单注册 + 执行 + 进度
├── BuildOrders/                     # 订单实现（新增）
│   ├── BuildOrderStart.cs           # 起始订单（恒启用，初始化上下文）
│   ├── BuildOrderPackageRes.cs      # 打 res AB
│   ├── BuildOrderPackageLua.cs      # 打 Lua AB
│   ├── BuildOrderPackageAll.cs      # res+lua 合并打 AB（单一 manifest）
│   ├── BuildOrderPackageAPK.cs      # 打 APK
│   ├── BuildOrderPackagePC.cs       # 打 PC exe
│   └── BuildOrderPackageIOS.cs      # 打 iOS 工程
├── BuildPackage/                    # 既有工具（沿用 + 扩展）
│   ├── BuildPackage_Define.cs       # [扩展] 新增 GetPlatformName(BuildTarget) 等指定平台接口
│   ├── BuildPackage_Builder.cs      # [扩展] 指定平台重载/合并打包/清单改名/respathtab 按平台
│   ├── BuildPackage_Lua.cs          # [新增] Lua 收集分组 + XOR 加密工具（预留）
│   ├── BuildPackage_Config.cs       # 既有（未改）
│   ├── BuildPackage_AssetMap.cs     # 既有（未改）
│   ├── BuildPackage_Incremental.cs  # 既有（未改，复用增量签名）
│   ├── BuildPackage_EditorWindow.cs # 既有（未改，原 AB 配置窗口仍可用）
│   └── BuildPackage_Menu.cs         # 既有（未改）
├── BuildPlayer/                     # 玩家包打包（新增）
│   ├── BuildPlayer_Common.cs        # 输出路径/PlayerSettings/keystore/场景/结果日志公共层
│   ├── BuildPlayer_Android.cs       # APK（LZ4/Development/符号/签名）
│   ├── BuildPlayer_PC.cs            # PC exe
│   └── BuildPlayer_iOS.cs           # iOS Xcode 工程
├── PackWindow/
│   └── PackWindow.cs                # 打包 Tab 窗口（新增）
├── PackMenu.cs                      # Honor/打包工具 菜单（新增）
└── BuildCommandLine.cs              # CI 命令行入口（新增）
```

> 说明：除 `BuildPackage_Define.cs`、`BuildPackage_Builder.cs` 为"只增不改"式扩展外，本系统未修改任何既有文件；旧 AB 打包窗口/菜单完全保留。

---

## 3. 与运行时约定的对齐（关键，勿改动）

| 约定 | 值 | 运行时依据 |
|---|---|---|
| bundle 名 | 相对路径 `'/'→'@'` 全小写 + `.bundle`（res 为相对 `Assets/Res`） | `AssetBundleLoadManager.GetABFormatPath` |
| 平台目录名 | Android / iOS / WebGL；**PC(StandaloneWindows64) 沿用 "Android"**（运行时宏无 Standalone 分支） | `GamePathUtils.PlatformName` |
| AB 输出目录 | `StreamingAssets/AssetBundles/{平台名}` | `GamePathUtils.AB.Streaming.GetRootDirectoryFullPath` |
| 清单包名 | `{平台名}.bundle`。注意：BuildPipeline 生成的清单文件名为**输出目录末级文件夹名（无扩展名）**，打包器已自动拷贝改名，**切勿删除该改名步骤** | `AssetBundleLoadManager.LoadManifest` |
| respathtab.txt | 每行 `相对路径(不含后缀)=bundle名=文件名`；res 条目 key 相对 `Assets/Res`，lua 条目 key 形如 `LuaScripts/Game/GameMain.lua`（去 `Assets/` 前缀、去 `.txt`） | 资源映射反查 |
| Lua bundle 名 | 基于 **`Assets/LuaScripts` 全路径**（只去 `.txt`，保留 `.lua`），如 `Assets/LuaScripts/Game/GameMain.lua.txt → assets@luascripts@game@gamemain.lua.bundle` | `LuaComponent.InitAssetBundleLuaPaths`（按 `GetABFormatPath("Assets/LuaScripts")` 前缀匹配 manifest，`GetABRestoredPath` 还原地址） |
| 主场景 | 默认 `Assets/Scenes/GameApp.unity`（工程 EditorBuildSettings 场景列表为空，打包器显式传入） | 启动入口 |

---

## 4. 打包窗口使用（菜单：Honor/打包工具/打开打包窗口）

窗口分两个 Tab：

### Tab0「AB资源打包」
- **目标平台**：下拉选择 Android / iOS / WebGL / Windows64（写入并持久化 `BuildSettingsStore.CurrentPlatform`）；下方只读展示对应 AB 输出目录。
- **增量打包**：开启后 bundle 内容无变化则跳过打包（复用既有 `BuildPackage_Incremental` 签名缓存）。
- **Lua加密**：**预留开关，默认关闭**。开启仅记日志，不改产物；启用前必须与运行时 LuacMode/解密逻辑对齐（见第 7 节）。
- **动作按钮**：打AB（选定平台全量 res+lua）、单独打Lua、单独打Res、打全平台AB（Android→iOS→PC 依次）、清空输出、打开输出目录。
- **步骤勾选**：6 个步骤勾选与流水线订单**共享同一 EditorPrefs 键**（`honor_build_order_` + 订单 Name），勾选状态跨窗口/重启保留；点「按勾选步骤执行」只执行勾选步骤（自动补 Start 在前）。

### Tab1「玩家包打包」
- 字段：版本号 / BundleCode / 开发包 / 应用名 / 包名 / 输出根目录（可浏览）/ 场景列表（分号分隔）/ Keystore 四字段（密码用掩码输入，不落日志）。
- 动作：打APK / 打PC / 打iOS / 一键全平台（Android: AB+APK → iOS: AB+iOS → PC: AB+PC）。

底部为进度条 + 最近日志 + 日志滚动区（上限 500 条，自动滚底）。

---

## 5. 菜单入口

`Honor/打包工具/`：打开打包窗口、打AB(选定平台)、打Lua、打Res、打APK、打PC、打iOS、打全平台AB、一键全平台。动作前均弹确认框。
（既有 `Honor/AB打包工具/...` 菜单与旧 AB 窗口保留，互不影响。）

---

## 6. CI 命令行（-batchmode -executeMethod）

入口类：`Honor.Editor.BuildTool.BuildCommandLine`，方法：`BuildAB / BuildRes / BuildLua / BuildAPK / BuildPC / BuildIOS / BuildAll`。

示例（Windows）：

```bat
Unity.exe -batchmode -projectPath "E:/Game/CatGame/CatGameCodeOptimizationBuild" ^
  -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildAPK ^
  -buildPlatform Android -version 1.0.0 -bundleCode 1 -development false ^
  -outPath "E:/Builds" -appName CatGame -packageName com.honor.catgame ^
  -scenes "Assets/Scenes/GameApp.unity" ^
  -keystorePath "Assets/Editor/BuildTool/qixia_sg.jks" -keyaliasName qixia_sg ^
  -logFile -
```

| 参数 | 说明 | 默认 |
|---|---|---|
| `-buildPlatform` | Android / iOS / PC / WebGL（PC=StandaloneWindows64） | Android |
| `-version` | bundleVersion | 沿用当前设置 |
| `-bundleCode` | Android versionCode | 沿用当前设置 |
| `-development` | true/false | false |
| `-outPath` | 玩家包输出根目录 | 工程根/Builds |
| `-appName` | 产物名前缀 | productName |
| `-packageName` | applicationIdentifier | 沿用当前设置 |
| `-scenes` | 场景列表（分号分隔） | Assets/Scenes/GameApp.unity |
| `-keystorePath / -keystorePass / -keyaliasName / -keyaliasPass` | Android 签名 | 默认路径/别名见窗口，密码默认空 |
| `-incremental` | true/false | false |
| `-luaEncrypt` | true/false | false |
| `-help` | 打印用法并退出 | — |

退出码：**0 = 成功，1 = 失败**（订单内 throw 或异常均置 1，可直接接入 Jenkins/GitLab CI）。
打 AB / 打 Lua / 打 Res 受 `-buildPlatform` 控制；`BuildAPK/PC/IOS` 忽略 `-buildPlatform`（各自锁定目标平台）。

---

## 7. 已知边界与注意事项

1. **PC 与 Android 的 AB 输出目录同名**（均 `AssetBundles/Android`）：运行时宏使然。两者会互相覆盖，勿同时保留；`一键全平台`按 Android→iOS→PC 顺序执行（后打者覆盖同名目录）。
2. **iOS 打包需 macOS/Xcode**：Windows 上调用会报错（代码已捕获并提示），AB 的 iOS 平台构建同样需要已安装 iOS Build Support 模块。
3. **Lua 加密为预留**：`BuildPackage_Lua.LuaEncrypt`（XOR，key `{0x6f,0x72,0x6e,0x6f,0x72}`）提供工具方法，但**默认不接入流水线**；启用需运行时 LuacMode/对应解密配合，生产使用前务必与运行时约定 key 与输出格式。
4. **增量语义**：增量判定按"合并后的完整映射"（res+lua）计算签名；lua-only 打包只按 luaMap 判定。签名缓存按 bundle 名存储，跨平台共享。
5. **打包场景**：工程 EditorBuildSettings 场景列表为空，玩家包打包显式传入 Store 中保存的场景（默认 GameApp.unity）；场景缺失仅警告不阻塞。
6. **Keystore**：工程当前无 .jks 文件，打包器默认路径 `Assets/Editor/BuildTool/qixia_sg.jks`（参照工程约定），文件缺失仅警告并跳过签名设置（此时使用 PlayerSettings 当前签名配置）；生产请放置签名文件并在窗口/CI 填写密码，**代码不硬编码口令**。
7. **分组配置**：流水线 res 打包使用默认分组配置（`BuildPackage_DefaultConfigs.GetDefaults()`）；如需自定义目录粒度，可在既有 AB 打包窗口调整后到 `BuildPackage_EditorWindow` 保存（`BuildPackage_Config` 序列化格式未变）。
8. **跨平台构建模块**：按目标平台打 AB / 玩家包需对应 Build Support 模块已安装，未安装时 Unity 会抛出明确错误（已捕获并记录日志）。
9. **勾选即持久化**：步骤勾选状态写 EditorPrefs（键 `honor_build_order_` + 订单 Name），与流水线执行共享；改勾选不需要重新编译。

---

## 8. 扩展点

- **新增打包步骤**：在 `OrderType` 增加枚举值 → 新建 `BuildOrder` 子类（构造函数里设 `Name/tips/orderType/action`）→ 在 `BuildOrderHandler._orderTypeDic` 注册 → 需要时加入对应流水线数组。
- **修改 res 分组**：编辑 `BuildPackage_Config` 配置（既有窗口）即可，打包器读取同一份配置。
- **新增平台**：`BuildPackage_Define.GetPlatformName(BuildTarget)` 增加分支即可（同时注意运行时宏命名是否一致）。
