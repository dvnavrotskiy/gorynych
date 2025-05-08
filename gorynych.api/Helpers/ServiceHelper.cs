using gorynych.api.Dal;
using gorynych.api.Services;
using gorynych.auth.Dal;
using gorynych.auth.Services;
using gorynych.common.Jaeger;
using gorynych.mq;
using gorynych.mq.Publishers;
using gorynych.mq.Subscribers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace gorynych.api.Helpers;

public static class ServiceHelper
{
    public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration cfg)
    {
        var jaegerConfig = new JaegerConfig();
        cfg.GetSection("JaegerConfig").Bind(jaegerConfig);
        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService("gorynych.api"))
            .WithTracing(
                t => t.AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    /*.AddJaegerExporter(
                        o =>
                        {
                            o.AgentHost = jaegerConfig.Host;
                            o.AgentPort = jaegerConfig.Port;
                        }
                    )*/
            );
        return services;
    }

    public static IServiceCollection AddLogin(this IServiceCollection services, IConfiguration cfg)
    {
        var connectionString = cfg.GetConnectionString("usersConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("User Connection string not found");
        return services
            .AddSingleton<ILoginRepo>(new LoginFileRepo(connectionString))
            .AddSingleton<LoginService>();
    }

    public static IServiceCollection AddGorMsgService(this IServiceCollection services, IConfiguration cfg)
    {
        var repo = cfg.GetValue<bool>("InMemoryMessages")
            ? new InMemoryMessageRepo()
            : SqlFactory();

        services.AddSingleton(repo);
        
        return services
            .AddSingleton<IGorMsgWriter, GorMsgWriterService>()
            .AddSingleton<IGorMsgReader, GorMsgReaderService>();

        IMessageRepo SqlFactory()
        {
            var connectionString = cfg.GetConnectionString("messagesConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Messages Connection string not found");
            return new SqlLiteMessageRepo(connectionString);
        }
    }

    public static IServiceCollection AddMqBus(this IServiceCollection services, IConfiguration cfg)
    {
        var connectionString = cfg.GetConnectionString("rabbitConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Rabbit Connection string not found");
        return services
            .AddSingleton(BusFactory.Create(connectionString));
    }
    
    public static IServiceCollection AddSimplePublisher(this IServiceCollection services)
    {
        return services
            .AddSingleton<SimplePublisher>();
    }
    
    public static IServiceCollection AddSimpleSubscriber(this IServiceCollection services)
    {
        return services
            .AddSingleton<SimpleSubscriber>();
    }
    
    public static IServiceCollection AddAdvancedPublisher(this IServiceCollection services)
    {
        return services
            .AddSingleton<AdvancedPublisher>();
    }
    
    public static IServiceCollection AddAdvancedSubscriber(this IServiceCollection services)
    {
        return services
            .AddSingleton<AdvancedSubscriber>();
    }
}