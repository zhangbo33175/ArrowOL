/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  FxBase.cs
 * author:  云毅
 * created:
 * descrip:   特效基类 - 统一特效组件生命周期（计时销毁/参数初始化/资源清理）
 * 优化记录: 由旧版 HonorGraphics.FxBase 迁移，统一命名空间
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 特效基类
    /// 功能：提供特效的通用生命周期——计时自动销毁、参数驱动初始化、销毁时资源清理
    /// 说明：所有挂载在特效预制体上的 Fx 组件都应继承本类
    /// </summary>
    public abstract class FxBase : MonoBehaviour
    {
        #region 字段

        /// <summary>存活时间（秒），大于 0 时倒计时结束后自动销毁组件</summary>
        protected float time = 0f;

        #endregion

        #region 生命周期

        /// <summary>
        /// 初始化入口（子类重写）
        /// </summary>
        protected virtual void Start()
        {
        }

        /// <summary>
        /// 每帧更新（子类重写时记得调用 base.Update）
        /// </summary>
        protected virtual void Update()
        {
            if (time > 0f)
            {
                time -= Time.deltaTime;
                if (time < 0f)
                {
                    Destroy(this);
                }
            }
        }

        /// <summary>
        /// 销毁时自动清理（子类重写 Clear 完成具体资源回收）
        /// </summary>
        protected void OnDestroy()
        {
            Clear();
        }

        #endregion

        #region 子类接口

        /// <summary>
        /// 按参数字符串初始化特效（形如 "a|b|c"，各特效自行解析）
        /// </summary>
        /// <param name="param">参数串</param>
        public virtual void Init(string param)
        {
        }

        /// <summary>
        /// 清理特效占用的运行时资源
        /// </summary>
        protected virtual void Clear()
        {
        }

        /// <summary>
        /// 特效功能描述（编辑器展示用）
        /// </summary>
        public virtual string GetDescription()
        {
            return "特效描述";
        }

        #endregion
    }
}
