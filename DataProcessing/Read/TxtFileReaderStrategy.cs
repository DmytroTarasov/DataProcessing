using DataProcessing.Models;

namespace DataProcessing.Read;

public class TxtFileReaderStrategy : IFileReaderStrategy
{
    private readonly ILineParser _lineParser;
    public TxtFileReaderStrategy(ILineParser lineParser)
    {
        _lineParser = lineParser;
    }

    public IEnumerable<Payer> Read(string filePath)
    {
        return _lineParser.ParseLines(filePath, File.ReadLines(filePath));
    }
}