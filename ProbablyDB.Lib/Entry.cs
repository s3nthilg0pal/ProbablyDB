using System;
using System.Buffers.Binary;
using System.Buffers;

public class Entry
{
    public byte[] Key { get; set; }
    public byte[] Value { get; set; }

    public byte[] Encode()
    {
        var length = 4 + 4 + Key.Length + Value.Length;
        var rented = ArrayPool<byte>.Shared.Rent(length);

        try
        {

        int offset = 0;

        //key
        Span<byte> keyBuffer = stackalloc byte[4];
        uint unsignedKeyLength = (uint)Key.Length;

        BinaryPrimitives.WriteUInt32LittleEndian(keyBuffer, unsignedKeyLength);

        keyBuffer.CopyTo(rented.AsSpan(offset));
        offset += keyBuffer.Length;

        //value
        Span<byte> valueBuffer = stackalloc byte[4];
        uint unsignedValueLength = (uint)Value.Length;

        BinaryPrimitives.WriteUInt32LittleEndian(valueBuffer, unsignedValueLength);

        valueBuffer.CopyTo(rented.AsSpan(offset));
        offset += valueBuffer.Length;

        //key
        Key.CopyTo(rented.AsSpan(offset));
        offset += Key.Length;

        //value
        Value.CopyTo(rented.AsSpan(offset));
        offset += Value.Length;

        return rented.AsSpan(0, offset).ToArray();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented);
        }
    }

    public void Decode(byte[] encodedData)
    {
        var sequence = new ReadOnlySequence<byte>(encodedData);
        var reader = new SequenceReader<byte>(sequence);

        reader.TryReadLittleEndian(out int keyLength);
        reader.TryReadLittleEndian(out int valueLength);

        var key  = new byte[keyLength];
        reader.TryCopyTo(key);
        reader.Advance(keyLength);
        Key = key;

        byte[] value = new byte[valueLength];
        reader.TryCopyTo(value);
        reader.Advance(valueLength);
        Value = value;
    }
}