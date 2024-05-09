namespace UEFI.Models
{
    public class Variable
    {
        public Variable(Guid @namespace, string name, byte[] value)
        {
            Namespace = @namespace;
            Name = name;
            Value = value;
        }

        public Guid Namespace { get; }
        public string Name { get; }

        public byte[] Value { get; set; }
    }
}
