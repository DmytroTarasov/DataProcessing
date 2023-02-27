using System.Collections.Generic;
using System.Threading.Tasks;
using DataProcessing.Models;

namespace DataProcessing.Process;

public interface ILineParser
{
    Task<IEnumerable<Payer>> ParseLinesAsync(string fileName, IEnumerable<string> lines);
    int ParsedFiles { get; }
    int ParsedLines { get; }
    int ErrorsCount { get; }
    IEnumerable<string> InvalidFiles { get; }
    void ClearProcessedInfo();
}