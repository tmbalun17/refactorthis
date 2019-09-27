using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly ProductContext _dbContext;

        public EfRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> whereFilter, Expression<Func<T, object>> orderBy = null, Expression<Func<T, object>> orderByDescending = null)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (whereFilter != null)
            {
                query = query.Where(whereFilter);
            }
            // Apply ordering if expressions are set
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }
            else if (orderByDescending != null)
            {
                query = query.OrderByDescending(orderByDescending);
            }
            return await query.ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

         public async Task DeleteAsync(Expression<Func<T, bool>> whereFilter)
        {
            var records = await _dbContext.Set<T>().AsQueryable().Where(whereFilter).ToListAsync();
            if (records.Count > 0)
            {
                _dbContext.Set<T>().RemoveRange(records);
            }
        }
    }
}
