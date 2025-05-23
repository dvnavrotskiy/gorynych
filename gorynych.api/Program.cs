using System.Reflection;
using gorynych.api.Dal.Migrations;
using gorynych.api.Helpers;
using gorynych.mq.Subscribers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
                     new OpenApiInfo
                     {
                         Title   = "Gorynych API",
                         Version = "v1"
                     }
        );
        
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
);

builder.Services.AddControllers();

builder.Services
    .AddTelemetry(builder.Configuration)
    .AddLogin(builder.Configuration)
    
    .AddGorMsgService(builder.Configuration)
    
    .AddMqBus(builder.Configuration)
    .AddSimplePublisher()
    .AddSimpleSubscriber()
    .AddAdvancedPublisher()
    .AddAdvancedSubscriber()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

MigrationRunner.Up(app.Configuration);

var simpleSubscriber = app.Services.GetRequiredService<SimpleSubscriber>();
simpleSubscriber.Subscribe();
var advancedSubscriber = app.Services.GetRequiredService<AdvancedSubscriber>();
advancedSubscriber.Subscribe();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GaMiddleware>();

app.MapControllers();
app.Run();

