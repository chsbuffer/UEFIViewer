using System.Reactive.Disposables;
using System.Reactive.Linq;

using ReactiveMarbles.ObservableEvents;

using ReactiveUI;
using ReactiveUI.Validation.Extensions;

using UEFI.View.Services;
using UEFI.View.Utils;
using UEFI.View.ViewModels;

namespace UEFI.View.Views;

public partial class CreateVariableDialog : IViewFor<CreateVariableDialogViewModel>
{
    public CreateVariableDialog()
    {
        InitializeComponent();
        Owner = DI.GetRequiredService<MainWindow>();
        ViewModel = new();

        this.WhenActivated(d =>
        {
            this.Bind(ViewModel,
                vm => vm.NamespaceStr,
                v => v.NamespaceComboBox.Text).DisposeWith(d);
            this.BindValidation(ViewModel,
                vm => vm.NamespaceStr,
                v => v.NamespaceError.Content).DisposeWith(d);
            this.OneWayBind(ViewModel,
                vm => vm.NamespaceDefine,
                v => v.NamespaceDefine.Content).DisposeWith(d);

            this.Bind(ViewModel,
                vm => vm.Name,
                v => v.VariableNameTextBox.Text).DisposeWith(d);
            this.BindValidation(ViewModel,
                vm => vm.Name,
                v => v.NameError.Content).DisposeWith(d);

            this.Bind(ViewModel,
                vm => vm.ValueStr,
                v => v.ValueTextBox.Text).DisposeWith(d);
            this.BindValidation(ViewModel,
                vm => vm.ValueStr,
                v => v.ValueError.Content).DisposeWith(d);
            this.OneWayBind(ViewModel,
                vm => vm.Value,
                v => v.ValuePreview.Content,
                bytes => bytes != null ? Util.BytesToHexString1(bytes) : "").DisposeWith(d);

            this.OneWayBind(ViewModel,
                vm => vm.ValidationContext.IsValid,
                v => v.IsPrimaryButtonEnabled).DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.CreateCommand,
                v => v,
                nameof(PrimaryButtonClick)
            ).DisposeWith(d);

            this.Events().PrimaryButtonClick
                .Select(x => x.args)
                .Subscribe(e =>
                {
                    ViewModel.CreateCommand.Execute().Subscribe(x => { }, err =>
                    {
                        this.CreateError.Content = err.Message;
                        e.Cancel = true;
                    }).DisposeWith(d);
                }).DisposeWith(d);
        });
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (CreateVariableDialogViewModel?)value;
    }

    public CreateVariableDialogViewModel? ViewModel { get; set; }
}