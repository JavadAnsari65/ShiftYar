using Microsoft.EntityFrameworkCore;
using ShiftYar.Application.DTOs.HospitalModel;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Domain.Entities.HospitalModel;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Infrastructure.Persistence.Repositories.HospitalModel
{
    public class HospitalService : IHospitalService
    {
        private readonly ShiftYarDbContext _context;

        public HospitalService(ShiftYarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HospitalDto_Add>> GetAllHospitalAsync()
        {
            return await _context.Hospitals
                .Include(h => h.PhoneNumbers)
                .Select(h => new HospitalDto_Add
                {
                    Id = h.Id,
                    SiamCode = h.SiamCode,
                    Name = h.Name,
                    Address = h.Address,
                    Email = h.Email,
                    Website = h.Website,
                    Description = h.Description,
                    IsActive = h.IsActive,
                    Logo = h.Logo,
                    PhoneNumbers = h.PhoneNumbers != null
                        ? h.PhoneNumbers.Select(p => p.PhoneNumber).ToList()
                        : new List<string>()
                })
                .ToListAsync();
        }

        public async Task AddHospitalAsync(HospitalDto_Add hospitalDto)
        {
            var hospital = new Hospital
            {
                SiamCode = hospitalDto.SiamCode,
                Name = hospitalDto.Name,
                Address = hospitalDto.Address,
                Email = hospitalDto.Email,
                Website = hospitalDto.Website,
                Description = hospitalDto.Description,
                IsActive = hospitalDto.IsActive,
                Logo = hospitalDto.Logo,
                PhoneNumbers = hospitalDto.PhoneNumbers?.Select(p => new HospitalPhoneNumber
                {
                    PhoneNumber = p,
                    IsActive = true
                }).ToList()
            };

            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();
        }
    }
}
