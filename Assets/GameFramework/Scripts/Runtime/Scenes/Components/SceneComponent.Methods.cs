namespace Honor.Runtime
{
    public delegate void SceneLoadOverCallback(string abPath, string assetName, UnityEngine.SceneManagement.Scene scene);
    public delegate void SceneUnloadOverCallback(string abPath, string assetName);

    public sealed partial class SceneComponent : GameComponent
    {

    }

}


