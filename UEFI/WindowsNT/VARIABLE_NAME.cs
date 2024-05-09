using System.Runtime.CompilerServices;

namespace UEFI.WindowsNT;

public struct VARIABLE_NAME : INextOffset
{
    public uint NextEntryOffset;
    public Guid VendorGuid;
    public char NameFirstChar;

    public uint GetNextEntryOffset() => NextEntryOffset;

    public string Name
    {
        get
        {
            if (NameFirstChar == 0)
                throw new InvalidOperationException($"Do NOT create your own VARIABLE_NAME.");

            unsafe
            {
                return new string((char*)Unsafe.AsPointer(ref NameFirstChar));
            }
        }
    }
}