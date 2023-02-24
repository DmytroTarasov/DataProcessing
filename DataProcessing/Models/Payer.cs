namespace DataProcessing.Models;

public class Payer
{
    public string FullName { get; set; }
    public string City { get; set; }
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
    public long AccountNumber { get; set; }
    public string Service { get; set; }
}