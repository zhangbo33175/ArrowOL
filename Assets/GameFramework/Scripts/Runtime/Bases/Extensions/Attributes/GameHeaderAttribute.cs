using UnityEngine;

namespace Honor.Runtime
{
    [System.Serializable]
    public class GameHeaderAttribute : PropertyAttribute
    {
        public readonly string header;

        public GameHeaderAttribute(string header)
        {
            this.header = header;
        }
    }
}


