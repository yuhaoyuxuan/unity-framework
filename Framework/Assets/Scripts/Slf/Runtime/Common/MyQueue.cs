using System.Collections.Generic;

//==========================
// - Author:      slf         
// - Date:        2021/08/09 13:31:07	
// - Description: 队列
//==========================
namespace Slf
{
    public class MyQueue<T> where T : new()
    {
        private Queue<T> Queue;
        public MyQueue()
        {
            Queue = new Queue<T>();
        }

        public T Dequeue()
        {
            if (Queue.Count > 0)
            {
                return Queue.Dequeue();
            }
            return new T();
        }

        public void Enqueue(T target)
        {
            Queue.Enqueue(target);
        }

        public void Destroy()
        {
            Queue.Clear();
            Queue = null;
        }
    }
}



