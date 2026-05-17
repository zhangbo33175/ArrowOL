/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextArea.cs
 * author:    云毅
 * created:   2026
 * descrip:   自定义自适应文本区域 | 基于UGUI Text扩展 | 支持最佳缩放 + 可见行数获取
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义文本区域组件 (继承UGUI Text)
    /// 核心功能：支持最佳文本自适应缩放，并对外提供可见行数属性
    /// 用于需要根据文本内容自动调整字体大小、并获取文本行数的场景
    /// </summary>
    public class AorTextArea : Text
    {
        //=========================================================================
        // 字段成员
        //=========================================================================
        #region Field - 缓存与只读
        /// <summary>
        /// 临时顶点数组，用于UI网格生成（优化，避免频繁GC）
        /// </summary>
        private readonly UIVertex[] _tmpVerts = new UIVertex[4];
        #endregion

        //=========================================================================
        // 属性成员
        //=========================================================================
        #region Property - 可见行数
        /// <summary>
        /// 当前文本可见行数（只读）
        /// </summary>
        public int VisibleLines { get; private set; }
        #endregion

        //=========================================================================
        // 私有方法 - 文本自适应逻辑
        //=========================================================================
        #region Method - 自适应缩放
        /// <summary>
        /// 执行文本自适应缩放逻辑
        /// 根据设置的最大/最小字号，自动调整到能完整显示所有文本的最大字号
        /// </summary>
        private void _UseFitSettings()
        {
            // 获取文本生成配置
            TextGenerationSettings settings = GetGenerationSettings(rectTransform.rect.size);
            // 关闭内置自适应，使用自定义逻辑
            settings.resizeTextForBestFit = false;

            // 未开启自适应，直接生成文本
            if (!resizeTextForBestFit)
            {
                cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
                return;
            }

            // 开启自适应：从最大字号向下遍历，找到能完整显示文本的最大字号
            int minSize = resizeTextMinSize;
            int textLength = text.Length;

            for (int fontSize = resizeTextMaxSize; fontSize >= minSize; --fontSize)
            {
                settings.fontSize = fontSize;
                // 生成文本并检查是否完整显示
                cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);

                // 所有字符都可见，停止缩放
                if (cachedTextGenerator.characterCountVisible == textLength)
                    break;
            }
        }
        #endregion

        //=========================================================================
        // 重写方法 - 网格生成
        //=========================================================================
        #region Method - 重写UGUI网格构建
        /// <summary>
        /// 重写UGUI网格生成方法
        /// 自定义文本渲染、自适应缩放、计算可见行数
        /// </summary>
        /// <param name="toFill">顶点辅助器，用于构建UI网格</param>
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            // 字体为空，直接返回
            if (font == null)
                return;

            // 禁用字体纹理重建回调，防止渲染冲突
            m_DisableFontTextureRebuiltCallback = true;

            // 执行文本自适应大小逻辑
            _UseFitSettings();

            // 获取文本生成的顶点数据
            IList<UIVertex> vertexList = cachedTextGenerator.verts;
            float unitsPerPixel = 1 / pixelsPerUnit;
            int vertexCount = vertexList.Count;

            // 无顶点数据，清空网格并返回
            if (vertexCount <= 0)
            {
                toFill.Clear();
                return;
            }

            // 计算像素对齐偏移量
            Vector2 roundingOffset = new Vector2(vertexList[0].position.x, vertexList[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();

            // 构建文本四边形顶点，处理像素对齐
            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < vertexCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    _tmpVerts[tempVertsIndex] = vertexList[i];
                    _tmpVerts[tempVertsIndex].position *= unitsPerPixel;
                    _tmpVerts[tempVertsIndex].position.x += roundingOffset.x;
                    _tmpVerts[tempVertsIndex].position.y += roundingOffset.y;

                    // 每4个顶点组成一个四边形，添加到网格
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(_tmpVerts);
                }
            }
            else
            {
                for (int i = 0; i < vertexCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    _tmpVerts[tempVertsIndex] = vertexList[i];
                    _tmpVerts[tempVertsIndex].position *= unitsPerPixel;

                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(_tmpVerts);
                }
            }

            // 恢复纹理回调
            m_DisableFontTextureRebuiltCallback = false;

            // 赋值当前可见的文本行数
            VisibleLines = cachedTextGenerator.lineCount;
        }
        #endregion
    }
}