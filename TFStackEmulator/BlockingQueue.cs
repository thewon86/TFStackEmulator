/*
 * Mainly inspired from Stackoverflow:
 * http://stackoverflow.com/questions/530211/creating-a-blocking-queuet-in-net/530228#530228
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TFStackEmulator
{
    class BlockingConcurrentQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();

        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    Monitor.PulseAll(queue);
                }
            }
        }

        public bool TryDequeue(out T result, int millisecondsTimeout)
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    if (!Monitor.Wait(queue, millisecondsTimeout))
                    {
                        result = default(T);
                        return false;
                    }
                }

                result = queue.Dequeue();
                return true;
            }
        }
    }
}
