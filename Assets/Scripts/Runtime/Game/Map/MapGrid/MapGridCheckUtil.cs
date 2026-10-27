/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapGridCheckUtil.cs
 * author:    云毅
 * created:   2026
 * descrip:   运行时网格校验探针
 *            对照编辑器坐标与运行时世界坐标，验证网格换算一致性
 ***************************************************************/

using UnityEngine;
using XLua;

namespace GameLib
{
    /// <summary>
    /// 运行时网格校验探针（移植自 MapGridCheckUtil.lua）
    /// 用于开发期诊断：编辑器格坐标 vs 运行时世界坐标 vs 保存格坐标三方对照
    /// </summary>
    [LuaCallCSharp]
    public static class MapGridCheckUtil
    {
        #region 配置

        /// <summary>校验总开关（生产环境关闭）</summary>
        public static bool MAP_GRID_VALIDATE_ENABLED = true;

        #endregion

        #region 校验方法

        /// <summary>
        /// 单实体对照诊断
        /// 计算编辑器格坐标与运行时世界格坐标，与保存格坐标三方比对并输出日志
        /// </summary>
        /// <param name="tag">实体标签（日志前缀）</param>
        /// <param name="editorX">编辑器X坐标（像素）</param>
        /// <param name="editorY">编辑器Y坐标（像素）</param>
        /// <param name="saveGridX">保存的网格X坐标</param>
        /// <param name="saveGridY">保存的网格Y坐标</param>
        /// <param name="worldX">运行时世界X坐标（像素）</param>
        /// <param name="worldY">运行时世界Y坐标（像素）</param>
        /// <param name="gridSize">单格像素尺寸</param>
        public static void ValidateEntity(string tag, float editorX, float editorY,
            float saveGridX, float saveGridY, float worldX, float worldY, int gridSize)
        {
            if (!MAP_GRID_VALIDATE_ENABLED) return;

            int expectedX = Mathf.FloorToInt(editorX / gridSize + 0.0001f);
            int expectedY = Mathf.FloorToInt(editorY / gridSize + 0.0001f);
            int actualX = Mathf.FloorToInt(worldX / gridSize + 0.0001f);
            int actualY = Mathf.FloorToInt(worldY / gridSize + 0.0001f);

            Debug.Log($"[网格校验] {tag} 保存格({saveGridX},{saveGridY}) " +
                      $"期望({expectedX},{expectedY}) 实际({actualX},{actualY})");
        }

        #endregion
    }
}
