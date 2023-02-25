﻿using System.Globalization;
using DataProcessing.Helpers;
using DataProcessing.Read;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DataProcessing.Process;

public class FileProcessor
{
    private readonly IDirectoryReader _directoryReader;
    private readonly IConfiguration _config;

    public FileProcessor(IDirectoryReader directoryReader, IConfiguration config)
    {
        _directoryReader = directoryReader;
        _config = config;
    }

    public bool Process(IReadOnlyList<string> fileExtensions, JsonSerializerSettings settings)
    {
        if (string.IsNullOrEmpty(_config["InputDirectory"]) ||
            string.IsNullOrEmpty(_config["OutputDirectory"])) return false;
        var inputDir = new DirectoryInfo(_config["InputDirectory"]);
        var outputDir = new DirectoryInfo(_config["OutputDirectory"]);
        var payers = _directoryReader.ReadFilesInDirectory(inputDir, fileExtensions);
        var saved = WriteFile(outputDir, payers.ToList().Transform().ToJson(settings));
        if (!saved) return false;
        DeleteFilesInDirectory(inputDir, fileExtensions);
        return true;
    }
    
    public bool WriteFile(DirectoryInfo outputDir, string content)
    {
        if (!outputDir.Exists) return false;
        var di = Directory.CreateDirectory(Path.Combine(outputDir.FullName, 
            DateTime.Today.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
        var filesCount = di.EnumerateFiles().Count();
        var fileName = $"output{filesCount + 1}.json";

        using var sw = new StreamWriter(Path.Combine(di.FullName, fileName));
        sw.Write(content);
        return true;
    }

    public void DeleteFilesInDirectory(DirectoryInfo directory, IReadOnlyList<string> fileExtensions)
    {
        directory
            .EnumerateFiles()
            .Where(f => fileExtensions.Contains(f.Extension))
            .ToList()
            .ForEach(file => File.Delete(file.FullName));
    }
}