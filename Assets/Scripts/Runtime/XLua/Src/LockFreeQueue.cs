
using System.Threading;

namespace XLua
{
    public class LockFreeQueue<T>
    {
        internal class SingleLinkNode
        {
            public SingleLinkNode Next;
            public T Item;
        }
        static private bool CAS<V>(ref V location, V comparand, V newValue) where V : class
        {
            return comparand == Interlocked.CompareExchange(ref location, newValue, comparand);
        }

        SingleLinkNode head;
        SingleLinkNode tail;
        int count;

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count <= 0; } }

        public LockFreeQueue()
        {
            head = new SingleLinkNode();
            tail = head;
            count = 0;
        }

        public void Enqueue(T item)
        {
            SingleLinkNode oldTail = null;
            SingleLinkNode oldTailNext;

            SingleLinkNode newNode = new SingleLinkNode();
            newNode.Item = item;

            bool newNodeAdded = false;
            while (!newNodeAdded)
            {
                oldTail = tail;
                oldTailNext = oldTail.Next;

                if (tail == oldTail)
                {
                    if (oldTailNext == null)
                        newNodeAdded = CAS<SingleLinkNode>(ref tail.Next, null, newNode);
                    else
                        CAS<SingleLinkNode>(ref tail, oldTail, oldTailNext);
                }
            }
            CAS<SingleLinkNode>(ref tail, oldTail, newNode);
            Interlocked.Increment(ref count);
        }

        public bool TryDequeue(out T item)
        {
            item = default(T);
            SingleLinkNode oldHead = null;

            bool haveAdvancedHead = false;
            while (!haveAdvancedHead)
            {
                oldHead = head;
                SingleLinkNode oldTail = tail;
                SingleLinkNode oldHeadNext = oldHead.Next;

                if (oldHead == head)
                {
                    if (oldHead == oldTail)
                    {
                        if (oldHeadNext == null)
                        {
                            return false;
                        }
                        CAS<SingleLinkNode>(ref tail, oldTail, oldHeadNext);
                    }
                    else
                    {
                        item = oldHeadNext.Item;
                        haveAdvancedHead = CAS<SingleLinkNode>(ref head, oldHead, oldHeadNext);
                    }
                }
            }
            Interlocked.Decrement(ref count);
            return true;
        }

        public T Dequeue()
        {
            T result;
            if (TryDequeue(out result)) return result;
            return default(T);
        }

        public void Clear()
        {
            while (Count > 0)
            {
                Dequeue();
            }
        }
    }
}


