/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HiddenObjectCS.cs
 * author:    云毅
 * created:   2026
 * descrip:   寻物游戏交互物体 - C#与XLua交互核心组件
 *            处理物体点击响应、收集状态、高亮效果、动画触发
 ***************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace GameLib
{
    /// <summary>
    /// 寻物游戏交互物体（C# 与 Lua 交互层）
    /// 负责物体点击、收集状态、高亮、动画触发
    /// 可被 Lua 调用 [LuaCallCSharp]
    /// </summary>
    [LuaCallCSharp]
    public class HiddenObjectCS : MonoBehaviour, IPointerClickHandler
    {
        #region 公开字段
        //=========================================================================
        // 公开字段
        //=========================================================================
        /// <summary>
        /// 物体类型（如：乌龟、贝壳等寻物分类标识）
        /// </summary>
        public string objectType = "Turtle";

        /// <summary>
        /// 物体是否已被收集
        /// </summary>
        public bool isCollected = false;
        #endregion

        #region  MonoBehaviour 生命周期
        //=========================================================================
        // 生命周期函数
        //=========================================================================
        /// <summary>
        /// 组件初始化
        /// </summary>
        private void Start()
        {
            // 初始化 Lua 侧数据（根据物体名和类型）
            //LuaManager.Instance.CallLuaFunction("HiddenObject.Init", gameObject.name, objectType);
        }
        #endregion

        #region  点击事件实现
        //=========================================================================
        // 点击事件接口实现
        //=========================================================================
        /// <summary>
        /// 指针点击事件响应
        /// </summary>
        /// <param name="eventData">点击事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 已收集则不再响应点击
            if (isCollected)
                return;

            // 调用 Lua 层点击逻辑
            //LuaManager.Instance.CallLuaFunction("HiddenObject.OnClick", gameObject.name);
        }
        #endregion

        #region  公开交互方法（供Lua调用）
        //=========================================================================
        // 公开交互方法
        //=========================================================================
        /// <summary>
        /// 收集物体（供 Lua 调用）
        /// 标记已收集状态 + 播放收集动画
        /// </summary>
        public void Collect()
        {
            isCollected = true;

            // 播放收集动画
            GetComponent<Animator>()?.SetTrigger("Collect");

            Debug.Log($"收集到物体：{objectType}");
        }

        /// <summary>
        /// 设置物体高亮状态（供 Lua 调用）
        /// 黄色 = 高亮 / 白色 = 正常显示
        /// </summary>
        /// <param name="isHighlight">是否开启高亮</param>
        public void Highlight(bool isHighlight)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = isHighlight ? Color.yellow : Color.white;
            }
        }
        #endregion
    }
}