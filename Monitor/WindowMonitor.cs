using Newtonsoft.Json;
using Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Monitor
{
    public class WindowMonitor
    {

        class WindowMonitorConfig
        {            

            public List<string> AllowPolling { get; set; }
            public int HowManyChangesBeforeDiskWrite { get; set; }
            public int PollingSleep { get; set; }
            public int WriteDiskIntervalMinutes { get; set; }

            public void validateConfig()
            {
                if (PollingSleep < 1000)
                {
                    throw new ArgumentOutOfRangeException("Polling sleep time too low, " + PollingSleep + "ms");
                }
                
                if (HowManyChangesBeforeDiskWrite < 50)
                {
                    throw new ArgumentOutOfRangeException("In order to improve performance, there must be a number of window changes that are stored in memory before writing to disk. Number " + HowManyChangesBeforeDiskWrite + " is too low.");
                }

                if (WriteDiskIntervalMinutes < 1)
                {
                    throw new ArgumentOutOfRangeException("Writing to disk every " + WriteDiskIntervalMinutes + " minutes is too often.");
                }
            }
        }


        
        WindowMonitorConfig Config;
        RecordCollection RecordCollection;
        User32.WinEventDelegate Callback;
        Thread PersistInterval;

        public WindowMonitor(string configFile)
        {
            if (!File.Exists(configFile))
            {
                throw new System.InvalidOperationException("File " + configFile + " doesn't exist.");
            }

            Config = JsonConvert.DeserializeObject<WindowMonitorConfig>(File.ReadAllText(configFile));
            Config.validateConfig();

            /* Memory before writing to disk */
            RecordCollection = new RecordCollection(Config.HowManyChangesBeforeDiskWrite);

            PersistInterval = new Thread(PersistIntervalJob);
            PersistInterval.Start();

            Callback = new User32.WinEventDelegate(UpdateProcess);

            User32.SetClickHook(Callback);
            UpdateProcess();
        }

        void PersistIntervalJob()
        {
            while (true)
            {
                Thread.Sleep(Config.WriteDiskIntervalMinutes * 60 * 1000);
                Console.WriteLine("{0} Writing to disk (SQLite)", DateTime.Now);
                ForcePersistData();                
            }
        }

        public void ForcePersistData()
        {
            RecordCollection.PersistData();
        }


        void UpdateProcess()
        {            
            ProcessModel proc = new ProcessModel(User32.GetForegroundProcess());
            RecordCollection.AddRecord(proc);
        }


    }       

}

