using DTOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDAL;

namespace Guard_Client.Services.Implementations
{
    public class PermissionService : DataService<Permission>
    {
        private readonly DbTestContext _service;
        public PermissionService(DbTestContext service) : base(service)
        {
            _service = service;
        }

        public async Task<Permission> GetByKey(KeyObject key)
        {
          var permission = await _service.Permissions.Where(x => x.KeyId == key.Id).Include(x => x.UsersWithPermissions).ToArrayAsync();
            return permission[0];
        }
        public bool IsHaveAcces(Permission perm, User user)
        {
            foreach(var item in perm.UsersWithPermissions)
            {
                if(item.Id == user.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
