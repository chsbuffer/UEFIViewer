using System.ComponentModel;
using System.Diagnostics;

using Windows.Win32;
using Windows.Win32.Security;

namespace UEFI;

internal static unsafe class Win32NativeMethods
{
    private static bool ObtainPrivilegeCalledOnce;

    internal static void ObtainPrivilege()
    {
        if (ObtainPrivilegeCalledOnce)
            return;

        ObtainPrivilegeCalledOnce = true;

        TOKEN_PRIVILEGES tp = new();
        PInvoke.OpenProcessToken(Process.GetCurrentProcess().SafeHandle,
            TOKEN_ACCESS_MASK.TOKEN_ADJUST_PRIVILEGES | TOKEN_ACCESS_MASK.TOKEN_QUERY, out var tokenHandle);
        PInvoke.LookupPrivilegeValue(null, "SeSystemEnvironmentPrivilege", out tp.Privileges._0.Luid);
        tp.PrivilegeCount = 1;
        tp.Privileges._0.Attributes = TOKEN_PRIVILEGES_ATTRIBUTES.SE_PRIVILEGE_ENABLED;
        var resuslt = PInvoke.AdjustTokenPrivileges(tokenHandle, false, tp, 0, null, null);
        if (!resuslt)
            throw new Win32Exception();
    }
}