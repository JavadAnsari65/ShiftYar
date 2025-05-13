﻿using Microsoft.EntityFrameworkCore;
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
    public class EfRepository<T> : IEfRepository<T> where T : class
    {
        private readonly ShiftYarDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(ShiftYarDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<(List<T> Items, int TotalCount)> GetByFilterAsync(IFilter<T> filter = null, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            try
            {
                // Include specified includes
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                if (filter != null)
                    query = query.Where(filter.GetExpression());

                // Get total count before pagination
                int totalCount = await query.CountAsync();

                // Apply pagination if the filter is a BaseFilter with pagination properties
                if (filter is BaseFilter<T> baseFilter)
                {
                    var pageNumber = (int?)baseFilter.GetType().GetProperty("PageNumber")?.GetValue(baseFilter) ?? 1;
                    var pageSize = (int?)baseFilter.GetType().GetProperty("PageSize")?.GetValue(baseFilter) ?? 10;

                    // Ensure page number is at least 1
                    pageNumber = Math.Max(1, pageNumber);
                    // Ensure page size is at least 1
                    pageSize = Math.Max(1, pageSize);

                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }

                var items = await query.ToListAsync();
                return (items, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private static IQueryable<TEntity> IncludeNavigations<TEntity>(IQueryable<TEntity> query, IEntityType entityType, HashSet<string> includedPaths, HashSet<string> visitedTypes, string currentPath = "")
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


        public async Task<T> GetByIdAsync(object id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            try
            {// Include specified includes
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
