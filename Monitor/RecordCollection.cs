using System.Collections.Generic;
using System.Diagnostics;
using Persistence;
using System;
using System.Threading;

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

        Mutex ListOperations;

        public RecordCollection(int capacity)
        {

            Records = new TimeList<ProcessModel>(capacity);
            TimeProcess = new List<DateTime>(capacity);
            ListOperations = new Mutex();
        }

        public void PersistData()
        {
            ListOperations.WaitOne();
            Console.WriteLine("Escribiendo con SQLite");
            for(int i=0; i<Records.Count; i++)
            {
                Console.WriteLine("--- {0} {1}", i, Records[i]);
            }
            DB.GetInstance().PersistRecordList(Records.GetElementsList(), TimeProcess, Records.GetSecondsList());
            Records.Clear();
            TimeProcess.Clear();
            ListOperations.ReleaseMutex();
        }


        public void AddRecord(ProcessModel proc)
        {
            ListOperations.WaitOne();

            if (!Records.CanAdd(proc))
            {
                PersistData();                
            }

            if (Records.Add(proc))
            {
                Console.WriteLine("#{0} {1}", Records.Count - 1, proc);
                TimeProcess.Add(DateTime.Now);
            }
            
            Debug.Assert(Records.ValidateAdjacentDifferent());
            ListOperations.ReleaseMutex();
        }
    }
}
