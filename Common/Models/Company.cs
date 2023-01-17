using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Company: BaseEntity
    {
        public Company()
        {
            Users = new HashSet<User>();
        }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
