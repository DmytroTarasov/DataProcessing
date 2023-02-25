using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DataProcessing.Process;

public interface IFileProcessor
{
    bool Process(IReadOnlyList<string> fileExtensions);
    bool WriteFile(DirectoryInfo outputDir, string content, string outputFileExtension);
    void DeleteFilesInDirectory(DirectoryInfo directory, IReadOnlyList<string> fileExtensions);
}