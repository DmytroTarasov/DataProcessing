using DataProcessing.Models;

namespace DataProcessing.Read;

public class CsvFileReaderStrategy : IFileReaderStrategy
{
    private readonly ILineParser _lineParser;
    public CsvFileReaderStrategy(ILineParser lineParser)
    {
        _lineParser = lineParser;
    }
    public IEnumerable<Payer> ReadFile(string filePath)
    {
        return _lineParser.ParseLines(filePath, File.ReadLines(filePath).Skip(1));
    }
}