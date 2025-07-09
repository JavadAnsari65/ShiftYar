using ShiftYar.Application.Common.Filters;
using ShiftYar.Domain.Entities.ShiftRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.ShiftRequestModel.Filters
{
    public class ShiftRequestFilter : BaseFilter<ShiftRequest>
    {
        public int? UserId { get; set; }
        public int? SupervisorId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ShiftYar.Domain.Enums.ShiftRequestModel.RequestStatus? Status { get; set; }

        //public override Expression<Func<ShiftRequest, bool>> GetExpression()
        //{
        //    return x => (!UserId.HasValue || x.UserId == UserId);
        //}

        public override Expression<Func<ShiftRequest, bool>> GetExpression()
        {
            return x =>
                (!UserId.HasValue || x.UserId == UserId) &&
                (!SupervisorId.HasValue || x.SupervisorId == SupervisorId) &&
                //(!DepartmentId.HasValue || (x.ShiftDate != null && x.ShiftDate.DepartmentId == DepartmentId)) &&
                (!Status.HasValue || x.Status == Status) &&
                (!FromDate.HasValue || (x.ShiftDate != null && x.ShiftDate.Date >= FromDate)) &&
                (!ToDate.HasValue || (x.ShiftDate != null && x.ShiftDate.Date <= ToDate));
        }
    }
}
