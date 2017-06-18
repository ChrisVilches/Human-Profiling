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

        public bool Add(T value, DateTime addTime)
        {
            if (value.Equals(WaitingToBeAdded))
                return false;

            T addNow = WaitingToBeAdded;
            WaitingToBeAdded = value;

            long seconds = (long)(addTime - LastTime).TotalSeconds;
            LastTime = addTime;

            if (!SomethingHasBeenAdded)
            {
                SomethingHasBeenAdded = true;
                return false;
            }

            Console.WriteLine("Se agrega a la lista el que estaba esperando {0}", addNow);
            List.Add(new Tuple<T, long>(addNow, seconds));
            return true;
        }

        public bool Add(T value)
        {
            return Add(value, DateTime.Now);
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


        public bool ValidateAdjacentDifferent()
        {
            for(int i=0; i<Count-1; i++)
            {
                if (List[i].Item1.Equals(List[i + 1].Item1))
                    return false;
            }
            return true;
        }

    }
}
