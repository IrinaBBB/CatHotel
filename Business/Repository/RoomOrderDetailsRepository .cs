using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class RoomOrderDetailsRepository : IRoomOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RoomOrderDetailsRepository(ApplicationDbContext db, IMapper mapper, ILogger logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<RoomOrderDetailsDto> Create(RoomOrderDetailsDto details)
        {
            try
            {
                details.CheckInDate = details.CheckInDate.Date;
                details.CheckOutDate = details.CheckOutDate.Date;
                var roomOrder = _mapper.Map<RoomOrderDetailsDto, RoomOrderDetails>(details);
                roomOrder.Status = SD.Status_Pending;
                var result = await _db.RoomOrderDetails.AddAsync(roomOrder);
                await _db.SaveChangesAsync();
                return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDto>(result.Entity);
            } catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<RoomOrderDetailsDto>> GetAllRoomOrderDetails()
        {
            try
            {
                IEnumerable<RoomOrderDetailsDto> roomOrders = _mapper.Map<IEnumerable<RoomOrderDetails>, IEnumerable<RoomOrderDetailsDto>>
                    (_db.RoomOrderDetails.Include(u => u.HotelRoom));

                return roomOrders;
            } catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task<RoomOrderDetailsDto> GetRoomOrderDetail(int roomOrderId)
        {
            try
            {
                RoomOrderDetails roomOrder = await _db.RoomOrderDetails
                    .Include(u => u.HotelRoom).ThenInclude(x => x.HotelRoomImages)
                    .FirstOrDefaultAsync(u => u.Id == roomOrderId);

                RoomOrderDetailsDto RoomOrderDetailsDto = _mapper.Map<RoomOrderDetails, RoomOrderDetailsDto>(roomOrder);
                RoomOrderDetailsDto.HotelRoomDto.TotalDays = RoomOrderDetailsDto.CheckOutDate
                    .Subtract(RoomOrderDetailsDto.CheckInDate).Days;

                return RoomOrderDetailsDto;
            } catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task<RoomOrderDetailsDto> MarkPaymentSuccessful(int id)
        {
            var data = await _db.RoomOrderDetails.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            if (!data.IsPaymentSuccessful)
            {
                data.IsPaymentSuccessful = true;
                data.Status = SD.Status_Booked;
                var markPaymentSuccessful = _db.RoomOrderDetails.Update(data);
                await _db.SaveChangesAsync();
                return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDto>(markPaymentSuccessful.Entity);
            }
            return new RoomOrderDetailsDto();
        }

        public async Task<bool> UpdateOrderStatus(int RoomOrderId, string status)
        {
            try
            {
                var roomOrder = await _db.RoomOrderDetails.FirstOrDefaultAsync(u => u.Id == RoomOrderId);
                if (roomOrder == null)
                {
                    return false;
                }
                roomOrder.Status = status;
                if (status == SD.Status_CheckedIn)
                {
                    roomOrder.ActualCheckInDate = DateTime.Now;
                }
                if (status == SD.Status_CheckedOut_Completed)
                {
                    roomOrder.ActualCheckOutDate = DateTime.Now;
                }
                await _db.SaveChangesAsync();
                return true;
            } catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}
