using DataProcessing.Models;

namespace DataProcessing.Read;

public interface IFileReaderStrategy
{
    IEnumerable<Payer> Read(string filePath);
}