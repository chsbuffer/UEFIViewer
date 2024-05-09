using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace UEFI.WindowsNT
{
    internal static unsafe partial class NativeMethods
    {
        [LibraryImport("ntdll", EntryPoint = "NtEnumerateSystemEnvironmentValuesEx")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static partial uint NtEnumerateSystemEnvironmentValuesEx(
            uint nInformationClass,
            void* lpBuffer,
            ref uint nSize);

        [LibraryImport("ntdll", EntryPoint = "NtQuerySystemEnvironmentValueEx")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static partial uint NtQuerySystemEnvironmentValueEx(
            [MarshalUsing(typeof(UnicodeStringMarshaller))]
            in string variableName,
            in Guid vendorGuid,
            void* lpValue,
            ref uint nSize,
            out uint attributes);

        [LibraryImport("ntdll", EntryPoint = "NtSetSystemEnvironmentValueEx")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static partial uint NtSetSystemEnvironmentValueEx(
            [MarshalUsing(typeof(UnicodeStringMarshaller))]
            in string variableName,
            in Guid vendorGuid,
            void* value,
            uint nSize,
            uint attributes);
    }
}