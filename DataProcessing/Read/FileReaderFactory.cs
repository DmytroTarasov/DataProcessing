using System;

namespace DataProcessing.Read;

public class FileReaderFactory
{
    private readonly ILineParser _lineParser;

    public FileReaderFactory(ILineParser lineParser)
    {
        _lineParser = lineParser;
    }
    
    public IFileReaderStrategy CreateStrategy(string fileExtension)
    {
        return fileExtension switch
        {
            ".txt" => new TxtFileReaderStrategy(_lineParser),
            ".csv" => new CsvFileReaderStrategy(_lineParser),
            _ => throw new ArgumentException($"There is no Strategy defined to handle {fileExtension} file")
        };
    }      
}