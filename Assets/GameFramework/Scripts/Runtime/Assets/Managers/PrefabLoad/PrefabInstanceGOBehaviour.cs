using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Prefab 实例化对象挂载脚本
    /// 用于管理通过克隆/动态加载生成的 GameObject，维护资源引用计数与自动销毁
    /// </summary>
    public class PrefabInstanceGOBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 资源实例ID（用于在资源管理器中定位资源对象）
        /// </summary>
        public int InstanceID = -1;

        /// <summary>
        /// 资源所在 AB 包路径
        /// </summary>
        public string ABPath = string.Empty;

        /// <summary>
        /// 资源名称（Prefab 名称）
        /// </summary>
        public string AssetName = string.Empty;

        /// <summary>
        /// 销毁时是否立即卸载资源
        /// true：立即释放资源内存
        /// false：按延迟策略自动释放
        /// </summary>
        public bool RightNowDestroyOnAsset = false;

        /// <summary>
        /// 绑定的 Lua 逻辑脚本（业务层使用）
        /// </summary>
        public LuaBehaviour LuaBehaviour = null;

        /// <summary>
        /// 激活时执行
        /// 处理【GameObject.Instantiate】克隆方式创建的对象，手动增加引用计数
        /// </summary>
        void Awake()
        {
            // 必须同时配置 ABPath 和 AssetName 才视为克隆对象
            if (string.IsNullOrEmpty(ABPath))
            {
                return;
            }

            if (string.IsNullOrEmpty(AssetName))
            {
                return;
            }

            // 克隆方式实例化的对象，需要手动维护引用计数，确保资源管理器计数正确
            // 注意：克隆对象不受 Manager 统一管控，仅特殊场景允许使用
            InstanceID = gameObject.GetInstanceID();
            GameMainRoot.Asset.PrefabLoadManager.AddAssetRef(ABPath, AssetName, gameObject);
        }

        /// <summary>
        /// 销毁时执行
        /// 被动销毁时自动通知资源管理器，减少引用计数并触发资源卸载
        /// </summary>
        void OnDestroy()
        {
            // 自动销毁，保证引用计数正确，防止资源泄漏
            GameMainRoot.Asset.PrefabLoadManager.Destroy(gameObject, RightNowDestroyOnAsset);
        }
    }
}