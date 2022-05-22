using DynamicData;

using UEFI.Utils;
using UEFI.View.Models;

namespace UEFI.View.Services;

internal class UEFIVariableService
{
    private readonly SourceList<Guid> _namespaces = new();
    private Dictionary<Guid, SourceList<UEFIVariableViewModel>> AllUefiVariables { get; set; } = null!;

    public IObservable<IChangeSet<Guid>> ConnectNamespace() => _namespaces.Connect();
    public IObservable<IChangeSet<UEFIVariableViewModel>>? ConnectVariables(Guid namespaceGuid) => AllUefiVariables.GetValueOrDefault(namespaceGuid)?.Connect();

    public async Task GetAndFillAllVariablesAsync()
    {
        var uefiVariables = await Task.Run(Util.GetAllUEFIVariable);
        FillAllVariables(uefiVariables);
    }

    private void FillAllVariables(Dictionary<Guid, List<string>> uefiVariables)
    {
        AllUefiVariables = uefiVariables
            .Select(pair =>
            {
                var sourceList = new SourceList<UEFIVariableViewModel>();
                sourceList.AddRange(pair.Value.Select(name => new UEFIVariableViewModel(pair.Key, name)));
                return new KeyValuePair<Guid, SourceList<UEFIVariableViewModel>>(pair.Key, sourceList);
            })
            .ToDictionary(x => x.Key, x => x.Value);

        var remove = _namespaces.Items.Except(AllUefiVariables.Keys).ToList();
        var add = AllUefiVariables.Keys.Except(_namespaces.Items).ToList();

        _namespaces.RemoveMany(remove);
        _namespaces.AddRange(add);
    }

    public async Task RemoveVariableAsync(UEFIVariableViewModel variable)
    {
        await Task.Run(() => Util.DeleteVariable(variable.NamespaceStr, variable.Name));

        var variables = AllUefiVariables[variable.Namespace];
        variables.Remove(variable);
        if (variables.Count == 0)
        {
            _namespaces.Remove(variable.Namespace);
            AllUefiVariables.Remove(variable.Namespace);
        }
    }

    public async Task CreateVariableAsync(Guid namespaceGuid, string name, byte[] bytes)
    {
        await Task.Run(() => Util.SetVariableValue(namespaceGuid.ToString("B"), name, bytes));
        if (!AllUefiVariables.ContainsKey(namespaceGuid))
        {
            _namespaces.Add(namespaceGuid);
            AllUefiVariables.Add(namespaceGuid, new SourceList<UEFIVariableViewModel>());
        }
        var variable = AllUefiVariables[namespaceGuid].Items.FirstOrDefault(v => v.Name == name);
        if (variable != null)
            variable.Value = bytes;
        else
            AllUefiVariables[namespaceGuid].Add(new UEFIVariableViewModel(namespaceGuid, name));
    }
}