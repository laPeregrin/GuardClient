using DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Services
{
    //this service gonna be connect to web api
    public interface IDataService<T> where T : DomainObject
    {
        Task Add(T obj);
        Task Update(T obj);
        Task<T> GetBy(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);
    }
}
