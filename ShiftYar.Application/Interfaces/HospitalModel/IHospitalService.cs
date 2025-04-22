using ShiftYar.Application.DTOs.HospitalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Interfaces.HospitalModel
{
    public interface IHospitalService
    {
        Task<IEnumerable<HospitalDto_Add>> GetAllHospitalAsync();
        Task AddHospitalAsync(HospitalDto_Add hospitalDto);
    }
}
