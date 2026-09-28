/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  UIUtils.cs
 * author:  云毅
 * created:
 * descrip:   UGUI 工具类 - 点击命中判断、通用 UI 辅助
 * 优化记录: 由旧版 HonorUtils.UIUtils 迁移，统一命名空间，补充触摸与鼠标双端判断
 ***************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

namespace Honor.Runtime
{
    /// <summary>
    /// UGUI 工具类
    /// 功能：提供 UI 点击命中判断等高频 UI 辅助方法
    /// </summary>
    public static class UIUtils
    {
        /// <summary>
        /// 当前帧按下位置是否命中 UI（鼠标 / 单点触摸）
        /// 用于区分"点击了 UI"与"点击了游戏场景"，避免 UI 与场景输入互相干扰
        /// </summary>
        /// <returns>true 表示点击在 UI 上</returns>
        public static bool IsUIRaycast()
        {
            // 无 EventSystem 时无法判定，视为未点击 UI
            if (EventSystem.current == null)
            {
                return false;
            }

            // 鼠标左键按下：直接判定指针是否悬停于 UI 对象
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return true;
                }
            }

            // 单点触摸开始：按手指 ID 判定（多点触控时使用手指索引）
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
