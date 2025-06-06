namespace BSLTours.API.Models.Dtos;

public class SummaryCardDto
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public CardDetailsDto Card { get; set; }
}

public class TourSummaryCardDto : SummaryCardDto
{
    public string Duration { get; set; }
    public decimal StartingFrom { get; set; }
    public string Currency { get; set; }
}
