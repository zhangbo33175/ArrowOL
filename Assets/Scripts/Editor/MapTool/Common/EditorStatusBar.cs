/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EditorStatusBar.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器状态栏管理器（UI 引用 + 增量刷新 + 临时消息）
 *            从 SceneEditModule 抽离，职责：状态栏 Label 引用、值变化检测、临时提示消息绘制
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器状态栏管理器
    /// 职责：
    /// 1. 持有状态栏 UI 元素引用（缩放/鼠标坐标/格子坐标/保存路径）
    /// 2. 增量刷新：只在值变化时更新 Label，避免每帧重建 UI
    /// 3. 临时提示消息：ShowMessage 显示 N 秒后自动消失
    /// </summary>
    public class EditorStatusBar
    {
        // ==================== UI 元素引用 ====================
        private Label m_ZoomLabel;
        private Label m_MouseXLabel;
        private Label m_MouseYLabel;
        private Label m_TileXLabel;
        private Label m_TileYLabel;
        private Label m_TileCountLabel;
        private Button m_SavePathBtn;

        // ==================== 上次值缓存（增量刷新） ====================
        private float m_LastZoom = -1;
        private float m_LastMouseX = float.MinValue;
        private float m_LastMouseY = float.MinValue;
        private int m_LastTileX = int.MinValue;
        private int m_LastTileY = int.MinValue;
        private string m_LastSavePath = "";

        // ==================== 临时消息 ====================
        private string m_TempMessage;
        private float m_TempMessageTime;

        /// <summary>
        /// 从 UXML 根节点查找状态栏元素
        /// </summary>
        public void FindElements(VisualElement root)
        {
            m_ZoomLabel = root.Q<Label>(name: MapToolUiNames.StatusZoom);
            m_MouseXLabel = root.Q<Label>(name: MapToolUiNames.StatusMouseX);
            m_MouseYLabel = root.Q<Label>(name: MapToolUiNames.StatusMouseY);
            m_TileXLabel = root.Q<Label>(name: MapToolUiNames.StatusTileX);
            m_TileYLabel = root.Q<Label>(name: MapToolUiNames.StatusTileY);
            m_TileCountLabel = root.Q<Label>(name: MapToolUiNames.StatusTileCount);
            m_SavePathBtn = root.Q<Button>(name: MapToolUiNames.StatusSavePath);
        }

        /// <summary>
        /// 绑定保存路径按钮点击事件
        /// </summary>
        public void BindSavePathClick(System.Action onClick)
        {
            if (m_SavePathBtn != null)
                m_SavePathBtn.clicked += () => onClick?.Invoke();
        }

        /// <summary>
        /// 增量刷新状态栏（只在值变化时更新 Label）
        /// </summary>
        /// <param name="zoom">当前缩放倍率</param>
        /// <param name="mousePos">鼠标世界坐标</param>
        /// <param name="tileX">格子 X</param>
        /// <param name="tileY">格子 Y</param>
        /// <param name="savePath">地图保存路径</param>
        public void Refresh(float zoom, Vector2 mousePos, int tileX, int tileY, string savePath)
        {
            bool needUpdate = false;

            float zoomPercent = zoom * 100f;
            if (!Mathf.Approximately(zoomPercent, m_LastZoom))
            {
                m_LastZoom = zoomPercent;
                needUpdate = true;
            }
            if (!Mathf.Approximately(mousePos.x, m_LastMouseX))
            {
                m_LastMouseX = mousePos.x;
                needUpdate = true;
            }
            if (!Mathf.Approximately(mousePos.y, m_LastMouseY))
            {
                m_LastMouseY = mousePos.y;
                needUpdate = true;
            }
            if (tileX != m_LastTileX)
            {
                m_LastTileX = tileX;
                needUpdate = true;
            }
            if (tileY != m_LastTileY)
            {
                m_LastTileY = tileY;
                needUpdate = true;
            }
            if (savePath != m_LastSavePath)
            {
                m_LastSavePath = savePath;
                needUpdate = true;
            }

            if (!needUpdate) return;

            if (m_ZoomLabel != null) m_ZoomLabel.text = $"{zoomPercent:F0}%";
            if (m_MouseXLabel != null) m_MouseXLabel.text = mousePos.x.ToString("F0");
            if (m_MouseYLabel != null) m_MouseYLabel.text = mousePos.y.ToString("F0");
            if (m_TileXLabel != null) m_TileXLabel.text = tileX.ToString();
            if (m_TileYLabel != null) m_TileYLabel.text = tileY.ToString();
            if (m_TileCountLabel != null) m_TileCountLabel.text = (tileX * tileY).ToString();
            if (m_SavePathBtn != null) m_SavePathBtn.text = savePath;
        }

        /// <summary>
        /// 显示临时提示消息（几秒后自动消失）
        /// </summary>
        public void ShowMessage(string msg)
        {
            m_TempMessage = msg;
            m_TempMessageTime = Time.realtimeSinceStartup + MapToolConstants.TempMessageDuration;
        }

        /// <summary>
        /// 在 IMGUI 场景视图里绘制临时消息（由 OnSceneGUI 调用）
        /// 样式：底部居中、红色、14号字（与原 SceneEditModule 行为一致）
        /// </summary>
        public void DrawTempMessage(Rect viewRect)
        {
            if (string.IsNullOrEmpty(m_TempMessage)) return;
            if (Time.realtimeSinceStartup > m_TempMessageTime)
            {
                m_TempMessage = null;
                return;
            }
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.red },
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, viewRect.height - 40, viewRect.width, 30), m_TempMessage, style);
        }
    }
}
