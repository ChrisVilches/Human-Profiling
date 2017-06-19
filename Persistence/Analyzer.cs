using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class Analyzer
    {

        public Analyzer()
        {

        }

        /// <summary>
        /// Counts how many times each process has been switched to. The return is something like: chrome (100), mirc (58), git (12). But this doesn't mean Chrome
        /// is the most used process, it's only the one that has more switches to it.
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, int>> MostSwitchedProcess(DateTime start, DateTime end)
        {
            Console.WriteLine("most switched process, fechas {0} --> {1}", start, end);

            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            string sql = "SELECT COUNT(id) AS count, process_name FROM window WHERE date_time > @Start AND date_time < @End GROUP BY process_name ORDER BY count DESC;";
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            param.Add(new SQLiteParameter("@Start", DbType.DateTime) { Value = start });
            param.Add(new SQLiteParameter("@End", DbType.DateTime) { Value = end });

            var data = DB.GetInstance().SelectQuery(sql, param);

            while (data.Read())
            {
                list.Add(new Tuple<string, int>((string)data["process_name"], Convert.ToInt32(data["count"])));
            }
            return list;
        }

        public List<Tuple<string, int>> MostSwitchedProcess()
        {
            return MostSwitchedProcess(new DateTime(1970, 1, 1, 1, 1, 1), DateTime.Now.AddDays(1));
        }


        public List<Tuple<string, int>> MostUsedProcess(DateTime start, DateTime end)
        {
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            string sql = "SELECT SUM(seconds_used) AS total_seconds, process_name FROM window WHERE date_time > @Start AND date_time < @End GROUP BY process_name ORDER BY total_seconds DESC;";
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            param.Add(new SQLiteParameter("@Start", DbType.DateTime) { Value = start });
            param.Add(new SQLiteParameter("@End", DbType.DateTime) { Value = end });
            var data = DB.GetInstance().SelectQuery(sql, param);

            while (data.Read())
            {
                list.Add(new Tuple<string, int>((string)data["process_name"], Convert.ToInt32(data["total_seconds"])));
            }
            return list;
        }


        public List<Tuple<string, int>> MostUsedProcess()
        {
            return MostUsedProcess(new DateTime(1970, 1, 1, 1, 1, 1), DateTime.Now.AddDays(1));
        }


        public int CountAll()
        {
            string sql = "SELECT COUNT(*) FROM window;";
            return DB.GetInstance().CountQuery(sql);
        }

        public int CountSince(DateTime time)
        {
            string sql = "SELECT COUNT(*) FROM window WHERE date_time > ( @Today );";
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            param.Add(new SQLiteParameter("@Today", DbType.DateTime) { Value = time });
            return DB.GetInstance().CountQuery(sql, param);
        }


        public int CountToday()
        {
            return CountSince(DateTime.Now.AddDays(-1));
        }


    }
}
