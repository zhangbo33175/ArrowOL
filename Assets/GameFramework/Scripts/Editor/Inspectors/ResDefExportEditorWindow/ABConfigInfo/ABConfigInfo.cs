/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ABConfigInfo.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 配置信息实体类
 ***************************************************************/

namespace Honor.Editor
{
    /// <summary>
    /// AssetBundle 打包配置信息实体类
    /// </summary>
    public class ABConfigInfo
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="path">资源路径</param>
        /// <param name="packageMeasureType">AB打包方式类型</param>
        /// <param name="rename">AB包重命名</param>
        /// <param name="groupName">分组名称</param>
        /// <param name="isIncreaserGroup">是否为增量分组</param>
        /// <param name="isCommonIncreaserGroup">是否为公用增量分组</param>
        public ABConfigInfo(int id, string path, int packageMeasureType, string rename, string groupName,
            bool isIncreaserGroup, bool isCommonIncreaserGroup)
        {
            ID = id;
            Path = path;
            PackageMeasureType = packageMeasureType;
            Rename = rename;
            GroupName = groupName;
            IsIncreaserGroup = isIncreaserGroup;
            IsCommonIncreaserGroup = isCommonIncreaserGroup;
        }
        #endregion

        //=========================================================================
        // 公开字段
        //=========================================================================
        #region 公开字段
        /// <summary>
        /// ID编码
        /// </summary>
        public int ID;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string Path;

        /// <summary>
        /// AB打包方式
        /// </summary>
        public int PackageMeasureType;

        /// <summary>
        /// AB包重命名
        /// </summary>
        public string Rename;

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName;

        /// <summary>
        /// 是否为增量分组
        /// </summary>
        public bool IsIncreaserGroup;

        /// <summary>
        /// 是否为公用增量分组
        /// </summary>
        public bool IsCommonIncreaserGroup;
        #endregion

        //=========================================================================
        // 只读属性
        //=========================================================================
        #region 只读属性
        /// <summary>
        /// 是否为平台Manifest-AB文件
        /// </summary>
        public bool IsPlatformManifest => IsPlatformManifestPath(Path);
        #endregion

        //=========================================================================
        // 私有工具方法
        //=========================================================================
        #region 私有工具方法
        /// <summary>
        /// 判断给定路径是否对应某个发布平台的 Manifest 资源分组
        /// </summary>
        /// <param name="assetPath">资源分组路径</param>
        /// <returns>命中 Android/iOS/WebGL 平台名时返回 true</returns>
        private static bool IsPlatformManifestPath(string assetPath)
        {
            return assetPath.Equals("Android")
                   || assetPath.Equals("iOS")
                   || assetPath.Equals("WebGL");
        }
        #endregion
    }
}