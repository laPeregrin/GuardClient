using DTOs.Models;
using DTOs.Services;
using Guard_Client.Exceptions;
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
    public class KeyObjectService : DataService<KeyObject>
    {
        private readonly DbTestContext _context;
        public KeyObjectService(DbTestContext context) : base(context)
        {
            _context = context;
        }

        public async Task<KeyObject> GetByAuditoryName(string auditory)
        {
            if (_context.KeyObjects.Any(x => x.AudNum == auditory))
            {
                var key = await _context.KeyObjects.FirstAsync(x => x.AudNum == auditory);
                return key;
            }
            return null;

        }

        public override async Task Add(KeyObject obj)
        {
            var key = await GetBy(obj.Id);
            await base.Add(key);
        }
        public async Task Add(string auditory)
        {
            var key = await GetByAuditoryName(auditory);
            if (key != null)
                throw new KeyAlreadyExist(key.AudNum);
            key = new KeyObject();
            key.Id = Guid.NewGuid();
            key.AudNum = auditory;
            await base.Add(key);
        }
    }
}
