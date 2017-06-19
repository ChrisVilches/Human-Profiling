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

            if (processList.Count == 0) return;

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

        public SQLiteDataReader SelectQuery(string sql)
        {
            using (SQLiteCommand cmd = Conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteReader();
            }
        }

        public int CountQuery(string sql)
        {
            return CountQuery(sql, null);
        }

        public int CountQuery(string sql, List<SQLiteParameter> parameters)
        {
            using (SQLiteCommand cmd = Conn.CreateCommand())
            {
                cmd.CommandText = sql;
                
                if(parameters != null)
                {
                    foreach(SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}

