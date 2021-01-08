using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Models
{
    public class Permission : DomainObject
    {
        public Permission() { }
        public Permission(KeyObject key, IEnumerable<User> usersWithPermissions)
        {
            Id = Guid.NewGuid();
            Key = key;
            KeyId = key.Id;
            UsersWithPermissions = usersWithPermissions;
        }

        public KeyObject? Key { get; set; }
        public Guid? KeyId { get; set; }
        public IEnumerable<User> UsersWithPermissions { get; set; }


    }
}
