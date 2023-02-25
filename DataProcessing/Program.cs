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

try
{
    var lineParser = new LineParser();
    var fileReaderFactory = new FileReaderFactory(lineParser);
    var directoryReader = new DirectoryReader(fileReaderFactory);
    
    var jsonSettings = new JsonSerializerSettings
    {
        DateFormatString = "yyyy-dd-MM",
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        },
        Formatting = Formatting.Indented
    };

    var fileProcessor = new FileProcessor(directoryReader, configuration);

    fileProcessor.Process(new List<string> { ".txt", ".csv" }, jsonSettings);

    Console.ReadLine();
}
catch (Exception ex)
{
    Log.Error(ex, ex.Message);
}
finally
{
    Log.CloseAndFlush();
}