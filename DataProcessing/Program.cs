using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using DataProcessing.Helpers;
using DataProcessing.Process;
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
    timer.Start();
    await ProcessAsync();
    string input;
    do
    {
        PrintMenu();
        input = Console.ReadLine();
        
        switch (input)
        {
            case "r":
                await ProcessAsync();
                break;
            case "s":
                Environment.Exit(0);
                break;
        }
        
    } while (string.IsNullOrEmpty(input) || !input.Equals("s"));
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

void PrintMenu()
{
    Console.WriteLine("Press r - to reset");
    Console.WriteLine("Press s - to stop");
}

async Task ProcessAsync()
{
    var processed = await fileProcessor.ProcessAsync(new List<string> { ".txt", ".csv" });
        
    Console.WriteLine(processed
        ? "Processed successfully"
        : "The file/files was/were not processed. Check a log file for more details");
}