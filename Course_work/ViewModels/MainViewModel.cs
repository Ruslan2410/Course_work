using Course_work.Items;
using Course_work.ViewModels;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Course_work.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Product_Fill TheProduct { get; } = new Product_Fill();
        public Supplier_Fill TheSupplier { get; } = new Supplier_Fill();
        public Supply_Fill TheSupply { get; } = new Supply_Fill();
        public Log_Fill TheLog { get; } = new Log_Fill();
        
        public Interaction<AddProductViewModel, Product?> ShowDialogp { get; }
        public Interaction<AddSupplierViewModel, Supplier?> ShowDialogs { get; }
        public Interaction<AddSupplyViewModel, Supply?> ShowDialogss { get; }
        

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ICommand AddSupplierCommand { get; }
        public ICommand EditSupplierCommand { get; }
        public ICommand DeleteSupplierCommand { get; }

        public ICommand AddSupplyCommand { get; }
        public ICommand EditSupplyCommand { get; }
        public ICommand DeleteSupplyCommand { get; }

        public ICommand SearchCommand { get; }

        public ICommand SearchProductCommand { get; }
        public ICommand ClearDataCommand { get; }

        private ObservableCollection<Product> _filteredProducts;
        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set => this.RaiseAndSetIfChanged(ref _filteredProducts, value);
        }

        private string _searchProductName;
        public string SearchProductName
        {
            get => _searchProductName;
            set => this.RaiseAndSetIfChanged(ref _searchProductName, value);
        }

        private string _searchProductCategory;
        public string SearchProductCategory
        {
            get => _searchProductCategory;
            set => this.RaiseAndSetIfChanged(ref _searchProductCategory, value);
        }

        private string _searchMinPrice;
        public string SearchMinPrice
        {
            get => _searchMinPrice;
            set => this.RaiseAndSetIfChanged(ref _searchMinPrice, value);
        }

        private string _searchMaxPrice;
        public string SearchMaxPrice
        {
            get => _searchMaxPrice;
            set => this.RaiseAndSetIfChanged(ref _searchMaxPrice, value);
        }

        public ICommand SearchSupplierCommand { get; }
        public ICommand ClearDataSupplierCommand { get; }

        private ObservableCollection<Supplier> _filteredSuppliers;
        public ObservableCollection<Supplier> FilteredSuppliers
        {
            get => _filteredSuppliers;
            set => this.RaiseAndSetIfChanged(ref _filteredSuppliers, value);
        }

        private string _searchSupplierName;
        public string SearchSupplierName
        {
            get => _searchSupplierName;
            set => this.RaiseAndSetIfChanged(ref _searchSupplierName, value);
        }

        private string _searchContactInfo;
        public string SearchContactInfo
        {
            get => _searchContactInfo;
            set => this.RaiseAndSetIfChanged(ref _searchContactInfo, value);
        }

        public ICommand SearchSupplyCommand { get; }
        public ICommand ClearDataSupplyCommand { get; }

        private ObservableCollection<Supply> _filteredSupplies;
        public ObservableCollection<Supply> FilteredSupplies
        {
            get => _filteredSupplies;
            set => this.RaiseAndSetIfChanged(ref _filteredSupplies, value);
        }

        private string _searchProduct_SupplyName;
        public string SearchProduct_SupplyName
        {
            get => _searchProduct_SupplyName;
            set => this.RaiseAndSetIfChanged(ref _searchProduct_SupplyName, value);
        }

        private string _searchSupplier_SupplyName;
        public string SearchSupplier_SupplyName
        {
            get => _searchSupplier_SupplyName;
            set => this.RaiseAndSetIfChanged(ref _searchSupplier_SupplyName, value);
        }

        private DateTime? _searchStartDate;
        public DateTime? SearchStartDate
        {
            get => _searchStartDate;
            set => this.RaiseAndSetIfChanged(ref _searchStartDate, value);
        }

        private DateTime? _searchEndDate;
        public DateTime? SearchEndDate
        {
            get => _searchEndDate;
            set => this.RaiseAndSetIfChanged(ref _searchEndDate, value);
        }

        public MainViewModel()
        {           
            ShowDialogp = new Interaction<AddProductViewModel, Product?>();
            ShowDialogs = new Interaction<AddSupplierViewModel, Supplier?>();
            ShowDialogss = new Interaction<AddSupplyViewModel, Supply?>();

            AddProductCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var product = new AddProductViewModel();
                product.mode = "insert";
                var result = await ShowDialogp.Handle(product);
                if (result != null)
                {
                    string insertProductSql = string.Format("INSERT INTO Products (ProductName, Category, UnitPrice, QuantityInStock) " +
                        "VALUES ('{0}', '{1}', '{2}', '{3}');",
                        result.ProductName, result.Category, result.UnitPrice, result.QuantityInStock);

                    // Логування операції додавання продукту
                    string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Added product: {0}', '{1}');",
                        result.ProductName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Database.Exec_SQL(insertProductSql);
                    Database.Exec_SQL(logSql);
                    TheProduct.Fill_Product();
                    TheLog.Fill_Logs();
                    FilterProducts();
                }
            });

            EditProductCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectItemp != null)
                {
                    var product = new AddProductViewModel();
                    product.mode = "edit";
                    product.ProductItem = SelectItemp;
                    var result = await ShowDialogp.Handle(product);
                    if (result != null)
                    {
                        string updateProductSql = string.Format("UPDATE Products SET " +
                            "ProductName = '{0}', " +
                            "Category = '{1}', " +
                            "UnitPrice = '{2}', " +
                            "QuantityInStock = '{3}' " +
                            "WHERE ProductID = '{4}';",
                            result.ProductName, result.Category, result.UnitPrice, result.QuantityInStock, SelectItemp.ProductID);

                        // Логування операції редагування продукту
                        string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Edited product: {0}', '{1}');",
                            result.ProductName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        Database.Exec_SQL(updateProductSql);
                        Database.Exec_SQL(logSql);
                        TheProduct.Fill_Product();
                        TheLog.Fill_Logs();
                        FilterProducts();
                    }
                }
            });

            DeleteProductCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectItemp != null)
                {
                    string deleteProductSql = string.Format("DELETE FROM Products WHERE ProductID = '{0}';", SelectItemp.ProductID);

                    // Логування операції видалення продукту
                    string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Deleted product: {0}', '{1}');",
                        SelectItemp.ProductName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Database.Exec_SQL(deleteProductSql);
                    Database.Exec_SQL(logSql);
                    TheProduct.Fill_Product();
                    TheLog.Fill_Logs();
                    FilterProducts();
                }
            });

            AddSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var supplier = new AddSupplierViewModel();
                supplier.mode = "insert";
                var result = await ShowDialogs.Handle(supplier);
                if (result != null)
                {
                    string insertSupplierSql = string.Format("INSERT INTO Suppliers (SupplierName, ContactInfo, Address) " +
                        "VALUES ('{0}', '{1}', '{2}');",
                        result.SupplierName, result.ContactInfo, result.Address);

                    // Логування операції додавання постачальника
                    string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Added supplier: {0}', '{1}');",
                        result.SupplierName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Database.Exec_SQL(insertSupplierSql);
                    Database.Exec_SQL(logSql);
                    TheSupplier.Fill_Supplier();
                    TheLog.Fill_Logs();
                }
            });
            EditSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectItems != null)
                {
                    var supplier = new AddSupplierViewModel();
                    supplier.mode = "edit";
                    supplier.SupplierItem = SelectItems;
                    var result = await ShowDialogs.Handle(supplier);
                    if (result != null)
                    {
                        string updateSupplierSql = string.Format("UPDATE Suppliers SET " +
                            "SupplierName = '{0}', " +
                            "ContactInfo = '{1}', " +
                            "Address = '{2}' " +
                            "WHERE SupplierID = '{3}';",
                            result.SupplierName, result.ContactInfo, result.Address, SelectItems.SupplierID);

                        // Логування операції редагування постачальника
                        string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Edited supplier: {0}', '{1}');",
                            result.SupplierName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        Database.Exec_SQL(updateSupplierSql);
                        Database.Exec_SQL(logSql);
                        TheSupplier.Fill_Supplier();
                        TheLog.Fill_Logs();
                    }
                }
            });

            DeleteSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectItems != null)
                {
                    string deleteSupplierSql = string.Format("DELETE FROM Suppliers WHERE SupplierID = '{0}';", SelectItems.SupplierID);

                    // Логування операції видалення постачальника
                    string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Deleted supplier: {0}', '{1}');",
                        SelectItems.SupplierName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Database.Exec_SQL(deleteSupplierSql);
                    Database.Exec_SQL(logSql);
                    TheSupplier.Fill_Supplier();
                    TheLog.Fill_Logs();
                }
            });



            AddSupplyCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var supply = new AddSupplyViewModel();
                supply.mode = "insert";
                var result = await ShowDialogss.Handle(supply);
                if (result != null)
                {
                    string insertSupplySql = string.Format("INSERT INTO Supply (ProductName, SupplierName, SupplyDate, Quantity, SupplyAmount) " +
                        "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');",
                        result.ProductName, result.SupplierName, result.SupplyDate.ToString("yyyy-MM-dd"), result.Quantity, result.SupplyAmount);

                    // Логування операції додавання поставки
                    string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Added supply: {0} from {1}', '{2}');",
                        result.ProductName, result.SupplierName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Database.Exec_SQL(insertSupplySql);
                    Database.Exec_SQL(logSql);
                    TheSupply.Fill_Supply();
                    TheLog.Fill_Logs();
                }
            });

            EditSupplyCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectItemss != null)
                {
                    var supply = new AddSupplyViewModel();
                    supply.mode = "edit";
                    supply.SupplyItem = SelectItemss;
                    var result = await ShowDialogss.Handle(supply);
                    if (result != null)
                    {
                        string updateSupplySql = string.Format("UPDATE Supply SET " +
                            "ProductName = '{0}', " +
                            "SupplierName = '{1}', " +
                            "SupplyDate = '{2}', " +
                            "Quantity = '{3}', " +
                            "SupplyAmount = '{4}' " +
                            "WHERE SupplyID = '{5}';",
                            result.ProductName, result.SupplierName, result.SupplyDate.ToString("yyyy-MM-dd"), result.Quantity, result.SupplyAmount, SelectItemss.SupplyID);

                        // Логування операції редагування поставки
                        string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Edited supply: {0} from {1}', '{2}');",
                            result.ProductName, result.SupplierName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        Database.Exec_SQL(updateSupplySql);
                        Database.Exec_SQL(logSql);
                        TheSupply.Fill_Supply();
                        TheLog.Fill_Logs();
                    }
                }
            });

            DeleteSupplyCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectItemss != null)
                {
                    string deleteSupplySql = string.Format("DELETE FROM Supply WHERE SupplyID = '{0}';", SelectItemss.SupplyID);

                    // Логування операції видалення поставки
                    string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Deleted supply: {0} from {1}', '{2}');",
                        SelectItemss.ProductName, SelectItemss.SupplierName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Database.Exec_SQL(deleteSupplySql);
                    Database.Exec_SQL(logSql);
                    TheSupply.Fill_Supply();
                    TheLog.Fill_Logs();
                }
            });

            SearchProductCommand = ReactiveCommand.Create(FilterProducts);
            ClearDataCommand = ReactiveCommand.Create(() =>
            {
                SearchProductName = string.Empty;
                SearchProductCategory = string.Empty;
                SearchMinPrice = null;
                SearchMaxPrice = null;
                FilterProducts();
            });

            FilterProducts();

            SearchSupplierCommand = ReactiveCommand.Create(FilterSuppliers);
            ClearDataSupplierCommand = ReactiveCommand.Create(() =>
            {
                SearchSupplierName = string.Empty;
                SearchContactInfo = string.Empty;
                FilterSuppliers();
            });

            FilterSuppliers();

            SearchSupplyCommand = ReactiveCommand.Create(FilterSupplies);
            ClearDataSupplyCommand = ReactiveCommand.Create(() =>
            {
                SearchProductName = string.Empty;
                SearchSupplierName = string.Empty;
                SearchStartDate = null;
                SearchEndDate = null;
                FilterSupplies();
            });

            FilterSupplies();           
        }
        Product _selectItemp = null;
        public Product SelectItemp
        {
            get { return _selectItemp; }
            set
            {
                if (_selectItemp != value)
                {
                    this.RaiseAndSetIfChanged(ref _selectItemp, value);
                }
            }
        }

        Supplier _selectItems = null;
        public Supplier SelectItems
        {
            get { return _selectItems; }
            set
            {
                if (_selectItems != value)
                {
                    this.RaiseAndSetIfChanged(ref _selectItems, value);
                }
            }
        }

        Supply _selectItemss = null;
        public Supply SelectItemss
        {
            get { return _selectItemss; }
            set
            {
                if (_selectItemss != value)
                {
                    this.RaiseAndSetIfChanged(ref _selectItemss, value);
                }
            }
        }
        private void FilterProducts()
        {
            var filtered = TheProduct.Where(p =>
            {
                bool matchesName = string.IsNullOrEmpty(SearchProductName) || p.ProductName.Contains(SearchProductName, StringComparison.OrdinalIgnoreCase);
                bool matchesCategory = string.IsNullOrEmpty(SearchProductCategory) || p.Category.Contains(SearchProductCategory, StringComparison.OrdinalIgnoreCase);
                bool matchesMinPrice = string.IsNullOrEmpty(SearchMinPrice) || p.UnitPrice >= decimal.Parse(SearchMinPrice);
                bool matchesMaxPrice = string.IsNullOrEmpty(SearchMaxPrice) || p.UnitPrice <= decimal.Parse(SearchMaxPrice);

                return matchesName && matchesCategory && matchesMinPrice && matchesMaxPrice;
            }).ToList();

            FilteredProducts = new ObservableCollection<Product>(filtered);
        }
        private void FilterSuppliers()
        {
            var filtered = TheSupplier.Where(s =>
            {
                bool matchesName = string.IsNullOrEmpty(SearchSupplierName) || s.SupplierName.Contains(SearchSupplierName, StringComparison.OrdinalIgnoreCase);
                bool matchesContactInfo = string.IsNullOrEmpty(SearchContactInfo) || s.ContactInfo.Contains(SearchContactInfo, StringComparison.OrdinalIgnoreCase);

                return matchesName && matchesContactInfo;
            }).ToList();

            FilteredSuppliers = new ObservableCollection<Supplier>(filtered);
        }
        private void FilterSupplies()
        {
            var filtered = TheSupply.Where(s =>
            {
                bool matchesProductName = string.IsNullOrEmpty(SearchProductName) || s.ProductName.Contains(SearchProductName, StringComparison.OrdinalIgnoreCase);
                bool matchesSupplierName = string.IsNullOrEmpty(SearchSupplierName) || s.SupplierName.Contains(SearchSupplierName, StringComparison.OrdinalIgnoreCase);
                bool matchesStartDate = !SearchStartDate.HasValue || s.SupplyDate >= SearchStartDate.Value;
                bool matchesEndDate = !SearchEndDate.HasValue || s.SupplyDate <= SearchEndDate.Value;

                return matchesProductName && matchesSupplierName && matchesStartDate && matchesEndDate;
            }).ToList();

            //FilteredSupplies = new ObservableCollection<Supply>(filtered);
        }
    }
}
