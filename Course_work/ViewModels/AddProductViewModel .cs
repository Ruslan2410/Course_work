using Course_work.Items;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Course_work.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Product?> SaveProduct { get; }
        public Product ProductItem;
        public string mode = "insert";

        public AddProductViewModel()
        {
            SaveProduct = ReactiveCommand.Create(() =>
            {
                ProductItem = new Product();
                ProductItem.ProductName = Name;
                ProductItem.Category = Category;
                ProductItem.UnitPrice = Price;
                ProductItem.QuantityInStock = Quantity;
                return ProductItem;
            });
        }

        string _name = "";
        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        string _category = "";
        public string Category
        {
            get { return _category; }
            set { this.RaiseAndSetIfChanged(ref _category, value); }
        }

        decimal _price = 0;
        public decimal Price
        {
            get { return _price; }
            set { this.RaiseAndSetIfChanged(ref _price, value); }
        }

        int _quantity = 0;
        public int Quantity
        {
            get { return _quantity; }
            set { this.RaiseAndSetIfChanged(ref _quantity, value); }
        }
    }
}
