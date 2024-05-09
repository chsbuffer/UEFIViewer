using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;

using UEFI.WindowsNT;

namespace UEFI;

public static unsafe class EfiVariables
{
    [DoesNotReturn]
    internal static void ThrowWin32Error(int error)
    {
        throw new Win32Exception(error, $"HRESULT 0x{error:x8}");
    }

    [DoesNotReturn]
    internal static void ThrowWin32Error()
    {
        var error = Marshal.GetLastPInvokeError();
        throw new Win32Exception(error, $"HRESULT 0x{error:x8}");
    }

    [DoesNotReturn]
    internal static void ThrowNtError(NTSTATUS stat, [CallerMemberName] string caller = "")
    {
        throw new Win32Exception($"NTStatus 0x{stat.Value:x8}");
    }

    public static AnySizeArrayEnumerable<VARIABLE_NAME> GetNames()
    {
        Win32NativeMethods.ObtainPrivilege();

        // get size
        uint len = 0;
        var result = (NTSTATUS)NativeMethods.NtEnumerateSystemEnvironmentValuesEx(
            (uint)InformationClass.VARIABLE_INFORMATION_NAMES, null, ref len);

        if (result != 0xC0000023) // STATUS_BUFFER_TOO_SMALL
        {
            ThrowNtError(result);
        }

        // real call
        var p = NativeMemory.Alloc(len);
        result = (NTSTATUS)NativeMethods.NtEnumerateSystemEnvironmentValuesEx(
            (uint)InformationClass.VARIABLE_INFORMATION_NAMES, p, ref len);

        if (result.SeverityCode != NTSTATUS.Severity.Success)
            ThrowNtError(result);

        return new AnySizeArrayEnumerable<VARIABLE_NAME>(p);
    }

    public static AnySizeArrayEnumerable<VARIABLE_NAME_AND_VALUE> GetValues()
    {
        Win32NativeMethods.ObtainPrivilege();

        // get size
        uint len = 0;
        var result = (NTSTATUS)NativeMethods.NtEnumerateSystemEnvironmentValuesEx(
            (uint)InformationClass.VARIABLE_INFORMATION_VALUES, null, ref len);
        if (result != 0xC0000023) // STATUS_BUFFER_TOO_SMALL
        {
            ThrowNtError(result);
        }

        // real call
        var p = NativeMemory.Alloc(len);
        result = (NTSTATUS)NativeMethods.NtEnumerateSystemEnvironmentValuesEx(
            (uint)InformationClass.VARIABLE_INFORMATION_VALUES, p, ref len);

        if (result.SeverityCode != NTSTATUS.Severity.Success)
            ThrowNtError(result);

        return new AnySizeArrayEnumerable<VARIABLE_NAME_AND_VALUE>(p);
    }

    public static byte[]? Get(string vendorGuid, string variableName) =>
        Get(Guid.Parse(vendorGuid), variableName, out _);

    public static byte[]? Get(Guid vendorGuid, string variableName) => Get(vendorGuid, variableName, out _);

    public static byte[]? Get(Guid vendorGuid, string variableName, out uint attributes)
    {
        Win32NativeMethods.ObtainPrivilege();

        uint size = 0;
        attributes = 0;
        var result = (NTSTATUS)NativeMethods.NtQuerySystemEnvironmentValueEx(variableName, vendorGuid, null,
            ref size, out _);

        if (result == 0xC0000100) // STATUS_VARIABLE_NOT_FOUND
            return null;

        if (result != 0xC0000023) // STATUS_BUFFER_TOO_SMALL
            ThrowNtError(result);

        var buffer = new byte[size];
        fixed (void* ptr = buffer)
            result = (NTSTATUS)NativeMethods.NtQuerySystemEnvironmentValueEx(variableName, vendorGuid, ptr,
                ref size, out attributes);

        if (result.SeverityCode != NTSTATUS.Severity.Success) ThrowNtError(result);

        return buffer;
    }

    public static void Set(string namespaceGuid, string variableName, ReadOnlySpan<byte> bytes)
    {
        Win32NativeMethods.ObtainPrivilege();

        fixed (void* pBuffer = bytes)
        {
            var result =
                PInvoke.SetFirmwareEnvironmentVariable(variableName, namespaceGuid, pBuffer, (uint)bytes.Length);
            if (!result) ThrowWin32Error();
        }
    }

    public static void Delete(string namespaceGuid, string variableName)
    {
        Win32NativeMethods.ObtainPrivilege();

        var result = PInvoke.SetFirmwareEnvironmentVariable(variableName, namespaceGuid, null, 0);
        if (!result)
        {
            var error = Marshal.GetLastPInvokeError();
            if (error == 0xcb) // ERROR_ENVVAR_NOT_FOUND
                return;

            ThrowWin32Error(error);
        }
    }
}