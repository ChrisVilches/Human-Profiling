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
            DB.GetInstance().PersistRecordList(Records.GetElementsList(), TimeProcess, Records.GetSecondsList());
        }

        public bool AddRecord(Process process)
        {
            

            /*if(Records.Count > 0) {
            if(process.Id == Records[Records.Count - 1].Id)
            {
                Console.WriteLine("ID igual {0}", process.Id);
            }

            if (process.MainWindowTitle.Equals(Records[Records.Count - 1].Title))
            {
                Console.WriteLine("titulo igual {0} (ids={1} {2})", process.MainWindowTitle, process.Id, Records[Records.Count - 1].Id);
            }
            }*/

            /* if (new process == last added process) */
            if (Records.Count > 0 &&
                process.Id == Records[Records.Count - 1].Id &&
                process.MainWindowTitle.Equals(Records[Records.Count - 1].Title))
            {
                /*Console.WriteLine("no se agrega. count={0}. ids {1}=={2}. titulos {3}=={4}", Records.Count,
                    process.Id, Records[Records.Count - 1].Id, process.MainWindowTitle.GetHashCode(), Records[Records.Count - 1].Title.GetHashCode());
                    */
                Console.WriteLine("no agregar");
                Console.WriteLine("-----------" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                return false;
            }
                

            TimeProcess.Add(DateTime.Now);

            if (Records.ShouldFlushBeforeAdding())            
            {
                PersistData();
                Records.Clear();
                TimeProcess.Clear();
            }

            Console.WriteLine("CANTIDAD ANTES: {0}", Records.Count);
            ProcessModel proc = new ProcessModel(process);
            Console.WriteLine(proc);
            Records.Add(proc);
            Console.WriteLine("CANTIDAD DESPUES: {0}", Records.Count);
            Console.WriteLine("-----------" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            /*Console.WriteLine(TimeProcess[TimeProcess.Count-1]);
            
            if(Records.Count> 0)
            Console.WriteLine("Se agrego! comparandolo con el anterior count={0}. ids {1}=={2}. titulos {3}=={4}", Records.Count,
                    process.Id, Records[Records.Count - 1].Id, process.MainWindowTitle, Records[Records.Count - 1].Title);
            */


            return true;
        }
    }
}
