using System;
using System.Collections.Generic;
using Autofac;
using DataProcessing.Helpers;
using DataProcessing.Process;
using DataProcessing.Read;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = new ContainerBuilder();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.RegisterInstance(configuration)
    .As<IConfiguration>()
    .SingleInstance();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.RegisterType<LineParser>().As<ILineParser>().SingleInstance();

builder.Register(_ => new JsonSerializerSettings
{
    DateFormatString = "yyyy-dd-MM",
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    },
    Formatting = Formatting.Indented
}).SingleInstance();

builder.RegisterType<FileProcessor>().As<IFileProcessor>();
builder.RegisterType<MidnightTimer>().AsSelf().SingleInstance();

var container = builder.Build();
var jsonSettings = container.Resolve<JsonSerializerSettings>();
JsonConvert.DefaultSettings = () => jsonSettings;

var fileProcessor = container.Resolve<IFileProcessor>();
var timer = container.Resolve<MidnightTimer>();

try
{
    await fileProcessor.ProcessAsync(new List<string> { "txt", "csv" });
    timer.Start();

    Console.ReadLine();
}
catch (Exception ex)
{
    Log.Error(ex, ex.Message);
}
finally
{
    Log.CloseAndFlush();
    timer.Stop();
}