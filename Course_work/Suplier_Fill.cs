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
    public class Supplier_Fill : ObservableCollection<Supplier>
    {
        public void AddSupplier(
            int supplierID,
            string supplierName,
            string contactInfo,
            string address)
        {
            this.Add(new Supplier
            {
                SupplierID = supplierID,
                SupplierName = supplierName,
                ContactInfo = contactInfo,
                Address = address
            });
        }

        public Supplier_Fill()
        {
            Fill_Supplier();
        }

        public ObservableCollection<Supplier> Fill_Supplier()
        {
            this.Clear();

            string connString = String.Format("Data Source={0};New=False;Version=3", MainWindow.mDBPath);
            SQLiteConnection sqlite_conn = new SQLiteConnection(connString);
            sqlite_conn.Open();

            string sql = String.Format("SELECT * FROM Suppliers;");

            SQLiteCommand sqlite_cmd = new SQLiteCommand(sql, sqlite_conn);
            try
            {
                SQLiteDataReader reader = (SQLiteDataReader)sqlite_cmd.ExecuteReader();
                while (reader.Read())
                {
                    int supplierID = Convert.ToInt32(reader["SupplierID"]);
                    string supplierName = Convert.ToString(reader["SupplierName"]);
                    string contactInfo = Convert.ToString(reader["ContactInfo"]);
                    string address = Convert.ToString(reader["Address"]);

                    AddSupplier(supplierID, supplierName, contactInfo, address);
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
