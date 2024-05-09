using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using UEFI.View.Models;
using UEFI.View.Services;
using UEFI.View.Views;

namespace UEFI.View.ViewModels
{
    public class WindowViewModel : ReactiveObject
    {
        private readonly UEFIVariableService _variableService;

        private readonly ReadOnlyObservableCollection<NamespaceTagItem> _namespaces;

        public WindowViewModel()
        {
            _variableService = DI.GetRequiredService<UEFIVariableService>();

            _variableService.ConnectNamespace()
                .Transform(guid => new NamespaceTagItem(guid))
                .Sort(SortExpressionComparer<NamespaceTagItem>.Ascending(o => o.Define ?? "").ThenByAscending(o => o.Text))
                .Bind(out _namespaces)
                .Subscribe();

            _variableService.GetAndFillAllVariables();

            this.ToggleUnlockCommand = ReactiveCommand.Create(() => { Locked = !Locked; });
            this.AddVariableCommand = ReactiveCommand.Create(() =>
            {
                var dialog = new CreateVariableDialog();
                dialog.ShowAsync();
            });
            var canRemove = this.WhenAnyValue(x => x.EditingVariable).Select(x => x != null);
            this.RemoveVariableCommand = ReactiveCommand.CreateFromTask<UEFIVariableViewModel>(_variableService.RemoveVariableAsync, canRemove);
            this.RefreshAllVariablesCommand = ReactiveCommand.Create(_variableService.GetAndFillAllVariables);

            // ReSharper disable once InvokeAsExtensionMethod
            Observable.Merge(
                    this.WhenAnyValue(x => x.Namespace),
                    this.RefreshAllVariablesCommand
                        .AsObservable()
                        .Select(_ => this.Namespace)
                )
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    Variables.Clear();
                    if (x.HasValue)
                        _variableService.ConnectVariables(x.Value)?
                            .Bind(Variables)
                            .Subscribe();
                });

            Locked = true;
        }

        [Reactive] public bool Locked { get; set; }

        public ReadOnlyObservableCollection<NamespaceTagItem> Namespaces => _namespaces;

        [Reactive] public Guid? Namespace { set; get; } = Guid.Parse(UefiNamespaces.EFI_GLOBAL_VARIABLE);

        public IObservableCollection<UEFIVariableViewModel> Variables { get; } = new ObservableCollectionExtended<UEFIVariableViewModel>();

        [Reactive] public UEFIVariableViewModel? EditingVariable { get; set; }

        public ReactiveCommand<Unit, Unit> ToggleUnlockCommand { get; }
        public ReactiveCommand<Unit, Unit> AddVariableCommand { get; set; }
        public ReactiveCommand<UEFIVariableViewModel, Unit> RemoveVariableCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshAllVariablesCommand { get; }
    }
}