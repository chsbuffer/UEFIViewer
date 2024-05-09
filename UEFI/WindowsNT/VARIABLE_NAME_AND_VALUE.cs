using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using UEFI;

namespace UEFI.WindowsNT;

[StructLayout(LayoutKind.Sequential)]
public struct VARIABLE_NAME_AND_VALUE : INextOffset
{
    public uint NextEntryOffset;
    public uint ValueOffset;
    public uint ValueLength;
    public uint Attributes;
    public Guid VendorGuid;

    public char NameFirstChar;
    // WCHAR[ANY_SIZE] Name; 
    // BYTE[ValueLength] Value;

    public string Name
    {
        get
        {
            if (NameFirstChar == 0)
                throw new InvalidOperationException($"Do NOT create your own VARIABLE_NAME_AND_VALUE.");

            unsafe
            {
                return new string((char*)Unsafe.AsPointer(ref NameFirstChar));
            }
        }
    }

    public Span<byte> Value
    {
        get
        {
            if (ValueOffset == 0)
                throw new InvalidOperationException();

            unsafe
            {
                var ptr = (byte*)Unsafe.AsPointer(ref this);
                return new Span<byte>(ptr + ValueOffset, (int)ValueLength);
            }
        }
    }

    public readonly uint GetNextEntryOffset() => NextEntryOffset;
}