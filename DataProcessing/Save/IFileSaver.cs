namespace DataProcessing.Save;

public interface IFileSaver
{
    void SaveFile(string outputDir, string content);
}