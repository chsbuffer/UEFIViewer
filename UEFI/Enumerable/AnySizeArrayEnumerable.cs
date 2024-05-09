using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UEFI;

public interface INextOffset
{
    public uint GetNextEntryOffset();
}

public unsafe class AnySizeArrayEnumerable<T> : IDisposable where T : INextOffset
{
    internal AnySizeArrayEnumerable(void* buffer)
    {
        Buffer = buffer;
    }

    internal readonly void* Buffer;

    public AnySizeArrayEnumerator<T> GetEnumerator()
    {
        return new AnySizeArrayEnumerator<T>(this);
    }

    public void Dispose()
    {
        NativeMemory.Free(Buffer);
    }
}

public unsafe ref struct AnySizeArrayEnumerator<T>(AnySizeArrayEnumerable<T> enumerable) where T : INextOffset
{
    private ref T _current = ref Unsafe.AsRef<T>(enumerable.Buffer);

    public bool MoveNext()
    {
        var offset = _current.GetNextEntryOffset();
        if (offset == 0)
            return false;

        _current = ref Unsafe.AddByteOffset(ref _current, offset);
        return true;
    }

    public readonly ref T Current
    {
        get => ref _current;
    }
}