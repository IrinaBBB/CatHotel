using AutoMapper;
using Business.Repository.IRepository;
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
    public class AmenityRepository : IAmenityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AmenityRepository(ApplicationDbContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelAmenityDto> CreateHotelAmenity(HotelAmenityDto hotelAmenity)
        {
            var amenity = _mapper.Map<HotelAmenityDto, HotelAmenity>(hotelAmenity);
            amenity.CreatedBy = "";
            amenity.CreatedDate = DateTime.UtcNow;
            var addedHotelAmenity = await _context.HotelAmenities.AddAsync(amenity);
            await _context.SaveChangesAsync();
            return _mapper.Map<HotelAmenity, HotelAmenityDto>(addedHotelAmenity.Entity);
        }

        public async Task<int> DeleteHotelAmenity(int amenityId, string userId)
        {
            var amenityDetails = await _context.HotelAmenities.FindAsync(amenityId);
            if (amenityDetails != null)
            {
                _context.HotelAmenities.Remove(amenityDetails);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelAmenityDto>> GetAllHotelAmenity()
        {
            return _mapper.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDto>>(await _context.HotelAmenities.ToListAsync());
        }

        public async Task<int> GetAmenitiesCount()
        {
            return await _context.HotelAmenities.CountAsync();
        }

        public async Task<HotelAmenityDto> GetHotelAmenity(int amenityId)
        {
            var amenityData = await _context.HotelAmenities
                .FirstOrDefaultAsync(x => x.Id == amenityId);

            if (amenityData == null)
            {
                return null;
            }
            return _mapper.Map<HotelAmenity, HotelAmenityDto>(amenityData);
        }

        public async Task<HotelAmenityDto> IsSameNameAmenityAlreadyExists(string name)
        {
            try
            {
                var amenityDetails =
                    await _context.HotelAmenities.FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim()
                    );
                return _mapper.Map<HotelAmenity, HotelAmenityDto>(amenityDetails);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new HotelAmenityDto();
        }

        public async Task<HotelAmenityDto> UpdateHotelAmenity(int amenityId, HotelAmenityDto hotelAmenity)
        {
            var amenityDetails = await _context.HotelAmenities.FindAsync(amenityId);
            var amenity = _mapper.Map(hotelAmenity, amenityDetails);
            amenity.UpdatedBy = "";
            amenity.UpdatedDate = DateTime.UtcNow;
            var updatedamenity = _context.HotelAmenities.Update(amenity);
            await _context.SaveChangesAsync();
            return _mapper.Map<HotelAmenity, HotelAmenityDto>(updatedamenity.Entity);
        }
    }
}
