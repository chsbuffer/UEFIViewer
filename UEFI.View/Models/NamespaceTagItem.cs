namespace UEFI.View.Models;

public record NamespaceTagItem
{
    public string Text { get; }
    public Guid Value { get; }

    public string? Define { get; }

    public NamespaceTagItem(Guid namespaceGuid)
    {
        var namespaceStr = namespaceGuid.ToString("B").ToUpper();
        Value = namespaceGuid;
        Define = UefiNamespaces.HumanReadableNames.GetValueOrDefault(namespaceStr);
        Text = Define != null ? $"{namespaceStr} | {Define}" : namespaceStr;
    }

    public override string ToString()
    {
        return Text;
    }
}