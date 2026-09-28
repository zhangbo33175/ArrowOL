/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  FxDisplayIn.cs
 * author:  云毅
 * created:
 * descrip:   显形特效 - 角色从镜头/裁切位置"浮现"（Shader 裁剪显形）
 * 优化记录: 由旧版 HonorGraphics.FxDisplayIn 迁移，移除对旧版 Reflection 组件的依赖
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 显形特效
    /// 功能：将角色主 SkinnedMeshRenderer 切换为显形 Shader，通过 _Clip 值控制从指定 Z 值开始显示
    /// 用法：挂到特效对象，设置 clip 后启用即生效，销毁时自动还原
    /// </summary>
    public class FxDisplayIn : FxBase
    {
        #region 字段

        /// <summary>角色网格</summary>
        private SkinnedMeshRenderer m_Smr;

        /// <summary>原始 Shader（还原用）</summary>
        private Shader m_OriginShader;

        /// <summary>显形裁剪值（世界 Z 阈值）</summary>
        public float clip = 0f;

        #endregion

        #region 生命周期

        protected override void Start()
        {
            base.Start();
            InitState();
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化：替换角色 Shader 并设置裁剪值
        /// </summary>
        private void InitState()
        {
            m_Smr = GetComponentInChildren<SkinnedMeshRenderer>();
            if (m_Smr == null || m_Smr.material == null)
            {
                enabled = false;
                return;
            }

            m_OriginShader = m_Smr.material.shader;
            Shader displayShader = ShaderManager.Find("Honor/Role/Honor_Role_Display_In");
            if (displayShader == null)
            {
                enabled = false;
                return;
            }

            m_Smr.material.shader = displayShader;
            m_Smr.material.SetFloat(Shader.PropertyToID("_Clip"), clip);
        }

        #endregion

        #region 清理

        /// <summary>
        /// 清理：还原原始 Shader
        /// </summary>
        protected override void Clear()
        {
            if (m_Smr != null && m_Smr.material != null && m_OriginShader != null)
            {
                m_Smr.material.shader = m_OriginShader;
                m_Smr = null;
            }
        }

        /// <summary>
        /// 特效描述
        /// </summary>
        public override string GetDescription()
        {
            return "效果：超过指定Z值得模型显示出来";
        }

        #endregion
    }
}
