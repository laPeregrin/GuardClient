using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Models
{
    public class User : DomainObject
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string  LastName { get; set; }
        public IEnumerable<Permission>? Permissions { get; set; } = null;
    }
}
