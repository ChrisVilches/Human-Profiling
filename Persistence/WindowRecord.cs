using System;
using System.Diagnostics;

namespace Persistence
{
    public class WindowRecord
    {
        public Process Process { get; private set; }
        public DateTime DateTime { get; private set; }

        public WindowRecord(Process p)
        {
            DateTime = DateTime.Now;
            Process = p;
        }

        public override string ToString()
        {
            string result = DateTime.ToString();
            result += Environment.NewLine;
            result += Process.MainWindowTitle;
            result += Environment.NewLine;
            result += Process.ProcessName;
            return result;
        }
    }
}
