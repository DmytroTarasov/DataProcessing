using DataProcessing.Models;

namespace DataProcessing.Read;

public interface IDirectoryReader
{
    IEnumerable<Payer> ReadFilesInDirectory(DirectoryInfo directoryInfo);
}