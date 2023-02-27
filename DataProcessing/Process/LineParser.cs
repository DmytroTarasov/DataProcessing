using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DataProcessing.Models;

namespace DataProcessing.Process;

public class LineParser : ILineParser
{
    private readonly Dictionary<string, int> _parsedFiles;
    private int _errorsCount;

    public LineParser()
    {
        _parsedFiles = new();
    }

    public int ParsedFiles => _parsedFiles.Keys.Count;
    public int ParsedLines => _parsedFiles.Select(pair => pair.Value).Sum();
    public int ErrorsCount => _errorsCount;
    public IEnumerable<string> InvalidFiles => _parsedFiles.Where(pair => pair.Value != 0).Select(pair => pair.Key);

    public async Task<IEnumerable<Payer>> ParseLinesAsync(string fileName, IEnumerable<string> lines)
    {
        var payers = new List<Payer>();
        await Task.Run(() =>
        {
            lines.ToList().ForEach(line =>
            {
                var data = line.Split(", ");
                if (IsValid(data))
                {
                    payers.Add(CreatePayer(data));
                }
                else
                {
                    _errorsCount++;
                }

                IncreaseParsedLinesNumber(fileName);
            });
        });
        return payers;
    }

    private bool IsValid(IReadOnlyList<string> data)
    {
        return data.Count == 9 && decimal.TryParse(data[5].Replace(".", ","), out _) &&
               DateTime.TryParseExact(data[6], "yyyy-dd-MM", CultureInfo.InvariantCulture,
                   DateTimeStyles.None, out _) && long.TryParse(data[7], out _);
    }

    private Payer CreatePayer(IReadOnlyList<string> data)
    {
        return new Payer
        {
            FullName = $"{data[0]} {data[1]}",
            City = data[2][1..],
            Payment = decimal.Parse(data[5].Replace(".", ",")),
            Date = DateTime.ParseExact(data[6], "yyyy-dd-MM", CultureInfo.InvariantCulture),
            AccountNumber = long.Parse(data[7]),
            Service = data[8]
        };
    }

    private void IncreaseParsedLinesNumber(string fileName)
    {
        if (_parsedFiles.ContainsKey(fileName))
        {
            _parsedFiles[fileName]++;
        }
        else
        {
            _parsedFiles.Add(fileName, 1);
        }
    }

    public void ClearProcessedInfo()
    {
        _parsedFiles.Clear();
        _errorsCount = 0;
    }
}