using Newtonsoft.Json;

namespace DataProcessing.Process;

public interface IFileProcessor
{
    bool Process(IReadOnlyList<string> fileExtensions, JsonSerializerSettings settings);
    bool WriteFile(DirectoryInfo outputDir, string content);
    void DeleteFilesInDirectory(DirectoryInfo directory, IReadOnlyList<string> fileExtensions);
}