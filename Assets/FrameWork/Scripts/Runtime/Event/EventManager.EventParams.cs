using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class EventManager
    {
        public class EventParams
        {
            /// <summary>
            /// 事件类型编号
            /// </summary>
            public EventCmd EventId { set; get; }

            /// <summary>
            /// 事件参数
            /// </summary>
            public Dictionary<string, object> EventParameters { set; get; }

            public EventParams(EventCmd eventId, Dictionary<string, object> eventParameters = null)
            {
                EventId = eventId;
                EventParameters = eventParameters;
            }

            /// <summary>
            /// 清理引用
            /// </summary>
            public void Clear()
            {
                EventId = EventCmd.None;
                EventParameters = null;
            }

            /// <summary>
            /// 获取Bool值
            /// </summary>
            /// <param name="name">名称</param>
            /// <returns></returns>
            public bool GetBool(string name)
            {
                if (EventParameters != null)
                {
                    if (EventParameters.ContainsKey(name))
                    {
                        return bool.Parse(EventParameters[name].ToString());
                    }
                }

                return false;
            }

            /// <summary>
            /// 获取Int值
            /// </summary>
            /// <param name="name">名称</param>
            /// <returns></returns>
            public int GetInt(string name)
            {
                if (EventParameters != null)
                {
                    if (EventParameters.ContainsKey(name))
                    {
                        return int.Parse(EventParameters[name].ToString());
                    }
                }

                return 0;
            }

            /// <summary>
            /// 获取Float值
            /// </summary>
            /// <param name="name">名称</param>
            /// <returns></returns>
            public float GetFloat(string name)
            {
                if (EventParameters != null)
                {
                    if (EventParameters.ContainsKey(name))
                    {
                        return float.Parse(EventParameters[name].ToString());
                    }
                }

                return 0;
            }

            /// <summary>
            /// 获取String值
            /// </summary>
            /// <param name="name">名称</param>
            /// <returns></returns>
            public string GetString(string name)
            {
                if (EventParameters != null)
                {
                    if (EventParameters.ContainsKey(name))
                    {
                        return EventParameters[name].ToString();
                    }
                }

                return string.Empty;
            }

            /// <summary>
            /// 获取object
            /// </summary>
            /// <param name="name">名称</param>
            /// <returns></returns>
            public object GetObject(string name)
            {
                if (EventParameters != null)
                {
                    if (EventParameters.ContainsKey(name))
                    {
                        return EventParameters[name];
                    }
                }

                return null;
            }
        }
    }
}