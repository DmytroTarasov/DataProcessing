using System.Collections.Generic;

namespace DataProcessing.Models;

public class Service
{
    public string Name { get; set; }
    public decimal Total { get; set; }
    public IEnumerable<Payer> Payers { get; set; }
}