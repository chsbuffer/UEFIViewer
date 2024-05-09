using System.Text.RegularExpressions;

using DynamicData.Kernel;

namespace UEFI.View.Utils;

internal static class Util
{
    public static bool IsPrintable(byte[] bytes)
    {
        return !bytes.Any(b => b is <= 31 or > 126);
    }

    public static string BytesToHexString(byte[] bytes)
    {
        return $"<{Regex.Replace(Convert.ToHexString(bytes), "(........)", "$1 ")}>";
    }

    public static string BytesToHexString1(IEnumerable<byte> bytes)
    {
        return string.Join(' ', bytes.Select(b => $"x{b:x2}"));
    }

    private static readonly char[] HexChars = "0123456789abcdefABCDEF".ToCharArray();

    public static byte[] HexString1ToBytes(string value)
    {
        return Convert.FromHexString(value.Replace("0x", "").Where(c => HexChars.Contains(c)).AsArray());
    }
}