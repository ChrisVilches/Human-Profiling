using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class TimeList<T>
    {
        List<Tuple<T, long>> List;        
        T WaitingToBeAdded;
        bool SomethingHasBeenAdded = false;
        DateTime LastTime;
        public int Count
        {
            get
            {
                return List.Count;
            }
        }

        public T this[int key]
        {
            get
            {
                return List[key].Item1;
            }
        }

        public long SecondsAt(int i)
        {
            return List[i].Item2;
        }


        public TimeList(int capacity)
        {
            List = new List<Tuple<T, long>>(capacity);
            LastTime = DateTime.Now;
        }
        
        public bool ShouldFlushBeforeAdding()
        {
            return List.Capacity == List.Count;
        }

        public void Clear()
        {
            List.Clear();
        }

        public void Add(T value, DateTime addTime)
        {
            T addNow = WaitingToBeAdded;
            WaitingToBeAdded = value;

            long seconds = (long)(addTime - LastTime).TotalSeconds;
            LastTime = addTime;

            if (!SomethingHasBeenAdded)
            {
                SomethingHasBeenAdded = true;
                return;
            }                    

            List.Add(new Tuple<T, long>(addNow, seconds));
        }

        public void Add(T value)
        {
            Add(value, DateTime.Now);
        }

        public List<T> GetElementsList()
        {
            List<T> l = new List<T>();
            foreach(Tuple<T, long> t in List)
            {
                l.Add(t.Item1);
            }
            return l;
        }

        public List<long> GetSecondsList()
        {
            List<long> l = new List<long>();
            foreach (Tuple<T, long> t in List)
            {
                l.Add(t.Item2);
            }
            return l;
        }

    }
}
