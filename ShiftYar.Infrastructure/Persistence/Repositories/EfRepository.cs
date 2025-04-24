using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly ShiftYarDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(ShiftYarDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public async Task<List<T>> GetByFilterAsync(IFilter<T> filter = null, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Include specified includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
                query = query.Where(filter.GetExpression());

            return await query.ToListAsync();
        }


        private IQueryable<TEntity> IncludeNavigations<TEntity>(IQueryable<TEntity> query, IEntityType entityType, HashSet<string> includedPaths, HashSet<string> visitedTypes, string currentPath = "")
            where TEntity : class
        {
            // If we've already processed this type in the current branch, stop to prevent cycles
            var typeName = entityType.Name;
            if (!visitedTypes.Add(typeName))
                return query;

            try
            {
                foreach (var navigation in entityType.GetNavigations())
                {
                    // Skip inverse navigation properties to prevent circular references
                    if (navigation.Inverse != null)
                        continue;

                    var path = string.IsNullOrEmpty(currentPath) ? navigation.Name : $"{currentPath}.{navigation.Name}";

                    // Skip if we've already included this exact path
                    if (includedPaths.Contains(path))
                        continue;

                    try
                    {
                        // Include the navigation property
                        query = query.Include(path);
                        includedPaths.Add(path);

                        // For collection navigations or reference navigations, include their properties
                        var targetType = navigation.TargetEntityType;
                        if (targetType != null)
                        {
                            // Create a new visited types set for this branch
                            var branchVisitedTypes = new HashSet<string>(visitedTypes);
                            query = IncludeNavigations(query, targetType, includedPaths, branchVisitedTypes, path);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Log the warning but continue processing other navigations
                        if (ex.Message.Contains("NavigationBaseIncludeIgnored"))
                            continue;
                        throw;
                    }
                }
            }
            finally
            {
                // Remove the current type from visited types when leaving this branch
                visitedTypes.Remove(typeName);
            }

            return query;
        }


        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
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


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
