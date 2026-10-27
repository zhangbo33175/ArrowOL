/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ItemMap.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图物品基础组件（标记组件）
 *            挂载于 ItemMap 预制体，作为地图物件的身份标识
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 地图物品基础组件
    /// 标记组件：用于预制体挂载与运行时 GetComponent 查找
    /// 实际行为由 Lua 侧 MapObjectBase / BuildingItem / ObjectItem 驱动
    /// </summary>
    public class ItemMap : MonoBehaviour
    {
    }
}
