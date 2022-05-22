using System.Text;

using Util = UEFI.Utils.Util;
using ViewUtil = UEFI.View.Utils.Util;

namespace UEFI.View.Models;

public record UEFIVariableViewModel : UefiVariable
{
    public bool IsPrintableValue { get; private set; }

    public string ReadableValue => IsPrintableValue ? Encoding.ASCII.GetString(Value) : ViewUtil.BytesToHexString(Value);

    public string EditValue
    {
        get
        {
            return ViewUtil.BytesToHexString1(Value);
        }
        set
        {
            var bytes = ViewUtil.HexString1ToBytes(value);
            Util.SetVariableValue(NamespaceStr, Name, bytes);
        }
    }

    public UEFIVariableViewModel(Guid namespaceGuid, string name) : base(namespaceGuid, name)
    {
        IsPrintableValue = ViewUtil.IsPrintable(Value);
    }
}