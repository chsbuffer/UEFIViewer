using System.Text;

using UEFI;

try
{

    Console.WriteLine("This is a tool for setting systemd-boot next boot entry.");

    var currentNextBoot =
        EfiVariables.Get(UefiNamespaces.SYSTEMD_BOOT.ToGuid(), "LoaderEntryOneShot")?.GetString().Trim();

    var entries = EfiVariables.Get(UefiNamespaces.SYSTEMD_BOOT.ToGuid(), "LoaderEntries")?.GetString()
        .Split('\0').SkipLast(1).ToList();

    if (entries == null || entries.Count==0)
    {
        Console.WriteLine("");
        return;
    }

    Console.WriteLine($"Current next boot entry: {currentNextBoot ?? "Not set"}");

    Console.WriteLine("[0] Clear next boot.");
    for (int i = 0; i < entries.Count; i++)
    {
        Console.WriteLine($"[{i + 1}] {entries[i]}");
    }

    while (true)
    {
        Console.Write("Which to boot next time? (number, otherwise exit)");
        var choice = Console.ReadLine();
        if (!int.TryParse(choice, out var value))
        {
            Console.WriteLine("Bye.");
            break;
        }
        else
        {
            if (value < 0 || value > entries.Count)
            {
                Console.WriteLine("Choice out of range.");
            }
            else if (value == 0)
            {
                if (currentNextBoot == null)
                {
                    Console.WriteLine("You have not set next boot entry.");
                    break;
                }

                Console.WriteLine($"Clear next boot.");
                EfiVariables.Delete(UefiNamespaces.SYSTEMD_BOOT, "LoaderEntryOneShot");
                Console.WriteLine("Done.");
                break;
            }
            else
            {
                var entry = entries[value - 1];
                Console.WriteLine($"Set next boot to \"{entry}\"");
                EfiVariables.Set(UefiNamespaces.SYSTEMD_BOOT, "LoaderEntryOneShot",
                    Encoding.Unicode.GetBytes(entry).Concat(new byte[] { 0, 0 }).ToArray());
                Console.WriteLine("Done.");
                break;
            }
        }
    }

    Console.WriteLine("Press any key or wait 3 seconds to exit.");
    Task.WaitAny(new[] { Task.Delay(TimeSpan.FromSeconds(3)), Task.Run(Console.ReadKey) });
}
catch (Exception e)
{
    Console.WriteLine("Some error occured: " + e);
    Console.ReadKey();
    throw;
}

internal static class Helper
{
    public static string GetString(this byte[] b) => Encoding.Unicode.GetString(b);

    public static Guid ToGuid(this string str) => Guid.Parse(str);
}