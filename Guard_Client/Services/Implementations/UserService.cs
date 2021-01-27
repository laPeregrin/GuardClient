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
            return await _context.Users.FirstOrDefaultAsync(x => x.LastName == lastName);
        }
        public async Task<User> GetByCardId(string cardId)
        {
            return await _context.Users.Include(x => x.Image).FirstOrDefaultAsync(x => x.CardId == cardId);
        }
        public override Task Add(User obj)
        {
            if (obj.Image != null)
                _context.Images.Add(obj.Image);
            return base.Add(obj);
        }
        public async Task UpdateWithImage(User pbj)
        {
            if (pbj.Image != null)
            {
                _context.Images.Add(pbj.Image);
                await _context.SaveChangesAsync();
            }
            await base.Add(pbj);
        }
    }
}
