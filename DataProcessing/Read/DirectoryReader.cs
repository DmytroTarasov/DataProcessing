using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataProcessing.Models;

namespace DataProcessing.Read;

public class DirectoryReader : IDirectoryReader
{
    private readonly FileReaderFactory _fileReaderFactory;

    public DirectoryReader(FileReaderFactory fileReaderFactory)
    {
        _fileReaderFactory = fileReaderFactory;
    }

    public IEnumerable<Payer> ReadFilesInDirectory(DirectoryInfo directoryInfo, IReadOnlyList<string> fileExtensions)
    {
        return directoryInfo.EnumerateFiles()
            .Where(f => fileExtensions.Contains(f.Extension))
            .ToList()
            .SelectMany(file => _fileReaderFactory
                .CreateStrategy(file.Extension)
                .ReadFile(file.FullName));
    }
}