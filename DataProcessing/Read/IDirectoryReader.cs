using System.Collections.Generic;
using System.IO;
using DataProcessing.Models;

namespace DataProcessing.Read;

public interface IDirectoryReader
{
    IEnumerable<Payer> ReadFilesInDirectory(DirectoryInfo directoryInfo, IReadOnlyList<string> fileExtensions);
}