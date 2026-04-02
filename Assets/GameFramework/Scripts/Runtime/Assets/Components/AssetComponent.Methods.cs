using UnityEngine;

namespace Honor.Runtime
{
    public delegate void PrefabLoadOverCallback(PrefabObject prefabObject, GameObject gameObject);
    public delegate void AssetLoadOverCallback(AssetObject assetObject, Object asset);
    public delegate void AssetUnloadOverCallback(AssetObject assetObject);
    public delegate void AssetBundleLoadOverCallBack(AssetBundleObject assetBundleObject, AssetBundle ab);

    public sealed partial class AssetComponent : GameComponent
    {

    }

}


