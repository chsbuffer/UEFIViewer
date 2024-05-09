using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

using UEFI;
using UEFI.WindowsNT;

namespace UnitTest;

[TestClass]
public class UnitTest1
{
    private const string Vendor = UefiNamespaces.TESTING;
    private const string Name = "TEST";
    readonly ReadOnlyMemory<byte> ExampleValue = Encoding.Unicode.GetBytes("HelloWorld");

    [TestMethod]
    public void TestCreateDelete()
    {
        // Create
        EfiVariables.Set(Vendor, Name, ExampleValue.Span);
        // Get
        var value = EfiVariables.Get(Vendor, Name);
        Assert.IsNotNull(value);
        Assert.IsTrue(value.SequenceEqual(ExampleValue.ToArray()));
        // Delete
        EfiVariables.Delete(Vendor, Name);
        Assert.IsNull(EfiVariables.Get(Vendor, Name));
    }

    [TestMethod]
    public void TestDelete()
    {
        EfiVariables.Delete(Vendor, Name);

        Assert.IsNull(EfiVariables.Get(Vendor, Name));
    }

    [TestMethod]
    public void TestWriteAndRead()
    {
        EfiVariables.Set(Vendor, Name, ExampleValue.Span);

        var variable = EfiVariables.Get(Vendor, Name);
        Assert.IsNotNull(variable);
        Assert.IsTrue(variable.SequenceEqual(ExampleValue.ToArray()));
    }

    [TestMethod]
    public void TestReadNotExist()
    {
        var variable = EfiVariables.Get(Vendor, Path.GetTempFileName());
        Assert.IsNull(variable);
    }

    [TestMethod]
    public void GetValues()
    {
        var enumable = EfiVariables.GetValues();

        foreach (ref var item in enumable)
        {
            Span<byte> value = item.Value;
            var valueStr = value.Length > 20 ? (Convert.ToHexString(value[..20]) + "...") : Convert.ToHexString(value);

            Console.WriteLine($"""
                               Namespace: {item.VendorGuid}
                               Name: {item.Name}
                               Value: {valueStr}

                               """);
        }

        Console.WriteLine();
    }

    [TestMethod]
    public void GetNames()
    {
        var enumable = EfiVariables.GetNames();

        foreach (ref var item in enumable)
        {
            Console.WriteLine($"""
                               Namespace: {item.VendorGuid}
                               Name: {item.Name}

                               """);
        }

        Console.WriteLine();
    }
}