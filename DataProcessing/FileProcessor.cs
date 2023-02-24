using DataProcessing.Helpers;
using DataProcessing.Read;
using DataProcessing.Save;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DataProcessing;

public class FileProcessor
{
    private readonly IDirectoryReader _directoryReader;
    private readonly IFileSaver _fileSaver;
    private readonly IConfiguration _config;
    private readonly JsonSerializerSettings _settings;

    public FileProcessor(IDirectoryReader directoryReader, IFileSaver fileSaver, 
        IConfiguration config, JsonSerializerSettings settings)
    {
        _directoryReader = directoryReader;
        _fileSaver = fileSaver;
        _config = config;
        _settings = settings;
    }

    public void Process()
    {
        var inputDir = new DirectoryInfo(_config["InputDirectory"]);
        var outputDir = new DirectoryInfo(_config["OutputDirectory"]);
        var payers = _directoryReader.ReadFilesInDirectory(inputDir);
        _fileSaver.SaveFile(outputDir, payers.ToList().Transform().ToJson(_settings));
    }
}