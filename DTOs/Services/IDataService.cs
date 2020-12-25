using DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Services
{
    public interface IDataService
    {
        void Add(string requestLine);
        void Update(string requestLine);
        DomainObject GetBy(string requestLine, DomainObject wrapper); //next time gonna make correct wrapper. DomainObject end cup here
        IEnumerable<DomainObject> GetAll(string requestLine);
        //next time add filtering 
    }
}
