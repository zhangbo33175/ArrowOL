/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PostEffectsBase.cs
 * author:  云毅
 * created:
 * descrip:   后处理基类 - 提供 Shader 校验、材质创建、平台支持检测等公共能力
 * 优化记录: 由旧版 HonorGraphics.PostEffectsBase 迁移（源自 Unity Image Effects），
 *           按框架规范精简，统一命名空间
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 后处理基类
    /// 功能：封装后处理特效的公共流程——Shader 查找/材质创建/平台能力检测/降级策略
    /// 说明：子类在 OnRenderImage 中调用 CheckResources 后执行 Blit
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class PostEffectsBase : MonoBehaviour
    {
        #region 字段

        /// <summary>是否支持 HDR（用于判定是否需要降级）</summary>
        protected bool supportHDRTextures = true;

        /// <summary>是否支持 DX11</summary>
        protected bool supportDX11 = false;

        /// <summary>是否支持 NPOT 纹理</summary>
        protected bool supportNPOT = true;

        /// <summary>是否需要创建 depth texture</summary>
        protected bool isSupported = true;

        #endregion

        #region 材质工具

        /// <summary>
        /// 创建材质（带缓存；Shader 为空或不可用返回 null）
        /// </summary>
        /// <param name="shader">目标 Shader</param>
        /// <param name="material">缓存的材质（非空且 Shader 一致时复用）</param>
        public Material CheckShaderAndCreateMaterial(Shader shader, Material material)
        {
            if (shader == null)
            {
                return null;
            }

            if (shader.isSupported == false)
            {
                return null;
            }

            if (material != null && material.shader == shader)
            {
                return material;
            }

            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            return material ? material : null;
        }

        /// <summary>
        /// 创建材质（不缓存版本）
        /// </summary>
        public Material CreateMaterial(Shader shader, Material material)
        {
            return CheckShaderAndCreateMaterial(shader, material);
        }

        /// <summary>
        /// 释放运行时材质
        /// </summary>
        public void DestroyMaterial(Material material)
        {
            if (material != null)
            {
                DestroyImmediate(material);
                material = null;
            }
        }

        #endregion

        #region 平台检测

        /// <summary>
        /// 平台能力检测（HDR/DX11/NPOT）
        /// </summary>
        /// <param name="needDepth">是否需要深度纹理支持</param>
        /// <param name="needHdr">是否需要 HDR 支持</param>
        /// <param name="needDX11">是否需要 DX11 支持</param>
        /// <returns>是否支持，不支持时自动禁用组件</returns>
        public bool CheckSupport(bool needDepth = false, bool needHdr = false, bool needDX11 = false)
        {
            isSupported = true;
            supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
            supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
            supportNPOT = SystemInfo.npotSupport != NPOTSupport.None;

            if (needDX11 && !supportDX11)
            {
                Log.Warning("PostEffectsBase 不支持 DX11，特效降级。");
                NotSupported();
                return false;
            }

            if (needHdr && !supportHDRTextures)
            {
                Log.Warning("PostEffectsBase 不支持 HDR 纹理，特效降级。");
                NotSupported();
                return false;
            }

            if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                Log.Warning("PostEffectsBase 不支持深度纹理，特效降级。");
                NotSupported();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 特效不受支持时的降级处理：禁用组件并标记
        /// </summary>
        protected void NotSupported()
        {
            enabled = false;
            isSupported = false;
        }

        #endregion

        #region 渲染

        /// <summary>
        /// 默认 Pass：直接拷贝
        /// </summary>
        protected void DefaultGraphicsBlit(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination);
        }

        #endregion
    }
}
