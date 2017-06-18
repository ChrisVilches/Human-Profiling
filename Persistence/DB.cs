using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System;

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
        
    
        public void PersistRecordList(List<ProcessModel> processList, List<DateTime> dateTimes, List<long> seconds)
        {

            if(processList.Count != dateTimes.Count || dateTimes.Count != seconds.Count)
            {
                throw new InvalidOperationException("Wrong list length");
            }

            using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO window (process_name, process_title, date_time, seconds_used) VALUES (@Name, @Title, @Time, @Seconds);", Conn))
            using (SQLiteTransaction transaction = Conn.BeginTransaction())
            {

                for(int i=0; i<processList.Count; i++)
                {
                    cmd.Parameters.Add(new SQLiteParameter("@Name", processList[i].Name));
                    cmd.Parameters.Add(new SQLiteParameter("@Title", processList[i].Title));
                    cmd.Parameters.Add(new SQLiteParameter("@Time", DbType.DateTime) { Value = dateTimes[i] });
                    cmd.Parameters.Add(new SQLiteParameter("@Seconds", DbType.Double) { Value = seconds[i] });
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
                string sql = @"CREATE TABLE window(id INTEGER PRIMARY KEY AUTOINCREMENT, process_name VARCHAR(32), process_title VARCHAR(64), date_time DATETIME, seconds_used INTEGER);";
                SQLiteCommand cmd = new SQLiteCommand(sql, Conn);
                cmd.ExecuteNonQuery();
            }            
        }

        /// <summary>
        /// Counts how many times each process has been switched to. The return is something like: chrome (100), mirc (58), git (12). But this doesn't mean Chrome
        /// is the most used process, it's only the one that has more switches to it.
        /// </summary>
        /// <returns></returns>
        public object SelectProcessCount()
        {
            string sql = "SELECT COUNT(id) AS count, process_name FROM window GROUP BY process_name ORDER BY count DESC;";
            return null;
        }

    }
}

