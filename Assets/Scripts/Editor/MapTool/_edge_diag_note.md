# 菱形右缘窄竖带 — 排查结论（2026-11-17）

## 现象
菱形（45° 斜格）模式下，地图最右缘一条竖向窄带内斜格被切成竖排细三角/窄条；用户同时要求网格紧挨背景边缘、不留空隙、不超出背景。

## 运行时实测（临时诊断日志，已移除）
容器 `m_GuiContainer.contentRect = 1493 × 492`（宽扁）；地图数据 `m_MapWidth = m_MapHeight = 4096`（方形）。

| 状态 | zoom | offset | mapRect | 宽高比 |
|---|---|---|---|---|
| 100% | 1.0000 | (0, 0) | 4096×4096 | 1.000 |
| 缩小中 | 0.4000 | (0, 0) | 1638.4×1638.4 | 1.000 |
| 铺满 | 0.3645 | (0, -500.5) | 1493×1493 | 1.000 |

**mapRect 全程是方形**，由同一 `m_Zoom` 等比缩放（`OnSceneGUI` Render.cs:39-44），不存在独立 X/Y 拉伸；斜线端点经 `CellScreen` 索引 Clamp + `TryClipToRect`（Liang-Barsky）裁剪到地图矩形内，深色画布区无斜线探出。

## 根因
- 视口容器被压成宽扁 `1493×492`；铺满缩放 `minZoom = Max(rect.width/4096, rect.height/4096) = width 方向主导`（Interaction.cs:289-291），方形地图按宽度铺满后高度仍有 1493，上下被 `Overflow.Hidden` 裁掉（Methods.cs:929），居中偏移 `offset.y = (492-1493)/2 = -500.5`。
- 右缘窄带 = 这张方形地图在宽扁视口里被裁到最右列时，外框把右边缘一列菱形切成半菱形竖条；底部同理出现一排被切下半的三角齿。属视口尺寸/裁口现象，不是网格坐标、吸附、落盘或斜线裁剪错误。
- 网格口径本身已满足"紧挨边缘、不留空隙、不超出背景"：首格点对齐左上角、末格点对齐右下角，线段裁剪在 mapRect 内。

## 涉及代码点
- mapRect 构造：`SceneEditModule.Render.cs:39-44`
- 菱形网格 / 边界对齐 EdgeCoord / 外框：`Render.cs:373-404`
- 斜线绘制与裁剪：`Render.cs:412-490`
- 铺满缩放与居中：`Interaction.cs:283-301`
- 拖拽/平移边界钳制：`Interaction.cs:48-55`、`261-264`
- 容器创建与裁剪：`Methods.cs:927-932`
- 窗口固定尺寸：`SenceMapTool.cs:31-32`（minSize=maxSize=1700×860）
- 视口布局：`SenceMapTool.uxml:107`（.scene-view）、`SenceMapTool.uss:306-311`

## 待确认/可选修复方向（未改生产代码）
1. 视口高度为何仅 492：窗口 1700×860 固定，右栏工具栏/预制体行/状态栏占用后 `.scene-view` 剩余高度异常偏小；需在实际布局里量取 `.right-main-area` / `.mode-panel` / `.scene-view` 的 computed 尺寸，确认是否有固定高度或 `min-height` 回收失败。
2. 若希望方形地图在宽扁视口里完整可见（上下不留裁切窄条），铺满缩放应取 `Min(rect.width/mapW, rect.height/mapH)` 以高度主导，并水平居中；当前取 `Max` 以宽度主导导致上下裁切。改缩放口径需同步评估平移钳制（Interaction.cs:54-55 / 263-264）。
3. 右缘/底部半菱形窄条是视口裁切的可见结果；只要地图矩形完整落在视口内，边缘即恢复整菱形。

## 本轮改动
- 临时诊断字段 `m_RectDiagTick` 与 `[RectDiag]` 日志已从 Render.cs / Visitors.cs 移除；编译产物回到删蓝点后基线 854528 字节，无新增 CS 错误。
- 备份（DefaultImporter，不参与编译）：`SceneEditModule.Render.cs.20261117_rectdiag.bak`、`SceneEditModule.Visitors.cs.20261117_rectdiag.bak`（含对应 .meta）。

## 2026-11-17 第二轮（ec021bf6 右缘竖窄带 + 底部横窄带）
### 实测复核
- 100% 态状态栏 zoom 100%、网格 128×128、宽高 4096；截图绿底 bbox L205 R1120 T117 B425（矩形，无梯形）；内部斜线最小二乘拟合斜率中位 ±1.0（45° 未压斜），行列步长等距；深色画布区 teal 斜线计数 0（无越界探出）。
- 放大件：右缘竖窄带 / 底部横窄带内是被外框线压住外侧半的半菱形；外框 boundaryColor 灰线压在 mapRect 最外格线上。
- 铺满态实机容器 1493×492；mapRect 全程方形、同一 m_Zoom 等比缩放。

### 根因（与上一轮一致，细化）
- 网格坐标 / 吸附 / 落盘 / 斜线裁剪（CellScreen 索引 Clamp + TryClipToRect）本身正确，首格点对齐左上角、末格点对齐右下角，满足"紧挨边缘、不留空隙、不超出背景"。
- 窄带可见结果：方形地图在宽扁视口（1493×492）里被裁到边缘，外框把最右列 / 最底行菱形切去外侧半。
- 铺满缩放旧口径 `minZoom = Max(rect.width/mapW, rect.height/mapH)` 宽度主导：宽扁视口按宽度铺满后地图高度仍超出视口，上下被 Overflow.Hidden 裁切（居中 offset.y=(492-1493)/2=-500.5）。

### 已落地代码改动（Interaction.cs OnScrollZoom）
- 铺满下限改为 `Mathf.Min(minZoomX, minZoomY)` 高度主导：整图完整落在视口内，水平/垂直居中；边缘菱形不再被视口裁成半格窄条。
- 同步更新方法头与行内注释；平移钳制（Interaction.cs:54-55 / 263-264）保持原样，Min 口径下地图短边方向本就完整可见，钳制不影响整图可见性。
- 备份：`SceneEditModule.Interaction.cs.20261117_fitmin.bak`。

### 编译与实机验证（已完成）
- 在运行实例内 Ctrl+R 触发重编译：最新编译块 412/412 仅既有 warning（CS0108/CS0618/CS0219/CS0414/CS0618），无 error CS；Assembly-CSharp-Editor.dll 重载成功。日志中 CS2001（_baseline_backup/*.cs，目录与 .meta 均 DefaultImporter 不参与编译）、CS0165（slot 源码已 `=0` 初始化）均为历史残留，与本轮改动无关。
- 实机铺满态（状态栏 12%）：地图矩形 489×489 方形完整落在视口内（上下 119–607），右侧/右侧外为深色画布留白；放大右缘竖带、底边横带均为整菱形齿，不再被视口裁成半格窄条。
- 备份：`SceneEditModule.Interaction.cs.20261117_fitmin.bak`。

### 未决（非本轮窄带成因，后续可选）
- 视口高度仅约 492 的布局成因仍未定位：窗口固定 1700×860，右栏工具栏/预制体行/状态栏/底部属性区占用后 .scene-view 剩余高度偏小；隐藏面板（ObjectEditPanel/PropertyPanel）代码初始 display=None，截图右侧无边框占位，但底栏整体仍占约 244px。后续可在实机读取各 VisualElement computed 尺寸确认是否有固定高度或 min-height 回收失败。
- 注意：Min 口径下宽扁视口铺满时地图按高度缩放、左右留画布边距；这是"整图完整可见"的等比代价，不影响网格坐标/吸附/落盘。
