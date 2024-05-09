using System.Text;

using UEFI.Models;

using ViewUtil = UEFI.View.Utils.Util;

namespace UEFI.View.Models;

public class UEFIVariableViewModel : Variable
{
    public string NamespaceStr => Namespace.ToString("B").ToUpper();

    public bool IsPrintableValue => ViewUtil.IsPrintable(Value);

    public string ReadableValue =>
        IsPrintableValue ? Encoding.ASCII.GetString(Value) : ViewUtil.BytesToHexString(Value);

    public string EditValue
    {
        get
        {
            return ViewUtil.BytesToHexString1(Value);
        }
        set
        {
            var bytes = ViewUtil.HexString1ToBytes(value);
            EfiVariables.Set(NamespaceStr, Name, bytes);
        }
    }

    public UEFIVariableViewModel(Guid namespaceGuid, string name, byte[] value) : base(namespaceGuid, name, value)
    {
    }

    public UEFIVariableViewModel(Variable variable) : base(variable.Namespace, variable.Name, variable.Value)
    {
    }
}