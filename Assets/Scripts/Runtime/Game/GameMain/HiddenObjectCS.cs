using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace GameLib
{
    [LuaCallCSharp]
    public class HiddenObjectCS: MonoBehaviour, IPointerClickHandler
    {
        public string objectType = "Turtle";
        public bool isCollected = false;

        void Start()
        {
            // 初始化Lua侧数据
            //LuaManager.Instance.CallLuaFunction("HiddenObject.Init", gameObject.name, objectType);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isCollected) return;
            // 调用Lua点击逻辑
            //LuaManager.Instance.CallLuaFunction("HiddenObject.OnClick", gameObject.name);
        }

        // 供Lua调用的收集物体方法
        public void Collect()
        {
            isCollected = true;
            // 播放收集动画/音效
            GetComponent<Animator>()?.SetTrigger("Collect");
            Debug.Log($"收集到物体：{objectType}");
        }

        // 供Lua调用的高亮方法
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