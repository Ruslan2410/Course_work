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
    public class AddSupplyViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Supply?> SaveSupply { get; }
        public Supply SupplyItem;
        public string mode = "insert";

        public AddSupplyViewModel()
        {
            SaveSupply = ReactiveCommand.Create(() =>
            {
                SupplyItem = new Supply();
                SupplyItem.ProductName = Name;
                SupplyItem.SupplierName = SupplierName;
                SupplyItem.Quantity = Quantity;
                SupplyItem.SupplyDate = DateTime.Now; // Assuming the current date is used for the supply date
                SupplyItem.SupplyAmount = Amount;
                return SupplyItem;
            });
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _supplierName = "";
        public string SupplierName
        {
            get => _supplierName;
            set => this.RaiseAndSetIfChanged(ref _supplierName, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => this.RaiseAndSetIfChanged(ref _quantity, value);
        }
        decimal _amount = 0;
        public decimal Amount
        {
            get { return _amount; }
            set { this.RaiseAndSetIfChanged(ref _amount, value); }
        }
    }
}
