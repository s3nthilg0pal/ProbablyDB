using System;
using System.Collections.Generic;

public class KV
{
    public Dictionary<string, byte[]> Mem { get; set; }

     public void Open()
    {
        Mem = [];
    }

    public void Close(KV kv)
    {
        
    }

    public (byte[] Value, bool Found) Get(byte[] key)
    {
        var stringKey = BitConverter.ToString(key);
        
        var isFound = Mem.TryGetValue(stringKey, out byte[] currentValue);
        
        return (currentValue, isFound);
    } 

    public bool Set(byte[] key, byte[] value)
    {
        var stringKey = BitConverter.ToString(key);

        Mem[stringKey] = value;

        return true;
    }

    public bool Delete(byte[] key)
    {
        var stringKey = BitConverter.ToString(key);
        
        return Mem.Remove(stringKey);
    }
}
