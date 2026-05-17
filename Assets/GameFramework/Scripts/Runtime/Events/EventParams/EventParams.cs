/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventParams.cs
 * author:    云毅
 * created:   2026
 * descrip:   全局事件参数类 - 封装事件ID与参数
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 全局事件参数类
    /// 用于封装事件ID与自定义参数，支持快速获取 bool/int/float/string/object 类型数据
    /// </summary>
    public class EventParams
    {
        //=========================================================================
        // 公共属性
        //=========================================================================
        #region Properties
        /// <summary>
        /// 事件命令ID（事件类型）
        /// </summary>
        public GameEventCmd Cmd { get; set; }

        /// <summary>
        /// 事件参数键值对（string -> object）
        /// </summary>
        public Dictionary<string, object> Objects { get; set; }
        #endregion

        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cmd">事件ID</param>
        /// <param name="objects">事件参数集合（可为null）</param>
        public EventParams(GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            Cmd = cmd;
            Objects = objects;
        }
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 清理参数引用，重置事件数据
        /// </summary>
        public void Clear()
        {
            Cmd = GameEventCmd.None;
            Objects = null;
        }

        /// <summary>
        /// 安全获取布尔值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>对应值，不存在则返回 false</returns>
        public bool GetBool(string name)
        {
            if (Objects != null && Objects.ContainsKey(name))
            {
                return bool.Parse(Objects[name].ToString());
            }
            return false;
        }

        /// <summary>
        /// 安全获取整型值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>对应值，不存在则返回 0</returns>
        public int GetInt(string name)
        {
            if (Objects != null && Objects.ContainsKey(name))
            {
                return int.Parse(Objects[name].ToString());
            }
            return 0;
        }

        /// <summary>
        /// 安全获取浮点值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>对应值，不存在则返回 0</returns>
        public float GetFloat(string name)
        {
            if (Objects != null && Objects.ContainsKey(name))
            {
                return float.Parse(Objects[name].ToString());
            }
            return 0f;
        }

        /// <summary>
        /// 安全获取字符串
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>对应值，不存在则返回 string.Empty</returns>
        public string GetString(string name)
        {
            if (Objects != null && Objects.ContainsKey(name))
            {
                return Objects[name].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 安全获取对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>对应对象，不存在则返回 null</returns>
        public object GetObject(string name)
        {
            if (Objects != null && Objects.ContainsKey(name))
            {
                return Objects[name];
            }
            return null;
        }
        #endregion
    }
}