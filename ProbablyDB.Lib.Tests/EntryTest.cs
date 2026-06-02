namespace ProbablyDB.Lib.Tests;

public class EntryTest
{
    [Fact]
    public void EntryEncode()
    {
        var entry = new Entry { Key = Bytes("k1"), Value = Bytes("xxx") };
        byte[] data = [2, 0, 0, 0, 3, 0, 0, 0, (byte)'k', (byte)'1', (byte)'x', (byte)'x', (byte)'x'];

        Assert.Equal(data, entry.Encode());

        var decoded = new Entry();
        decoded.Decode(data);

        Assert.Equal(entry.Key, decoded.Key);
        Assert.Equal(entry.Value, decoded.Value);
    }

    private static byte[] Bytes(string value) => System.Text.Encoding.UTF8.GetBytes(value);
}
