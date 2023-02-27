using System;
using Newtonsoft.Json;

namespace DataProcessing.Models;

public class Payer
{
    [JsonProperty("name")]
    public string FullName { get; set; }
    
    [JsonIgnore]
    public string City { get; set; }
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
    public long AccountNumber { get; set; }
    
    [JsonIgnore]
    public string Service { get; set; }
}