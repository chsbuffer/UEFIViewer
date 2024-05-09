using DynamicData;

using UEFI.Models;
using UEFI.View.Models;

namespace UEFI.View.Services;

internal class UEFIVariableService
{
    private readonly SourceList<Guid> _namespaces = new();
    private Dictionary<Guid, SourceList<UEFIVariableViewModel>> AllUefiVariables { get; set; } = null!;

    public IObservable<IChangeSet<Guid>> ConnectNamespace() => _namespaces.Connect();

    public IObservable<IChangeSet<UEFIVariableViewModel>>? ConnectVariables(Guid namespaceGuid) =>
        AllUefiVariables.GetValueOrDefault(namespaceGuid)?.Connect();

    public void GetAndFillAllVariables()
    {
        var uefiVariables = new List<Variable>();
        var enumable = EfiVariables.GetValues();
        foreach (ref var variable in enumable)
            uefiVariables.Add(new Variable(variable.VendorGuid, variable.Name, variable.Value.ToArray()));

        FillAllVariables(uefiVariables);
    }

    private void FillAllVariables(List<Variable> uefiVariables)
    {
        AllUefiVariables = uefiVariables
            .Select(x => new UEFIVariableViewModel(x))
            .GroupBy(x => x.Namespace)
            .ToDictionary(g => g.Key, g =>
            {
                var l = new SourceList<UEFIVariableViewModel>();
                l.AddRange(g);
                return l;
            });

        var remove = _namespaces.Items.Except(AllUefiVariables.Keys).ToList();
        var add = AllUefiVariables.Keys.Except(_namespaces.Items).ToList();

        _namespaces.RemoveMany(remove);
        _namespaces.AddRange(add);
    }

    public async Task RemoveVariableAsync(UEFIVariableViewModel variable)
    {
        EfiVariables.Delete(variable.NamespaceStr, variable.Name);

        var list = AllUefiVariables[variable.Namespace];
        list.Remove(variable);
        if (list.Count == 0)
        {
            _namespaces.Remove(variable.Namespace);
            AllUefiVariables.Remove(variable.Namespace);
        }
    }

    public async Task CreateVariableAsync(Guid namespaceGuid, string name, byte[] value)
    {
        EfiVariables.Set(namespaceGuid.ToString("B"), name, value);

        if (!AllUefiVariables.ContainsKey(namespaceGuid))
        {
            _namespaces.Add(namespaceGuid);
            AllUefiVariables.Add(namespaceGuid, new SourceList<UEFIVariableViewModel>());
        }

        var variable = AllUefiVariables[namespaceGuid].Items.FirstOrDefault(v => v.Name == name);

        if (variable == null)
        {
            AllUefiVariables[namespaceGuid].Add(new UEFIVariableViewModel(namespaceGuid, name, value));
        }
        else
        {
            variable.Value = value;
        }
    }
}