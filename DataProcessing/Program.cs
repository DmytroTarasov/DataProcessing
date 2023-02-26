using System;
using System.Collections.Generic;
using DataProcessing.Helpers;
using DataProcessing.Process;
using DataProcessing.Read;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var lineParser = new LineParser();

var jsonSettings = new JsonSerializerSettings
{
    DateFormatString = "yyyy-dd-MM",
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    },
    Formatting = Formatting.Indented
};

JsonConvert.DefaultSettings = () => jsonSettings;

var fileProcessor = new FileProcessor(lineParser, configuration);
var timer = new MidnightTimer(lineParser, configuration);

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