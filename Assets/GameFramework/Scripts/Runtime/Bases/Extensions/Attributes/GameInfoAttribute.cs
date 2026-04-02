using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 可以在组件成员头用[HonorInfo("XXXXX", infoType, messageAfterProperty)]的方式标注
    /// </summary>
    public class GameInfoAttribute : PropertyAttribute
    {
		public enum InfoType { Error, Info, None, Warning }

		public string Message;
		public InfoType Type;
		public bool MessageAfterProperty;

		public GameInfoAttribute(string message, InfoType type, bool messageAfterProperty)
		{
			this.Message = message;
			this.Type = type;
			this.MessageAfterProperty = messageAfterProperty;
		}
	}
}


