using System.Collections.Generic;
using System.Threading.Tasks;
using DataProcessing.Models;

namespace DataProcessing.Process;

public interface IFileProcessor
{
    Task<IEnumerable<Payer>> ReadFilesInDirectory(string directory, IReadOnlyList<string> fileExtensions);
    Task<IEnumerable<Payer>> ReadFileAsync(string filePath);
    Task ProcessAsync(IReadOnlyList<string> fileExtensions);
    Task<bool> WriteFileAsync(string directory, string content, string outputFileExtension);
    Task DeleteFilesInDirectoryAsync(string directory, IReadOnlyList<string> fileExtensions);
}