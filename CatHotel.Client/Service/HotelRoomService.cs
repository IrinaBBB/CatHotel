using CatHotel.Client.Service.IService;
using Models;
using Newtonsoft.Json;

namespace CatHotel.Client.Service
{
    public class HotelRoomService : IHotelRoomService
    {
        private readonly HttpClient _client;

        public HotelRoomService(HttpClient client)
        {
            _client = client;
        }

        public Task<HotelRoomDto> GetHotelRoomDetails(int roomId, string checkInDate, string checkOutDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HotelRoomDto>> GetHotelRooms(string checkInDate, string checkOutDate)
        {
            var response = await _client.GetAsync($"api/hotelroom?checkInDate={checkInDate}&checkOutDate={checkOutDate}");
            var content = await response.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<IEnumerable<HotelRoomDto>>(content);
            return rooms;
        }
    }
}
