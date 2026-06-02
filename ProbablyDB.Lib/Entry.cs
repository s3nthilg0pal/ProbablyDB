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
        byte[] result = new byte[length];

        int offset = 0;

        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(offset, 4), Key.Length);

        offset += 4;

        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(offset, 4), Value.Length);

        offset += 4;

        Key.CopyTo(result.AsSpan(offset));
        offset += Key.Length;

        Value.CopyTo(result.AsSpan(offset));


        return result;
    }

    public void Decode(byte[] encodedData)
    {
        var sequence = new ReadOnlySequence<byte>(encodedData);
        var reader = new SequenceReader<byte>(sequence);

        if (!reader.TryReadLittleEndian(out int keyLength) ||
            !reader.TryReadLittleEndian(out int valueLength))
        {
            throw new FormatException("Entry header is incomplete.");
        }

        if (keyLength < 0 || valueLength < 0)
        {
            throw new FormatException("Entry lengths are invalid.");
        }

        if (reader.Remaining < (long)keyLength + valueLength)
        {
            throw new FormatException("Entry payload is incomplete.");
        }

        var key = new byte[keyLength];
        if (!reader.TryCopyTo(key))
        {
            throw new FormatException("Entry key is incomplete.");
        }
        reader.Advance(keyLength);
        Key = key;

        var value = new byte[valueLength];
        if (!reader.TryCopyTo(value))
        {
            throw new FormatException("Entry value is incomplete.");
        }
        reader.Advance(valueLength);
        Value = value;
    }
}