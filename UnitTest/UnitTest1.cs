using UEFI.Utils;
namespace UnitTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Util.ObtainPrivilege();

        var variables = Util.GetAllUEFIVariable();
        foreach (var item in variables)
        {
            Console.WriteLine($"{{{item.Key}}}");
            foreach (var variable in item.Value)
            {
                Console.WriteLine($"\t{variable}");
            }
        }
        Console.ReadLine();
    }

    [TestMethod]
    public void TestMethod2()
    {
        
    }
}