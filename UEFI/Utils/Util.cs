using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Security;

using UEFI.Interops;

namespace UEFI.Utils
{
    public static unsafe class Util
    {
        public static void ObtainPrivilege()
        {
            TOKEN_PRIVILEGES tp = new();

            PInvoke.OpenProcessToken(Process.GetCurrentProcess().SafeHandle, TOKEN_ACCESS_MASK.TOKEN_ADJUST_PRIVILEGES | TOKEN_ACCESS_MASK.TOKEN_QUERY, out var tokenHandle);
            PInvoke.LookupPrivilegeValue(null, "SeSystemEnvironmentPrivilege", out tp.Privileges._0.Luid);
            tp.PrivilegeCount = 1;
            tp.Privileges._0.Attributes = TOKEN_PRIVILEGES_ATTRIBUTES.SE_PRIVILEGE_ENABLED;
            PInvoke.AdjustTokenPrivileges(tokenHandle, false, tp, 0, null, null);
        }

        public static Dictionary<Guid, List<string>> GetAllUEFIVariable()
        {
            const uint VARIABLE_INFORMATION_NAMES = 1;
            NativeMethods.NtEnumerateSystemEnvironmentValuesEx(VARIABLE_INFORMATION_NAMES, null, out var len);
            var buffer = new byte[len];
            NTSTATUS result;
            var dict = new Dictionary<Guid, List<string>>();

            fixed (void* p = buffer) result = NativeMethods.NtEnumerateSystemEnvironmentValuesEx(VARIABLE_INFORMATION_NAMES, p, out len);

            if (result.SeverityCode != NTSTATUS.Severity.Success)
                throw new Exception($"Error NTStatus: {(int)result}");

            var br = new BinaryReader(new MemoryStream(buffer));
            int nextOffset;

            while ((nextOffset = br.ReadInt32()) != 0)
            {
                var guid = new Guid(br.ReadBytes(16));
                var name = Encoding.Unicode.GetString(br.ReadBytes(nextOffset - 20));
                if (!dict.ContainsKey(guid))
                {
                    dict.Add(guid, new List<string>());
                }
                dict[guid].Add(name);
            }

            return dict;
        }

        public static string? GetVariableValueString(string namespaceGuid, string variableName, Encoding encoding)
        {
            var bytes = GetVariableValue(namespaceGuid, variableName);
            return bytes == null ? null : encoding.GetString(bytes);
        }

        public static byte[]? GetVariableValue(Guid namespaceGuid, string variableName) => GetVariableValue(namespaceGuid.ToString("B").ToUpper(), variableName);

        public static byte[]? GetVariableValue(string namespaceGuid, string variableName)
        {
            var size = 1024;

            while (true)
            {
                var buffer = new byte[size];
                uint len = 0;
                fixed (void* pBuffer = buffer)
                {
                    len = PInvoke.GetFirmwareEnvironmentVariable(variableName, namespaceGuid, pBuffer, (uint)buffer.Length);
                }

                if (len != 0)
                {
                    return buffer.Take((int)len).ToArray();
                }
                else
                {
                    var lastError = (WIN32_ERROR)Marshal.GetLastWin32Error();
                    switch (lastError)
                    {
                        case WIN32_ERROR.ERROR_ENVVAR_NOT_FOUND:
                            return null;
                        case WIN32_ERROR.ERROR_INSUFFICIENT_BUFFER:
                            // Fuck You Win32
                            size += 1024;
                            continue;
                        default:
                            throw new Exception($"Win32LastError {lastError}");
                    }
                }
            }
        }

        public static void SetVariableValue(string namespaceGuid, string variableName, byte[] bytes)
        {
            fixed (void* pBuffer = bytes)
            {
                var result = PInvoke.SetFirmwareEnvironmentVariable(variableName, namespaceGuid, pBuffer, (uint)bytes.Length);
                if (!result)
                {
                    var lastError = (WIN32_ERROR)Marshal.GetLastWin32Error();
                    throw new Exception($"Win32LastError {lastError}");
                }
            }
        }

        public static void DeleteVariable(string namespaceGuid, string variableName)
        {
            var result = PInvoke.SetFirmwareEnvironmentVariable(variableName, namespaceGuid, null, 0);
            if (!result)
            {
                var lastError = (WIN32_ERROR)Marshal.GetLastWin32Error();
                throw new Exception($"Win32LastError {lastError}");
            }
        }
    }
}