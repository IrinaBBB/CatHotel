using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelImagesRepository
    {
        public Task<int> CreateHotelRoomImageAsync(HotelRoomImageDto imageDto);
        public Task<int> DeleteHotelRoomImageByImageIdAsync(int imageId);
        public Task<int> DeleteHotelRoomImageByRoomIdAsync(int roomId);
        public Task<IEnumerable<HotelRoomImageDto>> GetHotelRoomImagesAsync(int roomId);
    }
}
