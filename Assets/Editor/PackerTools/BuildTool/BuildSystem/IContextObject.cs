/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  IContextObject.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 上下文对象标记接口
 *            所有注入 BuildContext 的上下文对象必须实现此空接口，
 *            便于以类型为键统一存取。
 ***************************************************************/

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打包上下文对象标记接口
    /// 说明：空接口，仅用于约束可注入 <see cref="BuildContext"/> 的对象类型，
    ///       以便通过类型安全方式存取各打包阶段共享数据。
    /// </summary>
    public interface IContextObject
    {
    }
}
