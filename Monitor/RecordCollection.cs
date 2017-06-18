using System.Collections.Generic;
using System.Diagnostics;
using Persistence;

namespace Monitor
{
    class RecordCollection
    {
        List<WindowRecord> Records;

        public RecordCollection(int capacity)
        {
            Records = new List<WindowRecord>(capacity);

        }

        void PersistData()
        {
            DB.GetInstance().PersistRecordList(Records);
        }

        public bool AddRecord(Process process)
        {
            return AddRecord(new WindowRecord(process));
        }

        public bool AddRecord(WindowRecord windowRecord)
        {
            /* if (new process == last added process) */
            if (Records.Count > 0 &&
                windowRecord.Process.Id == Records[Records.Count - 1].Process.Id &&
                windowRecord.Process.MainWindowTitle.Equals(Records[Records.Count - 1].Process.MainWindowTitle))
                return false;

            if (Records.Count == Records.Capacity)
            {
                PersistData();
                Records.Clear();
            }

            Records.Add(windowRecord);
            return true;
        }
    }
}
