/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrder.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 订单类型枚举与订单节点
 *            一个 BuildOrder 对应一个打包步骤，可挂子订单构成树形流水线；
 *            启用状态持久化到 EditorPrefs，窗口/CI 可勾选。
 *            本框架层去除了对任何打包窗口类的依赖。
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打包订单类型枚举（流水线步骤编号）
    /// 说明：顺序冻结，并行任务按此枚举注册对应的 BuildOrder 实现类，
    ///       严禁增删或调整顺序。
    /// </summary>
    public enum OrderType
    {
        /// <summary>无（占位）</summary>
        None = 0,
        /// <summary>起始订单（保底初始化，所有流水线首步）</summary>
        Start,
        /// <summary>单独打包资源 AB</summary>
        PackageRes,
        /// <summary>单独打包 Lua</summary>
        PackageLua,
        /// <summary>打包全部资源 AB</summary>
        PackageAll,
        /// <summary>打包 Android 安装包</summary>
        PackageAPK,
        /// <summary>打包 PC 客户端</summary>
        PackagePC,
        /// <summary>打包 iOS 工程</summary>
        PackageIOS,
    }

    /// <summary>
    /// 打包订单（流水线节点）
    /// 说明：持有执行动作 <see cref="action"/> 与子订单 <see cref="childOrders"/>，
    ///       运行时由 <see cref="BuildOrderHandler"/> 按流水线顺序实例化并递归执行。
    /// </summary>
    public class BuildOrder
    {
        #region 字段
        /// <summary>订单类型（对应 BuildOrderHandler._orderTypeDic 注册的实现类）</summary>
        public OrderType orderType = OrderType.None;

        /// <summary>订单名称（EditorPrefs key 的一部分，需在同批订单内唯一）</summary>
        public string Name = "";

        /// <summary>进度条提示文案（为空则不单独占用进度步、不弹进度条）</summary>
        public string tips = "";

        /// <summary>订单执行动作（为 null 时仅作为容器节点，不执行实际打包）</summary>
        public Action<BuildContext> action = null;

        /// <summary>子订单列表（递归执行，随父订单启用状态联动）</summary>
        public List<BuildOrder> childOrders = new List<BuildOrder>();

        /// <summary>强制选中（恒为启用、不可取消；set 时也强制写 true）</summary>
        public bool forceSelect = false;
        #endregion

        #region 进度基准
        /// <summary>
        /// 本次打包开始时刻（秒，EditorApplication.timeSinceStartup 基准）
        /// 说明：由 BuildOrderHandler 在每次构建开始时重置，供进度条计算“本次打包已耗时”。
        /// </summary>
        public static float BuildStartTimeSec = 0f;
        #endregion

        #region 启用状态（EditorPrefs 持久化）
        /// <summary>
        /// 是否启用该订单
        /// get: forceSelect 优先返回 true，否则读 EditorPrefs
        /// set: forceSelect 恒写 true；否则同步递归子订单后写 EditorPrefs
        /// </summary>
        public bool bEnable
        {
            get
            {
                if (forceSelect)
                {
                    return true;
                }

                return EditorPrefs.GetBool("honor_build_order_" + Name, false);
            }
            set
            {
                bool b = value;
                if (forceSelect)
                {
                    b = true;
                }

                // 同步子订单（子订单跟随父订单启用状态）
                foreach (BuildOrder v in childOrders)
                {
                    v.bEnable = b;
                }

                EditorPrefs.SetBool("honor_build_order_" + Name, b);
            }
        }
        #endregion

        #region 执行
        /// <summary>
        /// 执行该订单及其全部子订单
        /// </summary>
        /// <param name="progress">当前已展示进度（引用累计，tips 非空时自增）</param>
        /// <param name="totalProgress">总进度步数</param>
        /// <param name="context">打包上下文</param>
        /// <param name="force">强制执行（忽略 bEnable，CI/整包流程使用）</param>
        /// <returns>是否实际执行过动作</returns>
        public bool Run(ref int progress, int totalProgress, BuildContext context, bool force = false)
        {
            bool bRun = false;

            if ((force || bEnable) && action != null)
            {
                if (string.IsNullOrEmpty(tips) == false)
                {
                    progress++;
                }

                ShowProgress(progress, totalProgress);
                action(context);
                bRun = true;
            }

            // 递归执行子订单
            foreach (BuildOrder v in childOrders)
            {
                bRun = v.Run(ref progress, totalProgress, context, force) || bRun;
            }

            return bRun;
        }

        /// <summary>
        /// 显示进度条（tips 为空则不显示，避免无文案节点抢占进度条）
        /// </summary>
        /// <param name="cur">当前进度步</param>
        /// <param name="total">总进度步</param>
        public void ShowProgress(int cur, int total)
        {
            if (string.IsNullOrEmpty(tips))
            {
                return;
            }

            // 耗时基于 EditorApplication.timeSinceStartup，减去本次打包开始时刻
            float elapsed = (float)EditorApplication.timeSinceStartup - BuildStartTimeSec;
            string str = string.Format("{0:F2}s", elapsed);
            float ratio = total > 0 ? (cur + 0.0f) / total : 0f;
            EditorUtility.DisplayProgressBar("打包进度", tips + "(" + str + ")", ratio);
        }

        /// <summary>
        /// 统计将展示进度的步骤数（tips 非空计数 + 递归子订单）
        /// 说明：用于在真正执行前预算 totalProgress，保证进度条比例正确。
        /// </summary>
        /// <param name="force">强制统计（忽略 bEnable）</param>
        public int ProgressCount(bool force = false)
        {
            int count = 0;

            if ((force || bEnable) && action != null)
            {
                if (string.IsNullOrEmpty(tips) == false)
                {
                    count += 1;
                }
            }

            foreach (BuildOrder v in childOrders)
            {
                count += v.ProgressCount(force);
            }

            return count;
        }
        #endregion
    }
}
