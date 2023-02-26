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
            AutoReset = false
        };
        _timer.Elapsed += SaveFile;
    }
    
    private async void SaveFile(object sender, ElapsedEventArgs e)
    {
        if (DateTime.Now.TimeOfDay != TimeSpan.Zero || string.IsNullOrEmpty(_config["OutputDirectory"])) return;
        await using var sw = new StreamWriter(Path.Combine(_config["OutputDirectory"], 
            DateTime.Today.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), "meta.log"));
        var log = $"parsed_files: {_lineParser.ParsedFiles}\n" +
                        $"parsed_lines: {_lineParser.ParsedLines}\n" +
                        $"found_errors: {_lineParser.ErrorsCount}\n" + 
                        $"invalid_files: [{string.Join(", ", _lineParser.InvalidFiles)}]";
        await sw.WriteLineAsync(log);
        
        _timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
        _timer.Start();
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