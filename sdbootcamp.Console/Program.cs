using System.Text;

using UEFI;
using UEFI.Utils;

try
{
    Console.WriteLine("This is a tool for setting systemd-boot next boot entry.");

    var currentNextBoot = Util.GetVariableValueString(UefiNamespaces.SYSTEMD_BOOT, "LoaderEntryOneShot", Encoding.Unicode)?.Trim();
    var entries = Util.GetVariableValueString(UefiNamespaces.SYSTEMD_BOOT, "LoaderEntries", Encoding.Unicode)?.Split('\0').SkipLast(1).ToList() ?? new();

    Console.WriteLine($"Current next boot entry: {currentNextBoot ?? "Not set"}");

    Console.WriteLine("[0] Clear next boot.");
    for (int i = 0; i < entries.Count; i++)
    {
        Console.WriteLine($"[{i + 1}] {entries[i]}");
    }
    while (true)
    {
        Console.Write("Which to boot next time? ");
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
                Console.WriteLine("Invalid choice.");
            }
            else if (value == 0)
            {
                if (currentNextBoot == null)
                {
                    Console.WriteLine("You have not set next boot entry. Maybe you want to set one?");
                    continue;
                }

                Console.WriteLine($"Clear next boot.");
                Util.DeleteVariable(UefiNamespaces.SYSTEMD_BOOT, "LoaderEntryOneShot");
                Console.WriteLine("Done.");
                break;
            }
            else
            {
                var entry = entries[value - 1];
                Console.WriteLine($"Set next boot to \"{entry}\"");
                Util.SetVariableValue(UefiNamespaces.SYSTEMD_BOOT, "LoaderEntryOneShot", Encoding.Unicode.GetBytes(entry).Concat(new byte[]
                {
                    0, 0
                }).ToArray());
                Console.WriteLine("Done.");
                break;
            }
        }
    }
    Console.WriteLine("Press any key to exit or it will be closed automatically in 3 seconds.");
    Task.WaitAny(new[]
    {
        Task.Delay(TimeSpan.FromSeconds(3)), Task.Run(Console.ReadKey)
    });
}
catch (Exception e)
{
    Console.WriteLine("Some error occured: " + e);
    Console.ReadKey();
    throw;
}