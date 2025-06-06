namespace BSLTours.API.Models.Dtos
{
    public class DestinationCardDto
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public DestinationCardDetails Card { get; set; }
    }
}
