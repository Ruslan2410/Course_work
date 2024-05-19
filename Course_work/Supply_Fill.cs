using Course_work.Items;
using Course_work.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_work
{
    public class Supply_Fill : ObservableCollection<Supply>
    {
        public void AddSupply(
            int supplyID,
            string productName,
            string supplierName,
            DateTime supplyDate,
            int quantity,
            decimal supplyAmount)
        {
            this.Add(new Supply
            {
                SupplyID = supplyID,
                ProductName = productName,
                SupplierName = supplierName,
                SupplyDate = supplyDate,
                Quantity = quantity,
                SupplyAmount = supplyAmount
            });
        }

        public Supply_Fill()
        {
            Fill_Supply();
        }

        public ObservableCollection<Supply> Fill_Supply()
        {
            this.Clear();

            string connString = String.Format("Data Source={0};New=False;Version=3", MainWindow.mDBPath);
            SQLiteConnection sqlite_conn = new SQLiteConnection(connString);
            sqlite_conn.Open();

            string sql = String.Format("SELECT * FROM Supply;");

            SQLiteCommand sqlite_cmd = new SQLiteCommand(sql, sqlite_conn);
            try
            {
                SQLiteDataReader reader = (SQLiteDataReader)sqlite_cmd.ExecuteReader();
                while (reader.Read())
                {
                    int supplyID = Convert.ToInt32(reader["SupplyID"]);
                    string productName = Convert.ToString(reader["ProductName"]);
                    string supplierName = Convert.ToString(reader["SupplierName"]);
                    DateTime supplyDate = Convert.ToDateTime(reader["SupplyDate"]);
                    int quantity = Convert.ToInt32(reader["Quantity"]);
                    decimal supplyAmount = Convert.ToDecimal(reader["SupplyAmount"]);

                    AddSupply(supplyID, productName, supplierName, supplyDate, quantity, supplyAmount);
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

