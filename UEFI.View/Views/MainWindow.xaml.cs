using System.Reactive.Disposables;
using System.Reactive.Linq;

using ModernWpf.Controls;

using ReactiveMarbles.ObservableEvents;

using ReactiveUI;

using UEFI.View.Models;
using UEFI.View.ViewModels;

namespace UEFI.View.Views
{
    public partial class MainWindow
    {
        // https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font
        private const int Lock = 59182;
        private const int Unlock = 59269;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new WindowViewModel();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.Namespaces,
                    v => v.NamespaceComboBox.ItemsSource).DisposeWith(d);

                this.Bind(ViewModel,
                    vm => vm.Namespace,
                    v => v.NamespaceComboBox.SelectedValue
                ).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    vm => vm.Variables,
                    v => v.VariablesListView.ItemsSource).DisposeWith(d);

                this.Bind(ViewModel,
                    vm => vm.EditingVariable,
                    v => v.VariablesListView.SelectedItem, nameof(ListView.SelectionChanged)).DisposeWith(d);

                this.VariablesListView.Events().SelectionChanged
                    .SubscribeOn(RxApp.MainThreadScheduler)
                    .Subscribe(x =>
                    {
                        VariableValueTextBox.Text = VariablesListView.SelectedItem is UEFIVariableViewModel uefiVariableVm ? uefiVariableVm.ReadableValue : "";
                    }).DisposeWith(d);

                this.BindCommand(ViewModel,
                    vm => vm.ToggleUnlockCommand,
                    v => v.UnlockButton).DisposeWith(d);
                ViewModel.WhenAnyValue(vm => vm.Locked)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(b =>
                    {
                        ((SymbolIcon)UnlockButton.Icon).Symbol = (Symbol)(b ? Lock : Unlock);
                        UnlockButton.Label = b ? "Locked" : "Unlocked";
                        AddButton.IsEnabled = RemoveButton.IsEnabled = !b;
                        VariableValueTextBox.IsReadOnly = b;
                    }).DisposeWith(d);

                this.BindCommand(ViewModel,
                    vm => vm.AddVariableCommand,
                    v => v.AddButton
                ).DisposeWith(d);

                this.BindCommand(ViewModel,
                    vm => vm.RemoveVariableCommand,
                    v => v.RemoveButton,
                    withParameter: vm => vm.EditingVariable
                ).DisposeWith(d);

                this.BindCommand(ViewModel,
                    vm => vm.RefreshAllVariablesCommand,
                    v => v.RefreshButton
                ).DisposeWith(d);
            });
        }
    }
}