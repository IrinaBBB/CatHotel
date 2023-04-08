using AutoMapper;
using DataAccess.Data;
using Models;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<HotelRoom, HotelRoomDto>();
        }
    }
}
