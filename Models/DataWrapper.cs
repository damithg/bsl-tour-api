namespace BSLTours.API.Models
{
    public class DataWrapper<T>
    {
        public int Id { get; set; }
        public T Attributes { get; set; }
    }
}
