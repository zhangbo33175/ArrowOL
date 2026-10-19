/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.PathEdit.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 私有方法（对象编辑面板/路径点列表交互）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Editor.MapEditor;
using System.Linq;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景编辑模块 - 路径编辑与对象面板私有方法
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 显示对象编辑面板占位（左侧选中人物但地图上尚未放置该人物）
        /// </summary>
        /// <param name="name">人物资源名称</param>
        private void ShowEditPanelWithName(string name)
        {
            m_EditTarget = null;
            m_SelectedPathPointIndex = -1;
            m_IsPathDrawing = false;
            if (m_ObjectEditPanel != null)
            {
                m_ObjectEditPanel.style.display = DisplayStyle.Flex;
                SetActiveObjectTab(0);
            }
            if (m_ObjEditTitle != null)
                m_ObjEditTitle.text = $"对象编辑：{name}（未放置，放置后自动绑定路径）";
            if (m_BtnPathDraw != null)
                m_BtnPathDraw.text = "开始绘制路径";
            // 动画字段填充默认值
            if (m_AnimNameField != null)
                m_AnimNameField.SetValueWithoutNotify("Idle");
            if (m_AnimLoopToggle != null)
                m_AnimLoopToggle.SetValueWithoutNotify(true);
            if (m_AnimSpeedField != null)
                m_AnimSpeedField.SetValueWithoutNotify("1.0");
            RefreshPathPointList();
        }

        /// <summary>
        /// 切换对象编辑切页
        /// </summary>
        /// <param name="index">0=可编辑路径 1=编辑动画</param>
        private void SetActiveObjectTab(int index)
        {
            if (m_TabEditPath != null)
                m_TabEditPath.RemoveFromClassList("active");
            if (m_TabEditAnim != null)
                m_TabEditAnim.RemoveFromClassList("active");
            if (index == 0)
            {
                m_TabEditPath?.AddToClassList("active");
                if (m_PathEditContent != null)
                    m_PathEditContent.style.display = DisplayStyle.Flex;
                if (m_AnimEditContent != null)
                    m_AnimEditContent.style.display = DisplayStyle.None;
            }
            else
            {
                m_TabEditAnim?.AddToClassList("active");
                if (m_PathEditContent != null)
                    m_PathEditContent.style.display = DisplayStyle.None;
                if (m_AnimEditContent != null)
                    m_AnimEditContent.style.display = DisplayStyle.Flex;
            }
        }

        /// <summary>
        /// 可编辑路径切页点击
        /// </summary>
        private void OnTabEditPathClicked()
        {
            SetActiveObjectTab(0);
        }

        /// <summary>
        /// 编辑动画切页点击
        /// </summary>
        private void OnTabEditAnimClicked()
        {
            SetActiveObjectTab(1);
        }

        /// <summary>
        /// 刷新编辑目标面板信息
        /// </summary>
        private void UpdateEditTargetUI()
        {
            if (m_ObjEditTitle != null)
            {
                m_ObjEditTitle.text = m_EditTarget == null
                    ? "对象编辑：未选择"
                    : $"对象编辑：{m_EditTarget.Name}（类型：{MapToolEditorUtils.FormatObjectTypeName(m_EditTarget.Type)}）· 路径点 {m_EditTarget.PathPoints.Count}";
            }
            RefreshPathPointList();
        }

        /// <summary>
        /// 刷新路径点列表
        /// </summary>
        private void RefreshPathPointList()
        {
            if (m_PathPointList == null) return;
            m_PathPointList.Clear();
            if (m_EditTarget == null)
                return;
            for (int i = 0; i < m_EditTarget.PathPoints.Count; i++)
            {
                Vector2 p = m_EditTarget.PathPoints[i];
                var item = new VisualElement();
                item.AddToClassList("path-point-item");
                if (i == m_SelectedPathPointIndex)
                    item.AddToClassList("selected");
                var idxLabel = new Label($"{(i + 1):D2}");
                idxLabel.AddToClassList("path-point-index");
                item.Add(idxLabel);
                var posLabel = new Label($"({p.x:F0}, {p.y:F0})");
                posLabel.AddToClassList("path-point-pos");
                item.Add(posLabel);
                // 单个路径点删除按钮
                var delBtn = new Button(() => RemovePathPointAt(i));
                delBtn.text = "×";
                delBtn.AddToClassList("path-point-del");
                item.Add(delBtn);
                // 点击路径项：高亮选中（场景中同步画大圆）
                int idx = i;
                item.RegisterCallback<ClickEvent>(evt =>
                {
                    m_SelectedPathPointIndex = idx;
                    RefreshPathPointList();
                });
                m_PathPointList.Add(item);
            }
        }

        /// <summary>
        /// 删除指定索引的路径点
        /// </summary>
        private void RemovePathPointAt(int index)
        {
            if (m_EditTarget == null || index < 0 || index >= m_EditTarget.PathPoints.Count) return;
            m_EditTarget.PathPoints.RemoveAt(index);
            if (m_SelectedPathPointIndex >= m_EditTarget.PathPoints.Count)
                m_SelectedPathPointIndex = m_EditTarget.PathPoints.Count - 1;
            RefreshPathPointList();
        }

        /// <summary>
        /// 开始/停止绘制路径
        /// </summary>
        private void OnPathDrawClicked()
        {
            if (m_EditTarget == null) return;
            m_IsPathDrawing = !m_IsPathDrawing;
            if (m_BtnPathDraw != null)
                m_BtnPathDraw.text = m_IsPathDrawing ? "停止绘制路径" : "开始绘制路径";
            Debug.Log($"{(m_IsPathDrawing ? "进入" : "退出")}路径绘制模式，目标: {m_EditTarget.Name}");
        }

        /// <summary>
        /// 撤销最后一点
        /// </summary>
        private void OnPathUndoClicked()
        {
            if (m_EditTarget == null || m_EditTarget.PathPoints.Count == 0) return;
            m_EditTarget.PathPoints.RemoveAt(m_EditTarget.PathPoints.Count - 1);
            RefreshPathPointList();
            Debug.Log($"撤销路径点，剩余: {m_EditTarget.PathPoints.Count}");
        }

        /// <summary>
        /// 清空路径
        /// </summary>
        private void OnPathClearClicked()
        {
            if (m_EditTarget == null) return;
            m_EditTarget.PathPoints.Clear();
            m_SelectedPathPointIndex = -1;
            RefreshPathPointList();
            Debug.Log($"清空 {m_EditTarget.Name} 的路径");
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        private void OnClearTargetClicked()
        {
            ClearEditTarget();
        }

    }
}
