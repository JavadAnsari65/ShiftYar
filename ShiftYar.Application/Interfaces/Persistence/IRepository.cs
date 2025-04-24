using ShiftYar.Application.Common.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Interfaces.Persistence
{
    //اینترفیس عمومی ریپازیتوری
    public interface IRepository<T> where T : class
    {
        // دریافت لیست براساس فیلتر و بارگذاری روابط
        Task<List<T>> GetByFilterAsync(IFilter<T> filter = null, bool includeAllNestedCollections = true, params Expression<Func<T, object>>[] includes);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(object id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();

        // سایر متدها می‌تونن اضافه بشن
    }
}



//using ShiftYar.Application.Common.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShiftYar.Application.Interfaces.Persistence
//{
//    //اینترفیس عمومی ریپازیتوری
//    public interface IRepository<T> where T : class
//    {
//        // دریافت لیست براساس فیلتر و بارگذاری روابط
//        Task<List<T>> GetByFilterAsync(IFilter<T>? filter = null, params Expression<Func<T, object>>[] includes);
//        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
//        Task<T?> GetByIdAsync(object id);
//        Task AddAsync(T entity);
//        void Update(T entity);
//        void Delete(T entity);
//        Task SaveAsync();

//        // سایر متدها می‌تونن اضافه بشن
//    }
//}
