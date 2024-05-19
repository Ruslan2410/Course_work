using Course_work.Items;
using Course_work.Views;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace Course_work
{
    public class Product_Fill : ObservableCollection<Product>
    {
        public void AddProduct(
            int productID,
            string productName,
            string category,
            decimal unitPrice,
            int quantityInStock)
        {
            this.Add(new Product
            {
                ProductID = productID,
                ProductName = productName,
                Category = category,
                UnitPrice = unitPrice,
                QuantityInStock = quantityInStock
            });
        }

        public Product_Fill()
        {
            Fill_Product();
        }

        public ObservableCollection<Product> Fill_Product()
        {
            this.Clear();

            string connString = String.Format("Data Source={0};New=False;Version=3", MainWindow.mDBPath);
            SQLiteConnection sqlite_conn = new SQLiteConnection(connString);
            sqlite_conn.Open();

            string sql = String.Format("SELECT * FROM Products;");

            SQLiteCommand sqlite_cmd = new SQLiteCommand(sql, sqlite_conn);
            try
            {
                SQLiteDataReader reader = (SQLiteDataReader)sqlite_cmd.ExecuteReader();
                while (reader.Read())
                {
                    int productID = Convert.ToInt32(reader["ProductID"]);
                    string productName = Convert.ToString(reader["ProductName"]);
                    string category = Convert.ToString(reader["Category"]);
                    decimal unitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    int quantityInStock = Convert.ToInt32(reader["QuantityInStock"]);

                    AddProduct(productID, productName, category, unitPrice, quantityInStock);
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
