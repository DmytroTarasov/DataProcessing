using System.Collections.Generic;
using System.IO;
using DataProcessing.Models;

namespace DataProcessing.Read;

public class TxtFileReaderStrategy : IFileReaderStrategy
{
    private readonly ILineParser _lineParser;
    public TxtFileReaderStrategy(ILineParser lineParser)
    {
        _lineParser = lineParser;
    }

    public IEnumerable<Payer> ReadFile(string filePath)
    {
        return _lineParser.ParseLines(filePath, File.ReadLines(filePath));
    }
}