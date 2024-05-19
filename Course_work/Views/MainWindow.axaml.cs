using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Course_work.Items;
using Course_work.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;
using System;

namespace Course_work.Views
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public static String mPath = AppDomain.CurrentDomain.BaseDirectory;
        public static String mDBPath = mPath + "Sales_department.db";

        private async Task DoShowDialogAsyncP(InteractionContext<AddProductViewModel, Product?> interaction)
        {
            var dialog = new AddProduct();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<Product?>(this);
            interaction.SetOutput(result);
        }

        private async Task DoShowDialogAsyncS(InteractionContext<AddSupplierViewModel, Supplier?> interaction)
        {
            var dialog = new AddSupplier();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<Supplier?>(this);
            interaction.SetOutput(result);
        }

        private async Task DoShowDialogAsyncSy(InteractionContext<AddSupplyViewModel, Supply?> interaction)
        {
            var dialog = new AddSupply();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<Supply?>(this);
            interaction.SetOutput(result);
        }

        public MainWindow()
        {
            InitializeComponent();           

            this.WhenActivated(action =>
                action(ViewModel!.ShowDialogp.RegisterHandler(DoShowDialogAsyncP)));

            this.WhenActivated(action =>
                action(ViewModel!.ShowDialogs.RegisterHandler(DoShowDialogAsyncS)));

            this.WhenActivated(action =>
                action(ViewModel!.ShowDialogss.RegisterHandler(DoShowDialogAsyncSy)));
        }

        
    }
}

