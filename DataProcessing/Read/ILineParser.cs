using System.Collections.Generic;
using DataProcessing.Models;

namespace DataProcessing.Read;
public interface ILineParser
{
    IEnumerable<Payer> ParseLines(string fileName, IEnumerable<string> lines);
    int ParsedFiles { get; }
    int ParsedLines { get; }
    int ErrorsCount { get; }
    IEnumerable<string> InvalidFiles { get; }
}