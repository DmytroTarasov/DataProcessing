using System;
using System.Globalization;
using System.IO;
using System.Timers;
using DataProcessing.Read;
using Microsoft.Extensions.Configuration;

namespace DataProcessing.Helpers;

public class MidnightTimer
{
    private readonly ILineParser _lineParser;
    private readonly IConfiguration _config;
    private readonly Timer _timer;

    public MidnightTimer(ILineParser lineParser, IConfiguration config)
    {
        _lineParser = lineParser;
        _config = config;
        _timer = new Timer
        {
            Interval = (DateTime.Now.AddDays(1).Date - DateTime.Now).TotalMilliseconds,
            AutoReset = true
        };
        _timer.Elapsed += SaveMetaLogFile;
    }
    
    private void SaveMetaLogFile(object sender, ElapsedEventArgs e)
    {
        if (DateTime.Now.TimeOfDay != TimeSpan.Zero || string.IsNullOrEmpty(_config["OutputDirectory"])) return;
        using var sw = new StreamWriter(Path.Combine(_config["OutputDirectory"], 
            DateTime.Today.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), "meta.log"));
        sw.WriteLine($"parsed_files: {_lineParser.ParsedFiles}");
        sw.WriteLine($"parsed_lines: {_lineParser.ParsedLines}");
        sw.WriteLine($"found_errors: {_lineParser.ErrorsCount}");
        sw.WriteLine($"invalid_files: [{string.Join(", ", _lineParser.InvalidFiles)}]");
    }

    public void Start()
    {
        _timer.Start();
    }
    
    public void Stop()
    {
        _timer.Stop();
    }
}