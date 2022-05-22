using System.Runtime.InteropServices;

using Windows.Win32.Foundation;

namespace UEFI.Interops;

static class NativeMethods
{

    [DllImport("ntdll", ExactSpelling = true, EntryPoint = "NtEnumerateSystemEnvironmentValuesEx", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern unsafe NTSTATUS NtEnumerateSystemEnvironmentValuesEx(ulong nInformationClass, void* lpBuffer, out ulong nSize);
}