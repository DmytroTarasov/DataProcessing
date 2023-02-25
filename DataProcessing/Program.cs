using DataProcessing;
using DataProcessing.Read;
using DataProcessing.Save;
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

    var fileSaver = new FileSaver();

    var fileProcessor = new FileProcessor(directoryReader, fileSaver, configuration, jsonSettings);

    fileProcessor.Process();

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