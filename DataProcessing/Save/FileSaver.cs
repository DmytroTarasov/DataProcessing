using System.Globalization;

namespace DataProcessing.Save;

public class FileSaver : IFileSaver
{
    public bool SaveFile(DirectoryInfo outputDir, string content)
    {
        // if (!Directory.Exists(outputDir)) return false;
        var di = Directory.CreateDirectory(Path.Combine(outputDir.FullName, 
            DateTime.Today.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
        var filesCount = di.EnumerateFiles().Count();
        var fileName = $"output{filesCount + 1}.json";

        using var sw = new StreamWriter(Path.Combine(di.FullName, fileName));
        sw.Write(content);
        return true;
    }
}