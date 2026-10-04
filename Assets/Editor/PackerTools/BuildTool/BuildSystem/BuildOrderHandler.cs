/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrderHandler.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 订单处理器（单例）
 *            维护各平台/各阶段的 OrderType 流水线，注册 OrderType→实现类映射，
 *            按顺序实例化 BuildOrder 并递归执行，驱动进度条与日志；
 *            单步异常即终止后续步骤并收起进度条。
 *            资源/玩家打包的具体 BuildOrder 实现由并行任务提供。
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Honor.Editor.BuildPackage;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打包流水线处理器（单例）
    /// 说明：窗口 / 命令行均通过 <see cref="Inst"/> 调用公开入口发起打包。
    /// </summary>
    public class BuildOrderHandler
    {
        #region 单例
        private static readonly BuildOrderHandler _inst = new BuildOrderHandler();

        /// <summary>单例实例</summary>
        public static BuildOrderHandler Inst => _inst;
        #endregion

        #region 流水线定义
        /// <summary>打 AB（全部资源）流水线：Start → PackageAll</summary>
        private readonly OrderType[] _abPipeline = { OrderType.Start, OrderType.PackageAll };

        /// <summary>单独打资源 AB 流水线：Start → PackageRes</summary>
        private readonly OrderType[] _abResPipeline = { OrderType.Start, OrderType.PackageRes };

        /// <summary>单独打 Lua 流水线：Start → PackageLua</summary>
        private readonly OrderType[] _abLuaPipeline = { OrderType.Start, OrderType.PackageLua };

        /// <summary>打 APK 流水线：Start → PackageAll → PackageAPK</summary>
        private readonly OrderType[] _apkPipeline = { OrderType.Start, OrderType.PackageAll, OrderType.PackageAPK };

        /// <summary>打 PC 流水线：Start → PackageAll → PackagePC</summary>
        private readonly OrderType[] _pcPipeline = { OrderType.Start, OrderType.PackageAll, OrderType.PackagePC };

        /// <summary>打 iOS 流水线：Start → PackageAll → PackageIOS</summary>
        private readonly OrderType[] _iosPipeline = { OrderType.Start, OrderType.PackageAll, OrderType.PackageIOS };
        #endregion

        #region 类型映射
        /// <summary>
        /// OrderType → BuildOrder 实现类 映射
        /// 说明：BuildOrderStart 由本任务提供；其余实现类由并行任务提供，
        ///       命名空间同为 Honor.Editor.BuildTool，最终一起编译。
        /// </summary>
        private readonly Dictionary<OrderType, Type> _orderTypeDic = new Dictionary<OrderType, Type>();

        /// <summary>私有构造：注册全部 OrderType 映射</summary>
        private BuildOrderHandler()
        {
            _orderTypeDic[OrderType.Start] = typeof(BuildOrderStart);
            _orderTypeDic[OrderType.PackageRes] = typeof(BuildOrderPackageRes);
            _orderTypeDic[OrderType.PackageLua] = typeof(BuildOrderPackageLua);
            _orderTypeDic[OrderType.PackageAll] = typeof(BuildOrderPackageAll);
            _orderTypeDic[OrderType.PackageAPK] = typeof(BuildOrderPackageAPK);
            _orderTypeDic[OrderType.PackagePC] = typeof(BuildOrderPackagePC);
            _orderTypeDic[OrderType.PackageIOS] = typeof(BuildOrderPackageIOS);
        }
        #endregion

        #region 上下文
        /// <summary>一次打包流程共享的上下文</summary>
        private readonly BuildContext _buildContext = new BuildContext();
        #endregion

        #region 创建与执行
        /// <summary>
        /// 按 OrderType 反射创建对应的 BuildOrder 实例
        /// </summary>
        private BuildOrder CreateBuildOrder(OrderType orderType)
        {
            if (!_orderTypeDic.TryGetValue(orderType, out Type type) || type == null)
            {
                Debug.LogError($"[BuildOrderHandler] 未注册的 OrderType: {orderType}");
                return null;
            }

            BuildOrder order = null;
            try
            {
                order = Activator.CreateInstance(type) as BuildOrder;
            }
            catch (Exception e)
            {
                Debug.LogError($"[BuildOrderHandler] 实例化 {type} 失败: {e}");
            }

            return order;
        }

        /// <summary>
        /// 把 OrderType 数组转成 BuildOrder 实例列表（跳过创建失败项）
        /// </summary>
        private List<BuildOrder> BuildOrders(OrderType[] orders)
        {
            List<BuildOrder> list = new List<BuildOrder>();
            for (int i = 0; i < orders.Length; i++)
            {
                BuildOrder bo = CreateBuildOrder(orders[i]);
                if (bo != null)
                {
                    list.Add(bo);
                }
            }

            return list;
        }

        /// <summary>
        /// 顺序执行订单列表：预算总进度 → 逐序 Run → 每步完成打印名称+耗时；
        /// 单步异常即 LogError、收起进度条并停止后续步骤。
        /// </summary>
        /// <param name="handSelect">是否窗口手选（true=按勾选，false=整包强制）</param>
        /// <param name="buildOrders">待执行订单列表</param>
        /// <param name="context">打包上下文</param>
        private void RunBO(bool handSelect, List<BuildOrder> buildOrders, BuildContext context)
        {
            int currentProgress = 0;
            int totalProgress = 0;
            foreach (BuildOrder v in buildOrders)
            {
                totalProgress += v.ProgressCount(!handSelect);
            }

            // 重置进度条耗时基准
            BuildOrder.BuildStartTimeSec = (float)EditorApplication.timeSinceStartup;

            foreach (BuildOrder v in buildOrders)
            {
                float stepStart = (float)EditorApplication.timeSinceStartup;
                try
                {
                    v.Run(ref currentProgress, totalProgress, context, !handSelect);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[BuildOrderHandler] 订单 '{v.Name}' 执行异常，终止后续步骤: {e}");
                    EditorUtility.ClearProgressBar();
                    return;
                }

                float stepCost = (float)EditorApplication.timeSinceStartup - stepStart;
                BuildLog.Log($"{v.Name} 完成，耗时 {stepCost:F2}s");
            }

            EditorUtility.ClearProgressBar();
        }
        #endregion

        #region 构建入口
        /// <summary>
        /// 构建入口：保存设置 → 清空上下文 → 装配平台上下文 → 跑流水线
        /// </summary>
        /// <param name="target">目标构建平台</param>
        /// <param name="pipeline">该次构建的订单流水线</param>
        /// <param name="handSelect">
        /// 是否窗口手选模式：true=仅执行勾选(bEnable)的步骤；false=整包全量强制执行
        /// </param>
        private void BuildBegin(BuildTarget target, OrderType[] pipeline, bool handSelect = false)
        {
            // 1. 保底持久化当前设置
            BuildSettingsStore.Save();

            // 2. 清空上次残留上下文
            _buildContext.ClearAllContext();

            // 3. 装配平台上下文（平台名/输出根由 BuildPackage_Define 按目标平台解析）
            string abOutputRoot = BuildPackage_Define.GetBuildOutputFullRoot(target);
            BuildPlatformContext platformCtx = new BuildPlatformContext
            {
                Target = target,
                PlatformName = BuildPackage_Define.GetPlatformName(target),
                ABOutputRoot = abOutputRoot,
                ResPathTabPath = abOutputRoot + "/" + BuildPackage_Define.ResMappingFileName,
                PlayerOutPath = BuildSettingsStore.OutRoot,
                AppVersion = BuildSettingsStore.AppVersion,
                BundleVersionCode = BuildSettingsStore.BundleVersionCode,
                DevelopmentBuild = BuildSettingsStore.DevelopmentBuild,
                AppName = BuildSettingsStore.AppName,
                PackageName = BuildSettingsStore.PackageName,
                Scenes = ParseScenes(BuildSettingsStore.Scenes),
                LuaEncrypt = BuildSettingsStore.LuaEncrypt,
                Incremental = BuildSettingsStore.Incremental,
                KeystorePath = BuildSettingsStore.KeystorePath,
                KeystorePass = BuildSettingsStore.KeystorePass,
                KeyAliasName = BuildSettingsStore.KeyAliasName,
                KeyAliasPass = BuildSettingsStore.KeyAliasPass,
            };
            _buildContext.AddContextObject(platformCtx);

            // 4. 实例化并执行流水线（手选模式透传给 RunBO）
            List<BuildOrder> orders = BuildOrders(pipeline);
            RunBO(handSelect, orders, _buildContext);
        }
        #endregion

        #region 公开入口
        /// <summary>打 AB（全部资源），跟随当前编辑器设置平台</summary>
        public void BuildAB()
        {
            BuildBegin(BuildSettingsStore.CurrentPlatform, _abPipeline);
        }

        /// <summary>单独打资源 AB，跟随当前编辑器设置平台</summary>
        public void BuildRes()
        {
            BuildBegin(BuildSettingsStore.CurrentPlatform, _abResPipeline);
        }

        /// <summary>单独打 Lua，跟随当前编辑器设置平台</summary>
        public void BuildLua()
        {
            BuildBegin(BuildSettingsStore.CurrentPlatform, _abLuaPipeline);
        }

        /// <summary>打 Android APK（强制 Android 平台）</summary>
        public void BuildAPK()
        {
            BuildBegin(BuildTarget.Android, _apkPipeline);
        }

        /// <summary>打 PC 客户端（StandaloneWindows64）</summary>
        public void BuildPC()
        {
            BuildBegin(BuildTarget.StandaloneWindows64, _pcPipeline);
        }

        /// <summary>打 iOS 工程</summary>
        public void BuildIOS()
        {
            BuildBegin(BuildTarget.iOS, _iosPipeline);
        }

        /// <summary>
        /// 依次打全部平台 AB：Android → iOS → PC
        /// 说明：PC 与 Android 的 AB 输出目录同名会互相覆盖，按此顺序执行即可。
        /// </summary>
        public void BuildAllABs()
        {
            BuildBegin(BuildTarget.Android, _abPipeline);
            BuildLog.Log("Android AB 打包完成");

            BuildBegin(BuildTarget.iOS, _abPipeline);
            BuildLog.Log("iOS AB 打包完成");

            BuildBegin(BuildTarget.StandaloneWindows64, _abPipeline);
            BuildLog.Log("PC(StandaloneWindows64) AB 打包完成");
        }

        /// <summary>
        /// 全平台打包：Android(APK) → iOS(iOS) → PC(PC)
        /// 说明：各平台流水线已含 AB 打包（PackageAll），无需再单独先跑 _abPipeline，
        ///       避免 AB 重复打包。按 Android → iOS → PC 顺序执行。
        /// </summary>
        public void BuildAllPlatforms()
        {
            // Android: _apkPipeline 已含 Start + PackageAll + PackageAPK
            BuildBegin(BuildTarget.Android, _apkPipeline);

            // iOS: _iosPipeline 已含 Start + PackageAll + PackageIOS
            BuildBegin(BuildTarget.iOS, _iosPipeline);

            // PC: _pcPipeline 已含 Start + PackageAll + PackagePC
            BuildBegin(BuildTarget.StandaloneWindows64, _pcPipeline);
        }

        /// <summary>
        /// 自定义打包：按窗口勾选的步骤顺序执行，跟随当前编辑器设置平台
        /// 说明：以手选模式(handSelect=true)运行，仅执行 bEnable=true 的勾选步骤；
        ///       Start 订单因 forceSelect=true 恒先执行。
        /// </summary>
        /// <param name="orders">勾选的订单类型列表</param>
        public void BuildCustom(params OrderType[] orders)
        {
            BuildBegin(BuildSettingsStore.CurrentPlatform, orders, true);
        }
        #endregion

        #region 工具
        /// <summary>
        /// 把分号分隔的场景字符串拆成列表
        /// </summary>
        private static List<string> ParseScenes(string scenes)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(scenes))
            {
                return list;
            }

            string[] arr = scenes.Split(';');
            for (int i = 0; i < arr.Length; i++)
            {
                string p = arr[i].Trim();
                if (!string.IsNullOrEmpty(p))
                {
                    list.Add(p);
                }
            }

            return list;
        }
        #endregion
    }
}
