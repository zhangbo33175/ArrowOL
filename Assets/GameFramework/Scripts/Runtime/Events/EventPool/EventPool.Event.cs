
namespace Honor.Runtime
{
    public sealed partial class EventPool<T> where T : EventParams
    {
        private sealed class Event
        {
            private object m_Sender;
            private T m_EventParams;

            public Event()
            {
                m_Sender = null;
                m_EventParams = null;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }

            public T EventParams
            {
                get
                {
                    return m_EventParams;
                }
            }

            public static Event Create(object sender, T e)
            {
                Event eventNode = new Event();
                eventNode.m_Sender = sender;
                eventNode.m_EventParams = e;
                return eventNode;
            }

            public void Clear()
            {
                m_Sender = null;
                m_EventParams = null;
            }

        }
    }
}


