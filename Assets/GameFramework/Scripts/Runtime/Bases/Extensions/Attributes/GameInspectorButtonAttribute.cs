using UnityEngine;

namespace Honor.Runtime
{
	/// <summary>
	/// 可以在组件成员头用[HonorInspectorButton("XXXXX")]的方式标注
	/// XXXXX为当前类中的按钮回调方法名称
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class GameInspectorButtonAttribute : PropertyAttribute
	{
		public readonly string MethodName;

		public GameInspectorButtonAttribute(string MethodName)
		{
			this.MethodName = MethodName;
		}
	}
}


