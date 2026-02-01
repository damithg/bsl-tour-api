using System.Collections.Generic;

namespace BSLTours.API.Models.Dtos;

public class CardDetailsDto
{
    public CardImageDto Image { get; set; }
    public string Header { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; }
}
