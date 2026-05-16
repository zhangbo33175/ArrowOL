using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    #region 游戏组件全局注册与获取中心
    /// <summary>
    /// 游戏组件全局注册与获取中心（静态单例管理类）
    /// 负责统一注册、查找所有继承自 GameComponent 的框架核心组件
    /// 遵循单类型单例原则，禁止重复注册
    /// </summary>
    public static class GameComponentsGroup
    {
        //=========================================================================
        // 私有成员变量
        //=========================================================================
        /// <summary>
        /// 全局注册的框架组件链表（线程不安全，仅主线程使用）
        /// </summary>
        private static readonly GameLinkedList<GameComponent> _gameComponents = new GameLinkedList<GameComponent>();

        //=========================================================================
        // 泛型获取组件
        //=========================================================================
        /// <summary>
        /// 泛型获取已注册的框架组件（最常用、最安全）
        /// </summary>
        /// <typeparam name="T">目标组件类型（必须继承自 GameComponent）</typeparam>
        /// <returns>对应组件实例，未找到返回 null</returns>
        public static T GetComponent<T>() where T : GameComponent
        {
            return GetComponent(typeof(T)) as T;
        }

        //=========================================================================
        // Type 获取组件
        //=========================================================================
        /// <summary>
        /// 根据 Type 类型获取框架组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <returns>匹配的组件实例，未找到返回 null</returns>
        public static GameComponent GetComponent(Type type)
        {
            if (type == null)
            {
                Log.Error(nameof(GetComponent) + " 传入的类型为空！");
                return null;
            }

            LinkedListNode<GameComponent> currentNode = _gameComponents.First;
            while (currentNode != null)
            {
                if (currentNode.Value.GetType() == type)
                {
                    return currentNode.Value;
                }
                currentNode = currentNode.Next;
            }

            return null;
        }

        //=========================================================================
        // 字符串名称获取组件
        //=========================================================================
        /// <summary>
        /// 根据类名（全名/短名）获取框架组件
        /// </summary>
        /// <param name="typeName">类全名 或 类短名</param>
        /// <returns>匹配的组件实例，未找到返回 null</returns>
        public static GameComponent GetComponent(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Log.Error(nameof(GetComponent) + " 传入的类型名称为空！");
                return null;
            }

            LinkedListNode<GameComponent> currentNode = _gameComponents.First;
            while (currentNode != null)
            {
                Type componentType = currentNode.Value.GetType();
                if (componentType.FullName == typeName || componentType.Name == typeName)
                {
                    return currentNode.Value;
                }
                currentNode = currentNode.Next;
            }

            return null;
        }

        //=========================================================================
        // 注册组件
        //=========================================================================
        /// <summary>
        /// 注册框架组件（自动去重，同类型只能注册一次）
        /// </summary>
        /// <param name="component">需要注册的组件实例</param>
        public static void RegisterComponent(GameComponent component)
        {
            // 空值安全检查
            if (component == null)
            {
                Log.Error(nameof(RegisterComponent) + " 注册失败：组件实例为空！");
                return;
            }

            Type componentType = component.GetType();

            // 检查是否已注册同类型
            LinkedListNode<GameComponent> currentNode = _gameComponents.First;
            while (currentNode != null)
            {
                if (currentNode.Value.GetType() == componentType)
                {
                    Log.Error(nameof(RegisterComponent) + $" 注册失败：类型 {componentType.FullName} 已注册，不可重复注册！");
                    return;
                }
                currentNode = currentNode.Next;
            }

            // 添加到链表尾部
            _gameComponents.AddLast(component);
        }

        //=========================================================================
        // 清空所有组件
        //=========================================================================
        /// <summary>
        /// 清空所有已注册的组件（场景切换/游戏重启时调用）
        /// </summary>
        public static void Clear()
        {
            _gameComponents.Clear();
        }
    }
    #endregion
}