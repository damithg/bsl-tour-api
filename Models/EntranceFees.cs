namespace BSLTours.API.Models;

public class EntranceFees
{
    public Currency Currency { get; set; }
    public string LocalAdults { get; set; }
    public string ForeignAdults { get; set; }
    public string LocalChildren { get; set; }
    public string ForeignChildren { get; set; }
}