using Course_work.Views;
using System;
using System.Data.SQLite;

namespace Course_work
{
    internal class Database
    {
        public static bool Exec_SQL(string sql)
        {
            bool result = false;

            string connString = $"Data Source={MainWindow.mDBPath};New=False;Version=3";
            using (SQLiteConnection sqlite_conn = new SQLiteConnection(connString))
            {
                try
                {
                    sqlite_conn.Open();
                    using (SQLiteCommand sqlite_cmd = new SQLiteCommand(sql, sqlite_conn))
                    {
                        sqlite_cmd.ExecuteNonQuery();
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    result = false;
                }
            }

            return result;
        }
    }
}
