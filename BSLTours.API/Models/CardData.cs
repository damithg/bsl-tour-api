namespace BSLTours.API.Models;

public class CardData
{
    public string Header { get; set; }
    public string Heading { get; set; }
    public string Body { get; set; }
    public string Footer { get; set; }
    public string[] Tags { get; set; }
    public CardImage Image { get; set; }
}