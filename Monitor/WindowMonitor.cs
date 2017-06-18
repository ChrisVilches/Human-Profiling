using Newtonsoft.Json;
using Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Monitor
{
    public class WindowMonitor
    {

        class WindowMonitorConfig
        {
            public List<string> AllowPolling { get; set; }
            public int HowManyChangesBeforeDiskWrite { get; set; }
            public int PollingSleep { get; set; }

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
            }
        }


        
        WindowMonitorConfig Config;
        Process CurrentProcess;
        RecordCollection RecordCollection;
        User32.WinEventDelegate Callback;

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

            Callback = new User32.WinEventDelegate(UpdateProcess);

            User32.SetClickHook(Callback);
            User32.SetForegroundHook(Callback);
            UpdateProcess();
        }


  
        void UpdateProcess()
        {
            CurrentProcess = User32.GetForegroundProcess();
            WindowRecord record = new WindowRecord(CurrentProcess);

            if (RecordCollection.AddRecord(record))
            {
                Console.WriteLine(record);
                Console.WriteLine("-----------------");
            }
                   
        }

    }       

}

