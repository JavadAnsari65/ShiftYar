using AutoMapper;
using ShiftYar.Application.DTOs.DepartmentModel;
using ShiftYar.Domain.Entities.DepartmentModel;
using ShiftYar.Domain.Entities.HospitalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Common.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDtoGet>();
            CreateMap<Hospital, HospitalDto>();
            CreateMap<DepartmentDtoAdd, Department>();
        }
    }
}
