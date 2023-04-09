using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public HotelRoomRepository(ApplicationDbContext db, IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
            _db = db;
        }

        public async Task<HotelRoomDto> CreateHotelRoomAsync(HotelRoomDto hotelRoomDto)
        {
            var hotelRoom = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var addedHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDto>(addedHotelRoom.Entity);
        }

        public async Task<HotelRoomDto> UpdateHotelRoomAsync(int roomId, HotelRoomDto hotelRoomDto)
        {
            try
            {
                if (roomId != hotelRoomDto.Id) return null;

                var roomDetails = await _db.HotelRooms.FindAsync(roomId);
                var room = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto, roomDetails);
                room.UpdatedBy = "";
                room.UpdatedDate = DateTime.Now;
                var updatedRoom = _db.HotelRooms.Update(room);
                await _db.SaveChangesAsync();
                return _mapper.Map<HotelRoom, HotelRoomDto>(updatedRoom.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error updating Hotel Room with id - {id} : {message}",
                    roomId, ex.Message);
                return null;
            }
        }

        public async Task<HotelRoomDto> GetHotelRoomAsync(int roomId)
        {
            try
            {
                var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                    await _db.HotelRooms.FirstOrDefaultAsync(x => x.Id == roomId));
                return hotelRoom;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error getting Hotel Room with id - {id} : {message}",
                    roomId, ex.Message);
                return null;
            }
        }

        public async Task<int> DeleteHotelRoomAsync(int roomId)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(roomId);

            if (roomDetails == null) return 0;

            _db.HotelRooms.Remove(roomDetails);
            return await _db.SaveChangesAsync();
        }

        public IEnumerable<HotelRoomDto> GetAllHotelRooms()
        {
            try
            {
                var hotelRoomDtoList =
                    _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDto>>(_db.HotelRooms);
                return hotelRoomDtoList;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error getting Hotel Rooms List : {message}",
                    ex.Message);
                return null;
            }
        }

        public async Task<HotelRoomDto> IsRoomUniqueAsync(string name, int roomId = 0)
        {
            try
            {
                if (roomId == 0)
                {
                    var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                        await _db.HotelRooms.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync());

                    return hotelRoom;
                }
                else
                {
                    var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()
                                                                      && x.Id != roomId));

                    return hotelRoom;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error getting Hotel Room with id - {id} : {message}",
                    roomId, ex.Message);
                return null;
            }
        }
    }
}