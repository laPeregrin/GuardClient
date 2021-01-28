using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Models
{
    public class Image : DomainObject
    {
        public string Path { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }

        public Image() { Id = Guid.NewGuid(); }
        public Image(string path)
        {
            Path = path;
            Id = Guid.NewGuid();
        }
    }
}
