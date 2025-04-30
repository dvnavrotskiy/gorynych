namespace gorynych.api.Services;

public sealed class RabbitSenderService
{
    public void Send(string message)
    {
        var msg = new MyMessage {Text = message};
    }
}

public sealed class MyMessage
{
    public string Text { get; set; }
}