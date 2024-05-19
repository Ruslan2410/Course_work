using Course_work.Items;
using Course_work.Views;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using static System.Collections.Specialized.BitVector32;

namespace Course_work
{
    public class Log_Fill : ObservableCollection<Log>
    {
        public void AddLog(
            int logID,
            string action,
            DateTime timestamp)
        {
            this.Add(new Log
            {
                LogID = logID,
                Action = action,
                Timestamp = timestamp
            });
        }

        public Log_Fill()
        {
            Fill_Logs();
        }

        public ObservableCollection<Log> Fill_Logs()
        {
            this.Clear();

            string connString = String.Format("Data Source={0};New=False;Version=3", MainWindow.mDBPath);
            SQLiteConnection sqlite_conn = new SQLiteConnection(connString);
            sqlite_conn.Open();

            string sql = String.Format("SELECT * FROM Logs;");

            SQLiteCommand sqlite_cmd = new SQLiteCommand(sql, sqlite_conn);
            try
            {
                SQLiteDataReader reader = (SQLiteDataReader)sqlite_cmd.ExecuteReader();
                while (reader.Read())
                {
                    int logID = Convert.ToInt32(reader["LogID"]);
                    string action = Convert.ToString(reader["Action"]);
                    DateTime timestamp = Convert.ToDateTime(reader["Timestamp"]);

                    AddLog(logID, action, timestamp);
                }
                reader.Close();
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return this;
        }
    }
}
