using Business.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatHotel.Server.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IHotelRoomRepository _repo;

        public RoomController(IHotelRoomRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Get()
        {
            return Ok(_repo.GetAllHotelRooms());
        }
    }
}
