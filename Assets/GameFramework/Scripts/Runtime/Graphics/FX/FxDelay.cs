/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  FxDelay.cs
 * author:  云毅
 * created:
 * descrip:   延迟残影特效 - 从自身分出 N 个影子节点按帧延迟跟随动画（残影/分身表现）
 * 优化记录: 由旧版 HonorGraphics.FxDelay 迁移，修正字符串空判断，统一命名空间
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 延迟残影特效
    /// 功能：克隆 N 个角色副本，按帧数延迟回放骨骼动画并统一替换材质，形成残影拖尾
    /// 参数格式：shaderName|克隆数量|延迟帧数|颜色R|颜色G|颜色B|颜色A
    /// </summary>
    public class FxDelay : FxBase
    {
        #region 内部数据

        /// <summary>残影节点数据</summary>
        public class DelayData
        {
            /// <summary>克隆节点</summary>
            public GameObject Obj;

            /// <summary>延迟帧数</summary>
            public int Frame;

            /// <summary>克隆动画器</summary>
            public Animator Anim;
        }

        #endregion

        #region 字段

        /// <summary>残影列表</summary>
        public List<DelayData> delayDatas = new List<DelayData>();

        /// <summary>位置帧缓存</summary>
        private List<Vector3> m_FramePos = new List<Vector3>();

        /// <summary>动画状态哈希缓存</summary>
        private List<int> m_ShortNames = new List<int>();

        /// <summary>残影 Shader 名</summary>
        public string shaderName;

        /// <summary>残影 Shader</summary>
        public Shader shader;

        /// <summary>残影材质</summary>
        public Material mat;

        /// <summary>克隆数量</summary>
        [Range(0, 10)]
        public int cloneCount = 1;

        /// <summary>延迟帧数</summary>
        [Range(0, 30)]
        public int frame = 3;

        /// <summary>残影颜色</summary>
        public Color color = Color.white;

        /// <summary>位置偏移</summary>
        public Vector3 posOffset = Vector3.zero;

        /// <summary>克隆缩放</summary>
        public Vector3 scale = Vector3.one;

        /// <summary>当前帧计数</summary>
        private int m_CurFrame;

        /// <summary>角色根节点</summary>
        private Transform m_RootTrans;

        /// <summary>角色动画器</summary>
        private Animator m_Anim;

        /// <summary>特效描述</summary>
        public const string Description =
            "效果：从自身分出N个影子延迟跟随\n" +
            "参数默认: Honor/FX/FX_Alpha_Ext|1|3|1.0|1.0|1.0|1.0\n" +
            "参数描述：shaderName|数量|延迟(帧)|颜色R|颜色G|颜色B|颜色A";

        #endregion

        #region 生命周期

        protected override void Start()
        {
            base.Start();
            InitState();
        }

        protected override void Update()
        {
            if (mat == null)
            {
                InitState();
                return;
            }

            // 记录当前帧位置与动画状态
            m_CurFrame++;
            m_FramePos.Add(m_RootTrans.position);
            m_ShortNames.Add(m_Anim.GetCurrentAnimatorStateInfo(0).shortNameHash);

            // 超出缓存窗口移除最旧帧
            int maxCache = cloneCount * frame;
            if (m_FramePos.Count > maxCache)
            {
                m_FramePos.RemoveAt(0);
                m_ShortNames.RemoveAt(0);
            }

            // 各残影回放到对应延迟帧
            for (int i = 0; i < cloneCount; i++)
            {
                DelayData data = delayDatas[i];
                int index = i * frame;
                if (m_FramePos.Count > index)
                {
                    data.Obj.transform.position = m_FramePos[index] + posOffset;
                    data.Anim.Play(m_ShortNames[index]);
                }
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化残影状态：查找角色根、克隆节点、替换残影材质
        /// </summary>
        private void InitState()
        {
            if (m_RootTrans == null)
            {
                m_RootTrans = transform.Find("root");
                if (m_RootTrans == null)
                {
                    enabled = false;
                    return;
                }
            }

            if (shader == null)
            {
                if (string.IsNullOrEmpty(shaderName))
                {
                    enabled = false;
                    return;
                }

                shader = ShaderManager.Find(shaderName);
                if (shader == null)
                {
                    enabled = false;
                    return;
                }
            }

            if (mat == null)
            {
                mat = new Material(shader);
            }

            m_Anim = m_RootTrans.GetComponent<Animator>();

            // 清空旧残影
            delayDatas.Clear();
            for (int i = 0; i < delayDatas.Count; i++)
            {
                if (delayDatas[i].Obj != null)
                {
                    Destroy(delayDatas[i].Obj);
                }
            }

            // 创建克隆残影
            for (int i = 0; i < cloneCount; i++)
            {
                DelayData data = new DelayData();
                delayDatas.Add(data);

                GameObject obj = Instantiate(m_RootTrans.gameObject, transform.position, transform.rotation);
                obj.transform.SetParent(transform);
                obj.transform.localScale = scale;
                obj.name = "FxDelay" + i;

                data.Obj = obj;
                data.Anim = obj.GetComponent<Animator>();
                data.Frame = frame * (i + 1);

                HideEffectView(obj);
                SetFxMaterial(obj.GetComponentsInChildren<Renderer>());
            }

            m_FramePos.Clear();
            m_ShortNames.Clear();
            m_CurFrame = 0;
        }

        /// <summary>
        /// 隐藏克隆节点中的特效子物体（避免残影带上粒子）
        /// </summary>
        private void HideEffectView(GameObject go)
        {
            Transform[] children = go.GetComponentsInChildren<Transform>();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name.Contains("EffectView"))
                {
                    children[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 将克隆节点所有渲染器替换为残影材质
        /// </summary>
        private void SetFxMaterial(Renderer[] renderers)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer r = renderers[i];
                Material[] fxMats = new Material[r.sharedMaterials.Length];
                for (int j = 0; j < fxMats.Length; j++)
                {
                    fxMats[j] = mat;
                    mat.SetColor("_TintColor", color);
                }

                r.sharedMaterials = fxMats;
            }
        }

        #endregion

        #region 参数与清理

        /// <summary>
        /// 解析初始化参数
        /// </summary>
        public override void Init(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return;
            }

            string[] paramArray = param.Split('|');
            int index = 0;

            shaderName = paramArray[index++];
            cloneCount = int.Parse(paramArray[index++]);
            frame = int.Parse(paramArray[index++]);
            color.r = float.Parse(paramArray[index++]);
            color.g = float.Parse(paramArray[index++]);
            color.b = float.Parse(paramArray[index++]);
            color.a = float.Parse(paramArray[index++]);
        }

        /// <summary>
        /// 清理残影资源
        /// </summary>
        protected override void Clear()
        {
            if (mat != null)
            {
                Destroy(mat);
                mat = null;
            }

            for (int i = 0; i < delayDatas.Count; i++)
            {
                if (delayDatas[i].Obj != null)
                {
                    Destroy(delayDatas[i].Obj);
                }
            }

            delayDatas.Clear();
            m_FramePos.Clear();
            m_ShortNames.Clear();
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
