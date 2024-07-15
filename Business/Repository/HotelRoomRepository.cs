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

        public async Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto HotelRoomDto)
        {
            var hotelRoom = _mapper.Map<HotelRoomDto, HotelRoom>(HotelRoomDto);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var addedHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDto>(addedHotelRoom.Entity);
        }

        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(roomId);
            if (roomDetails != null)
            {

                var allimages = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();

                _db.HotelRoomImages.RemoveRange(allimages);
                _db.HotelRooms.Remove(roomDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelRoomDto>> GetAllHotelRooms(string checkInDate = null, string checkOutDate = null)
        {
            try
            {
                IEnumerable<HotelRoomDto> HotelRoomDtos =
                            _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDto>>
                            (_db.HotelRooms.Include(x => x.HotelRoomImages));
                if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                {
                    foreach (HotelRoomDto hotelRoom in HotelRoomDtos)
                    {
                        hotelRoom.IsBooked = await IsRoomBooked(hotelRoom.Id, checkInDate, checkOutDate);
                    }
                }
                return HotelRoomDtos;
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<HotelRoomDto> GetHotelRoom(int roomId, string checkInDate = null, string checkOutDate = null)
        {
            try
            {
                HotelRoomDto hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                    await _db.HotelRooms.Include(x => x.HotelRoomImages).FirstOrDefaultAsync(x => x.Id == roomId));

                if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                {
                    hotelRoom.IsBooked = await IsRoomBooked(roomId, checkInDate, checkOutDate);
                }

                return hotelRoom;
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> IsRoomBooked(int RoomId, string checkInDate, string checkOutDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(checkOutDate) && !string.IsNullOrEmpty(checkInDate))
                {
                    DateTime checkInDateVar = DateTime.ParseExact(checkInDate, "MM/dd/yyyy", null);
                    DateTime checkOutDateVar = DateTime.ParseExact(checkOutDate, "MM/dd/yyyy", null);

                    var existingBooking = await _db.RoomOrderDetails
                        .Where(x => x.RoomId == RoomId && (checkInDateVar < x.CheckOutDate && checkOutDateVar > x.CheckInDate))
                        .FirstOrDefaultAsync();

                    if (existingBooking != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<HotelRoomDto> IsRoomUnique(string name, int roomId = 0)
        {
            try
            {
                if (roomId == 0)
                {
                    var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));

                    return hotelRoom;
                } else
                {
                    var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()
                        && x.Id != roomId));

                    return hotelRoom;
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto HotelRoomDto)
        {
            try
            {
                if (roomId == HotelRoomDto.Id)
                {
                    //valid
                    HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDto, HotelRoom>(HotelRoomDto, roomDetails);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = _db.HotelRooms.Update(room);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDto>(updatedRoom.Entity);
                } else
                {
                    //invalid
                    return null;
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<int> GetHotelRoomCount()
        {
            return await _db.HotelRooms.CountAsync();
        }
    }
}