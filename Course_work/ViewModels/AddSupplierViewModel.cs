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
    public class AddSupplierViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Supplier?> SaveSupplier { get; }
        public Supplier SupplierItem;
        public string mode = "insert";

        public AddSupplierViewModel()
        {
            SaveSupplier = ReactiveCommand.Create(() =>
            {
                SupplierItem = new Supplier();
                SupplierItem.SupplierName = SupplierName;
                SupplierItem.ContactInfo = ContactInfo;
                SupplierItem.Address = Address;
                return SupplierItem;
            });
        }

        private string _supplierName = "";
        public string SupplierName
        {
            get => _supplierName;
            set => this.RaiseAndSetIfChanged(ref _supplierName, value);
        }

        private string _contactInfo = "";
        public string ContactInfo
        {
            get => _contactInfo;
            set => this.RaiseAndSetIfChanged(ref _contactInfo, value);
        }

        private string _address = "";
        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }
    }
}
