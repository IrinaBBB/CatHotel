using AutoMapper;
using DataAccess.Data;
using Models;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelRoomDto, HotelRoom>();
            CreateMap<HotelRoom, HotelRoomDto>();
            CreateMap<HotelAmenity, HotelAmenityDto>().ReverseMap();

            CreateMap<HotelRoomImage, HotelRoomImageDto>().ReverseMap();

            CreateMap<RoomOrderDetails, RoomOrderDetailsDto>().ForMember(x => x.HotelRoomDto, opt => opt.MapFrom(c => c.HotelRoom));
            CreateMap<RoomOrderDetailsDto, RoomOrderDetails>();
        }
    }
}
