using DataProcessing.Models;

namespace DataProcessing.Read;
public interface ILineParser
{
    IEnumerable<Payer> ParseLines(string fileName, IEnumerable<string> lines);
}