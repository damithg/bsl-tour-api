using System.Collections.Generic;

namespace BSLTours.API.Models;

public class CardDto
{
    public CardImageDto Image { get; set; }

    public string Header { get; set; }       // NEW - appears at top
    public string Heading { get; set; }      // was Title
    public string Body { get; set; }         // was Subtitle
    public string Footer { get; set; }       // NEW - appears at bottom

    public List<string> Tags { get; set; }   // was Tag, now an array
}