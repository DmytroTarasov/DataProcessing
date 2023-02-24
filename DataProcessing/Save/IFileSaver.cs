namespace DataProcessing.Save;

public interface IFileSaver
{
    bool SaveFile(DirectoryInfo outputDir, string content);
}