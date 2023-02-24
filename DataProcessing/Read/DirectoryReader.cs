using DataProcessing.Models;

namespace DataProcessing.Read;

public class DirectoryReader : IDirectoryReader
{
    private readonly FileReaderFactory _fileReaderFactory;

    public DirectoryReader(FileReaderFactory fileReaderFactory)
    {
        _fileReaderFactory = fileReaderFactory;
    }
    public IEnumerable<Payer> ReadFilesInDirectory(DirectoryInfo directoryInfo)
    {
        IEnumerable<Payer> payers = new List<Payer>();
        try
        {
            payers = directoryInfo.EnumerateFiles()
                .Where(f => f.Extension is ".txt" or ".csv")
                .Select(f => f.FullName)
                .ToList()
                .SelectMany(file => _fileReaderFactory.CreateStrategy(file.Split(".").Last()).Read(file));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return payers;
    }
}