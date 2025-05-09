using System.Text;

namespace gorynych.common;

public class LogDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
    public override string ToString()
    {
        var sb = new StringBuilder(Count);
        foreach (var pair in this)
        {
            sb.Append($"{pair.Key}: {pair.Value}");
        }
        return sb.ToString();
    }
}