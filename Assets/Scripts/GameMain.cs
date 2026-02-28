using UnityEngine;
using UnityEngine.UI;

namespace GameLib
{
    public class GameMain:MonoBehaviour
    {
        private static GameMain _instance;
        public static GameMain Instance() {
            return _instance;
        }

        #region UI节点
        /// <summary>
        /// 2DUIRoot
        /// </summary>
        [HideInInspector]
        public Transform UIRoot2D;
        /// <summary>
        /// UICamera2D
        /// </summary>
        [HideInInspector]
        public Camera UICamera2D;
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        private void Awake()
        {
            _instance = this;
            UIRoot2D = GameObject.Find("UIRoot/2DUIRoot").transform;
            UICamera2D = UIRoot2D.Find("2DUICamera").GetComponent<Camera>(); 
            //装载资源加载
            OnSetupAssetUtils();
            //装载Lua信息
            OnSetupLua();
        }
        /// <summary>
        /// 开始
        /// </summary>
        protected void Start()
        {
        }

        /// <summary>
        /// 清理安装管理
        /// </summary>
        private void OnCleanup()
        {
         
        }
        /// <summary>
        /// 安装Asset信息
        /// </summary>
        private void OnSetupAssetUtils()
        {
        }
        /// <summary>
        /// 安装UI信息
        /// </summary>
        private void OnSetupUI()
        {
        }
        /// <summary>
        /// 帧推更新
        /// </summary>
        protected void Update()
        {
            //ProcedureManager.Instance.Update();
        }

        /// <summary>
        /// Lua安装环境
        /// </summary>
        public void OnSetupLua()
        {
            //LuaComponent.Instance.Init();
        }
        /// <summary>
        /// 获取2DUI层节点
        /// </summary>
        /// <returns></returns>
        public Transform Get2DUILayers() {
            return UIRoot2D.Find("2DUILayers").transform;
        }
        /// <summary>
        /// 获取GetLayerWithLauncher节点
        /// </summary>
        /// <returns></returns>
        public Transform GetLayerWithLauncher() {
            return UIRoot2D.Find("2DUILayers/Launcher").transform;
        }
        /// <summary>
        /// 获取GetLayerWithLauncherCanvasScaler节点
        /// </summary>
        /// <returns></returns>
        public CanvasScaler GetLayerWithLauncherCanvasScaler() {
            return UIRoot2D.Find("2DUILayers/Launcher").GetComponent<CanvasScaler>();
        }
        /// <summary>
        /// 获取GetLayerWithLauncherCanvas节点
        /// </summary>
        /// <returns></returns>
        public Canvas GetLayerWithLauncherCanvas() {
            return UIRoot2D.Find("2DUILayers/Launcher").GetComponent<Canvas>();
        }
    }
}