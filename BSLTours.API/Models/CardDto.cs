using System.Collections.Generic;

namespace BSLTours.API.Models;

public class CardDto
{
    public CardImageDto Image { get; set; }

    public List<string> Tags { get; set; }
}