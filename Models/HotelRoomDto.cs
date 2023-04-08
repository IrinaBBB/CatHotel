using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class HotelRoomDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter room name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter occupancy")]
        public int Occupancy { get; set; }
        [Required(ErrorMessage = "Please enter regular rate")]
        public double RegularRate { get; set; }
        public string Details { get; set; }
        public string SqM { get; set; }
    }
}
