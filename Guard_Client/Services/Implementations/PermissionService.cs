using DTOs.Models;
using Guard_Client.Exceptions;
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

        public async Task<IEnumerable<Permission>> GetAllWithUserCollection()
        {
            return await _service.Permissions.Include(x => x.UsersWithPermissions).ToListAsync();
        }
        public async Task<IEnumerable<Permission>> GetAllWithUserCollectionAndKey()
        {
            return await _service.Permissions.Include(x => x.UsersWithPermissions).Include(x => x.Key).ToListAsync();
        }

        public async override Task<Permission> GetBy(Guid id)
        {
            return await _service.Permissions.Include(x => x.UsersWithPermissions).FirstAsync(x => x.Id == id);
        }

        public async Task<Permission> GetByKey(KeyObject key)
        {
            var permission = await _service.Permissions.Where(x => x.KeyId == key.Id).Include(x => x.UsersWithPermissions).ToArrayAsync();
            if (permission.Count() > 0)
                return permission[0];

            return null;
        }
        public async override Task Add(Permission obj)
        {
            if (!_service.Permissions.Any(x => x.KeyId == obj.KeyId || x.Key.AudNum == obj.Key.AudNum))
            {
                await _service.Permissions.AddAsync(obj);
                await _service.SaveChangesAsync();
                return;
            }
            throw new KeyAlreadyExist(obj.Key.AudNum);
        }
        public bool IsHaveAcces(Permission perm, User user)
        {
            foreach (var item in perm.UsersWithPermissions)
            {
                if (item.Id == user.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
