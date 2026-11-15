using JobPortal.Application.Contracts.Persistence;
using JobPortal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
     where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>()
                .FindAsync(id);
        }
        public async Task<int> CountAsync()
        {
            return await _context.Set<T>()
                .CountAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>()
                .AddAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>()
                .Update(entity);

            await Task.CompletedTask;
        }
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>()
                .Remove(entity);

            await Task.CompletedTask;
        }
        public async Task<IReadOnlyList<T>> GetPagedAsync(
    int pageNumber,
    int pageSize)
        {
            return await _context.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
    }
