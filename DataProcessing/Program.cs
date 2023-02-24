using DataProcessing;
using DataProcessing.Read;
using DataProcessing.Save;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var lineParser = new LineParser();
var fileReaderFactory = new FileReaderFactory(lineParser);
var directoryReader = new DirectoryReader(fileReaderFactory);

IConfiguration configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

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