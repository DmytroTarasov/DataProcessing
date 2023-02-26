using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataProcessing.Helpers;
using DataProcessing.Models;
using DataProcessing.Read;
using Microsoft.Extensions.Configuration;

namespace DataProcessing.Process;

public class FileProcessor : IFileProcessor
{
    private readonly IConfiguration _config;
    private readonly ILineParser _lineParser;

    public FileProcessor(ILineParser lineParser, IConfiguration config)
    {
        _lineParser = lineParser;
        _config = config;
    }

    public async Task<IEnumerable<Payer>> ReadFilesInDirectory(string directory,
        IReadOnlyList<string> fileExtensions)
    {
        var tasks = new List<Task<IEnumerable<Payer>>>();
        var files = Directory.EnumerateFiles(directory)
            .Where(file => fileExtensions.Contains(file.Split(".").Last()))
            .ToList();

        files.ForEach(file => { tasks.Add(Task.Run(() => ReadFileAsync(file))); });
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(p => p);
    }

    public async Task<IEnumerable<Payer>> ReadFileAsync(string filePath)
    {
        var payers = new List<Payer>();
        var lines = new List<string>();
        
        using var sr = new StreamReader(filePath);
        if (filePath.Split(".").Last() == "csv") await sr.ReadLineAsync();
        while (!sr.EndOfStream)
        {
            var buffer = new char[4096];
            var charsRead = await sr.ReadAsync(buffer, 0, 4096);
            var chunk = new string(buffer, 0, charsRead);
            
            var chunkLines = chunk.Split(Environment.NewLine);

            if (lines.Count > 0)
            {
                lines[^1] += chunkLines[0];
                chunkLines[0] = lines[^1];
                lines.RemoveAt(lines.Count - 1);
            }

            lines.AddRange(chunkLines.Take(chunkLines.Length - 1));

            if (!sr.EndOfStream)
            {
                lines.Add(chunkLines.Last());
            }

            if (lines.Count < 2000) continue;
            payers.AddRange(await _lineParser.ParseLinesAsync(filePath, lines));
            lines.Clear();
        }
        payers.AddRange(await _lineParser.ParseLinesAsync(filePath, lines));
        return payers;
    }

    public async Task ProcessAsync(IReadOnlyList<string> fileExtensions)
    {
        if (string.IsNullOrEmpty(_config["InputDirectory"]) ||
            string.IsNullOrEmpty(_config["OutputDirectory"])) return;
        var payers = await ReadFilesInDirectory(_config["InputDirectory"], fileExtensions);
        var saved = await WriteFileAsync(_config["OutputDirectory"], payers.ToList().Transform().ToJson());
        // if (saved) await DeleteFilesInDirectoryAsync(_config["InputDirectory"], fileExtensions);
    }

    public async Task<bool> WriteFileAsync(string directory, string content,
        string outputFileExtension = ".json")
    {
        if (!Directory.Exists(directory)) return false;
        var di = Directory.CreateDirectory(Path.Combine(directory,
            DateTime.Today.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
        var filesCount = di.EnumerateFiles().Count();
        var fileName = $"output{filesCount + 1}{outputFileExtension}";

        await using var sw = new StreamWriter(Path.Combine(di.FullName, fileName));
        await sw.WriteAsync(content);
        return true;
    }

    public async Task DeleteFilesInDirectoryAsync(string directory, IReadOnlyList<string> fileExtensions)
    {
        var tasks = new List<Task>();
        Directory
            .EnumerateFiles(directory)
            .Where(file => fileExtensions.Contains(file.Split(".").Last()))
            .ToList()
            .ForEach(file => { tasks.Add(Task.Run(() => File.Delete(file))); });
        await Task.WhenAll(tasks);
    }
}