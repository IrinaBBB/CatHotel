using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelImagesRepository
    {
        public Task<int> CreateHotelRoomImage(HotelRoomImageDto image);
        public Task<int> DeleteHotelRoomImageByImageId(int imageId);
        public Task<int> DeleteHotelRoomImageByRoomId(int roomId);
        public Task<int> DeleteHotelImageByImageUrl(string imageUrl);
        public Task<IEnumerable<HotelRoomImageDto>> GetHotelRoomImages(int roomId);
    }
}
