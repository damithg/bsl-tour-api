using System.Collections.Generic;

namespace BSLTours.API.Models.Dtos;

public class DestinationCardDetails
{
    public DestinationCardImage Image { get; set; }
    public string Header { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; }
}