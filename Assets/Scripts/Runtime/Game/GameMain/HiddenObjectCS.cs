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
        /// <summary>
        /// 物体类型（如：乌龟、贝壳等）
        /// </summary>
        public string objectType = "Turtle";

        /// <summary>
        /// 是否已被收集
        /// </summary>
        public bool isCollected = false;

        void Start()
        {
            // 初始化 Lua 侧数据（根据物体名和类型）
            //LuaManager.Instance.CallLuaFunction("HiddenObject.Init", gameObject.name, objectType);
        }

        /// <summary>
        /// 点击物体触发
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 已收集则不再响应点击
            if (isCollected)
                return;

            // 调用 Lua 层点击逻辑
            //LuaManager.Instance.CallLuaFunction("HiddenObject.OnClick", gameObject.name);
        }

        /// <summary>
        /// 收集物体（供 Lua 调用）
        /// 标记已收集 + 播放收集动画
        /// </summary>
        public void Collect()
        {
            isCollected = true;

            // 播放收集动画
            GetComponent<Animator>()?.SetTrigger("Collect");

            Debug.Log($"收集到物体：{objectType}");
        }

        /// <summary>
        /// 设置物体高亮（供 Lua 调用）
        /// 黄色 = 高亮 / 白色 = 正常
        /// </summary>
        public void Highlight(bool isHighlight)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = isHighlight ? Color.yellow : Color.white;
            }
        }
    }
}