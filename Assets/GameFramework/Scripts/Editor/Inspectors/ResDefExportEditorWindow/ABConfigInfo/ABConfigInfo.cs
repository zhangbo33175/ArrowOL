namespace Honor.Editor
{
    public class ABConfigInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="path">路径</param>
        /// <param name="packageMeasureType">AB打包方式</param>
        /// <param name="rename">AB重命名</param>
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

        /// <summary>
        /// ID编码
        /// </summary>
        public int ID;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path;

        /// <summary>
        /// AB打包方式
        /// </summary>
        public int PackageMeasureType;

        /// <summary>
        /// AB重命名
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

        /// <summary>
        /// 是否为平台Manifest-AB文件
        /// </summary>
        public bool IsPlatformManifest
        {
            get { return Path.Equals("Android") || Path.Equals("iOS") || Path.Equals("WebGL"); }
        }
    }
}