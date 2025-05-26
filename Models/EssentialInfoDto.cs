using System.Collections.Generic;

namespace BSLTours.API.Models;

public class EssentialInfoDto
{
    public int Id { get; set; }
    public string BestTimeToVisit { get; set; }
    public string NearestAirport { get; set; }
    public Dictionary<string, string> OpeningHours { get; set; }
    public List<string> TransportOptions { get; set; }
    public EntranceFees EntranceFees { get; set; }
    public List<string> Accessibility { get; set; }
    public List<string> TravelTips { get; set; }
    public List<string> Highlights { get; set; }
}