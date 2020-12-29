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
    public class UserService : DataService<User>
    {
        private readonly DbTestContext _context;
        public UserService(DbTestContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User> GetByLastName(string lastName)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.LastName == lastName);
        }
    }
}
