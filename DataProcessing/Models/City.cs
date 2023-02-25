using System.Collections.Generic;

namespace DataProcessing.Models;

public class City
{
    public string Name { get; set; }
    public decimal Total { get; set; }
    public IEnumerable<Service> Services { get; set; }
}