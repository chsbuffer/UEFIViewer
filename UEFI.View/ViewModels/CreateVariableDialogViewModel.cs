using System.Reactive;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.States;

using UEFI.View.Services;
using UEFI.View.Utils;

namespace UEFI.View.ViewModels;

public class CreateVariableDialogViewModel : ReactiveObject, IValidatableViewModel
{
    private readonly UEFIVariableService _variableService;

    public CreateVariableDialogViewModel()
    {
        _variableService = DI.GetRequiredService<UEFIVariableService>();

        var namespaceRule = this.ValidationRule(
            vm => vm.NamespaceStr,
            s => Guid.TryParse(s, out _), "Namespace must be a valid GUID.");

        this.ValidationRule(
            vm => vm.Name,
            s => !string.IsNullOrEmpty(s), "Variable name must not be empty.");
        IObservable<IValidationState> valueValidation =
            this.WhenAnyValue(x => x.ValueStr)
                .Throttle(TimeSpan.FromMilliseconds(200), RxApp.MainThreadScheduler)
                .Select(s =>
                {
                    if (string.IsNullOrWhiteSpace(s)) return new ValidationState(false, "Value must not be empty");
                    Value = null;
                    try
                    {
                        Value = Util.HexString1ToBytes(s);
                        if (!Value.Any())
                            return new ValidationState(false, "Value must not be empty");

                        return ValidationState.Valid;
                    }
                    catch (Exception e)
                    {
                        return new ValidationState(false, e.Message);
                    }
                });

        this.ValidationRule(
            vm => vm.ValueStr,
            valueValidation);

        this.WhenAnyValue(x => x.NamespaceStr)
            .WhereNotNull()
            .Select(s => UefiNamespaces.HumanReadableNames.GetValueOrDefault(s, ""))
            .ToPropertyEx(this, x => x.NamespaceDefine);

        CreateCommand = ReactiveCommand.CreateFromTask(CreateAsync, this.IsValid());
        NamespaceStr = UefiNamespaces.TESTING;
    }

    public IValidationContext ValidationContext { get; } = new ValidationContext();

    [Reactive] public string NamespaceStr { get; private set; }
    [ObservableAsProperty] public string NamespaceDefine { get; } = "";
    [Reactive] public string Name { get; private set; } = "";
    [Reactive] public string ValueStr { get; private set; } = "";
    [Reactive] public byte[]? Value { get; private set; }

    public ReactiveCommand<Unit, Unit> CreateCommand { get; private set; }

    private async Task CreateAsync()
    {
        var namespaceGuid = Guid.Parse(NamespaceStr);
        var bytes = Util.HexString1ToBytes(ValueStr);
        await _variableService.CreateVariableAsync(namespaceGuid, Name, bytes);
    }
}