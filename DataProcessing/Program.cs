using DataProcessing.Helpers;
using DataProcessing.Read;
using DataProcessing.Save;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var directory = @"C:\Dima\Radency\Files";

var lineParser = new LineParser();
var fileReaderFactory = new FileReaderFactory(lineParser);
var directoryReader = new DirectoryReader(fileReaderFactory);

var payers = directoryReader.ReadFilesInDirectory(new DirectoryInfo(directory));
var payersList = payers.ToList();
foreach (var payer in payersList)
{
  Console.WriteLine($"{payer.FullName} {payer.City} {payer.Payment} {payer.Date} {payer.AccountNumber } {payer.Service}");  
}

var parsedFiles = lineParser.ParsedFiles;
foreach (var pair in parsedFiles)
{
  Console.WriteLine($"{pair.Key}: {pair.Value}");
}

Console.WriteLine(lineParser.ErrorsCount);

var outputDirectory = @"C:\Dima\Radency\Results";

var saver = new FileSaver();

var jsonSettings = new JsonSerializerSettings
{
  DateFormatString = "yyyy-dd-MM",
  ContractResolver = new DefaultContractResolver
  {
    NamingStrategy = new SnakeCaseNamingStrategy()
  },
  Formatting = Formatting.Indented
};

saver.SaveFile(outputDirectory, payersList.Transform().ToJson(jsonSettings));

Console.ReadLine();