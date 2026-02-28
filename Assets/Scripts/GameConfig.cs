using UnityEngine;

namespace GameLib
{
    public partial class GameConfig:MonoBehaviour
    {
        public static GameConfig instance;
        
        void Awake() {
            instance = this;
        }  
    }
}