using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Business.Repository
{
    public class HotelImagesRepository : IHotelImagesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelImagesRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> CreateHotelRoomImageAsync(HotelRoomImageDto imageDto)
        {
            var image = _mapper.Map<HotelRoomImageDto, HotelRoomImage>(imageDto);
            await _db.HotelRoomImages.AddAsync(image);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageByImageIdAsync(int imageId)
        {
            var roomImage = await _db.HotelRoomImages.FindAsync(imageId);
            _db.HotelRoomImages.Remove(roomImage);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageByRoomIdAsync(int roomId)
        {
            var imageList = await _db.HotelRoomImages.Where(i => i.RoomId == roomId).ToListAsync();
            _db.HotelRoomImages.RemoveRange(imageList);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<HotelRoomImageDto>> GetHotelRoomImagesAsync(int roomId)
        {
            var imageList = await _db.HotelRoomImages.Where(i => i.RoomId == roomId).ToListAsync();
            return _mapper.Map<IEnumerable<HotelRoomImage>, IEnumerable<HotelRoomImageDto>>(imageList);
        }
    }
}
