using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Course_work.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;

namespace Course_work
{
    public partial class AddSupplier : ReactiveWindow<AddSupplierViewModel>
    {
        public AddSupplier()
        {
            InitializeComponent();
            this.WhenActivated(OnActivated);
            var bCancel = this.FindControl<Button>("bCancel");
            bCancel.Click += BCancel_Click;
        }

        private void OnActivated(CompositeDisposable disposables)
        {
            ViewModel!.SaveSupplier.Subscribe(Close).DisposeWith(disposables);
            if (ViewModel!.mode == "edit")
            {
                //ViewModel!.Name = ViewModel!.SupplierItem.Name;
            }
        }

        private void BCancel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(); // ��������� ������� ����
        }
    }
}
