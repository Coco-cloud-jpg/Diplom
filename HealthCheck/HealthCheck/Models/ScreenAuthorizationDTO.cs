using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Models
{
    public class ScreenAuthorizationDTO
    {
        public Guid CompanyId { get; set; }
        public Guid RecorderId { get; set; }
    }
}
