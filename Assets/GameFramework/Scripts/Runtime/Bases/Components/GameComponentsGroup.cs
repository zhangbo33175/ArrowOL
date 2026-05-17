/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameComponentsGroup.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏组件全局管理中心，提供组件的注册、获取、清空功能
 *            遵循单类型单例原则，保障框架组件全局唯一访问
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    #region 游戏组件全局注册与获取中心
    /// <summary>
    /// 游戏组件全局注册与获取中心
    /// </summary>
    /// <remarks>
    /// 静态单例管理类，负责统一注册、查找所有继承自 GameComponent 的框架核心组件
    /// 遵循单类型单例原则，禁止重复注册，仅支持Unity主线程使用
    /// </remarks>
    public static class GameComponentsGroup
    {
        //=========================================================================
        // 私有常量与变量
        //=========================================================================
        /// <summary>
        /// 全局注册的框架组件链表
        /// </summary>
        /// <remarks>线程不安全，仅在Unity主线程中操作</remarks>
        private static readonly GameLinkedList<GameComponent> _gameComponents = new GameLinkedList<GameComponent>();

        //=========================================================================
        // 公共获取方法 - 泛型获取
        //=========================================================================
        /// <summary>
        /// 泛型获取已注册的框架组件
        /// </summary>
        /// <typeparam name="T">目标组件类型，必须继承自 GameComponent</typeparam>
        /// <returns>查找到的组件实例，未找到则返回null</returns>
        public static T GetComponent<T>() where T : GameComponent
        {
            return GetComponent(typeof(T)) as T;
        }

        //=========================================================================
        // 公共获取方法 - Type类型获取
        //=========================================================================
        /// <summary>
        /// 根据Type类型获取已注册的框架组件
        /// </summary>
        /// <param name="type">组件目标类型</param>
        /// <returns>匹配的组件实例，未找到则返回null</returns>
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
        // 公共获取方法 - 字符串名称获取
        //=========================================================================
        /// <summary>
        /// 根据类名获取已注册的框架组件
        /// </summary>
        /// <param name="typeName">类全名或类短名称</param>
        /// <returns>匹配的组件实例，未找到则返回null</returns>
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
        // 公共注册方法
        //=========================================================================
        /// <summary>
        /// 注册框架组件到全局管理器
        /// </summary>
        /// <param name="component">待注册的组件实例</param>
        /// <remarks>自动去重，同类型组件仅允许注册一次</remarks>
        public static void RegisterComponent(GameComponent component)
        {
            // 空值安全校验
            if (component == null)
            {
                Log.Error(nameof(RegisterComponent) + " 注册失败：组件实例为空！");
                return;
            }

            Type componentType = component.GetType();

            // 校验组件是否重复注册
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

            // 将组件添加到链表末尾
            _gameComponents.AddLast(component);
        }

        //=========================================================================
        // 公共清空方法
        //=========================================================================
        /// <summary>
        /// 清空所有已注册的框架组件
        /// </summary>
        /// <remarks>适用于场景切换、游戏重启等资源释放场景</remarks>
        public static void Clear()
        {
            _gameComponents.Clear();
        }
    }
    #endregion
}