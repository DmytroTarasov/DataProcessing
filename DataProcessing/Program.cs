using DataProcessing.Read;

var directory = @"C:\Dima\Radency\Files";

var lineParser = new LineParser();
var fileReaderFactory = new FileReaderFactory(lineParser);
var directoryReader = new DirectoryReader(fileReaderFactory);

var payers = directoryReader.ReadFilesInDirectory(new DirectoryInfo(directory));
foreach (var payer in payers)
{
  Console.WriteLine($"{payer.FirstName} {payer.LastName} {payer.City} {payer.Payment} {payer.Date} {payer.AccountNumber } {payer.Service}");  
}

var parsedFiles = lineParser.ParsedFiles;
foreach (var pair in parsedFiles)
{
  Console.WriteLine($"{pair.Key}: {pair.Value}");
}

Console.WriteLine(lineParser.ErrorsCount);

Console.ReadLine();