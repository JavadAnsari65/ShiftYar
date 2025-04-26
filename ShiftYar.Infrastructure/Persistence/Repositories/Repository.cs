using Microsoft.EntityFrameworkCore;
using ShiftYar.Application.Common.Filters;
using ShiftYar.Application.Interfaces.Persistence;
using ShiftYar.Infrastructure.Persistence.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Infrastructure.Persistence.Repositories
{
    // 4. پیاده‌سازی EF Core برای ریپازیتوری
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ShiftYarDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ShiftYarDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        //public async Task<List<T>> GetByFilterAsync(IFilter<T> filter = null, bool includeAllNestedCollections = true, params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> query = _dbSet;

        //    // Include specified includes
        //    foreach (var include in includes)
        //        query = query.Include(include);

        //    // If includeAllNestedCollections is true, include all navigation properties
        //    if (includeAllNestedCollections)
        //    {
        //        var entityType = _context.Model.FindEntityType(typeof(T));
        //        if (entityType != null)
        //        {
        //            foreach (var navigation in entityType.GetNavigations())
        //            {
        //                query = query.Include(navigation.Name);
        //            }
        //        }
        //    }

        //    if (filter != null)
        //        query = query.Where(filter.GetExpression());

        //    return await query.ToListAsync();
        //}

        public async Task<List<T>> GetByFilterAsync(IFilter<T> filter = null, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Include specified includes
            foreach (var include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter.GetExpression());

            return await query.ToListAsync();
        }


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }


        //public async Task<T> GetByIdAsync(object id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}
        public async Task<T> GetByIdAsync(object id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Include specified includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}



//using Microsoft.EntityFrameworkCore;
//using ShiftYar.Application.Common.Filters;
//using ShiftYar.Application.Interfaces.Persistence;
//using ShiftYar.Infrastructure.Persistence.AppDbContext;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShiftYar.Infrastructure.Persistence.Repositories
//{
//    // 4. پیاده‌سازی EF Core برای ریپازیتوری
//    public class Repository<T> : IRepository<T> where T : class
//    {
//        private readonly ShiftYarDbContext _context;
//        private readonly DbSet<T> _dbSet;

//        public Repository(ShiftYarDbContext context)
//        {
//            _context = context;
//            _dbSet = _context.Set<T>();
//        }

//        public async Task<List<T>> GetByFilterAsync(IFilter<T>? filter = null, params Expression<Func<T, object>>[] includes)
//        {
//            IQueryable<T> query = _dbSet;

//            foreach (var include in includes)
//                query = query.Include(include);

//            if (filter != null)
//                query = query.Where(filter.GetExpression());

//            return await query.ToListAsync();
//        }

//        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
//        {
//            return await _dbSet.AnyAsync(predicate);
//        }

//        public async Task<T?> GetByIdAsync(object id)
//        {
//            return await _dbSet.FindAsync(id);
//        }

//        public async Task AddAsync(T entity)
//        {
//            await _dbSet.AddAsync(entity);
//        }

//        public void Update(T entity)
//        {
//            _dbSet.Update(entity);
//        }

//        public void Delete(T entity)
//        {
//            _dbSet.Remove(entity);
//        }

//        public async Task SaveAsync()
//        {
//            await _context.SaveChangesAsync();
//        }
//    }

//}
