using System.Collections.Generic;
using System.Diagnostics;
using Persistence;
using System;

namespace Monitor
{
    public class RecordCollection
    {
        TimeList<ProcessModel> Records;

        /// <summary>
        /// This is the datetime the user switched to that process. If the user clicked 
        /// on Chrome on 3:03:01, then that's the value (includes date dd/mm/yyyy, etc).
        /// </summary>
        List<DateTime> TimeProcess;

        public RecordCollection(int capacity)
        {
            Records = new TimeList<ProcessModel>(capacity);
            TimeProcess = new List<DateTime>(capacity);
        }

        void PersistData()
        {
            Console.WriteLine("Escribiendo con SQLite");
            for(int i=0; i<Records.Count; i++)
            {
                Console.WriteLine("--- {0} {1}", i, Records[i]);
            }
            DB.GetInstance().PersistRecordList(Records.GetElementsList(), TimeProcess, Records.GetSecondsList());
        }


        public bool AddRecord(ProcessModel proc)
        {

            if (Records.ShouldFlushBeforeAdding())
            {
                PersistData();
                Records.Clear();
                TimeProcess.Clear();
            }

            if (Records.Add(proc))
            {
                Console.WriteLine("#{0} {1}", Records.Count, proc);
                TimeProcess.Add(DateTime.Now);
            }            

            Debug.Assert(Records.ValidateAdjacentDifferent());


            return true;
        }
    }
}
