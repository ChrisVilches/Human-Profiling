using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class TimeList<T>
    {

        List<Tuple<T, long>> List;
        DateTime LastAdd;

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


        /// <summary>
        /// Get list of seconds used by each process.
        /// </summary>
        /// <param name="fillWith">Usually you'd use DateTime.Now so it completes the used time by the last process up to now.</param>
        public List<long> GetSecondsList(DateTime fillWith)
        {
            List<long> l = new List<long>();
            foreach (Tuple<T, long> t in List)
            {
                l.Add(t.Item2);
            }

            if (l.Count > 0)
                l[l.Count - 1] += (long)(fillWith - LastAdd).TotalSeconds;

            return l;
        }

        public List<long> GetSecondsList()
        {
            return GetSecondsList(DateTime.Now);
        }

        public List<T> GetElementsList()
        {
            List<T> l = new List<T>();
            foreach (Tuple<T, long> t in List)
            {
                l.Add(t.Item1);
            }
            return l;
        }

           
        public TimeList(DateTime lastAdd)
        {
            List = new List<Tuple<T, long>>();
            LastAdd = lastAdd;
        }

        public TimeList() : this(DateTime.Now)
        {
        }

        public void Clear()
        {
            List.Clear();
        }


        T GetLast()
        {
            if(Count == 0) return default(T);
            return this[Count - 1];
        }

        public bool Add(T value, DateTime addTime)
        {
            bool add = false;

            if(Count > 0)
            {
                long seconds = List[Count - 1].Item2;
                long extraSeconds = (long)(addTime - LastAdd).TotalSeconds;
                List[Count - 1] = new Tuple<T, long>(GetLast(), seconds + extraSeconds);
            }
            
            if (!value.Equals(GetLast()))
            {
                List.Add(new Tuple<T, long>(value, 0));
                add = true;
            }

            LastAdd = addTime;

            return add;

        }

        public bool Add(T value)
        {
            return Add(value, DateTime.Now);
        }

        public bool ValidateAdjacentDifferent()
        {
            for (int i = 0; i < Count - 1; i++)
            {
                if (this[i].Equals(this[i + 1]))
                    return false;
            }
            return true;
        }


    }
}
