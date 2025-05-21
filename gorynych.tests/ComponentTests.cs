using System.Reflection;
using gorynych.api.Commands;
using gorynych.api.Contracts;
using gorynych.api.Dal;
using gorynych.api.Helpers;
using gorynych.api.Services;
using gorynych.mq;
using gorynych.mq.Subscribers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace gorynych.tests;

public class ComponentTests
{
    private const string RabbitCs = "host=localhost";

    private readonly ServiceProvider serviceProvider;

    public ComponentTests()
    {
        var services = new ServiceCollection();

        var repo = new InMemoryMessageRepo();
        
        var assembly = Assembly.GetAssembly(typeof(GorMsgReaderService));
        
        services
            .AddLogging(logging => logging.AddConsole())
            .AddSingleton<IMessageRepo>(repo)
            .AddSingleton<IGorMsgWriter, GorMsgWriterService>()
            .AddSingleton<IGorMsgReader, GorMsgReaderService>()
            .AddSingleton(BusFactory.Create(RabbitCs))
            .AddSimplePublisher()
            .AddSimpleSubscriber()
            .AddAdvancedPublisher()
            .AddAdvancedSubscriber()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        
        serviceProvider = services.BuildServiceProvider();

        var simpleSubscriber = serviceProvider.GetRequiredService<SimpleSubscriber>();
        simpleSubscriber.Subscribe();
        var advancedSubscriber = serviceProvider.GetRequiredService<AdvancedSubscriber>();
        advancedSubscriber.Subscribe();
    }

    public abstract class CommandFactory
    {
        public abstract IRequest<GorMsg> GetCommand(GorMsg msg);
    }

    private sealed class SimpleCommandFactory : CommandFactory
    {
        public override IRequest<GorMsg> GetCommand(GorMsg msg)
            => new PublishMessageCommand(msg);
    }
    
    private sealed class AdvancedCommandFactory : CommandFactory
    {
        public override IRequest<GorMsg> GetCommand(GorMsg msg)
            => new AdvancedPublishMessageCommand(msg);
    }
    
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new SimpleCommandFactory() },
            new object[] { new AdvancedCommandFactory() }
        };
    
    [Theory]
    [MemberData(nameof(Data))]
    public async Task SimpleUseCaseTest(CommandFactory commandFactory)
    {
        var ct = CancellationToken.None;
        
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var repo = serviceProvider.GetRequiredService<IMessageRepo>();

        var guid = Guid.NewGuid();
        var msg = new GorMsg
        {
            Message = guid.ToString(),
            RequestId = guid,
            Timestamp = DateTime.UtcNow,
        };
        
        await mediator.Send(commandFactory.GetCommand(msg), ct);
        
        var p = new Paging { Page = 1, PageSize = 10 };

        IList<GorMsg> msgList = new List<GorMsg>();
        for (var i = 0; i < 10; ++i)
        {
            msgList = await repo.GetMessages(p, ct);
            if (msgList.Count > 0)
                break;
            await Task.Delay(100, ct);
        }
        Assert.Contains(guid, msgList.Select(x => x.RequestId));
    }
}