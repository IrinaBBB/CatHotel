using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDto> CreateHotelRoomAsync(HotelRoomDto hotelRoom);
        public Task<HotelRoomDto> UpdateHotelRoomAsync(int roomId, HotelRoomDto hotelRoom);
        public Task<HotelRoomDto> GetHotelRoomAsync(int roomId);
        public Task<int> DeleteHotelRoomAsync(int roomId);
        public IEnumerable<HotelRoomDto> GetAllHotelRooms();
        public Task<HotelRoomDto> IsRoomUniqueAsync(string name, int roomId = 0);
    }
}

