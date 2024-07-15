using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto HotelRoomDto);
        public Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto HotelRoomDto);
        public Task<HotelRoomDto> GetHotelRoom(int roomId, string checkInDate = null, string checkOutDate = null);
        public Task<int> DeleteHotelRoom(int roomId);
        public Task<IEnumerable<HotelRoomDto>> GetAllHotelRooms(string checkInDate = null, string checkOutDate = null);
        public Task<HotelRoomDto> IsRoomUnique(string name, int roomId = 0);
        public Task<bool> IsRoomBooked(int RoomId, string checkInDate, string checkOutDate);
    }
}

