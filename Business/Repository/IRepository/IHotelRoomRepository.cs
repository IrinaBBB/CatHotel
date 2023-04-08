using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoom);
        public Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto hotelRoom);
        public Task<HotelRoomDto> GetHotelRoom(int RoomId);
        public Task<int> DeleteHotelRoom(int roomId);
        public IEnumerable<HotelRoomDto> GetAllHotelRooms();
        public Task<HotelRoomDto> IsRoomUnique(string name, int roomId);
    }
}
