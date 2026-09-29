/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  FxDissolve.cs
 * author:  云毅
 * created:
 * descrip:   溶解特效 - 角色整体溶解出现/消失（可配置方向、颜色、遮罩量）
 * 优化记录: 由旧版 HonorGraphics.FxDissolve 迁移，修复 smr 引用 bug，统一命名空间
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 溶解特效
    /// 功能：将角色所有 SkinnedMeshRenderer 替换为溶解 Shader，并按进度推进溶解高度
    /// 字段：life 0→1 推进溶解进度；dir -1 向上溶解 / 1 向下溶解
    /// </summary>
    public class FxDissolve : MonoBehaviour
    {
        #region 字段

        /// <summary>溶解进度（0~1，由外部驱动）</summary>
        public float life = 0f;

        /// <summary>溶解方向：-1 向上，1 向下</summary>
        public float dir = -1f;

        /// <summary>溶解密度</summary>
        public float quantity = 50f;

        /// <summary>遮罩阈值 1</summary>
        [Range(-1f, 1f)]
        public float quantityMask1 = 0.25f;

        /// <summary>遮罩阈值 2</summary>
        [Range(0f, 1f)]
        public float quantityMask2 = 0.75f;

        /// <summary>主颜色 1</summary>
        public Color color1 = Color.white;

        /// <summary>主颜色 1 亮度</summary>
        [Range(0f, 5f)]
        public float bright1 = 1f;

        /// <summary>主颜色 2</summary>
        public Color color2 = Color.white;

        /// <summary>主颜色 2 亮度</summary>
        [Range(0f, 5f)]
        public float bright2 = 1f;

        /// <summary>边缘颜色</summary>
        public Color rimColor = Color.white;

        /// <summary>边缘亮度</summary>
        [Range(0f, 5f)]
        public float rimBright = 1f;

        /// <summary>边缘范围</summary>
        [Range(0f, 10f)]
        public float rimRange = 5f;

        /// <summary>当前溶解高度</summary>
        private float m_Height;

        private float m_HeightMin;
        private float m_HeightMax;
        private float m_HeightInit;
        private float m_HeightSize;

        /// <summary>角色网格渲染器</summary>
        private SkinnedMeshRenderer[] m_Smrs;

        /// <summary>原始材质组</summary>
        private Material[][] m_OriginMats;

        /// <summary>溶解材质</summary>
        private Material m_FxMat;

        /// <summary>角色对象（第一个子节点）</summary>
        private GameObject m_RoleObj;

        /// <summary>初始化次数（防重复销毁角色）</summary>
        public int initStateCount = 0;

        #endregion

        #region 生命周期

        /// <summary>
        /// 启用时初始化溶解材质与区间
        /// </summary>
        private void OnEnable()
        {
            InitState();
        }

        /// <summary>
        /// 禁用时还原材质并清理
        /// </summary>
        private void OnDisable()
        {
            Clear();
        }

        /// <summary>
        /// 每帧推进溶解高度并写入材质参数
        /// </summary>
        private void Update()
        {
            if (m_FxMat == null)
            {
                return;
            }

            // 溶解高度 = 起始高度 + 进度 * 方向 * 高度区间
            m_Height = m_HeightInit + life * dir * m_HeightSize;

            m_FxMat.SetFloat("_Dir", dir);
            m_FxMat.SetColor("_Color1", color1 * bright1);
            m_FxMat.SetColor("_Color2", color2 * bright2);
            m_FxMat.SetColor("_RimColor", rimColor * rimBright);
            m_FxMat.SetFloat("_RimRange", rimRange);
            m_FxMat.SetFloat("_Quantity", quantity);
            m_FxMat.SetFloat("_QuantityMask1", quantityMask1);
            m_FxMat.SetFloat("_QuantityMask2", quantityMask2);
            m_FxMat.SetFloat("_Height", m_Height);
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化：替换角色全部 SkinnedMeshRenderer 为溶解材质，并计算溶解区间
        /// </summary>
        private void InitState()
        {
            if (transform.childCount == 0)
            {
                return;
            }

            m_RoleObj = transform.GetChild(0).gameObject;
            m_RoleObj.SetActive(true);

            m_Smrs = GetComponentsInChildren<SkinnedMeshRenderer>();
            if (m_Smrs.Length == 0)
            {
                return;
            }

            m_FxMat = new Material(ShaderManager.Find("Honor/FX/FX_Dissolve"));

            // 记录原材质并整体替换
            m_OriginMats = new Material[m_Smrs.Length][];
            for (int i = 0; i < m_Smrs.Length; ++i)
            {
                m_OriginMats[i] = m_Smrs[i].sharedMaterials;
                Material[] tmpMats = new Material[m_OriginMats[i].Length];
                for (int j = 0; j < tmpMats.Length; ++j)
                {
                    tmpMats[j] = m_FxMat;
                }

                m_Smrs[i].sharedMaterials = tmpMats;
            }

            // 方向归一化
            dir = dir > 0 ? 1f : -1f;

            // 以 body_0 网格为溶解基准（修复原实现 smr = smrs 的引用 bug）
            SkinnedMeshRenderer baseSmr = m_Smrs[0];
            for (int i = 0; i < m_Smrs.Length; i++)
            {
                if (m_Smrs[i].transform.name == "body_0")
                {
                    baseSmr = m_Smrs[i];
                    break;
                }
            }

            m_HeightMin = baseSmr.bounds.min.y + (dir > 0 ? 0f : -1.5f);
            m_HeightMax = baseSmr.bounds.max.y + (dir > 0 ? 1.5f : 0f);
            m_HeightInit = dir > 0 ? m_HeightMin : m_HeightMax;
            m_HeightSize = m_HeightMax - m_HeightMin;

            initStateCount++;
        }

        #endregion

        #region 清理

        /// <summary>
        /// 清理：还原原始材质；主运行且仅初始化一次时销毁角色对象
        /// </summary>
        private void Clear()
        {
            if (m_Smrs != null)
            {
                for (int i = 0; i < m_Smrs.Length; ++i)
                {
                    if (m_Smrs[i] != null)
                    {
                        m_Smrs[i].sharedMaterials = m_OriginMats[i];
                    }
                }

                m_Smrs = null;
            }

            if (GraphicsUtils.IsMainRun && m_RoleObj != null && initStateCount == 1)
            {
                Destroy(m_RoleObj);
            }
        }

        #endregion
    }
}
