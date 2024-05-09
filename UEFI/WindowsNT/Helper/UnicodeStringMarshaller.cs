using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace UEFI.WindowsNT;

internal unsafe ref struct UNICODE_STRING
{
    public ushort Length;
    public ushort MaximumLength;
    public char* Buffer;
}

// references: 
// https://learn.microsoft.com/en-us/dotnet/standard/native-interop/tutorial-custom-marshaller
// https://github.com/dotnet/runtime/tree/release/8.0/src/libraries/System.Private.CoreLib/src/System/Runtime/InteropServices/Marshalling
// https://github.com/dotnet/runtime/blob/main/docs/design/libraries/LibraryImportGenerator/UserTypeMarshallingV2.md

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
internal static unsafe class UnicodeStringMarshaller
{
    public ref struct ManagedToUnmanagedIn
    {
        private ushort len;
        private GCHandle pin;

        public void FromManaged(string managed)
        {
            pin = GCHandle.Alloc(managed, GCHandleType.Pinned); // 'fixed' keyword alternative
            len = (ushort)(managed.Length * sizeof(char));
        }

        public UNICODE_STRING ToUnmanaged()
        {
            return new UNICODE_STRING
            {
                Buffer = (char*)pin.AddrOfPinnedObject().ToPointer(), // indent
                Length = len,
                MaximumLength = len
            };
        }

        public void Free()
        {
            pin.Free();
        }
    }
}