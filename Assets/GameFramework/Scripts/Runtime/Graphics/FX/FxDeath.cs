/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  FxDeath.cs
 * author:  云毅
 * created:
 * descrip:   死亡消散特效 - 角色溶解消散（支持正向/逆向、自定义角色对象）
 * 优化记录: 由旧版 HonorGraphics.FxDeath 迁移，移除对旧版 Game 命名空间依赖，
 *           修复渲染器分组逻辑，统一命名空间
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 死亡消散特效
    /// 功能：将角色渲染器替换为角色裁切/空材质，按 life 推进消散高度；
    ///       支持传入自定义角色对象（SetRole），支持从父级反查角色根
    /// 参数默认：1|50|1||0.5|1.0|1.0|1.0|1.0（方向|数量|范围|空|颜色RGB|亮度）
    /// </summary>
    public class FxDeath : FxBase
    {
        #region 字段

        /// <summary>消散进度（0~1）</summary>
        public float life = 0f;

        /// <summary>消散方向：-1 向上，1 向下</summary>
        public float dir = -1f;

        /// <summary>消散密度</summary>
        public float quantity = 50f;

        /// <summary>消散范围</summary>
        [Range(0f, 10f)]
        public float range = 1f;

        /// <summary>消散颜色</summary>
        public Color color = Color.white;

        /// <summary>颜色亮度</summary>
        [Range(0f, 5f)]
        public float bright = 1f;

        /// <summary>辉光强度</summary>
        [Range(0f, 10f)]
        public float gloss = 2f;

        /// <summary>是否显示（控制渲染器开关）</summary>
        public bool show = true;

        private bool m_LastShowState;

        private float m_Height;
        private float m_HeightMin;
        private float m_HeightMax;
        private float m_HeightInit;
        private float m_HeightSize;

        /// <summary>角色渲染器集合</summary>
        public Renderer[] smrs;

        /// <summary>需要开启的渲染器（使用角色裁切材质）</summary>
        public List<Renderer> enableRendererList = new List<Renderer>();

        /// <summary>需要关闭的渲染器</summary>
        public List<Renderer> disableRendererList = new List<Renderer>();

        /// <summary>原始材质组</summary>
        public Material[][] originMats;

        /// <summary>裁切材质（需要释放）</summary>
        public List<Material> fxMatList = new List<Material>();

        /// <summary>新材质（含空材质，需要释放）</summary>
        public List<Material> newMatList = new List<Material>();

        /// <summary>角色根节点（反查结果）</summary>
        public Transform roleRootTrans;

        /// <summary>外部设置的处理对象</summary>
        public GameObject setRoleObj;

        /// <summary>特效描述</summary>
        public const string Description =
            "效果: 死亡消散效果，正向和逆向\n" +
            "参数默认: 1|50|1||0.5|1.0|1.0|1.0|1.0\n" +
            "参数描述：消散方向|数量|范围|颜色R|颜色G|颜色B|颜色A|亮度";

        #endregion

        #region 外部设置

        /// <summary>
        /// 设置特效处理对象（游戏逻辑调用；会先清理旧状态再重新采集渲染器）
        /// </summary>
        /// <param name="obj">目标角色对象</param>
        public void SetRole(GameObject obj)
        {
            Clear();
            setRoleObj = obj;
        }

        #endregion

        #region 生命周期

        protected override void Start()
        {
            base.Start();
            InitState();
        }

        protected override void Update()
        {
            base.Update();

            if (smrs == null || smrs.Length == 0)
            {
                return;
            }

            // 显示状态切换：批量开关渲染器
            if (show != m_LastShowState)
            {
                m_LastShowState = show;
                SetRenderersActive(show);
            }

            // 推进消散高度
            m_Height = m_HeightInit + life * dir * m_HeightSize;
            for (int i = 0; i < fxMatList.Count; i++)
            {
                Material mat = fxMatList[i];
                if (mat == null)
                {
                    continue;
                }

                mat.SetFloat("_Dir", dir);
                mat.SetFloat("_Quantity", quantity);
                mat.SetFloat("_Range", range);
                mat.SetColor("_Color", color * bright);
                mat.SetFloat("_Gloss", gloss);
                mat.SetFloat("_Height", m_Height);
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 采集角色渲染器：优先外部对象 → 子节点 → 父级反查角色根
        /// </summary>
        private void InitState()
        {
            if (setRoleObj != null)
            {
                if (!setRoleObj.activeSelf)
                {
                    setRoleObj.SetActive(true);
                }

                smrs = setRoleObj.GetComponentsInChildren<Renderer>();
            }
            else if (transform.childCount == 0)
            {
                if (roleRootTrans == null)
                {
                    roleRootTrans = GraphicsUtils.GetRoleRootTransform(transform, LayerMask.GetMask(DefLayer.Role));
                    if (roleRootTrans == null)
                    {
                        return;
                    }
                }

                smrs = roleRootTrans.gameObject.GetComponentsInChildren<Renderer>();
            }
            else
            {
                smrs = GetComponentsInChildren<Renderer>();
            }

            if (smrs.Length == 0)
            {
                return;
            }

            enableRendererList.Clear();
            disableRendererList.Clear();
            originMats = new Material[smrs.Length][];

            for (int i = 0; i < smrs.Length; ++i)
            {
                originMats[i] = smrs[i].sharedMaterials;
                Material[] tmpMats = new Material[originMats[i].Length];

                for (int j = 0; j < originMats[i].Length; ++j)
                {
                    Material originMat = originMats[i][j];
                    if (originMat == null)
                    {
                        tmpMats[j] = null;
                        continue;
                    }

                    if (originMat.HasProperty("_RoleClip"))
                    {
                        // 支持角色裁切：复制材质并开启裁切关键字
                        Material clipMat = new Material(originMat);
                        clipMat.EnableKeyword("_ROLECLIP_ON");
                        tmpMats[j] = clipMat;
                        fxMatList.Add(clipMat);
                        newMatList.Add(clipMat);
                        if (!enableRendererList.Contains(smrs[i]))
                        {
                            enableRendererList.Add(smrs[i]);
                        }
                    }
                    else if (originMat.shader != null && originMat.shader.name == "Honor/FX/FX_Shield")
                    {
                        // 护盾材质替换为空材质
                        Material emptyMat = new Material(ShaderManager.Find("Honor/Model/Empty"));
                        tmpMats[j] = emptyMat;
                        newMatList.Add(emptyMat);
                    }
                    else
                    {
                        // 其余材质尝试匹配 _Clip 变体
                        string clipName = originMat.shader != null ? originMat.shader.name + "_Clip" : null;
                        Shader clipShader = clipName != null ? ShaderManager.Find(clipName) : null;
                        if (clipShader != null)
                        {
                            Material clipMat = new Material(clipShader);
                            clipMat.CopyPropertiesFromMaterial(originMat);
                            tmpMats[j] = clipMat;
                            fxMatList.Add(clipMat);
                            newMatList.Add(clipMat);
                            if (!enableRendererList.Contains(smrs[i]))
                            {
                                enableRendererList.Add(smrs[i]);
                            }
                        }
                        else
                        {
                            // 无裁切变体：直接关闭该渲染器
                            tmpMats[j] = originMat;
                            if (!disableRendererList.Contains(smrs[i]))
                            {
                                disableRendererList.Add(smrs[i]);
                            }
                        }
                    }
                }

                smrs[i].sharedMaterials = tmpMats;
            }

            // 应用初始显示状态
            m_LastShowState = show;
            SetRenderersActive(show);
        }

        /// <summary>批量开关渲染器</summary>
        private void SetRenderersActive(bool active)
        {
            if (enableRendererList != null)
            {
                for (int i = 0; i < enableRendererList.Count; i++)
                {
                    if (enableRendererList[i] != null)
                    {
                        enableRendererList[i].enabled = active;
                    }
                }
            }

            if (disableRendererList != null)
            {
                for (int i = 0; i < disableRendererList.Count; i++)
                {
                    if (disableRendererList[i] != null)
                    {
                        disableRendererList[i].enabled = !active;
                    }
                }
            }
        }

        #endregion

        #region 清理

        /// <summary>
        /// 清理：还原原始材质并释放运行时材质
        /// </summary>
        protected override void Clear()
        {
            if (smrs != null)
            {
                for (int i = 0; i < smrs.Length; ++i)
                {
                    if (smrs[i] != null && originMats != null && i < originMats.Length)
                    {
                        smrs[i].sharedMaterials = originMats[i];
                    }
                }

                smrs = null;
            }

            for (int i = 0; i < newMatList.Count; i++)
            {
                if (newMatList[i] != null)
                {
                    Destroy(newMatList[i]);
                }
            }

            fxMatList.Clear();
            newMatList.Clear();
            enableRendererList.Clear();
            disableRendererList.Clear();
        }

        /// <summary>
        /// 特效描述
        /// </summary>
        public override string GetDescription()
        {
            return Description;
        }

        #endregion
    }
}
