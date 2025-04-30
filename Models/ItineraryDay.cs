using System.Collections.Generic;

namespace BSLTours.API.Models
{
    public class ItineraryDay
    {
        public int Day { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImageSet Image { get; set; }
        public string Accommodation { get; set; }
        public List<Activity> Activities { get; set; } = new List<Activity>();
        public Meals Meals { get; set; } = new Meals();
    }


    public class ImageSet
    {
        public string BaseUrl { get; set; }

        public string Small => Transform("w_400,h_300,c_fill");
        public string Medium => Transform("w_800,h_500,c_fill");
        public string Banner => Transform("w_1200,h_600,c_fill");

        private string Transform(string transformation)
        {
            if (string.IsNullOrEmpty(BaseUrl)) return null;
            return BaseUrl.Replace("/upload/", $"/upload/{transformation}/");
        }
    }

    public class Meals
    {
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
    }
}