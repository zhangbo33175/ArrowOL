using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public static class GameComponentsGroup
    {
        /// <summary>
        /// 框架组件注册链表
        /// </summary>
        private static readonly GameLinkedList<GameComponent> SGameComponents = new GameLinkedList<GameComponent>();

        /// <summary>
        /// 获取游戏框架组件
        /// </summary>
        /// <typeparam name="T">GameComponent的派生类型</typeparam>
        /// <returns>GameComponent的派生类型的实例</returns>
        public static T GetComponent<T>() where T : GameComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取游戏框架组件
        /// </summary>
        /// <param name="type">GameComponent的派生类型</param>
        /// <returns>GameComponent的派生类型的实例</returns>
        public static GameComponent GetComponent(Type type)
        {
            LinkedListNode<GameComponent> current = SGameComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }
                current = current.Next;
            }
            return null;
        }

        /// <summary>
        /// 获取游戏框架组件
        /// </summary>
        /// <param name="typeName">GameComponent的派生类型的名称</param>
        /// <returns>GameComponent的派生类型的实例</returns>
        public static GameComponent GetComponent(string typeName)
        {
            LinkedListNode<GameComponent> current = SGameComponents.First;

            while (current != null)
            {
                Type type = current.Value.GetType();
                if (type.FullName == typeName || type.Name == typeName)
                {
                    return current.Value;
                }
                current = current.Next;
            }
            return null;
        }

        /// <summary>
        /// 注册游戏框架组件（不会重复添加）
        /// </summary>
        /// <param name="component">待注册的组件实例</param>
        public static void RegisterComponent(GameComponent component)
        {
            if (component == null)
            {
                Log.Error("Honor组件无效。");
                return;
            }

            Type type = component.GetType();

            LinkedListNode<GameComponent> current = SGameComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.Error("Honor组件类型'{0}'已经存在，不可重复注册。", type.FullName);
                    return;
                }

                current = current.Next;
            }

            SGameComponents.AddLast(component);
        }

        /// <summary>
        /// 清空已注册的所有游戏框架组件
        /// </summary>
        public static void Clear()
        {
            SGameComponents.Clear();
        }

    }

}


