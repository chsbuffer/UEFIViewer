using UEFI.Utils;

namespace UEFI
{
    public record UefiVariable
    {
        public Guid Namespace { get; init; }
        public string NamespaceStr { get; init; }
        public string Name { get; init; }
        public byte[] Value { get; set; }

        public UefiVariable(Guid namespaceGuid, string name)
        {
            Namespace = namespaceGuid;
            NamespaceStr = namespaceGuid.ToString("B").ToUpper();
            Name = name;
            Value = Util.GetVariableValue(Namespace, Name)!;
        }
    }
}