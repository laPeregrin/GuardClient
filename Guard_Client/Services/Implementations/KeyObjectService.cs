using DTOs.Models;
using DTOs.Services;
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
                var key = _context.KeyObjects.FirstOrDefault(x => x.AudNum == auditory);
                return key;
            }
            return null;

        }
    }
}
