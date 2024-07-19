namespace CatHotel.Client.Model.ViewModel
{
    public class HomeViewModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public int NumberOfNights { get; set; } = 1;
    }
}