using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class HotelRoomDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter room name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter occupancy")]
        [Range(1, int.MaxValue, ErrorMessage = "Occupancy should be greater than zero")]
        public int Occupancy { get; set; }

        [Required(ErrorMessage = "Please enter regular rate")]
        [Range(1, int.MaxValue, ErrorMessage = "Regular rate should be greater than zero")]
        public double RegularRate { get; set; }

        public string Details { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SqM should be greater than zero")]
        public string SqM { get; set; }
        //public virtual ICollection<HotelRoomImageDTO> HotelRoomImages { get; set; }
    }
}
