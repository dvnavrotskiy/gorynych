namespace gorynych.common.Jaeger;

public sealed class JaegerConfig
{
    public string Host           { get; set; }
    public int    Port           { get; set; }
    public string ServiceName    { get; set; }
    public string ServiceVersion { get; set; }
}