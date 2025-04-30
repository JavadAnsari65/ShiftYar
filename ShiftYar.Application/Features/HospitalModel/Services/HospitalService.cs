using AutoMapper;
using Microsoft.Extensions.Logging;
using ShiftYar.Application.Common.Models.ResponseModel;
using ShiftYar.Application.DTOs.HospitalModel;
using ShiftYar.Application.Features.HospitalModel.Filters;
using ShiftYar.Application.Interfaces.HospitalModel;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Domain.Entities.HospitalModel;

namespace ShiftYar.Infrastructure.Persistence.Repositories.HospitalModel
{
    public class HospitalService : IHospitalService
    {
        private readonly IEfRepository<Hospital> _repository;
        private readonly IEfRepository<HospitalPhoneNumber> _repositoryPhoneNumber;
        private readonly IMapper _mapper; // AutoMapper
        private readonly ILogger<HospitalService> _logger;

        public HospitalService(IEfRepository<Hospital> repository, IEfRepository<HospitalPhoneNumber> repositoryPhoneNumber,
                                IMapper mapper, ILogger<HospitalService> logger)
        {
            _repository = repository;
            _repositoryPhoneNumber = repositoryPhoneNumber;
            _mapper = mapper;
            _logger = logger;
        }

        ///Get All Hospital
        public async Task<ApiResponse<PagedResponse<HospitalDtoGet>>> GetFilteredUsersAsync(HospitalFilter filter)
        {
            _logger.LogInformation("Fetching hospitals with filter: {@Filter}", filter);

            var result = await _repository.GetByFilterAsync(filter, "PhoneNumbers");
            var data = _mapper.Map<List<HospitalDtoGet>>(result.Items);

            var pagedResponse = new PagedResponse<HospitalDtoGet>
            {
                Items = data,
                TotalCount = result.TotalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize)
            };

            _logger.LogInformation("Successfully fetched {Count} hospitals out of {TotalCount}", data.Count, result.TotalCount);

            return ApiResponse<PagedResponse<HospitalDtoGet>>.Success(pagedResponse);
        }

        Task<ApiResponse<HospitalDtoAdd>> IHospitalService.CreateAsync(HospitalDtoAdd dto)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse<string>> IHospitalService.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse<HospitalDtoAdd>> IHospitalService.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse<HospitalDtoAdd>> IHospitalService.UpdateAsync(int id, HospitalDtoAdd dto)
        {
            throw new NotImplementedException();
        }



        //public async Task<IEnumerable<HospitalDtoAdd>> GetAllHospitalAsync()
        //{
        //    return await _context.Hospitals
        //        .Include(h => h.PhoneNumbers)
        //        .Select(h => new HospitalDtoAdd
        //        {
        //            Id = h.Id,
        //            SiamCode = h.SiamCode,
        //            Name = h.Name,
        //            Address = h.Address,
        //            Email = h.Email,
        //            Website = h.Website,
        //            Description = h.Description,
        //            IsActive = h.IsActive,
        //            Logo = h.Logo,
        //            PhoneNumbers = h.PhoneNumbers != null
        //                ? h.PhoneNumbers.Select(p => p.PhoneNumber).ToList()
        //                : new List<string>()
        //        })
        //        .ToListAsync();
        //}

        //public async Task AddHospitalAsync(HospitalDtoAdd hospitalDto)
        //{
        //    var hospital = new Hospital
        //    {
        //        SiamCode = hospitalDto.SiamCode,
        //        Name = hospitalDto.Name,
        //        Address = hospitalDto.Address,
        //        Email = hospitalDto.Email,
        //        Website = hospitalDto.Website,
        //        Description = hospitalDto.Description,
        //        IsActive = hospitalDto.IsActive,
        //        Logo = hospitalDto.Logo,
        //        PhoneNumbers = hospitalDto.PhoneNumbers?.Select(p => new HospitalPhoneNumber
        //        {
        //            PhoneNumber = p,
        //            IsActive = true
        //        }).ToList()
        //    };

        //    _context.Hospitals.Add(hospital);
        //    await _context.SaveChangesAsync();
        //}



    }
}
