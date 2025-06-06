using ShiftYar.Application.Common.Filters;
using ShiftYar.Domain.Entities.ShiftDateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.CalendarSeeder.Filters
{
    public class ShiftDateFilter : BaseFilter<ShiftDate>
    {
        private readonly Expression<Func<ShiftDate, bool>> _expression;

        public ShiftDateFilter(Expression<Func<ShiftDate, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<ShiftDate, bool>> GetExpression()
        {
            return _expression;
        }
    }
}
