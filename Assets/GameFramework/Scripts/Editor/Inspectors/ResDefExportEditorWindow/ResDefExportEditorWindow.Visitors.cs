using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Honor.Editor
{
    
    public partial class ResDefExportEditorWindow : BaseEditorWindow<ResDefExportEditorWindow>
    {
        /// <summary>
        /// 滚动位置
        /// </summary>
        private Vector2 m_TextScrollViewPosition = Vector2.zero;
        private Vector2 m_LeftScrollViewPosition = Vector2.zero;
        private Vector2 m_RightScrollViewPosition = Vector2.zero;

        /// <summary>
        /// 窗口标题
        /// </summary>
        protected override GUIContent Title => new GUIContent("ResDef Exporter");

        /// <summary>
        /// 窗口最小尺寸
        /// </summary>
        protected override Vector2 MinSize => new Vector2(1700, 845);

        /// <summary>
        /// 窗口最大尺寸
        /// </summary>
        protected override Vector2 MaxSize => Vector2.zero;

        /// <summary>
        /// 转换本地记录的所有ResAB信息
        /// </summary>
        protected ResDefInfos m_AllResDefInfos;

        /// <summary>
        /// 当前选择的文件类型
        /// </summary>
        protected int m_SelectResType = 0;

        /// <summary>
        /// 查找到的ABPath路径
        /// </summary>
        private string m_FindFileABPath = String.Empty;

        /// <summary>
        /// 找到符合条件的文件未追加前
        /// </summary>
        private Dictionary<string, FileUseState> m_FindFileFullPathList;

        /// <summary>
        /// 缓冲区的所有临时数据
        /// </summary>
        private List<ResDefItem> m_TempResDefItems;

        /// <summary>
        /// 输入删除的开始ID
        /// </summary>
        private int m_InputDelStartID = 0;

        /// <summary>
        /// 输入删除的结束ID
        /// </summary>
        private int m_InputDelEndID = 0;

        /// <summary>
        /// 左右的宽度分割比例
        /// </summary>
        public float m_SubPercenttage = 0.64f;

        /// <summary>
        /// 当前id的最大值
        /// </summary>
        private int m_CurMaxResID = 0;

        /// <summary>
        /// 得到当前的Res资源ID
        /// </summary>
        private int CurMaxResID
        {
            get { return ++m_CurMaxResID; }
        }

        /// <summary>
        /// 折叠的框
        /// </summary>
        private Dictionary<string, TopItemInfo> m_AllShowFoldout;

        /// <summary>
        /// 当前查找的target
        /// </summary>
        private UnityEngine.Object m_FindTargetAsset;
        public UnityEngine.Object FindTargetAsset
        {
            get
            {
                return m_FindTargetAsset;
            }
            set
            {
                if (m_FindTargetAsset == value)
                {
                    return;
                }
                m_FindTargetAsset = value;
                SetFindTargetAsset(m_FindTargetAsset);
            }
        }

        /// <summary>
        /// AB配置信息
        /// <assetBundleName, assetBundlePath>
        /// </summary>
        public static Dictionary<string, string> s_ABConfigs = new Dictionary<string, string>();

        /// <summary>
        /// 结果页上失效的路径集合
        /// </summary>
        public Dictionary<string, Dictionary<int, Boolean>> m_ResultInvalidPath;

        /// <summary>
        /// 结果页上的路径集合
        /// </summary>
        public Dictionary<string, List<ResDefItem>> m_ResultDetailInfo;

        /// <summary>
        /// 结果页上的同名资源信息
        /// </summary>
        public Dictionary<string, bool> m_ResultSameResInfo;
        
        // 默认路径下加载的所有图片
        private List<Texture2D> m_AllDefaultTextures;
        
        /// <summary>
        /// 搜索资源的名字
        /// </summary>
        private string m_SearchResName = string.Empty;

        /// <summary>
        /// 搜索的Res信息集合
        /// </summary>
        private List<ResDefItem> m_SearchResDefItemInfo;

        /// <summary>
        /// 报错的Res信息集合
        /// </summary>
        private List<ResDefItem> m_ErrorResDefItemInfo;

        /// <summary>
        /// 结果页选择的类型（0: 普通， 1：仅报错）
        /// </summary>
        private bool m_OnlyShowError = false;

        /// <summary>
        /// 搜索页的tag
        /// </summary>
        private string m_SearchTag = "Search";

        /// <summary>
        /// 报错页的Tag
        /// </summary>
        private string m_ErrorTag = "Error";

        /// <summary>
        /// 导出文件按照每x个数据为一页导出
        /// </summary>
        private static int m_OneSheetMaxCount = 300;

    }
}