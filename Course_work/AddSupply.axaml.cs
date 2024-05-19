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
    public partial class AddSupply : ReactiveWindow<AddSupplyViewModel>
    {
        public AddSupply()
        {
            InitializeComponent();
            this.WhenActivated(OnActivated);
            var bCancel = this.FindControl<Button>("bCancel");
            bCancel.Click += BCancel_Click;
        }

        private void OnActivated(CompositeDisposable disposables)
        {
            ViewModel!.SaveSupply.Subscribe(Close).DisposeWith(disposables);
            if (ViewModel!.mode == "edit")
            {
                //ViewModel!.Name = ViewModel!.SupplyItem.Name;
            }
        }

        private void BCancel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(); // Закрываем текущее окно
        }
    }
}
