namespace ProbablyDB.Lib.Tests;

public class KVTest
{
    [Fact]
    public void BasicSetGetDelete()
    {
        var kv = new KV();
        kv.Open();
        try
        {
            var updated = kv.Set(Bytes("k1"), Bytes("v1"));
            Assert.True(updated);

            var (value, found) = kv.Get(Bytes("k1"));
            Assert.True(found);
            Assert.Equal(Bytes("v1"), value);

            (_, found) = kv.Get(Bytes("xxx"));
            Assert.False(found);

            updated = kv.Delete(Bytes("xxx"));
            Assert.False(updated);

            updated = kv.Delete(Bytes("k1"));
            Assert.True(updated);

            (_, found) = kv.Get(Bytes("xxx"));
            Assert.False(found);
        }
        finally
        {
            kv.Close(kv);
        }
    }

    private static byte[] Bytes(string value) => System.Text.Encoding.UTF8.GetBytes(value);
}
