using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace Persistence
{
    public class DB
    {

        SQLiteConnection Conn;
        const string DatabaseName = "data.db";
        public static DB Instance;

        DB()
        {          
        }

        public static DB GetInstance()
        {
            if(Instance == null)
            {
                Instance = new DB();
                Instance.EnsureDatabaseReady();
            }
            return Instance;
        }

        public void PersistRecordList(List<WindowRecord> list)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO window (process_name, process_title, date_time) VALUES (@Name, @Title, @Time);", Conn))
            using (SQLiteTransaction transaction = Conn.BeginTransaction())
            {
                foreach (WindowRecord record in list)
                {
                    cmd.Parameters.Add(new SQLiteParameter("@Name", record.Process.ProcessName));
                    cmd.Parameters.Add(new SQLiteParameter("@Title", record.Process.MainWindowTitle));
                    cmd.Parameters.Add(new SQLiteParameter("@Time", DbType.DateTime) { Value = record.DateTime });
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        void EnsureDatabaseReady()
        {
            /* If DB file doesn't exist, needs to create tables = true */
            bool createTable = !File.Exists(DatabaseName);

            Conn = new SQLiteConnection("Data Source=" + DatabaseName);
            Conn.Open();

            if (createTable)
            {
                string sql = @"CREATE TABLE window(id INTEGER PRIMARY KEY AUTOINCREMENT, process_name VARCHAR(32), process_title VARCHAR(64), date_time DATETIME);";
                SQLiteCommand cmd = new SQLiteCommand(sql, Conn);
                cmd.ExecuteNonQuery();
            }            
            
        }

    }
}
