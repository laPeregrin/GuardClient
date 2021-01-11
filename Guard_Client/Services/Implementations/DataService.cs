using DTOs.Models;
using DTOs.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using testDAL;

namespace Guard_Client.Services.Implementations
{
    public class DataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly DbTestContext _service;

        public DataService(DbTestContext service)
        {
            _service = service;
        }

        public virtual async Task Add(T obj)
        {
            if (!_service.Users.Any(x => x.Id == obj.Id))
            {
                await _service.Set<T>().AddAsync(obj);
                await _service.SaveChangesAsync();
            }

        }

        public Task<bool> Delete(T obj)
        {
            try
            {
                _service.Set<T>().Remove(obj);
                _service.SaveChanges();
                return Task.Run(() => true);
            }
            catch (Exception e)
            {
                return Task.Run(() => false);
            }

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _service.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression)
        {
            return await _service.Set<T>().Where(expression).AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetBy(Guid id)
        {
            return await _service.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(T obj)
        {
            _service.Set<T>().Update(obj);
            await _service.SaveChangesAsync();
        }
    }
}
