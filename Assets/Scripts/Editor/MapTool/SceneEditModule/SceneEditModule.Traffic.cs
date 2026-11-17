/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Traffic.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 交通规则配置（P2 编辑器可视化与一致性校验）
 *            数据仅落盘进 MapConfigData 交通子表；规则判定在运行时 Lua 门面，C# 层不写交通规则
 ***************************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 交通规则编辑工具模式
    /// </summary>
    public enum TrafficTool
    {
        /// <summary>无交通工具（默认选择/摆放模式）</summary>
        None = 0,

        /// <summary>框选刷人行道可走格</summary>
        SidewalkBrush = 1,

        /// <summary>框选刷车辆行驶格</summary>
        RoadBrush = 2,

        /// <summary>两点绘制斑马线</summary>
        Crosswalk = 3,

        /// <summary>点击放置红绿灯</summary>
        Light = 4
    }

    /// <summary>
    /// 场景编辑模块 - 交通规则配置系列私有方法
    /// </summary>
    public partial class SceneEditModule
    {
        #region 交通工具状态

        /// <summary>当前交通工具模式</summary>
        private TrafficTool m_TrafficTool = TrafficTool.None;

        /// <summary>两段式录入的第一锚点（刷格矩形 / 横道起点），无锚点为 null</summary>
        private Vector2? m_TrafficAnchor = null;

        #endregion

        #region 交通面板控件

        /// <summary>交通规则子面板容器（代码动态构建，挂到底部属性面板顶部；可由显隐开关折叠）</summary>
        private VisualElement m_TrafficPanel;

        /// <summary>交通规则界面显隐开关（常驻属性面板顶部，本体收起后仍可重新展开）</summary>
        private Toggle m_TrafficVisibleToggle;

        /// <summary>交通规则界面是否展开（默认不勾选、本体折叠）</summary>
        private bool m_TrafficPanelVisible = false;

        /// <summary>载具标记开关</summary>
        private Toggle m_TrafficVehicleToggle;

        /// <summary>交通角色下拉：行人 / 载具 / 不参与</summary>
        private DropdownField m_TrafficRoleField;

        /// <summary>停止线坐标输入（X,Y 世界像素）</summary>
        private TextField m_TrafficStopLineField;

        /// <summary>绑定红绿灯多选勾选列表容器（选项来自 m_TrafficLights，支持多选/全选）</summary>
        private VisualElement m_TrafficLightBindContainer;

        /// <summary>绑定灯全选开关</summary>
        private Toggle m_TrafficSelectAllLightsToggle;

        /// <summary>单灯勾选框映射：灯实例 ID → Toggle</summary>
        private readonly Dictionary<string, Toggle> m_TrafficLightToggles = new Dictionary<string, Toggle>();

        /// <summary>黄灯按红灯处理开关（仅车辆消费）</summary>
        private Toggle m_TrafficYellowRedToggle;

        /// <summary>交通工具按钮与工具枚举映射（用于激活态高亮刷新）</summary>
        private readonly Dictionary<TrafficTool, Button> m_TrafficToolButtons = new Dictionary<TrafficTool, Button>();

        #endregion

        #region 面板构建与回填

        /// <summary>
        /// 构建交通规则子面板（代码动态创建，不依赖 UXML 节点；挂到属性面板顶部）
        /// </summary>
        private void BuildTrafficPanel()
        {
            // 交通规则配置面板挂到右栏操作卡片；卡片缺失时回退属性流 host，再回退属性面板本体
            var actionHost = m_PropertyActionArea != null ? m_PropertyActionArea : (m_PropertyActionHost != null ? m_PropertyActionHost : m_PropertyPanel);

            // 交通规则开关：优先复用交互行（收集开关后面）的 UXML Toggle；缺失时兜底在右栏新建一行
            m_TrafficVisibleToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PropTrafficToggle);
            if (m_TrafficVisibleToggle == null)
            {
                var trafficToggleRow = new VisualElement();
                trafficToggleRow.AddToClassList("traffic-toggle-row");
                var trafficToggleLabel = new Label("交通规则");
                trafficToggleLabel.AddToClassList("traffic-toggle-label");
                m_TrafficVisibleToggle = new Toggle();
                m_TrafficVisibleToggle.AddToClassList("traffic-toggle");
                trafficToggleRow.Add(trafficToggleLabel);
                trafficToggleRow.Add(m_TrafficVisibleToggle);
                actionHost?.Add(trafficToggleRow);
            }
            m_TrafficVisibleToggle.value = m_TrafficPanelVisible;
            m_TrafficVisibleToggle.RegisterValueChangedCallback(evt => SetTrafficPanelVisible(evt.newValue));

            m_TrafficPanel = new VisualElement();
            m_TrafficPanel.name = "traffic-panel";
            m_TrafficPanel.AddToClassList("traffic-panel");
            m_TrafficPanel.style.display = m_TrafficPanelVisible ? DisplayStyle.Flex : DisplayStyle.None;

            // ===== 工具切换行 =====
            var toolRow = new VisualElement();
            toolRow.AddToClassList("traffic-tool-row");
            AddTrafficToolButton(toolRow, "人行道刷格", TrafficTool.SidewalkBrush);
            AddTrafficToolButton(toolRow, "道路刷格", TrafficTool.RoadBrush);
            AddTrafficToolButton(toolRow, "画横道", TrafficTool.Crosswalk);
            AddTrafficToolButton(toolRow, "放红绿灯", TrafficTool.Light);
            m_TrafficPanel.Add(toolRow);

            // ===== 载具 / 行人属性（选中角色时回填） =====
            var vehicleRow = new VisualElement();
            vehicleRow.AddToClassList("traffic-toggle-row");
            var vehicleLabel = new Label("是否载具");
            vehicleLabel.AddToClassList("traffic-toggle-label");
            m_TrafficVehicleToggle = new Toggle();
            m_TrafficVehicleToggle.AddToClassList("traffic-toggle");
            m_TrafficVehicleToggle.RegisterValueChangedCallback(evt =>
            {
                if (m_EditTarget == null) return;
                m_EditTarget.IsVehicle = evt.newValue;
                m_EditTarget.TrafficRole = evt.newValue ? "vehicle" : "pedestrian";
                ApplyTrafficLinkage(m_EditTarget);
                RefreshTrafficRoleChoices();
            });
            vehicleRow.Add(vehicleLabel);
            vehicleRow.Add(m_TrafficVehicleToggle);
            m_TrafficPanel.Add(vehicleRow);

            m_TrafficRoleField = new DropdownField();
            m_TrafficRoleField.choices = new List<string> { "行人", "载具", "不参与" };
            m_TrafficRoleField.RegisterValueChangedCallback(evt =>
            {
                if (m_EditTarget == null) return;
                m_EditTarget.TrafficRole = TrafficRoleToCode(evt.newValue);
                m_EditTarget.IsVehicle = m_EditTarget.TrafficRole == "vehicle";
                ApplyTrafficLinkage(m_EditTarget);
            });
            m_TrafficPanel.Add(AddTrafficFieldRow("交通角色", m_TrafficRoleField));

            m_TrafficStopLineField = new TextField();
            m_TrafficStopLineField.RegisterValueChangedCallback(evt =>
            {
                if (m_EditTarget == null) return;
                ParseStopLine(evt.newValue, out float sx, out float sy);
                m_EditTarget.StopLineX = sx;
                m_EditTarget.StopLineY = sy;
            });
            m_TrafficPanel.Add(AddTrafficFieldRow("停止线(X,Y)", m_TrafficStopLineField));

            // 绑定红绿灯：全选行 + 逐灯勾选列表（多选，选项来自当前地图已放置灯）
            var lightRow = new VisualElement();
            lightRow.AddToClassList("traffic-field-row");
            var lightLabel = new Label("绑定红绿灯");
            lightLabel.AddToClassList("traffic-field-label");
            m_TrafficSelectAllLightsToggle = new Toggle();
            m_TrafficSelectAllLightsToggle.AddToClassList("traffic-toggle");
            var selectAllLabel = new Label("全选");
            selectAllLabel.AddToClassList("traffic-toggle-label");
            selectAllLabel.AddToClassList("traffic-select-all");
            m_TrafficSelectAllLightsToggle.RegisterValueChangedCallback(OnSelectAllLightsChanged);
            lightRow.Add(lightLabel);
            lightRow.Add(m_TrafficSelectAllLightsToggle);
            lightRow.Add(selectAllLabel);
            m_TrafficPanel.Add(lightRow);

            var lightScroll = new ScrollView();
            lightScroll.AddToClassList("traffic-light-bind-list");
            m_TrafficLightBindContainer = lightScroll.contentContainer;
            m_TrafficPanel.Add(lightScroll);

            var yellowRow = new VisualElement();
            yellowRow.AddToClassList("traffic-toggle-row");
            var yellowLabel = new Label("黄灯按红灯处理（仅车辆）");
            yellowLabel.AddToClassList("traffic-toggle-label");
            m_TrafficYellowRedToggle = new Toggle();
            m_TrafficYellowRedToggle.AddToClassList("traffic-toggle");
            m_TrafficYellowRedToggle.RegisterValueChangedCallback(evt =>
            {
                if (m_EditTarget == null) return;
                m_EditTarget.YellowTreatAsRed = evt.newValue;
            });
            yellowRow.Add(yellowLabel);
            yellowRow.Add(m_TrafficYellowRedToggle);
            m_TrafficPanel.Add(yellowRow);

            // 地图级要素清单（灯/横道/区域）与一致性校验结果不属于单个选中物件，不在物件属性面板显示，避免冗余渲染。

            actionHost?.Add(m_TrafficPanel);
            RefreshTrafficToolButtons();
        }

        /// <summary>
        /// 构建「标签 + 控件」对齐字段行：标签等宽右对齐，控件占满剩余宽度
        /// </summary>
        /// <param name="label">字段标签</param>
        /// <param name="control">字段控件（下拉 / 输入框）</param>
        /// <returns>字段行容器</returns>
        private VisualElement AddTrafficFieldRow(string label, VisualElement control)
        {
            control.AddToClassList("traffic-input");
            var row = new VisualElement();
            row.AddToClassList("traffic-field-row");
            var lbl = new Label(label);
            lbl.AddToClassList("traffic-field-label");
            row.Add(lbl);
            row.Add(control);
            return row;
        }

        /// <summary>
        /// 展开 / 折叠交通规则配置界面（勾选显示，取消隐藏）
        /// </summary>
        /// <param name="visible">是否展开</param>
        private void SetTrafficPanelVisible(bool visible)
        {
            m_TrafficPanelVisible = visible;
            if (m_TrafficPanel != null)
            {
                m_TrafficPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
            RefreshActionAreaVisibility();
        }

        /// <summary>
        /// 添加交通工具切换按钮（互斥：再点当前工具即切回 None）
        /// </summary>
        private void AddTrafficToolButton(VisualElement parent, string text, TrafficTool tool)
        {
            var btn = new Button(() => SetTrafficTool(m_TrafficTool == tool ? TrafficTool.None : tool));
            btn.text = text;
            btn.AddToClassList("traffic-tool-btn");
            m_TrafficToolButtons[tool] = btn;
            parent.Add(btn);
        }

        /// <summary>刷新交通工具按钮激活态（当前工具高亮，其余还原）</summary>
        private void RefreshTrafficToolButtons()
        {
            foreach (var kv in m_TrafficToolButtons)
            {
                kv.Value.EnableInClassList("active", m_TrafficTool == kv.Key);
            }
        }

        /// <summary>
        /// 切换交通工具模式；退出时清空两段式锚点
        /// </summary>
        private void SetTrafficTool(TrafficTool tool)
        {
            m_TrafficTool = tool;
            m_TrafficAnchor = null;
            RefreshTrafficToolButtons();
            Debug.Log($"交通工具切换: {tool}");
            if (m_GuiContainer != null) m_GuiContainer.MarkDirtyRepaint();
        }

        /// <summary>
        /// 回填选中物体的交通属性，并刷新灯下拉 / 要素列表 / 校验结果
        /// </summary>
        private void RefreshTrafficPanel()
        {
            RefreshTrafficLightChoices();
            RefreshTrafficRoleChoices();

            if (m_EditTarget == null)
            {
                return;
            }

            if (m_TrafficVehicleToggle != null)
                m_TrafficVehicleToggle.SetValueWithoutNotify(m_EditTarget.IsVehicle);
            if (m_TrafficRoleField != null)
                m_TrafficRoleField.SetValueWithoutNotify(TrafficRoleToDisplay(m_EditTarget.TrafficRole));
            if (m_TrafficStopLineField != null)
                m_TrafficStopLineField.SetValueWithoutNotify($"{m_EditTarget.StopLineX:0.#}, {m_EditTarget.StopLineY:0.#}");
            RefreshLightBindChoices();
            if (m_TrafficYellowRedToggle != null)
                m_TrafficYellowRedToggle.SetValueWithoutNotify(m_EditTarget.YellowTreatAsRed);
        }

        /// <summary>刷新交通角色下拉选中态（随载具标记联动）</summary>
        private void RefreshTrafficRoleChoices()
        {
            if (m_TrafficRoleField == null) return;
            string role = m_EditTarget != null ? m_EditTarget.TrafficRole : "pedestrian";
            m_TrafficRoleField.SetValueWithoutNotify(TrafficRoleToDisplay(role));
        }

        /// <summary>
        /// 判断条目是否参与交通动态链路（载具或行人；none/空 不参与）
        /// </summary>
        /// <param name="obj">选中摆放物</param>
        /// <returns>是否参与交通链路</returns>
        private static bool IsTrafficRoleActive(PlacedObject obj)
        {
            if (obj == null) return false;
            if (obj.IsVehicle) return true;
            return obj.TrafficRole == "vehicle" || obj.TrafficRole == "pedestrian";
        }

        /// <summary>
        /// 按交通角色联动归属类型：参与交通的条目归入角色动态链路（Character），
        /// 复用角色表落盘/路径巡逻/红绿灯让行判定；静态物件链路不承载交通规则。
        /// </summary>
        /// <param name="obj">选中摆放物</param>
        private void ApplyTrafficLinkage(PlacedObject obj)
        {
            if (obj == null) return;
            if (IsTrafficRoleActive(obj))
            {
                obj.Type = MapObjectType.Character;
            }
        }

        /// <summary>重建/回填绑定灯多选勾选列表：按当前地图灯集合生成逐灯勾选框，并回填选中态与全选态</summary>
        private void RefreshTrafficLightChoices()
        {
            RefreshLightBindChoices();
        }

        /// <summary>重建/回填绑定灯多选勾选列表（单灯勾选 + 全选）</summary>
        private void RefreshLightBindChoices()
        {
            if (m_TrafficLightBindContainer == null) return;
            m_TrafficLightBindContainer.Clear();
            m_TrafficLightToggles.Clear();

            var selected = m_EditTarget != null ? m_EditTarget.BoundLightIds : new List<string>();
            // 旧单值兜底并入选中集合（旧地图兼容）
            if (m_EditTarget != null && !string.IsNullOrEmpty(m_EditTarget.BoundLightId)
                && !selected.Contains(m_EditTarget.BoundLightId))
            {
                selected.Add(m_EditTarget.BoundLightId);
            }

            if (m_TrafficLights.Count == 0)
            {
                var empty = new Label("（当前地图无红绿灯）");
                empty.AddToClassList("traffic-item-empty");
                m_TrafficLightBindContainer.Add(empty);
                SetSelectAllLightsState(false, false);
                return;
            }

            foreach (var light in m_TrafficLights)
            {
                var toggle = new Toggle(LightIdToChoice(light.id));
                toggle.AddToClassList("traffic-light-item");
                toggle.SetValueWithoutNotify(selected.Contains(light.id));
                string lightId = light.id;
                toggle.RegisterValueChangedCallback(evt => OnSingleLightChanged(lightId, evt.newValue));
                m_TrafficLightToggles[light.id] = toggle;
                m_TrafficLightBindContainer.Add(toggle);
            }
            RefreshSelectAllState();
        }

        /// <summary>单个灯勾选变化：加入/移出绑定列表，同步旧单值首项与全选态</summary>
        private void OnSingleLightChanged(string lightId, bool on)
        {
            if (m_EditTarget == null) return;
            var list = m_EditTarget.BoundLightIds;
            if (on && !list.Contains(lightId)) list.Add(lightId);
            if (!on) list.Remove(lightId);
            SyncLegacyBoundLightId();
            RefreshSelectAllState();
        }

        /// <summary>全选开关变化：勾选=绑定全部当前灯；取消=清空绑定</summary>
        private void OnSelectAllLightsChanged(ChangeEvent<bool> evt)
        {
            if (m_EditTarget == null) return;
            var list = m_EditTarget.BoundLightIds;
            list.Clear();
            if (evt.newValue)
            {
                foreach (var light in m_TrafficLights) list.Add(light.id);
            }
            SyncLegacyBoundLightId();
            foreach (var kv in m_TrafficLightToggles)
            {
                kv.Value.SetValueWithoutNotify(list.Contains(kv.Key));
            }
        }

        /// <summary>按当前勾选集合刷新全选开关态（不触发回调）</summary>
        private void RefreshSelectAllState()
        {
            bool all = m_TrafficLights.Count > 0 && m_TrafficLightToggles.Count == m_TrafficLights.Count;
            if (all)
            {
                foreach (var kv in m_TrafficLightToggles) all &= kv.Value.value;
            }
            SetSelectAllLightsState(all, false);
        }

        /// <summary>设置全选开关值；notify=false 时静默回填，不触发回调</summary>
        private void SetSelectAllLightsState(bool on, bool notify)
        {
            if (m_TrafficSelectAllLightsToggle == null) return;
            if (notify) m_TrafficSelectAllLightsToggle.value = on;
            else m_TrafficSelectAllLightsToggle.SetValueWithoutNotify(on);
        }

        /// <summary>同步旧单值 BoundLightId 为列表首项，兼容旧导出与校验口径</summary>
        private void SyncLegacyBoundLightId()
        {
            if (m_EditTarget == null) return;
            m_EditTarget.BoundLightId = m_EditTarget.BoundLightIds.Count > 0
                ? m_EditTarget.BoundLightIds[0] : string.Empty;
        }

        #endregion

        #region 数据录入（两段式工具点击）

        /// <summary>
        /// 交通工具鼠标点击入口（由 HandleMouseEvent 左键分支拦截调用）
        /// </summary>
        /// <param name="e">IMGUI 鼠标事件</param>
        private void OnTrafficToolClick(Event e)
        {
            Vector2 worldPos = SnapToGrid(ScreenToWorld(e.mousePosition));
            switch (m_TrafficTool)
            {
                case TrafficTool.SidewalkBrush:
                case TrafficTool.RoadBrush:
                    HandleBrushClick(worldPos);
                    break;
                case TrafficTool.Crosswalk:
                    HandleCrosswalkClick(worldPos);
                    break;
                case TrafficTool.Light:
                    HandleLightClick(worldPos);
                    break;
            }
            RefreshTrafficPanel();
            RefreshSceneObjects();
            if (m_GuiContainer != null) m_GuiContainer.MarkDirtyRepaint();
        }

        /// <summary>刷格：两段式矩形，第二点落格并回写区域回显</summary>
        private void HandleBrushClick(Vector2 worldPos)
        {
            bool sidewalk = m_TrafficTool == TrafficTool.SidewalkBrush;
            List<MapGridCellData> target = sidewalk ? m_SidewalkCells : m_RoadCells;

            if (!m_TrafficAnchor.HasValue)
            {
                m_TrafficAnchor = worldPos;
                Debug.Log($"{(sidewalk ? "人行道" : "道路")}刷格：第一角已记录，再点第二角成矩形");
                return;
            }

            Vector2 a = m_TrafficAnchor.Value;
            // ---TODO 形状分支：菱形（两点为对角）/ 矩形（轴对齐），跟随底部「摆放模式」形状下拉
            bool diamond = DrawShape.IsDiamond(m_DrawShape);
            if (diamond) ExpandDiamondToCells(a, worldPos, target);
            else ExpandRectToCells(a, worldPos, target);
            var region = new MapRegionData
            {
                id = NewTrafficId(),
                type = sidewalk ? "sidewalk" : "road",
                x = Mathf.Min(a.x, worldPos.x),
                y = Mathf.Min(a.y, worldPos.y),
                width = Mathf.Abs(worldPos.x - a.x) + MapToolConstants.GridSize,
                height = Mathf.Abs(worldPos.y - a.y) + MapToolConstants.GridSize
            };
            m_Regions.Add(region);
            m_TrafficAnchor = null;
            Debug.Log($"{(sidewalk ? "人行道" : "道路")}{(diamond ? "菱形" : "矩形")}区域已录入，格子 {target.Count}");
        }

        /// <summary>画横道：两段式两点，自动按走向判定方向并绑定最近横道对应的灯（后置可在面板改绑）</summary>
        private void HandleCrosswalkClick(Vector2 worldPos)
        {
            if (!m_TrafficAnchor.HasValue)
            {
                m_TrafficAnchor = worldPos;
                Debug.Log("横道：起点已记录，再点终点");
                return;
            }

            Vector2 a = m_TrafficAnchor.Value;
            bool horizontal = Mathf.Abs(worldPos.x - a.x) >= Mathf.Abs(worldPos.y - a.y);
            var cw = new MapCrosswalkData
            {
                id = NewTrafficId(),
                name = "横道" + (m_Crosswalks.Count + 1),
                pointA = new MapPathPointData(a.x, a.y),
                pointB = new MapPathPointData(worldPos.x, worldPos.y),
                width = MapToolConstants.GridSize,
                direction = horizontal ? "horizontal" : "vertical"
            };
            m_Crosswalks.Add(cw);
            m_TrafficAnchor = null;
            Debug.Log($"横道已录入: {cw.id} 方向 {cw.direction}");
        }

        /// <summary>放灯：单点放置，自动绑定距离最近的横道</summary>
        private void HandleLightClick(Vector2 worldPos)
        {
            var nearest = NearestCrosswalk(worldPos);
            var light = new MapTrafficLightData
            {
                id = NewTrafficId(),
                name = "红绿灯" + (m_TrafficLights.Count + 1),
                posX = worldPos.x,
                posY = worldPos.y,
                gridX = MapToolEditorUtils.WorldToCell(worldPos).x,
                gridY = MapToolEditorUtils.WorldToCell(worldPos).y,
                direction = nearest != null ? nearest.direction : "horizontal",
                side = "north",
                boundCrosswalkId = nearest != null ? nearest.id : string.Empty,
                initialPhase = "red"
            };
            m_TrafficLights.Add(light);
            if (nearest != null && string.IsNullOrEmpty(nearest.boundLightId))
            {
                nearest.boundLightId = light.id;
            }
            Debug.Log($"红绿灯已放置: {light.id} 绑定横道 {light.boundCrosswalkId}");
        }

        /// <summary>把矩形展开为格子集合（吸附网格、去重），写入目标集合</summary>
        private void ExpandRectToCells(Vector2 a, Vector2 b, List<MapGridCellData> target)
        {
            Vector2Int c0 = MapToolEditorUtils.WorldToCell(new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y)));
            Vector2Int c1 = MapToolEditorUtils.WorldToCell(new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y)));
            int x0 = c0.x, y0 = c0.y, x1 = c1.x, y1 = c1.y;

            var exist = new HashSet<long>(target.Select(c => MapToolEditorUtils.EncodeCell(c.x, c.y)));
            for (int gy = y0; gy <= y1; gy++)
            {
                for (int gx = x0; gx <= x1; gx++)
                {
                    long key = MapToolEditorUtils.EncodeCell(gx, gy);
                    if (exist.Contains(key)) continue;
                    exist.Add(key);
                    target.Add(new MapGridCellData { x = gx, y = gy });
                }
            }
        }

        /// <summary>把菱形（两点为对角）展开为格子集合（吸附网格、去重），写入目标集合</summary>
        private void ExpandDiamondToCells(Vector2 a, Vector2 b, List<MapGridCellData> target)
        {
            // 菱形以两点中点为中心，半宽/半高取两轴距离；格子满足曼哈顿距离 ≤ 半径即纳入
            float cx = (a.x + b.x) * 0.5f;
            float cy = (a.y + b.y) * 0.5f;
            float halfW = Mathf.Abs(b.x - a.x) * 0.5f;
            float halfH = Mathf.Abs(b.y - a.y) * 0.5f;
            if (halfW < MapToolConstants.GridSize) halfW = MapToolConstants.GridSize;
            if (halfH < MapToolConstants.GridSize) halfH = MapToolConstants.GridSize;

            Vector2Int c0 = MapToolEditorUtils.WorldToCell(new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y)));
            Vector2Int c1 = MapToolEditorUtils.WorldToCell(new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y)));
            int x0 = c0.x, y0 = c0.y, x1 = c1.x, y1 = c1.y;

            var exist = new HashSet<long>(target.Select(c => MapToolEditorUtils.EncodeCell(c.x, c.y)));
            for (int gy = y0; gy <= y1; gy++)
            {
                for (int gx = x0; gx <= x1; gx++)
                {
                    float cellCx = gx * MapToolConstants.GridSize + MapToolConstants.GridSize * 0.5f;
                    float cellCy = gy * MapToolConstants.GridSize + MapToolConstants.GridSize * 0.5f;
                    float nx = Mathf.Abs(cellCx - cx) / halfW;
                    float ny = Mathf.Abs(cellCy - cy) / halfH;
                    if (nx + ny > 1f) continue;

                    long key = MapToolEditorUtils.EncodeCell(gx, gy);
                    if (exist.Contains(key)) continue;
                    exist.Add(key);
                    target.Add(new MapGridCellData { x = gx, y = gy });
                }
            }
        }

        /// <summary>查找距离指定位置最近的横道（按两端点中点距离）；无横道返回 null</summary>
        private MapCrosswalkData NearestCrosswalk(Vector2 pos)
        {
            MapCrosswalkData best = null;
            float bestDist = float.MaxValue;
            foreach (var cw in m_Crosswalks)
            {
                if (cw.pointA == null || cw.pointB == null) continue;
                Vector2 mid = new Vector2((cw.pointA.x + cw.pointB.x) * 0.5f, (cw.pointA.y + cw.pointB.y) * 0.5f);
                float dist = Vector2.Distance(pos, mid);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = cw;
                }
            }
            return best;
        }

        /// <summary>生成 8 位交通实例 ID</summary>
        private static string NewTrafficId()
        {
            return System.Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        #endregion

        #region 一致性校验

        /// <summary>
        /// 校验交通引用完整性与基本几何合法性；返回问题描述列表（空=通过）
        /// </summary>
        private List<string> ValidateTraffic()
        {
            var issues = new List<string>();
            var lightIds = new HashSet<string>(m_TrafficLights.Select(l => l.id));
            var crosswalkIds = new HashSet<string>(m_Crosswalks.Select(c => c.id));

            // 灯 → 横道引用
            foreach (var light in m_TrafficLights)
            {
                if (string.IsNullOrEmpty(light.boundCrosswalkId))
                {
                    issues.Add($"灯 {light.name}（{light.id}）未绑定横道。");
                }
                else if (!crosswalkIds.Contains(light.boundCrosswalkId))
                {
                    issues.Add($"灯 {light.name}（{light.id}）绑定的横道 {light.boundCrosswalkId} 不存在。");
                }
            }

            // 横道 → 灯引用 + 几何
            foreach (var cw in m_Crosswalks)
            {
                if (!string.IsNullOrEmpty(cw.boundLightId) && !lightIds.Contains(cw.boundLightId))
                {
                    issues.Add($"横道 {cw.name}（{cw.id}）绑定的灯 {cw.boundLightId} 不存在。");
                }
                if (cw.pointA == null || cw.pointB == null)
                {
                    issues.Add($"横道 {cw.name}（{cw.id}）缺少端点。");
                    continue;
                }
                if (Vector2.Distance(new Vector2(cw.pointA.x, cw.pointA.y), new Vector2(cw.pointB.x, cw.pointB.y)) < MapToolConstants.GridSize)
                {
                    issues.Add($"横道 {cw.name}（{cw.id}）两端距离过短。");
                }
                // 横道两端应落在人行道格内（缺失人行道格时仅提示）
                if (m_SidewalkCells.Count > 0 && !PointInSidewalk(cw.pointA, out _))
                {
                    issues.Add($"横道 {cw.name}（{cw.id}）起点未接人行道。");
                }
                if (m_SidewalkCells.Count > 0 && !PointInSidewalk(cw.pointB, out _))
                {
                    issues.Add($"横道 {cw.name}（{cw.id}）终点未接人行道。");
                }
            }

            // 交通条目 → 归属链路 + 载具灯引用 / 停止线落路面
            foreach (var obj in m_PlacedObjects)
            {
                if (!IsTrafficRoleActive(obj)) continue;
                // 参与交通的条目必须归入角色动态链路，否则交通字段不进角色表、运行时不消费
                if (obj.Type != MapObjectType.Character)
                {
                    issues.Add($"交通条目 {obj.Name}（{obj.Id}）未归入角色动态链路，交通属性不会生效。");
                }
                if (!obj.IsVehicle) continue;
                // ---TODO 交通规则：绑定灯多选校验，旧单值兜底一并检查
                var boundLightSet = new List<string>();
                if (obj.BoundLightIds != null)
                {
                    foreach (var id in obj.BoundLightIds)
                    {
                        if (!string.IsNullOrEmpty(id) && !boundLightSet.Contains(id)) boundLightSet.Add(id);
                    }
                }
                if (!string.IsNullOrEmpty(obj.BoundLightId) && !boundLightSet.Contains(obj.BoundLightId))
                    boundLightSet.Add(obj.BoundLightId);
                foreach (var lightId in boundLightSet)
                {
                    if (!lightIds.Contains(lightId))
                        issues.Add($"载具 {obj.Name}（{obj.Id}）绑定的灯 {lightId} 不存在。");
                }
                if (m_RoadCells.Count > 0 && !PointInRoad(obj.Position))
                {
                    issues.Add($"载具 {obj.Name}（{obj.Id}）不在道路格上。");
                }
            }
            return issues;
        }

        /// <summary>判断世界坐标是否落在人行道格集合内</summary>
        private bool PointInSidewalk(MapPathPointData point, out int cellKey)
        {
            cellKey = 0;
            if (point == null) return false;
            Vector2Int cell = MapToolEditorUtils.WorldToCell(new Vector2(point.x, point.y));
            return m_SidewalkCells.Any(c => c.x == cell.x && c.y == cell.y);
        }

        /// <summary>判断世界坐标是否落在道路格集合内</summary>
        private bool PointInRoad(Vector2 worldPos)
        {
            Vector2Int cell = MapToolEditorUtils.WorldToCell(worldPos);
            return m_RoadCells.Any(c => c.x == cell.x && c.y == cell.y);
        }

        #endregion

        #region 叠加绘制

        /// <summary>
        /// 画布叠加绘制交通要素：人行道格 / 道路格 / 横道 / 灯位 / 两段式锚点预览
        /// </summary>
        private void DrawTrafficOverlay(Rect mapRect)
        {
            if (!mapRect.Overlaps(m_GuiContainer != null ? m_GuiContainer.contentRect : mapRect)) return;

            Color sidewalkColor = new Color(0.25f, 0.75f, 0.35f, 0.18f);
            Color roadColor = new Color(0.35f, 0.35f, 0.35f, 0.22f);
            DrawCellSet(m_SidewalkCells, sidewalkColor);
            DrawCellSet(m_RoadCells, roadColor);

            // 横道白色条纹
            foreach (var cw in m_Crosswalks)
            {
                if (cw.pointA == null || cw.pointB == null) continue;
                Vector2 a = WorldToScreen(new Vector2(cw.pointA.x, cw.pointA.y));
                Vector2 b = WorldToScreen(new Vector2(cw.pointB.x, cw.pointB.y));
                Handles.color = Color.white;
                Handles.DrawLine(a, b);
            }

            // 灯位圆点（按初始相位着色）
            foreach (var light in m_TrafficLights)
            {
                Vector2 sp = WorldToScreen(new Vector2(light.posX, light.posY));
                bool green = light.initialPhase == "green";
                Handles.color = green ? Color.green : Color.red;
                Handles.DrawSolidDisc(sp, Vector3.forward, 6f);
            }

            // 两段式锚点预览
            if (m_TrafficAnchor.HasValue)
            {
                Vector2 anchor = WorldToScreen(m_TrafficAnchor.Value);
                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(anchor, Vector3.forward, 5f);
            }
        }

        /// <summary>绘制格子集合半透明小方块</summary>
        private void DrawCellSet(List<MapGridCellData> cells, Color color)
        {
            if (cells == null) return;
            float g = MapToolConstants.GridSize * m_Zoom;
            foreach (var cell in cells)
            {
                Vector2 sp = WorldToScreen(new Vector2(cell.x * MapToolConstants.GridSize, cell.y * MapToolConstants.GridSize));
                EditorGUI.DrawRect(new Rect(sp.x, sp.y, g, g), color);
            }
        }

        #endregion

        #region 角色 / 灯 ID 文案互转

        /// <summary>交通角色代码 → 下拉显示文案</summary>
        private static string TrafficRoleToDisplay(string code)
        {
            switch (code)
            {
                case "vehicle": return "载具";
                case "none": return "不参与";
                default: return "行人";
            }
        }

        /// <summary>下拉显示文案 → 交通角色代码</summary>
        private static string TrafficRoleToCode(string display)
        {
            switch (display)
            {
                case "载具": return "vehicle";
                case "不参与": return "none";
                default: return "pedestrian";
            }
        }

        /// <summary>灯实例 ID → 选项文案（空=无绑定）</summary>
        private string LightIdToChoice(string id)
        {
            if (string.IsNullOrEmpty(id)) return "无绑定";
            var light = m_TrafficLights.FirstOrDefault(l => l.id == id);
            return light != null ? $"{light.name}（{light.id}）" : id;
        }

        /// <summary>解析停止线坐标文案 “x, y”；解析失败返回 0,0</summary>
        private static void ParseStopLine(string text, out float x, out float y)
        {
            x = 0; y = 0;
            if (string.IsNullOrEmpty(text)) return;
            string[] parts = text.Split(',');
            if (parts.Length >= 1) float.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out x);
            if (parts.Length >= 2) float.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out y);
        }

        #endregion
    }
}
